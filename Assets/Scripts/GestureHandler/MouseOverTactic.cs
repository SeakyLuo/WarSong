using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOverTactic : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas parentCanvas;
    public GameObject card;
    public static float xscale = Screen.width / 1920, yscale = Screen.width / 1080;

    private GameObject showCardInfo;
    private float enterTime;
    private static float timeTnterval = 0.05f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.time - enterTime < timeTnterval) return;
        if (showCardInfo != null) Destroy(showCardInfo);
        GameObject tactic = gameObject.transform.Find("Tactic").gameObject;        
        if (!tactic.activeSelf) return;
        enterTime = Time.time;
        showCardInfo = Instantiate(card);
        showCardInfo.transform.SetParent(parentCanvas.transform);
        showCardInfo.GetComponent<CardInfo>().SetAttributes(tactic.GetComponent<TacticInfo>().tactic);
        showCardInfo.transform.localPosition = AdjustedMousePosition() - new Vector3(150 * xscale, 0, 0);
    }

    // Doesn't work as expected.
    public void OnPointerExit(PointerEventData eventData)
    {
        if(showCardInfo!=null) Destroy(showCardInfo);
    }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePosition);
        return mousePosition;
    }
}
