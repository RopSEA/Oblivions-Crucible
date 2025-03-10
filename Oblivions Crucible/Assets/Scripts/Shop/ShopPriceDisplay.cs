using UnityEngine;
using TMPro;

public class ShopPriceDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemPriceText; 
    private int itemPrice = 0; 

    public void SetPrice(int price)
    {
        itemPrice = price;
        itemPriceText.text =  itemPrice.ToString(); 
    }
}
