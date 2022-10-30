using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedallionControl : MonoBehaviour
{
    public Image UpSpike;
    public Image LeftSpike;
    public Image RightSpike;
    public Image DownSpike;

    public Sprite[] UpMats;
    public Sprite[] LeftMats;
    public Sprite[] RightMats;
    public Sprite[] DownMats;

    public void ToggleSpikes(bool _Val)
    {
        UpSpike.enabled = _Val;
        LeftSpike.enabled = _Val;
        RightSpike.enabled = _Val;
        DownSpike.enabled = _Val;
    }

    public bool SetSpike(Direction Dir, int _Val)
    {
        switch (Dir)
        {
            case Direction.UP:
                if (_Val >= 0 && _Val < UpMats.Length)
                {
                    UpSpike.sprite = UpMats[_Val];
                    return true;
                }
                break;
            case Direction.LEFT:
                if (_Val >= 0 && _Val < LeftMats.Length)
                {
                    LeftSpike.sprite = LeftMats[_Val];
                    return true;
                }
                break;
            case Direction.RIGHT:
                if (_Val >= 0 && _Val < RightMats.Length)
                {
                    RightSpike.sprite = RightMats[_Val];
                    return true;
                }
                break;
            case Direction.DOWN:
                if (_Val >= 0 && _Val < DownMats.Length)
                {
                    DownSpike.sprite = DownMats[_Val];
                    return true;
                }
                break;
        }

        return false;
    }
}
