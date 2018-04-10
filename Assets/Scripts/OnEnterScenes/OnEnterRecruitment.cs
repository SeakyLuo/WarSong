using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnEnterRecruitment : MonoBehaviour, IPointerClickHandler {

    public GameObject store, popupInputAmountWindow;
    private Camera canvasCamera;
    private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
        canvasCamera = gameObject.GetComponent<Canvas>().worldCamera;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
	
    public void BackToMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenStoreWindow()
    {
        store.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (store.activeSelf)
        {
            Vector3 mouseposition = AdjustedMousePosition();
            Rect rect = store.GetComponent<RectTransform>().rect;
            if (-rect.width / 2 > mouseposition.x || mouseposition.x > rect.width / 2 || -rect.height / 2 > mouseposition.y || mouseposition.y > rect.height / 2)
            {
                store.SetActive(false);
            }
        }            
    }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, canvasCamera, out mousePosition);
        return mousePosition;
    }
}
