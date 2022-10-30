using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockTile : GeneralTile
{
    public List<GameObject> Options;

    // Start is called before the first frame update
    void Awake()
    {
        int RandOption = Random.Range(0, Options.Count);

        Options[RandOption].SetActive(true);
    }
}
