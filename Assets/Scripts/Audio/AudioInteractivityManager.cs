using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioInteractivityManager : MonoBehaviour
{
    //Graveyard and Tutorial
    public FMOD.Studio.EventInstance GraveyardBGM;
    public FMODUnity.EventReference fmodEvent;
    public FMOD.Studio.EventInstance GraveyardAmbience;
    public FMODUnity.EventReference fmodEvent2;
    public FMOD.Studio.EventInstance LanternAmbience;
    public FMODUnity.EventReference fmodEvent3;
    public FMODUnity.EventReference fmodEvent9;
    public FMOD.Studio.EventInstance GraveyardTwo;
    [SerializeField]
    [Range(0f, 3f)]
    public float DistanceToEnemy;
    [SerializeField]
    [Range(0f, 3f)]

    //Courtyard
    public FMOD.Studio.EventInstance CourtyardBGM;
    public FMODUnity.EventReference fmodEvent4;
    public FMOD.Studio.EventInstance CourtyardOutro;
    public FMODUnity.EventReference fmodEvent5;
    [SerializeField]
    [Range(0f, 1f)]
    public float stinger;
    public float GraveyardProgression;

    //Title

    // Start is called before the first frame update
    void Start()
    {
        //Courtyard
        stinger = 0;
        CourtyardBGM = FMODUnity.RuntimeManager.CreateInstance(fmodEvent4);
        CourtyardOutro = FMODUnity.RuntimeManager.CreateInstance(fmodEvent5);
        //Graveyard and Tutorial
        GraveyardProgression = 0;
        GraveyardBGM = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);     
        GraveyardAmbience = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
        LanternAmbience = FMODUnity.RuntimeManager.CreateInstance(fmodEvent3);
        GraveyardTwo = FMODUnity.RuntimeManager.CreateInstance(fmodEvent9);
        //Title

        //Scene Change
        SceneManager.activeSceneChanged += ChangedActiveScene;

    }

    private void ChangedActiveScene(UnityEngine.SceneManagement.Scene current, UnityEngine.SceneManagement.Scene next)
    {
        UnityEngine.SceneManagement.Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        string currentName = current.name;
        Debug.Log(sceneName);
        Debug.Log(currentName);
        if (sceneName == "GraveYard")
        {
            if (currentName == "CourtYard")
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardOutro.start();
                GraveyardBGM.start();
                //GraveyardAmbience.start();
                LanternAmbience.start();
            }
            else
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.start();
                GraveyardAmbience.start();
                LanternAmbience.start();
            }
           
        }
        else if (sceneName == "CourtYard")
        {
            GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CourtyardBGM.start();
        }
        else if (sceneName == "Tutorial")
        {
            GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            GraveyardBGM.start();
            //GraveyardAmbience.start();
            LanternAmbience.start();
        }
        else if (sceneName == "Title")
        {
            if (currentName == "CourtYard")
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardOutro.start();
                GraveyardTwo.start();
                //GraveyardAmbience.start();
                Debug.Log("Playing Title Music");
            }
            else
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardTwo.start();
                //GraveyardAmbience.start();
                Debug.Log("Playing Title Music");
            }
        }
        else if (sceneName == "OpeningCutscene")
        {
            if (currentName == "CourtYard")
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardOutro.start();
         
            }
            else
            {
                GraveyardTwo.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardAmbience.start();

            }

        }
        else if (sceneName == "LoadingScene")
        {
            if (currentName == "CourtYard")
            {
                Debug.Log("Playing Courtyard Outro");
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardOutro.start();
                GraveyardAmbience.start();


            }
            else
            {
                GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                CourtyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                GraveyardAmbience.start();

            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        GraveyardBGM.setParameterByName("DistanceToEnemy", DistanceToEnemy);
        GraveyardBGM.setParameterByName("GraveyardProgression", GraveyardProgression);
    }

    public void EnemyLayerIncrease()
    {
        if (DistanceToEnemy <= 3)
        {
            DistanceToEnemy = DistanceToEnemy + 1;
        }
        else
        {

        }
    }

    public void EnemyLayerDecrease()
    {
        if (DistanceToEnemy != 0)
        {
            DistanceToEnemy = DistanceToEnemy - 1;
        }
        else
        {

        }
    }
}
