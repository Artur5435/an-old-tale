using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsHolder : MonoBehaviour
{
    private static PrefabsHolder instance;
    public static PrefabsHolder Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject Hut;
    [SerializeField]
    private GameObject BigHut;
    [SerializeField]
    private GameObject WoodHouse;
    [SerializeField]
    private GameObject Mine;
    [SerializeField]
    private GameObject LumberHut;
    [SerializeField]
    private GameObject Forager;
    [SerializeField]
    private GameObject Hunter;
    [SerializeField]
    private GameObject SmallFarm;

    [SerializeField]
    private GameObject Tree;

    [SerializeField]
    private GameObject bush;

    [SerializeField]
    private List<GameObject> Shrooms ;

    [SerializeField]
    private GameObject WoodPalisade;

    [SerializeField]
    private GameObject GrassPrefab;

    [SerializeField]
    private GameObject PopulationZone;
    [SerializeField]
    private GameObject ProductionZone;
    [SerializeField]
    private GameObject CulturalZone;

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetBuilding(Enums.BuildingType type)
    {
        switch(type)
        {
            case Enums.BuildingType.Hut:
                return Hut;
            case Enums.BuildingType.BigHut:
                return BigHut;
            case Enums.BuildingType.WoodHouse:
                return WoodHouse;
            case Enums.BuildingType.Mine:
                return Mine;
            case Enums.BuildingType.Hunter:
                return Hunter;
            case Enums.BuildingType.LumberHut:
                return LumberHut;
            case Enums.BuildingType.Forager:
                return Forager;
            case Enums.BuildingType.SmallFarm:
                return SmallFarm;
        }
        return null;
    }

    public GameObject GetTree()
    {
        return Tree;
    }

    public GameObject GetBush()
    {
        return bush;
    }

    public GameObject GetShroom(int index)
    {
        return Shrooms[index];
    }

    public GameObject GetWall()
    {
        return WoodPalisade;
    }
    public GameObject GetGrass()
    {
        return GrassPrefab;
    }

    public GameObject GetZonePrefab(Enums.BuildingType type)
    {
        switch(type)
        {
            case Enums.BuildingType.Hut:
            case Enums.BuildingType.BigHut:
            case Enums.BuildingType.WoodHouse:
                return PopulationZone;
            case Enums.BuildingType.Mine:
            case Enums.BuildingType.Forager:
            case Enums.BuildingType.Hunter:
            case Enums.BuildingType.LumberHut:
            case Enums.BuildingType.LumberHouse:
                return ProductionZone;
        }
        return null;
    }
}
