/*
 * @author: Damian Link
 * @version: 11/24/21
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the trip wire minigame, V2 
/// </summary>
/// <remarks>
/// ### Variables
/// 
/// * sides - enum storing side of the screen an object is on
/// * MouseCollider - Collider for detecting when mouse crosses a line
/// * MousePressed - If the mouse is being pressed
/// * Starting Points - List of the starting points, Order XX, AA, OO
/// * LineImages - List of lines in image components, Order X, A, O 
/// </remarks>
public class TripWireMiniGame : MiniGame
{
    // side of the screen the object is on
    public enum sides { Right, Left };
    // collider for detecting when mouse crosses a line
    public SphereCollider MouseCollider;
    // if mouse is pressed
    public bool MousePressed = false;
    // list of the starting points, Order XX, AA, OO
    public List<TripwireStartPoints> StartingPoints;
    // list of lines in image components, Order X, A, O
    public List<Image> LineImages;

    // Right side positions of buttons
    [SerializeField] private List<Vector3> RightPos;
    // left side psoitions of buttons
    [SerializeField] private List<Vector3> LeftPos;

    // Text that shows insructions
    public Text InstructText;
    // Instructions that tells player what to do
    private string Instructions = "Click and drag on an end connector without touching the other wires";


    // total number of wires
    private int WireCount = 3;
    // number of Completed wires
    public int CompletedWires = 0;

    /// <summary>
    /// checks if the minigame was won
    /// </summary>
    /// <returns>true if won/false if not finished</returns>
    public override bool CheckWin()
    {
        return (CompletedWires == WireCount);
    }

    /// <summary>
    /// sets the Completed wires to number of buttons(Automatic win)
    /// </summary>
    public void ForceWin()
    {
        CompletedWires = WireCount;
    }

    /// <summary>
    /// sets listeners for the buttons
    /// </summary>
    private void Start()
    {
        // genereates the starting points randomly
        int RandInt = 0;
        for (int i = 0; i < StartingPoints.Count; i+=2)
        {
            RandInt = Random.Range(0, RightPos.Count);
            StartingPoints[i].transform.localPosition = RightPos[RandInt];
            RightPos.RemoveAt(RandInt);

            RandInt = Random.Range(0, LeftPos.Count);
            StartingPoints[i+1].transform.localPosition = LeftPos[RandInt];
            LeftPos.RemoveAt(RandInt);
        }

        InstructText.text = Instructions;

        // Moves the lines to correct positions
        for (int i = 0; i < StartingPoints.Count; i+=2)
        {
            MoveLine(StartingPoints[i].transform.localPosition, StartingPoints[i+1].transform.localPosition, LineImages[i/2]);
        }
    }

    /// <summary>
    /// moves collider based on given positions
    /// </summary>
    /// <param name="AngleVec">The angle of the line</param>
    /// <param name="Pos">Position of the line</param>
    /// <param name="Distance">Length of line, from distance between right and left positions</param>
    /// <param name="col">collider to move</param>
    public void MoveCollider(Vector3 AngleVec, Vector3 Pos, float Distance, BoxCollider col)
    {
        col.size = new Vector3(Distance, 10f, 5f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        col.transform.localPosition = Pos;
        col.transform.eulerAngles = AngleVec;
    }

    /// <summary>
    /// moves collider based on given positions
    /// </summary>
    /// <param name="RightPos">Right position of line</param>
    /// <param name="LeftPos">Left position of line</param>
    /// <param name="col">collider to move</param>
    public void MoveLine(Vector3 RightPos, Vector3 LeftPos, Image Img)
    {
        // Moves the Image of the line
        float Dist = Vector3.Distance(LeftPos, RightPos);
        Img.rectTransform.sizeDelta = new Vector2(Dist, 5);
        //Img.size = new Vector3(Vector3.Distance(LeftPos, RightPos), 10f, 5f); // size of collider is set where X is length of line, Y is width of line, Z will be set as per requirement
        Img.transform.localPosition = (RightPos + LeftPos) / 2;
        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(RightPos.y - LeftPos.y) / Mathf.Abs(RightPos.x - LeftPos.x));
        if ((RightPos.y < LeftPos.y && RightPos.x > LeftPos.x) || (LeftPos.y < RightPos.y && LeftPos.x > RightPos.x))
        {
            angle *= -1;
        }
        Img.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan(angle));

        // Moves the Collider of the line
        MoveCollider(Img.transform.eulerAngles, Img.transform.localPosition, Dist, Img.gameObject.GetComponent<BoxCollider>());
    }
}