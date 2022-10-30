using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Author: Declin Anderson
 *  Version: 10.19.2021 
 */

//Makes the camera follow the user based on the LERP function
public class DistanceCamMove : MonoBehaviour
{
    // Player is the object that the camera must follow
    [SerializeField] private GameObject Player;

    // Speed at which the camera will reach the target position
    public float speed = 1.5f;

    // How far away the camera should end up from the player
    public float OffSet = 5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
         * Moves the camera using the LERP function to smooth the transition of the camera following
         * LERP(vector3 current, vector3 target, float speed)
         * Parameters:
         *      current = The current position of the object this script is attached to
         *      target = The position that the camera is trying to reach, use the offset in z position to position best the camera
         *      speed = the intial rate that the camera will move at, slows as it approaches target
        */
        transform.position = Vector3.Lerp(transform.position, 
                Player.transform.position - new Vector3(0, Player.transform.position.y - transform.position.y, OffSet), 
                speed * Time.deltaTime);
    }
}
