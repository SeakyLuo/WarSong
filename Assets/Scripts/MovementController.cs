using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovementController : MonoBehaviour
{
    public static float raiseHeight = 1.5f;
    public static Vector3 raiseVector = new Vector3(0f, raiseHeight, 0f);
    public BoardAttributes boardAttributes;

    public GameObject pathDot;
    public Material newLocation;
    public Image gameOverImage;
    public Text roundCounter;

    private Setup setup;
    private static float radius;
    private static float scale;
    private static float gridScale;
    private GameObject selected;
    private bool gameOver = false;
    private List<Vector2> validPath = new List<Vector2>();
    private List<GameObject> pathDots = new List<GameObject>();
    private Vector2 previousLocation;
    private Material previousMaterial;
    private GameObject locationMark;
    private int round = 0;
    private RaycastHit hit;

    private void Start()
    {
        setup = GetComponent<Setup>();
        radius = Setup.radius;
        scale = Setup.scale;
        gridScale = Setup.gridScale;
        locationMark = Instantiate(Resources.Load<GameObject>("Prefabs/LocationMark"));
        gameOverImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyUp(KeyCode.R)) restart();
            return;
        }
        // if (gameOver || !respond()) return;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj == selected) SetLocation();
                else if (hitObj.tag == "Ally")
                {
                    if (selected != null) SetLocation();
                    hit.collider.gameObject.transform.position += raiseVector;
                    selected = hit.collider.gameObject;
                    validPath = ValidPath(hitObj, hitObj.transform.position.x, hitObj.transform.position.z);
                    if (validPath.Count == 0) return;
                    // Draw Valid path
                    foreach (Vector2 path in validPath)
                    {
                        float posX = path.x * scale + gridScale * scale / 2;
                        float posY = 0f;
                        if (FindAt(path.x, path.y) == 'E') posY = hitObj.transform.localScale.y * scale;
                        float posZ = path.y * scale + gridScale * scale / 2;
                        pathDots.Add(Instantiate(pathDot, new Vector3(posX, posY, posZ), Quaternion.identity));
                    }
                }
                else if (selected != null)
                {
                    Vector2 location = new Vector2(Mathf.Floor(hit.point.x / scale), Mathf.Floor(hit.point.z / scale));
                    foreach (Vector2 path in validPath)
                    {
                        if (path == location)
                        {
                            killAt(location);
                            SetLocation(hit.point.x, hit.point.z);
                            break;
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(1) && selected != null) SetLocation();
    }

    private void killAt(Vector2 loc)
    {
        foreach (GameObject obj in setup.GetActive())
        {
            if (obj.GetComponent<PieceInfo>().GetLocation() == loc)
            {
                if (obj.GetComponent<PieceInfo>().GetCardType() == "General")
                {
                    gameOver = true;
                    gameOverImage.gameObject.SetActive(true);
                }
                setup.deactivate(obj);
                break;
            }
        }
    }

    private Vector2 GetGridLocation(float x, float z) { return new Vector2(Mathf.Floor(x / scale), Mathf.Floor(z / scale)); }

    private void SetLocation(float x = 0, float z = 0)
    {
        foreach (GameObject pathDot in pathDots) Destroy(pathDot);
        pathDots.Clear();
        selected.transform.position -= raiseVector;
        if (x != 0 && z != 0)
        {
            locationMark.transform.position = new Vector3(selected.GetComponent<PieceInfo>().GetLocation().x + gridScale / 2, 0f, selected.GetComponent<PieceInfo>().GetLocation().y + gridScale / 2) * scale;
            if (previousMaterial != null) GameObject.Find(previousLocation.x.ToString() + previousLocation.y.ToString()).GetComponent<Renderer>().material = previousMaterial;

            Vector2 location = GetGridLocation(x, z);
            selected.GetComponent<PieceInfo>().SetLocation(location);
            selected.transform.position = new Vector3(location.x * scale + radius, selected.transform.position.y, location.y * scale + radius);
            round++;
            roundCounter.text = "Round: " + round.ToString();

            previousLocation = location;
            previousMaterial = GameObject.Find(location.x.ToString() + location.y.ToString()).GetComponent<Renderer>().material;
            GameObject.Find(location.x.ToString() + location.y.ToString()).GetComponent<Renderer>().material = newLocation;
        }
        selected = null;
    }

    private List<Vector2> ValidPath(GameObject obj, float x, float z)
    {
        List<Vector2> validPath = new List<Vector2>();
        x = Mathf.Floor(x / scale);
        z = Mathf.Floor(z / scale);
        switch (obj.GetComponent<PieceInfo>().GetCardType()[0])
        {
            case 'G':
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InPalace(x, z + i) && FindAt(x, z + i) != 'A')
                        validPath.Add(new Vector2(x, z + i));
                    if (InPalace(x + i, z) && FindAt(x + i, z) != 'A')
                        validPath.Add(new Vector2(x + i, z));
                }
                break;
            case 'A':
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InPalace(x + i, z + j) && FindAt(x + i, z + j) != 'A')
                            validPath.Add(new Vector2(x + i, z + j));
                }
                break;
            case 'E':
                for (int i = -1; i <= 1; i += 2)
                {
                    for (int j = -1; j <= 1; j += 2)
                        if (InSelfField(x + i * 2, z + j * 2) && FindAt(x + i, z + j) == 'B' && FindAt(x + i * 2, z + j * 2) != 'A')
                            validPath.Add(new Vector2(x + i * 2, z + j * 2));
                }
                break;
            case 'H':
                for (int i = -1; i <= 1; i += 2)
                {
                    if (InBoard(x, z + i) && FindAt(x, z + i) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + j, z + i * 2) && FindAt(x + j, z + i * 2) != 'A')
                                validPath.Add(new Vector2(x + j, z + i * 2));
                    if (InPalace(x + i, z) && FindAt(x + i, z) == 'B')
                        for (int j = -1; j <= 1; j += 2)
                            if (InBoard(x + i * 2, z + j) && FindAt(x + i * 2, z + j) != 'A')
                                validPath.Add(new Vector2(x + i * 2, z + j));
                }
                break;
            case 'R':
                for (int j = (int)z + 1; j < boardAttributes.boardHeight; j++)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validPath.Add(new Vector2(x, j));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2(x, j));
                            break;
                    }
                    break;
                }
                for (int j = (int)z - 1; j >= 0; j--)
                {
                    switch (FindAt(x, j))
                    {
                        case 'B':
                            validPath.Add(new Vector2(x, j));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2(x, j));
                            break;
                    }
                    break;
                }
                for (int i = (int)x - 1; i >= 0; i--)
                {
                    switch (FindAt(i, z))
                    {
                        case 'B':
                            validPath.Add(new Vector2(i, z));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2(i, z));
                            break;
                    }
                    break;
                }
                for (int i = (int)x + 1; i < boardAttributes.boardWidth; i++)
                {
                    switch (FindAt(i, z))
                    {
                        case 'B':
                            validPath.Add(new Vector2(i, z));
                            continue;
                        case 'E':
                            validPath.Add(new Vector2(i, z));
                            break;
                    }
                    break;
                }
                break;
            case 'C':
                for (int j = (int)z + 1; j < boardAttributes.boardHeight; j++)
                {
                    if (FindAt(x, j) == 'B') validPath.Add(new Vector2(x, j));
                    else
                    {
                        for (int jj = j + 1; jj < boardAttributes.boardHeight; jj++)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int j = (int)z - 1; j >= 0; j--)
                {
                    if (FindAt(x, j) == 'B') validPath.Add(new Vector2(x, j));
                    else
                    {
                        for (int jj = j - 1; jj > 0; jj--)
                        {
                            switch (FindAt(x, jj))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2(x, jj));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = (int)x - 1; i >= 0; i--)
                {
                    if (FindAt(i, z) == 'B') validPath.Add(new Vector2(i, z));
                    else
                    {
                        for (int ii = i - 1; ii >= 0; ii--)
                        {
                            switch (FindAt(ii, z))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2(ii, z));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                for (int i = (int)x + 1; i < boardAttributes.boardHeight; i++)
                {
                    if (FindAt(i, z) == 'B') validPath.Add(new Vector2(i, z));
                    else
                    {
                        for (int ii = i + 1; ii < boardAttributes.boardHeight; ii++)
                        {
                            switch (FindAt(ii, z))
                            {
                                case 'B':
                                    continue;
                                case 'E':
                                    validPath.Add(new Vector2(ii, z));
                                    break;
                            }
                            break;
                        }
                        break;
                    }
                }
                break;
            case 'S':  //↑←→                
                if (InBoard(x, z + 1) && FindAt(x, z + 1) != 'A')
                    validPath.Add(new Vector2(x, z + 1));
                if (!InSelfField(x, z))
                    for (int i = -1; i <= 1; i += 2)
                        if (InBoard(x + i, z) && FindAt(x + i, z) != 'A')
                            validPath.Add(new Vector2(x + i, z));
                break;
        }
        return validPath;
    }

    private char FindAt(float x, float z)
    {
        Vector2 loc = new Vector2(x, z);
        foreach (GameObject obj in setup.GetActive())
        {
            if (obj.GetComponent<PieceInfo>().GetLocation() == loc)
                return obj.tag[0];
        }
        return 'B';
    }

    private bool InPalace(float x, float z) { return boardAttributes.palaceDownLeft.x <= x && x <= boardAttributes.palaceUpperRight.x && boardAttributes.palaceDownLeft.y <= z && z <= boardAttributes.palaceUpperRight.y; }

    private bool InSelfField(float x, float z) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= z && z <= boardAttributes.allyField; }

    private bool InBoard(float x, float z) { return 0 <= x && x < boardAttributes.boardWidth && 0 <= z && z < boardAttributes.boardHeight; }

    public void restart() { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
}