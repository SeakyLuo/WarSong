using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnEnterCollections : MonoBehaviour {

    public GameObject selectBoardPanel, createLineupPanel, settingsPanel;

    private CollectionManager collectionManager;

    private void Start()
    {
        collectionManager = transform.Find("Collection").GetComponent<CollectionManager>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            settingsPanel.SetActive(true);
    }

    public void Back()
    {
        if (createLineupPanel.activeSelf)
        {
            collectionManager.SetCardsPerPage(8);
            Destroy(createLineupPanel.transform.Find("BoardPanel/Board/LineupBoard(Clone)").gameObject);
            createLineupPanel.SetActive(false);
            if (LineupsManager.modifyLineup == -1)
                selectBoardPanel.SetActive(true);
        }
        else if (selectBoardPanel.activeSelf)
            selectBoardPanel.SetActive(false);
        else
            SceneManager.LoadScene(InfoLoader.switchSceneCaller);
    }
}
