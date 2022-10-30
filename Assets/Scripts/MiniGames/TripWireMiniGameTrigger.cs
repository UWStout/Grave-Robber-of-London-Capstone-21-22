/**
 * @author Damian Link
 * 
 * Version: 11/24/2021
 */
using UnityEngine;

/// <summary>
/// detects when the line intsects with the mouse collider and resets the line position
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * MiniGame - The instance of the tripwiwre minigame being used
/// * CurrentLineName - The name of the current line that the mouse collider is on
/// 
/// </remarks>
public class TripWireMiniGameTrigger : MonoBehaviour
{
    // The minimgame object
    public TripWireMiniGame MiniGame;
    // the name of current line that the collider is on
    public string CurrentLineName;

    /// <summary>
    /// detects when mouse enters a line collider
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Line") && other.name != CurrentLineName && CurrentLineName != "") {
            Debug.Log("contact");
            MiniGame.MousePressed = false;
        }
    }
}
