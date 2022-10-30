using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCommands : MonoBehaviour
{
    MapGeneration MG;

    // If the scene is frozen for a cutscene/dialogue
    protected bool SceneFrozen = false;

    private int UpdateTimer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        MG = transform.GetComponent<MapGeneration>();
    }

    private void Update()
    {
        UpdateTimer++;

        if (UpdateTimer % 30 == 0)
        {
            Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            PlayerPos.y = 0;

            int[] SpikeValues = new int[] { 0, 0, 0, 0 };
            foreach (GameObject G in MG.GetGuardList())
            {
                Vector3 TempPos = G.transform.position;
                TempPos.y = 0;

                float TempDist = Vector3.Distance(PlayerPos, TempPos);
                if (TempDist <= 15.0f)
                {
                    Direction TempDir;
                    TempPos = TempPos - PlayerPos;
                    TempPos = TempPos.normalized;

                    if (Mathf.Abs(TempPos.x) >= 1 / Mathf.Sqrt(2))
                    {
                        if (TempPos.x >= 0)
                        {
                            TempDir = Direction.RIGHT;
                        }
                        else
                        {
                            TempDir = Direction.LEFT;
                        }
                    }
                    else
                    {
                        if (TempPos.z >= 0)
                        {
                            TempDir = Direction.UP;
                        }
                        else
                        {
                            TempDir = Direction.DOWN;
                        }
                    }

                    if (TempDist <= 5.5f)
                    {
                        if (SpikeValues[(int)TempDir] < 3)
                        {
                            UIManager.Instance.SetSpike(TempDir, 3);
                            SpikeValues[(int)TempDir] = 3;
                        }
                    }
                    else if (TempDist <= 9.5f)
                    {
                        if (SpikeValues[(int)TempDir] < 2)
                        {
                            UIManager.Instance.SetSpike(TempDir, 2);
                            SpikeValues[(int)TempDir] = 2;
                        }
                    }
                    else
                    {
                        if (SpikeValues[(int)TempDir] < 1)
                        {
                            UIManager.Instance.SetSpike(TempDir, 1);
                            SpikeValues[(int)TempDir] = 1;
                        }
                    }
                }
            }

            if (SpikeValues[0] == 0)
            {
                UIManager.Instance.SetSpike(Direction.UP, 0);
            }
            if (SpikeValues[1] == 0)
            {
                UIManager.Instance.SetSpike(Direction.LEFT, 0);
            }
            if (SpikeValues[2] == 0)
            {
                UIManager.Instance.SetSpike(Direction.RIGHT, 0);
            }
            if (SpikeValues[3] == 0)
            {
                UIManager.Instance.SetSpike(Direction.DOWN, 0);
            }
        }
    }

    // Freezes all action in the current scene, use before dialogue
    public void FreezeScene()
    {
        foreach (GameObject G in MG.GetGuardList())
        {
            G.GetComponent<Guard>().PauseGuard();
        }

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Player.GetComponent<Player>().SetPaused(true);
        }

        SceneFrozen = true;
    }

    public void UnFreezeScene()
    {
        foreach (GameObject G in MG.GetGuardList())
        {
            G.GetComponent<Guard>().UnPauseGuard();
        }

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Player.GetComponent<Player>().SetPaused(false);
        }

        SceneFrozen = false;
    }

    public bool GetSceneFrozen()
    {
        return SceneFrozen;
    }

    /// <summary>
    /// Move player to passed in tile. Fades screen out and back in.
    /// </summary>
    /// <param name="Pos">
    /// The x/y position of the tile to teleport to.
    /// </param>
    /// <returns></returns>
    public IEnumerator TeleportPlayer(Vector2Int Pos)
    {
        UIManager.Instance.EnableBlackScreen();
        yield return new WaitForSeconds(1.0f);

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player != null)
        {
            Vector3 TelePos = MG.GetTile(Pos.x, Pos.y).transform.position;
            TelePos.y = Player.transform.position.y;
            Player.transform.position = TelePos;
        }

        yield return new WaitForSeconds(0.5f);

        UIManager.Instance.DisableBlackScreen();
    }

    /// <summary>
    /// Decides the tier of grave to be spawned based on map gen settings.
    /// </summary>
    /// <returns>
    /// A randomized tier for the grave based on map information.
    /// </returns>
    public int RandomizeGraveTier(Vector2Int TilePos, TileType[,] MapLayout)
    {
        // Hard coding tutorial grave type
        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.TUTORIAL)
        {
            return 0;
        }

        float HighChance = 0;
        float MedChance = 0;

        // Base spawn chances based on graveyard tier
        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.HIGH)
        {
            HighChance = 25.0f;
            MedChance = 60.0f;
        }
        else if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.MEDIUM)
        {
            HighChance = 10.0f;
            MedChance = 40.0f;
        }
        else
        {
            MedChance = 20.0f;
        }

        // Lowering RNG if surrounded by other graves
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (MG.SafeTileCheck(TilePos.x + i, TilePos.y + j, TileType.GRAVE, MapLayout))
                {
                    HighChance -= 1.5f;
                    MedChance *= 0.85f;
                }
            }
        }

        int Result = 0;// The variable to be passed back. Index of the correct headstone

        // 0-2 are low tier, 3-5 are mid tier, 6-8 are high tier
        if (Random.Range(0, 100) < HighChance)
        {
            Result = 6;
        }
        else if (Random.Range(0, 100) < MedChance)
        {
            Result = 3;
        }

        Result += Random.Range(0, 3);

        return Result;
    }

    /// <summary>
    /// Generates a random amount of money for a guard to ask the player as a bribe
    /// </summary>
    /// <returns></returns>
    public int GetRandomBribe()
    {
        int BribeAmount = 0;

        switch (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier())
        {
            case GraveyardTier.LOW:
                BribeAmount = Random.Range(15, 25);
                break;
            case GraveyardTier.MEDIUM:
                BribeAmount = Random.Range(35, 55);
                break;
            case GraveyardTier.HIGH:
                BribeAmount = Random.Range(70, 110);
                break;
            default:
                BribeAmount = 1;
                break;
        }

        if (StealthManager.Instance.GetAlertLevel() < 100)
        {
            BribeAmount = (int)(BribeAmount * 0.8f);
        }
        else if (StealthManager.Instance.GetAlertLevel() >= 200 && StealthManager.Instance.GetAlertLevel() < 300)
        {
            BribeAmount = (int)(BribeAmount * 1.2f);
        }
        else if (StealthManager.Instance.GetAlertLevel() >= 300)
        {
            BribeAmount = (int)(BribeAmount * 1.5f);
        }

        return BribeAmount;
    }

    public Vector2Int GetNearestCorner(Vector2Int TilePos)
    {
        List<Vector2Int> Corners = MG.GetCornerPoints();

        Vector2Int ReturnCorner = new Vector2Int();
        int MinDist = int.MaxValue;

        foreach (Vector2Int C in Corners)
        {
            if (MG.GetDistance(TilePos, C) < MinDist)
            {
                MinDist = MG.GetDistance(TilePos, C);
                ReturnCorner = C;
            }
        }

        return ReturnCorner;
    }

    /// <summary>
    /// Checks for all corner points connected to the current point, excluding the previous corner.
    /// Will return only the previous corner if there are no other connections available.
    /// </summary>
    /// <param name="CurrentTile">
    /// The tile to check from.
    /// </param>
    /// <param name="PreviousTile">
    /// The previous corner point in a path.
    /// </param>
    /// <returns>
    /// A list of all viable pathing options.
    /// </returns>
    public List<Vector2Int> PathingOptions(Vector2Int CurrentTile, Vector2Int PreviousTile)
    {
        List<Vector2Int> GuardPathCorners = MG.GetCornerPoints();
        List<Vector2Int> Options = new List<Vector2Int>();
        for (int i = 0; i < GuardPathCorners.Count; i++)
        {
            if (MG.CheckConnected(CurrentTile, GuardPathCorners[i]) && !MG.EqualTiles(PreviousTile, GuardPathCorners[i]))
            {
                Options.Add(GuardPathCorners[i]);
            }
        }

        if (Options.Count == 0)
        {
            Options.Add(PreviousTile);
        }

        return Options;
    }

    /// <summary>
    /// Uses a shortest path algorithm to find what corner point to move to next in order to approach a goal.
    /// </summary>
    /// <param name="StartPos">
    /// The tile to start from. Must be a corner tile.
    /// </param>
    /// <param name="GoalPos">
    /// The goal. Must be a corner tile.
    /// </param>
    /// <returns>
    /// The tile to move to next. Returns (-1, -1) if the start position is already at the goal, or the goal isn't found within 99 checks.
    /// </returns>
    public Vector2Int GetShortestPath(Vector2Int StartPos, Vector2Int GoalPos)
    {
        Vector2Int ReturnVector = new Vector2Int(-1, -1);

        if (StartPos.x < 0 || StartPos.y < 0 || GoalPos.x < 0 || GoalPos.y < 0)
        {
            return ReturnVector;
        }

        bool[,] VisitArray = new bool[MG.GetGraveyardWidth(), MG.GetGraveyardLength()]; // An array which checks which corners you've already visited so that they're ignored in further checks
        VisitArray[StartPos.x, StartPos.y] = true;

        List<PathTile> CornerList = new List<PathTile>(); // List of corners currently in the running
        List<Vector3Int> DistanceList = new List<Vector3Int>(); // Holds the first tile in each corners path, as well as its total distance

        CornerList.Add(MG.GetTile(StartPos.x, StartPos.y).GetComponent<PathTile>()); // Adding the starting tile
        DistanceList.Add(new Vector3Int(-1, -1, 0)); // Adding a dummy position for the starting tile to be replaced later

        int Repititions = 0; // Holds how many checks have been made to prevent infinite loop

        bool PathFound = false; // If the correct path has been found

        while (!PathFound && Repititions <= 99)
        {
            Repititions++;

            int TempShortest = int.MaxValue; // Setting up shortest path algorithm

            int TempWinner = -1; // Holds the winning tile's position in the list

            Vector3Int NextCorner = new Vector3Int(-1, -1, -1); // Holds the position of the next shortest corner

            // Each path corner tile holds a list of all other corners it's connected to. This loops through them and finds
            // which one will lead to the next shortest path.
            for (int i = 0; i < CornerList.Count; i++)
            {
                for (int j = 0; j < CornerList[i].GetCornerConnections().Count; j++)
                {
                    Vector3Int CheckedCorner = CornerList[i].GetCornerConnections()[j];
                    int CheckedDistance = DistanceList[i].z + CheckedCorner.z;

                    // If the corner tile hasn't been visited yet, and has a smaller distance then the previously checked ones
                    if (!VisitArray[CheckedCorner.x, CheckedCorner.y] && CheckedDistance < TempShortest)
                    {
                        TempWinner = i;
                        TempShortest = CheckedDistance;
                        NextCorner = CheckedCorner;
                    }
                }
            }

            // If at least one tile to visit was found
            if (TempWinner != -1)
            {
                VisitArray[NextCorner.x, NextCorner.y] = true;

                bool KeepCorner = false;

                // Checks the corner we just moved from to see if any of its connections have yet to be visited. If it has none left
                // it will be removed from the list.
                for (int j = 0; j < CornerList[TempWinner].GetCornerConnections().Count; j++)
                {
                    Vector3Int CheckedCorner = CornerList[TempWinner].GetCornerConnections()[j];
                    if (!VisitArray[CheckedCorner.x, CheckedCorner.y])
                    {
                        KeepCorner = true;
                    }
                }

                // If it does have at least one corner left to visit, a copy will be added to the end of the list.
                if (KeepCorner)
                {
                    CornerList.Add(CornerList[TempWinner]);
                    DistanceList.Add(DistanceList[TempWinner]);
                }

                // The winning tile is replaced with the tile moved to in the path
                CornerList[TempWinner] = MG.GetTile(NextCorner.x, NextCorner.y).GetComponent<PathTile>();

                // Setting the distance vector of the new tile
                Vector3Int EditVector = DistanceList[TempWinner];
                EditVector.z = TempShortest;
                if (EditVector.x == -1)
                {
                    EditVector.x = NextCorner.x;
                    EditVector.y = NextCorner.y;
                }

                DistanceList[TempWinner] = EditVector;

                // Checking if the new tile is the goal tile
                if (NextCorner.x == GoalPos.x && NextCorner.y == GoalPos.y)
                {
                    PathFound = true;
                    ReturnVector = new Vector2Int(DistanceList[TempWinner].x, DistanceList[TempWinner].y);
                }
            }
        }

        return ReturnVector;
    }
}
