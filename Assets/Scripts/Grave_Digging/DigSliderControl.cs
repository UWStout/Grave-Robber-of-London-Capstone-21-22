/**
 * @author Damian Link
 * 
 * @version 10/21/2021
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the Slider minigame
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * DigSlideDelta - change per update for slider
/// * TargetMax - max of range of sucessful dig
/// * TargetMin - min of range of sucessful dig
/// * DigProgressChange - the changes per dig, can be changed based on soil hardness
/// </remarks>
public class DigSliderControl : MiniGame
{
    //Audio
    public FMOD.Studio.EventInstance DiggingLoop;
    public FMODUnity.EventReference fmodEvent6;
    public FMOD.Studio.EventInstance SnipCage;
    // slider for the dig
    public Slider DigSliderObj;
    // slider for that shows the dig progress
    public Slider DigProgressBar;
    // area showing boost area
    public RectTransform GreenBackGround;
    // Handle for the moving slider
    public Image HandleObj;

    // change per update for slider
    [SerializeField] private float DigSlideDelta = 2f;
    // boolean based on if button to dig was pressed
    private bool Slide = false;
    // min/max of range of sucessful dig
    [SerializeField] private float TargetMax = 0.75f;
    [SerializeField] private float TargetMin = 0.35f;
    // the changes per dig, can be changed based on soil hardness
    public float DigProgressChange = 0.2f;
    //dig CD
    [SerializeField] private bool DigCooldown = false;
    // Tells if the minigame is being used as digging(true) or bolt cutting(false)
    public bool DiggingMinigame = true;
    // Tells if the shovel animation is finished 
    private bool ShovelReady = false;
    private float DigCooldownTime;
    private Animator Anim;

    [SerializeField] private Text InstructText;

    /// <summary>
    /// sets up the DigSlider
    /// </summary>
    private void Start()
    {
        // sets green area
        GreenBackGround.offsetMin = new Vector2((151.5f * TargetMin), 0);
        GreenBackGround.offsetMax = new Vector2((-53.5f * TargetMax), 0);
        //Audio
        DiggingLoop = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Digging");
        SnipCage = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Cage Snip");
        // 
        if (DiggingMinigame == true)
        {
            InstructText.text = "Press Space/E to Dig";
            HandleObj.sprite = GameManager.Instance._UpgradeManager.GetCurrentShovel().GetIcon();
            Anim = GameObject.Find("Player").GetComponent<Animator>();
        }
        else
        {
            HandleObj.sprite = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetClosedtexture();
            HandleObj.transform.localScale = new Vector3(3, 1, 1);
        }

    }

    /// <summary>
    /// moves slider value back and forth and detects when "dig" button is clicked
    /// </summary>
    private void Update()
    {
        // changes the direction when hits edge of slider
        if (DigSliderObj.value == DigSliderObj.maxValue || DigSliderObj.value == DigSliderObj.minValue)
        {
            DigSlideDelta *= -1;
        }

        // Checks for the Diggin to be pressed
        if (Input.GetButtonDown("Interact") && DigCooldown == false)
        {
            Slide = true;
            // turns on digging animation
            if (DiggingMinigame == true) {
                Animator Anim = GameObject.Find("Player").GetComponent<Animator>();
                Anim.SetBool("CanDig", true);
                StealthManager.Instance.MakeNoise(transform, 2.0f);
            }
            else
            {
                StealthManager.Instance.MakeNoise(transform, 2.0f);
                SnipCage.start();
                SnipCage.release();
            }
            DigCooldown = true;
            StartCoroutine(DigRefresh());
        } else
        {
            // turns off digging animation
            if (DiggingMinigame == true)
            {
                GameObject.Find("Player").GetComponent<Animator>().SetBool("CanDig", false);
            }
        }

        // if Digging was pressde then checks if within range, if not then add the change
        if (Slide)
        {
            Vector2Int PlayerTile = GameObject.FindGameObjectWithTag("MainEntrance").GetComponent<MapGeneration>().GetTilePos(GameObject.FindGameObjectWithTag("Player").transform);

            if (DigSliderObj.value > TargetMin && DigSliderObj.value < TargetMax)
            {
                DigCooldownTime = 0.2f;
                if (DiggingMinigame == true)
                {
                     DigProgressBar.value += DigProgressChange * (2 + GameManager.Instance._UpgradeManager.GetCurrentShovel().GetBoost());
                } else
                {
                    DigProgressBar.value += DigProgressChange * (2 + GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetBoost());
                }
               
                if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() != GraveyardTier.TUTORIAL)
                {
                    StealthManager.Instance.AddAlertPulse(StealthManager.Instance.GetGreenAlertPulse(), PlayerTile);
                }
            } else
            {
                DigCooldownTime = 0.6f;
                if (DiggingMinigame == true)
                {
                    DigProgressBar.value += DigProgressChange * (1 + GameManager.Instance._UpgradeManager.GetCurrentShovel().GetBoost());
                } else
                {
                    DigProgressBar.value += DigProgressChange * (1 + GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetBoost());
                }
                
                if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() != GraveyardTier.TUTORIAL)
                {
                    StealthManager.Instance.AddAlertPulse(StealthManager.Instance.GetRedAlertPulse(), PlayerTile);
                }
            }
            Slide = false;
        } else
        {
            //adds the change
            DigSliderObj.value += DigSlideDelta * Time.deltaTime;
        }
    }

    /// <summary>
    /// Enables shovel animation
    /// </summary>
    private void OnEnable()
    {
        // turns on digging animation
        if (DiggingMinigame == true)
        {
            // GameObject.Find("Player").GetComponent<Animator>().SetBool("HasLantern", false);
            // GameObject.Find("Player").GetComponent<Animator>().SetBool("CanDig", true);
            //Animator Anim = GameObject.Find("Player").GetComponent<Animator>();
            Anim = GameObject.Find("Player").GetComponent<Animator>();
            Anim.speed = 1.5f;
            Anim.SetBool("HasShovel", true);
            //Audio
            DiggingLoop.start();
            Debug.Log("Playing Digging Noise");
        }
    }

    /// <summary>
    /// Sets animation back to default
    /// </summary>
    private void OnDisable()
    {
        DigCooldown = false;
        // turns off digging animation
        if (DiggingMinigame == true)
        {
            Debug.Log("Releasing Digging Noise");
            Anim.SetBool("HasLantern", true);
            Anim.SetBool("CanDig", false);
            Anim.SetBool("HasShovel", false);
            Anim.speed = 1f;
            //Audio
            DiggingLoop.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    /// <summary>
    /// checks if the slider minigame has been won
    /// </summary>
    /// <returns>whether or not minigame is won</returns>
    public override bool CheckWin()
    {
        return (DigProgressBar.value == DigProgressBar.maxValue);

    }

    /// <summary>
    /// Disables digging for 1 second
    /// </summary>
    /// <returns></returns>
    IEnumerator DigRefresh()
    {
        yield return new WaitForSeconds(DigCooldownTime);
        DigCooldown = false;
    }
}
