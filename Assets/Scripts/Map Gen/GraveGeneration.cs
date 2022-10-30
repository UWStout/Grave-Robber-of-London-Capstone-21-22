using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// body generation takes inputs from grave info then generates the information for the 
/// grave. This script takes in the grave quality and spits on what traps are on the grave and how
/// good the body is. Along with generating the amount of tressure.
/// </summary>
public class GraveGeneration : MonoBehaviour
{
    //holds references to the mini games
    public GameObject TripWireTrap;
    public GameObject CoffinCageTrap;
    public GameObject DiggingTrap;
    public GameObject LockPickTrap;
    public GameObject CoffinMortar;
    public Canvas Canvas;

    GameObject TripWireInstance;
    GameObject CoffinCageTrapInstance;
    GameObject DiggingTrapInstance;
    GameObject LockPickInstance;
    GameObject CoffinMortarInstance;

    public bool CoffinCageActive = false;

    //stores body prams 
    [SerializeField] Body GraveBody;

    //random number to see if tresure is in grave
    int TreasureNumber;
    //amount of Treasure in a grave
    public int TreasureAmount = 0;
    //holds the body decay level higher is better
    public int DecayLevelBody;

    //the chance one of those traps will appear
    int CageChance = 0;
    int TorpeadoChance = 0;
    int TripWireChance = 0;
    int LockPickChance = 0;

    //array that store what traps are active in standard form Tripwire = false, digging = true, cage = false, lockpick = false, torpeado = false 
    public bool[] Traps = new bool[] { false, false, true, false, false };
    public List<GameObject> MiniGameList = new List<GameObject>();
    /*
     * GraveTier allows the map generator to chose what level of body top spawn
     * this is done by passing in a string 
     * of Low, Mid, High or None
     * Low generates a low tier grave
     * Mid generates a mid tier grave 
     * High Generates a high tier grave 
     * None if we want it too random a grave, which currently doesnt do anything 
     */

    /// <summary>
    /// over loaded operator to generate a default grave
    /// </summary>
    /// <param name="GraveTier"> is the overlaoded operator</param>
    public Body GenerateGrave(GraveQuality GraveTier) //factory object 
    {
        return GenerateGrave(GraveTier, new bool[] { false, false, true, false, false });
    }
    /// <summary>
    /// This is the main part of this script. it calls the correct grave tier and makes the grave
    /// </summary>
    /// <param name="GraveTier">generates a grave based on the given tier</param>
    /// <param name="ForceTrap">allows the traps to be forced with an array of bools</param>
    public Body GenerateGrave(GraveQuality GraveTier, bool[] ForceTrap)
    {
        foreach (GameObject obj in MiniGameList)
        {
            Destroy(obj);
        }

        MiniGameList = new List<GameObject>();

        //MiniGameList = new List<GameObject>();
        //calls the low tier grave class
        if (GraveTier == GraveQuality.LOW)
        {
            GenerateLowTierBody(ForceTrap);
        }
        //calls the mid tier grave script
        if (GraveTier == GraveQuality.MEDIUM)
        {
            GenerateMidTierBody(ForceTrap);
        }
        //calls the high tier glove
        if (GraveTier == GraveQuality.HIGH)
        {
            GenerateHighTierBody(ForceTrap);
        }
        // is suppose to handle mistakes current does nothing but it will later
        //will likely return a low tier grave
        if (GraveTier == GraveQuality.NULL)
        {
            //random a grave 
        }
        
        return new Body(DecayLevelBody);
    }
    /// <summary>
    /// generates a low tier grave with all the important parts 
    /// decay level, traps and type of body
    /// </summary>
    /// <param name="ForceTrap">allows you to force graves with an array of bools</param>
    void GenerateLowTierBody(bool[] ForceTrap)
    {
        //forces the tripwire to appear 
        if (ForceTrap[0] == true) 
        {
            Traps[0] = true;
            TripWireInstance = Instantiate(TripWireTrap, Canvas.transform);
            MiniGameList.Add(TripWireInstance); 
        }
        //forces the cage to appear 
        if (ForceTrap[1] == true)
        {
            Traps[1] = true;
            CoffinCageTrapInstance = Instantiate(CoffinCageTrap, Canvas.transform);
            MiniGameList.Add(CoffinCageTrapInstance);
            CoffinCageActive = true;
        }
        //forces digging trap
        if (ForceTrap[2] == true)
        {
            Traps[2] = true;
            DiggingTrapInstance = Instantiate(DiggingTrap, Canvas.transform);
            MiniGameList.Add(DiggingTrapInstance);
        }
        //forces the lock pick trap to appear 
        if (ForceTrap[3] == true)
        {
            Traps[3] = true;
            LockPickInstance = Instantiate(LockPickTrap, Canvas.transform);
            MiniGameList.Add(LockPickInstance); 
        }
        //forces the torpeado to appear 
        if (ForceTrap[4] == true)
        {
            Traps[4] = true;
            CoffinMortarInstance = Instantiate(CoffinMortar, Canvas.transform);
            MiniGameList.Add(CoffinMortarInstance);
        }
        //determins the body decay
        DecayLevelBody = Random.Range(0, 51);
    }
    /// <summary>
    /// enerates a mid tier grave with all the important parts 
    /// decay level, traps and type of body
    /// </summary>
    /// <param name="ForceTrap">allows you to force a trap with an array of bools</param>
    void GenerateMidTierBody(bool[] ForceTrap)
    {
        //determins body quality 
        DecayLevelBody = Random.Range(25, 76);

        TreasureAmount = Random.Range(0, 2);

        if (DecayLevelBody >= 80)
        {
            //tripwire chance 
            TripWireChance = Random.Range(0, 101);
            if (TripWireChance >= 20 || ForceTrap[0] == true) //also allow it to be forced 
            {
                Traps[0] = true;
                TripWireInstance = Instantiate(TripWireTrap, Canvas.transform);
                MiniGameList.Add(TripWireInstance);
                TreasureAmount += Random.Range(0, 2);
            }
        }

        //chance a cage apepars 
        CageChance = Random.Range(0, 101);
        if (CageChance >= 50 || ForceTrap[1] == true) //also allow it to be forced 
        {
            Traps[1] = true;
            CoffinCageTrapInstance = Instantiate(CoffinCageTrap, Canvas.transform);
            MiniGameList.Add(CoffinCageTrapInstance);
            CoffinCageActive = true;
            TreasureAmount += Random.Range(0, 2);
        }

        //forces digging trap
        if (ForceTrap[2] == true)
        {
            Traps[2] = true;
            DiggingTrapInstance = Instantiate(DiggingTrap, Canvas.transform);
            MiniGameList.Add(DiggingTrapInstance);
        }
        LockPickChance = Random.Range(0, 101);
        //forces the lock pick trap to appear 
        if (ForceTrap[3] == true || LockPickChance <= 10)
        {
            Traps[3] = true;
            TreasureAmount += 6;
            LockPickInstance = Instantiate(LockPickTrap, Canvas.transform);
            MiniGameList.Add(LockPickInstance);
        }

        //chance for torpeado trap to be on a medium grave
        TorpeadoChance = Random.Range(0, 101);
        if (TorpeadoChance >= 50 || ForceTrap[4] == true) //also allow it to be forced 
        {
            Traps[4] = true;
            TreasureAmount += Random.Range(0, 2);
            CoffinMortarInstance = Instantiate(CoffinMortar, Canvas.transform);
            MiniGameList.Add(CoffinMortarInstance);
        }

        //determines the amount of treaure before bonus
        TreasureNumber = Random.Range(0, 101);
        if (TreasureNumber >= 80)
        {
            TreasureAmount =+ Random.Range(1, 3);
        }
    }
    /// <summary>
    /// enerates a high tier grave with all the important parts 
    /// decay level, traps and type of body
    /// </summary>
    /// <param name="ForceTrap">allows you to force traps with an array of bools</param>
    void GenerateHighTierBody(bool[] ForceTrap)
    {
        //determines the decay level
        DecayLevelBody = Random.Range(50, 101);

        int TreasureBonusOne = Random.Range(1, 4);
        int TreasureBonusTwo = Random.Range(1, 4);

        TreasureAmount = Mathf.Max(TreasureBonusOne, TreasureBonusTwo);

        //tripwire chance 
        TripWireChance = Random.Range(0, 101);
        if (DecayLevelBody >= 50)
        {
            //tripwire chance 
            if (TripWireChance >= 20 || ForceTrap[0] == true) //also allow it to be forced 
            {
                Traps[0] = true;
                TripWireInstance = Instantiate(TripWireTrap, Canvas.transform);
                MiniGameList.Add(TripWireInstance);
                TreasureAmount += Random.Range(0, 3);
            }
        }
        //chance a cage apepars 
        CageChance = Random.Range(0, 101);
        if (CageChance >= 40 || ForceTrap[1] == true) //also allow it to be forced 
        {
            Traps[1] = true;
            TreasureAmount += Random.Range(0, 3);
            CoffinCageTrapInstance = Instantiate(CoffinCageTrap, Canvas.transform);
            MiniGameList.Add(CoffinCageTrapInstance);
            CoffinCageActive = true;
        }
        //forces digging trap
        if (ForceTrap[2] == true)
        {
            Traps[2] = true;
            DiggingTrapInstance = Instantiate(DiggingTrap, Canvas.transform);
            MiniGameList.Add(DiggingTrapInstance);
        }
        LockPickChance = Random.Range(0, 101);
        //forces the lock pick trap to appear 
        if (ForceTrap[3] == true || LockPickChance <= 20)
        {
            Traps[3] = true;
            TreasureAmount += 10;
            LockPickInstance = Instantiate(LockPickTrap, Canvas.transform);
            MiniGameList.Add(LockPickInstance);
        }
        //chance for torpeado trap
        TorpeadoChance = Random.Range(0, 101);
        if (TorpeadoChance >= 50 || ForceTrap[4] == true) //also allow it to be forced 
        {
            Traps[4] = true;
            CoffinMortarInstance = Instantiate(CoffinMortar, Canvas.transform);
            MiniGameList.Add(CoffinMortarInstance);
            TreasureAmount += Random.Range(0, 3);
        }
        //determines the gold
        TreasureNumber = Random.Range(0, 101);
        if (TreasureNumber >= 80)
        {
            TreasureAmount =+ Random.Range(1, 4);
        }
    }
    public List<GameObject> ReturnTrapList()
    {
        return MiniGameList;
    }

    /// <summary>
    /// Getter for treasure amount. Hard coded to return 0 in tutorial.
    /// </summary>
    /// <returns>
    /// The amount of treasure present in the grave.
    /// </returns>
    public int GetTreasureAmount()
    {
        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.TUTORIAL)
        {
            return 0;
        }
        else
        {
            return TreasureAmount;
        }
    }
}