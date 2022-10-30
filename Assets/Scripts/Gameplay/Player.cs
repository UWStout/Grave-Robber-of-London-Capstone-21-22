using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * author: Aaron Tweden
 * 
 * version: 11/10/2021
 */
public class Player : MonoBehaviour
{
    public GameObject Beacon;
    Rigidbody M_Rigidbody;
    //speed of character 
    public float M_Speed = 5f;
    //starting noise level
    public int NoiseLevel = 0;

    // Amount of noise made while walking
    private float FootstepNoise = 10.5f;

    //time intervials that the noise will decay
    float DecayNoiseTime = 5f;
    //amount noise will decay
    public int DecayNoiseAmount = -5;
    Animator Animator;
    //various bools for noise level intended for the UI
    bool NoiseLevelQuiet = false;
    bool NoiseLevelMild = false;
    bool NoiseLevelLoud = false;
    bool NoiseLevelNone = true;

    bool NoiseDecayBool = false;

    private bool Walking = false;
    
    [SerializeField] private GameObject ShovelMateral;

    bool BeaconDrop = false;

    protected bool StepCooldown = false;

    // Used to check if the player is being forced to not move
    private bool Paused = false;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        M_Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
        if (ShovelMateral != null)
        {
            ShovelMateral.GetComponent<MeshRenderer>().material.mainTexture = GameManager.Instance._UpgradeManager.GetCurrentShovel().GetIcon().texture;
        }
        
    }
    private void Update()
    {
       
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown("g"))
        {
            //AddNoise(50);
        }
        if(NoiseDecayBool == false)
        {
            NoiseDecay();
        }
    }

    protected void FixedUpdate()
    {
        //Store user input as a movement vector
        Vector3 M_Input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //Apply the movement vector to the current position, which is
        //multiplied by deltaTime and speed for a smooth MovePosition
        if (M_Input != new Vector3(0, 0, 0) && !Paused)
        {
            //M_Rigidbody.MovePosition(transform.position + M_Input * Time.deltaTime * M_Speed);
            M_Rigidbody.velocity = M_Input.normalized * Time.deltaTime * (M_Speed + GameManager.Instance._UpgradeManager.GetCurrentShoes().GetSpeedBoost());
            Animator.SetBool("Walking", true);
            Walking = true;
            if (!StepCooldown)
            {
                StartCoroutine(Footstep());
            }
        }
        else 
        {
            M_Rigidbody.velocity = new Vector3(0, 0, 0);
            Animator.SetBool("Walking", false);
            Walking = false;
        }
    }
    
    //plug in to allow the UI to see and understand noise level just a bunch bools 
    public void NoiseIndicator()
    {
        if(NoiseLevel <= 100)
        {
            NoiseLevelNone = false;
            NoiseLevelQuiet = false;
            NoiseLevelMild = false;
            NoiseLevelLoud = true;
        }
        else if (NoiseLevel <= 75)
        {
            NoiseLevelNone = false;
            NoiseLevelQuiet = false;
            NoiseLevelMild = true;
            NoiseLevelLoud = false;
        }
        else if(NoiseLevel <= 50)
        {
            NoiseLevelNone = false;
            NoiseLevelQuiet = true;
            NoiseLevelMild = false;
            NoiseLevelLoud = false;
        }
        else
        {
            NoiseLevelNone = true;
            NoiseLevelQuiet = false;
            NoiseLevelMild = false;
            NoiseLevelLoud = false;
        }
    }
    //allows other scrips to add noise level
    public int AddNoise(int noise)
    {
        NoiseLevel += noise;
        NoiseIndicator();
        return NoiseLevel;
    }
    //resets noise level to zero
    public void NoiseReset()
    {
        NoiseLevel = 0;
    }
    //returns noise level, always good to have return statments
    public int NoiseReturn()
    {
        return NoiseLevel;
    }
    //trigures noise decay: when this is triggured the noise will drop by DecayNoiseAmount at intervals of DecayNoiseTime allowing full control 
    IEnumerator NoiseDecay()
    {
        NoiseDecayBool = true;
        yield return new WaitForSeconds(DecayNoiseTime);
        NoiseLevel += DecayNoiseAmount;
        NoiseDecayBool = false;
    }

    /// <summary>
    /// Makes a moderate noise at the player's current position with a short cooldown.
    /// </summary>
    protected virtual IEnumerator Footstep()
    {
        StepCooldown = true;
        yield return new WaitForSeconds(0.25f);
        if (Walking)
        {
            StealthManager.Instance.MakeNoise(transform, FootstepNoise);
        }
        StepCooldown = false;
    }

    public void SetPaused(bool Val)
    {
        Paused = Val;
    }

    public bool GetPaused()
    {
        return Paused;
    }
}
