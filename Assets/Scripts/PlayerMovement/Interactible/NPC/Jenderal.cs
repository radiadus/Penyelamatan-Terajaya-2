using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jenderal : NPC
{

    protected override int CheckUsedText()
    {
        return PlayerPrefs.GetInt("jenderal", 0) == 1 ? 2 : 1;
    }

    public override void Interact()
    {
        base.Interact();
        Debug.Log("jenderal: " + PlayerPrefs.GetInt("jenderal", 0));
        PlayerPrefs.SetInt("jenderal", 1);
    }
}
