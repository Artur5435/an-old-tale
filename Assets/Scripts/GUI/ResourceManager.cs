using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static List<ResourceManager> ResourcesList = new List<ResourceManager>();

    [SerializeField]
    private Enums.Material materialToDisplay;

    private Text thisText;
    private CityMaterial materialInstance;

    private void Awake()
    {
        ResourcesList.Add(this);
        thisText = this.gameObject.GetComponent<Text>();
    }

    public void InitMaterialGUI()
    {
        materialInstance = City.Instance.MaterialList.Find(x => x.GetMaterialType() == materialToDisplay);
        thisText.text = materialInstance.GetCount().ToString();
        StartCoroutine(DisplayLoop());
    }

    private IEnumerator DisplayLoop()
    {
        while (true)
        {
            thisText.text = materialInstance.GetCount().ToString();
            yield return null;
        }
    }

}
