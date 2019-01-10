using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Collision_Node_setEdit : MonoBehaviour
{
    string nazwa_obiektu = "";
<<<<<<< HEAD
    bool isEnabled = false;

=======
>>>>>>> e828daf7e964ac79fb05d0a387a773f247e3e2b3

    //IEnumerator
    void OnTriggerEnter(Collider collison)
    {

        nazwa_obiektu = collison.gameObject.name;
<<<<<<< HEAD
        // Debug.Log(nazwa_obiektu);
        if(isEnabled){
            if (collison.gameObject.name.Substring(0, 14) == "PedestrianNode") //dziala
            {
                Debug.Log(collison.gameObject.name);
                Debug.Log("Jestem w setEdit");

                //jezeli ma byc kotwica
                //PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, PedestrianNode);

                //jezeli ma byc klucz
                PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, gameObject.GetComponent(typeof(PedestrianNode)) as PedestrianNode);

                //yield return new WaitForSeconds(0.1f);

                PedestrianSystem.Instance.LinkNode();
            }
        }



    }

}

=======
       // Debug.Log(nazwa_obiektu);

        if (collison.gameObject.name.Substring(0, 14) == "PedestrianNode") //dziala
        {
            Debug.Log(collison.gameObject.name);
            Debug.Log("Jestem w setEdit");

            //jezeli ma byc kotwica
            //PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.ANCHOR, PedestrianNode);

            //jezeli ma byc klucz
            PedestrianSystem.Instance.SetPedestrianNode(PedestrianSystem.Tooltip.EDIT, gameObject.GetComponent(typeof(PedestrianNode)) as PedestrianNode);

            //yield return new WaitForSeconds(0.1f);

            PedestrianSystem.Instance.LinkNode();
        }

     
    }

}
>>>>>>> e828daf7e964ac79fb05d0a387a773f247e3e2b3
