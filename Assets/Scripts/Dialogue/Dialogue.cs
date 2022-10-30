using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The controller that manages a single dialgue script.
/// </summary>
/// <remarks>
/// * _The dialogue script can be swapped out at run time_
/// </remarks>
[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [Header("UI Elements")]
    public Image Portrait; /*! Reference to the Portrait UI*/
    public TMP_Text Title; /*! Reference to the Title UI*/
    public TMP_Text Line; /*! Reference to the Line UI*/

    [Header("Variables")]
    //Line incrementor
    [SerializeField] private int LineNumber = 0; /*! The current line number the dialogue is on*/
    private List<string[]> DialogueVars = new List<string[]>(); /*! A list of all symbols which should be replaced by a variable in text*/

    [Header("Give Quest Variables")]
    [SerializeField] private bool HasQuest = false; /*! Whether the quest is arleady given*/
    [SerializeField] private int QuestIndex = 0; /*! The index that the quest will be given*/
    [SerializeField] private Quests NewQuest; /*! Reference to what quest will be given*/

    //---------------------------

    [Header("Lists")]
    [SerializeField] private Sprite[] Portraits; /*! List of sprites used as portraits*/
    [SerializeField] private Script Script; /*! Reference to the Portrait UI*/

    private bool interactButton = false; /*! Flag to make axis act as button*/

    /// <summary>
    /// Called on Script enable - will update the UI with the starting information
    /// </summary>
    public void OnEnable()
    {
        SetDialogueUI(Script.GetLine(LineNumber)); /*! Sets the dialogue UI to the current line*/
    }
    

    /// <summary>
    /// Will set the dialogue script based on what is passed
    /// </summary>
    /// <param name="_Script">New dialogue script to be assigned as reference.</param>
    public void SetScript(Script _Script)
    {
        Script = _Script;
        LineNumber = 0;
        SetDialogueUI(Script.GetLine(LineNumber));
    }

    public void SetScriptVars(Script _Script)
    {
        Script = _Script;
        LineNumber = 0;
        Line TempLine = ReplaceVars(Script.GetLine(LineNumber));

        SetDialogueUI(TempLine);
    }

    /// <summary>
    /// Updates the Dialogue UI based on the current line passed in
    /// </summary>
    /// <param name="_Line">The current line information to be displayed</param>
    /// <returns>true or false on whether a valid line was passed in</returns>
    public bool SetDialogueUI(Line _Line)
    {
        //Check to see if at end of file
        if(_Line != null)
        {
            //Condition statements allow for modularity of UI elements
            if(Portrait != null)
            { 
                Portrait.sprite = Portraits[_Line.Portrait];
            }
            if(Title != null)
            {
                Title.text = _Line.Title;
            }
            if(Line != null)
            {
                Line.text = _Line.ScriptLine;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// If not on last line update to the next line in the script
    /// 
    /// Also if on the line that a quest should be given, give that quest.
    /// </summary>
    /// <returns>True if line has been progressed</returns>
    public bool NextLine()
    {
        if(LineNumber + 1 < Script.GetNumLines())
        {
            LineNumber++;
            SetDialogueUI(Script.GetLine(LineNumber));
            if(QuestIndex == LineNumber && HasQuest)
            {
                GameManager.Instance.CreateQuest(NewQuest); /*! Will pass the quest to the game manager to be accepted*/
                HasQuest = false;
            }
            return true;
        }
        return false;
    }

    public bool NextLineVars()
    {
        if (LineNumber + 1 < Script.GetNumLines())
        {
            LineNumber++;
            Line TempLine = Script.GetLine(LineNumber);
            Line CopyLine = new Line(TempLine.Title, TempLine.ScriptLine, TempLine.Portrait);
            CopyLine = ReplaceVars(CopyLine);

            SetDialogueUI(CopyLine);
            if (QuestIndex == LineNumber && HasQuest)
            {
                GameManager.Instance.CreateQuest(NewQuest); /*! Will pass the quest to the game manager to be accepted*/
                HasQuest = false;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get the current line number
    /// </summary>
    /// <returns>The line number of the current line</returns>
    public int GetLineNumber()
    {
        return LineNumber;
    }

    /// <summary>
    /// Will reset the line number and UI to be back at the start of the dialogue
    /// </summary>
    public void ResetDialogue()
    {
        LineNumber = 0;
        SetDialogueUI(Script.GetLine(LineNumber));
    }

    /// <summary>
    /// Will set a random line to the current line displayed
    /// </summary>
    public void GetRandom()
    {
        SetDialogueUI(Script.GetRandomLine());
    }

    /// <summary>
    /// Getter to check if at the end of the dialogue
    /// </summary>
    /// <returns>True if current line is last line, 
    /// false if not last line</returns>
    public bool IsEnd()
    {
        return LineNumber >= Script.GetNumLines();
    }

    private Line ReplaceVars(Line _Inp)
    {
        foreach (string[] V in DialogueVars)
        {
            _Inp.ScriptLine = _Inp.ScriptLine.Replace(V[0], V[1]);
        }

        return _Inp;
    }

    /// <summary>
    /// Adds a new text replacement variable to the list. Replaces it in the list if it already exists.
    /// </summary>
    /// <param name="_Val">
    /// The variable to be added.
    /// </param>
    /// <returns>
    /// True if the variable was already in the list, false otherwise.
    /// </returns>
    public bool AddVar(string[] _Val)
    {
        bool Replacement = false;

        for (int i = 0; i < DialogueVars.Count; i++)
        {
            if (DialogueVars[i][0] == _Val[0])
            {
                DialogueVars[i][1] = _Val[1];
                Replacement = true;
            }
        }

        if (!Replacement)
        {
            DialogueVars.Add(_Val);
        }

        return Replacement;
    }
}
