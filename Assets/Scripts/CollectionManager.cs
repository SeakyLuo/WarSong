using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;


public class CollectionManager : MonoBehaviour {
 
    public static string[] types = new string[] { "General", "Advisor", "Elephant", "Horse", "Chariot", "Cannon", "Soldier", "Tactic" };
    public int cardsPerPage = 8;
    public Dictionary<string, int> pageLimits = new Dictionary<string, int>();
    public KeyValuePair<string, int> currentPage = new KeyValuePair<string, int>("General", 1), notFound = new KeyValuePair<string, int>("", 0);
    public Vector3 raise = new Vector3(0, 0, 5);
    public GameObject left, right, clearSearch, selectedBoardPanel, createLineupPanel;
    public Text TitleText, pageText;
    public InputField searchByInput;
    public Dropdown searchByGold, searchByOre, searchByHealth;
    public UserInfo user;

    private List<Collection> collections, searchedCollections;
    private Dictionary<string, List<Collection>> collectionDict = new Dictionary<string, List<Collection>>();
    private Dictionary<string, List<Collection>> originalDict = new Dictionary<string, List<Collection>>();
    private int coins,        
        pageNumber = 1, 
        searchByGoldValue = -1, 
        searchByOreValue = -1, 
        searchByHealthValue = -1;
    private static GameObject[] tabs = new GameObject[types.Length];
    private GameObject[] cards, counters;
    private Vector3 mousePosition;
    private string[] exitOneTypeMode = { "",""};
    private string oneTypeMode = "", searchByKeyword = "";

    // Use this for initialization
    void Start () {
        user = InfoLoader.user;
        coins = user.coins;
        cards = new GameObject[cardsPerPage];
        counters = new GameObject[cardsPerPage];
        foreach (string type in types)
        {
            pageLimits.Add(type, 1);
            collectionDict.Add(type, new List<Collection>());
            originalDict.Add(type, new List<Collection>());
        }
        for (int i = 0; i < cardsPerPage; i++)
        {
            Transform slot = GameObject.Find("Slot" + i.ToString()).transform;
            cards[i] = slot.Find("Card").gameObject;
            counters[i] = slot.Find("Count").gameObject;
        }
        for (int i = 0; i < types.Length; i++)
        {
            tabs[i] = GameObject.Find("Tabs/" + types[i]);
            tabs[i].SetActive(true);
        }
        GameObject.Find("CoinNumber").GetComponent<Text>().text = coins.ToString();
        LoadUserCollections();
        foreach (Collection collection in collections)
            originalDict[collection.type].Add(collection);
        SetPageLimits();
        ShowCurrentPage();
    }

    public void AddCollection(Collection collection)
    {
        bool found = false;
        foreach(Collection target in user.collections)
        {
            if (target.name == collection.name && target.health == collection.health)
            {
                target.count++;
                found = true;
                break;
            }
        }
        if (!found)
        {
            user.collections.Add(collection);
            collections.Add(collection);
            collectionDict[collection.type].Add(collection);
            SetPageLimits();
        }        
        ShowCurrentPage();
    }

    public void RemoveCollection(Collection collection)
    {
       // foreach (Collection c in user.collections) Debug.Log(c.name);
        Collection target = new Collection();
        foreach(Collection c in user.collections)
            if(c.name == collection.name && c.health == collection.health)
            {                
                target = c;
                target.count--;
                break;
            }
        if (target.count == 0)
        {
            user.collections.Remove(target);
            collections.Remove(target);
            collectionDict[target.type].Remove(target);
            SetPageLimits();
        }        
        ShowCurrentPage();
    }

    public void RemoveStandardCards()
    {
        // 还要补回来的
        List<Collection> noStandardCollections = new List<Collection>();
        foreach (Collection collection in user.collections)
            if (!collection.name.StartsWith("Standard "))
                noStandardCollections.Add(collection);
        user.collections = noStandardCollections;
        LoadUserCollections();
        SetPageLimits();
        ShowCurrentPage();
    }

    private void LoadUserCollections()
    {
        collections = user.collections;
        LoadCollections();
    }

    private void LoadCollections()
    {
        foreach (KeyValuePair<string, List<Collection>> pair in collectionDict)
            pair.Value.Clear();
        // Sort?
        foreach (Collection collection in collections)
            collectionDict[collection.type].Add(collection);
    }

    private void SetPageLimits(string type = "")
    {
        int count;
        foreach (KeyValuePair<string, List<Collection>> item in collectionDict)
        {
            count = item.Value.Count;
            if (oneTypeMode != "" && item.Key != oneTypeMode) count = 0;
            else if (0 < count && count <= 4) count = 1;
            else if (count != 0)
            {
                count = (int)Mathf.Floor(count / cardsPerPage);
                if (count % 4 != 0) count++;
            }
            pageLimits[item.Key] = count;
        }
    }

    public void SetCurrentPage(string type,int page)
    {
        if (page <= pageLimits[type])
            currentPage = new KeyValuePair<string, int>(type, page);
    }

    public void SetCardsPerPage(int number)
    {
        cardsPerPage = number;
        SetPageLimits();
        SetCurrentPage("General", 1);
        ShowCurrentPage();
    }

    private void ShowSearchedCollection()
    {
        collections = new List<Collection>(searchedCollections); 
        LoadCollections();
        SetPageLimits();
        for (int i = types.Length - 1; i >= 0; i--)
        {
            // only show tab with result
            if (collectionDict[types[i]].Count == 0) tabs[i].SetActive(false);
            else tabs[i].SetActive(true);
        }
        currentPage = FirstPage();
        ShowCurrentPage(); 
    }

    public KeyValuePair<string, int> FirstPage()
    {
        for (int i = 0; i < types.Length; i++)
            if (pageLimits[types[i]] != 0)
                return new KeyValuePair<string, int>(types[i], 1);
        return notFound;
    }

    public KeyValuePair<string, int> LastPage()
    {
        for (int i = types.Length - 1; i >= 0; i--)
            if (pageLimits[types[i]] != 0)
                return new KeyValuePair<string, int>(types[i], pageLimits[types[i]]);
        return notFound;
    }

    public void ShowCurrentPage()
    {
        // Disable things
        if (currentPage.Equals(FirstPage())) left.SetActive(false);
        else left.SetActive(true);
        if (currentPage.Equals(LastPage())) right.SetActive(false);
        else right.SetActive(true);
        for (int i = 0; i < cardsPerPage; i++)
        {
            if (!cards[i].activeSelf) break;
            cards[i].SetActive(false);
            counters[i].GetComponent<Text>().text = "";
        }
        pageText.text = "";

        string type = currentPage.Key;
        pageNumber = currentPage.Value;
        for (int i = 0; i < types.Length; i++)
        {
            if (types[i] != type) pageNumber += pageLimits[types[i]];
            else break;
        }
        pageText.text = "Page " + pageNumber.ToString();

        if (currentPage.Equals(notFound))
        {
            TitleText.text = "Not Found";
            return;
        }

        int page = currentPage.Value - 1;
        TitleText.text = type;
        RaiseButton(tabs[Array.IndexOf(types,type)]);
        SortCollections();        

        GameObject card;
        Collection collection;
        List<Collection> collectionWithType = collectionDict[type];        
        for (int i = 0; i < Mathf.Min(cardsPerPage, collectionWithType.Count - cardsPerPage * page); i++)
        {
            card = cards[i];
            collection = collectionWithType[page * cardsPerPage + i];
            card.GetComponent<CardInfo>().SetAttributes(collection);
            card.SetActive(true);
            if (collection.count == 1) counters[i].GetComponent<Text>().text = "";
            else if (collection.count > 99) counters[i].GetComponent<Text>().text = "×99+";
            else counters[i].GetComponent<Text>().text = "×" + collection.count.ToString();
        }
    }

    public void PreviousPage()
    {
        // Turn page animatioin    
        string type = currentPage.Key;
        int page = currentPage.Value;
        if (currentPage.Value == 1)
        {
            int index = Array.IndexOf(CollectionManager.types, type) - 1;
            while (true)
            {
                type = types[index];
                page = pageLimits[type];
                if (page != 0 || index == 0) break;
                index--;
            }           
        }
        else page--;
        SetCurrentPage(type, page);
        ShowCurrentPage();
    }

    public void NextPage()
    {
        // Turn page animatioin      
        string type = currentPage.Key;
        int page = currentPage.Value;
        if (currentPage.Value == pageLimits[type])
        {
            int index = Array.IndexOf(types, type) + 1;
            while (true)
            {
                type = types[index];
                page = 1;
                if (pageLimits[type] != 0 || index == types.Length - 1) break;
                index++;
            }
        }
        else page++;
        SetCurrentPage(type, page);
        ShowCurrentPage();
    }

    public void ClickTab(GameObject obj)
    {
        RaiseButton(obj);
        SetCurrentPage(obj.name, 1);
        ShowCurrentPage();
    }

    private void SortCollections()
    {

    }

    // Doesn't work for now
    private void RaiseButton(GameObject obj)
    {
        foreach (GameObject tab in GameObject.FindGameObjectsWithTag("Tab"))
        {
            if (tab.transform.position.z == 0)
            {
                if (tab == obj) return;
                tab.transform.position -= raise;
                break;
            }
        }
        obj.transform.position += raise;
    }

    private void OneTypeMode(string type)
    {
        if (type == "") return;
        oneTypeMode = type;
        foreach (GameObject tab in tabs)
        {
            if (tab.name == type) RaiseButton(tab);
            else tab.SetActive(false);
        }
        SetPageLimits();
        SetCurrentPage(type, 1);
        ShowCurrentPage();
    }

    public void EnterOneTypeMode(string type,string loc)
    {
        if (type == "") return;
        ExitOneTypeMode();
        // click on the same obj to cancel this mode
        string[] lastEnterOneTypeMode = new string[2] { type, loc };
        if (lastEnterOneTypeMode.SequenceEqual(exitOneTypeMode))
        {
            exitOneTypeMode =new string[] { "","" };
            return;
        }
        exitOneTypeMode = lastEnterOneTypeMode;
        OneTypeMode(type);
    }

    public string[] GetLastEnterOneTypeMode() { return exitOneTypeMode; }

    public void ExitOneTypeMode()
    {
        foreach (GameObject tab in tabs) tab.SetActive(true);
        LoadUserCollections();
        SetPageLimits();
        searchByOre.value = searchByHealth.value = searchByGold.value = 0;
        ShowCurrentPage();
    }

    public void Search(string word = "", int gold = -1, int ore = -1, int health = -1)
    {
        if (oneTypeMode != "") searchedCollections = originalDict[oneTypeMode];
        else searchedCollections = user.collections;
        List<Collection> newSearched = new List<Collection>();
        if (word != "" && searchedCollections!=null)
        {
            foreach (Collection collection in searchedCollections)
            {
                if (collection.name.Contains(word) ||
                    (collection.type == "Tactic" && Resources.Load<TacticAttributes>("Tactics/Info/" + collection.name + "/Attributes").description.Contains(word)) ||
                    (collection.type != "Tactic" && Resources.Load<PieceAttributes>("Pieces/Info/" + collection.name + "/Attributes").description.Contains(word)))
                    newSearched.Add(collection);
            }
            searchedCollections = newSearched;
        }
        if (gold != -1 && searchedCollections != null)
        {
            foreach (Collection collection in searchedCollections)
            {
                if (collection.type == "Tactic")
                {
                    int goldCost = Resources.Load<TacticAttributes>("Tactics/Info/" + collection.name + "/Attributes").goldCost;
                    if ((gold == 5 && goldCost >= gold) ||
                        (gold < 5 && goldCost == gold))
                        newSearched.Add(collection);
                }
                // search description
            }
            searchedCollections = newSearched;
        }
        if (ore != -1 && searchedCollections != null)
        {
            foreach (Collection collection in searchedCollections)
            {
                int oreCost;
                if (collection.type == "Tactic") oreCost = Resources.Load<TacticAttributes>("Tactics/Info/" + collection.name + "/Attributes").oreCost;
                else oreCost = Resources.Load<PieceAttributes>("Pieces/Info/" + collection.name + "/Attributes").oreCost;
                if ((ore == 5 && oreCost >= ore) ||
                    (ore < 5 && oreCost == ore))
                    newSearched.Add(collection);
            }
            searchedCollections = newSearched;
        }
        if (health != -1 && searchedCollections != null)
        {
            foreach (Collection collection in searchedCollections)
            {
                if (collection.type != "Tactic")
                {
                    // ∞不知道该不该算5+
                    int Health = Resources.Load<PieceAttributes>("Pieces/Info/" + collection.name + "/Attributes").health;
                    if ((health == 0 && Health == health) ||
                        (health == 5 && Health >= health) ||
                        (health < 5 && Health == health))
                        newSearched.Add(collection);
                }
            }
            searchedCollections = newSearched;
        }
        ShowSearchedCollection();
    }

    public void InputFieldSearch()
    {
        searchByKeyword = searchByInput.text.Trim();
        if (searchByKeyword != "")
        {
            clearSearch.SetActive(true);
            Search(searchByKeyword);
        }
    }

    public void ClearSearch()
    {
        searchByInput.text = "";
        clearSearch.SetActive(false);
        Search(searchByKeyword, searchByGoldValue, searchByOreValue, searchByHealthValue);
    }

    public void SetSearchByGold()
    {
        if (searchByGold.value == 0) searchByGoldValue = -1;
        else searchByGoldValue = searchByGold.value - 1;
        Search(searchByKeyword, searchByGoldValue, searchByOreValue, searchByHealthValue);
    }

    public void SetSearchByOre()
    {
        if (searchByOre.value == 0) searchByOreValue = -1;
        else searchByOreValue = searchByOre.value - 1;
        Search(searchByKeyword, searchByGoldValue, searchByOreValue, searchByHealthValue);
    }

    public void SetSearchByHealth()
    {
        if (searchByHealth.value == 0) searchByHealthValue = -1;
        else if (searchByHealth.value == 6) searchByHealthValue = 0;
        else searchByHealthValue = searchByHealth.value;
        Search(searchByKeyword, searchByGoldValue, searchByOreValue, searchByHealthValue);
    }


}
