using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceAlertHandler : MonoBehaviour
{
    [SerializeField] private Equipment equipment;
    [SerializeField] private Text alertText;
    [SerializeField] private Button yes, no;
    [SerializeField] private GameObject self;
    [SerializeField] private Inventory inventory;
    public int price;
    public EnhanceItem enhanceButton;
    public ShopHandler shopHandler;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);

        yes.onClick.AddListener(delegate { Enhance(); });
        no.onClick.AddListener(delegate { Cancel(); });
    }

    public void Instantiate(Equipment equipment, EnhanceItem enhanceButton, ShopHandler shopHandler)
    {
        this.equipment = equipment;
        this.price = equipment.enhancePrice;
        this.enhanceButton = enhanceButton;
        this.shopHandler = shopHandler;
        this.alertText.text = "Ingin melakukan peningkatan pada \"" + equipment.equipmentName + "\"?";
    }

    void Enhance()
    {
        if (inventory.money >= price)
        {
            inventory.money -= price;
            equipment.enhanceLevel++;
            equipment.attackStat += 2;
            equipment.enhancePrice += 1000;
            enhanceButton.Reinitialize();
            shopHandler.UpdateMoney();
            Destroy(self);
        }
    }

    void Cancel()
    {
        Destroy(self);
    }
}
