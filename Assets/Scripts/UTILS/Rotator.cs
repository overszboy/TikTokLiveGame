using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rotator : MonoBehaviour
{
      public int speed ;
      public bool toRight;
    void Update()
    {   
        if(toRight)
        {   
           transform.Rotate(Vector3.back* speed *Time.deltaTime); 
        }
       
        else
        {
           transform.Rotate(Vector3.forward* speed *Time.deltaTime);
        }
      
    }
}
