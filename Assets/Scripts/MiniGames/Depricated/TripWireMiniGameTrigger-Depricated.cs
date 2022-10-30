/**
 * @author Damian Link
 * 
 * Version: 11/05/2021
 */
using UnityEngine;

/// <summary>
/// detects when the line intsects with the mouse collider and resets the line position
/// </summary>
public class TripWireMiniGameTriggerDepricated : MonoBehaviour
{
 
    //TripWireMiniGame MiniGame = null;

    /// <summary>
    /// gets the trip wire minigame object
    /// </summary>
    private void Start()
    {
        //MiniGame = GameObject.Find("TripWireMiniGame").GetComponent<TripWireMiniGame>();
    }

    /// <summary>
    /// detects when mouse enters a line collider
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Collider") {
            Debug.Log("contact");
            //MiniGame.MousePressed = false;
        }
    }
}
