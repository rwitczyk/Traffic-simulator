using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymulatorPauseMenu : MonoBehaviour {

    public GameObject ui;

    void Update()
    {
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
        }
        else
        {
            Time.timeScale = 1f; // powrót do normalnego czasu gry 
        }

    }

    public void GoToCreator()
    {
        Debug.Log("Going to creator");
    }
    public void LoadMap()
    {
        Debug.Log("Loading map");
    }



}
