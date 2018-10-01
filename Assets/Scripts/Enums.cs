using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Enums
{
    public enum Direction : byte
    {
        BottomNorthWest,
        BottomNorth,
        BottomNorthEast,
        BottomMiddleWest,
        BottomMiddle,
        BottomMiddleEast,
        BottomSouthWest,
        BottomSouth,
        BottomSouthEast,

        MiddleNorthWest,
        MiddleNorth,
        MiddleNorthEast,
        MiddleMiddleWest,
        MiddleMiddle,
        MiddleMiddleEast,
        MiddleSouthWest,
        MiddleSouth,
        MiddleSouthEast,

        UpNorthWest,
        UpNorth,
        UpNorthEast,
        UpMiddleWest,
        UpMiddle,
        UpMiddleEast,
        UpSouthWest,
        UpSouth,
        UpSouthEast,
    }

    public enum BuildingType : byte
    {
        None,

        Hut,
        BigHut,
        WoodHouse,
        StoneHouse,
        BigStoneHouse,  StoneDormitory,
        BrickHouse,     BrickDormitory,
        TenementHouse,  SoldiersQuaters,
        BigTenmentHouse,


        Forager,
        Hunter,     SmallFarm,
        Ranch,      Farm,
        BigRanch,   BigFarm,
        Agroculture,VegetableFarm,  WheatFarm,

        Mine,
        DeepMine,   Quarry,
        IronMine,   CoalMine,   StoneMine,

        LumberHut,
        LumberHouse,
        Sawmill,    LumberComplex,
                    CharcoalForge,  LumberManufactory,

        
    }

    public enum CameraMode
    {
        Izometric,
        FPP,
        Strategy,
        TPP,
    }

    public enum Material : byte
    {
        wood,
        greenFood,
        meat,
        stone,
        skin,
        iron,
        coal,
        glass,
        sand,
        dirt,
        plank,
        brick,
        money,
    }

    public enum BlockType : byte
    {
        Dirt,
        Stone,
        Grass,
        Sand,
        Road,
        Zone,
        None,
    }

    public enum ModuleDamageType : byte
    {
        None,
        Bow,
        Crossbow,
        Magic,
        CatapultMissle,
        BallistaMissle,
        TrebuchetMissle,
    }
}
