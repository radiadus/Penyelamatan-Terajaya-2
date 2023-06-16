using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueBank : ScriptableObject
{
    public Dictionary<string, Dictionary<int, List<string>>> dialogues;
}
