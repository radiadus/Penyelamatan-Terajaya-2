using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour
{
    private List<int> itemIds = new List<int>();
    [SerializeField] private Button shopButton, upgradeButton, exit;
    [SerializeField] private GameObject shop, upgrade, canvas;
    [SerializeField] private GameObject shopItem;
    private RectTransform shopBGRtf;
    

    // Start is called before the first frame update
    void Start()
    {
        shopButton.onClick.AddListener(delegate { OpenShop(); });
        upgradeButton.onClick.AddListener(delegate { OpenUpgrade(); });
        exit.onClick.AddListener(delegate { Exit(); });
        //perlu diapus
        itemIds.Add(1);
        itemIds.Add(2);
        shopBGRtf = shop.GetComponent<RectTransform>();
        
        int x = 150;
        int y = -150;
        //sampe sini
        foreach (int id in itemIds)
        {
            CreateShopItem(id, new Vector2(x, y));
            y -= 150;
            //blm kelar
        }
    }

    private void CreateShopItem(int itemId, Vector2 position)
    {
        Debug.Log("" + itemId + " " + position.x + " " + position.y);
        RectTransform shopItemRtf = shopItem.GetComponent<RectTransform>();
        shopItemRtf.position = new Vector2(0, 0);
        shopItemRtf.position = position;
        shopItemRtf.anchorMax = shopBGRtf.anchorMax;
        shopItemRtf.anchorMin = shopBGRtf.anchorMin;
        shopItemRtf.pivot = shopBGRtf.pivot;

        Item item = Resources.Load<Item>("Items/Item_" + itemId);
        shopItem.GetComponent<ShopItem>().Instantiate(item);
        GameObject.Instantiate(shopItem, shop.transform);

    }

    private void OpenShop()
    {
        upgrade.SetActive(false);
        shop.SetActive(true);
    }

    private void OpenUpgrade()
    {
        shop.SetActive(false);
        upgrade.SetActive(true);
    }
    
    private void Exit()
    {
        shop.SetActive(false);
        upgrade.SetActive(false);
        canvas.SetActive(false);
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
    }

    
}
