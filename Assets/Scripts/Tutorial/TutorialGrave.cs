using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGrave : GraveInfoRefactor
{
    private bool Diggable = false;

    private GameObject GraveArrow; // Arrow prefab pointing down at grave, child object

    protected override void Update()
    {
        if (Diggable)
        {
            base.Update();
        }
    }

    protected override void GrabBody()
    {
        base.GrabBody();
        if (GraveArrow != null)
        {
            GraveArrow.SetActive(false);
        }
    }

    protected override void HighLight()
    {
        if (Diggable)
        {
            base.HighLight();
        }
    }

    public void CopyPublicVars()
    {
        GraveInfoRefactor TempScript = transform.GetComponent<GraveInfoRefactor>();
        DigPrompt = TempScript.DigPrompt;
        DugGrave = TempScript.DugGrave;
        GraveMound = TempScript.GraveMound;
        DeadBody = TempScript.DeadBody;
        MiniGameList = TempScript.MiniGameList;
        Grave = TempScript.Grave;
        Grey = TempScript.Grey;
        Empty = TempScript.Empty;
    }

    public void SetDiggable(bool Val)
    {
        Diggable = Val;
        if (GraveArrow != null)
        {
            GraveArrow.SetActive(Val);
        }
    }

    public void SetArrow(GameObject Arr)
    {
        GraveArrow = Arr;
        GraveArrow.transform.parent = this.transform;
        GraveArrow.transform.localPosition = new Vector3(0, 2.0f, 0);
        GraveArrow.SetActive(false);
    }
}
