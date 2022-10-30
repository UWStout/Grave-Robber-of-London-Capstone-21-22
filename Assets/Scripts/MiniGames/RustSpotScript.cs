/*
 * Author: Damian Link
 * Version: 4/8/22
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Changes the cursor for coffin cage
/// </summary>
public class RustSpotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Image for the cursor
    private Image FakeCursorObj;

    /// <summary>
    /// Gets the fake cursor object
    /// </summary>
    private void OnEnable()
    {
        FakeCursorObj = GameObject.Find("FakeCursor").GetComponent<Image>();
    }

    /// <summary>
    /// Changes Cursor when entering the rust spot
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        FakeCursorObj.sprite = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetOpentexture();

        //Debug.Log("Enter");
        //Cursor.SetCursor(GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetOpenSprite().texture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Changes Cursor back to closed cutters
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
        FakeCursorObj.sprite = GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetClosedtexture();
        //Cursor.SetCursor(GameManager.Instance._UpgradeManager.GetCurrentBolltCutters().GetIcon().texture, Vector2.zero, CursorMode.Auto);
    }
}
