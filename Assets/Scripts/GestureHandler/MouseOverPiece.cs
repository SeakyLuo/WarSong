using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseOverPiece : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler//, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{    
    public GameObject card;
    public static float xscale = Screen.width / 1920, yscale = Screen.width / 1080;

    private bool dragBegins;
    private GameObject parentCanvas, showCardInfo, dragCard;
    private Image cardImage;
    private BoardInfo boardInfo;
    private Vector2 nameLoc;
    private float enterTime;
    private static float timeTnterval = 0.05f;
    private Color tmpColor;

    private void Start()
    {
        parentCanvas = GameObject.Find("Canvas");
        cardImage = gameObject.transform.Find("CardImage").GetComponent<Image>();
        //parentCanvas = gameObject.transform.parent.parent.parent.parent.parent;
        boardInfo = gameObject.transform.parent.parent.GetComponent<BoardInfo>();
        nameLoc = StringToVector2(gameObject.name);
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    dragBegins = true;
    //    EnableImage(cardImage, false);
    //    dragCard = Instantiate(card, parentCanvas.transform);
    //    dragCard.GetComponent<CardInfo>().SetAttributes(boardInfo.cardLocations[StringToVector2(gameObject.name)]);
    //    dragCard.transform.position = AdjustedMousePosition();
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (!dragBegins) return;
    //    dragCard.transform.position = AdjustedMousePosition();
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if (!dragBegins) return;
    //    dragBegins = false;
    //    CardInfo newCard = dragCard.GetComponent<CardInfo>();
    //    if (!newCard.GetCardName().StartsWith("Standard "))
    //    {
    //        cardImage.sprite = null;
    //        GameObject find = gameObject.transform.parent.Find(FindLoc(Input.mousePosition));
    //        if (find != null)
    //        {
    //            Transform oldObject = find.transform;
    //            Image oldCardImage = oldObject.Find("CardImage").GetComponent<Image>();
    //            if (oldCardImage.sprite != null)
    //            {
    //                 switch
    //                PieceAttributes attributes = boardInfo.attributesDict[oldObject.name];
    //                if (attributes.type == newCard.GetCardType())
    //                {
    //                    boardInfo.SetCard(attributes, StringToVector2(gameObject.name));
    //                    cardImage.sprite = attributes.image;
    //                    boardInfo.SetCard(newCard.piece, StringToVector2(oldObject.name));
    //                    oldCardImage.sprite = newCard.piece.image;
    //                }
    //                else
    //                {
    //                     Show Animation: Can't Switch
    //                    cardImage.GetComponent<Image>().sprite = dragCard.GetComponent<CardInfo>().piece.image;
    //                }
    //            }
    //            else
    //            {
    //                 Drag to an empty spot and resume.
    //                cardImage.GetComponent<Image>().sprite = dragCard.GetComponent<CardInfo>().piece.image;
    //            }
    //        }
    //        else
    //        {
    //             Drag outside the board.
    //            string cardType = newCard.GetCardType();
    //            collectionManager.AddCollection(new Collection(newCard.piece));
    //            boardInfo.SetStandardCard(newCard.piece.type, StringToVector2(gameObject.name));
    //            collectionManager.RemoveCollection(new Collection("Standard " + cardType, cardType));
    //            cardImage.sprite = BoardInfo.standardAttributes["Standard " + cardType].image;
    //             drag to a card to switch?
    //        }
    //    }
    //    EnableImage(cardImage);
    //    Destroy(dragCard);
    //}

    //private void EnableImage(Image image, bool enable = true)
    //{
    //    tmpColor = image.color;
    //    if (enable) tmpColor.a = 255;
    //    else tmpColor.a = 0;
    //    image.color = tmpColor;
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{

    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.time - enterTime < timeTnterval) return;
        if (showCardInfo != null) Destroy(showCardInfo);
        enterTime = Time.time;
        showCardInfo = Instantiate(card);
        showCardInfo.transform.SetParent(parentCanvas.transform);
        showCardInfo.GetComponent<CardInfo>().SetAttributes(boardInfo.cardLocations[nameLoc]);
        showCardInfo.transform.position = AdjustedMousePosition() + new Vector3(-150 * xscale, 150 * yscale, 0);
    }

    // Doesn't work as expected.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (showCardInfo != null) Destroy(showCardInfo);
    }

    private Vector2 StringToVector2(string loc) { return new Vector2((int)Char.GetNumericValue(loc[0]), (int)Char.GetNumericValue(loc[1])); }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.GetComponent<Canvas>().worldCamera, out mousePosition);
        return mousePosition;
    }
}
