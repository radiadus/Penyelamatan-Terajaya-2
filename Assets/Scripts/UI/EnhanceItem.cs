using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceItem : MonoBehaviour
{
    [SerializeField] private Text itemName;
    [SerializeField] private Button enhanceButton;
    [SerializeField] private GameObject enhanceAlertPanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Equipment equipment;
    private RectTransform canvasRtf;

    // Start is called before the first frame update
    void Start()
    {
        canvasRtf = canvas.GetComponent<RectTransform>();
        itemName.text = equipment.equipmentName.ToUpper();
        enhanceButton.onClick.AddListener(delegate { OpenEnhancePanel(); });
    }

    void OpenEnhancePanel()
    {
        enhanceAlertPanel.GetComponent<EnhanceAlertHandler>().Instantiate(equipment);
        GameObject.Instantiate(enhanceAlertPanel, canvas.transform.position, Quaternion.identity, canvas.transform);
    }
}
