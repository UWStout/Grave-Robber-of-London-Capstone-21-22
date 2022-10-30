using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds all important information for spawning in a graveyard level.
/// </summary>
[CreateAssetMenu(fileName = "Map Information", menuName = "ScriptableObjects/New Graveyard Map", order = 3)]
[System.Serializable]
public class MapInformation : ScriptableObject
{
    [Header("Map Image")]
    // The image used to load the map
    [SerializeField] private Texture2D MapImage; // Texture object for loading the map
    [SerializeField] private ImageElement MapColor; // If not prebuilt, the color of the image that will be used
    [SerializeField] private bool Prebuilt; // Whether the map should be loaded as prebuilt or not

    [Header("Graveyard Information")]
    // Name of the graveyard
    [SerializeField] private string GraveyardName = "Default Name";

    // The position of the entrance tile in the layout array, other tiles are built out from this position
    [SerializeField] private Vector2Int GraveyardEntrancePos = new Vector2Int(0, 0);

    // The tier of current graveyard. Influences map generation
    [SerializeField] private GraveyardTier LevelTier = GraveyardTier.LOW;

    // Distance from spawn point which guards cannot move to on a route
    [SerializeField] private int SpawnProtectionDistance = 5;

    [Header("Grave Spawn")]
    // Determines the increase gravespawn chance if the previous chance failed (10 = 10%)
    [SerializeField] private int GraveRNGIncrease = 10;

    // Determines the minimum percent of empty grass tiles connected to paths in the correct direction
    // which will be guarenteed to be populated with graves
    [SerializeField] private float GraveDensity = 0.1f;

    // Determines the percent chance each empty grass tile not attached to a path will have of having a grave
    [SerializeField] private float DetachedGraveChance = 0.02f;

    // Determines the number of grave clusters on the map
    // (Grave clusters are areas with greatly increased grave spawn rates spreading two tiles in each direction from a central point)
    [SerializeField] private int GraveClusters = 3;

    // Direction of the graves in the graveyard
    [SerializeField] private Direction GraveDirection = Direction.LEFT;

    [Header("Tree Spawn")] // Maybe make these a scriptable object?
    // Spawn parameters for trees
    [SerializeField] private int MaxTrees = 2;
    [SerializeField] private int TreeMinDistance = 6;

    [Header("Bush Spawn")]
    // Spawn parameters for bushes
    [SerializeField] private int MaxBushes = 5;
    [SerializeField] private int BushMinDistance = 5;

    [Header("BirdBath Spawn")]
    // Spawn parameters for birdbaths
    [SerializeField] private int MaxBirdBaths = 2;
    [SerializeField] private int BirdBathMinDistance = 10;

    [Header("Rock Spawn")]
    // Spawn parameters for birdbaths
    [SerializeField] private int MaxRocks = 10;
    [SerializeField] private int RockMinDistance = 5;

    [Header("Cloud Spawn")]
    // Spawn parameters for birdbaths
    [SerializeField] private int MaxClouds = 10;
    [SerializeField] private int CloudMinDistance = 5;

    [Header("Guard Spawn")]
    // Determines number of guards in level
    [SerializeField] private int GuardNumber = 2;

    public Texture2D GetMapImage()
    {
        return MapImage;
    }

    public ImageElement GetMapColor()
    {
        return MapColor;
    }

    public bool GetPrebuilt()
    {
        return Prebuilt;
    }

    public string GetGraveyardName()
    {
        return GraveyardName;
    }

    public Vector2Int GetGraveyardEntrancePosition()
    {
        return GraveyardEntrancePos;
    }

    public int GetSpawnProtectionDistance()
    {
        return SpawnProtectionDistance;
    }

    public int GetGraveRNGIncrease()
    {
        return GraveRNGIncrease;
    }

    public GraveyardTier GetLevelTier()
    {
        return LevelTier;
    }

    public float GetGraveDensity()
    {
        return GraveDensity;
    }

    public float GetDetachedGraveChance()
    {
        return DetachedGraveChance;
    }

    public int GetGraveClusters()
    {
        return GraveClusters;
    }

    public Direction GetGraveDirection()
    {
        return GraveDirection;
    }

    public int GetMaxTrees()
    {
        return MaxTrees;
    }

    public int GetTreeMinDistance()
    {
        return TreeMinDistance;
    }

    public int GetMaxBushes()
    {
        return MaxBushes;
    }

    public int GetBushMinDistance()
    {
        return BushMinDistance;
    }

    public int GetMaxRocks()
    {
        return MaxRocks;
    }

    public int GetRockMinDistance()
    {
        return RockMinDistance;
    }

    public int GetMaxBirdBaths()
    {
        return MaxBirdBaths;
    }

    public int GetBirdBathMinDistance()
    {
        return BirdBathMinDistance;
    }

    public int GetMaxClouds()
    {
        return MaxClouds;
    }

    public int GetCloudMinDistance()
    {
        return CloudMinDistance;
    }

    public int GetGuardNumber()
    {
        return GuardNumber;
    }
}
