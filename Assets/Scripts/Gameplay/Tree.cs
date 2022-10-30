using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private Transform PlayerTransf;
    public MeshRenderer TreeRend;
    public Material TreeSwitchMat;

    private bool IsTransparent = false;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            PlayerTransf = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (PlayerTransf != null)
        {
            if (PlayerTransf.position.z - 7.5f >= transform.position.z)
            {
                if (!IsTransparent)
                {
                    Material TempMat = TreeRend.material;
                    TreeRend.material = TreeSwitchMat;
                    TreeSwitchMat = TempMat;
                    gameObject.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
                    IsTransparent = true;
                }
            }
            else if (IsTransparent)
            {
                Material TempMat = TreeRend.material;
                TreeRend.material = TreeSwitchMat;
                TreeSwitchMat = TempMat;
                gameObject.GetComponentInChildren<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                IsTransparent = false;
            }
        }
    }

    /// <summary>
    /// Looks for if the player is inside the collider to prompt them to hide behind the tree
    /// Or if they are already behind it remove the hidden affect
    /// </summary>
    /// <param name="other"> Object that is colliding with the tree</param>
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.Instance.ToggleHidden();
            // Disable movement of the Player "Need to figure out how to approach"
            Debug.Log("You have hidden in the tree");
        }
    }

    /// <summary>
    /// Checks for if the player is inside in the bush to activate the hidden status
    /// </summary>
    /// <param name="other"> Object currently Colliding with the Bush</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 PlayerDist = PlayerPos - transform.position;
            PlayerDist += new Vector3(0.73f, 0, -3.5f);
            if (PlayerDist.x > 2.0f)
            {
                StealthManager.Instance.HideBehindTree(Direction.RIGHT);
            }
            else if (PlayerDist.z < -0.2f)
            {
                StealthManager.Instance.HideBehindTree(Direction.DOWN);
            }
            else if (PlayerDist.x < -0.5f)
            {
                StealthManager.Instance.HideBehindTree(Direction.LEFT);
            }
            else if (PlayerDist.z > 1.5f)
            {
                StealthManager.Instance.HideBehindTree(Direction.UP);
            }
            Debug.Log(PlayerDist);
        }
    }

    /// <summary>
    /// Removes the hidden status affect from the player when he leaves the bush
    /// </summary>
    /// <param name="other"> Object that just stop Colliding with the Bush</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StealthManager.Instance.LeaveTree();
        }
    }
}
