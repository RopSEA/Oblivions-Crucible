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

    public ShopItem(string name, string desc, Sprite image, int str, int vit, int sta, int intel, int def, int price)
    {
        itemName = name;
        description = desc;
        itemImage = image;
        strengthBoost = str;
        vitalityBoost = vit;
        staminaBoost = sta;
        intelligenceBoost = intel;
        defenseBoost = def;
        cost = price;
    }
}
