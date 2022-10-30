using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class courtyardaudio : MonoBehaviour
{
    public FMOD.Studio.EventInstance CourtyardBGM;
    public FMODUnity.EventReference fmodEvent;
    public FMOD.Studio.EventInstance CourtyardOutro;
    public FMODUnity.EventReference fmodEvent2;
    [SerializeField]
    [Range(0f, 1f)]
    public float stinger;
    // Start is called before the first frame update
    void Start()
    {
        stinger = 0;
        //CourtyardBGM = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //CourtyardBGM.start();
        //CourtyardOutro = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
    }

    // Update is called once per frame
    void Update()
    {
        //CourtyardBGM.setParameterByName("PlayerChoosesLevel", stinger);
    }

    void OnDestroy()
    {
        //CourtyardOutro.start();
        stinger = 1;
        //CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    //public void StopCourtyardAudio()
    //{
        //CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    //}
}
