using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionZone : Zone
{
    public static List<ProductionZone> Industry = new List<ProductionZone>();

    public List<Mob> Workers = new List<Mob>();
    public int maxWorkers;

    public List<ProducedMaterial> Production = new List<ProducedMaterial>();



    private void Awake()
    {
        AllZones.Add(this);
        Industry.Add(this);
        box = this.gameObject.GetComponent<BoxCollider>();
    }

    private void OnDestroy()
    {
        AllZones.Remove(this);
        Industry.Remove(this);
    }


    public override void BeforeBuildingActions(Enums.BuildingType newType)
    {
        type = newType;
        building = Instantiate(PrefabsHolder.Instance.GetBuilding(type), transform.position, Quaternion.identity);
        building.transform.parent = this.transform;
        SetBoundingBox();

        //StartCoroutine(PreviewTerrainChanges());

        //turn Off colliders

        foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = false;
        }
        box.enabled = true;
        gameObject.layer = 2;
    }



    public override void AfterBuildingActions()
    {
        //StopCoroutine(PreviewTerrainChanges());
        //turn On colliders
        foreach (Collider c in this.gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = true;
        }

        gameObject.layer = 10;

        building.GetComponent<BuildingSuicideScript>().OnBuildingFinished += OnProductionBuildingReady;
        building.GetComponent<BuildingSuicideScript>().Run();
        base.ClearZone();
    }

    public override void Promote(Enums.BuildingType newType)
    {
        StopAllCoroutines();
        FireWorkers();
        Production.Clear();
        Destroy(building);

        type = newType;
        building = Instantiate(PrefabsHolder.Instance.GetBuilding(type), transform.position, Quaternion.identity);
        building.transform.parent = this.transform;
        SetBoundingBox();

        Enter();

        building.GetComponent<BuildingSuicideScript>().OnBuildingFinished += OnProductionBuildingReady;
        building.GetComponent<BuildingSuicideScript>().Run();
    }

    public void OnProductionBuildingReady()
    {
        ManageProduction();
        FireWorkers();
        GetWorkers();
        StartCoroutine(ProductionLoop());
    }

    private void ManageProduction()
    {
        switch (type)
        {
            case Enums.BuildingType.Forager:
                Production.Clear();
                Production.Add(new ProducedMaterial(Enums.Material.greenFood, 20));
                maxWorkers = 2;
                break;
            case Enums.BuildingType.Hunter:
                Production.Clear();
                Production.Add(new ProducedMaterial(Enums.Material.meat, 30));
                Production.Add(new ProducedMaterial(Enums.Material.skin, 5));
                maxWorkers = 2;
                break;
            case Enums.BuildingType.SmallFarm:
                Production.Clear();
                Production.Add(new ProducedMaterial(Enums.Material.greenFood, 40));
                maxWorkers = 3;
                break;
            case Enums.BuildingType.Mine:
                Production.Clear();
                Production.Add(new ProducedMaterial(Enums.Material.stone, 10));
                maxWorkers = 4;
                break;
            case Enums.BuildingType.LumberHut:
                Production.Clear();
                Production.Add(new ProducedMaterial(Enums.Material.wood, 10));
                maxWorkers = 2;
                break;
        }
    }

    

    public void GetWorkers()
    {
        for (int i = 0; i < maxWorkers || Mob.MobInstances.FindAll(x => x.GetSleepPlace() == null).Count == 0; i++)
        {
            Mob.MobInstances.Find(x => x.GetWorkPlace() == null).SetWorkPlace(this);
        }
        Workers = Mob.MobInstances.FindAll(x => x.GetWorkPlace() == this);
    }

    public void FireWorkers()
    {
        List<Mob> tempList = new List<Mob>(Workers);
        tempList.ForEach(x => x.UnsetWorkPlace());
        tempList.Clear();
        Workers.Clear();
    }

    private IEnumerator ProductionLoop()
    {
        while (true)
        {
            foreach (ProducedMaterial m in Production)
            {
                City.Instance.MaterialList.Find(x => x.GetMaterialType() == m.type).DebugSetResources(m.GetOneIteration(Workers.Count / maxWorkers));
            }
            yield return new WaitForSeconds(1);
        }
    }
}

public class ProducedMaterial
{
    private int Count;
    public Enums.Material type;
    public int BaseProduction;

    public ProducedMaterial(Enums.Material type, int BaseProduction)
    {
        this.type = type;
        Count = 0;
        this.BaseProduction = BaseProduction;
    }

    public void ProduceMaterial(float BaseMultiplier)
    {
        Count += Mathf.RoundToInt(BaseProduction * BaseMultiplier);
    }

    public int GetOneIteration(float baseMultilier)
    {
        return Mathf.RoundToInt(BaseProduction * baseMultilier);
    }

    public int GetMaterial(int MaterialToGet)
    {
        Count -= MaterialToGet;
        return MaterialToGet;
    }

    public int GetAllMaterial()
    {
        return Count;
    }

    public int GetCount()
    {
        return Count;
    }
}
