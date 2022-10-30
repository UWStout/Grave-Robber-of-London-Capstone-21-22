/*
 * @ author: Damian Link
 * @ version: 11/18/21
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// handles the dragging of the lines in the tripwire minigame
/// </summary>
public class TripwireStartPointsDepricated : MonoBehaviour  //, IDragHandler, IBeginDragHandler, IEndDragHandler 
{
    //// line that corisponds to the object
    //[SerializeField] private LineRenderer AttachedLine;
    //// side that the object is on
    ////[SerializeField] private TripWireMiniGame.sides AttachedSide;
    //// canvas showing the minigame
    //public Canvas Canvas;
    //// where the line was before moving it
    //private Vector3 PreviousLinePos = Vector3.zero;
    //// the tripwire minigame contoling it
    ////public TripWireMiniGame minigame;

    ///// <summary>
    ///// begins when dragging starts, sets the starting position
    ///// </summary>
    ///// <param name="eventData">Data about the pointer event</param>
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    //minigame.MousePressed = true;
    //    //PreviousLinePos = AttachedLine.GetPosition((int)AttachedSide);
    //}

    ///// <summary>
    ///// moves the position of the line when moving
    ///// </summary>
    ///// <param name="eventData">Data about the pointer event</param>
    //public void OnDrag(PointerEventData eventData)
    //{
    //    if (false)
    //    {
    //        // moves the attached line end posistion
    //        //AttachedLine.SetPosition((int)AttachedSide, AttachedLine.GetPosition((int)AttachedSide) + new Vector3(eventData.delta.x, eventData.delta.y, 0));
    //        //AttachedLine.SetPosition((int)AttachedSide, AttachedLine.GetPosition((int)AttachedSide) + new Vector3(eventData.delta.x / Canvas.scaleFactor,  eventData.delta.y / Canvas.scaleFactor, 0));
    //        // moves the sphere collider
    //        //minigame.MouseCollider.transform.localPosition = AttachedLine.GetPosition((int)AttachedSide);

    //        // moves collider for line
    //        //minigame.MoveCollider(AttachedLine.GetPosition(0), AttachedLine.GetPosition(1), AttachedLine.transform.GetChild(0).GetComponent<BoxCollider>());

    //        // Disables the current line when gets to the other side 
    //        if (Vector3.Distance(AttachedLine.GetPosition(0), AttachedLine.GetPosition(1)) < 25)
    //        {
    //            //AttachedLine.enabled = false;
    //            AttachedLine.gameObject.SetActive(false);
    //            //minigame.
    //            //    += 1;
    //        }
    //    }
    //    else
    //    {
    //        // cancels the drag
    //        eventData.pointerDrag = null;
    //        // moves position back to starting point
    //        //AttachedLine.SetPosition((int)AttachedSide, PreviousLinePos);
    //    } 
    //}

    ///// <summary>
    ///// resets position of line when drag is released
    ///// </summary>
    ///// <param name="eventData">Data about the pointer event</param>
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    //minigame.MousePressed = false;
    //    //AttachedLine.SetPosition((int)AttachedSide, PreviousLinePos);
    //}
}
