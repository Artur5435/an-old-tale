using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trade : MonoBehaviour
{
    private static Trade instance;
    public static Trade Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Text WoodPrice;
    [SerializeField]
    private Text StonePrice;
    [SerializeField]
    private Text GreenFoodPrice;
    [SerializeField]
    private Text MeatPrice;
    [SerializeField]
    private Text SkinPrice;


    public List<AICity> Cities = new List<AICity>();

    public AICity currentCity;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cities.Add(new AICity());
        Cities.Add(new AICity());
        Cities.Add(new AICity());
        Cities.Add(new AICity());
        Cities.Add(new AICity());

        currentCity = Cities[0];
        UpdatePrices();

        StartCoroutine(CitiesLoop());
    }
    

    private IEnumerator CitiesLoop()
    {
        while(true)
        {
            foreach(AICity c in Cities)
            {
                c.Iterate();
            }
            yield return new WaitForSeconds(30);
        }
    }

    private void UpdatePrices()
    {
        WoodPrice.text = currentCity.GetWoodPrice().ToString();
        StonePrice.text = currentCity.GetStonePrice().ToString();
        GreenFoodPrice.text = currentCity.GetGreenFoodPrice().ToString();
        MeatPrice.text = currentCity.GetMeatPrice().ToString();
        SkinPrice.text = currentCity.GetSkinPrice().ToString();
    }

    public void Sell(int type)
    {
        switch (type)
        {
            case 0:
                if (City.Instance.TryToGetMaterial(Enums.Material.wood, 100))
                {
                    currentCity.WoodCount += 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.money, currentCity.GetWoodPrice() * 100);
                }
                break;
            case 1:
                if (City.Instance.TryToGetMaterial(Enums.Material.stone, 100))
                {
                    currentCity.StoneCount += 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.money, currentCity.GetStonePrice() * 100);
                }
                break;
            case 2:
                if (City.Instance.TryToGetMaterial(Enums.Material.greenFood, 100))
                {
                    currentCity.GreenFoodCount += 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.money, currentCity.GetGreenFoodPrice() * 100);
                }
                break;
            case 3:
                if (City.Instance.TryToGetMaterial(Enums.Material.meat, 100))
                {
                    currentCity.MeatCount += 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.money, currentCity.GetMeatPrice() * 100);
                }
                break;
            case 4:
                if (City.Instance.TryToGetMaterial(Enums.Material.skin, 100))
                {
                    currentCity.SkinCount += 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.money, currentCity.GetSkinPrice() * 100);
                }
                break;
        }
        currentCity.CalculateResources();
        UpdatePrices();
    }

    public void Buy(int type)
    {
        switch (type)
        {
            case 0:
                if (City.Instance.TryToGetMaterial(Enums.Material.money, currentCity.GetWoodPrice() * 100))
                {
                    currentCity.WoodCount -= 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.wood, 100);
                }
                break;
            case 1:
                if (City.Instance.TryToGetMaterial(Enums.Material.money, currentCity.GetStonePrice() * 100))
                {
                    currentCity.StoneCount -= 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.stone, 100);
                }
                break;
            case 2:
                if (City.Instance.TryToGetMaterial(Enums.Material.money, currentCity.GetGreenFoodPrice() * 100))
                {
                    currentCity.GreenFoodCount -= 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.greenFood, 100);
                }
                break;
            case 3:
                if (City.Instance.TryToGetMaterial(Enums.Material.money, currentCity.GetMeatPrice() * 100))
                {
                    currentCity.MeatCount -= 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.meat, 100);
                }
                break;
            case 4:
                if (City.Instance.TryToGetMaterial(Enums.Material.money, currentCity.GetSkinPrice() * 100))
                {
                    currentCity.SkinCount -= 100;
                    City.Instance.OtherSourcesIncome(Enums.Material.skin, 100);
                }
                break;
        }
        currentCity.CalculateResources();
        UpdatePrices();
    }

    public void changeCity(int index)
    {
        currentCity = Cities[index];
        UpdatePrices();
    }
}
