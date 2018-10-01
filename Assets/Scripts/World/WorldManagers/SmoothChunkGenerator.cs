using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothChunkGenerator : MonoBehaviour
{
    public List<Vector3> newVert = new List<Vector3>();
    public List<int> newTri = new List<int>();
    public List<Vector2> newUV = new List<Vector2>();
    public List<Vector2> newUV_second = new List<Vector2>();

    public int chunkXZ = 16;
    public int chunkY = 128;
    public int wX;
    public int wY;
    public int wZ;

    public bool upd;

    private float tUnit = 1f;

    private Vector2 tGrassTop = new Vector2(0, 0);
    private float tRoadTop = (float)0.5f;
    private float tSandTop = (float)1.0f - (float)(1.0f / 256.0f);

    private Mesh mesh;
    private Collider col;

    public List<GameObject> Structures = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();
        genTerrain();
    }

    void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVert.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTri.ToArray();
        mesh.RecalculateNormals();
        col.GetComponent<MeshCollider>().sharedMesh = null;
        col.GetComponent<MeshCollider>().sharedMesh = mesh;

        newUV.Clear();
        newVert.Clear();
        newTri.Clear();
    }

    public void genTerrain()
    {
        float x;
        float z;

        foreach (GameObject g in Structures)
        {
            Object.Destroy(g);
        }

        Structures.Clear();

        for (int xl = 0; xl < chunkXZ; xl++)
        {
            for (int zl = 0; zl < chunkXZ; zl++)
            {
                if (SmoothTerrainGenerator.Instance.GetBlockType(xl + wX, zl + wZ) == Enums.BlockType.None)
                {
                    continue;
                }
                x = (float)xl;
                z = (float)zl;

                newTri.Add(newVert.Count + 1);
                newTri.Add(newVert.Count + 2);
                newTri.Add(newVert.Count + 3);
                newTri.Add(newVert.Count + 0);
                newTri.Add(newVert.Count + 1);
                newTri.Add(newVert.Count + 3);

                newVert.Add(new Vector3(x, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + 1 + wZ), z + 1));
                newVert.Add(new Vector3(x + 1, SmoothTerrainGenerator.Instance.GetBlock(xl + 1 + wX, zl + 1 + wZ), z + 1));
                newVert.Add(new Vector3(x + 1, SmoothTerrainGenerator.Instance.GetBlock(xl + 1 + wX, zl + wZ), z));
                newVert.Add(new Vector3(x, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ), z));

                //"+/- 0.02" to konieczna poprawka błędu obliczeniowego
                newUV.Add(new Vector2(tUnit + tGrassTop.x + tUnit - 0.02f, tUnit * tGrassTop.y + 0.02f));
                newUV.Add(new Vector2(tUnit + tGrassTop.x + tUnit - 0.02f, tUnit * tGrassTop.y + tUnit - 0.02f));
                newUV.Add(new Vector2(tUnit + tGrassTop.x + 0.02f, tUnit * tGrassTop.y + tUnit - 0.02f));
                newUV.Add(new Vector2(tUnit + tGrassTop.x + 0.02f, tUnit * tGrassTop.y + 0.02f));


                if (SmoothTerrainGenerator.Instance.GetStruct(xl + wX, zl + wZ) == 1)
                {
                    Structures.Add(Instantiate(PrefabsHolder.Instance.GetTree(), new Vector3(x + wX, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ) - 0.5f, z + wZ), Quaternion.identity, this.gameObject.transform));
                }
                else if (SmoothTerrainGenerator.Instance.GetStruct(xl + wX, zl + wZ) == 2)
                {
                    Structures.Add(Instantiate(PrefabsHolder.Instance.GetBush(), new Vector3(x + wX, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ) - 0.1f, z + wZ), PrefabsHolder.Instance.GetBush().transform.rotation, this.gameObject.transform));
                }
                if (SmoothTerrainGenerator.Instance.GetStruct(xl + wX, zl + wZ) == 3)
                {
                    Structures.Add(Instantiate(PrefabsHolder.Instance.GetShroom(0), new Vector3(x + wX, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ), z + wZ), Quaternion.identity, this.gameObject.transform));
                }
                else if (SmoothTerrainGenerator.Instance.GetStruct(xl + wX, zl + wZ) == 4)
                {
                    Structures.Add(Instantiate(PrefabsHolder.Instance.GetShroom(1), new Vector3(x + wX, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ), z + wZ), Quaternion.identity, this.gameObject.transform));
                }
                else if (SmoothTerrainGenerator.Instance.GetStruct(xl + wX, zl + wZ) == 5)
                {
                    Structures.Add(Instantiate(PrefabsHolder.Instance.GetShroom(2), new Vector3(x + wX, SmoothTerrainGenerator.Instance.GetBlock(xl + wX, zl + wZ), z + wZ), Quaternion.identity, this.gameObject.transform));
                }
            }

        }
        updateMesh();
    }
    

    private void LateUpdate()
    {
        if (upd)
        {
            genTerrain();
            upd = false;
        }
    }
}
