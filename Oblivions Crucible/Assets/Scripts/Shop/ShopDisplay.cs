using UnityEngine;
using UnityEngine.UI;
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
        Debug.Log("Updating Shop UI");

        for (int i = 0; i < displayedItems.Length; i++)
        {
            itemImages[i].sprite = displayedItems[i].itemImage;
            itemNames[i].text = displayedItems[i].itemName;
            itemDescriptions[i].text = displayedItems[i].description;
            itemPrices[i].text = displayedItems[i].cost.ToString();
            buyButtons[i].onClick.RemoveAllListeners();
            buyButtons[i].onClick.AddListener(() => ShopManager.instance.BuyItem(i));
        }
    }

    public void ShowShop()
    {
        Debug.Log("✅ ShowShop() called!");
        shopCanvasGroup.alpha = 1;
        shopCanvasGroup.interactable = true;
        shopCanvasGroup.blocksRaycasts = true;
        Time.timeScale = 0f;
    }

    public void HideShop()
    {
        Debug.Log("❌ HideShop() called!");
        shopCanvasGroup.alpha = 0;
        shopCanvasGroup.interactable = false;
        shopCanvasGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
    }
}
