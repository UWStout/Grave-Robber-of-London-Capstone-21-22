using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTomb : MonoBehaviour
{
    public NewspaperFadeIn NFI;

    private void Start()
    {
        UIManager.Instance.DisableBlackScreen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NFI.SetActiveFade(true);
            StartCoroutine(DelayFreeze());
        }
    }

    private IEnumerator DelayFreeze()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCourtyard>().SetPaused(true);
        yield return new WaitForSeconds(10.0f);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("GameManager");
    }
}
