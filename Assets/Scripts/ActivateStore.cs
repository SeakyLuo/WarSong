using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateStore : MonoBehaviour {

    public GameObject purchaseWithCoins, purchaseWithMoney, popupInputAmountWindow;
    public Text choiceContractsAmount, choiceCoinsAmount, purchaseWithCoinsText;
    public InputField inputField;

    private bool clicked = false;
    private float firstClick = 0f, clickInterval = 1f;

    // Use this for initialization
    void Start () {

    }
	
	public void ClickCoinChoice()
    {
        if (clicked)
        {
            clicked = false;
            if (Time.time - firstClick < clickInterval)
            {
                popupInputAmountWindow.SetActive(true);
            }
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

    }

    public void ConfirmInput()
    {
        string contractCount = (int.Parse(inputField.text)*10).ToString();
        purchaseWithCoinsText.text = contractCount;
        choiceCoinsAmount.text = inputField.text;
        choiceContractsAmount.text = contractCount;
        popupInputAmountWindow.SetActive(false);
    }

    public void CancelInput()
    {
        popupInputAmountWindow.SetActive(false);
    }
}
