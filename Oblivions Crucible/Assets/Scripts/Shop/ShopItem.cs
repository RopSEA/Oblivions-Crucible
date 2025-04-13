using UnityEngine;

[System.Serializable]
public class ShopItem
{
    public string itemName;
    public string description;
    public Sprite itemImage;

    public int strengthBoost;
    public int vitalityBoost;
    public int staminaBoost;
    public int intelligenceBoost;
    public int defenseBoost;
    public int cost;

    public bool isSold = false;
    public Sprite soldImageOverride;

    public ShopItem(string name, string desc, Sprite img, int str, int vit, int sta, int intel, int def, int price)
    {
        itemName = name;
        description = desc;
        itemImage = img;

        strengthBoost = str;
        vitalityBoost = vit;
        staminaBoost = sta;
        intelligenceBoost = intel;
        defenseBoost = def;
        cost = price;

        // Load Sold + ItemName.png 
        Debug.Log($"Loading sold sprite for {name}: Sold{name.Replace(" ", "")}");
        soldImageOverride = Resources.Load<Sprite>($"Sold{name.Replace(" ", "")}");
        

    }
}
