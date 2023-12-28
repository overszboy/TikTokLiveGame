using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{    
     [SerializeField] private Sprite defSprite;
    public SpriteRenderer userAvatar;
    private Rigidbody2D rb;
    private Vector3 rnd= new Vector3 (0f,0f,0f);
    private Vector2  dmSize=new Vector2 (0.12f,0.12f);
   [HideInInspector] public Collider2D coll;
    [HideInInspector]public string NickName;
  
    // Start is called before the first frame update
    private void Awake() {
        rb=GetComponent<Rigidbody2D>();
        coll=GetComponent<Collider2D>();
        
        EventBuss.OnRaundOver.AddListener(OnRoundOver);
         
    }
    

    private void OnEnable() {
        rnd.x=((float)Random.Range(-200,200))/1000;
        coll.enabled=true;
        userAvatar.drawMode=SpriteDrawMode.Sliced;
        userAvatar.size=dmSize;
        gameObject.transform.position=gameObject.transform.position +rnd;
       
    }
    
    private void OnDisable() {
    
    }

   private void  OnRoundOver ()
   {
     
      Destroy(this.gameObject);
   }
   

  private void OnDestroy() {
     EventBuss.OnRaundOver.RemoveListener(OnRoundOver);
  }
}
