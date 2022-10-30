using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mausoleum : MonoBehaviour
{
    public GameObject InteractParticles; // Particle system turned on when you're close enough to interact

    protected bool Interactable = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InteractParticles.SetActive(true);
            Interactable = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            InteractParticles.SetActive(false);
            Interactable = false;
        }
    }
}
