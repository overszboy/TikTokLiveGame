using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersStats 
{
    
    private Dictionary<string,int> playersStatsBoard = new();
    private Dictionary<string,Sprite> playersAvatarsDict= new();

    
    public void UpdatePlayerScore(Ball ball,int val)
    {
            
            if(playersStatsBoard.ContainsKey(ball.NickName))
            {

                playersStatsBoard[ball.NickName]+=val;
            }
            else
            {
                playersStatsBoard.Add(ball.NickName,val);
                playersAvatarsDict.Add(ball.NickName,ball.userAvatar.sprite);
            }
    }

   public List< Player> GetTopFive()
   {

       if( playersStatsBoard.Count<5)
       {
        for (int i=1; i <6;i++)
        {
            playersStatsBoard.Add(i.ToString(),0);
             Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.clear);
             texture.Apply(); 
             var sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            playersAvatarsDict.Add(i.ToString(),sprite);
        }
         
         
       }
       var result= new List<Player>();
    var top5Keys = playersStatsBoard
            .OrderByDescending(kv => kv.Value)
            .Take(5)
            .Select(kv => kv.Key)
            .ToList();

        
        foreach (var key in top5Keys)
        {
            result.Add(new Player(key,playersStatsBoard[key],playersAvatarsDict[key]));
        }

       return result;
   }

   public void Reset()
   {
    playersAvatarsDict.Clear();
    playersStatsBoard.Clear();

   }
   
}
