using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudTransparancy : MonoBehaviour
{
    public ParticleSystemRenderer DustRightOne;
    public ParticleSystemRenderer DustRightTwo;
    public ParticleSystemRenderer DustLeftOne;
    public ParticleSystemRenderer DustLeftTwo;

    private GameObject Player; // Reference to the player object
    public float MaxDistance = 6; // Distance from the player needs to be before dust starts to become more transparent

    private float BaseOpacity;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        BaseOpacity = DustRightOne.material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerDist = Player.transform.position - transform.position;
        PlayerDist.y = 0;

        if (PlayerDist.magnitude <= MaxDistance)
        {
            Color TempColor = new Color(1.0f, 1.0f, 1.0f, BaseOpacity * Mathf.Pow(PlayerDist.magnitude / MaxDistance, 4));

            DustRightOne.material.color = TempColor;
            DustRightTwo.material.color = TempColor;
            DustLeftOne.material.color = TempColor;
            DustLeftTwo.material.color = TempColor;
        }
    }
}
