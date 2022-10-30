using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectHolder : MonoBehaviour
{
    public MapInformation[] Maps;

    public MapInformation[] GetRandomMaps(int _Size)
    {
        MapInformation[] RandMaps = new MapInformation[_Size];
        bool[] TakenMaps = new bool[Maps.Length];
        if (_Size > Maps.Length)
        {
            return RandMaps;
        }

        for (int i = 0; i < _Size; i++)
        {
            bool Found = false;

            while (!Found)
            {
                int RandMap = Random.Range(0, Maps.Length);

                if (!TakenMaps[RandMap])
                {
                    TakenMaps[RandMap] = true;
                    RandMaps[i] = Maps[RandMap];
                    Found = true;
                }
            }
        }

        return RandMaps;
    }
}