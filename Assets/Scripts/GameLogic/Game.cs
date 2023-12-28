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
    //[SerializeField] TextMeshProUGUI playerStatsTMPRO;
    [SerializeField] TextMeshProUGUI winerNickNameTMPRO;
    [SerializeField] TextMeshProUGUI winerScoreTMPRO;
    [SerializeField] Image  winerImage;
    [SerializeField] GameObject winerUIObject;
    [SerializeField] int roundTime;
    [Header("Leader Board")]
    [SerializeField] TextMeshProUGUI t1text;
    [SerializeField] Image  t1Image;
    [SerializeField] TextMeshProUGUI t2text;
    [SerializeField] Image  t2Image;
    [SerializeField] TextMeshProUGUI t3text;
    [SerializeField] Image  t3Image;
    [SerializeField] TextMeshProUGUI t4text;
    [SerializeField] Image  t4Image;
    [SerializeField] TextMeshProUGUI t5text;
    [SerializeField] Image  t5Image;
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
            //  var stats= playersStats.GetTopFive();
            //  var sb= new StringBuilder();
            //  foreach (var item in stats)
            //  { 
            //      sb.Append($" {item.NickName} ---> {item.Score} \n");

            //  }
            //  playerStatsTMPRO.text=sb.ToString();
            UpdateLeaderBoard();

            yield return new WaitForSeconds(1f);
        }



    }

    private void UpdateLeaderBoard()
    {
        var stats = playersStats.GetTopFive();

        t1Image.sprite = stats[0].UserAvatar;
        t1text.text = $"--> {stats[0].Score}";
        t2Image.sprite = stats[1].UserAvatar;
        t2text.text = $"--> {stats[1].Score}";
        t3Image.sprite = stats[2].UserAvatar;
        t3text.text = $"--> {stats[2].Score}";
        t4Image.sprite = stats[3].UserAvatar;
        t4text.text = $"--> {stats[3].Score}";
        t5Image.sprite = stats[4].UserAvatar;
        t5text.text = $"--> {stats[4].Score}";
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
        winerUIObject.SetActive(true);
        winerImage.sprite=stats[0].UserAvatar;
        winerNickNameTMPRO.text=stats[0].NickName;
        winerScoreTMPRO.text=stats[0].Score.ToString();
         

      playersStats.Reset();
      

      StartCoroutine(BreakTimerCorutine());
      tikTokLiveReader.ballQueue.Clear();
     
   }
   
   private void OnDisable() {
    StopAllCoroutines();
   }
}
