using UnityEngine;
//using System.Collections;
using System.Collections.Generic;
using System.IO;

//[System.Serializable]
//[ExecuteInEditMode]
public class PedestrianSystem : MonoBehaviour
{
//<<<<<<< HEAD
    public static PedestrianSystem Instance { get; set; }

    public bool m_showGizmos = true;
    public PedestrianNode m_nodePrefab = null;
    public bool m_autoLink = true;             // enable to automatically link the edit and anchor node together on generate node
    public bool m_linkBothDir = true;             // enable to link the anchor with the edit node and also the edit with the anchor node

    [Range(0.0f, 5.0f)]
    public float m_globalSpeedVariation = 0.5f; // used to generate a slight variation of speed each node a object gets to

    [Range(0.0f, 5.0f)]
    public float m_globalLanePosVariation = 0.0f; // used to generate a slight variation of lane position for all object

    public Texture2D TextureIconAnchor = null;
    public Texture2D TextureIconEdit = null;
    public Texture2D TextureIconAnchorToEdit = null;
    public Texture2D TextureIconEditToAnchor = null;
    public Texture2D TextureIconRemoveAnchor = null;
    public Texture2D TextureIconRemoveEdit = null;
    public Texture2D TextureIconRemoveAll = null;

    public PedestrianNode AnchorNode { get; set; }
    public PedestrianNode EditNode { get; set; }
    public PedestrianNode PreviousAnchorNode { get; set; }
    public PedestrianNode PreviousEditNode { get; set; }
    public GameObject TooltipAnchor { get; set; }
    public GameObject TooltipEdit { get; set; }

    public int m_objectSpawnCountMax = -1;   // if -1 then unlimited objects can spawn. If higher than only this amount will ever spawn using the Traffic System spawn options
    public bool m_randomObjectSpawnPerNode = false;
    [Range(0, 1)]
    public float m_randomObjectSpawnChancePerNode = 0.0f;
    public int m_numOfObjectsSpawnedPerNode = 1;

    private List<Transform> CLRevealObjectsFrom = new List<Transform>();
    private List<Transform> CLRevealObjectsTo = new List<Transform>();

    public List<PedestrianObject> m_objectPrefabs = new List<PedestrianObject>();
    private List<PedestrianObjectSpawner> m_objectSpawners = new List<PedestrianObjectSpawner>();
    private List<PedestrianObject> m_spawnedObjects = new List<PedestrianObject>();
    public List<PedestrianObject> GetSpawnedObjects() { return m_spawnedObjects; }


    public List<PedestrianNode> global_list_of_nodes = new List<PedestrianNode>();


    //***************** Witor dodaje 

    public GameObject m_nodeToCreate = null;
    //public PedestrianNode[] arrayOfPedestrianNodes = new PedestrianNode[6];
    public List<PedestrianNode> arrayOfPedestrianNodes = new List<PedestrianNode>();
    public GameObject skrzyzowanie_2jezdniowe;
    public GameObject droga_2jezdniowa;
    public GameObject skrzyzowanie_4jezdniowe;
    public GameObject droga4pasy;

    //***************************************

    public enum Tooltip
    {
        ANCHOR = 0,
        EDIT = 1
    }

    public enum ObjectFrequency
    {
        HIGH = 0,
        MEDIUM = 1,
        LOW = 2
    }

    void Awake()
    {

        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        //**********Wiktor dodaje
        for (int i = 0; i < 5; i++)// pętla do tworzenia nodów
        {
            //GameObject node = (GameObject)Instantiate(m_nodeToCreate, new Vector3(-81+(i*5), 2, -15), transform.rotation);
            PedestrianNode node = Instantiate(PedestrianSystem.Instance.m_nodePrefab, new Vector3(-81 + (i * i), 2, -15 * (i * i)), transform.rotation) as PedestrianNode;
            arrayOfPedestrianNodes.Add(node);

        }

        for (int i = 0; i < 4; i++) // pętla do łączenia nodów 
        {
            arrayOfPedestrianNodes[i].canSpawn = false;
            SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, arrayOfPedestrianNodes[i] as PedestrianNode);
            SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, arrayOfPedestrianNodes[i + 1] as PedestrianNode);
            Instance.LinkNode();

        }

        //arrayOfPedestrianNodes[0].canSpawn = false;
        //LoadMap("C:\\ProjektyUnity/mapaJSon.json");
        LoadMap("mapaJSon.json");


        //Debug.Log("Numer instancji skrzyzowanie_2jezdniowe = " + skrzyzowanie_2jezdniowe.GetInstanceID()); //Numer instancji skrzyzowanie_2jezdniowe = 3458
        //Debug.Log("Numer instancji droga_2jezdniowa = " + droga_2jezdniowa.GetInstanceID()); //Numer instancji droga_2jezdniowa = 3492
        //Debug.Log("Numer instancji skrzyzowanie_2jezdniowe = " + skrzyzowanie_4jezdniowe.GetInstanceID()); //Numer instancji skrzyzowanie_2jezdniowe = 2496
        //Debug.Log("Numer instancji skrzyzowanie_2jezdniowe = " + droga4pasy.GetInstanceID()); //Numer instancji skrzyzowanie_2jezdniowe = 3564

        //TODO Ogarnąć coś z tymi numerami instancji w prefabach modeli,  bo chuja idzie dostać !!!

        //********************

        
    }

    public void GenerateNodesOn(ElementDrogi element)// funkcja do gnerowania nodów na danym elemencie
    {
        switch (element.name)
        {
            case "droga_2jezdniowa":
                GenerateNodesOn_droga_2jezdniowa(element);
                break;
            case "skrzyzowanie_2jezdniowe":
                GenerateNodesOn_skrzyzowanie_2jezdniowe(element);
                break;

        }
    }
    public void GenerateNodesOn_skrzyzowanie_2jezdniowe(ElementDrogi element)// funkcja do generowania i łączenia nodów na droga_2jezdniowa
    {
        List<PedestrianNode> listOfNodes = new List<PedestrianNode>();
        List<Vector3> vectorsList = new List<Vector3>
        {
            new Vector3(37.5f, 2, 3),//0
            new Vector3(7f, 2, 3),//1
            new Vector3(3.5f, 2, 4),//2
            new Vector3(3, 2, 8),//3
            new Vector3(3, 2, 37.5f),//4
            new Vector3(-3, 2, 37.5f),//5
            new Vector3(-3, 2, 8),//6
            new Vector3(-4, 2, 4),//7
            new Vector3(-7, 2, 2),//8
            new Vector3(-37.5f, 2, 2),//9
            new Vector3(-37.5f, 2, -3),//10
            new Vector3(-7, 2, -3),//11
            new Vector3(-4, 2, -4),//12
            new Vector3(-3, 2, -7),//13
            new Vector3(-3, 2, -37.5f),//14
            new Vector3(3, 2, -37.5f),//15
            new Vector3(3, 2, -7),//16
            new Vector3(3.5f, 2, -4),//17
            new Vector3(7, 2, -3),//18
            new Vector3(37.5f, 2, -3),//19
            new Vector3(0, 2, 0) // 20 240 2 45
        };// lista do przechowywanie pozycji względnej każdego noda




        for (int k = 0; k < vectorsList.Count; k++)// pętla do tworzenia nodów
        {
            PedestrianNode node = Instantiate(PedestrianSystem.Instance.m_nodePrefab, vectorsList[k] + element.position, transform.rotation) as PedestrianNode;
            listOfNodes.Add(node);
            node.canSpawn = false;
            global_list_of_nodes.Add(node);
        }
        //listOfNodes[0].canSpawn = true;

        // LinkNodes(listOfNodes[0], listOfNodes[1]);
        for (int i = 1; i < vectorsList.Count - 1; i++)
        {
            if (i % 5 != 0)
            {
                LinkNodes(listOfNodes[i - 1], listOfNodes[i]);
            }

        }
        /* listOfNodes[3].canSpawn = true;

         SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, listOfNodes[1] as PedestrianNode);
         SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, listOfNodes[0] as PedestrianNode);
         Instance.LinkNode();

         SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, listOfNodes[3] as PedestrianNode);
         SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, listOfNodes[2] as PedestrianNode);
         Instance.LinkNode();*/

    }




    public void LinkNodes(PedestrianNode anchorNode, PedestrianNode editNode) // łaczenie ndówch nodów ze sobą 
    {
        SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, anchorNode as PedestrianNode);// ustawiam pierwszego jako kotwice 
        SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, editNode as PedestrianNode);// drugiego jako klucz 
        Instance.LinkNode();// Łączę 
    
    }




    public void GenerateNodesOn_droga_2jezdniowa(ElementDrogi element)// funkcja do generowania i łączenia nodów na droga_2jezdniowa
    {
        List<PedestrianNode> listOfNodes = new List<PedestrianNode>();
        List<Vector3> vectorsList = new List<Vector3>
        {
            new Vector3(-7.5f, 2, 3),
            new Vector3(7.5f, 2, 3),
            new Vector3(7.5f, 2, -3),
            new Vector3(-7.5f, 2, -3)
        };// lista do przechowywanie pozycji względnej każdego noda

        for (int i = 0; i < vectorsList.Count; i++)// pętla do tworzenia nodów
        {
            PedestrianNode node = Instantiate(PedestrianSystem.Instance.m_nodePrefab, vectorsList[i] + element.position, transform.rotation) as PedestrianNode;
            listOfNodes.Add(node);
            node.canSpawn = false;
            // Debug.Log(node.transform.position);
            global_list_of_nodes.Add(node);
        }
        listOfNodes[0].canSpawn = true;
        listOfNodes[3].canSpawn = true;
        listOfNodes[0].Set_Is_Anchor(true);
        listOfNodes[1].Set_Is_Key(true);
        listOfNodes[2].Set_Is_Anchor(true);
        listOfNodes[3].Set_Is_Key(true);


        SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, listOfNodes[1] as PedestrianNode);
        SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, listOfNodes[0] as PedestrianNode);
        Instance.LinkNode();

        SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, listOfNodes[3] as PedestrianNode);
        SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, listOfNodes[2] as PedestrianNode);
        Instance.LinkNode();

    }




    public void Link_All_Nodes(List<PedestrianNode> lista)
    {
        int length = lista.Count;
        for (int i=0;i<length;i++)
        {
            for(int j=0;j<length;j++)
            {
                if(lista[i].transform.position.Equals(lista[j].transform.position))
                {
                        SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, lista[j] as PedestrianNode);
                        SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, lista[i] as PedestrianNode);

                    Instance.LinkNode();
                }
            }
        }
    }




    //*************************WIKTOR dodaje 
    public void LoadMap(string path) // funkcja ładująca mape z pliku
    {
        string jsonLoadedMap = File.ReadAllText(path); // pobieramy mape z Jsona do stringa 
        ElementDrogi[] loadedMap = JsonHelper.FromJson<ElementDrogi>(jsonLoadedMap); // zamiana z Jsona na tablice naszych elementów na drodze

        for (int k = 0; k < loadedMap.Length; k++) // ta pętla BARRRDZO ważna jest robi pożądek z instancjami prefabów bo jak się w jednym projekcie tworzy mape a w drugim wczytuje to się psujo instanceID
        {
            switch (loadedMap[k].name)
            {
                case "skrzyzowanie_2jezdniowe":
                    loadedMap[k].prefab = skrzyzowanie_2jezdniowe;
                    break;
                case "droga_2jezdniowa":
                    loadedMap[k].prefab = droga_2jezdniowa;
                    break;
                case "skrzyzowanie_4jezdniowe":
                    loadedMap[k].prefab = skrzyzowanie_4jezdniowe;
                    break;
                case "droga4pasy":
                    loadedMap[k].prefab = droga4pasy;
                    break;

            }
        }



        for (int k = 0; k < loadedMap.Length; k++)// kreator tworzy mape na podstawie tablicy 
        {
            // Debug.Log("Prefab " + loadedMap[k].prefab);
            // Debug.Log( "Tu się zaczynam " + jsonLoadedMap);
            GameObject wczytanyElementDrogi = (GameObject)Instantiate(loadedMap[k].prefab, loadedMap[k].position, Quaternion.identity);
            GenerateNodesOn(loadedMap[k]);
        }
    }




    //**************************




    void Start()
    {
        if (Instance != this)
            return;

        if (!global_list_of_nodes.Equals(null))
        {
            Link_All_Nodes(global_list_of_nodes);
        }

        if (m_randomObjectSpawnPerNode && Application.isPlaying)
        {
            PedestrianNode[] nodes = GameObject.FindObjectsOfType<PedestrianNode>();

            for (int rIndex = 0; rIndex < nodes.Length; rIndex++)
            {
                int perNodeCount = 0;
                while (perNodeCount < m_numOfObjectsSpawnedPerNode)
                {
                    float rand = Random.Range(0.0f, 1.0f);
                    if (rand <= m_randomObjectSpawnChancePerNode && CanSpawn())
                    {
                        int randObjectIndex = Random.Range(0, m_objectPrefabs.Count);
                        PedestrianObject obj = Instantiate(m_objectPrefabs[randObjectIndex], transform.position, transform.rotation) as PedestrianObject;
                        obj.Spawn(nodes[rIndex].transform.position, nodes[rIndex]);
                    }

                    perNodeCount++;
                }
            }
        }

    }

    public void SetPedestrianNode(Tooltip a_tooltip, PedestrianNode a_obj, bool a_show = true)
    {
        switch (a_tooltip)
        {
            case Tooltip.ANCHOR:
                {
                    AnchorNode = a_obj;
                    if (a_obj)
                        PreviousAnchorNode = a_obj;
                    if (AnchorNode)
                    {
                        ShowTooltip(Tooltip.ANCHOR, a_show);
                        PositionTooltip(Tooltip.ANCHOR, AnchorNode.gameObject);
                    }
                }
                break;
            case Tooltip.EDIT:
                {
                    EditNode = a_obj;
                    if (a_obj)
                        PreviousEditNode = a_obj;
                    if (EditNode)
                    {
                        ShowTooltip(Tooltip.EDIT, a_show);
                        PositionTooltip(Tooltip.EDIT, EditNode.gameObject);
                    }
                }
                break;
        }
    }


    public void ShowTooltip(Tooltip a_tooltip, bool a_show)
    {
        switch (a_tooltip)
        {
            case Tooltip.ANCHOR:
                {
                    if (TooltipAnchor)
                    {
                        TooltipAnchor.SetActive(a_show);
                    }
                }
                break;
            case Tooltip.EDIT:
                {
                    if (TooltipEdit)
                    {
                        TooltipEdit.SetActive(a_show);
                    }
                }
                break;
        }
    }

    public void PositionTooltip(Tooltip a_tooltip, GameObject a_obj, bool a_show = true)
    {
        switch (a_tooltip)
        {
            case Tooltip.ANCHOR:
                {
                    if (TooltipAnchor)
                    {

                        ShowTooltip(Tooltip.ANCHOR, a_show);
                        TooltipAnchor.transform.position = new Vector3(a_obj.transform.position.x, a_obj.transform.position.y + a_obj.GetComponent<Renderer>().bounds.extents.y + 2.0f, a_obj.transform.position.z);
                    }
                }
                break;
            case Tooltip.EDIT:
                {
                    if (TooltipEdit)
                    {

                        ShowTooltip(Tooltip.EDIT, a_show);
                        TooltipEdit.transform.position = new Vector3(a_obj.transform.position.x, a_obj.transform.position.y + a_obj.GetComponent<Renderer>().bounds.extents.y + 2.4f, a_obj.transform.position.z);
                    }
                }
                break;
        }
    }

    public bool CanSpawn()
    {
        if (m_objectSpawnCountMax <= -1)
            return true;

        if (m_spawnedObjects.Count < m_objectSpawnCountMax)
            return true;

        return false;
    }

    public void LinkNode(bool a_anchorToEdit = true)
    {
        PedestrianNode useEditNode = null;
        PedestrianNode useAnchorNode = null;
        Debug.Log("tutaj");
        if (EditNode)
            useEditNode = EditNode;
        else if (PreviousEditNode)
            useEditNode = PreviousEditNode;

        if (AnchorNode)
            useAnchorNode = AnchorNode;
        else if (PreviousAnchorNode)
            useAnchorNode = PreviousAnchorNode;

        if (m_linkBothDir)
        {
         //   Debug.Log("jestem w m_linkBothDir ");
            if (useAnchorNode)
                useAnchorNode.AddNode(useEditNode);
            if (useEditNode)
                useEditNode.AddNode(useAnchorNode);
        }
        else if (a_anchorToEdit)
        {
         //   Debug.Log("jestem w a_anchorToEdit ");
            if (useAnchorNode)
                useAnchorNode.AddNode(useEditNode);
        }
        else
        {
         //   Debug.Log("jestem w else ");
            if (useEditNode)
                useEditNode.AddNode(useAnchorNode);
        }
    }

    public void AddToCLRevealObjsFrom(Transform a_obj)
    {
        bool foundObj = false;

        if (!foundObj)
            CLRevealObjectsFrom.Add(a_obj);
    }

    public void ClearCLRevealObjsFrom()
    {
        CLRevealObjectsFrom.Clear();
    }

    public void AddToCLRevealObjsTo(Transform a_obj)
    {
        bool foundObj = false;

        if (!foundObj)
            CLRevealObjectsTo.Add(a_obj);
    }

    public void ClearCLRevealObjsTo()
    {
        CLRevealObjectsTo.Clear();
    }

    public void RegisterObject(PedestrianObject a_object)
    {
        m_spawnedObjects.Add(a_object);
    }

    public void UnRegisterObject(PedestrianObject a_object)
    {
        m_spawnedObjects.Remove(a_object);
        RespawnObject();
    }

    public void RegisterObjectSpawner(PedestrianObjectSpawner a_spawner)
    {
        m_objectSpawners.Add(a_spawner);
    }

    public void RespawnObject()
    {
        if (m_objectSpawners.Count <= 0)
            return;

        PedestrianObjectSpawner spawners = m_objectSpawners[Random.Range(0, m_objectSpawners.Count)];
        spawners.RespawnObject();
    }

    void OnDrawGizmos()
    {

        if (CLRevealObjectsFrom.Count > 0 && CLRevealObjectsFrom.Count == CLRevealObjectsTo.Count)
        {
            float scaleFactorCube = 0.25f;
            float scaleFactorSphere = 0.35f;

            for (int rIndex = 0; rIndex < CLRevealObjectsFrom.Count; rIndex++)
            {
                if (CLRevealObjectsFrom[rIndex] == null)
                    continue;

                if (CLRevealObjectsTo[rIndex] == null)
                    continue;

                Vector3 offset = new Vector3(0.0f, 0.225f, 0.0f);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(CLRevealObjectsFrom[rIndex].position + offset, CLRevealObjectsTo[rIndex].position + offset);

                Vector3 dir = CLRevealObjectsFrom[rIndex].position - CLRevealObjectsTo[rIndex].position;
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube((CLRevealObjectsFrom[rIndex].position - (dir.normalized * ((dir.magnitude / 2) + scaleFactorSphere))) + offset, new Vector3(scaleFactorCube, scaleFactorCube, scaleFactorCube));
                Gizmos.color = Color.red;
                Gizmos.DrawSphere((CLRevealObjectsFrom[rIndex].position - (dir.normalized * (dir.magnitude / 2))) + offset, scaleFactorSphere);

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(CLRevealObjectsFrom[rIndex].position + offset, scaleFactorSphere);
                Gizmos.DrawSphere(CLRevealObjectsTo[rIndex].position + offset, scaleFactorSphere);
            }
        }
    }
}
