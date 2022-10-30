using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a MapInfo object. Attach to level select buttons so they can set their MapInfo on click.
/// </summary>
public class MapInfoHolder : MonoBehaviour
{
    public MapInformation MI;

    /// <summary>
    /// Attaches the MapInformation object currently on the button to the RunManager.
    /// </summary>
    public void SetMIAsActive()
    {
        GameManager.Instance._RunManager.SetMapInfo(MI);
    }

    /// <summary>
    /// Sets the button's MapInformation object.
    /// </summary>
    /// <param name="_Val">
    /// A MapInformation object.
    /// </param>
    public void SetMI(MapInformation _Val)
    {
        MI = _Val;
    }
}
