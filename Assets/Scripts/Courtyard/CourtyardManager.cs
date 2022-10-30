using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Joseph Vang
/// 2.23.2022
/// </summary>
public class CourtyardManager : MonoBehaviour
{

	/// <summary>
	/// When called from Courtyard Collisions script it will set the Notice Board UI 
	/// to True and the interaction indicator to false
	/// </summary>
	public void BoardOn()
    {
		GameObject InteractIndicator = transform.GetChild(0).gameObject;
		GameObject NoticeBoardUI = transform.GetChild(1).gameObject;
		//InteractIndicator.SetActive(false);
		NoticeBoardUI.SetActive(true);
	}

	/// <summary>
	/// When called from Courtyard Collisions script it will set the Notice Board UI 
	/// to false and the interaction indicator to true
	/// </summary>
	public void BoardOff()
    {
		GameObject InteractIndicator = transform.GetChild(0).gameObject;
		GameObject NoticeBoardUI = transform.GetChild(1).gameObject;
		//InteractIndicator.SetActive(true);
		NoticeBoardUI.SetActive(false);
    }



}

