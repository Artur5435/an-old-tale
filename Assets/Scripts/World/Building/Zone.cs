using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public static List<Zone> AllZones = new List<Zone>();

    protected Enums.BuildingType type;
    protected GameObject building;
    protected Vector3 size;
    protected BoxCollider box;

    private void Awake()
    {

    }

    #region VIRTUALS

    public virtual void BeforeBuildingActions(Enums.BuildingType newType) { }

    public virtual void AfterBuildingActions() { }

    public virtual void Promote(Enums.BuildingType newType) { }

    public virtual void OtherRemovingActions() { }

    #endregion

    public Enums.BuildingType GetBuildingType()
    {
        return type;
    }

    public BoxCollider GetBox()
    {
        return box;
    }

    public bool IsBuildable()
    {
        if (this.GetBox().bounds.Intersects(CityCenter.Instance.box.bounds))
        {
            return false;
        }
        if (AllZones.Find(x => x.GetBox().bounds.Intersects(this.GetBox().bounds) && x != this) != null)
        {
            return false;
        }
        return true;
    }

    protected IEnumerator PreviewTerrainChanges()
    {
        List<Vector3> Old = new List<Vector3>();
        List<Vector3> czysciec = new List<Vector3>();
        while (true)
        {
            for (int x = Mathf.FloorToInt(transform.position.x - box.size.x / 2) + 1; x <= transform.position.x + box.size.x / 2; x++)
            {
                for (int z = Mathf.RoundToInt(transform.position.z - box.size.z / 2) + 1; z <= transform.position.z + box.size.z / 2; z++)
                {
                    if (Old.Find(v => Mathf.RoundToInt(v.x) == x && Mathf.RoundToInt(v.z) == z) == Vector3.zero)
                    {
                        Old.Add(new Vector3(x, SmoothTerrainGenerator.Instance.GetBlock(x, z), z));
                    }
                    SmoothTerrainModify.Instance.UpdateChunk(x, z, this.transform.position.y);
                    SmoothTerrainGenerator.Instance.SetLock(x, z, true);
                }
            }
            foreach (Vector3 v in Old.FindAll(x => !box.bounds.Contains(x)))
            {
                Debug.Log(v);
                SmoothTerrainModify.Instance.UpdateChunk(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.z), v.y);
                czysciec.Add(v);
            }
            czysciec.ForEach(x => Old.Remove(x));
            czysciec.Clear();
            yield return null;
        }
    }

    public void ClearZone()
    {
        for (int x = Mathf.FloorToInt(transform.position.x - box.size.x / 2) + 1; x <= transform.position.x + box.size.x / 2; x++)
        {
            for (int z = Mathf.RoundToInt(transform.position.z - box.size.z / 2) + 1; z <= transform.position.z + box.size.z / 2; z++)
            {
                SmoothTerrainGenerator.Instance.SetBlockType(x, z, Enums.BlockType.Sand);
                SmoothTerrainModify.Instance.UpdateChunk(x, z, this.transform.position.y);
                SmoothTerrainGenerator.Instance.SetLock(x, z, true);
            }
        }
    }

    public void RemoveZone()
    {
        for (int x = Mathf.FloorToInt(transform.position.x - box.size.x / 2) + 1; x <= transform.position.x + box.size.x / 2; x++)
        {
            for (int z = Mathf.RoundToInt(transform.position.z - box.size.z / 2) + 1; z <= transform.position.z + box.size.z / 2; z++)
            {
                SmoothTerrainGenerator.Instance.SetLock(x, z, false);
            }
        }
        OtherRemovingActions();
    }

    protected void SetBoundingBox()
    {
        switch (type)
        {
            case Enums.BuildingType.Hut:
            case Enums.BuildingType.BigHut:
            case Enums.BuildingType.WoodHouse:
                box.size = new Vector3(12, 10, 12);
                break;
            case Enums.BuildingType.LumberHut:
            case Enums.BuildingType.LumberHouse:
                box.size = new Vector3(12, 10, 12);
                break;
            case Enums.BuildingType.Forager:
            case Enums.BuildingType.Hunter:
            case Enums.BuildingType.SmallFarm:
                box.size = new Vector3(20, 10, 20);
                break;
            case Enums.BuildingType.Mine:
            case Enums.BuildingType.Quarry:
            case Enums.BuildingType.IronMine:
            case Enums.BuildingType.DeepMine:
            case Enums.BuildingType.CoalMine:
            case Enums.BuildingType.StoneMine:
                box.size = new Vector3(15, 10, 15);
                break;
        }
    }

    public void Exit()
    {
        BuildGUIManager.Instance.thisCanvas.enabled = false;
        BuildGUIManager.Instance.UnBindZone();
    }
    public void Enter()
    {
        BuildGUIManager.Instance.UnBindZone();
        BuildGUIManager.Instance.thisCanvas.enabled = true;
        BuildGUIManager.Instance.BindZone(this);
    }

    private void OnDestroy()
    {
        AllZones.Remove(this);
    }
}
