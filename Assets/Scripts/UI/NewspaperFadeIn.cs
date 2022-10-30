using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewspaperFadeIn : MonoBehaviour
{
    private bool ActiveFade = false;
    private Image Paper;

    // Start is called before the first frame update
    void Awake()
    {
        Paper = GetComponent<Image>();
        Paper.color = new Color(1, 1, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveFade)
        {
            Paper.color = new Color(Paper.color.r, Paper.color.g, Paper.color.b, Paper.color.a + 0.008f);
        }
    }

    public void SetActiveFade(bool _Val)
    {
        ActiveFade = _Val;
    }
}
