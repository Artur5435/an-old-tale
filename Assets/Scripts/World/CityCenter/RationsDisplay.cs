using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RationsDisplay : MonoBehaviour {

    private static RationsDisplay instance;
    public static RationsDisplay Instance
    {
        get
        {
            return instance;
        }
    }
    public Slider slider;
    private Text text;
    // Use this for initialization
    private void Awake()
    {
        instance = this;
    }

    void Start () {
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = City.Instance.foodRations.ToString();
	}
}
