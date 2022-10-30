using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFlipScript : MonoBehaviour
{
    //Audio
    private FMOD.Studio.EventInstance footstepsGuard;
    float timer = 0.0f;
    [SerializeField]
    float footstepSpeed = 0.4f;
    private float TurningRate = 20.0f;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public Animator AnimationController;
    private Quaternion _TargetRotation = Quaternion.identity;
    public GameObject FlipObject;
    public bool IsWalking;
    public Vector3 BlendedEuler = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        SetBlendedEulerAngles(BlendedEuler);

    }

    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _TargetRotation = Quaternion.Euler(angles);
    }


    // Update is called once per frame
    void Update()
    {
        float HorizontalInput = navMeshAgent.velocity.x;
        float VerticalInput = navMeshAgent.velocity.z;


        if (HorizontalInput != 0 || VerticalInput != 0)
        {
            if (timer > footstepSpeed)
            {
                PlayGuardFootstep();
                timer = 0.0f;
            }
            timer += Time.deltaTime;
            IsWalking = true;
        }
        else
        {
            IsWalking = false;
        }

        AnimationController.SetBool("Walking", IsWalking);

        if (HorizontalInput < 0)
        {
            SetBlendedEulerAngles(BlendedEuler);
        }
        else if (HorizontalInput > 0)
        {
            SetBlendedEulerAngles(new Vector3(BlendedEuler.x, BlendedEuler.y-180, BlendedEuler.z));

        }
        FlipObject.transform.rotation = Quaternion.RotateTowards(FlipObject.transform.rotation, _TargetRotation, TurningRate * Time.deltaTime * 80);

    }
    public void PlayGuardFootstep()
    {
        footstepsGuard = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Footsteps - Guard");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(footstepsGuard, GetComponent<Transform>(), GetComponent<Rigidbody>());
        footstepsGuard.start();
        //Debug.Log("playing footstep");
        footstepsGuard.release();
    }
}
