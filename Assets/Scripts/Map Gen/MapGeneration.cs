using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// Handles everything for map generation withing a graveyard.
/// </summary>
public class MapGeneration : MonoBehaviour
{
    // The parent object which contains all ground elements
    protected GameObject Parent; // Rename to match gamespace

    [Header("Prefabs")]
    // Public array of all possible tile objects for map generation
    // [0 = Grass Tile, 1 = Entrance Tile, 2 = Path Tile, 3 = Fence Tile, 4 = Bush Tile, 5 = Tree Tile]
    public GameObject[] Tiles;

    // Public array of headstones for all tiers of graves
    // [0-3 = Low tier graves, 4-7 = Mid tier graves, 8-11 = High tier graves]
    public GameObject[] Headstones;

    // The guard game object prefab
    public GameObject Guard;

    // List of all guards in the level
    protected List<GameObject> GuardList = new List<GameObject>();

    // List of path corners that guards can use in their routes
    protected List<Vector2Int> GuardPathCorners;

    // Layout of the tiles in the graveyard
    // First position is x, second is y, and stored variable is tile type
     // Rename to be more clear, map layout can be removed to just youse TileDatas[x].tiletype
    protected TileData[,] TileDatas;

    // List of the graves in the graveyard
    protected List<Vector2Int> GraveList = new List<Vector2Int>(); // Turn int[] into Vector2s
    protected List<Vector2Int> GraveClusterCenters = new List<Vector2Int>(); // Make names more obvious like GraveListCoords

    // List of the bushes in the graveyard
    protected List<Vector2Int> BushList = new List<Vector2Int>();

    // List of the trees in the graveyard
    protected List<Vector2Int> TreeList = new List<Vector2Int>();

    // List of all cloud tiles in the graveyard
    protected List<Vector2Int> CloudList = new List<Vector2Int>();

    // List of all bird bath tiles in the graveyard
    protected List<Vector2Int> BirdBathList = new List<Vector2Int>();

    // List of all rock tile in the graveyard
    protected List<Vector2Int> RockList = new List<Vector2Int>();

    // Direction of the graves in the graveyard
    public Direction GraveDirection = Direction.LEFT;

    // Dimensions for the graveyard (in tiles)
    protected int GraveyardWidth = 10;
    protected int GraveyardLength = 10;

    // Distance towards spawn which guards can't approach on route
    private int SpawnProtectionDistance = 5;

    [Header("Graveyard Information")]
    // The position of the entrance tile in the layout array, other tiles are built out from this position
    public Vector2Int GraveyardEntrancePos = new Vector2Int(7, 0);

    // The scale of the square tile prefabs
    protected float TileSize = 2.0f;

    [Header("Grave Spawn")]
    // Determines the increase gravespawn chance if the previous chance failed
    public int GraveRNGIncrease = 10;

    // Determines the minimum percent of empty grass tiles connected to paths in the correct direction
    // which will be guarenteed to be populated with graves
    public float GraveDensity = 0.5f;

    // Determines the percent chance each empty grass tile not attached to a path will have of having a grave
    public float DetachedGraveChance = 0.1f;

    // Determines the number of grave clusters on the map
    // (Grave clusters are areas with greatly increased grave spawn rates spreading two tiles in each direction from a central point)
    public int GraveClusters = 3;

    [Header("Tree Spawn")] // Maybe make these a scriptable object?
    // Spawn parameters for trees
    public int MaxTrees = 2;
    public int TreeMinDistance = 6;

    [Header("Bush Spawn")]
    // Spawn parameters for bushes
    public int MaxBushes = 5;
    public int BushMinDistance = 5;

    // Spawn parameters for birdbaths
    private int MaxBirdBaths = 2;
    private int BirdBathMinDistance = 10;

    // Spawn parameters for birdbaths
    private int MaxRocks = 10;
    private int RockMinDistance = 5;

    // Spawn parameters for birdbaths
    private int MaxClouds = 10;
    private int CloudMinDistance = 5;

    [Header("Guard Spawn")]
    // Determines number of guards in level
    public int GuardNumber = 2;

    protected NavMeshSurface NMSurface;

    [Header("Map Image")]
    // The image used to load the map
    public Texture2D MapImage;
    public ImageElement MapColor;
    public bool Prebuilt;

    // Start is called before the first frame update
    void Start()
    {
        // Gets all the variables for map gen from the run manager
        PullMapVars();

        // Sets this as the current active map for the stealth manager
        StealthManager.Instance.SetMG(this);

        Parent = gameObject.transform.parent.gameObject;
        NMSurface = Parent.GetComponent<NavMeshSurface>();

        ImageLoader il = new ImageLoader();

        GraveyardWidth = MapImage.width;
        GraveyardLength = MapImage.height;

        // A temporary map object that holds the type of each tile before the map is properly generated
        TileType[,] MapLayout;

        // Loading map layout from passed in image
        if (!Prebuilt)
        {
            // Reads all information from a single color
            MapLayout = il.LoadMap(MapImage, MapColor);
        }
        else
        {
            // Uses all colors to read special information for prebuilt map
            MapLayout = il.PrebuiltMap(MapImage);
        }

        // Forcing graveyard entry position to path tile
        MapLayout[GraveyardEntrancePos[0], GraveyardEntrancePos[1]] = TileType.PATH;

        if (!Prebuilt)
        {
            // Randomizing grave positions
            GenerateGravePoints(ref MapLayout);

            // Adding trees
            GenerateGenericTilePoints(TileType.TREE, TreeMinDistance, MaxTrees, TreeList, 2, 2, false, true, ref MapLayout);

            // Adding bushes
            GenerateGenericTilePoints(TileType.BUSH, BushMinDistance, MaxBushes, BushList, 1, 1, true, false, ref MapLayout);

            GenerateGenericTilePoints(TileType.CLOUD, CloudMinDistance, MaxClouds, CloudList, 1, 1, true, false, ref MapLayout);

            GenerateGenericTilePoints(TileType.BIRDBATH, BirdBathMinDistance, MaxBirdBaths, BirdBathList, 1, 1, true, false, ref MapLayout);

            GenerateGenericTilePoints(TileType.ROCK, RockMinDistance, MaxRocks, RockList, 1, 1, true, false, ref MapLayout);
        }

        // Spawning in tile prefabs
        GenerateMap(MapLayout);

        // Setting up corner connections
        SetPathConnections(MapLayout);

        // Building nav mesh
        NMSurface.BuildNavMesh();

        // Mapping all corners points on paths the guards can path to
        GuardPathCorners = PathCornerPoints();
        
        List<List<Vector2Int>> GuardRoutes = new List<List<Vector2Int>>();
        List<Vector2Int> GuardSpawnPositions = new List<Vector2Int>();

        if (!Prebuilt || GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() != GraveyardTier.TUTORIAL)
        {
            // Chooses a random starting position for all guards
            for (int i = 0; i < GuardNumber; i++)
            {
                int StartingCorner = Random.Range(0, GuardPathCorners.Count);
                //GuardRoutes.Add(SinglePathCheck(40, 5, 0, new List<int[]>(), PathCorners[StartingCorner], PathCorners[StartingCorner], PathCorners[StartingCorner], new int[GraveyardWidth, GraveyardLength]));
                List<Vector2Int> TempRoute = new List<Vector2Int>();
                TempRoute.Add(GuardPathCorners[StartingCorner]);
                GuardRoutes.Add(TempRoute);
                GuardSpawnPositions.Add(GuardPathCorners[StartingCorner]);
            }
        }
        else
        {
            // Pulls guard starting locations from map image
            GuardRoutes = il.PrebuiltGuardRoutes(MapImage);
        }

        // Spawns in each guard
        foreach (Vector2Int Route in GuardSpawnPositions)
        {
            GameObject GuardSpawn = TileDatas[Route[0], Route[1]].gameObject;
            GameObject TempGuard = (GameObject)Instantiate(Guard, GuardSpawn.transform.position, Quaternion.identity);
            Vector2Int LookoutGoalsTemp = new Vector2Int(0, 0);
            Transform[] RoutePointsTemp = new Transform[1];
            RoutePointsTemp[0] = TileDatas[Route[0], Route[1]].transform;

            TempGuard.GetComponent<Guard>().GenerateGuard(RoutePointsTemp, LookoutGoalsTemp);
            GuardList.Add(TempGuard);
            //Debug.Log(GuardList.Count);
        }

        GameManager.Instance.UpdateGraveTotal(GraveList.Count);
    }

    public void TestSetCorners()
    {
        GuardPathCorners = PathCornerPoints();
    }

    public void TestSetSize(int wi, int le)
    {
        GraveyardLength = le;
        GraveyardWidth = wi;
    }

    public bool TestGetConnected(Vector2Int FirstPoint, Vector2Int SecondPoint)
    {
        return CheckConnected(FirstPoint, SecondPoint);
    }

    public int TestGetDistance(Vector2Int FirstPoint, Vector2Int SecondPoint)
    {
        return GetDistance(FirstPoint, SecondPoint);
    }

    /// <summary>
    /// Pulls information used for map gen from the RunManager's MapInfo object.
    /// </summary>
    /// <returns>
    /// Whether or not the RunManager's MapInfo object was not null.
    /// </returns>
    private bool PullMapVars()
    {
        MapInformation TempMI = GameManager.Instance._RunManager.GetMapInfo();
        if (TempMI != null)
        {
            this.MapImage = TempMI.GetMapImage();
            this.MapColor = TempMI.GetMapColor();
            this.Prebuilt = TempMI.GetPrebuilt();
            this.GraveyardEntrancePos = TempMI.GetGraveyardEntrancePosition();
            this.SpawnProtectionDistance = TempMI.GetSpawnProtectionDistance();
            this.GraveRNGIncrease = TempMI.GetGraveRNGIncrease();
            this.GraveDensity = TempMI.GetGraveDensity();
            this.DetachedGraveChance = TempMI.GetDetachedGraveChance();
            this.GraveClusters = TempMI.GetGraveClusters();
            this.GraveDirection = TempMI.GetGraveDirection();
            this.MaxTrees = TempMI.GetMaxTrees();
            this.TreeMinDistance = TempMI.GetTreeMinDistance();
            this.MaxBushes = TempMI.GetMaxBushes();
            this.BushMinDistance = TempMI.GetBushMinDistance();
            this.MaxRocks = TempMI.GetMaxRocks();
            this.RockMinDistance = TempMI.GetRockMinDistance();
            this.MaxBirdBaths = TempMI.GetMaxBirdBaths();
            this.BirdBathMinDistance = TempMI.GetBirdBathMinDistance();
            this.MaxClouds = TempMI.GetMaxClouds();
            this.CloudMinDistance = TempMI.GetCloudMinDistance();
            this.GuardNumber = TempMI.GetGuardNumber();
        }

        return TempMI != null;
    }

    /// <summary>
    /// Spawns in tile prefabs using the passed in map array to generate a playable level.
    /// </summary>
    /// <param name="Layout">
    /// A 2d array covering the graveyard's width and hight. Uses ints to indicate the tile type of each spot in the graveyard.
    /// </param>
    public void GenerateMap(TileType[,] MapLayout)
    {
        GraveyardWidth = MapLayout.GetLength(0);
        GraveyardLength = MapLayout.GetLength(1);

        // Temporary GameObject array of the whole map, TileDatas will hold final references
        GameObject[,] TileMap = new GameObject[GraveyardWidth, GraveyardLength];

        // Setting size of TileDatas array
        TileDatas = new TileData[GraveyardWidth, GraveyardLength];

        // Getting the position of the entrance tile so other tiles can have their position based on it
        float EntranceX = gameObject.transform.position.x;
        float EntranceY = gameObject.transform.position.y;
        float EntranceZ = gameObject.transform.position.z;

        
        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                TileInitialize.UpdateMap(ref MapLayout, new Vector2Int(i, j));
            }
        }
        

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                // Determining how far from the entrance tile each tile is
                int XSpaces = i - GraveyardEntrancePos[0];
                int ZSpaces = j - GraveyardEntrancePos[1];

                // Calculating position in world space
                Vector3 NewTilePos = new Vector3(EntranceX + (TileSize * XSpaces), EntranceY, EntranceZ + (TileSize * ZSpaces));

                // Instantiation tile
                TileMap[i, j] = Instantiate(Tiles[(int)MapLayout[i, j]], NewTilePos, Tiles[(int)MapLayout[i, j]].transform.rotation);

                // Renaming it to make it easier to identify in editor
                TileMap[i, j].name += "_" + i + "_" + j;

                // Setting parent object
                TileMap[i, j].transform.parent = Parent.transform;

                // Adding tile to TileDatas array
                TileDatas[i, j] = TileMap[i, j].GetComponent<TileData>();
                TileDatas[i, j].SetTileType(MapLayout[i, j]);
                TileDatas[i, j].SetPos(i, j);

                // Special commands which update the MapLayout array based on tile type passed in.
                //TileInitialize.UpdateMap(ref TileDatas[i, j], ref MapLayout);
                TileInitialize.InitializeTile(ref TileDatas[i, j], ref MapLayout);
            }
        }
    }

    /// <summary>
    /// Sets the connections list on all corner path tiles to be used in a shortest path algorithm.
    /// </summary>
    /// <param name="MapLayout">
    /// An array of the types of each tile in the map.
    /// </param>
    private void SetPathConnections(TileType[,] MapLayout)
    {
        List<Vector2Int> Corners = PathCornerPoints(MapLayout);

        // Adding information to each corner tile to work with a shortest path algorithm
        foreach (Vector2Int C in Corners)
        {
            List<Vector3Int> CornerConnections = new List<Vector3Int>();
            foreach (Vector2Int D in Corners)
            {
                if (CheckConnected(C, D))
                {
                    CornerConnections.Add(new Vector3Int(D.x, D.y, GetDistance(C, D)));
                }
            }

            TileDatas[C.x, C.y].transform.GetComponent<PathTile>().SetCornerConnections(CornerConnections);
        }
    }

    /// <summary>
    /// Returns a 2d array of bools the same size as the map layout indicating if each tile is empty and adjacent to a path
    /// </summary>
    /// <returns>
    /// A 2d array of bools indicating if each tile in the graveyard is empty, and adjacent to a path tile.
    /// </returns>
    public bool[,] PathAdjacencyMap(TileType[,] MapLayout)
    {
        bool[,] PathAdjacencyArray = new bool[GraveyardWidth, GraveyardLength];

        // Determines which side of the grave that a path should be checked for
        int PathOffset = 1;
        if (GraveDirection == Direction.LEFT)
        {
            PathOffset = -1;
        }

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                if (MapLayout[i, j] == TileType.GRASS && SafeTileCheck(i + PathOffset, j, TileType.PATH, MapLayout))
                {
                    PathAdjacencyArray[i, j] = true;
                }
                else
                {
                    PathAdjacencyArray[i, j] = false;
                }
            }
        }

        return PathAdjacencyArray;
    }

    /// <summary>
    /// Returns an array of just the path adjacent tiles.
    /// </summary>
    /// <returns>
    /// A list of all tiles that are adjacent to paths.
    /// </returns>
    private List<Vector2Int> PathAdjacentArray(TileType[,] MapLayout)
    {
        List<Vector2Int> PathAdjacencyArray = new List<Vector2Int>();
        bool[,] AdjacencyMap = PathAdjacencyMap(MapLayout);

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                if (AdjacencyMap[i, j])
                {
                    Vector2Int Temp = new Vector2Int(i, j);
                    PathAdjacencyArray.Add(Temp);
                }
            }
        }

        return PathAdjacencyArray;
    }

    /// <summary>
    /// Randomly assigns tiles along the paths to be grave tiles. Tiles that are adjacent to paths have higher chances of being a grave.
    /// </summary>
    private void GenerateGravePoints(ref TileType[,] MapLayout) // A few more comments/cleanup
    {
        // List of possible grave spawn points
        List<Vector2Int> PathAdjacencyArray = PathAdjacentArray(MapLayout);

        // Base chance out of 100 for a grave to spawn on a given tile
        float GraveChance = 50;

        // Max number of graves to spawn, based on map size and desired density percent
        float MaxGraves = PathAdjacencyArray.Count * GraveDensity;

        // Make sure not to make density too high, may cause infinite loop
        while (GraveList.Count < MaxGraves)
        {
            foreach (Vector2Int Tile in PathAdjacencyArray)
            {
                bool SpawnGrave = Random.Range(0, 100) < GraveChance;

                // Checking if grave already exists on spot
                bool AlreadyExists = GraveList.Any(p => EqualTiles(p, Tile));

                // Every time a grave doesn't spawn, it will increase the chance of a grave spawning on the next spot by a slight amount
                // Every time a grave does spawn, it will reset the chance back to 0
                if (SpawnGrave && !AlreadyExists)
                {
                    MapLayout[Tile[0], Tile[1]] = TileType.GRAVE;
                    GraveChance = 0;
                    GraveList.Add(new Vector2Int(Tile[0], Tile[1]));
                }
                else
                {
                    GraveChance += GraveRNGIncrease;
                }
            }
        }

        // Looks through all spawned graves and decides a couple to be grave clusters, which are roughly ~6 graves clustered together
        GenerateGraveClusters();

        // Recursive functions that look through the tiles near the grave cluster centers and choose which should spawn graves
        foreach (Vector2Int Cluster in GraveClusterCenters)
        {
            GraveClusterPulse(Cluster[0] - 1, Cluster[1], 80, false, false, true, ref MapLayout);
            GraveClusterPulse(Cluster[0], Cluster[1] + 1, 80, true, false, true, ref MapLayout);
            GraveClusterPulse(Cluster[0], Cluster[1] - 1, 80, false, true, true, ref MapLayout);
            GraveClusterPulse(Cluster[0] + 1, Cluster[1], 80, true, true, false, ref MapLayout);
        }

        // A final run through the grave that spawns a small number of graves detached from the paths
        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardWidth; j++)
            {
                bool SpawnGrave = Random.Range(0, 100) < DetachedGraveChance * 100;

                if (MapLayout[i, j] == TileType.GRASS && SpawnGrave && !IsAdjacent(i, j, TileType.FENCE, MapLayout))
                {
                    MapLayout[i, j] = TileType.GRAVE;
                    GraveList.Add(new Vector2Int(i, j));
                }
            }
        }
    }

    /// <summary>
    /// Turns a specified number of graves into grave clusters.
    /// </summary>
    private void GenerateGraveClusters()
    {
        // Randomly chooses a grave to be a grave cluster and be the starting point for searching for more
        int FirstSpot = Random.Range(0, GraveList.Count);

        GraveClusterCenters.Add(GraveList[FirstSpot]);

        // All grave clusters should be a decent distance from one another
        // If the generation is having a hard time finding viable spots, the minimum distance will slowly shrink by increasing leniency
        float Leniency = 0;

        // The minimum distance between grave clusters is determined by the size of the graveyard devided by number of clusters
        float DefaultDist = Mathf.Max(GraveyardLength, GraveyardWidth) / Mathf.Max(GraveClusters - 1, 1);

        // Keep cluster amount small
        while (GraveClusterCenters.Count < GraveClusters)
        {
            // Checks a random grave in the grave list
            int CheckedSpot = Random.Range(0, GraveList.Count);

            bool Viable = true;

            // Makes sure they are the minimum distance from all other clusters
            foreach (Vector2Int Cluster in GraveClusterCenters)
            {
                if (GetDistance(Cluster, GraveList[CheckedSpot]) <= DefaultDist - Leniency)
                {
                    Viable = false;
                }
            }

            // If check failed, increase leniency slightly
            if (Viable)
            {
                GraveClusterCenters.Add(GraveList[CheckedSpot]);
                Leniency = 0;
            }
            else
            {
                Leniency += 0.1f;
            }
        }
    }

    /// <summary>
    /// Recursively adds graves around the grave cluster center.
    /// </summary>
    /// <param name="XPos">
    /// The current x position in the map array.
    /// </param>
    /// <param name="ZPos">
    /// The current z position in the map array.
    /// </param>
    /// <param name="Chance">
    /// The chance a grave will spawn on this tile, out of 100. Recursive calls will end once this reaches 50.
    /// </param>
    /// <param name="PulseUp">
    /// Whether this function will call itself recursively up.
    /// </param>
    /// <param name="PulseDown">
    /// Whether this function will call itself recursively down.
    /// </param>
    /// <param name="PulseLeft">
    /// Whether this function will call itself recursively left. If false, it will call itself recursively right.
    /// </param>
    private void GraveClusterPulse(int XPos, int ZPos, int Chance, bool PulseUp, bool PulseDown, bool PulseLeft, ref TileType[,] MapLayout)
    {
        // If checked tile is out of bounds, quit immediatly
        if (XPos >= GraveyardWidth || XPos < 0 || ZPos >= GraveyardLength || ZPos < 0 || MapLayout[XPos, ZPos] == TileType.PATH)
        {
            return;
        }

        // Randomly decides if the spot should have a grave
        int RandInt = Random.Range(1, 101);
        if (MapLayout[XPos, ZPos] == TileType.GRASS && RandInt < Chance)
        {
            MapLayout[XPos, ZPos] = TileType.GRAVE;
            GraveList.Add(new Vector2Int(XPos, ZPos));
        }

        // Uses recursion to continue to nearby spots
        if (Chance > 50)
        {
            if (PulseUp && ZPos + 1 < GraveyardLength)
            {
                GraveClusterPulse(XPos, ZPos + 1, Chance - 30, true, false, PulseLeft, ref MapLayout);
            }

            if (PulseDown && ZPos - 1 >= 0)
            {
                GraveClusterPulse(XPos, ZPos - 1, Chance - 30, false, true, PulseLeft, ref MapLayout);
            }

            if (PulseLeft && XPos - 1 >= 0)
            {
                GraveClusterPulse(XPos - 1, ZPos, Chance - 30, false, false, true, ref MapLayout);
            }
            else if (!PulseLeft && XPos + 1 < GraveyardWidth)
            {
                GraveClusterPulse(XPos + 1, ZPos, Chance - 30, false, false, false, ref MapLayout);
            }
        }
    }

    /// <summary>
    /// Creates a list of all viable tiles for spawning a generic decoration using passed in params.
    /// </summary>
    /// <param name="XSize">
    /// The width of the decoration in tiles.
    /// </param>
    /// <param name="ZSize">
    /// The length of the decoration in tiles.
    /// </param>
    /// <param name="PathAdjacentAllowed">
    /// Whether the decoration can spawn adjacent to a path or not.
    /// </param>
    /// <param name="AllowGraveOverwrite">
    /// Allows the decoration to overwrite 1 grave tile if the decoration is larger than 1x1.
    /// </param>
    /// <returns>
    /// A list of all valid spawn positions for the decoration tile.
    /// </returns>
    private List<Vector2Int> GenericValidityArray(int XSize, int ZSize, bool PathAdjacentAllowed, bool AllowGraveOverwrite, TileType[,] MapLayout)
    {
        List<Vector2Int> ValidityArray = new List<Vector2Int>();

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                bool Valid = true;

                if (MapLayout[i, j] != TileType.GRASS)
                {
                    Valid = false;
                }
                if (Valid && (XSize > 1 || ZSize > 1))
                {
                    int Leniancy = 0;
                    for (int k = 0; k < XSize; k++)
                    {
                        for (int l = 0; l < ZSize; l++)
                        {
                            if (i + k >= GraveyardWidth || j + l >= GraveyardLength || (!PathAdjacentAllowed && IsAdjacent(i + k, j + l, TileType.PATH, MapLayout)))
                            {
                                Valid = false;
                                break;
                            }
                            else if (MapLayout[i + k, j + l] != TileType.GRASS)
                            {
                                if (MapLayout[i + k, j + l] == TileType.GRAVE && AllowGraveOverwrite && Leniancy == 0)
                                {
                                    Leniancy++;
                                }
                                else
                                {
                                    Valid = false;
                                    break;
                                }
                            }
                        }
                        if (!Valid)
                        {
                            break;
                        }
                    }
                }
                if (Valid && !PathAdjacentAllowed && IsAdjacent(i, j, TileType.PATH, MapLayout))
                {
                    Valid = false;
                }
                if (Valid)
                {
                    ValidityArray.Add(new Vector2Int(i, j));
                }
            }
        }

        return ValidityArray;
    }

    /// <summary>
    /// Will add a specified decoration tile to the map array with specified parameters.
    /// </summary>
    /// <param name="TileType">
    /// The decoration tile's tile array position.
    /// </param>
    /// <param name="MinDist">
    /// The minimum distance each of the decorations must be from one another. Distance is measured as X + Z distance.
    /// </param>
    /// <param name="MaxQty">
    /// The maximum number of the decoration that can be placed.
    /// </param>
    /// <param name="PosList">
    /// A list to hold the position of all the placed decoration.
    /// </param>
    /// <param name="XSize">
    /// The width of the decoration in tiles.
    /// </param>
    /// <param name="ZSize">
    /// The length of the decoration in tiles.
    /// </param>
    /// <param name="PathAdjacentAllowed">
    /// Whether the decoration can spawn adjacent to a path or not.
    /// </param>
    /// <param name="AllowGraveOverwrite">
    /// Allows the decoration to overwrite 1 grave tile if the decoration is larger than 1x1.
    /// </param>
    private void GenerateGenericTilePoints(TileType TileID, int MinDist, int MaxQty, List<Vector2Int> PosList, int XSize, int ZSize, bool PathAdjacentAllowed, bool AllowGraveOverwrite, ref TileType[,] MapLayout)
    {
        List<Vector2Int> ValidTiles = GenericValidityArray(XSize, ZSize, PathAdjacentAllowed, AllowGraveOverwrite, MapLayout);
        int RandOffset = Random.Range(0, ValidTiles.Count);

        for (int i = 0; i < ValidTiles.Count; i++)
        {
            Vector2Int CheckedTile = ValidTiles[(i + RandOffset) % ValidTiles.Count];
            bool SpawnDeco = Random.Range(0, 100) < 25;

            if (SpawnDeco)
            {
                bool Valid = true;
                foreach (Vector2Int ExistingTile in PosList)
                {
                    //Debug.Log(ExistingTile[0] + ", " + ExistingTile[1] + " -> " + CheckedTile[0] + ", " + CheckedTile[1] + ": " + GetDistance(CheckedTile, ExistingTile) + "/" + GetDistance(ExistingTile, CheckedTile));
                    if (GetDistance(CheckedTile, ExistingTile) < MinDist)
                    {
                        Valid = false;
                        break;
                    }
                }
                if (Valid)
                {
                    PosList.Add(CheckedTile);
                    for (int k = 0; k < XSize; k++)
                    {
                        for (int l = 0; l < ZSize; l++)
                        {
                            if (MapLayout[CheckedTile[0] + k, CheckedTile[1] + l] == TileType.GRAVE)
                            {
                                Debug.Log("Grave overwrite at " + (CheckedTile[0] + k) + ", " + (CheckedTile[1] + l));
                                Vector2Int GraveTile = new Vector2Int(CheckedTile[0] + k, CheckedTile[l] + 1);
                                var ItemToRemove = GraveList.SingleOrDefault(r => EqualTiles(r, GraveTile));
                                GraveList.Remove(ItemToRemove);
                            }
                            MapLayout[CheckedTile[0] + k, CheckedTile[1] + l] = TileID;
                        }
                    }
                    if (PosList.Count >= MaxQty)
                    {
                        return; 
                    }
                    i = 0;
                    RandOffset = Random.Range(0, ValidTiles.Count);
                }
            }
        }
    }

    /// <summary>
    /// Returns a list of all corners in the paths that can be used for guard routes, Counts dead end as a corner.
    /// </summary>
    /// <returns>
    /// A list of all corner points.
    /// </returns>
    private List<Vector2Int> PathCornerPoints()
    {
        List<Vector2Int> Corners = new List<Vector2Int>();

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardWidth; j++)
            {
                if (TileDatas[i, j].TileType == TileType.PATH &&
                    ((i > 0 && TileDatas[i - 1, j].TileType == TileType.PATH) || (i < GraveyardWidth - 1 && TileDatas[i + 1, j].TileType == TileType.PATH)) &&
                    ((j > 0 && TileDatas[i, j - 1].TileType == TileType.PATH) || (j < GraveyardLength - 1 && TileDatas[i, j + 1].TileType == TileType.PATH)) &&
                    GetDistance(GraveyardEntrancePos, new Vector2Int(i, j)) >= SpawnProtectionDistance) // More comments
                {
                    Vector2Int Temp = new Vector2Int(i, j);
                    Corners.Add(Temp);
                }
                else if (TileDatas[i, j].TileType == TileType.PATH) // Checking for dead ends
                {
                    int EndCheck = 0;
                    if (i > 0 && TileDatas[i - 1, j].TileType == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (i < GraveyardWidth - 1 && TileDatas[i + 1, j].TileType == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (j > 0 && TileDatas[i, j - 1].TileType == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (j < GraveyardLength - 1 && TileDatas[i, j + 1].TileType == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (EndCheck == 1 && GetDistance(GraveyardEntrancePos, new Vector2Int(i, j)) >= SpawnProtectionDistance) // Change distance from spawn to a variable
                    {
                        Vector2Int Temp = new Vector2Int(i, j);
                        Corners.Add(Temp);
                    }
                }
            }
        }

        return Corners;
    }

    private List<Vector2Int> PathCornerPoints(TileType[,] MapLayout)
    {
        List<Vector2Int> Corners = new List<Vector2Int>();

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardWidth; j++)
            {
                if (MapLayout[i, j] == TileType.PATH &&
                    ((i > 0 && MapLayout[i - 1, j] == TileType.PATH) || (i < GraveyardWidth - 1 && MapLayout[i + 1, j] == TileType.PATH)) &&
                    ((j > 0 && MapLayout[i, j - 1] == TileType.PATH) || (j < GraveyardLength - 1 && MapLayout[i, j + 1] == TileType.PATH)) &&
                    GetDistance(GraveyardEntrancePos, new Vector2Int(i, j)) >= SpawnProtectionDistance) // More comments
                {
                    Vector2Int Temp = new Vector2Int(i, j);
                    Corners.Add(Temp);
                }
                else if (MapLayout[i, j] == TileType.PATH) // Checking for dead ends
                {
                    int EndCheck = 0;
                    if (i > 0 && MapLayout[i - 1, j] == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (i < GraveyardWidth - 1 && MapLayout[i + 1, j] == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (j > 0 && MapLayout[i, j - 1] == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (j < GraveyardLength - 1 && MapLayout[i, j + 1] == TileType.PATH)
                    {
                        EndCheck++;
                    }
                    if (EndCheck == 1 && GetDistance(GraveyardEntrancePos, new Vector2Int(i, j)) >= SpawnProtectionDistance) // Change distance from spawn to a variable
                    {
                        Vector2Int Temp = new Vector2Int(i, j);
                        Corners.Add(Temp);
                    }
                }
            }
        }

        return Corners;
    }

    /// <summary>
    /// Checks if the passed in point is a corner / split in the path.
    /// </summary>
    /// <param name="Point">
    /// The point to be checked.
    /// </param>
    /// <returns>
    /// Returns true if checked point is corner, false otherwise.
    /// </returns>
    private bool CheckCornerPoint(Vector2Int Point)
    {
        int i = Point[0];
        int j = Point[1];

        if (TileDatas[i, j].TileType == TileType.PATH &&
            ((i > 0 && TileDatas[i - 1, j].TileType == TileType.PATH) || (i < GraveyardWidth - 1 && TileDatas[i + 1, j].TileType == TileType.PATH)) &&
            ((j > 0 && TileDatas[i, j - 1].TileType == TileType.PATH) || (j < GraveyardLength - 1 && TileDatas[i, j + 1].TileType == TileType.PATH)))
        {
            return true;
        }

        return false;
    }

    // Checks if two points are connected by path tiles. Assumes the start and end points are path tiles. Returns false if one of the path
    // tiles between the two points are a corner point.
    public bool CheckConnected(Vector2Int StartPos, Vector2Int EndPos)
    {
        Vector2Int Direction = new Vector2Int(EndPos[0] - StartPos[0], EndPos[1] - StartPos[1]);

        if (!(Direction[0] == 0 ^ Direction[1] == 0))
        {
            return false;
        }

        Direction[0] = Direction[0] < 0 ? -1 : (Direction[0] > 0 ? 1 : 0);
        Direction[1] = Direction[1] < 0 ? -1 : (Direction[1] > 0 ? 1 : 0);

        Vector2Int CurrentPos = StartPos;
        CurrentPos[0] += Direction[0];
        CurrentPos[1] += Direction[1];

        while (CurrentPos[0] != EndPos[0] || CurrentPos[1] != EndPos[1])
        {
            if (CurrentPos[0] >= GraveyardWidth || CurrentPos[0] < 0 ||
                CurrentPos[1] >= GraveyardLength || CurrentPos[1] < 0 ||
                TileDatas[CurrentPos[0], CurrentPos[1]].TileType != TileType.PATH ||
                CheckCornerPoint(CurrentPos))
            {
                return false;
            }

            CurrentPos[0] += Direction[0];
            CurrentPos[1] += Direction[1];
        }

        return true;
    }

    /// <summary>
    /// Gets distance between two tiles on the map (gives x distance + z distance).
    /// </summary>
    /// <param name="TileOne">
    /// The first tile.
    /// </param>
    /// <param name="TileTwo">
    /// The second tile.
    /// </param>
    /// <returns>
    /// The combined x and z distance of the two tiles.
    /// </returns>
    public int GetDistance(Vector2Int TileOne, Vector2Int TileTwo)
    {
        return Mathf.Abs(TileOne[0] - TileTwo[0]) + Mathf.Abs(TileOne[1] - TileTwo[1]);
    }

    /// <summary>
    /// Checks if two tiles are the same tile.
    /// </summary>
    /// <param name="TileOne">
    /// The first tile.
    /// </param>
    /// <param name="TileTwo">
    /// The second tile.
    /// </param>
    /// <returns>
    /// True if the tiles are the same, false otherwise.
    /// </returns>
    public bool EqualTiles(Vector2Int TileOne, Vector2Int TileTwo)
    {
        return GetDistance(TileOne, TileTwo) == 0;
    }

    /// <summary>
    /// Checks if specific tile position is adjacent to a specific type of tile.
    /// </summary>
    /// <param name="XPos">
    /// The x position of the checked tile.
    /// </param>
    /// <param name="ZPos">
    /// The z position of the checked tile.
    /// </param>
    /// <param name="TileType">
    /// The type of tile being checked for adjacency.
    /// </param>
    /// <returns>
    /// Returns true if tile is adjacent to the specified tile type, false otherwise.
    /// </returns>
    private bool IsAdjacent(int XPos, int ZPos, TileType TileID)
    {
        for (int i = -1; i <= 1; i += 2)
        {
            if (XPos + i >= 0 && XPos + i < GraveyardWidth
                && TileDatas[XPos + i, ZPos].TileType == TileID)
            {
                return true;
            }
        }

        for (int j = -1; j <= 1; j += 2)
        {
            if (ZPos + j >= 0 && ZPos + j < GraveyardLength
                && TileDatas[XPos, ZPos + j].TileType == TileID)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsAdjacent(int XPos, int ZPos, TileType TileID, TileType[,] MapLayout)
    {
        for (int i = -1; i <= 1; i += 2)
        {
            if (XPos + i >= 0 && XPos + i < GraveyardWidth
                && MapLayout[XPos + i, ZPos] == TileID)
            {
                return true;
            }
        }

        for (int j = -1; j <= 1; j += 2)
        {
            if (ZPos + j >= 0 && ZPos + j < GraveyardLength
                && MapLayout[XPos, ZPos + j] == TileID)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if specific tile is viable to spawn a grave on. Essentially checking if tile is adjecent to a path, but ignores the spot to the north.
    /// </summary>
    /// <param name="XPos">
    /// The x position of the checked tile.
    /// </param>
    /// <param name="ZPos">
    /// The z position of the checked tile.
    /// </param>
    /// <param name="TileType">
    /// The type of tile being checked for adjacency.
    /// </param>
    /// <returns>
    /// Returns true if tile is viable for a grave, false otherwise.
    /// </returns>
    private bool GraveViable(int XPos, int ZPos, TileType TileID)
    {
        for (int i = -1; i <= 1; i += 2)
        {
            if (XPos + i >= 0 && XPos + i < GraveyardWidth
                && TileDatas[XPos + i, ZPos].TileType == TileID)
            {
                return true;
            }
        }

        if (ZPos - 1 >= 0 && ZPos - 1 < GraveyardLength
            && TileDatas[XPos, ZPos - 1].TileType == TileID)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the passed in tile is within the graveyard bounds.
    /// </summary>
    /// <param name="XPos">
    /// The tile's x position in the graveyard, base 0.
    /// </param>
    /// <param name="ZPos">
    /// The tile's z position in the graveyard, base 0.
    /// </param>
    /// <returns>
    /// Returns true if the checked tile is within bounds, false otherwise.
    /// </returns>
    public bool SafeTileCheck(int XPos, int ZPos)
    {
        if (XPos >= 0 && XPos < GraveyardWidth && ZPos >= 0 && ZPos < GraveyardLength)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the passed in tile is of the passed in type, with added vallidation to make sure tile is within graveyard bounds,
    /// and tile type is valid.
    /// </summary>
    /// <param name="XPos">
    /// The tile's x position in the graveyard, base 0.
    /// </param>
    /// <param name="ZPos">
    /// The tile's z position in the graveyard, base 0.
    /// </param>
    /// <param name="TileType">
    /// The intended tile's type.
    /// </param>
    /// <returns>
    /// If the checked tile is the intended tile type and validation is passed, returns true, otherwise false.
    /// </returns>
    public bool SafeTileCheck(int XPos, int ZPos, TileType TileID)
    {
        if (XPos >= 0 && XPos < GraveyardWidth && ZPos >= 0 && ZPos < GraveyardLength && (int)TileID < Tiles.Length && TileDatas[XPos, ZPos].TileType == TileID)
        {
            return true;
        }
        
        return false;
    }

    public bool SafeTileCheck(int XPos, int ZPos, TileType TileID, TileType[,] MapLayout)
    {
        if (XPos >= 0 && XPos < GraveyardWidth && ZPos >= 0 && ZPos < GraveyardLength && (int)TileID < Tiles.Length && MapLayout[XPos, ZPos] == TileID)
        {
            return true;
        }

        return false;
    }

    public Vector2Int GetTilePos(Transform Pos)
    {
        Vector3 RelativePos = Pos.position - Parent.transform.position;

        // Calculating the tile's position in the array based on its transform
        int XPos = (int)Mathf.Ceil(RelativePos.x);
        XPos -= Mathf.Abs(XPos % 2);
        XPos /= 2;
        XPos += GraveyardEntrancePos[0];

        int ZPos = (int)Mathf.Ceil(RelativePos.z);
        ZPos -= Mathf.Abs(ZPos % 2);
        ZPos /= 2;
        ZPos += GraveyardEntrancePos[1];

        return new Vector2Int(XPos, ZPos);
    }

    /// <summary>
    /// Returns the tile data script of the tile at a specified transform.
    /// </summary>
    /// <param name="Pos">
    /// The transform point on top of the specified tile.
    /// </param>
    /// <returns>
    /// The tile data script of the tile at specified transform.
    /// </returns>
    public TileData GetTileData(Transform Pos)
    {
        Vector3 RelativePos = Pos.position - Parent.transform.position;

        // Calculating the tile's position in the array based on its transform
        int XPos = (int)Mathf.Ceil(RelativePos.x);
        XPos -= Mathf.Abs(XPos % 2);
        XPos /= 2;
        XPos += GraveyardEntrancePos[0];

        int ZPos = (int)Mathf.Ceil(RelativePos.z);
        ZPos -= Mathf.Abs(ZPos % 2);
        ZPos /= 2;
        ZPos += GraveyardEntrancePos[1];

        return TileDatas[XPos, ZPos];
    }

    /// <summary>
    /// Returns the tile data script of the tile at the specified location.
    /// </summary>
    /// <param name="XPos">
    /// The x position of the tile.
    /// </param>
    /// <param name="ZPos">
    /// The z position of the tile.
    /// </param>
    /// <returns>
    /// The tile data script of the specified tile.
    /// </returns>
    public TileData GetTileData(int XPos, int ZPos)
    {
        return TileDatas[XPos, ZPos];
    }

    public TileData GetTileData(Vector2Int TilePos)
    {
        return GetTileData(TilePos.x, TilePos.y);
    }

    public GameObject GetTile(int XPos, int ZPos)
    {
        return TileDatas[XPos, ZPos].gameObject;
    }

    public int GetGraveCount()
    {
        return GraveList.Count;
    }

    public List<GameObject> GetGuardList()
    {
        return GuardList;
    }

    public int GetGraveyardWidth()
    {
        return GraveyardWidth;
    }

    public int GetGraveyardLength()
    {
        return GraveyardLength;
    }

    public List<Vector2Int> GetCornerPoints()
    {
        return GuardPathCorners;
    }
}
