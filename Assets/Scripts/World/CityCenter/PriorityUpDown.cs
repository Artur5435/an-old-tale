using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PriorityUpDown : MonoBehaviour
{
    public static List<PriorityUpDown> PriorityList = new List<PriorityUpDown>();

    public Enums.BuildingType type;
    public Button up;
    public Button down;
    public Toggle toggle;
    public bool state = true;

    private void Awake()
    {
        PriorityList.Add(this);
    }
    // Use this for initialization
    void Start()
    {
        up.onClick.AddListener(moveUp);
        down.onClick.AddListener(moveDown);
        toggle.onValueChanged.AddListener(typeState);
    }


    void typeState(bool state)
    {
        this.state = state;
        CityCenter.Instance.changedPriority();
    }

    void moveUp()
    {
        gameObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() - 1);
        CityCenter.Instance.changedPriority();
    }

    void moveDown()
    {
        gameObject.transform.SetSiblingIndex(gameObject.transform.GetSiblingIndex() + 1);
        CityCenter.Instance.changedPriority();
    }
}
