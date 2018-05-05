using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Challenge", menuName = "Challenge")]
public class Challenge: ScriptableObject {

    public string challengeName;
    [TextArea(2, 3)]
    public string description;
    public int progress;    // a of a/b
    public int requirement; // b of a/b
    public int reward;

}