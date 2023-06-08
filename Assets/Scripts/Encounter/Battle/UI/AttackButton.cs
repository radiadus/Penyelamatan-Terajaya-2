using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public Button attackButton;
    public Skill skill;
    public Text skillName;
    public TextMeshProUGUI skillCost;

    public void InitializeText()
    {
        skillName.text = skill.skillName;
        skillCost.text = skill.mpCost + " MP";
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
