﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateContractStore : MonoBehaviour {

    public GameObject popupInputAmountWindow, displayContractsPanel, contractPanel, contractPrefab, standardContract;
    public Text priceText, contractsAmount, playerCoinsAmount, contractDescription;
    public InputField inputField;

    private ContractAttributes current_contract;
    private int many_contracts = 5, contractsCount = 1;
    private List<GameObject> contracts = new List<GameObject>();
    private ContractsManager contractsManager;

    public void Start()
    {
        ChooseContract(standardContract);
        LoadContract();
        contractsManager = contractPanel.GetComponent<ContractsManager>();
    }

    public void ClickModifyAmount()
    {
        popupInputAmountWindow.SetActive(true);
    }

    public void ConfirmInput()
    {
        if (inputField.text != "")
        {
            contractsCount = int.Parse(inputField.text);
            priceText.text = (contractsCount * current_contract.price).ToString();
            contractsAmount.text = contractsCount.ToString() + " Contract";
            if (contractsCount > 1) contractsAmount.text += "s";
            int contractsNumber = contracts.Count;
            if (contractsCount > contracts.Count)
            {
                for (int i = contractsNumber; i < Mathf.Min(contractsCount, many_contracts); i++)
                    LoadContract();
            }
            else if (contractsCount < contracts.Count)
            {
                for (int i = contractsNumber - 1; i >= contractsCount; i--)
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

    public void ChooseContract(GameObject contract)
    {
        current_contract = contract.GetComponent<ContractInfo>().attributes;
        priceText.text = (current_contract.price * contractsCount).ToString();
        contractDescription.text = current_contract.description.Replace("\\n", "\n");
        foreach(GameObject obj in contracts) obj.GetComponent<Image>().sprite = current_contract.image;
    }

    public void Purchase()
    {
        int price = int.Parse(priceText.text);
        if (InfoLoader.user.coins >= price)
        {
            InfoLoader.user.coins -= price;
            playerCoinsAmount.text = InfoLoader.user.coins.ToString();
            contractsManager.AddContract(current_contract, contractsCount);
            // Show Purchase Successful
        }
        else
        {
            // Show Purchase Unsuccessful
        }
    }

    private void LoadContract()
    {
        GameObject contract = Instantiate(contractPrefab, displayContractsPanel.transform);
        contract.GetComponent<Image>().sprite = current_contract.image;
        contracts.Add(contract);
    }
}