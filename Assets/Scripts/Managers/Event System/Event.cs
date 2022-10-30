using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] private string prompt;
    //    [SerializeField] private 
    Dictionary<string, List<string>> JsonData = new Dictionary<string, List<string>>();
}
