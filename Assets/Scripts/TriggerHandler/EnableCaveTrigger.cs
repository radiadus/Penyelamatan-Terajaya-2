using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableCaveTrigger : MonoBehaviour
{
    public GameObject[] roofObjects;

    // Start is called before the first frame update
    void Start()
    {
        roofObjects = GameObject.FindGameObjectsWithTag("Roof");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            foreach (GameObject roof in roofObjects)
            {
                roof.SetActive(true);
            }
        }
    }
}
