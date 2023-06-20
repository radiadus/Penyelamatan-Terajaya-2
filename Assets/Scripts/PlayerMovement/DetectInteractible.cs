using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectInteractible : MonoBehaviour
{
    private GameObject[] interactibleList;
    private GameObject closestInteractible;
    private float closestDist;
    public Button interactButton;
    public GameObject playerCanvas;
    // Start is called before the first frame update
    void Start()
    {
        interactibleList = GameObject.FindGameObjectsWithTag("Interactible");
        interactButton.onClick.AddListener(delegate { Interact(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.State.DEFAULT)
        {
            interactButton.gameObject.SetActive(false);
            return;
        }
        closestDist = 9999;
        foreach (GameObject interactible in interactibleList)
        {
            float dist = Vector3.Distance(transform.position, interactible.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestInteractible = interactible;
            }
        }
        if (closestDist < 1.5f)
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(false);
            }
        }

    }

    void Interact()
    {
        this.playerCanvas.SetActive(false);
        GameManager.Instance.gameState = GameManager.State.INTERACT;
        this.closestInteractible.GetComponent<Interactible>().Interact();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (GameManager.Instance.gameState != GameManager.State.DEFAULT)
        {
            yield return null;
        }
        this.playerCanvas.SetActive(true);
    }
}
