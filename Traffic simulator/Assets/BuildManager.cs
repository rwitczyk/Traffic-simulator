using UnityEngine;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance; // instancja BuildMenagera, żeby można było sie od niego optymalnie odwóływac z innych klas
    public GameObject meshNode = null;//  element z którego skłąda się siatka
    
    public int wys = 30;
    public int szer = 30;
    private Transform[,] siatka = new Transform[30,30];// siatka z nodami
    private bool[,] isNodeEmpty = new bool[30, 30];
   

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
                //siatka[i,j].SetParent(parent);// to
                
            }
        }
      // Debug.Log(parent.childCount);

      //  newParent.transform.SetParent(siatka[1,1]);
    //GameObject oneNode = (GameObject)Instantiate(elementDrogiDoZbudowania, transform.position + possitionOffset, transform.rotation);

}

   



    public GameObject standardowyPrefabDrogi;
    public GameObject kolejnyPrefabDrogi; //nowe 


    private GameObject elementDrogiDoZbudowania;




   /* private void Start()
    {
        elementDrogiDoZbudowania = standardowyPrefabDrogi;
    } */

    

    public GameObject GetelementDrogiDoZbudowania()
    {
        return elementDrogiDoZbudowania;
    }
    public void SetElementDrogiDoZbudowania(GameObject elementDrogi)
    {
        elementDrogiDoZbudowania = elementDrogi;
    }

    //to dodałem // początek FUNKCJE OD SPRAWDZANIA ZAJĘTOŚCI POLA 
    public bool getIsNodeEmpty(int x, int z)
    {
        return isNodeEmpty[x, z];
    }
    public void setIsNodeEmptyFalse(int x, int z)
    {
        isNodeEmpty[x, z] = false ;
    }
    //koniec

    public Transform Getsiatka(int i, int j)
    {
        return siatka[i,j];
    }
}
