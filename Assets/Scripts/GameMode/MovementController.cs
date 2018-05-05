using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    public static float raiseHeight = -3f;
    public static Vector3 raiseVector = new Vector3(0, 0, raiseHeight);
    public static BoardAttributes boardAttributes;
    public static List<Vector2Int> validLoc = new List<Vector2Int>();
    public static Collider selected;
    public static float scale;

    public Transform boardCanvas;
    public Sprite newLocation;
    
    private static GameObject oldLocation;
    private static GameObject pathDot;
    private static List<GameObject> pathDots = new List<GameObject>();
    private static Image previousImage;
    private static Sprite previousSprite;
    private static BoardSetup boardSetup;

    private OnEnterGame onEnterGame;

    private void Start()
    {
        GameObject UIPanel = GameObject.Find("UIPanel");
        onEnterGame = UIPanel.GetComponent<OnEnterGame>();
        scale = transform.localScale.x;
        boardSetup = GetComponent<BoardSetup>();
        boardAttributes = boardSetup.boardAttributes;
        oldLocation = Instantiate(onEnterGame.oldLocation);
        oldLocation.transform.position = new Vector3(0, 0, 100);
        pathDot = onEnterGame.pathDot;
    }

    private void Update()
    {
        if (OnEnterGame.gameover || GameInfo.actionTaken || ActivateAbility.activated) return;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Collider hitObj = hit.collider;
                if (hitObj == selected) PutDownPiece(); // Put down
                else if (hitObj.tag == "Ally")
                {
                    if (selected != null) PutDownPiece(); // switch piece
                    hit.collider.transform.position += raiseVector;
                    selected = hit.collider;
                    PieceInfo pieceInfo = selected.GetComponent<PieceInfo>();
                    pieceInfo.HideInfoCard();
                    ActivateAbility.ActivateButton();
                    validLoc = pieceInfo.ValidLoc();
                    if (validLoc.Count == 0) return;
                    // Draw Valid path
                    foreach (Vector2Int path in validLoc)
                    {
                        float posZ = -1;
                        if (FindAt(path) == 'E') posZ -= hitObj.transform.localScale.z;
                        GameObject copy = Instantiate(pathDot);
                        copy.name = InfoLoader.Vec2ToString(path);
                        copy.transform.position = new Vector3(path.x * scale, path.y * scale, posZ);
                        if (oldLocation.transform.position == copy.transform.position) oldLocation.SetActive(false);
                        pathDots.Add(copy);
                    }
                }
                else if (selected != null)
                {
                    Vector2Int location;
                    if (hitObj.name == "Piece") location = InfoLoader.StringToVec2(hitObj.transform.parent.name);
                    else location = InfoLoader.StringToVec2(hitObj.name);
                    if (validLoc.Contains(location))
                    {
                        KillAt(location);
                        SetLocation(location);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && selected != null) PutDownPiece();
    }

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
        HidePathDots();
        selected.transform.position -= raiseVector;
        if (!oldLocation.activeSelf && previousSprite != null) oldLocation.SetActive(true);
        ActivateAbility.DeactivateButton();
        selected = null;
    }

    public static void HidePathDots()
    {
        foreach (GameObject pathDot in pathDots) Destroy(pathDot);
        pathDots.Clear();
    }

    private void SetLocation(Vector2Int loc)
    {
        oldLocation.transform.position = selected.transform.position - raiseVector;
        if (previousSprite != null)
            previousImage.sprite = previousSprite;

        GameInfo.Move(GetGridLocation(selected.transform.position.x, selected.transform.position.y), loc); // SetLocationData
        Transform location = boardCanvas.Find(InfoLoader.Vec2ToString(loc));
        selected.transform.parent = location;
        selected.transform.localPosition = new Vector3(0, 0, selected.transform.position.z);

        previousImage = location.Find("Image").GetComponent<Image>();
        previousSprite = previousImage.sprite;
        previousImage.sprite = newLocation;

        PutDownPiece();
        onEnterGame.NextTurn();
    }

    private List<Vector2Int> ValidLoc(Collider obj)
    {
        int x = (int)Mathf.Floor(obj.transform.position.x / scale);
        int y = (int)Mathf.Floor(obj.transform.position.y / scale);
        return ValidLoc(x, y, obj.GetComponent<PieceInfo>().GetPieceType());  //GameInfo.board[new Vector2Int(x, y)].GetPieceType()
    }
    public static List<Vector2Int> ValidLoc(int x, int y, string type, bool link = false)
    {
        switch (type)
        {
            case "General":
                return GeneralLoc(x, y);
            case "Advisor":
                return AdvisorLoc(x, y, link);
            case "Elephant":
                return ElephantLoc(x, y, link);
            case "Horse":
                return HorseLoc(x, y, link);
            case "Chariot":
                return ChariotLoc(x, y, link);
            case "Cannon":
                if (link) return CannonTarget(x, y, true);
                return CannonLoc(x, y);
            case "Soldier":
                return SoldierLoc(x, y, link);
        }
        return new List<Vector2Int>();
    }
    public static List<Vector2Int> GeneralLoc(int x, int y)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            if (InPalace(x, y + i) && FindAt(x, y + i) != 'A')
                validLoc.Add(new Vector2Int(x, y + i));
            if (InPalace(x + i, y) && FindAt(x + i, y) != 'A')
                validLoc.Add(new Vector2Int(x + i, y));
        }
        return validLoc;
    }
    public static List<Vector2Int> AdvisorLoc(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
                if (InPalace(x + i, y + j) && (FindAt(x + i, y + j) != 'A' ^ link))
                    validLoc.Add(new Vector2Int(x + i, y + j));
        }
        return validLoc;
    }
    public static List<Vector2Int> ElephantLoc(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = -1; j <= 1; j += 2)
                if (InAllyField(x + i * 2, y + j * 2) && FindAt(x + i, y + j) == 'B' && (FindAt(x + i * 2, y + j * 2) != 'A' ^ link))
                    validLoc.Add(new Vector2Int(x + i * 2, y + j * 2));
        }
        return validLoc;
    }
    public static List<Vector2Int> HorseLoc(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int i = -1; i <= 1; i += 2)
        {
            if (InBoard(x, y + i) && FindAt(x, y + i) == 'B')
                for (int j = -1; j <= 1; j += 2)
                    if (InBoard(x + j, y + i * 2) && (FindAt(x + j, y + i * 2) != 'A') ^ link)
                        validLoc.Add(new Vector2Int(x + j, y + i * 2));
            if (InBoard(x + i, y) && FindAt(x + i, y) == 'B')
                for (int j = -1; j <= 1; j += 2)
                    if (InBoard(x + i * 2, y + j) && (FindAt(x + i * 2, y + j) != 'A') ^ link)
                        validLoc.Add(new Vector2Int(x + i * 2, y + j));
        }
        return validLoc;
    }
    public static List<Vector2Int> ChariotLoc(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int j = y + 1; j < boardAttributes.boardHeight; j++)
        {
            switch (FindAt(x, j))
            {
                case 'A':
                    if (link) validLoc.Add(new Vector2Int(x, j));
                    break;
                case 'B':
                    if (!link) validLoc.Add(new Vector2Int(x, j));
                    continue;
                case 'E':
                    if (!link) validLoc.Add(new Vector2Int(x, j));
                    break;
            }
            break;
        }
        for (int j = y - 1; j >= 0; j--)
        {
            switch (FindAt(x, j))
            {
                case 'A':
                    if (link) validLoc.Add(new Vector2Int(x, j));
                    break;
                case 'B':
                    if (!link) validLoc.Add(new Vector2Int(x, j));
                    continue;
                case 'E':
                    if (!link) validLoc.Add(new Vector2Int(x, j));
                    break;
            }
            break;
        }
        for (int i = x - 1; i >= 0; i--)
        {
            switch (FindAt(i, y))
            {
                case 'A':
                    if (link) validLoc.Add(new Vector2Int(i, y));
                    break;
                case 'B':
                    if (!link) validLoc.Add(new Vector2Int(i, y));
                    continue;
                case 'E':
                    if (!link) validLoc.Add(new Vector2Int(i, y));
                    break;
            }
            break;
        }
        for (int i = x + 1; i < boardAttributes.boardWidth; i++)
        {
            switch (FindAt(i, y))
            {
                case 'A':
                    if (link) validLoc.Add(new Vector2Int(i, y));
                    break;
                case 'B':
                    if (!link) validLoc.Add(new Vector2Int(i, y));
                    continue;
                case 'E':
                    if (!link) validLoc.Add(new Vector2Int(i, y));
                    break;
            }
            break;
        }
        return validLoc;
    }
    public static List<Vector2Int> CannonLoc(int x, int y)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
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
        return validLoc;
    }
    public static List<Vector2Int> CannonScope(int x, int y)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int j = y + 1; j < boardAttributes.boardHeight; j++)
        {
            if (FindAt(x, j) != 'B')
            {
                validLoc.Add(new Vector2Int(x, j));
                break;
            }
        }
        for (int j = y - 1; j >= 0; j--)
        {
            if (FindAt(x, j) == 'B'){
                validLoc.Add(new Vector2Int(x, j));
                break;
            }
        }
        for (int i = x - 1; i >= 0; i--)
        {
            if (FindAt(i, y) == 'B'){
                validLoc.Add(new Vector2Int(i, y));
                break;
            }
        }
        for (int i = x + 1; i < boardAttributes.boardWidth; i++)
        {
            if (FindAt(i, y) == 'B')
            {
                validLoc.Add(new Vector2Int(i, y));
                break;
            }
        }
        return validLoc;
    }
    public static List<Vector2Int> CannonTarget(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        for (int j = y + 1; j < boardAttributes.boardHeight; j++)
        {
            if (FindAt(x, j) != 'B')
            {
                for (int jj = j + 1; jj < boardAttributes.boardHeight; jj++)
                {
                    switch (FindAt(x, jj))
                    {
                        case 'A':
                            if (link) validLoc.Add(new Vector2Int(x, jj));
                            break;
                        case 'B':
                            continue;
                        case 'E':
                            if (!link) validLoc.Add(new Vector2Int(x, jj));
                            break;
                    }
                    break;
                }
                break;
            }
        }
        for (int j = y - 1; j >= 0; j--)
        {
            if (FindAt(x, j) != 'B')
            {
                for (int jj = j - 1; jj >= 0; jj--)
                {
                    switch (FindAt(x, jj))
                    {
                        case 'A':
                            if (link) validLoc.Add(new Vector2Int(x, jj));
                            break;
                        case 'B':
                            continue;
                        case 'E':
                            if (!link) validLoc.Add(new Vector2Int(x, jj));
                            break;
                    }
                    break;
                }
                break;
            }
        }
        for (int i = x - 1; i >= 0; i--)
        {
            if (FindAt(i, y) != 'B')
            {
                for (int ii = i - 1; ii >= 0; ii--)
                {
                    switch (FindAt(ii, y))
                    {
                        case 'A':
                            if (link) validLoc.Add(new Vector2Int(ii, y));
                            break;
                        case 'B':
                            continue;
                        case 'E':
                            if (!link) validLoc.Add(new Vector2Int(ii, y));
                            break;
                    }
                    break;
                }
                break;
            }
        }
        for (int i = x + 1; i < boardAttributes.boardWidth; i++)
        {
            if (FindAt(i, y) != 'B')
            {
                for (int ii = i + 1; ii < boardAttributes.boardWidth; ii++)
                {
                    switch (FindAt(ii, y))
                    {
                        case 'A':
                            if (link) validLoc.Add(new Vector2Int(ii, y));
                            break;
                        case 'B':
                            continue;
                        case 'E':
                            if (!link) validLoc.Add(new Vector2Int(ii, y));
                            break;
                    }
                    break;
                }
                break;
            }
        }
        return validLoc;
    }
    public static List<Vector2Int> SoldierLoc(int x, int y, bool link = false)
    {
        List<Vector2Int> validLoc = new List<Vector2Int>();
        if (InBoard(x, y + 1) && (FindAt(x, y + 1) != 'A' ^ link))
            validLoc.Add(new Vector2Int(x, y + 1));
        if (!InAllyField(x, y))
            for (int i = -1; i <= 1; i += 2)
                if (InBoard(x + i, y) && (FindAt(x + i, y) != 'A' ^ link))
                    validLoc.Add(new Vector2Int(x + i, y));
        return validLoc;
    }
    public static bool IsLink(Piece piece, List<Vector2Int> locations)
    {
        /// For non-Cannon Pieces
        string type = piece.GetPieceType();
        Vector2Int location = piece.GetLocation();
        foreach (Piece ally in GameInfo.activeAllies)
            if (ally.GetPieceType() == type && locations.Contains(ally.GetLocation()) && boardSetup.pieces[ally.GetLocation()].GetComponent<PieceInfo>().trigger.ValidLoc(true).Contains(location))
                return true;
        return false;
    }

    private static char FindAt(float x, float y) { return FindAt(new Vector2Int((int)x, (int)y)); }
    public static char FindAt(int x, int y) { return FindAt(new Vector2Int(x, y)); }
    private static char FindAt(Vector2Int loc)
    {
        Piece piece;
        if(GameInfo.board.TryGetValue(loc, out piece))
        {
            if (piece.IsAlly()) return 'A';
            else return 'E';
        }
        return 'B';
    }

    public static bool InPalace(int x, int y) { return boardAttributes.palaceDownLeft.x <= x && x <= boardAttributes.palaceUpperRight.x && boardAttributes.palaceDownLeft.y <= y && y <= boardAttributes.palaceUpperRight.y; }

    public static bool InAllyField(int x, int y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y <= boardAttributes.allyField; }

    public static bool InBoard(int x, int y) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= y && y < boardAttributes.boardHeight; }

    public static Trigger FindPieceTrigger(string piecename) { return Resources.Load<Trigger>("Pieces/Info/" + piecename + "/Trigger"); }
}