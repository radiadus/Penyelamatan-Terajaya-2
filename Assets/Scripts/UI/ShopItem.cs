using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Item item;
    public Button itemButton;
    public Text itemName;
    public TextMeshProUGUI itemPrice;
    public Inventory inventory;
    public GameObject amountPanel;
    private GameObject canvas;
    private int amount, ownedAmount;

    // Start is called before the first frame update
    void Start()
    {
        itemButton.onClick.AddListener(delegate { OpenAmountPanel(); });
        Debug.Log(item);
        canvas = GameObject.Find("ShopCanvas");
    }

    public void Instantiate(Item item)
    {
        this.item = item;
        itemName.text = item.itemName.ToUpper();
        itemPrice.text = "Rp " + item.buyPrice.ToString();
    }

    void OpenAmountPanel()
    {
        ownedAmount = 0;
        ItemInstance instance = inventory.FindItemInstance(item.GetType());
        if (instance != null)
            ownedAmount = instance.quantity;
        if (ownedAmount == 99) return;
        Debug.Log(ownedAmount);
        amountPanel.GetComponent<ShopAlertHandler>().Instantiate(item, ownedAmount);
        GameObject.Instantiate(amountPanel, canvas.transform);
    }

}
