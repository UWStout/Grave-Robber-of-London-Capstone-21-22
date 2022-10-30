using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceTile : GeneralTile
{
    // Holds each of the disabled fence objects
    public GameObject FenceUp;
    public GameObject FenceRight;
    public GameObject FenceDown;
    public GameObject FenceLeft;

    /// <summary>
    /// Enables the fence in each direction based on the passed in connections.
    /// </summary>
    /// <param name="connections">
    /// Determines which directions the fence connects to.
    /// 0 = Does not connect.
    /// 1 = Connect
    /// </param>
    public void GenerateFence(int[] Connections, int Goal)
    {
        if (Connections[0] == Goal)
        {
            FenceUp.SetActive(true);
        }
        if (Connections[1] == Goal)
        {
            FenceRight.SetActive(true);
        }
        if (Connections[2] == Goal)
        {
            FenceDown.SetActive(true);
        }
        if (Connections[3] == Goal)
        {
            FenceLeft.SetActive(true);
        }
    }
}
