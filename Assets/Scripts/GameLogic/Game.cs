using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TikTokLiveSharp.Events.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Game : MonoBehaviour
{
   
    [SerializeField] TikTokLiveReader tikTokLiveReader;
    [SerializeField] TextMeshProUGUI timerTMPRO;
    [SerializeField] TextMeshProUGUI playerStatsTMPRO;
    [SerializeField] TextMeshProUGUI winerNickNameTMPRO;
    [SerializeField] TextMeshProUGUI winerScoreTMPRO;
    [SerializeField] Image  winerImage;
    [SerializeField] GameObject winerUIObject;
    [SerializeField] int roundTime;
     public  PlayersStats playersStats;
     private AudioService audioService;
     [Inject]
     private void Inject(AudioService _audioService) {

      audioService=_audioService;
     
     } 
     
     
     
     
     private void Awake() {
           playersStats= new PlayersStats();
     }
private void Start() {
       StartRound();
   }

    IEnumerator RoundTimerCorutine() {

       int timertime=roundTime;
       int minutes;
       int remainingSeconds;
       while (timertime>0)
       {
          minutes = timertime / 60;
          remainingSeconds = timertime % 60;

        // Use string interpolation to format the output
      
          timerTMPRO.text=$"TIME LEFT - {minutes}:{remainingSeconds:D2}";
          timertime--;
         yield return new WaitForSecondsRealtime(1);

       }

       StopRound();
     
    
    } 
    IEnumerator BreakTimerCorutine() {

       int timertime=5;
       int minutes;
       int remainingSeconds;
       while (timertime>0)
       {
          minutes = timertime / 60;
          remainingSeconds = timertime % 60;

        // Use string interpolation to format the output
      
          timerTMPRO.text=$"NEW RAUND AFTER {minutes}:{remainingSeconds:D2}";
          timertime--;
         yield return new WaitForSecondsRealtime(1);

       }
       timerTMPRO.text="GO!!!!";
         

       StartRound();
      
    } 
    
   IEnumerator EnqueueBallsCorutine() {
    while(true)
    {   
        if(tikTokLiveReader.ballQueue.Count>0)
        {
          tikTokLiveReader.ballQueue.Dequeue().gameObject.SetActive(true);
        }
       
      yield return new WaitForEndOfFrame();
    }
   

   } 
   IEnumerator UpdatePlayerStatsCorutine() {
       while(true)
       {
           var stats= playersStats.GetTopFive();
           var sb= new StringBuilder();
           foreach (var item in stats)
           { 
               sb.Append($" {item.NickName} ---> {item.Score} \n");
            
           }
           playerStatsTMPRO.text=sb.ToString();
         yield return new WaitForSeconds(1f);  
       }
        


   } 
   

   public void StartRound()
   { 
     StopAllCoroutines();
     EventBuss.OnRaundStart?.Invoke();
     winerUIObject.SetActive(false);
     StartCoroutine(EnqueueBallsCorutine());
     StartCoroutine (RoundTimerCorutine ());
     StartCoroutine(UpdatePlayerStatsCorutine());


   }
   public void StopRound()
   {
      StopAllCoroutines();
      EventBuss.OnRaundOver?.Invoke();
      audioService.PlayWinFx();
      var stats= playersStats.GetTopFive();
           var sb= new StringBuilder();
           foreach (var item in stats)
           { 
               sb.Append($" {item.NickName} ---> {item.Score} \n");
            
           }
        winerUIObject.SetActive(true);
        winerImage.sprite=stats[0].UserAvatar;
        winerNickNameTMPRO.text=stats[0].NickName;
        winerScoreTMPRO.text=stats[0].Score.ToString();
         

      playersStats.Reset();
      

      StartCoroutine(BreakTimerCorutine());
     
   }
   
   private void OnDisable() {
    StopAllCoroutines();
   }
}
