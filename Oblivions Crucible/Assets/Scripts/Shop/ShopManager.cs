using UnityEngine;
using System.Collections.Generic;
using System;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Shop Controls")]
    public int shopRound = 5;


    [Header("Player Stats & Currency")]



    private List<ShopItem> availableItems = new List<ShopItem>();
    private ShopItem[] displayedItems = new ShopItem[3];

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadShopItems();
        SelectRandomItems();
        ShopDisplay.instance.UpdateShopUI(displayedItems);
    }

    public void clearDisplayed()
    {
        Array.Clear(displayedItems, 0, displayedItems.Length);
        SelectRandomItems();
    }

    public ShopItem[] getDisplay()
    {
        return displayedItems;
    }

    void LoadShopItems()
    {
        availableItems.Add(new ShopItem("Demon Shield", "A strong steel shield \n +5 Def", 
            Resources.Load<Sprite>("DemonShield"), Resources.Load<Sprite>("SoldDemonShield"), 0, 0, 0, 0, 5, 50));

        availableItems.Add(new ShopItem("Mythril Sword", "A powerful blade \n+5 Str \n+5 int", 
            Resources.Load<Sprite>("MythrilSword"), Resources.Load<Sprite>("SoldMythrilSword"), 5, 0, 0, 5, 0, 50));

        availableItems.Add(new ShopItem("Vitality Potion", "Increases health \n+5 Vit", 
            Resources.Load<Sprite>("VitalityPotion"), Resources.Load<Sprite>("SoldVitalityPotion"), 0, 5, 0, 0, 0, 30));
       
        availableItems.Add(new ShopItem("Mega Potion", "Increases health Intell and Strength \n+5 Vit \n+5 Str \n+5 Int",
            Resources.Load<Sprite>("MegaPotion"), Resources.Load<Sprite>("SoldMegaPotion"), 5, 5, 0, 5, 0, 80));

        availableItems.Add(new ShopItem("Stamina Potion", "Increases health \n+5 Sta",
            Resources.Load<Sprite>("StamPotion"), Resources.Load<Sprite>("SoldStamPotion"), 0, 0, 0, 0, 5, 30));

        availableItems.Add(new ShopItem("Demon Sword", "Powerful sword \n+10 Str \n+10 Int",
            Resources.Load<Sprite>("DemSword"), Resources.Load<Sprite>("SoldDemSword"), 0, 10, 0, 10, 0, 100));

        availableItems.Add(new ShopItem("Sheild Of Light", "Defends you \n+10 Def",
            Resources.Load<Sprite>("MythrilShield"), Resources.Load<Sprite>("SoldMythrilShield"), 0, 0, 0, 0, 10, 85));
    }

    void SelectRandomItems()
    {
        List<ShopItem> tempList = new List<ShopItem>(availableItems);
        for (int i = 0; i < 3; i++)
        {   
            if (tempList.Count == 0) break;
            int randomIndex = UnityEngine.Random.Range(0, tempList.Count);
            displayedItems[i] = tempList[randomIndex];
            tempList.RemoveAt(randomIndex);
        }
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= displayedItems.Length)
        {
            Debug.LogError($" ERROR: Invalid item index {index} in BuyItem().");
            return;
        }

        ShopItem selectedItem = displayedItems[index];
        Debug.Log($" Attempting to buy {selectedItem.itemName} for {selectedItem.cost} coins");

        if (coinManager.instance.SpendCoins(selectedItem.cost))
        {
            //  Fetch PlayerStats from MenuUIManager
            if (MenuUIManager.instance != null && MenuUIManager.instance.playerStats != null)
            {
                PlayerStats playerStats = MenuUIManager.instance.playerStats;
                HealthSystem playerHp = GameObject.FindWithTag("Player").GetComponent<HealthSystem>();
                Classes playerStat = GameObject.FindWithTag("Player").GetComponent<Classes>();

                // Update Player Stats
                playerStats.Strength += selectedItem.strengthBoost;
                playerStats.Vitality += selectedItem.vitalityBoost;
                playerStats.Stamina += selectedItem.staminaBoost;
                playerStats.Intelligence += selectedItem.intelligenceBoost;
                playerStats.Defense += selectedItem.defenseBoost;

                playerHp.addHealth(selectedItem.vitalityBoost);
                playerStat.addVit(selectedItem.vitalityBoost);
                playerStat.addStren(selectedItem.strengthBoost);
                playerStat.addStam(selectedItem.staminaBoost);
                playerStat.addIntell(selectedItem.intelligenceBoost);
                playerStat.addDef(selectedItem.defenseBoost);

                playerStats.OwnedUpgrades.Add(selectedItem.itemName);

                Debug.Log($" Purchased {selectedItem.itemName}. Remaining Coins: {coinManager.instance.GetCoinCount()}");
                // Update Item Description
                selectedItem.isSold = true;
                selectedItem.itemImage = selectedItem.soldImageOverride ?? selectedItem.itemImage;
                Debug.Log("Assigned Sprite: " + selectedItem.itemImage.name);

                //  Update the Shop UI
                ShopDisplay.instance.UpdateShopUI(displayedItems);

             

                // Ensure Menu UI updates with new stats
                MenuUIManager.instance.UpdateMenuStats();
                Debug.Log(" Menu UI updated after purchase.");
            }
            else
            {
                Debug.LogError(" ERROR: MenuUIManager instance or PlayerStats not found!");
            }
        }
        else
        {
            Debug.Log(" Not enough coins!");
        }
    }
}
