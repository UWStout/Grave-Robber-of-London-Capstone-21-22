using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalLevelBuyButtons : MonoBehaviour
{
    public GameObject YesButton;
    public GameObject NoButton;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.GetMoney() >= 500)
        {
            YesButton.GetComponent<Button>().interactable = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AcceptBuy()
    {
        UIManager.Instance.DialogueDisable();
        GameManager.Instance.UpdateMoney(-500);
        StartCoroutine(GameManager.Instance._SceneManager.ChangeSceneFade(SceneName.QueensApproach));
    }

    public void DeclineBuy()
    {
        UIManager.Instance.DialogueDisable();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCourtyard>().SetPaused(false);
        Destroy(gameObject);
    }
}
