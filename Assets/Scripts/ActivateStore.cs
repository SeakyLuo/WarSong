using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateStore : MonoBehaviour {

    public GameObject purchaseWithCoins, purchaseWithMoney, popupInputAmountWindow;
    public Text choiceContractsAmount, choiceCoinsAmount, purchaseWithCoinsText, playerCoinsAmount;
    public InputField inputField;

    private bool clicked = false;
    private float firstClick = 0f, clickInterval = 1f;

    // Use this for initialization
    void Start () {

    }
	
	public void ClickCoinChoice()
    {
        if (Time.time - firstClick > clickInterval) clicked = false;
        if (clicked)
        {
            firstClick = Time.time;
            popupInputAmountWindow.SetActive(true);
        }
        else
        {
            clicked = true;
            purchaseWithCoins.SetActive(true);
            purchaseWithMoney.SetActive(false);
            firstClick = Time.time;
        }
        
    }

    public void ClickMoneyChoice(Text amount)
    {
        purchaseWithCoins.SetActive(false);
        purchaseWithMoney.SetActive(true);
        purchaseWithMoney.GetComponent<Text>().text = amount.text;
    }

    public void KeepIntOnly()
    {
        int amount = int.Parse(inputField.text);
        if (amount > 99)
            inputField.text = Mathf.Floor(amount/10).ToString();
        else
            inputField.text = amount.ToString();
        
    }

    public void ConfirmInput()
    {
        if (inputField.text != "")
        {
            int contract = int.Parse(inputField.text);
            string coins = (contract * 10).ToString();
            purchaseWithCoinsText.text = coins;
            choiceCoinsAmount.text = coins;
            choiceContractsAmount.text = inputField.text + " Contract";
            if (contract > 1) choiceContractsAmount.text += "s";
        }
        popupInputAmountWindow.SetActive(false);
    }

    public void CancelInput()
    {
        popupInputAmountWindow.SetActive(false);
    }

    public void Purchase()
    {
        if (purchaseWithCoins.activeSelf)
        {
            OnEnterRecruitment.user.coins -= int.Parse(purchaseWithCoinsText.text);
            playerCoinsAmount.text = OnEnterRecruitment.user.coins.ToString();
            // Add card packs
            gameObject.SetActive(false);
        }
    }
}
