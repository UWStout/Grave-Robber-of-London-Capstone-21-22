using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    

    void Start()
    {
        
    }
    public void StartRun()
    {
      
        GameManager.Instance.StartRun();
        
    }

    public void QuitGame()
    {
        GameManager.Instance.ExitGame();
    }

    private void Update()
    {
       
    }
}
