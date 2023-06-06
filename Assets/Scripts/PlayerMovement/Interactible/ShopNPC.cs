using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactible
{
    public int[] itemIds;
    public GameObject shopCanvas;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact()
    {
        base.Interact();
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
        while (shopCanvas.activeSelf)
        {
            yield return null;
        }
    }
}
