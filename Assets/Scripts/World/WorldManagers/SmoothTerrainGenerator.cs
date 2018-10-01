using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTerrainGenerator : MonoBehaviour
{
    private static SmoothTerrainGenerator instance;
    public static SmoothTerrainGenerator Instance
    {
        get
        {
            return instance;
        }
    }

    private float[,] blocksHeights;
    private Enums.BlockType[,] blockTypes;
    private bool[,] locked;
    private byte[,] structures;

    public int farx = 2048;
    public int farz = 2048;
    public int fary = 128;
    public GameObject chunk;
    public SmoothChunkGenerator[,] chunks;

    public int chunkXZ = 32;
    public int chunkY = 128;
    public GameObject Player;

    private void Awake()
    {
        instance = this;

        blocksHeights = new float[farx, farz];
        blockTypes = new Enums.BlockType[farx, farz];
        locked = new bool[farx, farz];

        structures = new byte[farx, farz];

        Vector2Int offsetOfNoise = new Vector2Int(Random.Range(0, 1000), Random.Range(0, 1000));

        for (int x = 0; x < farx; x++)
        {
            for (int z = 0; z < farz; z++)
            {
                locked[x, z] = false;
                blocksHeights[x, z] = (float)(PerlinNoise(x + offsetOfNoise.x, z + offsetOfNoise.y, 100, 50, 1) + 128) / 4;
                int tree = PerlinNoise(x, z, Random.Range(25, 75), 20, 1);
                structures[x, z] = 0;
                if (tree > 14)
                {
                    if (tree < 16)
                    {
                        structures[x, z] = 1;
                    }
                    else if (tree < 17)
                    {
                        structures[x, z] = 2;
                    }
                    else
                    {
                        int b = Random.Range(0, 3);
                        structures[x, z] = (byte)(3 + b);
                    }
                }
            }
        }
        chunks = new SmoothChunkGenerator[Mathf.FloorToInt(farx / chunkXZ), Mathf.FloorToInt(farz / chunkXZ)];
    }

    private void Start()
    {
        Player = Instantiate(Player, new Vector3(farx / 2, GetBlock(farx / 2, farz / 2) + 2, farz / 2), new Quaternion(0, 0, 0, 0));
    }

    public void GenChunk(int x, int z)
    {
        for (int y = 0; y < chunks.GetLength(1); y++)
        {
            if (chunks[x, z] == null)
            {
                GameObject newchunk = Instantiate(chunk, new Vector3(x * chunkXZ, y * chunkY, z * chunkXZ), new Quaternion(0, 0, 0, 0), this.gameObject.transform) as GameObject;
                chunks[x, z] = newchunk.GetComponent<SmoothChunkGenerator>();
                chunks[x, z].chunkXZ = chunkXZ;
                chunks[x, z].chunkY = chunkY;
                chunks[x, z].wX = x * chunkXZ;
                chunks[x, z].wY = y * chunkY;
                chunks[x, z].wZ = z * chunkXZ;
            }
            else
            {
                chunks[x, z].gameObject.SetActive(true);
            }
        }
    }

    public float GetBlock(int x, int z)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return (byte)1;
        }
        return blocksHeights[x, z];
    }

    public void SetBlock(int x, int z, float newY)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return;
        }
        blocksHeights[x, z] = newY;
    }

    public Enums.BlockType GetBlockType(int x, int z)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return Enums.BlockType.None;
        }
        return blockTypes[x, z];
    }

    public void SetBlockType(int x, int z, Enums.BlockType newType)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return;
        }
        blockTypes[x, z] = newType;
    }

    public bool GetLock(int x, int z)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return true;
        }
        return locked[x, z];
    }

    public void SetLock(int x, int z, bool newLock)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return;
        }
        locked[x, z] = newLock;
    }

    public byte GetStruct(int x, int z)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return (byte)255;
        }
        return structures[x, z];
    }

    public void SetStruct(int x, int z, byte type)
    {
        if (x < 0 || x >= farx || z < 0 || z >= farz)
        {
            return;
        }
        structures[x, z] = type;
    }

    private int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }

    private int PerlinNoise(float x, float y, float scale, float mag, float exp)
    {
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * mag), (exp)));
    }
}
