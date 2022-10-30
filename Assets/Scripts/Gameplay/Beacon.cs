using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * author: Aaron Tweden
 * 
 * version: 11/10/2021
 */
public class Beacon : MonoBehaviour
{
    public int BeaconNoise = 0;
    bool PulseActive = false;
    bool CleanUpActive = false;
    int DecayNoiseAmount = -10;
    float DecayNoiseTime = 2;
    bool NoiseDecayBool = false;

    void LateUpdate()
    {
        if(PulseActive == false)
        {
            StartCoroutine(Pulse());
        }
        if (PulseActive == false)
        {
            StartCoroutine(CleanUp());
        }
        if(BeaconNoise == 0)
        {
            Debug.Log("destroied");
            Destroy(gameObject);
        }
        if(NoiseDecayBool == false)
        {
            StartCoroutine(NoiseDecay());
        }
    }
    //allows the code to set the noise level to what you want useful for testing and its how the code sets the noise level of the beacon 
    public void SetNoise(int Noise)
    {
        BeaconNoise = Noise;
        Debug.Log(Noise);
    }
    public int ReturnNoise()
    {
        return BeaconNoise;
    }
    //noises dont hang in the air they should here 
    //this decay works as it does in the player script but it decays by DecayNoiseAmount at intervals of DecayNoiseTime
    IEnumerator NoiseDecay()
    {
        NoiseDecayBool = true;
        yield return new WaitForSeconds(DecayNoiseTime);
        BeaconNoise += DecayNoiseAmount;
        NoiseDecayBool = false;
    }
    //summons the guards: all beacons pulse to "summon the guards" we pull an array of things tagged with the Guard tag.
    //then it checks if they are close enough to be summoned 
    //if they are in range it calls the guard script to move that instance of the guard to the beacon, This can lead the guard to the player.
    //this is capped to once every 2 secs to prevent spam and slow down. becuase every beacon can do this.
    IEnumerator Pulse()
    {
        PulseActive = true;
        yield return new WaitForSeconds(2);
        foreach(GameObject Guard in GameObject.FindGameObjectsWithTag("Guard"))
        {
            if (Vector3.Distance(Guard.GetComponent<Transform>().position, transform.position) <= 50)
            {
                //Guard.GetComponent<Guard>().CallAlerted(transform);
            }
        }
        PulseActive = false;
    }
    //combines near by beacons 
    IEnumerator CleanUp()
    {
        CleanUpActive = true;
        yield return new WaitForSeconds(1);
        //search for other beacons 
        foreach (GameObject Beacon in GameObject.FindGameObjectsWithTag("Beacon"))
        {
            if (Vector3.Distance(Beacon.GetComponent<Transform>().position, transform.position) <= 10)
            {
                //AddNoise(Beacon.GetComponent<Beacon>().ReturnNoise());
                //Beacon.GetComponent<Beacon>().DestroyBeacon();
            }
        }
        CleanUpActive = false;
    }
    //adds noise from other beacons during cleanup
    void AddNoise(int Noise)
    {

    }
    //destroys beacon during clean up
    void DestroyBeacon()
    {

    }
}
