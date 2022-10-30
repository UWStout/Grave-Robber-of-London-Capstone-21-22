using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTextVisualizer : MonoBehaviour
{
    public string Text = "";
    public TextMesh TextObj;
    private GameObject Player;
    private Animator TextAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Text = Text.Replace("\\n", "\n");
        TextObj.text = Text;
        Player = GameObject.FindGameObjectWithTag("Player");
        TextAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float PlrDistance = Vector3.Distance(this.transform.position, Player.transform.position);
        //Debug.Log(Player.transform.position.x + ", " + Player.transform.position.z);

        if (PlrDistance <= 1.5f) {
            //Debug.Log("Set true");
            TextAnimator.SetBool("Enabled", true);
        }
        else { TextAnimator.SetBool("Enabled", false);
        }

    }
}
