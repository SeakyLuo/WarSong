using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapInfo : MonoBehaviour {

    public Text nameText;
    public Text descriptionText;
    public Image image;

	public void SetAttributes(Trap trap)
    {
        nameText.text = trap.name;
        descriptionText.text = trap.description;
        image.sprite = trap.image;
    }
}
