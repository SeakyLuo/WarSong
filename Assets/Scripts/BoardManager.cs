using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {

    public GameObject nextBoardButton, previousBoardButton, confirmButton, preferButton, createLineupButton,
        createLineupPanel, collectionPanel, board;
    public Text boardName;
    public Image boardImage;
    public BoardAttributes standardBoardAttributes;

    private CollectionManager collectionManager;
    private List<BoardAttributes> boardAttributes;
    private int currentBoard = 0;
    private GameObject loadedBoard;

    private void Start()
    {
        boardAttributes = InfoLoader.boards;
        collectionManager = collectionPanel.GetComponent<CollectionManager>();
        DisplayBoardSelectionInterface();
        gameObject.SetActive(false);
    }

    public void NextBoard()
    {
        if (++currentBoard == boardAttributes.Count - 1)
            nextBoardButton.SetActive(false);
        previousBoardButton.SetActive(true);
        DisplayBoardSelectionInterface();
    }

    public void PreviousBoard()
    {
        if (--currentBoard == 0)
            previousBoardButton.SetActive(false);
        nextBoardButton.SetActive(true);
        DisplayBoardSelectionInterface();
    }

    private void DisplayBoardSelectionInterface()
    {
        boardName.text = boardAttributes[currentBoard].boardName;
        boardImage.sprite = boardAttributes[currentBoard].completeImage;
        if (boardAttributes[currentBoard].available)
        {
            preferButton.SetActive(true);
            confirmButton.SetActive(true);
        }
        else
        {
            preferButton.SetActive(false);
            confirmButton.SetActive(false);
        }            
    }

    public void ConfirmBoardSelection()
    {
        gameObject.SetActive(false);
        LoadBoard(boardAttributes[currentBoard]);
    }

    public void LoadBoard(Lineup lineup)
    {
        LoadBoard(Resources.Load<BoardAttributes>("Board/Info/" + lineup.boardName + "/Attributes"), lineup.cardLocations);
    }

    public void LoadBoard(BoardAttributes attributes, Dictionary<Vector2, Collection> newLocations = null)
    {
        collectionManager.SetCardsPerPage(4);
        collectionManager.ShowCurrentPage();
        createLineupPanel.SetActive(true);
        loadedBoard = Instantiate(Resources.Load<GameObject>("Board/Info/" + attributes.boardName + "/BoardObject"), board.transform);
        loadedBoard.transform.localPosition = new Vector3(0, 0, 0);
        loadedBoard.SetActive(true);
        loadedBoard.GetComponent<BoardInfo>().SetAttributes(attributes, newLocations);
    }

    public void BackToCollection()
    {
        collectionManager.SetCardsPerPage(8);
        collectionManager.ShowCurrentPage();
        gameObject.SetActive(false);
        createLineupButton.SetActive(true);
    }

    public void DestroyBoard()
    {
        Destroy(loadedBoard);
    }
}
