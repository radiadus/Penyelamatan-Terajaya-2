using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceItem : MonoBehaviour
{
    public Text itemName;
    public TextMeshProUGUI itemPrice;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private GameObject enhanceAlertPanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Equipment equipment;
    private RectTransform canvasRtf;

    // Start is called before the first frame update
    void Start()
    {
        canvasRtf = canvas.GetComponent<RectTransform>();
        itemName.text = equipment.equipmentName + " +" + equipment.enhanceLevel;
        itemPrice.text = "Rp " + (equipment.enhanceLevel == 5 ? "-" : equipment.enhancePrice.ToString());
        enhanceButton.onClick.AddListener(delegate { OpenEnhancePanel(); });
    }

    void OpenEnhancePanel()
    {
        if (equipment.enhanceLevel == 5) return;
        enhanceAlertPanel.GetComponent<EnhanceAlertHandler>().Instantiate(equipment, this, canvas.GetComponent<ShopHandler>());
        GameObject.Instantiate(enhanceAlertPanel, canvas.transform.position, Quaternion.identity, canvas.transform);
    }

    public void Reinitialize()
    {
        itemName.text = equipment.equipmentName + " +" + equipment.enhanceLevel;
        itemPrice.text = "Rp " + (equipment.enhanceLevel == 5 ? "-" : equipment.enhancePrice.ToString());
    }
}
