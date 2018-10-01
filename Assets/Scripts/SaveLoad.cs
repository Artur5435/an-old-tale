using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour
{
    public bool loadable = false;
    public bool toload = false;
    public Game.city City;
    public byte[,,] blocks;
    public bool started;
    Game loaded;
    public Vector3 playerPos;

    public uint wood;
    public uint stone;
    public uint skin;
    public uint greenfood;
    public uint meat;
    public uint popcount;
    public List<Game.zon> zones;
    public byte[,] structures;

    public bool[] enable;
    public byte[] priority;

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/saves/game.s"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves/game.s", FileMode.Open);
            loaded = (Game)bf.Deserialize(file);
            file.Close();
            blocks = loaded.blocks;
            playerPos = new Vector3(loaded.playerPos.x, loaded.playerPos.y,loaded.playerPos.z);
            started = loaded.started;
            City = loaded.CITY;
            wood = loaded.CITY.wood;
            stone = loaded.CITY.stone;
            skin = loaded.CITY.skin;
            greenfood = loaded.CITY.greenfood;
            meat = loaded.CITY.meat;
            popcount = loaded.CITY.popcount;
            structures = loaded.structures;
            foreach(Game.zon z in loaded.CITY.zones)
            {
                zones.Add(z);
            }
            loadable = true;
            enable = loaded.CITY.enabled;
            priority = loaded.CITY.priority;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class Game
{
    [System.Serializable]
    public struct zon
    {
        public pos3 location;
        public pos3 size;
        public Enums.BuildingType type;
        public byte lvl;
        public byte tier;
        public bool builded;
        public byte floor;
        public zon(pos3 location, pos3 size, Enums.BuildingType type, byte lvl, byte tier, bool builded, byte floor)
        {
            this.location = location;
            this.size = size;
            this.type = type;
            this.lvl = lvl;
            this.tier = tier;
            this.builded = builded;
            this.floor = floor;
        }
    }
    [System.Serializable]
    public struct city
    {
        public pos3 cityCenLoc;
        public uint wood;
        public uint stone;
        public uint meat;
        public uint greenfood;
        public uint skin;
        public uint popcount;
        public List<zon> zones;
        public bool[] enabled;
        public byte[] priority;
    }
    [System.Serializable]
    public struct pos3
    {
        public float x;
        public float y;
        public float z;

        public pos3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public Game current;
    public city CITY;
    public bool started;
    public byte[,,] blocks;
    public byte[,] structures;
    public pos3 playerPos;

    public Game()
    {
        current = this;
        blocks = null;
        playerPos = new pos3(0,0,0);
        CITY = new city();
        started = false;
    }

    public Game(byte[,,] bloks,Vector3 playerp, bool star)
    {
        started = star;
        playerPos = new pos3(playerp.x, playerp.y,playerp.z);
        blocks = bloks;
        current = this;
        CITY = new city();
    }
}