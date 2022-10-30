using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveTile : GeneralTile
{
    // A list of headstone prefabs which can be on the tile
    public GameObject[] Headstones;

    // Reference to the headstone prefab
    private GameObject Tombstone;

    private GraveInfoRefactor GI;

    // X/Y position of the grave compared to the tile's center
    private float[] GravePos = new float[2];

    // Creates the tombstone object on the grave tile
    public void Initialize(int HeadstoneIndex, Direction DI)
    {
        GravePos[0] = Random.Range(0.4f, 0.7f);
        if (DI == Direction.RIGHT || DI == Direction.UP)
        {
            GravePos[0] *= -1;
        }

        GravePos[1] = Random.Range(-0.6f, 0.6f);

        if (DI == Direction.LEFT || DI == Direction.RIGHT)
        {
            Tombstone = Instantiate(Headstones[HeadstoneIndex], new Vector3(GravePos[0], 0.5f, GravePos[1]) + this.transform.position, Headstones[HeadstoneIndex].transform.rotation, this.transform);
        }
        else
        {
            Tombstone = Instantiate(Headstones[HeadstoneIndex], new Vector3(GravePos[1], 0.5f, GravePos[0]) + this.transform.position, Headstones[HeadstoneIndex].transform.rotation, this.transform);
        }

        GI = Tombstone.GetComponent<GraveInfoRefactor>();
        GI.Initiate();
    }

    public GraveInfoRefactor GetGraveInfo()
    {
        GI = Tombstone.GetComponent<GraveInfoRefactor>();
        return GI;
    }

    public void SetGraveInfo(GraveInfoRefactor Val)
    {
        GI = Val;
    }

    public GameObject GetTombstone()
    {
        return Tombstone;
    }
}
