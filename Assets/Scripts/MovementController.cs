using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MovementController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static float raiseHeight = -3f;
    public static Vector3 raiseVector = new Vector3(0, 0, raiseHeight);
    public static BoardAttributes boardAttributes;

    public GameObject pathDot, oldLocation, gameOverObject, cardInfo;
    public Sprite newLocation;
    public Text roundCounter;

    private GameObject InfoCard;
    private static float scale = 10;
    private GameObject board, selected;
    private BoardSetup boardSetup;
    private bool gameOver = false;
    private List<Vector2Int> validPath = new List<Vector2Int>();
    private List<GameObject> pathDots = new List<GameObject>();
    private Image previousImage;
    private Sprite previousSprite;
    private int round = 0;

    private void Start()
    {
        Lineup lineup = InfoLoader.user.lineups[InfoLoader.user.lastLineupSelected];
        board = Instantiate(Resources.Load<GameObject>("Board/Info/" + lineup.boardName + "/Board"));
        boardSetup = board.GetComponent<BoardSetup>();
        boardSetup.Setup(lineup, true);  // Set up Player Lineup
        boardSetup.Setup(new EnemyLineup(), false);  // Set up Enemy Lineup
        boardAttributes = board.GetComponent<BoardSetup>().boardAttributes;
        oldLocation = Instantiate(oldLocation);
        oldLocation.SetActive(false);
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetMouseButtonUp(0))
            {
                SceneManager.LoadScene("PlayerMatching");
                GameInfo.Clear();
            }
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj == selected) SetLocation(); // Put down
                else if (hitObj.tag == "Ally")
                {
                    if (selected != null) SetLocation(); // switch piece
                    hit.collider.gameObject.transform.position += raiseVector;
                    selected = hit.collider.gameObject;
                    validPath = ValidPath(hitObj, hitObj.transform.position);
                    if (validPath.Count == 0) return;
                    // Draw Valid path
                    foreach (Vector2Int path in validPath)
                    {
                        float posX = path.x * scale + scale / 2;
                        float posY = path.y * scale + scale / 2;
                        float posZ = -1;
                        if (FindAt(path.x, path.y) == 'E') posZ -= hitObj.transform.localScale.z;
                        GameObject copy = Instantiate(pathDot);
                        copy.transform.position = new Vector3(posX, posY, posZ);
                        if (oldLocation.transform.position == copy.transform.position) oldLocation.SetActive(false);
                        pathDots.Add(copy);
                    }
                }
                else if (selected != null)
                {
                    Vector2Int location = GetGridLocation(hit.point.x, hit.point.y);
                    foreach (Vector2Int path in validPath)
                    {
                        if (path == location)
                        {
                            KillAt(location);
                            SetLocation(hit.point.x, hit.point.y);
                            break;
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && selected != null) SetLocation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Transform pieceTransform = board.transform.Find(Vec2ToString(GetGridLocation(Input.mousePosition.x, Input.mousePosition.y)) + "/Piece");
        //if (pieceTransform!=null)
        //{
        //    GameObject piece = pieceTransform.gameObject;
        //    if (!piece.activeSelf)
        //    {
        //        InfoCard = Instantiate(cardInfo);
        //        InfoCard.GetComponent<CardInfo>().SetAttributes(piece.GetComponent<PieceInfo>().GetPieceAttributes());
        //        InfoCard.transform.position = new Vector3(Input.mousePosition.x + 10, Input.mousePosition.y + 10, -2);
        //    }
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (InfoCard != null)
        //{
        //    Destroy(InfoCard);
        //}
    }

    private void KillAt(Vector2Int loc)
    {
        Transform enemy = board.transform.Find(Vec2ToString(loc) + "/Piece");
        if(enemy != null && enemy.tag[0] == 'E')
        {
            if (enemy.GetComponent<PieceInfo>().GetPieceType() == "General")
            {
                gameOver = true;
                gameOverObject.SetActive(true);
            }
            boardSetup.DeactivateEnemy(enemy.gameObject, loc);
        }
    }

    private Vector2Int GetGridLocation(float x, float y) { return new Vector2Int((int)Mathf.Floor(x / scale), (int)Mathf.Floor(y / scale)); }

    private void SetLocation(float x = -1, float y = -1)
    {
        foreach (GameObject pathDot in pathDots) Destroy(pathDot);
        pathDots.Clear();
        selected.transform.position -= raiseVector;
        if (!oldLocation.activeSelf && previousSprite != null) oldLocation.SetActive(true);
        if (x != -1 && y != -1)
        {            
            oldLocation.transform.position = selected.transform.position;
            if (previousSprite != null)
                previousImage.sprite = previousSprite;

            Vector2Int location = GetGridLocation(x, y);
            //SetLocationData(selected.transform.position, location);
            selected.transform.parent = board.transform.Find(Vec2ToString(location));
            selected.transform.localPosition = new Vector3(0, 0, selected.transform.position.z);
            round++;
            roundCounter.text = "Round: " + round.ToString();

            previousImage = board.transform.Find(Vec2ToString(location) + "/Canvas").GetComponentInChildren<Image>();
            previousSprite = previousImage.sprite;
            previousImage.sprite = newLocation;
        }
        selected = null;
    }

    private void SetLocationData(Vector3 before, Vector2Int after)
    {
        Vector2Int previous = GetGridLocation(before.x, before.y);
        Piece piece = GameInfo.board[previous];
        GameInfo.board.Remove(previous);
        GameInfo.board.Add(after, piece);
    }

    private List<Vector2Int> ValidPath(GameObject obj, Vector3 position)
    {
        List<Vector2Int> validPath = new List<Vector2Int>();
        int x = (int)Mathf.Floor(position.x / scale);
        int y = (int)Mathf.Floor(position.y / scale);
        switch (obj.GetComponent<PieceInfo>().GetPieceType())
        {
            case "General":
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InPalace(x, y + i) && FindAt(x, y + i) != 'A')
                        validPath.Add(new Vector2Int(x, y + i));
                    if (InPalace(x + i, y) && FindAt(x + i, y) != 'A')
                        validPath.Add(new Vector2Int(x + i, y));
                }
                break;
            case "Advisor":
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InPalace(x + i, y + j) && FindAt(x + i, y + j) != 'A')
                            validPath.Add(new Vector2Int(x + i, y + j));
                }
                break;
            case "Elephant":
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InSelfField(x + i * 2, y + j * 2) && FindAt(x + i, y + j) == 'B' && FindAt(x + i * 2, y + j * 2) != 'A')
                            validPath.Add(new Vector2Int(x + i * 2, y + j * 2));
                }
                break;
            case "Horse":
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InBoard(x, y + i) && FindAt(x, y + i) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + j, y + i * 2) && FindAt(x + j, y + i * 2) != 'A')
                                validPath.Add(new Vector2Int(x + j, y + i * 2));
                    if (InPalace(x + i, y) && FindAt(x + i, y) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + i * 2, y + j) && FindAt(x + i * 2, y + j) != 'A')
                                validPath.Add(new Vector2Int(x + i * 2, y + j));
                }
                break;
            case "Chariot":
                for (int j = y + 1; j < boardAttributes.boardHeight; j++)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validPath.Add(new Vector2Int(x, j));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2Int(x, j));
                            break;
                    }
                    break;
                }
                for (int j = y - 1; j >= 0; j--)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validPath.Add(new Vector2Int(x, j));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2Int(x, j));
                            break;
                    }
                    break;
                }
                for (int i = x - 1; i >= 0; i--)
                {
                    switch (FindAt(i, y))
                    {
                        case 'B':
                            validPath.Add(new Vector2Int(i, y));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2Int(i, y));
                            break;
                    }
                    break;
                }
                for (int i = x + 1; i < boardAttributes.boardWidth; i++)
                {
                    switch (FindAt(i, y))
                    {
                        case 'B':
                            validPath.Add(new Vector2Int(i, y));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2Int(i, y));
                            break;
                    }
                    break;
                }
                break;
            case "Cannon":
                for (int j = y + 1; j < boardAttributes.boardHeight; j++)
                {
                    if (FindAt(x, j) == 'B') validPath.Add(new Vector2Int(x, j));
                    else
                    {
                        for (int jj = j + 1; jj < boardAttributes.boardHeight; jj++)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2Int(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int j = y - 1; j >= 0; j--)
                {
                    if (FindAt(x, j) == 'B') validPath.Add(new Vector2Int(x, j));
                    else
                    {
                        for (int jj = j - 1; jj >= 0; jj--)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2Int(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = x - 1; i >= 0; i--)
                {
                    if (FindAt(i, y) == 'B') validPath.Add(new Vector2Int(i, y));
                    else
                    {
                        for (int ii = i - 1; ii >= 0; ii--)
                        {
                            switch (FindAt(ii, y))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2Int(ii, y));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = x + 1; i < boardAttributes.boardWidth; i++)
                {
                    if (FindAt(i, y) == 'B') validPath.Add(new Vector2Int(i, y));
                    else
                    {
                        for (int ii = i + 1; ii < boardAttributes.boardWidth; ii++)
                        {
                            switch (FindAt(ii, y))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2Int(ii, y));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                break;
            case "Soldier":
                if (InBoard(x, y + 1) && FindAt(x, y + 1) != 'A')
                    validPath.Add(new Vector2Int(x, y + 1));
                if (!InSelfField(x, y))
                    for (int i = -1; i <= 1; i += 2)
                        if (InBoard(x + i, y) && FindAt(x + i, y) != 'A')
                            validPath.Add(new Vector2Int(x + i, y));
                break;
        }
        return validPath;
    }

    private char FindAt(float x, float y) { return FindAt((int) x, (int) y); }

    private char FindAt(int x, int y)
    {
        Transform piece = board.transform.Find(x.ToString() + y.ToString() + "/Piece");
        if (piece != null && piece.gameObject.activeSelf) return piece.tag[0];
        return 'B';
    }

    private string Vec2ToString(Vector2Int vec) { return vec.x.ToString() + vec.y.ToString(); }

    private bool InPalace(float x, float y) { return boardAttributes.palaceDownLeft.x <= x && x <= boardAttributes.palaceUpperRight.x && boardAttributes.palaceDownLeft.y <= y && y <= boardAttributes.palaceUpperRight.y; }

    private bool InSelfField(float x, float y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y <= boardAttributes.allyField; }

    private bool InBoard(float x, float y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y < boardAttributes.boardHeight; }
}