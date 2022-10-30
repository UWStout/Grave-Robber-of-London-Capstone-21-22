using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically changes the textures on the path tile depending on its connections to other path tiles.
/// </summary>
/// <param name="ChildTiles">
/// The four child tiles making up the path tile object.
/// </param>
public class PathTile : GeneralTile
{
    // The four child tiles making up the path tile object
    public GameObject[] ChildTiles = new GameObject[4];

    // Textures for each corner of the path tiles
    public Material[] PathMaterials = new Material[12];

    // Whether or not the path is a corner tile.
    private bool IsCorner = false;

    // If it is a corner tile, a list of other corners it connects to.
    // First two ints are x/y position, last is distance in tiles
    public List<Vector3Int> CornerConnections;

    /// <summary>
    /// Changes the texture of one of the four child tiles (one in each corner) so that it connects to adjecent path tiles.
    /// Since each child tile is in a corner, it is connected to two other tiles, the tile that comes first clockwise is the first connection, and the one 90 degrees clockwise is the second.
    /// Example: The top left corner is connected to the North and West tiles. West is considered the first connection, and North is the second.
    /// </summary>
    /// <param name="Mats">
    /// A list of textures for the path tile.
    /// </param>
    /// <param name="FirstConnection">
    /// The tile type in the first direction the corner connects to (clockwise).
    /// </param>
    /// <param name="SecondConnection">
    /// The tile type in the second direction the corner connects to (clockwise).
    /// </param>
    /// <param name="Offset">
    /// The position of the child tile in the ChildTiles array.
    /// </param>
    public void SetPathConnections(int FirstConnection, int SecondConnection, int Offset)
    {
        GameObject Child = ChildTiles[Offset];

        if (FirstConnection == 2 && SecondConnection == 2)
        {
            Offset = Offset + 4;
        }
        else if (FirstConnection == 2)
        {
            Offset = (Offset + 1) % 4;
        }
        else if (SecondConnection == 2)
        {
            Offset = Offset + 0;
        }
        else
        {
            Offset = Offset + 8;
        }

        Child.GetComponent<Renderer>().material = PathMaterials[Offset];
    }

    public List<Vector3Int> GetCornerConnections()
    {
        return CornerConnections;
    }

    public bool GetIsCorner()
    {
        return IsCorner;
    }

    public void SetCornerConnections(List<Vector3Int> _Val)
    {
        CornerConnections = _Val;
        IsCorner = true;
    }
}
