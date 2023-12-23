using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class WinSlot : MonoBehaviour
{

    private BallFabric ballFabric;
    [SerializeField] private int winCoefficient;
    private Game game;
    private AudioService audioService;
    private Animator animator;
    [Inject]
    private void Inject(Game _game , BallFabric _ballFabric, AudioService _audioService) {
    
    game =_game;
    ballFabric=_ballFabric;
    audioService =_audioService;
    
    } 
    private void Awake() {
        animator= GetComponent<Animator>();
    }
    
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) {

        
       other.gameObject.TryGetComponent<Ball>(out Ball ball);
       if(ball!=null)
       {
        ball.coll.enabled=false;
       game.playersStats.UpdatePlayerScore(ball ,winCoefficient);
       ballFabric.AddToStack(ball);
       audioService.PlaySlotFx();
       animator.SetTrigger("WIN");
       }
  
   }
}
