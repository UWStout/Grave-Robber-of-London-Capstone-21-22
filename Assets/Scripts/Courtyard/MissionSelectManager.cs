using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Generates the Random Graveyard information for randomizing levels
/// This is used in the Mission Select Screen for making random levels to load into
/// </summary>
public class MissionSelectManager : MonoBehaviour
{
    // This will determine the difficulty of the graveyard
    [SerializeField]
    private int Quality;
    // The list of Buttons with Names
    [SerializeField]
    private Sprite[] NameChoices;
    // The Button that was selected
    [SerializeField]
    private Sprite SelectedName;
    // Number of Guards to be in the Graveyard
    [SerializeField]
    private int NumberOfGuards;
    // Max Number of Bushes to be shown
    [SerializeField]
    private int MaxBushes;
    // Max Number of Trees to be shown
    [SerializeField]
    private int MaxTrees;
    // The List of the Map choices for randomizing
    [SerializeField]
    private Texture2D[] MapChoices;
    // The Map Selected for making
    [SerializeField]
    private Texture2D SelectedMap;

    // Text object for graveyard's name
    [SerializeField]
    private TMPro.TextMeshProUGUI GraveNameObject;
    // The grave info object
    [SerializeField]
    private GameObject GraveInfoObject;
    // The name of the graveyard
    private string GraveName;
    // Number of guards in graveyard
    private int GuardNum;
    // Graveyard's tier as string
    private string GraveTier;

    public void Start()
    {
        //GenerateRandomGraveyards();
        PullInfo();
    }

    // Pulls information from attached graveyardinfo object into this script
    public void PullInfo()
    {
        MapInformation MI = GetComponent<MapInfoHolder>().MI;

        GraveName = MI.GetGraveyardName();
        GuardNum = MI.GetGuardNumber();

        switch (MI.GetLevelTier())
        {
            case GraveyardTier.LOW:
                GraveTier = "Low";
                break;
            case GraveyardTier.MEDIUM:
                GraveTier = "Medium";
                break;
            case GraveyardTier.HIGH:
                GraveTier = "High";
                break;
            default:
                GraveTier = "I made a whoopsie.";
                break;
        }

        GraveNameObject.text = GraveName;
    }

    // Sets the information in the mission info UI
    public void PassInfo()
    {
        GraveInfoObject.GetComponent<MissionInfoUI>().SetText(GraveName, GuardNum, GraveTier);
    }

    public void GenerateRandomGraveyards()
    {
        // Generating a random number to determine graveyard info
        Quality = Random.Range(0, 3);
        switch (Quality)
        {
            // Low-Tier Graveyard Generation
            case 0:
                NumberOfGuards = Random.Range(1, 4);
                MaxBushes = Random.Range(10, 16);
                MaxTrees = Random.Range(2, 4);
                SelectedMap = MapChoices[Random.Range(0, 5)];
                SelectedName = NameChoices[Random.Range(0, 5)];
                gameObject.GetComponent<Image>().sprite = SelectedName;
                break;
            // Medium-Tier Graveyard Generation
            case 1:
                NumberOfGuards = Random.Range(3, 6);
                MaxBushes = Random.Range(8, 16);
                MaxTrees = Random.Range(2, 6);
                SelectedMap = MapChoices[Random.Range(0, 5)];
                SelectedName = NameChoices[Random.Range(0, 5)];
                gameObject.GetComponent<Image>().sprite = SelectedName;
                break;
            // High-Tier Graveyard Generation
            case 2:
                NumberOfGuards = Random.Range(6, 11);
                MaxBushes = Random.Range(6, 15);
                MaxTrees = Random.Range(2, 8);
                SelectedMap = MapChoices[Random.Range(0, 5)];
                SelectedName = NameChoices[Random.Range(0, 5)];
                gameObject.GetComponent<Image>().sprite = SelectedName;
                break;
        }
    }
    // When the Graveyard is Selected it passes the information for the graveyard
    public void PassGraveyardInfo()
    {
        MapGeneration map = GameObject.FindGameObjectWithTag("Main Entrance").GetComponent<MapGeneration>();
        map.GuardNumber = NumberOfGuards;
        map.MaxBushes = MaxBushes;
        map.MaxTrees = MaxTrees;
        map.MapImage = SelectedMap;
    }
}

