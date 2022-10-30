using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestNewButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject buttonParent;
    public GameObject buttonPrefab;

    public List<GameObject> buttonList = new List<GameObject>();
    public void NewButton()
    {
        //Add to the Quest list
        
        GameObject NewButton = (GameObject)Instantiate(buttonPrefab, buttonParent.transform);
        
        buttonList.Add(NewButton);

        NewButton.GetComponentInChildren<TextMeshProUGUI>().text = "Print Index";

        NewButton.GetComponent<Button>().onClick.AddListener(() => Print(buttonList.Count - 1));
    }

    public static void Print(int index)
    {
        Debug.Log($"button index {index}");
    }
}
