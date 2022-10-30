using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThanosScript : MonoBehaviour
{
    public GameObject BuyCanvas;
    public Script ThanosDialogue;

    private bool Talking = false;
    private bool PlayerNear = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (GameManager.Instance.GetMoney() < 150)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (Talking && !GameManager.Instance._DialogueManager.NextLineVars())
            {
                Instantiate(BuyCanvas, Vector3.zero, Quaternion.identity);
                Talking = false;
            }

            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCourtyard>().GetPaused() && PlayerNear)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCourtyard>().SetPaused(true);

                GameManager.Instance._DialogueManager.SetScriptVars(ThanosDialogue);
                GameManager.Instance._DialogueManager.ResetDialogue();
                UIManager.Instance.DialogueEnable();
                Talking = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNear = false;
        }
    }
}
