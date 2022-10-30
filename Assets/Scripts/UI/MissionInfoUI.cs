using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionInfoUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI GYName;
    [SerializeField] private TMPro.TextMeshProUGUI GYGuards;
    [SerializeField] private TMPro.TextMeshProUGUI GYTier;

    public void SetText(string Name, int Guards, string Tier)
    {
        GYName.text = "Name: " + Name;
        GYGuards.text = "Number of Guards: " + Guards;
        GYTier.text = "Quality of Graves: " + Tier;
    }
}
