using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : Friendly
{
    public override void InitializeStats()
    {
        base.InitializeStats();
        evasion = 20;
    }
}
