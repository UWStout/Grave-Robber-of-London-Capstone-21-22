using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/**
 * author: Aaron Tweden
 * 
 * version: 11/13/2021
 */

public class StealthManager : MonoBehaviour
{
    //Audio
    public AudioInteractivityManager audioInteractManager;
    
    //singlton logic
    public static StealthManager Instance { get; private set; }
    //alert level which guards use to become more dangerous. Max is 300
    public float AlertLevel = 0;
    //allows the alert level to decrease
    public bool AlertDecrease = false;

    public bool AlertDecayStarted = false;
    public int AlertDecayTime = 60;

    private float AlertPulseProgress = 0; // Progress towards making a large noise which will draw guards towards you
    private Vector2Int AlertPulseOrigin = new Vector2Int(); // Point which caused the noise pulse

    // Checks if the player is currently hidden in a bush
    private bool InBush = false;

    // Checks if the player is currently hidding behind a tree, and in which direction
    private bool InTree = false;
    private Direction TreeDirection = Direction.DOWN;

    public int NoiseDecayTime = 7;

    private MapGeneration MG;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
           
            




           
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
      
        //if(Input.GetKeyDown("f"))
        //{
        //    AlertIncrease();
        //}
    }


    public float GetSightRange()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 3.5f;
            case var e when AlertLevel < 200.0f:
                return 4.5f;
            case var e when AlertLevel < 300.0f:
                return 5.5f;
            case var e when AlertLevel >= 300.0f:
                return 6.0f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetHearingRange()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 9.5f;
            case var e when AlertLevel < 200.0f:
                return 8.5f;
            case var e when AlertLevel < 300.0f:
                return 7.5f;
            case var e when AlertLevel >= 300.0f:
                return 6.0f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetPatrolSpeed()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 2.0f;
            case var e when AlertLevel < 200.0f:
                return 2.2f;
            case var e when AlertLevel < 300.0f:
                return 2.4f;
            case var e when AlertLevel >= 300.0f:
                return 3.0f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetSprintingSpeed()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 2.6f;
            case var e when AlertLevel < 200.0f:
                return 2.8f;
            case var e when AlertLevel < 300.0f:
                return 3.0f;
            case var e when AlertLevel >= 300.0f:
                return 3.5f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetChaseSpeed()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 2.1f;
            case var e when AlertLevel < 200.0f:
                return 2.5f;
            case var e when AlertLevel < 300.0f:
                return 2.7f;
            case var e when AlertLevel >= 300.0f:
                return 3.5f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetGreenAlertPulse()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 3.0f;
            case var e when AlertLevel < 200.0f:
                return 5.0f;
            case var e when AlertLevel < 300.0f:
                return 8.0f;
            case var e when AlertLevel >= 300.0f:
                return 15.0f;
            default:
                return Mathf.Infinity;
        }
    }

    public float GetRedAlertPulse()
    {
        switch (AlertLevel)
        {
            case var e when AlertLevel < 100.0f:
                return 40.0f;
            case var e when AlertLevel < 200.0f:
                return 75.0f;
            case var e when AlertLevel < 300.0f:
                return 100.0f;
            case var e when AlertLevel >= 300.0f:
                return 100.0f;
            default:
                return Mathf.Infinity;
        }
    }

    /// <summary>
    /// Returns the amount needed to trigger an alert pulse, depends on graveyard tier.
    /// </summary>
    /// <returns>
    /// The amount needed to trigger an alert pulse.
    /// </returns>
    public float AlertPulseCap()
    {
        switch (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier())
        {
            case GraveyardTier.LOW:
                return 150;
            case GraveyardTier.MEDIUM:
                return 120;
            case GraveyardTier.HIGH:
                return 100;
            default:
                return float.MaxValue;
        }
    }

    /// <summary>
    /// Checks if the player is currently hidden and calculates the penalty to guard's vision range.
    /// </summary>
    /// <param name="GuardPos">
    /// The guard's current position.
    /// </param>
    /// <returns>
    /// The total penalty to the guard's vision range.
    /// </returns>
    public float GetHiddenBonus(Transform GuardPos)
    {
        float HiddenBonus = 0;

        if (InBush)
        {
            HiddenBonus += 5;
        }

        if (InTree)
        {
            Vector3 PlayerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
            Vector3 PlayerDist = PlayerPos - GuardPos.position;
            switch (TreeDirection)
            {
                case Direction.UP:
                    if (PlayerDist.z >= 1)
                    {
                        HiddenBonus += 3;
                    }
                    break;
                case Direction.DOWN:
                    if (PlayerDist.z <= -1)
                    {
                        HiddenBonus += 3;
                    }
                    break;
                case Direction.RIGHT:
                    if (PlayerDist.x >= 1)
                    {
                        HiddenBonus += 3;
                    }
                    break;
                case Direction.LEFT:
                    if (PlayerDist.x <= -1)
                    {
                        HiddenBonus += 3;
                    }
                    break;
                default:
                    break;
            }
        }

        return HiddenBonus;
    }

    //increases alert level when called
    public void AlertIncrease(float val)
    {
        AlertLevel += val;
        audioInteractManager.EnemyLayerIncrease();

    }
    //starts a count down of 60sec
    public void AlertDecay()
    {
        if(AlertLevel != 0)
        {
            audioInteractManager.EnemyLayerDecrease();
            StartCoroutine("DecayAlert");
        }
    }

    public void NoiseDecay()
    {
        if (AlertLevel != 0)
        {
            
            AlertDecayStarted = true;
            StartCoroutine("DecayNoise");
        }
    }

    /// <summary>
    /// Makes a noise centered on the tile at the passed in position.
    /// </summary>
    /// <param name="Pos">
    /// The position of the object making noise, will be converted to tile coordinates
    /// </param>
    /// <param name="Noise">
    /// The level of noise being made
    /// </param>
    /// <returns>
    /// If a noise was succesfuly made, returns true, otherwise false.
    /// </returns>
    public bool MakeNoise(Transform Pos, float Noise)
    {
        if (MG != null)
        {
            Vector2Int TilePos = MG.GetTilePos(Pos);
            float NoiseChange = (1 - GameManager.Instance._UpgradeManager.GetCurrentShoes().GetStealthBoost());

            if (MG.SafeTileCheck(TilePos[0], TilePos[1])) {
                TileNoisePulse(TilePos[0], TilePos[1], TilePos, (Noise * NoiseChange), true, true, true, true);
                return true;
            }
        }
        return false;
    }

    private void TileNoisePulse(int XPos, int ZPos, Vector2Int Source, float Noise, bool Up, bool Down, bool Left, bool Right)
    {
        if (Noise <= 0)
        {
            return;
        }

        if (MG.SafeTileCheck(XPos, ZPos) && MG.GetTileData(XPos, ZPos).OverrideNoise(Noise, Source))
        {
            if (Up)
            {
                TileNoisePulse(XPos, ZPos + 1, Source, Noise - 1, true, false, Left, Right);
            }
            
            if (Down)
            {
                TileNoisePulse(XPos, ZPos - 1, Source, Noise - 1, false, true, Left, Right);
            }

            if (Left)
            {
                TileNoisePulse(XPos - 1, ZPos, Source, Noise - 1, Up, Down, true, false);
            }

            if (Right)
            {
                TileNoisePulse(XPos + 1, ZPos, Source, Noise - 1, Up, Down, false, true);
            }
        }
    }

    /// <summary>
    /// Toggles the InBush variable between true and false.
    /// </summary>
    /// <returns>
    /// The new value of InBush.
    /// </returns>
    public bool ToggleInBush()
    {
        InBush = !InBush;
        return InBush;
    }

    public void HideBehindTree(Direction dir)
    {
        InTree = true;
        TreeDirection = dir;
    }

    public void LeaveTree()
    {
        InTree = false;
    }

    //return alert level     
    public float ReturnAlertLevel()
    {
        return AlertLevel; 
    }
    /// <summary>
    /// resets alert level to zero
    /// </summary>
    public void ResetAlert()
    {
        AlertLevel = 0;
    }

    IEnumerator DecayAlert()
    {
        // counts down for decay time in secs
        yield return new WaitForSeconds(AlertDecayTime);
        //checks to see if Alert level is zero just incase silly things happen
        if(AlertLevel != 0)
        {
            AlertLevel -= 1;
        }
        AlertDecayStarted = false;
        //allows another decay to be called
    }


    IEnumerator DecayNoise(Transform Pos, float Noise)
    {
        yield return new WaitForSeconds(NoiseDecayTime);
    }
    public void SetMG(MapGeneration Script)
    {
        MG = Script;
    }

    /// <summary>
    /// Getter for alert level.
    /// </summary>
    /// <returns>
    /// Current alert level.
    /// </returns>
    public float GetAlertLevel()
    {
        return AlertLevel;
    }

    /// <summary>
    /// Adds to the progress of the alert pulse.
    /// </summary>
    /// <param name="_Val">
    /// Amount to increase by.
    /// </param>
    /// <returns>
    /// The new value of alert pulse progress.
    /// </returns>
    public bool AddAlertPulse(float _Val, Vector2Int Pos)
    {
        Debug.Log("Pulse " + _Val);

        AlertPulseProgress += _Val;

        AlertPulseOrigin = Pos;

        if (AlertPulseProgress >= AlertPulseCap())
        {
            List<GameObject> TempGuards = MG.GetGuardList();

            int RandGuard = Random.Range(0, TempGuards.Count);

            TempGuards[RandGuard].GetComponent<Guard>().BeginLongTermActive(AlertPulseOrigin);

            AlertPulseProgress = 0;

            AlertLevel += 20;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the position that caused the noise pulse.
    /// </summary>
    /// <returns>
    /// The x/y position of the alert pulse.
    /// </returns>
    public Vector2Int GetAlertPulseOrigin()
    {
        return AlertPulseOrigin;
    }

    /// <summary>
    /// Determines if the alert pulse has hit the cap, which will draw in guards.
    /// </summary>
    /// <returns>
    /// Whether or not alert pulse is active.
    /// </returns>
    public bool GetAlertPulseActive()
    {
        return AlertPulseProgress >= AlertPulseCap();
    }
}
