using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Grave info is all about generating the needed for the game play to work
/// map generation populates a prefab in the level
/// Then this program runs and generates witht he help of grave generation 
/// the actual information of the grave and its usable parts.
/// </summary>
/// <param name="DigPrompt">will be used to tell players they can interact here </param>
/// <param name="MiniGame">saves a copy of the mini game prefab to make one as a child object</param>
/// <param name="LittleMiniGame"> the saved  copy of the mini game</param>
/// <param name="Body">saves the generation of body generation</param>
/// <param name="GraveTresure">will store the amount of treasure in the grave</param>
/// <param name="DecayLevel">stores the decay level of the body</param>
/// <param name="TierOfBody">this is the tier of body passed into GraveGeneration</param>
/// <param name="TrapProgression">this</param>
/// <param name="BodyTaken"></param>
/// <param name="Traps"></param>
/// <param name="CanGraveDig"></param>

public class GraveInfoRefactor : MonoBehaviour
{
    public GameObject DigPrompt;
    public GameObject DugGrave;
    public GameObject GraveMound;
    public GameObject DeadBody;
    public GameObject Grave;
    public GameObject CoffinCage;
    public bool CoffinCageActive = false;
    GraveGeneration Generate;
    BodyRandom RandomSpriteBody;
    MoneyExplosion Money;
    [SerializeField] private Body GraveBody;
    public int DecayLevel = 0;
    public GraveQuality TierOfGrave = GraveQuality.LOW;
    bool BodyTaken = false;

    //bool TamperedWith = false;
    bool PlayerIn = false;

    bool FinishedTraps = false;
    public List<GameObject> MiniGameList = new List<GameObject>();
    //allows traps to be forced and the script to be disabled for a time 
    public bool ForceTrap = false;
    bool Distracted = false;
    bool Waiting = true;
    bool waitingActive = false;

    //grave material 
    public Material Grey;
    public Material Empty;
    Material[] MatArray = new Material[2];

    Material MaterialBody;
    public Material DefaultMaterialBody;

    /// <summary>
    /// first it set body an instance of GameObject i have saved above setting it to an instance of BodyGeneration 
    /// then I make the mini game as this objects child
    /// Then I take info from body generation 
    /// </summary>
    /// 
    public void Initiate()
    {
        //maybe a singlton if we do need to add a clear function 
        Generate = GetComponent<GraveGeneration>();
        RandomSpriteBody = GetComponent<BodyRandom>();
        Money = GetComponent<MoneyExplosion>();
        GraveBody = Generate.GenerateGrave(TierOfGrave);
        MiniGameList = Generate.ReturnTrapList();
        DecayLevel = Generate.DecayLevelBody;
        RandomSpriteBody.GenerateRandomBody(DecayLevel, TierOfGrave);
        GraveBody.SetMaterial(RandomSpriteBody.ReturnMaterial());
        CoffinCageActive = Generate.CoffinCageActive;
        TurnOnCage();
        AssignBody();
    }
    /// <summary>
    /// This is how you interact with the grave it flips a bool true to allow you to interact with the traps 
    /// </summary>
    protected virtual void Update()
    {
        if (PlayerIn && !Distracted)
        {
            //make the e key the interact button
            //checks that the player is in the grave interact area and that they pressed e
            if (Input.GetButtonDown("Interact") && !FinishedTraps && !GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapCommands>().GetSceneFrozen())//might need to limit this 
            {
                NextTrap();
            }
            if (FinishedTraps && Input.GetButtonDown("Interact") && !BodyTaken)
            {
                GrabBody();
            }
            if (!FinishedTraps)
            {
                CheckWin();
            }
        }
    }
    /// <summary>
    /// this checks that the player enters the trigger colider then 
    /// it activates TrapProg which updates the traps you are on and keeps track of that
    /// then if the player is collided and hits t it activates the canvas 
    /// we did this to use some some performance instead of using up more memory.
    /// </summary>
    /// <param name="other">it returns what collider is collided</param>
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerIn = true;
            GameManager.Instance._RunManager.SetActiveGrave(gameObject.GetComponent<GraveInfoRefactor>());
        }
        if (MiniGameList.Count > 0)
        {
            HighLight();
        }
    }
    /// <summary>
    /// checks to see when the player leaves the trigger zone, it was done like this to prevent other objkects from doing that
    /// </summary>
    /// <param name="other"> it gets the ridged coliding with the zone</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && !FinishedTraps)
        {
            PlayerIn = false;
            UnHighLight();
            GameManager.Instance.ToggleMinigame(MiniGameList[0], false);
        }
        else
        {
            PlayerIn = false;
        }
    }
    /// <summary>
    /// gets next trap and activate the trap
    /// additiaonlly it removes the grave mound and adds the replacemnt and makers the dead body appear
    /// it also triggures the money spawn if there is anyp
    /// </summary>
    private void NextTrap()
    {
        if(MiniGameList.Count != 0)
        {
            OpenMiniGame();
        }
        else
        {
            if(!BodyTaken)
            {
                FinishedTraps = true;
                //have body appear 
                DeadBody.SetActive(true);
                GraveMound.SetActive(false);
                DugGrave.SetActive(true);

                if (Generate.GetTreasureAmount() > 0)
                {
                    Money.SpreadTreasure(Generate.GetTreasureAmount());
                }
            }
        }
    }
    /// <summary>
    /// opens the mini game for the player to play
    /// </summary>
    private void OpenMiniGame()
    {
        // turns on the mini game at position zero and if you solve it 
        GameManager.Instance.ToggleMinigame(MiniGameList[0], true);
        UIManager.Instance.ActiveGame = MiniGameList[0];
        //Debug.Log("Im open");
    }
    private void CheckWin()
    {
            if (MiniGameList[0].GetComponent<MiniGame>().CheckWin())
            {
                if(Waiting)
                {
                    StartCoroutine(PauseForElation());
                }
                    
            }
    }
    /// <summary>
    /// grabs the body from the grave and adds it to the game manager, then it calles empty grave to clean up the grave
    /// </summary>
    protected virtual void GrabBody()
    {
        if (GameManager.Instance._UpgradeManager.GetCurrentBodyBag().GetMaxBodies() > GameManager.Instance._RunManager.GetBodies().Length + GameManager.Instance.ReturnBodyList().Count)
        {
            GameManager.Instance.AddBody(GraveBody);
            BodyTaken = true;
            UnHighLight();
            GraveEmpty();
        }
    }
    /// <summary>
    /// will clean up the grave of ui and various other bits of code to reduce load on computer
    /// </summary>
    private void GraveEmpty()
    {
        DeadBody.SetActive(false);
        //Destroy(transform.parent.gameObject);

    }
    /// <summary>
    /// stops the player from interacting with teh graves
    /// </summary>
    public void AmDistracted()
    {
        Distracted = true;
    }
    /// <summary>
    /// reenables distracted allowing the player to interact with the graves 
    /// </summary>
    public void NotDistracted()
    {
        Distracted = false;
    }

    protected virtual void HighLight()
    {
        MatArray = Grave.GetComponent<MeshRenderer>().materials;
        Material[] temp = new Material[2];
        MatArray.CopyTo(temp, 0);
        temp[1] = Grey;
        Grave.GetComponent<MeshRenderer>().materials = temp;
    }

    void UnHighLight()
    {
        MatArray = Grave.GetComponent<MeshRenderer>().materials;
        Material[] temp = new Material[2];
        MatArray.CopyTo(temp, 0);
        temp[1] = Empty;
        Grave.GetComponent<MeshRenderer>().materials = temp;
    }
    void AssignBody()
    {
        DeadBody.GetComponent<SpriteRenderer>().material = GraveBody.GetMaterial();
        if(DeadBody == null)
        {
            DeadBody.GetComponent<SpriteRenderer>().material = DefaultMaterialBody;
        }
    }
    void TurnOnCage()
    {
            if (CoffinCageActive)
            {
                CoffinCage.SetActive(true);
            }
    }
    public void TurnOffCage()
    {
        if (CoffinCageActive)
        {
            CoffinCage.SetActive(false);
        }
    }

    public void SetPlayerIn(bool _Val)
    {
        PlayerIn = _Val;
    }

    IEnumerator PauseForElation()
    {
        Waiting = false;
        yield return new WaitForSeconds(.3f);
        GameManager.Instance.CompleteTask(GoalType.Minigame, BodyQuality.Fresh);
        GameManager.Instance.ToggleMinigame(MiniGameList[0], false);
        MiniGameList.RemoveAt(0);
        NextTrap();
        Waiting = true;
    }

    public void ForceTraps(bool[] Traps)
    {
        Generate = GetComponent<GraveGeneration>();
        GraveBody = Generate.GenerateGrave(GraveQuality.LOW, Traps);
        MiniGameList = Generate.ReturnTrapList();
        GraveBody.SetMaterial(RandomSpriteBody.ReturnMaterial());
        DecayLevel = Generate.DecayLevelBody;
    }
}
