using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRandom : MonoBehaviour
{
    int DecayLevel = 0;
    GraveQuality TierOfBody;
    Material BodyMaterial;
    public Material Skeleton;

    //arrays for mats
    public Material[] LowTierMaleFresh = new Material[3];
    public Material[] LowTierMaleLightDecay = new Material[3];
    public Material[] LowTierMaleDecayed = new Material[3];

    public Material[] LowTierFemaleFresh = new Material[3];
    public Material[] LowTierFemaleLightDecay = new Material[3];
    public Material[] LowTierFemaleDecayed = new Material[3];

    public Material[] MidTierMaleFresh = new Material[3];
    public Material[] MidTierMaleLightDecay = new Material[3];
    public Material[] MidTierMaleDecayed = new Material[3];

    public Material[] MidTierFemaleFresh = new Material[3];
    public Material[] MidTierFemaleLightDecay = new Material[3];
    public Material[] MidTierFemaleDecayed = new Material[3];

    public Material[] HighTierMaleFresh = new Material[3];
    public Material[] HighTierMaleLightDecay = new Material[3];
    public Material[] HighTierMaleDecayed = new Material[3];

    public Material[] HighTierFemaleFresh = new Material[3];
    public Material[] HighTierFemaleLightDecay = new Material[3];
    public Material[] HighTierFemaleDecayed = new Material[3];

    public void GenerateRandomBody(int Decay, GraveQuality Tier)
    {
        DecayLevel = Decay;
        TierOfBody = Tier;
        RandomBodyTexture();
    }


    void RandomBodyTexture()
    {
        if (GameManager.Instance._RunManager.GetMapInfo().GetLevelTier() == GraveyardTier.TUTORIAL)
        {
            BodyMaterial =  MidTierMaleLightDecay[1];
            return;
        }
        int RandomBody = Random.Range(0, 3);
        int MaleFemale = Random.Range(0, 101);
        if (TierOfBody == GraveQuality.HIGH)
        {
            if (DecayLevel >= 60)
            {
                if(MaleFemale <= 50)
                {
                    BodyMaterial = HighTierFemaleFresh[RandomBody];
                }
                else
                {
                    BodyMaterial = HighTierMaleFresh[RandomBody];
                }
            }
            else if (DecayLevel >= 30)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = HighTierFemaleLightDecay[RandomBody];
                }
                else
                {
                    BodyMaterial = HighTierMaleLightDecay[RandomBody];
                }
            }
            else if (DecayLevel >= 10)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = HighTierFemaleDecayed[RandomBody];
                }
                else
                {
                    BodyMaterial = HighTierMaleDecayed[RandomBody];
                }
            }
            else
            {
                BodyMaterial = Skeleton;
            }
        }
        if (TierOfBody == GraveQuality.MEDIUM)
        {
            if (DecayLevel >= 80)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = MidTierFemaleFresh[RandomBody];
                }
                else
                {
                    BodyMaterial = MidTierMaleFresh[RandomBody];
                }
            }
            else if (DecayLevel >= 60)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = MidTierFemaleLightDecay[RandomBody];
                }
                else
                {
                    BodyMaterial = MidTierMaleLightDecay[RandomBody];
                }
            }
            else if (DecayLevel >= 30)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = MidTierFemaleDecayed[RandomBody];
                }
                else
                {
                    BodyMaterial = MidTierMaleDecayed[RandomBody];
                }
            }
            else
            {
                BodyMaterial = Skeleton;
            }
        }
        if (TierOfBody == GraveQuality.LOW)
        {
            if (DecayLevel >= 90)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = LowTierFemaleFresh[RandomBody];
                }
                else
                {
                    BodyMaterial = LowTierMaleFresh[RandomBody];
                }
            }
            else if (DecayLevel >= 70)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = LowTierFemaleLightDecay[RandomBody];
                }
                else
                {
                    BodyMaterial = LowTierMaleLightDecay[RandomBody];
                }
            }
            else if (DecayLevel >= 50)
            {
                if (MaleFemale <= 50)
                {
                    BodyMaterial = LowTierFemaleDecayed[RandomBody];
                }
                else
                {
                    BodyMaterial = LowTierMaleDecayed[RandomBody];
                }
            }
            else
            {
                BodyMaterial = Skeleton;
            }
        }
    }

    public Material ReturnMaterial()
    {
        return BodyMaterial;
    }
}
