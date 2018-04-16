using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SwitchScenes : MonoBehaviour, IPointerClickHandler
{    
    public GameObject playerInfo;
    public Text time;

    private Canvas parentCanvas;

    private void Start()
    {
        parentCanvas = gameObject.GetComponent<Canvas>();
    }

    private void FixedUpdate()
    {
        time.text = DateTime.Now.ToString("h:mm tt");
    }

    public void EnterCollection()
    {
        SceneManager.LoadScene("Collections");
    }

    public void EnterWar()
    {
        //SceneManager.LoadScene("DemoGameMode");
        SceneManager.LoadScene("PlayerMatching");
    }

    public void EnterRecruitment()
    {
        SceneManager.LoadScene("Recruitment");
    }

    public void ShowPlayerInfo()
    {
        playerInfo.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!playerInfo.activeSelf) return;
        Vector2 mousePosition = AdjustedMousePosition();
        Rect rect = playerInfo.GetComponent<RectTransform>().rect;
        // rect.x and rect.y are negative
        if (mousePosition.x < rect.x || mousePosition.x > -rect.x || mousePosition.y < rect.y || mousePosition.y > -rect.y)
            playerInfo.SetActive(false);
    }

    private Vector3 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePosition);
        return mousePosition;
    }
}
