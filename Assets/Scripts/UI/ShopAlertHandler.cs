using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopAlertHandler : MonoBehaviour
{
    public int amount, ownedAmount;
    public ShopHandler shopHandler;
    [SerializeField] private Item item;
    [SerializeField] private Text amountText, alertText;
    [SerializeField] private Button increment, decrement, buy, cancel;
    [SerializeField] private GameObject self;
    [SerializeField] private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
        increment.onClick.AddListener(delegate { Increment(); });
        decrement.onClick.AddListener(delegate { Decrement(); });
        buy.onClick.AddListener(delegate { Buy(); });
        cancel.onClick.AddListener(delegate { Cancel(); });
    }

    public void Instantiate(Item item, int ownedAmount, ShopHandler shopHandler)
    {
        this.item = item;
        this.amount = 1;
        this.ownedAmount = ownedAmount;
        this.shopHandler = shopHandler;
        this.amountText.text = this.amount.ToString();
        this.alertText.text = "Jumlah Pembelian \"" + this.item.itemName + "\"";
    }

    private void Increment()
    {
        Debug.Log(this.amount + " + " + this.ownedAmount + " = " + (this.amount + this.ownedAmount));
        if (this.amount < 99 - this.ownedAmount && this.item.buyPrice * (this.amount+1) <= inventory.money)
            this.amount++;
        SetAmountText();
    }

    private void Decrement()
    {
        if (this.amount > 1)
            this.amount--;
        SetAmountText();
    }

    private void SetAmountText()
    {
        this.amountText.text = this.amount.ToString();
    }

    private void Buy()
    {
        Debug.Log(this.item);
        int price = this.item.buyPrice * this.amount;
        if (inventory.money >= price)
        {
            inventory.money -= price;
            inventory.addItem(this.item, this.amount);
            shopHandler.UpdateMoney();
            Destroy(self);
        }
    }

    private void Cancel()
    {
        Destroy(self);
    }
}
