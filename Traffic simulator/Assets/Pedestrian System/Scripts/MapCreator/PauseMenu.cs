using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PauseMenu : MonoBehaviour {

    public GameObject ui;
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
	}
    public void Toggle() // funkcja do włączania i wyaczania pauzy 
    {
        ui.SetActive(!ui.activeSelf);

        if (ui.activeSelf)
        {
            Time.timeScale = 0f; // zatrzymanie czasu w grze
        }else
        {
            Time.timeScale = 1f; // powrót do normalnego czasu gry 
        }
        
    }

    public void Continue()
    {
        Toggle();
    }

    public void SaveMap()
    {
        Toggle();
        Debug.Log("Map Saved");

        //Otwarcie okna do zapiswyanaia
        var path = EditorUtility.SaveFilePanel(
            "Save map as JSON",
            "",
            "nazwaPliku" + ".json",
            "json");

        if (path.Length != 0)
        {
            BuildManager.instance.StoreMap(path);
        }

    }

    public void LoadMap()
    {
        Debug.Log("Ładuje mape");
        Toggle();
        string path = EditorUtility.OpenFilePanel("Load map from JSON", "", "json");
        if (path.Length != 0)
        {
            BuildManager.instance.LoadMap(path);
        }
    }

    public void ResetMap()
    {
        Toggle();
        BuildManager.instance.mapa.Clear();// czyszcze liste emenetów drogi znajdujących się w w aktualnej lisćie do zapisania 
        BuildManager.instance.StoreMap("mapaJSon.json");// zapisuje pustą mape
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// przeładowuje scene kreatora
    }

    public void StartSimulation()
    {
        BuildManager.instance.StoreMap("mapaJSon.json"); // zapisuje aktalnie stworzoną mapę 
       Time.timeScale = 1f; // właczenie normalnego czasu
        Debug.Log("Simulation Started");
        SceneManager.LoadScene("Demo");// ładuje scene symulatora
    }

    public void Menu()
    {
        Debug.Log("Go to menu");
    }

    public void Exit()
    {
        Debug.Log("Exiting the game");
    }


}
