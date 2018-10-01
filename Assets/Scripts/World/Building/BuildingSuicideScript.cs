using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingSuicideScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> GroupsToBuild;

    public event Action OnBuildingFinished = new Action(delegate { });

    public bool ReadyToBuild = true;

    public void Start()
    {
        OnBuildingFinished += () => Destroy(this);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void Run()
    {
        TurnOff();
        StartCoroutine(BuildInProgress());
    }

    public void TurnOff()
    {
        for (int i = 0; i < GroupsToBuild.Count; i++)
        {
            GameObject currentGroup = GroupsToBuild[i];
            for (int j = 0; j < currentGroup.transform.childCount; j++)
            {
                currentGroup.transform.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator BuildInProgress()
    {
        if (ReadyToBuild == false)
        {
            yield return new WaitWhile(() => ReadyToBuild == false);
        }
        while (GroupsToBuild.Count != 0)
        {
            GameObject currentGroup = GroupsToBuild[0];
            GroupsToBuild.Remove(currentGroup);
            for (int i = 0; i < currentGroup.transform.childCount; i++)
            {
                currentGroup.transform.GetChild(i).gameObject.SetActive(true);
                yield return null;// new WaitForSeconds(0.1f);
            }
        }
        OnBuildingFinished.Invoke();
    }



}
