using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopTextManager : MonoBehaviour
{
    private static PopTextManager instance;
    public static PopTextManager Instance
    {
        get
        {
            return instance;
        }
    }

    private Text PopText;

    private void Awake()
    {
        instance = this;
        PopText = this.gameObject.GetComponent<Text>();
        
    }

    public void Run()
    {
        StartCoroutine(PopTextLoop());
    }

    private IEnumerator PopTextLoop()
    {
        while(true)
        {
            PopText.text = City.Instance.GetPopString();
            yield return new WaitForSeconds(1);
        }
    }
}
