using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ChangeScene(SceneName.Title);
    }
}
