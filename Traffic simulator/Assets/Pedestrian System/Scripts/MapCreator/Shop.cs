using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public ElementDrogi skrzyzowanie_2Jezdniowe;
    public ElementDrogi droga_2Jezdniowa;
    public ElementDrogi skrzyzowanie_4Jezdniowe;
    public ElementDrogi droga4Pasy;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }

    public void selectSkrzyzowanie2Jezdniowe()
    {
        Debug.Log("wybierz skrzyzowanie_2jezdniowe");
        buildManager.SelectElementDrogiDoZbudowania(skrzyzowanie_2Jezdniowe);
    }
    public void SelectDroga_2jezdniowa()
    {
        Debug.Log("wybierz droga_2jezdniowa");
        buildManager.SelectElementDrogiDoZbudowania(droga_2Jezdniowa);
    }
    public void Selectskrzyzowanie_4jezdniowe()
    {
        Debug.Log("wybierz skrzyzowanie_4jezdniowe");
        buildManager.SelectElementDrogiDoZbudowania(skrzyzowanie_4Jezdniowe);
    }
          public void Selectdroga4pasy()
    {
        Debug.Log("wybierz droga4pasy");
        buildManager.SelectElementDrogiDoZbudowania(droga4Pasy);
    }
}
