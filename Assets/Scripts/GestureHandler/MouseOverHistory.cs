using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverHistory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image cardImage;
    public GameObject historyCard;
    public GameEvent gameEvent;

    private PieceAttributes pieceAttributes;
    private TacticAttributes tacticAttributes;
    private TrapAttributes trapAttributes;

    public void SetAttributes(GameEvent game_event)
    {
        gameEvent = game_event;
        if (gameEvent.playerID == Login.playerID) background.sprite = allyBackground;
        else background.sprite = enemyBackground;
        if (gameEvent.piece)
        {
            PieceAttributes pieceAttributes = Database.FindPieceAttributes(gameEvent.eventTrigger);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        historyCard.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        historyCard.SetActive(false);
    }
}
