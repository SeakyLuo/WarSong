using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateContractStore : MonoBehaviour {

    public GameObject popupInputAmountWindow, displayContractsPanel;
    public Text coinsText, contractsAmount, playerCoinsAmount;
    public InputField inputField;

    private int many_contracts = 5;
    private List<Object> contracts = new List<Object>();

    public void Start()
    {
        LoadContract("StandardContract");
    }

    public void ClickModifyCoins()
    {
        popupInputAmountWindow.SetActive(true);
    }

    public void ConfirmInput()
    {
        if (inputField.text != "")
        {
            int purchaseContractsCount = int.Parse(inputField.text);
            string coins = (purchaseContractsCount * 10).ToString();
            coinsText.text = coins;
            contractsAmount.text = purchaseContractsCount.ToString() + " Contract";
            if (purchaseContractsCount > 1) contractsAmount.text += "s";
            int contractsCount = contracts.Count;
            if (purchaseContractsCount > contracts.Count)
            {
                for (int i = contractsCount; i < Mathf.Min(purchaseContractsCount, many_contracts); i++)
                    LoadContract("StandardContract");
            }
            else if (purchaseContractsCount < contracts.Count)
            {
                for (int i = contractsCount - 1; i >= purchaseContractsCount; i--)
                {
                    Destroy(contracts[i]);
                    contracts.RemoveAt(i);
                }
            }
        }
        popupInputAmountWindow.SetActive(false);
    }

    public void CancelInput()
    {
        popupInputAmountWindow.SetActive(false);
    }

    public void Purchase()
    {
        int coins = int.Parse(coinsText.text);
        if (InfoLoader.user.coins >= coins)
        {
            InfoLoader.user.coins -= coins;
            playerCoinsAmount.text = InfoLoader.user.coins.ToString();
            // Add card packs
            // Purchase Successful
        }else{
            // Purchase Unsuccessful
        }

    }

    private void LoadContract(string contractName)
    {        
        contracts.Add(Instantiate(Resources.Load<Object>("Recruitment/"+contractName+"/Contract"), displayContractsPanel.transform));
    }
}
