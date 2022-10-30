/*
 * @ author: Damian Link
 * @ version: 11/24/21
 */
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// handles the dragging of the lines in the tripwire minigame
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * minigame - The instance of the tripwiwre minigame being used
/// * CurCavas - the canvas that is holding the minigame
/// </remarks>
public class TripwireStartPoints : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    
    // line that corisponds to the object
    [SerializeField] private Image AttachedLine;
    // side that the object is on
    [SerializeField] private TripWireMiniGame.sides AttachedSide;
    // canvas showing the minigame
    public Canvas CurCanvas;
    // where the line was before moving it
    private Vector3 PreviousLinePos = Vector3.zero;
    // the tripwire minigame contoling it
    public TripWireMiniGame minigame;
    [SerializeField] private TripwireStartPoints OppPoint;

    /// <summary>
    /// gets the current canvas being used
    /// </summary>
    void Start()
    {
        CurCanvas = transform.parent.parent.parent.GetComponent<Canvas>();
    }

    /// <summary>
    /// begins when dragging starts, sets the starting position
    /// </summary>
    /// <param name="eventData">Data about the pointer event</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        minigame.MousePressed = true;
        PreviousLinePos = transform.localPosition;
        minigame.MouseCollider.GetComponent<TripWireMiniGameTrigger>().CurrentLineName = AttachedLine.name;
        minigame.MouseCollider.transform.localPosition = transform.localPosition;
    }

    /// <summary>
    /// moves the position of the line when moving
    /// </summary>
    /// <param name="eventData">Data about the pointer event</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (minigame.MousePressed)
        {
            // moves the sphere collider
            minigame.MouseCollider.transform.localPosition += new Vector3(eventData.delta.x / CurCanvas.scaleFactor, eventData.delta.y / CurCanvas.scaleFactor, 0);

            // moves the attached line end posistion / moves collider for line
            if (AttachedSide == TripWireMiniGame.sides.Left)
            {
                minigame.MoveLine(OppPoint.transform.localPosition, minigame.MouseCollider.transform.localPosition, AttachedLine);
            }
            else
            {
                minigame.MoveLine(minigame.MouseCollider.transform.localPosition, OppPoint.transform.localPosition, AttachedLine);
            }

            // Disables the current line when gets to the other side 
            if (AttachedLine.rectTransform.sizeDelta.x < 25 && AttachedLine.IsActive() == true)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Cord Tie");
                AttachedLine.gameObject.SetActive(false);
                minigame.CompletedWires += 1;
            }
        }
        else
        {
            // cancels the drag
            eventData.pointerDrag = null;
            // moves position back to starting point
            if (AttachedSide == TripWireMiniGame.sides.Left)
            {
                minigame.MoveLine(OppPoint.transform.localPosition, PreviousLinePos, AttachedLine);
            }
            else
            {
                minigame.MoveLine(PreviousLinePos, OppPoint.transform.localPosition, AttachedLine);
            }
        }
    }

    /// <summary>
    /// resets position of line when drag is released
    /// </summary>
    /// <param name="eventData">Data about the pointer event</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        minigame.MousePressed = false;
        if (AttachedSide == TripWireMiniGame.sides.Left)
        {
            minigame.MoveLine(OppPoint.transform.localPosition, PreviousLinePos, AttachedLine);
        }
        else
        {
            minigame.MoveLine(PreviousLinePos, OppPoint.transform.localPosition, AttachedLine);
        }
    }
}