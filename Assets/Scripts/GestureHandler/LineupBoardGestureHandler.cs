﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineupBoardGestureHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static string GRIDSLOTPANEL = "GridSlotPanel";
    public static bool dragBegins;
    public static float xscale = Screen.width / 1920, yscale = Screen.width / 1080;

    public Canvas parentCanvas;
    public GameObject collectionPanel, createLineupPanel, infoCard, cantSwitch;

    private GameObject selectedObject, clickedObject, mouseOver, showCardInfo;
    private Transform parent;
    private CollectionManager collectionManager;
    private BoardInfo boardInfo;
    private Image cardImage;
    private Color tmpColor;

    private void Start()
    {
        collectionManager = collectionPanel.GetComponent<CollectionManager>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        selectedObject = eventData.pointerCurrentRaycast.gameObject;
        parent = selectedObject.transform.parent;
        cardImage = parent.Find("CardImage").GetComponent<Image>();
        if (selectedObject.name == GRIDSLOTPANEL && cardImage.sprite!=null && cardImage.sprite.name == "Image")
        {
            dragBegins = true;            
            EnableImage(cardImage, false);
            infoCard.SetActive(true);
            infoCard.GetComponent<CardInfo>().SetAttributes(boardInfo.cardLocations[InfoLoader.StringToVec2(parent.name)]);
            infoCard.transform.position = AdjustedMousePosition();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        infoCard.transform.position = AdjustedMousePosition();
    }

    public string FindLoc(Vector3 loc)
    {
        int x = (int)Mathf.Floor((loc.x - 380 * xscale) / (100 * xscale));
        int y = (int)Mathf.Floor((loc.y - 10 * yscale) / (100 * yscale));
        return x.ToString() + y.ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        dragBegins = false;
        if (!infoCard.GetComponent<CardInfo>().IsStandard())
        {
            cardImage.GetComponent<Image>().sprite = null;
            CardInfo newCard = infoCard.GetComponent<CardInfo>();
            GameObject find = GameObject.Find(FindLoc(Input.mousePosition));
            if (find != null)
            {
                Transform oldObject = find.transform;
                Image oldCardImage = oldObject.Find("CardImage").GetComponent<Image>();
                if (oldCardImage.sprite != null)
                {
                    // switch
                    PieceAttributes attributes = boardInfo.attributesDict[oldObject.name];                    
                    if (attributes.type == newCard.GetCardType())
                    {
                        boardInfo.SetCard(attributes, InfoLoader.StringToVec2(parent.name));
                        cardImage.GetComponent<Image>().sprite = attributes.image;
                        parent = oldObject;
                        boardInfo.SetCard(newCard.piece, InfoLoader.StringToVec2(parent.name));
                        oldCardImage.sprite = newCard.piece.image;
                    }
                    else
                    {
                        // Show Animation: Can't Switch
                        StartCoroutine(ShowCantSwitch());
                        cardImage.GetComponent<Image>().sprite = infoCard.GetComponent<CardInfo>().piece.image;
                    }
                }
                else
                {
                    // Drag to an empty spot and resume.
                    cardImage.GetComponent<Image>().sprite = infoCard.GetComponent<CardInfo>().piece.image;
                }
            }
            else
            {
                // Drag outside the board.
                string cardType = newCard.GetCardType();
                collectionManager.AddCollection(new Collection(newCard.piece));
                boardInfo.SetStandardCard(cardType, InfoLoader.StringToVec2(parent.name));
                collectionManager.RemoveCollection(Collection.standardCollectionDict[cardType]);
                cardImage.sprite = InfoLoader.standardAttributes["Standard " + cardType].image;
                // drag to a infoCard to switch?
            }
        }
        EnableImage(cardImage);
        infoCard.SetActive(false);
    }

    private IEnumerator ShowCantSwitch()
    {
        cantSwitch.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        cantSwitch.SetActive(false);
    }

    private void EnableImage(Image image, bool enable = true)
    {
        tmpColor = image.color;
        if (enable) tmpColor.a = 255;
        else tmpColor.a = 0;
        image.color = tmpColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        string cardType, parentName = clickedObject.transform.parent.name;
        if (clickedObject.name == GRIDSLOTPANEL && boardInfo.locationType.TryGetValue(parentName, out cardType))
        {
            collectionManager.ClickTab(cardType);
        }
    }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePosition);
        return mousePosition;
    }

    public void SetBoardInfo(BoardInfo info) { boardInfo = info; }
}
