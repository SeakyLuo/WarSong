using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setup : MonoBehaviour {

    public static float radius;
    public static float scale;
    public static float gridScale;

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
        // Lineup lineup = DataSaver.loadData<Lineup>("Lineup");
        // string jsonData = PlayerPrefs.GetString("Lineup");
        // Dictionary<Vector2, GameObject> loadedData = JsonUtility.FromJson<Dictionary<Vector2, GameObject>>(jsonData);
        // foreach (KeyValuePair<Vector2, string> item in lineup.location) Debug.Log(item);
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

        //SetLocation(as1, BoardInfo.Getasloc1());
        //SetLocation(as2, BoardInfo.Getasloc2());
        //SetLocation(as3, BoardInfo.Getasloc3());
        //SetLocation(as4, BoardInfo.Getasloc4());
        //SetLocation(as5, BoardInfo.Getasloc5());
        //SetLocation(ac1, BoardInfo.Getacloc1());
        //SetLocation(ac2, BoardInfo.Getacloc2());
        //SetLocation(ar1, BoardInfo.Getarloc1());
        //SetLocation(ah1, BoardInfo.Getahloc1());
        //SetLocation(ae1, BoardInfo.Getaeloc1());
        //SetLocation(aa1, BoardInfo.Getaaloc1());
        //SetLocation(ag, BoardInfo.Getagloc());
        //SetLocation(aa2, BoardInfo.Getaaloc2());
        //SetLocation(ae2, BoardInfo.Getaeloc2());
        //SetLocation(ah2, BoardInfo.Getahloc2());
        //SetLocation(ar2, BoardInfo.Getarloc2());
        //SetLocation(es1, BoardInfo.Getesloc1());
        //SetLocation(es2, BoardInfo.Getesloc2());
        //SetLocation(es3, BoardInfo.Getesloc3());
        //SetLocation(es4, BoardInfo.Getesloc4());
        //SetLocation(es5, BoardInfo.Getesloc5());
        //SetLocation(ec1, BoardInfo.Getecloc1());
        //SetLocation(ec2, BoardInfo.Getecloc2());
        //SetLocation(er1, BoardInfo.Geterloc1());
        //SetLocation(eh1, BoardInfo.Getehloc1());
        //SetLocation(ee1, BoardInfo.Geteeloc1());
        //SetLocation(ea1, BoardInfo.Getealoc1());
        //SetLocation(eg, BoardInfo.Getegloc());
        //SetLocation(ea2, BoardInfo.Getealoc2());
        //SetLocation(ee2, BoardInfo.Geteeloc2());
        //SetLocation(eh2, BoardInfo.Getehloc2());
        //SetLocation(er2, BoardInfo.Geterloc2());
        GameObject[] allies = { as1, as2, as3, as4, as5, ac1, ac2, ar1, ah1, ae1, aa1, ag, aa2, ae2, ah2, ar2 };
        foreach (GameObject obj in allies) obj.tag = "Ally";
        GameObject[] enemies = { es1, es2, es3, es4, es5, ec1, ec2, er1, eh1, ee1, ea1, eg, ea2, ee2, eh2, er2 };
        foreach (GameObject obj in enemies) obj.tag = "Enemy";
    }

    public List<GameObject> GetActive() { return active; }
    public List<GameObject> GetInactive() { return inactive; }

    public void reactivate(GameObject obj)
    {
        obj.GetComponent<CardInfo>().SetActive(true);
        obj.SetActive(true);
        active.Add(obj);
        inactive.Remove(obj);
    }

    public void deactivate(GameObject obj)
    {
        obj.GetComponent<CardInfo>().SetActive(false);
        obj.SetActive(false);
        active.Remove(obj);
        inactive.Add(obj);
    }


    public void SetLocation(GameObject obj, Vector2 loc)
    {
        obj.GetComponent<CardInfo>().SetStartLocation(loc);
        obj.GetComponent<CardInfo>().SetLocation(loc);
        obj.transform.position = new Vector3(loc.x * scale + radius, obj.transform.localScale.y, loc.y * scale + radius);
        active.Add(obj);
    }

}
