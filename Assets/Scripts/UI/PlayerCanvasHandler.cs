using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCanvasHandler : MonoBehaviour
{
    private GameObject npc;
    private GameObject player;
    private GameObject inGameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        inGameCanvas = GameObject.Find("InGameCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNPC(GameObject npc)
    {
        this.npc = npc;
    }

    public void Interact()
    {
        Debug.Log(npc.name);
        Vector3 direction = npc.transform.position - player.transform.position;
        direction.y = 0;
        player.transform.rotation = Quaternion.LookRotation(direction);
        npc.transform.rotation = Quaternion.LookRotation(-direction);        
    }

}
