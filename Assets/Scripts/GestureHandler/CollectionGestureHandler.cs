﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectionGestureHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject collectionPanel ,createLineupPanel;
    public Canvas parentCanvas;
    public static string CARDSLOTPANEL = "CardSlotPanel";
    public static float xscale = Screen.width / 1920, yscale = Screen.width / 1080;

    private BoardInfo boardInfo;
    private GameObject dragCard;
    private bool dragBegins = false;
    private LineupBuilder lineupBuilder;

    private void Start()
    {
        lineupBuilder = createLineupPanel.GetComponent<LineupBuilder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!createLineupPanel.activeSelf) return;
        GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;
        if (selectedObject.name == CARDSLOTPANEL)
        {
            dragBegins = true;
            dragCard = Instantiate(selectedObject.transform.parent.Find("Card").gameObject, parentCanvas.transform);
            dragCard.transform.position = AdjustedMousePosition();
            dragCard.GetComponent<CardInfo>().SetAttributes(selectedObject.transform.parent.Find("Card").GetComponent<CardInfo>());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        dragCard.transform.position = AdjustedMousePosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        dragBegins = false;
        CardInfo cardInfo = dragCard.GetComponent<CardInfo>();
        if (InTacticRegion(Input.mousePosition) && cardInfo.GetCardType() == "Tactic")
            lineupBuilder.AddTactic(cardInfo);
        else if (InBoardRegion(Input.mousePosition) && cardInfo.GetCardType() != "Tactic")
            lineupBuilder.AddPiece(cardInfo, Input.mousePosition);
        Destroy(dragCard);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!createLineupPanel.activeSelf) return;
        GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;
        if (selectedObject.name == CARDSLOTPANEL)
        {
            CardInfo cardInfo = selectedObject.transform.parent.Find("Card").GetComponent<CardInfo>();
            if (cardInfo.GetCardType() == "Tactic")
            {
                // Same tactic should warn.
                lineupBuilder.AddTactic(cardInfo);
            }            
            else
            {
                string cardName = cardInfo.GetCardName();
                foreach (Vector2 loc in boardInfo.typeLocations[cardInfo.GetCardType()])
                {
                    string oldLocPieceName = boardInfo.cardLocations[loc].name;
                    if ((cardInfo.IsStandard() && !oldLocPieceName.StartsWith("Standard ")) ||
                        (!cardInfo.IsStandard() && oldLocPieceName.StartsWith("Standard ")) ||
                        (cardInfo.GetCardType() == "General" && cardName != oldLocPieceName)) // need to compare with count
                    {
                        lineupBuilder.AddPiece(cardInfo, loc);
                        break;
                    }
                }
                //GameObject clickCard = Instantiate(selectedObject.transform.parent.Find("Card").gameObject, parentCanvas.transform);
                //string cardName = cardInfo.GetCardName();
                //foreach (Vector2 loc in boardInfo.typeLocations[cardInfo.GetCardType()])
                //{
                //    string oldLocPieceName = boardInfo.cardLocations[loc].name;
                //    if ((cardInfo.IsStandard() && !oldLocPieceName.StartsWith("Standard ")) ||
                //        (!cardInfo.IsStandard() && oldLocPieceName.StartsWith("Standard ")) ||
                //        (cardInfo.GetCardType() == "General" && cardName != oldLocPieceName)) // need to compare with count
                //    {
                //        clickCard.GetComponent<CardInfo>().SetAttributes(cardInfo);
                //        lineupBuilder.AddPiece(clickCard.GetComponent<CardInfo>(), loc);
                //        break;
                //    }
                //}
                //Destroy(clickCard);
            }            
        }
    }

    // needs swipe for mobile device

    private bool InTacticRegion(Vector2 pos) { return createLineupPanel.activeSelf && 0.75 * Screen.width <= pos.x && pos.x <= Screen.width && 100 * yscale <= pos.y && pos.y <= 1000 * yscale; }

    private bool InSwipeRegion(Vector2 pos)
    {
        int ymin = 60;
        if (createLineupPanel.activeSelf) ymin = 560;
        return 0 <= pos.x && pos.x <= 0.75 * Screen.width && ymin * yscale <= pos.y && pos.y <= 1020 * yscale;
    }

    private bool InBoardRegion(Vector2 pos) { return 200 * xscale <= pos.x && pos.x <= 1440 * xscale && 10 * yscale <= pos.y && pos.y <= 510 * yscale; }

    public void SetBoardInfo(BoardInfo info) { boardInfo = info; }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePosition);
        return mousePosition;
    }
}
