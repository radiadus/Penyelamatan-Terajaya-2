using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBank : ScriptableObject
{
    public Dictionary<QuestionCategory, List<Question>> questions;
}
