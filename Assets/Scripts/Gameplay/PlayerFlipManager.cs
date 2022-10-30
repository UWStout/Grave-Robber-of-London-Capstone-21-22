using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlipManager : MonoBehaviour
{
    //Audio
    private FMOD.Studio.EventInstance footstepsPlayer;
    float timer = 0.0f;
    [SerializeField]
    float footstepSpeed = 0.4f;
    public GameObject FlipObject;
    public GameObject HandDepth;
    public GameObject HandObjectDepth;
    public Animator AnimationController;
    private float TurningRate = 30.0f;
    private Quaternion _TargetRotation = Quaternion.identity;
    public GameObject Particles;
    public bool HasShovel;
    public bool HasLantern;
    public bool IsWalking;
    ParticleSystem.EmissionModule particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        SetBlendedEulerAngles(new Vector3(-90, 0, 0));
        //Debug.Log(HandDepth.transform.position);
        HandDepth.transform.localPosition = new Vector3(0, 0.1f, 0);
        HandObjectDepth.transform.localPosition = new Vector3(0, 0, -0.05f);
        particleSystem = Particles.GetComponent<ParticleSystem>().emission;
    }

    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _TargetRotation = Quaternion.Euler(angles);
    }

    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
       

        if (HorizontalInput != 0 || VerticalInput != 0)
        {
            if (timer > footstepSpeed)
            {
                PlayFootstep();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
            HasShovel = false;
            if (!IsWalking && !GetComponent<Player>().GetPaused())
            {
                particleSystem.enabled = true;
                IsWalking = true;
                

            }
        }
        else {
            particleSystem.enabled = false;
            IsWalking = false;
        }


        if (HorizontalInput < 0) {
            AnimationController.SetBool("Left",true);
            SetBlendedEulerAngles(new Vector3(-90, 0, 0));
            //Debug.Log(HandDepth.transform.position);
            HandDepth.transform.localPosition = new Vector3(0, 0.1f, 0);
            HandObjectDepth.transform.localPosition = new Vector3(0, 0, -0.05f);
        }
        else if (HorizontalInput > 0) {
            AnimationController.SetBool("Left", false);
            SetBlendedEulerAngles(new Vector3(-90, 0, -180));
            if (HasShovel)
            {
                HandDepth.transform.localPosition = new Vector3(0, -0.45f, 0);
                HandObjectDepth.transform.localPosition = new Vector3(0, 0, 0.5f);
            }
            else 
            {

                HandDepth.transform.localPosition = new Vector3(0, -0.1f, 0);
                HandObjectDepth.transform.localPosition = new Vector3(0, 0, 0.05f);

            }

        }
        FlipObject.transform.rotation = Quaternion.RotateTowards(FlipObject.transform.rotation, _TargetRotation, TurningRate * Time.deltaTime* 80);

    }
    public void PlayFootstep()
    {
        footstepsPlayer = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Footsteps - PlayerNPC");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstepsPlayer, GetComponent<Transform>(), GetComponent<Rigidbody>());
        footstepsPlayer.start();
        //Debug.Log("playing footstep");
        footstepsPlayer.release();
    }
}
