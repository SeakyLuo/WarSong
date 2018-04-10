using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnEnterRecruitment : MonoBehaviour, IPointerClickHandler {

    public GameObject store, popupInputAmountWindow;


	// Use this for initialization
	void Start () {
		
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
            //if (popupInputAmountWindow.activeSelf && eventData.pointerCurrentRaycast.gameObject != popupInputAmountWindow)
            //    popupInputAmountWindow.SetActive(false);
            //else if(eventData.pointerCurrentRaycast.gameObject != store)
            //    store.SetActive(false);
        }            
    }
}
