using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenFade : MonoBehaviour
{
    private SpriteRenderer BodySprite;

    private bool StartFade = false;

    // Start is called before the first frame update
    void Start()
    {
        BodySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (StartFade)
        {
            BodySprite.color = new Color(1f, 1f, 1f, Mathf.Max(BodySprite.color.a-0.08f, 0));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartFade = true;
        }
    }
}
