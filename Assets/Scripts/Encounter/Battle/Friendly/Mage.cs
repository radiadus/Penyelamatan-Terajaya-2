using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Friendly
{
    public override void OnKill()
    {
        MP += (int)(0.1f * maxMP);
        CheckMaxHPMP();
    }
}
