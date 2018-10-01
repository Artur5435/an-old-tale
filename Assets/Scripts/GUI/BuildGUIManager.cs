using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Resources;

public class BuildGUIManager : MonoBehaviour
{
    private static BuildGUIManager instance;
    public static BuildGUIManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Canvas thisCanvas;

    [SerializeField]
    private Text BuildingFirstPromotionText;
    [SerializeField]
    private Text BuildingSecondPromotionText;
    [SerializeField]
    private Button BuildingFirstPromotionButton;
    [SerializeField]
    private Button BuildingSecondPromotionButton;

    [SerializeField]
    private Button LevelUPButton;

    [SerializeField]
    private Button RemoveBuildingButton;

    [SerializeField]
    private Text LevelInfoText;
    [SerializeField]
    private Text DescriptionText;

    [SerializeField]
    private GameObject BuildOptionsMenu;
    

    public void BindZone(Zone z)
    {
        BuildOptionsMenu.SetActive(true);
        if (z.GetBuildingType() == Enums.BuildingType.None)
        {
            BuildingFirstPromotionButton.gameObject.SetActive(false);
            BuildingSecondPromotionButton.gameObject.SetActive(false);
            LevelUPButton.gameObject.SetActive(false);
            RemoveBuildingButton.gameObject.SetActive(true);
            RemoveBuildingButton.onClick.AddListener(() => z.RemoveZone());
            DescriptionText.enabled = false;
            LevelInfoText.enabled = false;
        }
        else
        {
            RemoveBuildingButton.gameObject.SetActive(true);
            RemoveBuildingButton.onClick.AddListener(() => z.RemoveZone());
            UpdateBuildGui(z);
        }
    }

    public void UnBindZone()
    {
        BuildingFirstPromotionButton.onClick.RemoveAllListeners();
        BuildingSecondPromotionButton.onClick.RemoveAllListeners();
        RemoveBuildingButton.onClick.RemoveAllListeners();
        LevelUPButton.onClick.RemoveAllListeners();
        BuildOptionsMenu.SetActive(false);
        thisCanvas.enabled = false;
    }

    public void ResetGUI(Zone z)
    {
        UnBindZone();
        BindZone(z);

    }

    public void UpdateBuildGui(Zone z)
    {
        ManageBranches(z);
        ManageTexts(z);
    }

    private void ManageTexts(Zone z)
    {
    }

    public void SetButtons(string FirstBranchName, string SecondBranchName, Enums.BuildingType firstBranch, Enums.BuildingType secondBranch, Zone z)
    {
        if (firstBranch != Enums.BuildingType.None)
        {
            BuildingFirstPromotionButton.gameObject.SetActive(true);
            BuildingFirstPromotionText.text = FirstBranchName;
            BuildingFirstPromotionButton.onClick.AddListener(() => z.Promote(firstBranch));
        }
        else
        {
            BuildingFirstPromotionButton.gameObject.SetActive(false);
        }
        if (secondBranch != Enums.BuildingType.None)
        {
            BuildingSecondPromotionButton.gameObject.SetActive(true);
            BuildingSecondPromotionText.text = SecondBranchName;
            BuildingSecondPromotionButton.onClick.AddListener(() => z.Promote(secondBranch));
        }
        else
        {
            BuildingSecondPromotionButton.gameObject.SetActive(false);
        }
    }

    private void ManageBranches(Zone z)
    {
        BuildingFirstPromotionButton.onClick.RemoveAllListeners();
        BuildingSecondPromotionButton.onClick.RemoveAllListeners();
        switch (z.GetBuildingType())
        {
            case Enums.BuildingType.Hut:
                SetButtons( "Duża chatka", "", Enums.BuildingType.BigHut, Enums.BuildingType.None, z);
                break;
            case Enums.BuildingType.BigHut:
                SetButtons( "Drewniany Dom", "", Enums.BuildingType.WoodHouse, Enums.BuildingType.None, z);
                break;
            case Enums.BuildingType.LumberHut:
                SetButtons("Dom Drwala", "",Enums.BuildingType.LumberHouse, Enums.BuildingType.None, z);
                break;
            case Enums.BuildingType.Mine:
                SetButtons("Głęboka Kopalnia", "Kamieniołom", Enums.BuildingType.DeepMine, Enums.BuildingType.Quarry, z);
                break;
            case Enums.BuildingType.Forager:
                SetButtons( "Chata Myśliwego", "Dom Rolnika", Enums.BuildingType.Hunter, Enums.BuildingType.SmallFarm, z);
                break;
            case Enums.BuildingType.Hunter:
                SetButtons("Ranczo", "", Enums.BuildingType.Ranch, Enums.BuildingType.None, z);
                break;
            case Enums.BuildingType.SmallFarm:
                SetButtons( "Farma", "", Enums.BuildingType.Farm, Enums.BuildingType.None, z);
                break;
        }

    }

    private void Awake()
    {
        instance = this;
        thisCanvas = this.gameObject.GetComponentInParent<Canvas>();
    }
}
