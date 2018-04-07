using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoLoader : MonoBehaviour {

    public static UserInfo user;
    public static List<BoardAttributes> boards = new List<BoardAttributes>();
    public GameObject selectedBoardPanel, createLineupPanel;

	// Use this for initialization
	void Start () {
        user = new CheatAccount();
		foreach (string path in Directory.GetDirectories("Assets/Resources/Board/Info"))
		{
			Debug.Log (SystemInfo.operatingSystem);
			boards.Add(Resources.Load<BoardAttributes>("Board/Info/" + path.Substring(path.IndexOf("Info") + 5) + "/Attributes"));
		}
        boards = Sorted(boards);
    }

    // Needs Sort By Last Use
    private List<BoardAttributes> Sorted(List<BoardAttributes> boardAttributes)
    {
        List<BoardAttributes> newList = boardAttributes;
        BoardAttributes standardBoard = boardAttributes[0];
        foreach (BoardAttributes attribute in boardAttributes)
            if(attribute.boardName == "Standard Board")
            {
                standardBoard = attribute;
                newList.Remove(attribute);
                break;
            }
        newList.OrderBy(BoardAttributes => BoardAttributes.boardName);
        newList.Insert(0, standardBoard);
        return newList;
    }

    public void Back()
    {
        if (createLineupPanel.activeSelf)
        {
            Destroy(createLineupPanel.transform.Find("BoardPanel/Board/BoardObject(Clone)").gameObject);
            createLineupPanel.SetActive(false);
            selectedBoardPanel.SetActive(true);
        }
        else SceneManager.LoadScene("Main");
    }
}
