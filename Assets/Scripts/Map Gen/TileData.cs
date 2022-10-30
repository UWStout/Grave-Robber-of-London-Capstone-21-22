using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores information on each tile in the map.
/// </summary>
/// <param name="Noise">
/// Indicates the noise level of the current tile, determines if a guard can detect the actions of the player.
/// </param>
/// <param name="NoiseSource">
/// Hold which tile is the source of the loudest noise affecting the tile.
/// </param>
/// <param name="Light">
/// Holds the current light level of the tile.
/// </param>
/// <param name="TileType">
/// Holds the tile's position in the tile array.
/// </param>
/// <param name="HasGrave">
/// Whether this tile has a grave or not.
/// </param>
/// <param name="GravePos">
/// The grave's offset from the center of this tile.
/// </param>
/// <param name="Tombstone">
/// The game object for the tile's grave.
/// </param>
/// <param name="GI">
/// A reference to the child object's GraveInfo script.
/// </param>
/// <param name="IsGraveCluster">
/// Whether or not this tile is the center of a grave cluster.
/// </param>
public class TileData : MonoBehaviour
{
    // Indicates the noise level of the current tile, determines if a guard can detect the actions of the player
    public float Noise;

    // Hold which tile is the source of the loudest noise affecting the tile
    private Vector2Int NoiseSource = new Vector2Int();

    // Holds the current light level of the tile
    private int Light;

    // Holds the tile's position in the tile array
    public TileType TileType;

    // Holds the x and y positions of the tile
    private int XPos;
    private int YPos;

    // Information on the tile's grave, if it has one
    private bool HasGrave = false;
    private float[] GravePos = new float[2];
    private GameObject Tombstone;
    private GraveInfoRefactor GI;

    // Information on a grave cluster, if this tile is the center
    private bool IsGraveCluster = false;

    private void FixedUpdate()
    {
        // Noise decay
        if (Noise > 0)
        {
            Noise -= 0.03f;
        }
    }

    /// <summary>
    /// Overides tile's noise value if the new value is larger.
    /// </summary>
    /// <param name="Val">
    /// The value of the incoming noise.
    /// </param>
    /// <param name="Source">
    /// The source tile of the noise.
    /// </param>
    /// <returns>
    /// Returns true if the noise was high enough to overwrite, false otherwise.
    /// </returns>
    public bool OverrideNoise(float Val, Vector2Int Source)
    {
        if (Val > Noise)
        {
            Noise = Val;
            NoiseSource = Source;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Instantiates a grave prefab on the current tile.
    /// </summary>
    /// <param name="TSPrefab">
    /// The prefab to be created.
    /// </param>
    /// <param name="Dir">
    /// The direction the grave is facing.
    /// </param>
    /// <param name="GQ">
    /// The quality tier of the grave.
    /// </param>
    /// <returns>
    /// Returns true if function succesfully generated grave, false otherwise.
    /// </returns>
    public bool InitiateGrave(GameObject TSPrefab, Direction Dir, GraveQuality GQ)
    {
        if (HasGrave)
        {
            GravePos[0] = Random.Range(0.4f, 0.7f);
            if (Dir == Direction.RIGHT || Dir == Direction.UP)
            {
                GravePos[0] *= -1;
            }

            GravePos[1] = Random.Range(-0.6f, 0.6f);

            if (Dir == Direction.LEFT || Dir == Direction.RIGHT)
            {
                Tombstone = Instantiate(TSPrefab, new Vector3(GravePos[0], 0.5f, GravePos[1]) + this.transform.position, TSPrefab.transform.rotation, this.transform);
            }
            else
            {
                Tombstone = Instantiate(TSPrefab, new Vector3(GravePos[1], 0.5f, GravePos[0]) + this.transform.position, TSPrefab.transform.rotation, this.transform);
            }
            GI = Tombstone.GetComponent<GraveInfoRefactor>();
            // RUN GRAVEINFO GENERATION FUNCTION HERE

            return true;
        }

        return false;
    }

    // Tile Type get/set
    public TileType GetTileType()
    {
        return TileType;
    }

    public void SetTileType(TileType val)
    {
        TileType = val;
    }

    // Tile position get/set
    public int GetXPos()
    {
        return XPos;
    }

    public int GetYPos()
    {
        return YPos;
    }

    public Vector2Int GetPos()
    {
        return new Vector2Int(XPos, YPos);
    }

    public void SetPos(int x, int y)
    {
        XPos = x;
        YPos = y;
    }

    // Noise get/set
    public float GetNoise()
    {
        return Noise;
    }

    public void SetNoise(float Val)
    {
        Noise = Val;
    }

    // Noise source get/set
    public Vector2Int GetNoiseSource()
    {
        return NoiseSource;
    }

    public void SetNoiseSource(Vector2Int Val)
    {
        NoiseSource = Val;
    }

    // HasGrave get/set
    public bool GetHasGrave()
    {
        return HasGrave;
    }

    public void SetHasGrave(bool Val)
    {
        HasGrave = Val;
    }

    // GravePos get/set
    public float[] GetGravePos()
    {
        return GravePos;
    }

    public void SetGravePos(float[] val)
    {
        GravePos = val;
        if (HasGrave)
        {
            Tombstone.transform.position = new Vector3(GravePos[0], Tombstone.transform.position.y, GravePos[1]);
        }
    }
}
