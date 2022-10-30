using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.EventSystems;

/**
 * @author Damian Link
 * 
 * @version 10/29/2021
 */
/// <summary>
/// controls the tripwire minigame
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * sides - enum of sides(right or left)
/// * RightButtons - buttons for the starting right point of the line
/// * LeftButtons - buttons for the starting left point of the line
/// </remarks>
public class TripWireMiniGameDepricated1 : MonoBehaviour
{
    //// side of the screen the object is on
    //public enum sides { Right, Left };
    //// Holds currrent line being held
    //private LineRenderer CurrentLine = new LineRenderer();
    //// Holds the position of the current side being moved. 1=Left, 0=Right
    //private sides CurrentSide = sides.Left;
    //// collider for detecting when mouse crosses a line
    //private SphereCollider MouseCollider = new SphereCollider();
    
    //// if mouse is pressed
    //public bool MousePressed = false;
    //// location of previous mouse before moving it
    //private Vector3 PreviousMousePos = Vector3.zero;
    //// where the line was before moving it
    //private Vector3 PreviousLinePos = Vector3.zero;

    //// total number of wires
    //private int WireCount = 3;
    //// number of disarmed wires
    //private int CompletedWires = 0;

    //// buttons for the starting points of the lines
    //public List<Button> RightButtons = new List<Button>();
    //public List<Button> LeftButtons = new List<Button>();

    ///// <summary>
    ///// resets the mouse info back to default
    ///// </summary>
    //public void ResetMouseInfo()
    //{
    //    if (PreviousLinePos != Vector3.zero)
    //    {
    //        CurrentLine.SetPosition((int)CurrentSide, PreviousLinePos);
    //        MousePressed = false;
    //        PreviousLinePos = Vector3.zero;
    //        PreviousMousePos = Vector3.zero;
    //        CurrentLine = new LineRenderer();
    //    }
    //}

    ///// <summary>
    ///// gets current Side
    ///// </summary>
    ///// <returns> the current side of the line being messed with</retu
    //public sides GetCurrentSide()
    //{
    //    return CurrentSide;
    //}

    ///// <summary>
    ///// gets current line
    ///// </summary>
    ///// <returns> the current line being messed with</returns>
    //public LineRenderer GetCurrentLine()
    //{
    //    return CurrentLine;
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
    //    // sets rights listeners
    //    for (int i = 0; i < RightButtons.Count; i++)
    //    {
    //        if (RightButtons[i].gameObject.name.Contains("A"))
    //        {
    //            RightButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("ALine").GetComponent<LineRenderer>()));
    //        } else if (RightButtons[i].gameObject.name.Contains("O"))
    //        {
    //            RightButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("OLine").GetComponent<LineRenderer>()));
    //        } else if (RightButtons[i].gameObject.name.Contains("X"))
    //        {
    //            RightButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("XLine").GetComponent<LineRenderer>()));
    //        }
    //    }
    //    // sets lefts listeners
    //    for (int i = 0; i < LeftButtons.Count; i++)
    //    {
    //        if (LeftButtons[i].gameObject.name.Contains("A"))
    //        {
    //            LeftButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("ALine").GetComponent<LineRenderer>()));
    //        }
    //        else if (LeftButtons[i].gameObject.name.Contains("O"))
    //        {
    //            LeftButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("OLine").GetComponent<LineRenderer>()));
    //        }
    //        else if (LeftButtons[i].gameObject.name.Contains("X"))
    //        {
    //            LeftButtons[i].onClick.AddListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("XLine").GetComponent<LineRenderer>()));
    //        }
    //    }

    //    // checks if buttons are equal on each side
    //    if (LeftButtons.Count == RightButtons.Count)
    //    {
    //        WireCount = LeftButtons.Count;
    //    } else
    //    {
    //        Debug.LogError("Sides Are Not Even On Trip Wire Minigame");
    //    }

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
    //private void MoveCollider(Vector3 RightPos, Vector3 LeftPos, BoxCollider col)
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

    ///// <summary>
    ///// moves the line
    ///// </summary>
    //private void MoveLine ()
    //{
    //    // moves the main line end posistion
    //    Vector3 DeltaMousePos = Vector3.zero;
    //    if (PreviousMousePos != Vector3.zero)
    //    {
    //        DeltaMousePos = Input.mousePosition - PreviousMousePos;
    //    }
    //    PreviousMousePos = Input.mousePosition;
    //    CurrentLine.SetPosition((int)CurrentSide, CurrentLine.GetPosition((int)CurrentSide) + DeltaMousePos);

    //    // moves the sphere collider
    //    MouseCollider.transform.localPosition = CurrentLine.GetPosition((int)CurrentSide);

    //    // moves collider for line
    //    MoveCollider(CurrentLine.GetPosition(0), CurrentLine.GetPosition(1), CurrentLine.transform.GetChild(0).GetComponent<BoxCollider>());
    //}

    ///// <summary>
    ///// removes the listeners from the buttons
    ///// </summary>
    //private void Destroy()
    //{
    //    for (int i = 0; i < RightButtons.Count; i++)
    //    {
    //        if (RightButtons[i].gameObject.name.Contains("A"))
    //        {
    //            RightButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("ALine").GetComponent<LineRenderer>()));
    //        }
    //        else if (RightButtons[i].gameObject.name.Contains("O"))
    //        {
    //            RightButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("OLine").GetComponent<LineRenderer>()));
    //        }
    //        else if (RightButtons[i].gameObject.name.Contains("X"))
    //        {
    //            RightButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Right, GameObject.Find("XLine").GetComponent<LineRenderer>()));
    //        }
    //    }
    //    // sets lefts listeners
    //    for (int i = 0; i < LeftButtons.Count; i++)
    //    {
    //        if (LeftButtons[i].gameObject.name.Contains("A"))
    //        {
    //            LeftButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("ALine").GetComponent<LineRenderer>()));
    //        }
    //        else if (LeftButtons[i].gameObject.name.Contains("O"))
    //        {
    //            LeftButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("OLine").GetComponent<LineRenderer>()));
    //        }
    //        else if (LeftButtons[i].gameObject.name.Contains("X"))
    //        {
    //            LeftButtons[i].onClick.RemoveListener(() => OnCLickLineSetting(sides.Left, GameObject.Find("XLine").GetComponent<LineRenderer>()));
    //        }
    //    }
    //}

    ///// <summary>
    ///// on click function that handles setting of the line and side that is being used
    ///// </summary>
    ///// <param name="LineRender">the lineRenderer of that corrisponds to the clicked button</param>
    ///// <param name="ClickedButton">the button that was clicked</param>
    //public void OnCLickLineSetting (sides Side, LineRenderer LineRender)
    //{
    //    CurrentLine = LineRender;
    //    CurrentSide = Side;
    //    PreviousLinePos = Vector3.zero;
    //    if (CurrentLine != null)
    //    {
    //        PreviousLinePos = LineRender.GetPosition((int)CurrentSide);
    //    }
    //}

    ///// <summary>
    ///// checks if the minigame was won
    ///// </summary>
    ///// <returns>true if won/false if not finished</returns>
    //public bool CheckWin()
    //{
    //    return (CompletedWires == WireCount);
    //}

    ///// <summary>
    ///// changes position of the line
    ///// </summary>
    //private void Update()
    //{
    //    // empty line renderer to check against
    //    LineRenderer EmptyLineRenderer = new LineRenderer();

    //    // Disables the current line when gets to the other side
    //    if (CurrentLine != EmptyLineRenderer)
    //    {
    //        if (Vector3.Distance(CurrentLine.GetPosition(0), CurrentLine.GetPosition(1)) < 25)
    //        {
    //            CurrentLine.enabled = false;
    //            CompletedWires += 1;
    //            ResetMouseInfo();
    //        }
    //    }

    //    // detects when mouse is clicked
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        MousePressed = true;
    //    }

    //    // detects when mouse is released
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        MousePressed = false;
    //    }

    //    if (CurrentLine != EmptyLineRenderer && MousePressed == false)
    //    {
    //        if (Vector3.Distance(CurrentLine.GetPosition((int)CurrentSide), PreviousLinePos) > 25)
    //        {
    //            CurrentLine.SetPosition((int)CurrentSide, PreviousLinePos);
    //            ResetMouseInfo();
    //        }
    //    }
        
    //    // changes the position of the current line to the mouse position
    //    if (MousePressed && CurrentLine != EmptyLineRenderer)
    //    {
    //        MoveLine();
    //    }
    //}
}