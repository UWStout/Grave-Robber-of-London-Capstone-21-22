using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    /// <summary>
    /// Checks for if the player is inside in the bush to
    ///     Make the bush transparent, lantern turns off, player is hidden
    /// </summary>
    /// <param name="other"> Object currently Colliding with the Bush</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Toggles the Stealth
            StealthManager.Instance.ToggleInBush();

            // Makes the bush transparent
            gameObject.GetComponentInChildren<Renderer>().material.color += new Color(0, 0, 0, -0.4f);

            // Turns off the Lantern
            GameObject.Find("PlayerLightFront").GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("PlayerLightBack").GetComponent<MeshRenderer>().enabled = false;

            // Makes the Player and all Parts semi Transparent
            List<GameObject> playerParts = GatherChildrenOfPlayer();
            foreach (GameObject part in playerParts)
            {
                part.GetComponent<Renderer>().material.color += new Color(0, 0, 0, -0.4f);
            }

            UIManager.Instance.EnableStealth();
        }
    }

    /// <summary>
    /// Checks if the player left the bush to
    ///     turn on lantern, make the bushes opaque, no longer hidden
    /// </summary>
    /// <param name="other"> Object that just stop Colliding with the Bush</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Unstealths the player
            StealthManager.Instance.ToggleInBush();

            // Makes the Bush Opaque
            gameObject.GetComponentInChildren<Renderer>().material.color += new Color(0, 0, 0, 0.4f);

            // Makes the Lantern Reappear
            GameObject.Find("PlayerLightFront").GetComponent<MeshRenderer>().enabled = true;
            GameObject.Find("PlayerLightBack").GetComponent<MeshRenderer>().enabled = true; ;

            // Makes the Player and all Parts Opaque
            List<GameObject> playerParts = GatherChildrenOfPlayer();
            foreach (GameObject part in playerParts)
            {
                part.GetComponent<Renderer>().material.color += new Color(0, 0, 0, 0.4f);
            }

            UIManager.Instance.DisableStealth();
        }
    }

    /// <summary>
    /// Gathers the children of Player that have a renderer on them to affect the materials
    /// </summary>
    /// <returns> List of GameObjects that have a Renderer attached to them</returns>
    private List<GameObject> GatherChildrenOfPlayer()
    {
        // Intializing an array of the renderers and making a list for the gameobjects
        Renderer[] materialChildren = GameObject.Find("Player").GetComponentsInChildren<Renderer>();
        List<GameObject> childrenWithRenderer = new List<GameObject>();

        // Goes through the renderer array and adds the GameObjects that the renderers are on to the GameObject Lists
        foreach(Renderer renderer in materialChildren)
        {
            childrenWithRenderer.Add(renderer.gameObject);
        }

        return childrenWithRenderer;
    }

}
