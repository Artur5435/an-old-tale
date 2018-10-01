using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBuildGUIManager : MonoBehaviour
{
    private static NewBuildGUIManager instance;
    public static NewBuildGUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Button GrowButton;
    [SerializeField]
    private GameObject GrowGroup;
    [SerializeField]
    private Button PopulationButton;
    [SerializeField]
    private Button FoodButton;
    [SerializeField]
    private Button MinerButton;
    [SerializeField]
    private Button LumberButton;
    [SerializeField]
    private Button CraftersButton;
    [SerializeField]
    private Button WeaponsButton;

    [SerializeField]
    private Button DefendButton;
    [SerializeField]
    private GameObject DefendGroup;
    [SerializeField]
    private Button WallsButton;
    [SerializeField]
    private Button GateButton;
    [SerializeField]
    private Button TrapButton;
    [SerializeField]
    private Button SupportButton;
    [SerializeField]
    private Button TowersButton;

    [SerializeField]
    private Button ExpandButton;
    [SerializeField]
    private GameObject ExpandGroup;
    [SerializeField]
    private Button SoldiersButton;
    [SerializeField]
    private Button EducationButton;
    [SerializeField]
    private Button FaithButton;
    [SerializeField]
    private Button EntertainButton;
    [SerializeField]
    private Button MagicButton;
    [SerializeField]
    private Button CastleButton;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PopulationButton.onClick.AddListener(BuildPopulation);
        FoodButton.onClick.AddListener(BuildFood);
        MinerButton.onClick.AddListener(BuildMiner);
        LumberButton.onClick.AddListener(BuildLumber);
        CraftersButton.onClick.AddListener(BuildCrafters);
        WeaponsButton.onClick.AddListener(BuildWeapons);

        WallsButton.onClick.AddListener(BuildWalls);
        GateButton.onClick.AddListener(BuildGate);
        TrapButton.onClick.AddListener(BuildTrap);
        SupportButton.onClick.AddListener(BuildSupport);
        TowersButton.onClick.AddListener(BuildTower);

        SoldiersButton.onClick.AddListener(BuildSoldiers);
        EducationButton.onClick.AddListener(BuildEducation);
        FaithButton.onClick.AddListener(BuildFaith);
        EntertainButton.onClick.AddListener(BuildEntertain);
        MagicButton.onClick.AddListener(BuildMagic);
        CastleButton.onClick.AddListener(BuildCastle);

        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        GrowGroup.SetActive(false);
        DefendGroup.SetActive(false);
        ExpandGroup.SetActive(false);
        GrowButton.gameObject.SetActive(true);
        DefendButton.gameObject.SetActive(true);
        ExpandButton.gameObject.SetActive(true);
    }

    public void BuildPopulation()
    {
        DefaultActions();
        SmoothTerrainModify.Instance.Build(Enums.BuildingType.Hut);
    }

    public void BuildFood()
    {
        DefaultActions();
        SmoothTerrainModify.Instance.Build(Enums.BuildingType.Forager);
    }

    public void BuildMiner()
    {
        DefaultActions();
        SmoothTerrainModify.Instance.Build(Enums.BuildingType.Mine);
    }

    public void BuildLumber()
    {
        DefaultActions();
        SmoothTerrainModify.Instance.Build(Enums.BuildingType.LumberHut);
    }

    public void BuildCrafters()
    {
        DefaultActions();
        //ModifyTerrain.Instance.Build(Enums.BuildingType.);
    }

    public void BuildWeapons()
    {
        DefaultActions();
    }
    
    private void DefaultActions()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Player.Instance.building = true;
    }

    public void BuildWalls()
    {
        SmoothTerrainModify.Instance.BuildWall();
        Player.Instance.building = true;
    }
    public void BuildGate()
    {
        DefaultActions();
    }
    public void BuildTrap()
    {
        DefaultActions();
    }
    public void BuildSupport()
    {
        DefaultActions();
    }
    public void BuildTower()
    {
        DefaultActions();
    }

    public void BuildSoldiers()
    {
        DefaultActions();
    }
    public void BuildEducation()
    {
        DefaultActions();
    }
    public void BuildFaith()
    {
        DefaultActions();
    }
    public void BuildEntertain()
    {
        DefaultActions();
    }
    public void BuildMagic()
    {
        DefaultActions();
    }
    public void BuildCastle()
    {
        DefaultActions();
    }
}
