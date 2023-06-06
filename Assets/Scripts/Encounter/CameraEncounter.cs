using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEncounter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-4, 5, -5);
        transform.rotation = Quaternion.Euler(30, 30, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
