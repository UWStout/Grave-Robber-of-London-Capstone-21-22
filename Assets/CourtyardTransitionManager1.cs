using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourtyardTransitionManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance CourtyardBGM;
    public FMODUnity.EventReference fmodEvent;
    [SerializeField]
    [Range(0f, 1f)]
    public float stinger;
    // Start is called before the first frame update
    void Start()
    {
        stinger = 0;
        CourtyardBGM = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        CourtyardBGM.start();
        
    }

    // Update is called once per frame
    void Update()
    {
        CourtyardBGM.setParameterByName("PlayerChoosesLevel", stinger);
    }

    public void PlayerChoosesLevel()
    {
        stinger = 1;
    }
}
