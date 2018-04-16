using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

    public static float radius;
    public static float scale;
    public static float gridScale;
    public BoardAttributes boardAttributes;
    public GameObject as1, as2, as3, as4, as5, ac1, ac2, ar1, ah1, ae1, aa1, ag, aa2, ae2, ah2, ar2,
                      es1, es2, es3, es4, es5, ec1, ec2, er1, eh1, ee1, ea1, eg, ea2, ee2, eh2, er2; 

    private List<GameObject> active = new List<GameObject>();
    private List<GameObject> inactive = new List<GameObject>();
    private Dictionary<Vector2, GameObject> loadedData;

    private void Awake()
    {
        radius = Resources.Load<GameObject>("Prefabs/ActualPiece").transform.localScale.x / 2;
        scale = transform.localScale.x;
        gridScale = GameObject.Find("00").transform.localScale.x;
    }

    private void Start()
    {
        as1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        as2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        as3 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        as4 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        as5 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        ac1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rc"));
        ac2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rc"));
        ar1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rr"));
        ah1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rh"));
        ae1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/re"));
        aa1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ra"));
        ag = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rg"));
        aa2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ra"));
        ae2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/re"));
        ah2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rh"));
        ar2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rr"));
        es1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bs"));
        es2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bs"));
        es3 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bs"));
        es4 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bs"));
        es5 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bs"));
        ec1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bc"));
        ec2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bc"));
        er1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/br"));
        eh1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bh"));
        ee1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/be"));
        ea1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ba"));
        eg = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bg"));
        ea2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ba"));
        ee2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/be"));
        eh2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/bh"));
        er2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/br"));

        SetLocation(as1, boardAttributes.asloc1);
        SetLocation(as2, boardAttributes.asloc2);
        SetLocation(as3, boardAttributes.asloc3);
        SetLocation(as4, boardAttributes.asloc4);
        SetLocation(as5, boardAttributes.asloc5);
        SetLocation(ac1, boardAttributes.acloc1);
        SetLocation(ac2, boardAttributes.acloc2);
        SetLocation(ar1, boardAttributes.arloc1);
        SetLocation(ah1, boardAttributes.ahloc1);
        SetLocation(ae1, boardAttributes.aeloc1);
        SetLocation(aa1, boardAttributes.aaloc1);
        SetLocation(ag, boardAttributes.agloc);
        SetLocation(aa2, boardAttributes.aaloc2);
        SetLocation(ae2, boardAttributes.aeloc2);
        SetLocation(ah2, boardAttributes.ahloc2);
        SetLocation(ar2, boardAttributes.arloc2);
        SetLocation(es1, boardAttributes.esloc1);
        SetLocation(es2, boardAttributes.esloc2);
        SetLocation(es3, boardAttributes.esloc3);
        SetLocation(es4, boardAttributes.esloc4);
        SetLocation(es5, boardAttributes.esloc5);
        SetLocation(ec1, boardAttributes.ecloc1);
        SetLocation(ec2, boardAttributes.ecloc2);
        SetLocation(er1, boardAttributes.erloc1);
        SetLocation(eh1, boardAttributes.ehloc1);
        SetLocation(ee1, boardAttributes.eeloc1);
        SetLocation(ea1, boardAttributes.ealoc1);
        SetLocation(eg, boardAttributes.egloc);
        SetLocation(ea2, boardAttributes.ealoc2);
        SetLocation(ee2, boardAttributes.eeloc2);
        SetLocation(eh2, boardAttributes.ehloc2);
        SetLocation(er2, boardAttributes.erloc2);
        GameObject[] allies = { as1, as2, as3, as4, as5, ac1, ac2, ar1, ah1, ae1, aa1, ag, aa2, ae2, ah2, ar2 };
        foreach (GameObject obj in allies) obj.tag = "Ally";
        GameObject[] enemies = { es1, es2, es3, es4, es5, ec1, ec2, er1, eh1, ee1, ea1, eg, ea2, ee2, eh2, er2 };
        foreach (GameObject obj in enemies) obj.tag = "Enemy";
    }

    public List<GameObject> GetActive() { return active; }
    public List<GameObject> GetInactive() { return inactive; }

    public void reactivate(GameObject obj)
    {
        obj.SetActive(true);
        active.Add(obj);
        inactive.Remove(obj);
    }

    public void deactivate(GameObject obj)
    {
        obj.SetActive(false);
        active.Remove(obj);
        inactive.Add(obj);
    }


    public void SetLocation(GameObject obj, Vector2 loc)
    {
        obj.GetComponent<PieceInfo>().SetStartLocation(loc);
        obj.GetComponent<PieceInfo>().SetLocation(loc);
        obj.transform.position = new Vector3(loc.x * scale + radius, loc.y * scale + radius, obj.transform.localScale.y);
        active.Add(obj);
    }

}
