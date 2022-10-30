using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{

    private SaveData Data = new SaveData();

    public void SaveToJson()
    {
        //TODO: Get all information form the scripts before saving.
        Data.Money = GameManager.Instance.GetMoney();
        Data.MoneyCap = GameManager.Instance.GetMoneyCap();
        Data.BodyCollection = GameManager.Instance.GetBodyCollection();
        Data.Quests = GameManager.Instance._QuestManager.GetQuests();
        Data.TreasureCollection = GameManager.Instance.GetTreasureCollection();
        Data._Upgrades = GameManager.Instance._UpgradeManager.Convert();

        string SaveData = JsonUtility.ToJson(Data, true);
        File.WriteAllText(Application.persistentDataPath + "/GameSave.json", SaveData);
    }

    public void LoadFromJson()
    {
        if (File.Exists(Application.persistentDataPath + "/GameSave.json"))
        {
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/GameSave.json");
            SaveData SaveInfo = JsonUtility.FromJson<SaveData>(fileContents);
            
            GameManager.Instance.UpdateMoney(SaveInfo.Money);
            GameManager.Instance.SetMoneyCap(SaveInfo.MoneyCap);
            GameManager.Instance.SetBodyCollection(SaveInfo.BodyCollection);
            GameManager.Instance._QuestManager.SetQuests(SaveInfo.Quests);
            GameManager.Instance.SetTreasureCollection(Data.TreasureCollection);
            GameManager.Instance._UpgradeManager.Convert(SaveInfo._Upgrades);
        }
        else
        {
            Debug.Log("File Doesn't exist");
        }
        
        //TODO: Set all values to the one in the scripts
    }
};

[System.Serializable]
class SaveData
{
    [SerializeField] public int Money = 50;
    [SerializeField] public int MoneyCap = 200;
    [SerializeField] public Body[] BodyCollection;
    [SerializeField] public Treasure[] TreasureCollection;
    [SerializeField] public Quest[] Quests;
    [SerializeField] public Upgrades _Upgrades;
};