using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorativeRaven : MonoBehaviour
{
    public int Chance = 20;

    // Start is called before the first frame update
    void Start()
    {
        int RandInt = Random.Range(0, Chance);
        if (RandInt != 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
