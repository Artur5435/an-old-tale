using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICity
{
    public static int BaseGreenFoodPrice = 10;
    public static int BaseMeatPrice = 10;
    public static int BaseWoodPrice = 5;
    public static int BaseStonePrice = 15;
    public static int BaseSkinPrice = 25;

    public int population;
    public int freePopulation;

    public int GreenFoodDemands;
    public int GreenFoodCount;
    public int GreenFoodProduction;
    public int GreenFoodDeficit;

    public int MeatDemands;
    public int MeatCount;
    public int MeatProduction;
    public int MeatDeficit;

    public int StoneDemands;
    public int StoneCount;
    public int StoneProduction;
    public int StoneDeficit;

    public int WoodDemands;
    public int WoodCount;
    public int WoodProduction;
    public int WoodDeficit;

    public int SkinDemands;
    public int SkinCount;
    public int SkinProduction;
    public int SkinDeficit;

    public AICity()
    {
        population = 10;
        MeatCount = 5000;
        StoneCount = 5000;
        WoodCount = 5000;
        SkinCount = 5000;
        GreenFoodCount = 5000;

        GreenFoodDemands = 50;
        MeatDemands = 50;
        WoodDemands = 10;
        StoneDemands = 10;
        SkinDemands = 10;

        freePopulation = 10;
    }

    public void CalculateResources()
    {
        int Sum = GreenFoodProduction - GreenFoodDemands;
        GreenFoodCount += Sum;
        GreenFoodDeficit = GreenFoodCount < 0 ? -GreenFoodCount : 0;

        Sum = MeatProduction - MeatDemands;
        MeatCount += Sum;
        MeatDeficit = MeatCount < 0 ? -MeatCount : 0;

        Sum = StoneProduction - StoneDemands;
        StoneCount += Sum;
        StoneDeficit = StoneCount < 0 ? -StoneCount : 0;

        Sum = WoodProduction - WoodDemands;
        WoodCount += Sum;
        WoodDeficit = WoodCount < 0 ? -WoodCount : 0;

        Sum = SkinProduction - SkinDemands;
        SkinCount += Sum;
        SkinDeficit = SkinCount < 0 ? -SkinCount : 0;
    }

    public void Iterate()
    {
        CalculateResources();

        if (GreenFoodCount > 0 && MeatCount > 0)
        {
            population++;
            freePopulation++;
            GreenFoodDemands += 5;
            MeatDemands += 5;
            WoodDemands += Random.Range(1, 10);
            SkinDemands += Random.Range(1, 10);
            StoneDemands += Random.Range(1, 10);
        }

        bool breakNow = true; 
        while (freePopulation > 0 && !breakNow)
        {
            breakNow = true;
            if (GreenFoodProduction + MeatProduction < GreenFoodDemands + MeatDemands)
            {
                breakNow = false;
                GreenFoodProduction += 10;
                MeatProduction += 10;
                freePopulation--;
                if(freePopulation == 0)
                {
                    break;
                }
            }
            if (StoneProduction < StoneDemands)
            {
                breakNow = false;
                StoneProduction += 20;
                freePopulation--;
                if (freePopulation == 0)
                {
                    break;
                }
            }
            if (WoodProduction < WoodDemands)
            {
                breakNow = false;
                WoodProduction += 20;
                freePopulation--;
                if (freePopulation == 0)
                {
                    break;
                }
            }
            if (SkinProduction < SkinDemands)
            {
                breakNow = false;
                SkinProduction += 10;
                freePopulation--;
                if (freePopulation == 0)
                {
                    break;
                }
            }
        }

    }

    public int GetGreenFoodPrice()
    {
        return BaseGreenFoodPrice * (GreenFoodDemands - GreenFoodProduction / 1000 + 1);
    }

    public int GetMeatPrice()
    {
        return BaseMeatPrice * (MeatDemands - MeatProduction / 1000 + 1);
    }

    public int GetWoodPrice()
    {
        return BaseWoodPrice * (WoodDemands - WoodProduction / 1000 + 1);
    }

    public int GetStonePrice()
    {
        return BaseStonePrice * (StoneDemands - StoneProduction / 1000 + 1);
    }

    public int GetSkinPrice()
    {
        return BaseSkinPrice * (SkinDemands - SkinProduction / 1000 + 1);
    }
}
