using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContractsManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas parentCanvas;
    public GameObject GetCards;
    public GameObject dragContract;
    public GameObject useAllContracts;
    public List<GameObject> contractSlots = new List<GameObject>();
    public static List<string> contractName = new List<string>()
    {
        "Standard Contract", "Artillery Seller", "Human Resource", "Animal Smuggler", "Wise Elder"
    };

    private GameObject targetContract;

    private bool dragBegins = false;

    // Use this for initialization
    private void Start()
    {
        foreach (KeyValuePair<string, int> pair in InfoLoader.user.contracts)
            if (pair.Value != 0)
                contractSlots[contractName.IndexOf(pair.Key)].GetComponent<PlayerContract>().SetCount(pair.Value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject selectedObject = eventData.pointerCurrentRaycast.gameObject;
        bool contractDragged = (selectedObject.tag == "Contract"),
             countDragged = (selectedObject.name == "CountPanel");
        if (contractDragged || countDragged)
        {
            dragBegins = true;
            dragContract.SetActive(true);
            dragContract.transform.position = Input.mousePosition;
            if (contractDragged)
            {
                targetContract = selectedObject.transform.parent.gameObject;
                dragContract.GetComponent<PlayerContract>().SetAttributes(targetContract.GetComponent<PlayerContract>().attributes);
                targetContract.GetComponent<PlayerContract>().SetCount(InfoLoader.user.contracts[targetContract.name] - 1);
                dragContract.GetComponent<PlayerContract>().SetCount(1);
            }
            else
            {
                targetContract = selectedObject.transform.parent.parent.gameObject;
                dragContract.GetComponent<PlayerContract>().SetAttributes(targetContract.GetComponent<PlayerContract>().attributes);
                targetContract.GetComponent<PlayerContract>().SetCount(0);
                dragContract.GetComponent<PlayerContract>().SetCount(InfoLoader.user.contracts[targetContract.name]);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        dragContract.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragBegins) return;
        dragBegins = false;
        dragContract.SetActive(false);
        if (Input.mousePosition.y >= GetComponent<RectTransform>().rect.height)
        {
            if (dragContract.GetComponent<PlayerContract>().GetCount() == 1)
            {
                --InfoLoader.user.contracts[targetContract.name];
                // retrieve cards from the server and set them
                GetCards.SetActive(true);
            }
            else
            {
                useAllContracts.SetActive(true);
            }
        }
        else CancelUseAllContract();
    }

    public void AddContract(ContractAttributes contractAttributes, int contractsCount)
    {
        InfoLoader.user.contracts[contractAttributes.contractName] += contractsCount;
        contractSlots[contractName.IndexOf(contractAttributes.contractName)].GetComponent<PlayerContract>().SetCount(InfoLoader.user.contracts[contractAttributes.contractName]);
    }

    public void CancelUseAllContract()
    {
        useAllContracts.SetActive(false);
        targetContract.GetComponent<PlayerContract>().SetCount(InfoLoader.user.contracts[targetContract.name]);
    }

    public void UseAllContracts()
    {
        useAllContracts.SetActive(false);
        GetCards.SetActive(true);
    }
}
