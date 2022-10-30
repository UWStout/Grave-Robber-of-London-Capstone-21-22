using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningCutsceneScript : MonoBehaviour
{
    public GameObject PlayerSprite;
    public GameObject MentorSprite;
    public MapInformation TutorialObject;

    private bool Moving = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveSprites());
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            PlayerSprite.transform.Translate(Vector3.right * Time.deltaTime * 22.0f);
            MentorSprite.transform.Translate(Vector3.right * Time.deltaTime * 20.0f);
        }
    }

    private IEnumerator MoveSprites()
    {
        UIManager.Instance.DisableBlackScreen();
        UIManager.Instance.DisableGameUI();
        yield return new WaitForSeconds(0.5f);
        Moving = true;
        yield return new WaitForSeconds(6.0f);
        UIManager.Instance.EnableBlackScreen();
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.EnableGameUI();
        GameManager.Instance._RunManager.SetMapInfo(TutorialObject);
        GameManager.Instance.ChangeScene(SceneName.Tutorial);
    }
}
