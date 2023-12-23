using UnityEngine;
public class Player {

    public Sprite UserAvatar;
    public string NickName;
    public int Score;
    public Player (string _nickName ,int _score ,Sprite _userAvatar)
    {
         NickName=_nickName;
         Score=_score;
         UserAvatar=_userAvatar;

    }
    
}