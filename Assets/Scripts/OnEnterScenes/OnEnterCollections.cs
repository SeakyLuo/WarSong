using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnEnterCollections : MonoBehaviour {

    public GameObject selectedBoardPanel, createLineupPanel;

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

    public void EnterRecruitment()
    {
        SceneManager.LoadScene("Recruitment");
    }
}
