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
    private BoardInfo boardInfo;
    private Vector2 nameLoc;
    private float enterTime;
    private static float timeTnterval = 0.05f;
    private Color tmpColor;

    private void Start()
    {
        parentCanvas = GameObject.Find("Canvas");
        //parentCanvas = gameObject.transform.parent.parent.parent.parent.parent;
        boardInfo = gameObject.transform.parent.parent.GetComponent<BoardInfo>();
        nameLoc = StringToVector2(gameObject.name);
    }

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
