using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Attributes", menuName = "Board")]
public class BoardAttributes : ScriptableObject {

    public string boardName;
    public bool available = true;
    public Sprite selfFieldImage, completeImage;
    public float boardWidth = 9f;
    public float boardheight = 10f;
    public float SelfField = 4f;
    public Vector2 palaceDownLeft = new Vector2(3, 0);
    public Vector2 palaceUpperRight = new Vector2(5, 2);

    public Vector2 asloc1 = new Vector2(0, 3);
    public Vector2 asloc2 = new Vector2(2, 3);
    public Vector2 asloc3 = new Vector2(4, 3);
    public Vector2 asloc4 = new Vector2(6, 3);
    public Vector2 asloc5 = new Vector2(8, 3);
    public Vector2 acloc1 = new Vector2(1, 2);
    public Vector2 acloc2 = new Vector2(7, 2);
    public Vector2 arloc1 = new Vector2(0, 0);
    public Vector2 arloc2 = new Vector2(8, 0);
    public Vector2 ahloc1 = new Vector2(1, 0);
    public Vector2 ahloc2 = new Vector2(7, 0);
    public Vector2 aeloc1 = new Vector2(2, 0);
    public Vector2 aeloc2 = new Vector2(6, 0);
    public Vector2 aaloc1 = new Vector2(3, 0);
    public Vector2 aaloc2 = new Vector2(5, 0);
    public Vector2 agloc = new Vector2(4, 0);

    public Vector2 esloc1 = new Vector2(0, 6);
    public Vector2 esloc2 = new Vector2(2, 6);
    public Vector2 esloc3 = new Vector2(4, 6);
    public Vector2 esloc4 = new Vector2(6, 6);
    public Vector2 esloc5 = new Vector2(8, 6);
    public Vector2 ecloc1 = new Vector2(1, 7);
    public Vector2 ecloc2 = new Vector2(7, 7);
    public Vector2 erloc1 = new Vector2(0, 9);
    public Vector2 erloc2 = new Vector2(8, 9);
    public Vector2 ehloc1 = new Vector2(1, 9);
    public Vector2 ehloc2 = new Vector2(7, 9);
    public Vector2 eeloc1 = new Vector2(2, 9);
    public Vector2 eeloc2 = new Vector2(6, 9);
    public Vector2 ealoc1 = new Vector2(3, 9);
    public Vector2 ealoc2 = new Vector2(5, 9);
    public Vector2 egloc = new Vector2(4, 9);

}
