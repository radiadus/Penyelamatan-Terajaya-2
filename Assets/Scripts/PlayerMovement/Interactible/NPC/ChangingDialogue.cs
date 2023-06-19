using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingDialogue : NPC
{
    protected override int CheckUsedText()
    {
        return PlayerPrefs.GetInt("batuPusaka", 0) == 1 ? 2 : 1;
    }
}
