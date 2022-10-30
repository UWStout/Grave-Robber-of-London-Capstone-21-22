using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : MonoBehaviour
{
    public Material AlternateTexture;

    /// <summary>
    /// Randomly determines the texture of the grass tile.
    /// </summary>
    void Start()
    {
        bool rand = Random.Range(0, 4) == 0;

        if (rand)
        {
            gameObject.GetComponent<MeshRenderer>().material = AlternateTexture;
        }
    }
}
