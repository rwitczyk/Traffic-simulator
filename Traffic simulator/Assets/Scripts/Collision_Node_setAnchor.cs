using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collision_Node_setAnchor : MonoBehaviour
{
    string nazwa_obiektu = "";

    void OnTriggerEnter(Collider collison)
    {
        /*
        nazwa_obiektu = collison.gameObject.name;
        // Debug.Log(nazwa_obiektu);
        //PedestrianSystem.LinkNode(true);
            if (collison.gameObject.name.Substring(0, 14) == "PedestrianNode") //dziala
            {
                Debug.Log(collison.gameObject.name);

                //jezeli ma byc kotwica
                PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, gameObject.GetComponent(typeof(PedestrianNode)) as PedestrianNode);


                //jezeli ma byc klucz
                //PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, PedestrianNode);
            }
            */
    }
}
