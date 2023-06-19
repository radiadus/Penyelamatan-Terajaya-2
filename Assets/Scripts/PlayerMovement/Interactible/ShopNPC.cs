using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactible
{
    public int[] itemIds;
    public GameObject shopCanvas;
    public GameObject inGameCanvas;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.mage.FullHeal();
        GameManager.Instance.assassin.FullHeal();
        GameManager.Instance.warrior.FullHeal();
        StartCoroutine(Shopping());
    }

    IEnumerator Shopping()
    {
        while (GameManager.Instance.gameState == GameManager.State.INTERACT)
        {
            yield return null;
        }
        shopCanvas.SetActive(true);
        GameManager.Instance.gameState = GameManager.State.SHOPPING;
        inGameCanvas.SetActive(false);
        while (shopCanvas.activeSelf)
        {
            yield return null;
        }
        GameManager.Instance.gameState = GameManager.State.DEFAULT;
        inGameCanvas.SetActive(true);
    }
}
