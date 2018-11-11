using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour {
	
	public Color hoverColor_if_empty;
    public Color hoverColor; // kolor siatki po najechaniu myszką na okienko 
    public Vector3 possitionOffset; // parametr służący do tego aby tworzony przez nas element był na siatce a nie w niej lub pod 
    public Quaternion rotationOffset; // zeby obrócić o 90 stopni Y = 1 & W = 1


    private GameObject elementDrogi;
    private bool test = true; // do testowania 
                              //private isNodeEmpty


    private Renderer rend;
    private Color startColor; // początkowy kolor siatki

    BuildManager buildManager;


    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())// po to żeby nie budować na siatce jak wybieramy element do budowania a akurat menu wyboru jest nad siatką 
            return;

        if (buildManager.GetelementDrogiDoZbudowania() == null)
            return;
        
        if (elementDrogi != null)
        {
            Debug.Log("W tym miejscu istnieje juz element drogi !!!!"); // zmienić żeby się wyświetlało na ekranie a nie na konsli 
            return;
        }
        if (isEnoghtPlace((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15))
        {
            GameObject elementDrogiDoZbudowania = BuildManager.instance.GetelementDrogiDoZbudowania();
            elementDrogi = (GameObject)Instantiate(elementDrogiDoZbudowania, transform.position + possitionOffset, transform.rotation * rotationOffset);
            takeYourDamnPlace((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15);
            elementDrogi.transform.SetParent(rend.transform);// elementu siatki jako parenta do elementu drogi 
            Debug.Log(elementDrogi.name);

        }
        //Debug.Log("x : " + rend.transform.position.x + " z : " + rend.transform.position.z);//pozycja aktualnie klikanego noda
        //Debug.Log("x : " + rend.transform.position.x / 15 + " z : " + rend.transform.position.z / 15);// pozycja w tablicy
        //Debug.Log(BuildManager.instance.getIsNodeEmpty((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15));
        // Debug.Log(elementDrogi.transform.name);//to dodałem 
        //Debug.Log(rend.transform.worldToLocalMatrix);//to też 

        //Debug.Log();
    }

    //TODO
    //1) przerobić funkcje void takeYourDamnPlace(int x, int z) i isEnoghtPlace(int x, int z) tak żeb offsety
    //   dostosowały do nazwy budowanego elementu
    //2) dorobić haszmape <elementDrogi.name  , int rozmiar>
    //3) Posprzątać szit z nazwami w BuildManagerze (standardowyPrefabDrogi, kolejnyPrefabDrogi...)
    //4) opierdolić banana żeby wykminił drogę 4pasy do 2 pasy jakoś bardziej kwadratowo 
    //5) dorobić żeby się zaznaczał pod myszką taki obszar na jakim stawiamy ( LINIA 134)

    void takeYourDamnPlace(int x, int z)// tylko do modelu 5x5 zajmuje odpowiednią ilość miejsca dla danego modelu
    {
        for (int i = x - 2; i <= x + 2; i++)
        {
            for (int j = z - 2; j <= z + 2; j++)
            {
                BuildManager.instance.setIsNodeEmptyFalse(i, j);
                //Debug.Log(i + " " + j);

            }
        }

    }

    bool isEnoghtPlace(int x, int z)// tylko do modelu 5x5
    {
        bool result = true;

        if (x >= 29 || x <= 1 || z >= 29 || z <= 1)// gdy mamy model 5x5 sprawdzenie czy nie wychodzimy poza mape 
            return false;
        else
        {
            for (int i = x - 2; i <= x + 2; i++)
            {
                for (int j = z - 2; j <= z + 2; j++)
                {
                    if (!BuildManager.instance.getIsNodeEmpty(i, j))
                        result = false;
                }
            }
            return result;
        }
    }

   /* bool isEnoghtPlace(int x, int z)// metoda sprawdzająca czy jest wystarczająco dużo miejsca na siatce, żeby wstawić element
    {
        if (x >= 29 || x <= 0 || z >= 29 || z <= 0)
            return false;
        else
        {
            if (BuildManager.instance.getIsNodeEmpty(x, z) && //0
            BuildManager.instance.getIsNodeEmpty(x - 1, z) && //1
            BuildManager.instance.getIsNodeEmpty(x - 1, z + 1) && //2
            BuildManager.instance.getIsNodeEmpty(x, z + 1) && //3
            BuildManager.instance.getIsNodeEmpty(x + 1, z + 1) && //4
            BuildManager.instance.getIsNodeEmpty(x + 1, z) && //5
            BuildManager.instance.getIsNodeEmpty(x + 1, z - 1) && //6
            BuildManager.instance.getIsNodeEmpty(x, z - 1) && //7
            BuildManager.instance.getIsNodeEmpty(x - 1, z - 1)) //8
                return true;
            else
                return false;
        }
    }*/
	/*
	public void OnMouseEnter() 
    {
        if (isEnoghtPlace((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15))   
            rend.material.color = hoverColor_if_empty;
        else
            rend.material.color = hoverColor;
        //  Debug.Log(rend.bounds.center.x);
    }
	
	*/


    void OnMouseEnter() 
    {
        if (EventSystem.current.IsPointerOverGameObject())// po to żeby nie budować na siatce jak wybieramy element do budowania a akurat menu wyboru jest nad siatką 
            return;

        if (buildManager.GetelementDrogiDoZbudowania() == null)
            return;

        //rend.material.color = hoverColor;
		
		 if (isEnoghtPlace((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15))   
            rend.material.color = hoverColor_if_empty;
        else
            rend.material.color = hoverColor;
       // BuildManager.instance.Getsiatka((int)rend.transform.position.x / 15, (int)rend.transform.position.z / 15 + 15).GetComponent<Renderer>().material.color = hoverColor;
       //POMYSŁ DOBRY TYLKO DOROBIĆ PĘTLE JAKĄŚ DO TEGO ALBO CUŚ I WARUNKI 
        //Debug.Log(rend.transform.position.x);
        //Debug.Log(rend.transform.position.z);
        // Debug.Log(rend.bounds.center.x);
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;

        
    }


}
