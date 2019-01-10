using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance; // instancja BuildMenagera, żeby można było sie od niego optymalnie odwóływac z innych klas
    public GameObject meshNode = null;//  element z którego skłąda się siatka
    
    static int wys = 30; // wysokość siatki 
    static int szer = 30;// szerokość siatki 
    private Transform[,] siatka = new Transform[wys,szer];// siatka z nodami || podać te same parametry co [wys, szer]
    private bool[,] isNodeEmpty = new bool[wys, szer];// tablica zajętości planszy || podać te same parametry co [wys, szer]

    //string json = JsonConvert.SerializeObject(instance);
    public List<ElementDrogi> mapa = new List<ElementDrogi>(); // mapa do którje zapisujemy podczas tworzenia
   
   


    private void Awake()
    {

        if(instance != null)
        {
            Debug.LogError("Wiecej niz jeden BuildMenager !!!!");
        }
        instance = this;

        for (int i = 0; i<wys ; i++)
        {
            for (int j = 0; j<szer ; j++ )
            {
                GameObject oneNode = (GameObject)Instantiate(meshNode, new Vector3(i*15, 0, j*15), transform.rotation);
                siatka[i, j] = oneNode.transform;
                isNodeEmpty[i, j] = true; // zapełnienie tablicy "pustości" warotściami true (ZEROWANIE)
                
                
            }
        }
     // LoadMap("test.json"); //############################# LOAD MAP ###################################################
       // Debug.Log(skrzyzowanie_2jezdniowe.GetInstanceID());
       
    }

    public void LoadMap(string path) // funkcja ładująca mape z pliku
    {
        string jsonLoadedMap = File.ReadAllText(path); // pobieramy mape z Jsona do stringa 
        ElementDrogi[] loadedMap = JsonHelper.FromJson<ElementDrogi>(jsonLoadedMap); // zamiana z Jsona na tablice naszych elementów na drodze
       // Debug.Log(jsonLoadedMap);

        for (int k = 0; k < loadedMap.Length; k++)// kreator tworzy mape na podstawie tablicy 
        {
           //Debug.Log(loadedMap[k].prefab);
           GameObject wczytanyElementDrogi = (GameObject)Instantiate(loadedMap[k].prefab, loadedMap[k].position, Quaternion.identity);
        }
    }

    
    public GameObject skrzyzowanie_2jezdniowe;
    public GameObject droga_2jezdniowa;
    public GameObject skrzyzowanie_4jezdniowe;
    public GameObject droga4pasy;

   
    

   


    private ElementDrogi elementDrogiDoZbudowania;//ŚWIĘTE
    //private Node selectedNode;

     
    public bool CanBuild { get { return elementDrogiDoZbudowania != null; } } //property 


    public void BuildElementDrogiOn (Node node)
    {
        GameObject elementDrogi = (GameObject)Instantiate(elementDrogiDoZbudowania.prefab, node.GetBuildPosition(), Quaternion.identity); // Tutaj 
        node.elementDrogi = elementDrogi;// przekazanie elemnetu dorgi do node
        ElementDrogi element = new ElementDrogi(elementDrogiDoZbudowania.prefab, elementDrogiDoZbudowania.prefab.name, node.GetBuildPosition());// z każdym postawieniem elementu twporzymy nowy, żeby się nie odnosić za kazdym razem przez referencje

      
        mapa.Add(element);// dodajemy stworzony przez nas element drogi do mapy


        StoreMap("C:\\ProjektyUnity/mapaJSon.json");// #################################### STORE MAP 

        
        //Debug.Log(json);
        /*mapa.ForEach(el => Debug.Log(
            "Pozycja: " + el.position.x + " " + el.position.y + " " + el.position.z +
            "Nazwa : " + el.prefab.name

            )); */

    }

    public void StoreMap(string path) // zapisuje stworzoną w kreatorze mape do pliku
    {
        string json = JsonHelper.ToJson(mapa.ToArray(), true);// trzeba używać dodatkowej klasy z Wrapperem, bo GUPIE UNITY  nie umie Jsonować list i tablic
                                                              //i wógóle nic poza obiektami gupi gupek jest 7:06 dnia 07.10.18 Parsowanie DZIAŁA!!
        File.WriteAllText(path, json); // zapis jsona do pliku

    }

    public void SelectElementDrogiDoZbudowania(ElementDrogi elementDrogi)//ŚWIĘTE
    {
        elementDrogiDoZbudowania = elementDrogi;
    }


    // to na potem do edycji elementu na nodzied
    /*public void SelectNode(Node node)// metoda do wyboru node
    {
        selectedNode = node;
        elementDrogiDoZbudowania = null;
    }*/ 

    //to dodałem // początek FUNKCJE OD SPRAWDZANIA ZAJĘTOŚCI POLA 
    public bool getIsNodeEmpty(int x, int z)
    {
        return isNodeEmpty[x, z];
    }
    public void setIsNodeEmptyFalse(int x, int z)
    {
        isNodeEmpty[x, z] = false ;
    }
  

    public Transform Getsiatka(int i, int j)
    {
        return siatka[i,j];
    }
    public int GetWys()
    {
        return wys;
    }
    public int GetSzer()
    {
        return szer;
    }

}
//DODAWANIE NOWEGO ELEMENTU DROGI DO KREATORA
//1) Stwórz nowy GameObject w tym pliku
//2) Przejdź do Shop.cs i stwórz nową metode dla danego oiektu, która poźniej możan przypisać do buttona
//3) Przejdź do Node.cs i w funkcjach takeYourDamnPlace i isEnoghtPlace dodaj nowe warunek dla danego modelu 
//4) Przejdź do Unity: GameMaster-> (po prawo) w Build Manager powinno pojawić sie nowe pole o nazwie którą dodałeś, przenieś w to pole prefaba modelu który chcesz dodać
//5)    Canvas->Shop-> zduplikuj dowolny button -> (po prawo) nadaj mu adekwatną nazwe -> w komponencie Button(Script) zmień metode obsługującą dany button na tą którą stworzyłeś w punkcie 2.
//6)    Button już powinien działać, ale jest brzdki AF wiec zmień mu ikonę na właściwą
//6.1)Weź od Banana fantastyczną ikone
//6.2)Przeciągnij ją do folderu Ikony (najleiej bezpośrednio do Unity, bo jak wrzuciłem do folderu to coś sie nie odświeżało )
//6.3)Kliknij na nową ikone i zmień jej Texture Type na Sprite(2D and UI)
//6.4)Przejdź do : Canvas ->Shop -> twójNowoDodanyButton -> w komponencie Image(Script) jest pole Source Image, przecianij tam swoją ikone 
// GOTOWE 