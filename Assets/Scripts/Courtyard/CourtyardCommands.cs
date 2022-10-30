using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Various commands for the courtyard scene publicly accessable.
/// </summary>
public class CourtyardCommands : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.DisableBlackScreen();

        UIManager.Instance.RandomizeLevels();
    }
}
