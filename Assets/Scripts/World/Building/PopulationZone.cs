using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationZone : Zone
{
    public static List<PopulationZone> Houses = new List<PopulationZone>();

    public List<Mob> Inhabitants;
    public int MaxInhabitants;
    

    public static int GetMaximumPop()
    {
        int x = 0;
        foreach(PopulationZone house in Houses)
        {
            x += house.MaxInhabitants;
        }
        return x;
    }

    private void Awake()
    {
        AllZones.Add(this);
        Houses.Add(this);
        box = this.gameObject.GetComponent<BoxCollider>();
    }

    private void OnDestroy()
    {
        AllZones.Remove(this);
        Houses.Remove(this);
    }

    public override void BeforeBuildingActions(Enums.BuildingType newType)
    {
        type = newType;
        building = Instantiate(PrefabsHolder.Instance.GetBuilding(type), transform.position, Quaternion.identity);
        building.transform.parent = this.transform;

        SetBoundingBox();

        foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        box.enabled = true;
        gameObject.layer = 2;
    }
    

    public override void AfterBuildingActions()
    {
        foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }
        gameObject.layer = 10;

        building.GetComponent<BuildingSuicideScript>().OnBuildingFinished += OnPopulationBuildingReady;
        building.GetComponent<BuildingSuicideScript>().Run();

        base.ClearZone();
    }

    public void OnPopulationBuildingReady()
    {
        ManageInhabitantsMaxCount();
        UnSetInhabitants();
        GetInhibitants();
    }

    public override void Promote(Enums.BuildingType newType)
    {
        UnSetInhabitants();

        Destroy(building);

        type = newType;
        building = Instantiate(PrefabsHolder.Instance.GetBuilding(type), transform.position, Quaternion.identity);
        building.transform.parent = this.transform;
        SetBoundingBox();

        Enter();

        building.GetComponent<BuildingSuicideScript>().OnBuildingFinished += OnPopulationBuildingReady;
        building.GetComponent<BuildingSuicideScript>().Run();
    }

    public override void OtherRemovingActions()
    {
        foreach(Transform g in this.gameObject.GetComponentsInChildren<Transform>())
        {
            g.gameObject.AddComponent<Rigidbody>();
        }
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    private void ManageInhabitantsMaxCount()
    {
        switch (type)
        {
            case Enums.BuildingType.Hut:
                MaxInhabitants = 2;
                break;
            case Enums.BuildingType.BigHut:
                MaxInhabitants = 3;
                break;
            case Enums.BuildingType.WoodHouse:
                MaxInhabitants = 5;
                break;
            case Enums.BuildingType.StoneHouse:
                MaxInhabitants = 7;
                break;
        }
    }

    private void GetInhibitants()
    {
        for (int i = 0; i < MaxInhabitants || Mob.MobInstances.FindAll(x => x.GetSleepPlace() == null).Count == 0; i++)
        {
            Mob.MobInstances.Find(x => x.GetSleepPlace() == null)?.SetSleepPlace(this);
        }
        Inhabitants = Mob.MobInstances.FindAll(x => x.GetSleepPlace() == this);
    }

    private void UnSetInhabitants()
    {
        List<Mob> tempList = new List<Mob>(Inhabitants);
        tempList.ForEach(x => x.UnsetSleepPlace());
        tempList.Clear();
        Inhabitants.Clear();
    }
}
