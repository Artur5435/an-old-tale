using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityCenter : MonoBehaviour
{
    private static CityCenter instance;
    public static CityCenter Instance
    {
        get
        {
            return instance;
        }
    }


    private GameObject gui;
    private Canvas guiCanvas;
    private InputField cityname;
    private Text pop;
    private byte curType;

    public Enums.BuildingType[] priority;
    public Button[] tempbut;
    public Toggle[] temptoggle;
    public BoxCollider box;

    public string cityName;


    private void Awake()
    {
        instance = this;
        box = gameObject.GetComponent<BoxCollider>();
    }

    void Start()
    {

        priority = new Enums.BuildingType[255];
        gui = GameObject.Find("CityCent");
        guiCanvas = gui.GetComponent<Canvas>();
        temptoggle = gui.GetComponentsInChildren<Toggle>();
        tempbut = gui.GetComponentsInChildren<Button>();
        for (int i = 0; i < priority.Length; i++) priority[i] = 0;
        

        //cityname = gui.GetComponentInChildren<InputField>();
        //cityname.onEndEdit.AddListener(setName);
        changedPriority();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            guiCanvas.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
            guiCanvas.enabled = false;
    }

    void setName(string s)
    {
        cityName = s;
    }

    public void changedPriority()
    {
        for (int i = 0; i < PriorityUpDown.PriorityList.Count; i++)
        {
            if (PriorityUpDown.PriorityList[i].state)
            {
                priority[PriorityUpDown.PriorityList[i].gameObject.transform.GetSiblingIndex()] = PriorityUpDown.PriorityList[i].type;
            }
            else
            {
                priority[PriorityUpDown.PriorityList[i].gameObject.transform.GetSiblingIndex()] = Enums.BuildingType.None;
            }
        }
    }
}
