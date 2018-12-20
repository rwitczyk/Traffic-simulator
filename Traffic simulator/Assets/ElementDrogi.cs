using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementDrogi {

    public GameObject prefab;
    public string name;
    public Vector3 position;
    public int width;
    public int height;

    public ElementDrogi(GameObject prefab, string name, Vector3 position)
    {
        this.prefab = prefab;
        this.name = name;
        this.position = position;
    }




    // dodać pola opisujące sąsiadów
    /* private ElementDrogi elementUp;
     private ElementDrogi elementDown;
     private ElementDrogi elementLeft;
     private ElementDrogi elementRight;
     */

    //public int
    /* public void SetPosition(Vector3 position)
     {
         this.position = position;
     }

     public Vector3 GetPosition()
     {
         return this.position;
     }*/


}

 