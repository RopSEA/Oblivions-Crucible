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
        Debug.Log($" Updating Shop UI | Items Count: {displayedItems.Length}");

        for (int i = 0; i < displayedItems.Length; i++) // This should be 0-2
        {
            if (i >= itemImages.Length || i >= buyButtons.Length)
            {
                Debug.LogError($" ERROR: UI element index {i} is out of bounds!");
                continue; // Prevents further execution for this index
            }

            itemImages[i].sprite = displayedItems[i].itemImage;
            itemNames[i].text = displayedItems[i].itemName;
            itemDescriptions[i].text = displayedItems[i].description;
            itemPrices[i].text = displayedItems[i].cost.ToString();

            buyButtons[i].onClick.RemoveAllListeners(); // Ensure no duplicate listeners

            int buttonIndex = i; // Capture the correct index
            buyButtons[i].onClick.AddListener(() =>
            {
                Debug.Log($" Button {buttonIndex} clicked, calling BuyItem({buttonIndex})");
                ShopManager.instance.BuyItem(buttonIndex);
            });
        }
    }


    public void ShowShop()
    {
        Debug.Log("ShowShop() called!");
        shopCanvasGroup.alpha = 1;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;
        Shop = true;
        Time.timeScale = 0f;
    }

    public void HideShop()
    {
        Debug.Log(" HideShop() called!");
        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;
        Shop = false;
        
        Time.timeScale = 1f;
        //Notify TutorialManager that shop was closed
        OnShopClosed.Invoke();
    }
}
