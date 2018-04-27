using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    public static float raiseHeight = -3f;
    public static Vector3 raiseVector = new Vector3(0, 0, raiseHeight);
    public static BoardAttributes boardAttributes;

    public GameObject pathDot, cardInfo;
    public Transform boardCanvas;
    public Sprite newLocation;

    private static Collider selected;
    private static Button activateAbilityButton;
    private static GameObject activateAbilityText;
    private static GameObject oldLocation;
    private static List<GameObject> pathDots = new List<GameObject>();
    private static Image previousImage;
    private static Sprite previousSprite;
    private static List<Vector2Int> validLoc = new List<Vector2Int>();

    private OnEnterGame onEnterGame;
    private float scale;
    private BoardSetup boardSetup;


    private void Start()
    {
        GameObject UIPanel = GameObject.Find("UIPanel");
        onEnterGame = UIPanel.GetComponent<OnEnterGame>();
        Transform activateAbility = UIPanel.transform.Find("Canvas/ActivateAbility");
        activateAbilityButton = activateAbility.GetComponent<Button>();
        activateAbilityText = activateAbility.Find("Text").gameObject;
        scale = transform.localScale.x;
        boardSetup = GetComponent<BoardSetup>();
        boardAttributes = boardSetup.boardAttributes;
        oldLocation = Instantiate(Resources.Load<GameObject>("GameMode/OldLocation"));
    }

    private void Update()
    {
        if (OnEnterGame.gameover) return;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Collider hitObj = hit.collider;
                if (hitObj == selected) SetLocation(Piece.noLocation); // Put down
                else if (hitObj.tag == "Ally")
                {
                    if (selected != null) SetLocation(Piece.noLocation); // switch piece
                    hit.collider.transform.position += raiseVector;
                    selected = hit.collider;
                    if (!selected.GetComponent<PieceInfo>().IsStandard()) ActivateActivateAbility();
                    validLoc = ValidLoc(hitObj);
                    if (validLoc.Count == 0) return;
                    // Draw Valid path
                    foreach (Vector2Int path in validLoc)
                    {
                        float posZ = -1;
                        if (FindAt(path) == 'E') posZ -= hitObj.transform.localScale.z;
                        GameObject copy = Instantiate(pathDot);
                        copy.name = Vec2ToString(path);
                        copy.transform.position = new Vector3(path.x * scale, path.y * scale, posZ);
                        if (oldLocation.transform.position == copy.transform.position) oldLocation.SetActive(false);
                        pathDots.Add(copy);
                    }
                }
                else if (selected != null)
                {
                    if (GameInfo.pieceMoved)
                    {
                        onEnterGame.ShowMoved();
                        // show moved
                        return;
                    }
                    Vector2Int location;
                    if (hit.collider.name == "Piece") location = StringToVec2(hit.collider.transform.parent.name);
                    else location = StringToVec2(hit.collider.name);
                    foreach (Vector2Int path in validLoc)
                    {
                        if (path == location)
                        {
                            KillAt(location);                            
                            SetLocation(location);
                            break;
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && selected != null) SetLocation(Piece.noLocation);
    }

    private Vector2Int StringToVec2(string loc) { return new Vector2Int((int)Char.GetNumericValue(loc[0]), (int)Char.GetNumericValue(loc[1])); }

    private void KillAt(Vector2Int loc)
    {
        Piece enemy;
        if (GameInfo.board.TryGetValue(loc, out enemy) && !enemy.IsAlly())
        {
            if(enemy.GetPieceType() == "General")
                onEnterGame.Victory();
            boardSetup.Deactivate(enemy);
        }
    }

    private Vector2Int GetGridLocation(float x, float y) { return new Vector2Int((int)Mathf.Floor(x / scale), (int)Mathf.Floor(y / scale)); }

    public static void PutDownPiece()
    {
        if (selected == null) return;
        foreach (GameObject pathDot in pathDots) Destroy(pathDot);
        pathDots.Clear();
        selected.transform.position -= raiseVector;
        if (!oldLocation.activeSelf && previousSprite != null) oldLocation.SetActive(true);
        DeactivateActivateAbility();
        selected = null;
    }

    private void SetLocation(Vector2Int loc)
    {
        foreach (GameObject pathDot in pathDots) Destroy(pathDot);
        pathDots.Clear();
        selected.transform.position -= raiseVector;
        if (loc != Piece.noLocation)
        {
            oldLocation.transform.position = selected.transform.position;
            if (previousSprite != null)
                previousImage.sprite = previousSprite;

            SetLocationData(selected.transform.position, loc);
            Transform location = boardCanvas.Find(Vec2ToString(loc));
            selected.transform.parent = location;
            selected.transform.localPosition = new Vector3(0, 0, selected.transform.position.z);
            //onEnterGame.NextTurn();

            previousImage = location.Find("Image").GetComponent<Image>();
            previousSprite = previousImage.sprite;
            previousImage.sprite = newLocation;

            GameInfo.pieceMoved = true;
        }
        if (!oldLocation.activeSelf && previousSprite != null)
            oldLocation.SetActive(true);
        DeactivateActivateAbility();
        selected = null;
    }

    private void SetLocationData(Vector3 before, Vector2Int after)
    {
        Vector2Int previous = GetGridLocation(before.x, before.y);
        Piece piece = GameInfo.board[previous];
        GameInfo.board.Remove(previous);
        GameInfo.board.Add(after, piece);
    }

    private List<Vector2Int> ValidLoc(Collider obj)
    {
        int x = (int)Mathf.Floor(obj.transform.position.x / scale);
        int y = (int)Mathf.Floor(obj.transform.position.y / scale);
        List<Vector2Int> validLoc = new List<Vector2Int>();
        switch (obj.GetComponent<PieceInfo>().GetPieceType()) //GameInfo.board[new Vector2Int(x, y)].GetPieceType()
        {
            case "General":
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InPalace(x, y + i) && FindAt(x, y + i) != 'A')
                        validLoc.Add(new Vector2Int(x, y + i));
                    if (InPalace(x + i, y) && FindAt(x + i, y) != 'A')
                        validLoc.Add(new Vector2Int(x + i, y));
                }
                break;
            case "Advisor":
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InPalace(x + i, y + j) && FindAt(x + i, y + j) != 'A')
                            validLoc.Add(new Vector2Int(x + i, y + j));
                }
                break;
            case "Elephant":
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InAllyField(x + i * 2, y + j * 2) && FindAt(x + i, y + j) == 'B' && FindAt(x + i * 2, y + j * 2) != 'A')
                            validLoc.Add(new Vector2Int(x + i * 2, y + j * 2));
                }
                break;
            case "Horse":
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InBoard(x, y + i) && FindAt(x, y + i) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + j, y + i * 2) && FindAt(x + j, y + i * 2) != 'A')
                                validLoc.Add(new Vector2Int(x + j, y + i * 2));
                    if (InBoard(x + i, y) && FindAt(x + i, y) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + i * 2, y + j) && FindAt(x + i * 2, y + j) != 'A')
                                validLoc.Add(new Vector2Int(x + i * 2, y + j));
                }
                break;
            case "Chariot":
                for (int j = y + 1; j < boardAttributes.boardHeight; j++)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validLoc.Add(new Vector2Int(x, j));
                            continue;
                        case 'E':
                            validLoc.Add(new Vector2Int(x, j));
                            break;
                    }
                    break;
                }
                for (int j = y - 1; j >= 0; j--)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validLoc.Add(new Vector2Int(x, j));
                            continue;
                        case 'E':
                            validLoc.Add(new Vector2Int(x, j));
                            break;
                    }
                    break;
                }
                for (int i = x - 1; i >= 0; i--)
                {
                    switch (FindAt(i, y))
                    {
                        case 'B':
                            validLoc.Add(new Vector2Int(i, y));
                            continue;
                        case 'E':
                            validLoc.Add(new Vector2Int(i, y));
                            break;
                    }
                    break;
                }
                for (int i = x + 1; i < boardAttributes.boardWidth; i++)
                {
                    switch (FindAt(i, y))
                    {
                        case 'B':
                            validLoc.Add(new Vector2Int(i, y));
                            continue;
                        case 'E':
                            validLoc.Add(new Vector2Int(i, y));
                            break;
                    }
                    break;
                }
                break;
            case "Cannon":
                for (int j = y + 1; j < boardAttributes.boardHeight; j++)
                {
                    if (FindAt(x, j) == 'B') validLoc.Add(new Vector2Int(x, j));
                    else
                    {
                        for (int jj = j + 1; jj < boardAttributes.boardHeight; jj++)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validLoc.Add(new Vector2Int(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int j = y - 1; j >= 0; j--)
                {
                    if (FindAt(x, j) == 'B') validLoc.Add(new Vector2Int(x, j));
                    else
                    {
                        for (int jj = j - 1; jj >= 0; jj--)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validLoc.Add(new Vector2Int(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = x - 1; i >= 0; i--)
                {
                    if (FindAt(i, y) == 'B') validLoc.Add(new Vector2Int(i, y));
                    else
                    {
                        for (int ii = i - 1; ii >= 0; ii--)
                        {
                            switch (FindAt(ii, y))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validLoc.Add(new Vector2Int(ii, y));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = x + 1; i < boardAttributes.boardWidth; i++)
                {
                    if (FindAt(i, y) == 'B') validLoc.Add(new Vector2Int(i, y));
                    else
                    {
                        for (int ii = i + 1; ii < boardAttributes.boardWidth; ii++)
                        {
                            switch (FindAt(ii, y))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validLoc.Add(new Vector2Int(ii, y));
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
                    validLoc.Add(new Vector2Int(x, y + 1));
                if (!InAllyField(x, y))
                    for (int i = -1; i <= 1; i += 2)
                        if (InBoard(x + i, y) && FindAt(x + i, y) != 'A')
                            validLoc.Add(new Vector2Int(x + i, y));
                break;
        }
        return validLoc;
    }

    private char FindAt(float x, float y) { return FindAt(new Vector2Int((int)x, (int)y)); }
    private char FindAt(int x, int y) { return FindAt(new Vector2Int(x, y)); }

    private char FindAt(Vector2Int loc)
    {
        Piece piece;
        if(GameInfo.board.TryGetValue(loc, out piece))
        {
            if (piece.IsAlly()) return 'A';
            else return 'E';
        }
        return 'B';
    }

    private static void ActivateActivateAbility()
    {
        activateAbilityButton.interactable = true;
        activateAbilityText.SetActive(true);
    }

    private static void DeactivateActivateAbility()
    {
        activateAbilityButton.interactable = false;
        activateAbilityText.SetActive(false);
    }

    private string Vec2ToString(Vector2Int vec) { return vec.x.ToString() + vec.y.ToString(); }

    private bool InPalace(int x, int y) { return boardAttributes.palaceDownLeft.x <= x && x <= boardAttributes.palaceUpperRight.x && boardAttributes.palaceDownLeft.y <= y && y <= boardAttributes.palaceUpperRight.y; }

    private bool InAllyField(int x, int y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y <= boardAttributes.allyField; }

    private bool InBoard(int x, int y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y < boardAttributes.boardHeight; }
}