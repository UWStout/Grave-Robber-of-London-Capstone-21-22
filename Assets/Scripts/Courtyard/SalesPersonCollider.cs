/**
 * Author: Damian Link
 * Version: 3/8/22
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the collider of the sales person
/// </summary>
public class SalesPersonCollider : MonoBehaviour
{
    // UI for the player
    private UIManager PlayerUI;
    // Boolean to show when the store UI is open
    private bool StoreOpen = false;
    private bool PlayerInArea = false;

    /// <summary>
    /// Gets the playerUI
    /// </summary>
    void Awake()
    {
        PlayerUI = GameObject.Find("PlayerUI").GetComponent<UIManager>();
    }

    /// <summary>
    /// Sets the store to close
    /// </summary>
    /// <param name="other">object colliding</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInArea = false;
        }
    }

    /// <summary>
    /// Opens the store Ui when the interact key is pressed
    /// </summary>
    /// <param name="other">object colliding</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            PlayerInArea = true;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && PlayerInArea == true)
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

        if (PlayerInArea == false && StoreOpen == true)
        {
            PlayerUI.CloseStore();
            StoreOpen = false;
        }
    }
}
