using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private Button itemButton;
    [SerializeField] private Text itemName;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GameObject amountPanel;
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
    }

    void OpenAmountPanel()
    {
        ownedAmount = 0;
        ItemInstance instance = inventory.items.Find(item => item.id == this.item.id);
        if (instance != null)
            ownedAmount = instance.quantity;
        Debug.Log(item.ToString());
        amountPanel.GetComponent<ShopAlertHandler>().Instantiate(item, ownedAmount);
        GameObject.Instantiate(amountPanel, canvas.transform);
    }

}
