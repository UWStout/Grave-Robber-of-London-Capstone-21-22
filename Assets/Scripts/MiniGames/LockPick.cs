using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockPick : MonoBehaviour, IDragHandler
{
    bool CanClick = true;
    public Canvas CurCanvas;
    Vector3 Home;

    void OnEnable()
    {
        Home = transform.position;
        CurCanvas = transform.parent.parent.parent.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData data)
    {
        Debug.Log("clunk");
        if (CanClick)
        {
            Debug.Log("click");
            transform.localPosition += new Vector3((data.delta.x / CurCanvas.scaleFactor) * 2.5f, (data.delta.y / CurCanvas.scaleFactor) * 2.5f, 0);
        }
        else
        {
            data.pointerDrag = null;
        }
    }

    public void ReturnHome()
    {
        CanClick = false;
        //Debug.Log("halt youve broken the law");
        transform.position = Home;
        StartCoroutine(LockOutTime());
    }

    private IEnumerator LockOutTime()
    {
        yield return new WaitForSeconds(.2f);
        CanClick = true;
    }
}
