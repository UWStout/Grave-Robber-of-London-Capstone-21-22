using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyExplosion : MonoBehaviour
{
    public List<string> TextName;
    public List<string> TextDescription;
    public GameObject[] TreasureList;
    GameObject Treasure;
    float RandomX;
    float RandomZ;
    int RandomItemDescription;
    int TreasureAmount = 1;
    int RandomTreasure;
    
    /// <summary>
    /// Spawns treasure objects around the dug grave.
    /// </summary>
    /// <param name="TreasureAmount"></param>
    public void SpreadTreasure(int TreasureAmount)
    {
        for (int i = 0; i < TreasureAmount; i++)
        {
            RandomTreasure = Random.Range(0, 2);
            RandomItemDescription = Random.Range(0, TextName.Count);
            RandomX = Random.Range(-0.5f, 0.5f);
            RandomZ = Random.Range(-0.5f, 0.5f);
            Vector3 Place = transform.position;
            Place.x += RandomX;
            Place.z += RandomZ;

            Instantiate(TreasureList[RandomTreasure], Place, Quaternion.identity);
        }
    }
    public void TreasurePickUp(GameObject TreasureObject)
    {
        new Treasure(TextName[RandomItemDescription], TreasureAmount, TextDescription[RandomItemDescription]);
        Destroy(TreasureObject);
    }
}
