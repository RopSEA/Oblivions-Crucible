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
            Resources.Load<Sprite>("DemonShield"), 0, 0, 0, 0, 5, 50));

        availableItems.Add(new ShopItem("Mythril Sword", "A powerful blade \n +5 Str", 
            Resources.Load<Sprite>("MythrilSword"), 5, 0, 0, 0, 0, 50));

        availableItems.Add(new ShopItem("Vitality Potion", "Increases health \n+5 Vit", 
            Resources.Load<Sprite>("VitalityPotion"), 0, 5, 0, 0, 0, 30));
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
