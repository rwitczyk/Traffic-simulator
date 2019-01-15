using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {
	
	public Color hoverColor_if_empty;
    public Color hoverColor; // kolor siatki po najechaniu myszką na okienko 
    public Vector3 possitionOffset; // parametr służący do tego aby tworzony przez nas element był na siatce a nie w niej lub pod 
    public Quaternion rotationOffset; // zeby obrócić o 90 stopni Y = 1 & W = 1


    [Header("Opcjonalny parametr")]
    public GameObject elementDrogi;

  
    private Renderer rend;
    private Color startColor; // początkowy kolor siatki

    BuildManager buildManager;


    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }
    public Vector3 GetBuildPosition()
    {
        return transform.position + possitionOffset;
    }
    
    private void OnMouseDown()
    {
       
        if (EventSystem.current.IsPointerOverGameObject())// po to żeby nie budować na siatce jak wybieramy element do budowania a akurat menu wyboru jest nad siatką 
        {
            Debug.Log(" klikasz w siatkę przez element sklepu ");
            return;
        }

        if (!buildManager.CanBuild)
            return;

        if (elementDrogi != null)
        {
            Debug.Log("W tym miejscu istnieje juz element drogi !!!!"); // zmienić żeby się wyświetlało na ekranie a nie na konsli 
           // buildManager.SelectNode(this);
            return;
        }
        buildManager.BuildElementDrogiOn(this);
    }
    //NewtonSoftJSon.dll
    //TODO
    //1) przerobić funkcje void takeYourDamnPlace(int x, int z) i isEnoghtPlace(int x, int z) tak żeb offsety
    //   dostosowały do nazwy budowanego elementu
    //2) dorobić haszmape <elementDrogi.name  , int rozmiar>
    //3) Posprzątać szit z nazwami w BuildManagerze (standardowyPrefabDrogi, kolejnyPrefabDrogi...)
    //4) opierdolić banana żeby wykminił drogę 4pasy do 2 pasy jakoś bardziej kwadratowo 
    //5) dorobić żeby się zaznaczał pod myszką taki obszar na jakim stawiamy ( LINIA 134)

    void takeYourDamnPlace(int x, int z, string modelName)// tylko do modelu 5x5 zajmuje odpowiednią ilość miejsca dla danego modelu
    {

        int offset;
       

        if (modelName == "skrzyzowanie_2jezdniowe" || modelName == "skrzyzowanie_2jezdniowe(clone)" ||
            modelName == "skrzyzowanie_4jezdniowe" || modelName == "skrzyzowanie_4jezdniowe(clone)")
            offset = 2;
        else if (modelName == "droga4pasy" || modelName == "droga4pasy(clone)")
            offset = 1;
        else if (modelName == "droga_2jezdniowa" || modelName == "droga_2jezdniowa(clone)")
            offset = 0;
        else
            offset = 0;
        for (int i = x - offset; i <= x + offset; i++)
        {
            for (int j = z - offset; j <= z + offset; j++)
            {
                BuildManager.instance.setIsNodeEmptyFalse(i, j);
                //Debug.Log(i + " " + j);

            }
        }

    }

    bool isEnoghtPlace(int x, int z, string modelName)// tylko do modelu 5x5
    {

        int offset;
        bool result = true;

        if (modelName == "skrzyzowanie_2jezdniowe" || modelName == "skrzyzowanie_2jezdniowe(clone)" ||
            modelName == "skrzyzowanie_4jezdniowe" || modelName == "skrzyzowanie_4jezdniowe(clone)")
            offset = 2;
        else if (modelName == "droga4pasy" || modelName == "droga4pasy(clone)")
            offset = 1;
        else if (modelName == "droga_2jezdniowa" || modelName == "droga_2jezdniowa(clone)")
            offset = 0;

        else
            offset = 0;
       /* switch (modelName)
        {
            case "skrzyzowanie_2jezdniowe":
                offset = 2;
                Debug.Log("Jestem na 1");
                    break;
            case "droga_2jezdniowa":
                offset = 0;
                Debug.Log("Jestem na 2");
                break;
            default:
                offset = 0;
                Debug.Log("Jestem na defaulcie");
                break;

        }*/

        if (x < offset || z < offset || x >= BuildManager.instance.GetWys() - offset || z >= BuildManager.instance.GetSzer() - offset)// gdy mamy model 5x5 sprawdzenie czy nie wychodzimy poza mape 
            return false;
        else
        {
            for (int i = x - offset; i <= x + offset; i++)
            {
                for (int j = z - offset; j <= z + offset; j++)
                {
                    if (!BuildManager.instance.getIsNodeEmpty(i, j))
                        result = false;
                }
            }
            return result;
        }
    }

 
    void OnMouseEnter() 
    {
               
        if (EventSystem.current.IsPointerOverGameObject())// po to żeby nie budować na siatce jak wybieramy element do budowania a akurat menu wyboru jest nad siatką 
            return;

        if (!buildManager.CanBuild)
            return;

            rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;

        
    }


}
