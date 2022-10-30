using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonFix : MonoBehaviour
{
    private GameObject PlayerObject;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerObject != null)
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y, PlayerObject.transform.position.z - 5.0f);
        }
    }
}
