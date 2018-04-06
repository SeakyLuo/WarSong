using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineupManager : MonoBehaviour {

    public InputField input;
    public BoardAttributes attributes;

    private Lineup lineup = new Lineup();
    private string lineupName;

    private GameObject s1;
    private GameObject s2;
    private GameObject s3;
    private GameObject s4;
    private GameObject s5;
    private GameObject c1;
    private GameObject c2;
    private GameObject r1;
    private GameObject r2;
    private GameObject h1;
    private GameObject h2;
    private GameObject e1;
    private GameObject e2;
    private GameObject a1;
    private GameObject a2;
    private GameObject g;

    private Vector2 s1loc;
    private Vector2 s2loc;
    private Vector2 s3loc;
    private Vector2 s4loc;
    private Vector2 s5loc;
    private Vector2 c1loc;
    private Vector2 c2loc;
    private Vector2 r1loc;
    private Vector2 r2loc;
    private Vector2 h1loc;
    private Vector2 h2loc;
    private Vector2 e1loc;
    private Vector2 e2loc;
    private Vector2 a1loc;
    private Vector2 a2loc;
    private Vector2 gloc;

    public Dictionary<Vector2, GameObject> objloc;

    private void Awake()
    {
        s1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        s2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        s3 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        s4 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        s5 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rs"));
        c1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rc"));
        c2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rc"));
        r1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rr"));
        r2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rr"));
        h1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rh"));
        h2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rh"));
        e1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/re"));
        e2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/re"));
        a1 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ra"));
        a2 = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/ra"));
        g = Instantiate(Resources.Load<GameObject>("Pieces/Thumbnails/rg"));

        attributes = Resources.Load<BoardAttributes>("Board/Attributes/StandardBoard");

        s1loc = attributes.asloc1;
        s2loc = attributes.asloc2;
        s3loc = attributes.asloc3;
        s4loc = attributes.asloc4;
        s5loc = attributes.asloc5;
        c1loc = attributes.acloc1;
        c2loc = attributes.acloc2;
        r1loc = attributes.arloc1;
        r2loc = attributes.arloc2;
        h1loc = attributes.ahloc1;
        h2loc = attributes.ahloc2;
        e1loc = attributes.aeloc1;
        e2loc = attributes.aeloc2;
        a1loc = attributes.aaloc1;
        a2loc = attributes.aaloc2;
        gloc = attributes.agloc;

        lineupName = input.text;
}

    // Use this for initialization
    void Start () {
        objloc = new Dictionary<Vector2, GameObject>{
            { s1loc, s1},
            { s2loc, s2},
            { s3loc, s3},
            { s4loc, s4},
            { s5loc, s5},
            { c1loc, c1},
            { c2loc, c2},
            { r1loc, r1},
            { r2loc, r2},
            { h1loc, h1},
            { h2loc, h2},
            { e1loc, e1},
            { e2loc, e2},
            { a1loc, a1},
            { a2loc, a2},
            { gloc, g},
        };
        foreach (KeyValuePair<Vector2, GameObject> item in objloc) SetLocation(item.Value, item.Key);
        save();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void SetLocation(GameObject obj, Vector2 loc) { obj.transform.position = new Vector3(loc.x, 0, loc.y) * transform.localScale.x; }

    public void delete()
    {

    }

    public void save()
    {
        Dictionary<Vector2, string> nameloc = new Dictionary<Vector2, string>();
        foreach (KeyValuePair<Vector2, GameObject> item in objloc) nameloc.Add(item.Key, item.Value.name);
        load();
    }

    public void load()
    {
    }

    private void rename() { lineupName = input.text; }
    
}
