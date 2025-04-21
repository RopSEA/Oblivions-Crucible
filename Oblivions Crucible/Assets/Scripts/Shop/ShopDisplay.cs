using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ShopDisplay : MonoBehaviour
{
    public static ShopDisplay instance;

    [Header("Shop UI Elements")]
    public CanvasGroup shopCanvasGroup;
    public Image[] itemImages;
    public TextMeshProUGUI[] itemNames;
    public TextMeshProUGUI[] itemDescriptions;
    public TextMeshProUGUI[] itemPrices;
    public Button[] buyButtons;
    public Button exitButton;
    public bool debugShowShop = false;
    public bool Shop = false;
    public GameObject cursor;
    //For tutorial Access
    public UnityEvent OnShopClosed = new UnityEvent();
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {   
        if (debugShowShop == true)
            ShowShop();
        else
            HideShop();
        if (exitButton != null)
            exitButton.onClick.AddListener(HideShop);
    }

    public void UpdateShopUI(ShopItem[] displayedItems)
    {
        Debug.Log($"Updating Shop UI | Items Count: {displayedItems.Length}");

        for (int i = 0; i < displayedItems.Length; i++)
        {
            if (i >= itemImages.Length || i >= buyButtons.Length)
            {
                Debug.LogError($"ERROR: UI element index {i} is out of bounds!");
                continue;
            }

            if (displayedItems[i] == null)
            {
                continue;
            }
            ShopItem item = displayedItems[i];

            // Set shared UI text
            itemNames[i].text = item.itemName;
            itemDescriptions[i].text = item.description;

            if (item.isSold)
            {
                itemDescriptions[i].text = item.description;

                Sprite tempImage;

                //Force refresh by clearing first
                itemImages[i].sprite = item.soldImageOverride;

                Debug.Log($"Sprite visually reassigned: {item.soldImageOverride.name}");

                itemPrices[i].text = "";
                buyButtons[i].interactable = false;
            }

            else
            {
      
      
                itemImages[i].sprite = item.itemImage;
                itemPrices[i].text = item.cost.ToString();
                buyButtons[i].interactable = true;

                buyButtons[i].onClick.RemoveAllListeners();
                int buttonIndex = i; 
                buyButtons[i].onClick.AddListener(() =>
                {
                    Debug.Log($"Button {buttonIndex} clicked, calling BuyItem({buttonIndex})");
                    ShopManager.instance.BuyItem(buttonIndex);
                });
            }
        }
    }


    


    public void ShowShop()
    {
        Debug.Log("ShowShop() called!");
        cursor.SetActive(true);
        shopCanvasGroup.alpha = 1;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;
        Shop = true;
        Time.timeScale = 0f;
    }

    public void HideShop()
    {
        Debug.Log(" HideShop() called!");
        cursor.SetActive(false);


        ShopItem[] displayedItems = ShopManager.instance.getDisplay();

        for (int i = 0; i < displayedItems.Length; i++)
        {
            if (i >= itemImages.Length || i >= buyButtons.Length)
            {
                Debug.LogError($"ERROR: UI element index {i} is out of bounds!");
                continue;
            }
            if (displayedItems[i] == null)
            {
                continue;
            }

            ShopItem item = displayedItems[i];
            item.isSold = false;
            item.itemImage = item.itemImageOverride;
        }

        ShopManager.instance.clearDisplayed();

        UpdateShopUI(displayedItems);

        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;
        Shop = false;

        Time.timeScale = 1f;
        //Notify TutorialManager that shop was closed
        OnShopClosed.Invoke();
    }
}
