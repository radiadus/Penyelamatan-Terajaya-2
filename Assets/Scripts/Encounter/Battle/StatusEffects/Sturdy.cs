using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sturdy : StatusEffect
{
    public Sturdy()
    {
        statusPrefab = Resources.Load<GameObject>(path + "Sturdy");
    }
}
