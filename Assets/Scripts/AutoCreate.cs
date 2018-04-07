using UnityEngine;

[ExecuteInEditMode]
public class AutoCreate : MonoBehaviour {
    public GameObject obj;
    public int row;
    public int column;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < column; y++)
            {
                Instantiate(obj).name = y.ToString() + x.ToString();
                // obj.transform.position = new Vector3(x, 0, y);
            }
        }

    }
}
