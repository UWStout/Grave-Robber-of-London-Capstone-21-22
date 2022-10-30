using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesPersonColider1 : MonoBehaviour
{
    // UI for the player
    private UIManager PlayerUI;
    // Boolean to show when the store UI is open
    private bool StoreOpen = false;


    // Start is called before the first frame update
    void Awake()
    {
        PlayerUI = GameObject.Find("PlayerUI").GetComponent<UIManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (StoreOpen == true)
            {
                PlayerUI.CloseStore();
                StoreOpen = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            if (StoreOpen == false)
            {
                PlayerUI.OpenStore();
                StoreOpen = true;
            }
            else
            {
                PlayerUI.CloseStore();
                StoreOpen = false;
            }
        }
    }
}
