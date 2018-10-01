using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTerrainModify : MonoBehaviour
{

    private static SmoothTerrainModify instance;
    public static SmoothTerrainModify Instance
    {
        get
        {
            return instance;
        }
    }



    public bool locked = false;

    [SerializeField]
    private GameObject CityCentrePrefab;
    

    private bool chunkLoadedThisFrame;

    private void Awake()
    {
        instance = this;
    }

    public void LoadChunks(Vector3 actorPos, float loadDistance)
    {
        if (chunkLoadedThisFrame)
            return;
        for (int x = 0; x < SmoothTerrainGenerator.Instance.chunks.GetLength(0); x++)
        {
            for (int z = 0; z < SmoothTerrainGenerator.Instance.chunks.GetLength(1); z++)
            {
                float dist = Vector2.Distance(new Vector2(x * SmoothTerrainGenerator.Instance.chunkXZ, z * SmoothTerrainGenerator.Instance.chunkXZ), new Vector2(actorPos.x, actorPos.z));
                if (dist < loadDistance)
                {
                    if (SmoothTerrainGenerator.Instance.chunks[x, z] == null)
                    {
                        chunkLoadedThisFrame = true;
                        SmoothTerrainGenerator.Instance.GenChunk(x, z);
                    }
                    else if (SmoothTerrainGenerator.Instance.chunks[x, z].enabled == false)
                    {
                        chunkLoadedThisFrame = true;
                        SmoothTerrainGenerator.Instance.GenChunk(x, z);
                    }
                }
            }
        }
    }

    public void Build(Enums.BuildingType type)
    {
        StartCoroutine(Building(type));
    }

    public void BuildWall()
    {
        StartCoroutine(BuildingWall());
    }

    private IEnumerator Building(Enums.BuildingType type)
    {
        Zone newBuilding = Instantiate(PrefabsHolder.Instance.GetZonePrefab(type)).GetComponent<Zone>();
        newBuilding.BeforeBuildingActions(type);
        RaycastHit hit;
        while (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            newBuilding.transform.position = hit.point;

            if (newBuilding.IsBuildable() && hit.collider.gameObject.tag.Equals("Chunk"))
            {
                if (Input.GetKeyDown("r"))
                {
                    newBuilding.transform.Rotate(Vector3.up, 90);
                }

                if (Input.GetMouseButton(0))
                {
                    newBuilding.AfterBuildingActions();
                    Player.Instance.building = false;
                    yield break;
                }
                
            }
            
            if (Input.GetMouseButton(1))
            {
                Object.Destroy(newBuilding.gameObject);
                Player.Instance.building = false;
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator BuildingWall()
    {
        Vector3 start = Vector3.zero;
        RaycastHit hit;

        List<GameObject> Walls = new List<GameObject>();

        while (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                start = hit.point;
                start.y = SmoothTerrainGenerator.Instance.GetBlock(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                Walls.Add(Instantiate(PrefabsHolder.Instance.GetWall(), start, Quaternion.identity, City.Instance.gameObject.transform.parent));
                break;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Player.Instance.building = false;
                foreach (GameObject g in Walls)
                {
                    Object.Destroy(g);
                }
                yield break; 
            }
            yield return null;
        }
        int currentLength = 1;
        while (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            for (int i = 0; i < currentLength; i++)
            {
                if (i >= Walls.Count)
                {
                    Walls.Add(Instantiate(PrefabsHolder.Instance.GetWall(), start, Quaternion.identity, City.Instance.gameObject.transform.parent));
                    Walls[i].transform.rotation = Quaternion.Euler(0, -Vector3.SignedAngle(start - hit.point, Vector3.forward, Vector3.up), 0);
                    Vector3 newPos = start - Walls[i].transform.TransformDirection(Vector3.forward) * i;
                    newPos.y = SmoothTerrainGenerator.Instance.GetBlock(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z)) - 1;
                    Walls[i].transform.position = newPos;

                    Walls[i].GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    Walls[i].transform.rotation = Quaternion.Euler(0, -Vector3.SignedAngle(start - hit.point, Vector3.forward, Vector3.up), 0);
                    Vector3 newPos = start - Walls[i].transform.TransformDirection(Vector3.forward) * i;
                    newPos.y = SmoothTerrainGenerator.Instance.GetBlock(Mathf.RoundToInt(newPos.x), Mathf.RoundToInt(newPos.z)) - 1;
                    Walls[i].transform.position = newPos;

                    Walls[i].GetComponent<BoxCollider>().enabled = false;
                }
                if (currentLength < Walls.Count)
                {
                    for (int j = currentLength; j < Walls.Count; j++)
                    {
                        GameObject wall = Walls[j];
                        Walls.Remove(wall);
                        Destroy(wall);
                    }
                }
            }
            currentLength = Mathf.FloorToInt(Vector3.Distance(start, hit.point));

            if (Input.GetMouseButtonUp(0))
            {
                Player.Instance.building = false;
                foreach (GameObject g in Walls)
                {
                    g.GetComponent<BoxCollider>().enabled = true;
                }
                break;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Player.Instance.building = false;
                foreach (GameObject g in Walls)
                {
                    Object.Destroy(g);
                }
                break;
            }
            yield return null;
        }
        foreach(GameObject g in Walls)
        {
            g.transform.parent = City.Instance.gameObject.transform;
        }
    }

    public void spawnStartHouse(int x, int z)
    {
        int y = (int)Player.Instance.gameObject.transform.position.y;
        City.Instance.CityCenterGO = Instantiate(CityCentrePrefab, new Vector3((float)x - 1, (float)y, (float)z - 1), Quaternion.identity);
        for (int xm = x - 10; xm < x + 10; xm++)
        {
            for (int zm = z - 10; zm < z + 10; zm++)
            {
                SmoothTerrainGenerator.Instance.SetBlockType(xm, zm, Enums.BlockType.Sand);
                UpdateChunk(xm, zm, y);
                SmoothTerrainGenerator.Instance.SetLock(xm, zm, true);
            }
        }
    }

    public void LeftClickIzo()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.tag.Equals("Zone"))
            {
                hit.collider.gameObject.GetComponent<Zone>().Enter();
            }
            else if (hit.collider.gameObject.tag.Equals("Chunk"))
            {
               BuildGUIManager.Instance.UnBindZone();
            }
        }
    }

    public void RightClickIzo()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag.Equals("Chunk"))
            {
                BuildGUIManager.Instance.UnBindZone();
            }
        }
    }

    public void LeftClickFpp(float range)
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                Player.Instance.GetBall().SetActive(true);
                Player.Instance.GetBall().transform.position = hit.point + (Vector3.up / 2);
                Vector3 v = hit.collider.ClosestPoint(hit.point);
                UpdateChunk(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z), SmoothTerrainGenerator.Instance.GetBlock(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z)) - Time.deltaTime);
            }
        }
    }

    public void RightClickFpp(float range)
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < range)
            {
                Player.Instance.GetBall().SetActive(true);
                Player.Instance.GetBall().transform.position = hit.point + (Vector3.up / 2);
                Vector3 v = hit.collider.ClosestPoint(hit.point);
                UpdateChunk(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z), SmoothTerrainGenerator.Instance.GetBlock(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z)) + Time.deltaTime);
            }
        }
    }

    public void UpdateChunk(int x, int y, float newPos)
    {
        if (SmoothTerrainGenerator.Instance.GetStruct(x, y) != 0)
        {
            SmoothTerrainGenerator.Instance.SetStruct(x, y, 0);
        }
        if(SmoothTerrainGenerator.Instance.GetLock(x,y))
        {
            return;
        }
        SmoothTerrainGenerator.Instance.SetBlock(x, y, newPos);

        SmoothTerrainGenerator.Instance.chunks[x / SmoothTerrainGenerator.Instance.chunkXZ, y / SmoothTerrainGenerator.Instance.chunkXZ].upd = true;

        if (x % SmoothTerrainGenerator.Instance.chunkXZ == 0)
        {
            SmoothTerrainGenerator.Instance.chunks[x / SmoothTerrainGenerator.Instance.chunkXZ - 1, y / SmoothTerrainGenerator.Instance.chunkXZ].upd = true;
            SmoothTerrainGenerator.Instance.chunks[x / SmoothTerrainGenerator.Instance.chunkXZ + 1, y / SmoothTerrainGenerator.Instance.chunkXZ].upd = true;
        }
        if (y % SmoothTerrainGenerator.Instance.chunkXZ == 0)
        {
            SmoothTerrainGenerator.Instance.chunks[x / SmoothTerrainGenerator.Instance.chunkXZ, y / SmoothTerrainGenerator.Instance.chunkXZ - 1].upd = true;
            SmoothTerrainGenerator.Instance.chunks[x / SmoothTerrainGenerator.Instance.chunkXZ, y / SmoothTerrainGenerator.Instance.chunkXZ + 1].upd = true;
        }
    }
}
