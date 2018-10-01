using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public GameObject loadedlvl;

    public void Exit()
    {
        Application.Quit();
    }

    public void Load()
    {
        if (loadedlvl.GetComponent<SaveLoad>().loadable)
        {
            loadedlvl.GetComponent<SaveLoad>().toload = true;
            SceneManager.LoadScene("Teren");
            DontDestroyOnLoad(loadedlvl);
            SceneManager.MoveGameObjectToScene(loadedlvl, SceneManager.GetSceneByName("Teren"));
        }


    }

    public void NewGame()
    {
        
        SceneManager.LoadScene("Teren");
        DontDestroyOnLoad(loadedlvl);
        SceneManager.MoveGameObjectToScene(loadedlvl, SceneManager.GetSceneByName("Teren"));
    }
}
