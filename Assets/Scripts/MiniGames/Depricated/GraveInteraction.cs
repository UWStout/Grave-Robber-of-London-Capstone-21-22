using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 *  Author: Declin Anderson
 *  Version: 10.19.2021 
 */

// Allows the User to interact with graves and proceed through the minigames associated
public class GraveInteraction : MonoBehaviour
{
    // UI element that will display key prompt
    // Object for Dig Prompt text
    [SerializeField]
    private GameObject DigPrompt;
    // Object for Access the Dig Minigame
    [SerializeField]
    private GameObject DigSlide;
    // Object for Getting current progress and resetting
    [SerializeField]
    private GameObject ProgressBar;
    // Object for Dig Done text
    [SerializeField]
    private GameObject DigDone;
    // Coffin Minigame UI
    [SerializeField]
    private GameObject WirePanel;

    // Scripts 
    private DigSliderControl Digging;
    //private CoffinCageMiniGame Coffin;

    bool StillDigging = false;

    // sets the UI object to false at the start of the game
    private void Start()
    {
        // Setting the prompt to inactive to start(find better way to do this)
        DigPrompt.SetActive(false);
        DigSlide.SetActive(false);
        DigDone.SetActive(false);
        WirePanel.SetActive(false);

        // Getting the Script for digging
        Digging = GetComponent<DigSliderControl>();
        Digging.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // Checks if the digging minigame is finished
        if (ProgressBar.GetComponent<Slider>().value == ProgressBar.GetComponent<Slider>().maxValue)
        {
            // Turning the UI elements on and off respectively
            DigSlide.SetActive(false);
            Digging.enabled = false;
            ProgressBar.GetComponent<Slider>().value = 0f;

            WirePanel.SetActive(true);
            StillDigging = true;
        }
        // Checks if the Coffin Trap Minigame is finished
        if(false)
        {
            WirePanel.SetActive(false);
            DigDone.SetActive(true);

            // Turning off the Dig Done text after 2 seconds
            Invoke("disableDone", 2);
        }
    }

    // When entering the collision give user Key Prompt
    private void OnTriggerEnter()
    {
        DigPrompt.SetActive(true);
    }

    // While the user is in the collision look for key prompt for minigame
    private void OnTriggerStay()
    {
        // If user enters the key prompt while colliding
        // Remove collider and prompt, Activate Digging Game
        if (Input.GetKeyDown(KeyCode.T))
        {
            GetComponent<BoxCollider>().enabled = false;
            DigPrompt.SetActive(false);
            DigSlide.SetActive(true);
            Digging.enabled = true;
        }
    }

    // When exiting the trigger it removes the Key prompt
    private void OnTriggerExit()
    {
        DigPrompt.SetActive(false);
    }

    // Turns off the Dig Done text
    void disableDone()
    {
        DigDone.SetActive(false);
    }
}