using UnityEngine;
using UnityEngine.UI;

public class CardInfo : MonoBehaviour {

    public PieceAttributes piece;
    public TacticAttributes tactic;

    public Text nameText, descriptionText, costText, healthText, typeText;
    public Image image;
    public GameObject HealthImage, Background;
    public Sprite heartImage, coinImage;

    private string cardName, type, description;
    private int health = 1;

    public void SetAttributes(CardInfo cardInfo)
    {
        if (cardInfo == null) return;
        if (cardInfo.tactic != null) SetAttributes(cardInfo.tactic);
        else if (cardInfo.piece != null)
        {
            SetAttributes(cardInfo.piece);
            health = cardInfo.GetHealth();
            if (health == 0) healthText.text = "∞";
            else healthText.text = health.ToString();
            healthText.color = cardInfo.healthText.color;
        }
    }

    public void SetAttributes(Collection collection)
    {
        if (collection == null) return;
        if(collection.type == "Tactic")
            SetAttributes(Resources.Load<TacticAttributes>("Tactics/Info/" + collection.name + "/Attributes"));
        else
        {
            SetAttributes(Resources.Load<PieceAttributes>("Pieces/Info/" + collection.name + "/Attributes"));
            if (collection.health != 0 && health != collection.health)
            {
                if (health > collection.health) healthText.color = Color.red;
                else healthText.color = Color.green;
                health = collection.health;
                healthText.text = collection.health.ToString();
            }            
        }
    }

    // Need to highlight keywords

    public void SetAttributes(PieceAttributes attributes)
    {
        if (attributes == null) return;
        tactic = null;
        piece = attributes;
        nameText.text = attributes.Name;
        cardName = attributes.Name;
        description = attributes.description;
        descriptionText.text = attributes.description;
        costText.text = attributes.oreCost.ToString();
        health = attributes.health;
        healthText.color = Color.white;
        if (health == 0) healthText.text = "∞";
        else healthText.text = attributes.health.ToString();
        healthText.GetComponent<Outline>().enabled = true;
        image.sprite = attributes.image;
        type = attributes.type;
        typeText.text = type;
        HealthImage.GetComponent<Image>().sprite = heartImage;
    }

    public void SetAttributes(TacticAttributes attributes)
    {
        if (attributes == null) return;
        piece = null;
        tactic = attributes;
        nameText.text = attributes.Name;
        cardName = attributes.Name;
        description = attributes.description;
        descriptionText.text = attributes.description;
        costText.text = attributes.oreCost.ToString();
        health = attributes.goldCost;
        healthText.text = attributes.goldCost.ToString();
        healthText.color = Color.black;
        healthText.GetComponent<Outline>().enabled = false;
        image.sprite = attributes.image;
        type = "Tactic";
        typeText.text = type;
        HealthImage.GetComponent<Image>().sprite = coinImage;
    }

    public void Clear()
    {
        piece = null;
        tactic = null;
        nameText.text = "Name";
        descriptionText.text = "Description";
        costText.text = "0";
        healthText.text = "0";
        healthText.color = Color.white;
        image.sprite = null;
        type = "";
        HealthImage.GetComponent<Image>().sprite = heartImage;
    }

    public string GetCardName() { return cardName; }
    public string GetCardType() { return type; }
    public int GetHealth() { return health; }
    public string GetDescription() { return description; }
    public bool IsStandard() { return cardName.StartsWith("Standard "); }
}
