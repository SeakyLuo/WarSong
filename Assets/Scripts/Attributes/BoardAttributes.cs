﻿using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "Board")]
public class BoardAttributes : ScriptableObject {

    public string boardName;
    public bool available = true;
    public Sprite allyFieldImage, completeImage;
    public int boardWidth = 9;
    public int boardHeight = 10;
    public int allyField = 4;
    public Vector2Int palaceDownLeft = new Vector2Int(3, 0);
    public Vector2Int palaceUpperRight = new Vector2Int(5, 2);

    public Vector2Int asloc1 = new Vector2Int(0, 3);
    public Vector2Int asloc2 = new Vector2Int(2, 3);
    public Vector2Int asloc3 = new Vector2Int(4, 3);
    public Vector2Int asloc4 = new Vector2Int(6, 3);
    public Vector2Int asloc5 = new Vector2Int(8, 3);
    public Vector2Int acloc1 = new Vector2Int(1, 2);
    public Vector2Int acloc2 = new Vector2Int(7, 2);
    public Vector2Int arloc1 = new Vector2Int(0, 0);
    public Vector2Int arloc2 = new Vector2Int(8, 0);
    public Vector2Int ahloc1 = new Vector2Int(1, 0);
    public Vector2Int ahloc2 = new Vector2Int(7, 0);
    public Vector2Int aeloc1 = new Vector2Int(2, 0);
    public Vector2Int aeloc2 = new Vector2Int(6, 0);
    public Vector2Int aaloc1 = new Vector2Int(3, 0);
    public Vector2Int aaloc2 = new Vector2Int(5, 0);
    public Vector2Int agloc = new Vector2Int(4, 0);

    public Vector2Int esloc1 = new Vector2Int(0, 6);
    public Vector2Int esloc2 = new Vector2Int(2, 6);
    public Vector2Int esloc3 = new Vector2Int(4, 6);
    public Vector2Int esloc4 = new Vector2Int(6, 6);
    public Vector2Int esloc5 = new Vector2Int(8, 6);
    public Vector2Int ecloc1 = new Vector2Int(1, 7);
    public Vector2Int ecloc2 = new Vector2Int(7, 7);
    public Vector2Int erloc1 = new Vector2Int(0, 9);
    public Vector2Int erloc2 = new Vector2Int(8, 9);
    public Vector2Int ehloc1 = new Vector2Int(1, 9);
    public Vector2Int ehloc2 = new Vector2Int(7, 9);
    public Vector2Int eeloc1 = new Vector2Int(2, 9);
    public Vector2Int eeloc2 = new Vector2Int(6, 9);
    public Vector2Int ealoc1 = new Vector2Int(3, 9);
    public Vector2Int ealoc2 = new Vector2Int(5, 9);
    public Vector2Int egloc = new Vector2Int(4, 9);

}
