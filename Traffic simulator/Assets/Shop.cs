using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void selectSkrzyzowanie2Jezdniowe()
    {
        Debug.Log("wybierz skrzyzowanie_2jezdniowe");
        buildManager.SetElementDrogiDoZbudowania(buildManager.standardowyPrefabDrogi);
    }
    public void selectSkrzyzowanie3Jezdniowe()
    {
        Debug.Log("wybierz skrzyzowanie_3jezdniowe");
        buildManager.SetElementDrogiDoZbudowania(buildManager.kolejnyPrefabDrogi);
    }
}
