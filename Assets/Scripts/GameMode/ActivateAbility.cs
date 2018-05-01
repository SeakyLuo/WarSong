using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateAbility : MonoBehaviour {

    private static Button button;
    private static GameObject text;

	// Use this for initialization
	void Start () {
        button = GetComponent<Button>();
        text = transform.Find("Text").gameObject;
    }

    public void ActivatePieceAbility()
    {

    }

    public static void Activate()
    {
        // if activatable
        button.interactable = true;
        text.SetActive(true);
    }

    public static void Deactivate()
    {
        button.interactable = false;
        text.SetActive(false);
    }
}
