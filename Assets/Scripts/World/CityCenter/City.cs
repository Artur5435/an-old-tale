using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class City : MonoBehaviour
{
    private static City instance;
    public static City Instance
    {
        get
        {
            return instance;
        }
    }

    public bool changedSth = true;
    public Canvas GameOver;
    public GameObject MobPrefab;
    public GameObject CityCenterGO = null;
    public List<CityMaterial> MaterialList;

    private byte hallLvl = 1;
    public int foodRations = 1;
    

    //Flags
    public bool building = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitMaterials();
        foodRations = Mathf.RoundToInt(RationsDisplay.Instance.slider.value);

        foreach (CityMaterial m in MaterialList)
        {
            m.DebugSetResources(10000);
        }
        StartCoroutine(PopulationIncome());
        ResourceManager.ResourcesList.ForEach(x => x.InitMaterialGUI());
        for (int i = 0; i < 10; i++)
        {
            Instantiate(MobPrefab, CityCenter.Instance.gameObject.transform.position + (5 * Vector3.up) + ( (Vector3.left * 1.2f)), Quaternion.identity);
        }
        PopTextManager.Instance.Run();
    }

    private void InitMaterials()
    {
        MaterialList.Add(new CityMaterial(Enums.Material.greenFood));
        MaterialList.Add(new CityMaterial(Enums.Material.wood));
        MaterialList.Add(new CityMaterial(Enums.Material.meat));
        MaterialList.Add(new CityMaterial(Enums.Material.stone));
        MaterialList.Add(new CityMaterial(Enums.Material.money));
        MaterialList.Add(new CityMaterial(Enums.Material.skin));
        MaterialList.Add(new CityMaterial(Enums.Material.iron));
        MaterialList.Add(new CityMaterial(Enums.Material.coal));
        MaterialList.Add(new CityMaterial(Enums.Material.plank));
    }

    public string GetPopString()
    {
        return (Mob.MobInstances.FindAll(x => x.GetWorkPlace() == null).Count + "/" + Mob.MobInstances.Count + "/" + CalculateMaxPop());
    }

    public int CalculateMaxPop()
    {
        int maxPop = 0;
        PopulationZone.Houses.ForEach(x => maxPop += x.MaxInhabitants);
        return maxPop;
    }

    public void SetFoodRations(float newFoodRations)
    {
        foodRations = Mathf.RoundToInt(RationsDisplay.Instance.slider.value);
    }

    private IEnumerator PopulationIncome()
    {
        while (true)
        {
            if (Mob.MobInstances.Count == CalculateMaxPop())
            {

            }
            if (Mob.MobInstances.Count < CalculateMaxPop())
            {
                Instantiate(MobPrefab, CityCenter.Instance.gameObject.transform.position + (5 * Vector3.up), Quaternion.identity);
            }
            yield return new WaitForSeconds(30);
        }
    }

    public bool TryToGetMaterial(Enums.Material type, int count)
    {
        CityMaterial m = MaterialList.Find(x => x.GetMaterialType() == type);

        if (m.GetCount() > count)
        {
            m.UseMaterial(count);
            return true;
        }
        return false;
    }

    public void OtherSourcesIncome(Enums.Material type, int count)
    {
        CityMaterial m = MaterialList.Find(x => x.GetMaterialType() == type);
        if (m == null)
        {
            MaterialList.Add(m = new CityMaterial(type));
        }
        m.UseMaterial(-count);
    }

    
    public void OtherSourcesOutcome(Enums.Material type, int count)
    {
        CityMaterial m = MaterialList.Find(x => x.GetMaterialType() == type);
        if (m == null)
            return;
        if (m.GetCount() < count)
            return;
        m.UseMaterial(count);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mob.MobInstances.Count == 0)
        {
            GameOver.enabled = true;
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}

[System.Serializable]
public class CityMaterial
{
    private Enums.Material type;
    private int count;

    public int GetCount()
    {
        return count;
    }

    public CityMaterial(Enums.Material type)
    {
        this.type = type;
        count = 0;
    }
    

    public Enums.Material GetMaterialType()
    {
        return type;
    }

    public void UseMaterial(int usedCount)
    {
        count -= usedCount;
    }
    
    public void DebugSetResources(int count)
    {
        this.count += count;
    }
}
