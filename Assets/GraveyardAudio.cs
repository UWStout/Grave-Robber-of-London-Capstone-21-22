using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class GraveyardAudio : MonoBehaviour
    {
        public FMOD.Studio.EventInstance GraveyardBGM;
        public FMODUnity.EventReference fmodEvent;
        public FMOD.Studio.EventInstance GraveyardAmbience;
        public FMODUnity.EventReference fmodEvent2;
        public FMOD.Studio.EventInstance LanternAmbience;
        public FMODUnity.EventReference fmodEvent3;
        [SerializeField]
        [Range(0f, 3f)]
        public float DistanceToEnemy;
        [SerializeField]
        [Range(0f, 3f)]
        public float GraveyardProgression;
        // Start is called before the first frame update
        void Start()
        {
            GraveyardProgression = 0;
            GraveyardBGM = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
            //GraveyardBGM.start();
            GraveyardAmbience = FMODUnity.RuntimeManager.CreateInstance(fmodEvent2);
            //GraveyardAmbience.start();
            LanternAmbience = FMODUnity.RuntimeManager.CreateInstance(fmodEvent3);
            //LanternAmbience.start();
        }

        // Update is called once per frame
        void Update()
        {
            GraveyardBGM.setParameterByName("DistanceToEnemy", DistanceToEnemy);
            GraveyardBGM.setParameterByName("GraveyardProgression", GraveyardProgression);
        }
        void OnDestroy()
        {
            //GraveyardBGM.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //GraveyardAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //LanternAmbience.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
