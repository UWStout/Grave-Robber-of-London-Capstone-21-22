/*
 * @author: Damian Link
 * @version: 11/18/21
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Controls the trip wire minigame, V2 
/// </summary>
public class TripWireMiniGameDepricated2 : MonoBehaviour
{
    //// side of the screen the object is on
    //public enum sides { Right, Left };
    //// collider for detecting when mouse crosses a line
    //public SphereCollider MouseCollider = new SphereCollider();
    //// if mouse is pressed
    //public bool MousePressed = false;
    //// list of the starting points
    ////public List<TripwireStartPoints> StartingPoints;

    //// total number of wires
    //private int WireCount = 3;
    //// number of Completed wires
    //public int CompletedWires = 0;

    ///// <summary>
    ///// checks if the minigame was won
    ///// </summary>
    ///// <returns>true if won/false if not finished</returns>
    //public bool CheckWin()
    //{
    //    return (CompletedWires == WireCount);
    //}

    ///// <summary>
    ///// sets the Completed wires to number of buttons(Automatic win)
    ///// </summary>
    //public void ForceWin()
    //{
    //    CompletedWires = WireCount;
    //}

    ///// <summary>
    ///// sets listeners for the buttons
    ///// </summary>
    //private void Start()
    //{
    //    // adds colliders to lines
    //    AddColliderToLine(GameObject.Find("XLine").GetComponent<LineRenderer>());
    //    AddColliderToLine(GameObject.Find("ALine").GetComponent<LineRenderer>());
    //    AddColliderToLine(GameObject.Find("OLine").GetComponent<LineRenderer>());

    //    // adds collider/rigidbody for mouse
    //    SphereCollider MouseCol = new GameObject("MouseCollider").AddComponent<SphereCollider>();
    //    MouseCol.isTrigger = true;
    //    MouseCol.transform.parent = GameObject.Find("TripWireMiniGame").transform;
    //    MouseCol.transform.localScale = new Vector3(1, 1, 1);
    //    MouseCol.radius = 10;
    //    Rigidbody MouseRbody = MouseCol.gameObject.AddComponent<Rigidbody>();
    //    MouseRbody.isKinematic = true;
    //    MouseCollider = MouseCol;
    //    MouseCol.gameObject.AddComponent<TripWireMiniGameTrigger>();
    //}

    ///// <summary>
    ///// moves collider based on given positions
    ///// </summary>
    ///// <param name="RightPos">Right position of line</param>
    ///// <param name="LeftPos">Left position of line</param>
    ///// <param name="col">collider to move</param>
    //public void MoveCollider(Vector3 RightPos, Vector3 LeftPos, BoxCollider col)
    //{
    //    col.size = new Vector3(Vector3.Distance(LeftPos, RightPos), 10f, 5f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
    //    col.transform.localPosition = (RightPos + LeftPos) / 2;
    //    // Following lines calculate the angle between startPos and endPos
    //    float angle = (Mathf.Abs(RightPos.y - LeftPos.y) / Mathf.Abs(RightPos.x - LeftPos.x));
    //    if ((RightPos.y < LeftPos.y && RightPos.x > LeftPos.x) || (LeftPos.y < RightPos.y && LeftPos.x > RightPos.x))
    //    {
    //        angle *= -1;
    //    }
    //    col.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan(angle));
    //}

    ///// <summary>
    ///// moves collider based on given positions
    ///// </summary>
    ///// <param name="RightPos">Right position of line</param>
    ///// <param name="LeftPos">Left position of line</param>
    ///// <param name="col">collider to move</param>
    //public void MoveLine(Vector3 RightPos, Vector3 LeftPos, Image Img)
    //{
    //    Img.rectTransform.sizeDelta = new Vector2(Vector3.Distance(LeftPos, RightPos), 10);
    //    //Img.size = new Vector3(Vector3.Distance(LeftPos, RightPos), 10f, 5f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
    //    Img.transform.localPosition = (RightPos + LeftPos) / 2;
    //    // Following lines calculate the angle between startPos and endPos
    //    float angle = (Mathf.Abs(RightPos.y - LeftPos.y) / Mathf.Abs(RightPos.x - LeftPos.x));
    //    if ((RightPos.y < LeftPos.y && RightPos.x > LeftPos.x) || (LeftPos.y < RightPos.y && LeftPos.x > RightPos.x))
    //    {
    //        angle *= -1;
    //    }
    //    Img.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan(angle));
    //}

    ///// <summary>
    ///// adds collider to to the line
    ///// </summary>
    ///// <param name="line">line to add collider to</param>
    //private void AddColliderToLine(LineRenderer line)
    //{

    //    // collider for line
    //    BoxCollider BoxCol = new GameObject("Collider").AddComponent<BoxCollider>();

    //    // adds the collider box collider with correct size
    //    BoxCol.transform.parent = line.transform;
    //    MoveCollider(line.GetPosition(0), line.GetPosition(1), BoxCol);
    //    BoxCol.isTrigger = true;
    //    BoxCol.transform.localScale = new Vector3(1, 1, 1);

    //    // adds a rigidbody to the collider
    //    Rigidbody Rbody = BoxCol.gameObject.AddComponent<Rigidbody>();
    //    Rbody.isKinematic = true;
    //}

    //private GameObject AddLine(string LineName, Vector3 RightPos, Vector3 LeftPos)
    //{
    //    GameObject NewLine = new GameObject(LineName);

    //    Image LineImage = NewLine.AddComponent<Image>();
    //    MoveLine(RightPos, LeftPos, LineImage);

    //    BoxCollider BoxCol = NewLine.AddComponent<BoxCollider>();
    //    MoveCollider(RightPos, LeftPos, BoxCol);

    //    return NewLine;
    //}
}