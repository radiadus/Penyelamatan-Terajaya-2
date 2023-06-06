using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
   [SerializeField] private Vector3 cameraPosition;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = new Vector3(0, 4.5f, -4);
        transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition + player.position;
    }
}
