using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class TutorialMapGen : MapGeneration
{
    // Mentor game object
    public GameObject MentorPrefab;
    private GameObject Mentor;
    public GameObject InvisibleWallPrefab;

    public Vector2Int GuardResetPointOne; // Point the player will warp to upon being found by first guard.
    public Vector2Int GuardResetPointTwo; // Point the player will warp to upon being found by second guard.

    public GameObject GraveArrow; // An arrow which points down to graves

    /// <summary>
    /// Alternate start that hard codes necessary elements for the tutorial graveyard.
    /// </summary>
    private void Start()
    {
        StealthManager.Instance.SetMG(this);

        UIManager.Instance.MedallionOff();

        Parent = gameObject.transform.parent.gameObject;
        NMSurface = Parent.GetComponent<NavMeshSurface>();

        ImageLoader il = new ImageLoader();

        GraveyardWidth = MapImage.width;
        GraveyardLength = MapImage.height;

        TileType[,] MapLayout;

        MapImage = GameManager.Instance._RunManager.GetMapInfo().GetMapImage();

        // Loading map layout from passed in image
        if (!Prebuilt)
        {
            MapLayout = il.LoadMap(MapImage, MapColor);
        }
        else
        {
            MapLayout = il.PrebuiltMap(MapImage);
        }

        // Forcing graveyard entry position to path tile
        MapLayout[GraveyardEntrancePos[0], GraveyardEntrancePos[1]] = TileType.PATH;

        // Spawning in tile prefabs
        GenerateMap(MapLayout);

        // Converting graves to special tutorial versions
        ConvertGraveToTutorial(6, 8);
        ConvertGraveToTutorial(6, 4);
        ConvertGraveToTutorial(10, 8);
        ConvertGraveToTutorial(10, 4);

        ConvertGraveToTutorial(46, 8);
        ConvertGraveToTutorial(46, 4);
        ConvertGraveToTutorial(50, 8);
        ConvertGraveToTutorial(50, 4);

        // Forcing traps on second set of graves
        TileDatas[46, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().ForceTraps(new bool[] { true, false, true, false, false });
        TileDatas[46, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().ForceTraps(new bool[] { false, true, true, false, false });
        TileDatas[50, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().ForceTraps(new bool[] { false, false, true, true, false });
        TileDatas[50, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().ForceTraps(new bool[] { false, false, true, false, true });

        // Adding arrows above the first four graves
        TileDatas[6, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[6, 8].transform.position, GraveArrow.transform.rotation));
        TileDatas[6, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[6, 4].transform.position, GraveArrow.transform.rotation));
        TileDatas[10, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[10, 8].transform.position, GraveArrow.transform.rotation));
        TileDatas[10, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[10, 4].transform.position, GraveArrow.transform.rotation));

        // Adding arrows above the second set of graves
        TileDatas[46, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[46, 8].transform.position, GraveArrow.transform.rotation));
        TileDatas[46, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[46, 4].transform.position, GraveArrow.transform.rotation));
        TileDatas[50, 8].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[50, 8].transform.position, GraveArrow.transform.rotation));
        TileDatas[50, 4].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<TutorialGrave>().SetArrow(Instantiate(GraveArrow, TileDatas[50, 4].transform.position, GraveArrow.transform.rotation));

        // Building nav mesh
        NMSurface.BuildNavMesh();

        // Hard coding guard routes
        Transform[] GuardOneRoute = new Transform[4];
        GuardOneRoute[0] = TileDatas[25, 9].transform;
        GuardOneRoute[1] = TileDatas[32, 9].transform;
        GuardOneRoute[2] = TileDatas[32, 3].transform;
        GuardOneRoute[3] = TileDatas[25, 3].transform;

        Transform[] GuardTwoRoute = new Transform[2];
        GuardTwoRoute[0] = TileDatas[64, 6].transform;
        GuardTwoRoute[1] = TileDatas[74, 6].transform;

        Transform[] GuardThreeRoute = new Transform[4];
        GuardThreeRoute[2] = TileDatas[25, 9].transform;
        GuardThreeRoute[3] = TileDatas[32, 9].transform;
        GuardThreeRoute[0] = TileDatas[32, 3].transform;
        GuardThreeRoute[1] = TileDatas[25, 3].transform;

        // Generating tutorial guards
        Vector2Int LookoutGoalsTemp = new Vector2Int(0, 0);

        GameObject TempGuardOne = (GameObject)Instantiate(Guard, GuardOneRoute[0].position, Quaternion.identity);
        GameObject TempGuardTwo = (GameObject)Instantiate(Guard, GuardTwoRoute[0].position, Quaternion.identity);
        GameObject TempGuardThree = (GameObject)Instantiate(Guard, GuardThreeRoute[0].position, Quaternion.identity);

        TempGuardOne.GetComponent<TutorialGuard>().GenerateGuard(GuardOneRoute, LookoutGoalsTemp, GuardResetPointOne);
        TempGuardTwo.GetComponent<TutorialGuard>().GenerateGuard(GuardTwoRoute, LookoutGoalsTemp, GuardResetPointTwo);
        TempGuardThree.GetComponent<TutorialGuard>().GenerateGuard(GuardThreeRoute, LookoutGoalsTemp, GuardResetPointOne);

        GuardList.Add(TempGuardOne);
        GuardList.Add(TempGuardTwo);
        GuardList.Add(TempGuardThree);

        // Creating mentor
        Mentor = (GameObject)Instantiate(MentorPrefab, TileDatas[5, 6].transform.position, Quaternion.identity);

        transform.GetComponent<TutorialProgression>().SetMentor(Mentor);

        // Adding invisible walls to level
        List<GameObject> InvisWalls = new List<GameObject>();
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[1, 7].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[1, 5].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[14, 6].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[22, 6].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[42, 6].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[54, 6].transform.position, Quaternion.identity));
        InvisWalls.Add((GameObject)Instantiate(InvisibleWallPrefab, TileDatas[62, 6].transform.position, Quaternion.identity));

        transform.GetComponent<TutorialProgression>().SetInvisibleWalls(InvisWalls);

        UIManager.Instance.DisableBlackScreen();
    }

    /// <summary>
    /// Uses navmesh to move mentor to passed in position.
    /// </summary>
    /// <param name="Pos">
    /// Tile coords to move the mentor to.
    /// </param>
    public void MoveMentor(Vector2Int Pos)
    {
        Mentor.GetComponent<TutorialMentor>().MoveToPosition(TileDatas[Pos.x, Pos.y].transform);
    }

    public void SetMentorTalk(bool _Val)
    {
        Mentor.GetComponent<TutorialMentor>().SetTalkSign(_Val);
    }

    /// <summary>
    /// Warps the mentor to passed in position.
    /// </summary>
    /// <param name="Pos">
    /// Tile coords to warp to.
    /// </param>
    public void TeleportMentor(Vector2Int Pos)
    {
        Mentor.GetComponent<NavMeshAgent>().Warp(TileDatas[Pos.x, Pos.y].transform.position);
    }

    /// <summary>
    /// Converts a normal grave to a hard coded tutorial version by swapping attached scripts.
    /// </summary>
    /// <param name="XPos">
    /// X coord of tile.
    /// </param>
    /// <param name="YPos">
    /// Y coord of tile.
    /// </param>
    private void ConvertGraveToTutorial(int XPos, int YPos)
    {
        TutorialGrave TempScript = TileDatas[XPos, YPos].transform.GetComponent<GraveTile>().GetTombstone().AddComponent<TutorialGrave>();
        TempScript.CopyPublicVars();
        Destroy(TileDatas[XPos, YPos].transform.GetComponent<GraveTile>().GetTombstone().GetComponent<GraveInfoRefactor>());
        TempScript.Initiate();
    }
}
