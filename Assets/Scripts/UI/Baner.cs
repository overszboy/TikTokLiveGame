using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Baner : MonoBehaviour
{    
     [SerializeField] private int inteval=1;
     [TextArea]
      public List<String> infoList;
      private TextMeshProUGUI text;
  
   
        private void Awake() {
            text=GetComponent<TextMeshProUGUI>();
            StartCoroutine(ShowInfoCorutine());
            
        }
   

 IEnumerator ShowInfoCorutine() {

    while(true)
    {
        foreach(var info in infoList)
        {  
            text.text=info;
           yield return new WaitForSecondsRealtime(inteval);
        }
        
      
    }
 
 } 
 
}
