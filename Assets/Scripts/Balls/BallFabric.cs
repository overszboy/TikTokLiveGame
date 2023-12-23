using System.Collections.Generic;
using UnityEngine;

public class BallFabric : MonoBehaviour {


   
    [SerializeField] private GameObject ballPrefub;
   
   
   private Stack<GameObject> ballsStack;

    private void Awake() {
         ballsStack= new();
         ballPrefub.SetActive(false);
       EventBuss.OnRaundOver.AddListener(ClearStack);
    }

    public Ball CreateBall ()
    {

        if(ballsStack.Count>0)
        {
           var ballObj= ballsStack.Pop();
           ballObj.transform.position=this.gameObject.transform.position;
           ballObj.transform.rotation= Quaternion.identity;

            return ballObj.GetComponent<Ball>();
        }
      
        else
        {
            var obj = Instantiate (ballPrefub,this.gameObject.transform.position,ballPrefub.transform.rotation);
           
            return obj.GetComponent<Ball>();

        }

    }
    public void AddToStack(Ball ball)
    {
        ball.gameObject.SetActive(false);
        ballsStack.Push(ball.gameObject);
    }

  public void ClearStack()
  {
     ballsStack.Clear();
  }
}