using System.Collections;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;
using TikTokLiveSharp.Events.Objects;
using TikTokLiveUnity.Utils;
using TikTokLiveUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using Zenject;
using Unity.VisualScripting;
using System;

public class TikTokLiveReader: MonoBehaviour
    {
        #region Properties
        [Header("Settings")]
        [SerializeField]
        [Tooltip("Duration for objects to exist")]
        private float timeToLive = 3f;
        [Header("UIRootPanel")]
        [SerializeField]
        [Tooltip("ReaderUI")]
        private GameObject panelGameObject;
        [SerializeField]
        private Color disconectedColor;
        [SerializeField]
        private Color connectedColor;
        private Image panelObjetImage;
        private Vector2 touchStartPosition;
      
        /// <summary>
        /// ScrollRect for Join-Texts
        /// </summary>
      
         [Header("ScrollRects")]

          /// <summary>
        ///  Fabric 
        /// </summary>
         private BallFabric ballFabric;
         public Queue<Ball> ballQueue = new Queue<Ball>();
        
        /// <summary>
        /// Title for Status-Panel
        /// </summary>
        [Header("StatusPanel")]
        [SerializeField]
        [Tooltip("Title for Status-Panel")]
        private TMP_Text txtStatusTitle;
        /// <summary>
        /// Text displaying Host connected to
        /// </summary>
        [SerializeField]
        [Tooltip("Text displaying Host connected to")]
        private TMP_Text txtStatusHostId;
        /// <summary>
        /// InputField for Host to connect to
        /// </summary>
        [SerializeField]
        [Tooltip("InputField for Host to connect to")]
        private TMP_InputField ifHostId;
        /// <summary>
        /// InputField for Host to connect to
        /// </summary>
        [SerializeField]
        [Tooltip("InputField for Host to connect to")]
        private TMP_InputField ifRoomId;
        /// <summary>
        /// Connect-Button
        /// </summary>
        [SerializeField]
        [Tooltip("Connect-Button")]
        private Button btnConnect;
      
       

        /// <summary>
        /// ShortHand for TikTokLiveManager-Access
        /// </summary>
        private TikTokLiveManager mgr => TikTokLiveManager.Instance;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Initializes this Object
        /// </summary>
        /// 
     [Inject ]
     private void Inject( BallFabric _ballFabric) {
          ballFabric=_ballFabric;
        } 
        
        private void Awake() {
            panelObjetImage=panelGameObject.gameObject.GetComponent<Image>();
        
        }
        private IEnumerator Start()
        {
            btnConnect.onClick.AddListener(OnClick_Connect);
            mgr.OnConnected += ConnectStatusChange;
            mgr.OnDisconnected += ConnectStatusChange;
            mgr.OnSocialMessage+=OnSocialMassage;
             mgr.OnJoin+=OnJoin;
            mgr.OnLike += OnLike;
            mgr.OnChatMessage += OnComment;
            mgr.OnGift += OnGift;
            for (int i = 0; i < 3; i++)
                yield return null; // Wait 3 frames in case Auto-Connect is enabled
            UpdateStatus();
        }

    [System.Obsolete]
    private void Update() {
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                panelGameObject.SetActive(!panelGameObject.active);
            }
             DetectSwipe();
        }
     private void DetectSwipe()
    {
       
        if(Input.touchCount>0)
        {
           var  touch=Input.GetTouch(0);
          
           if(touch.phase==TouchPhase.Began)
           {
            touchStartPosition=touch.position;
           }

           if(touch.phase==TouchPhase.Ended)
           {
               float swipeDistance = touch.position.x - touchStartPosition.x;

                        if (Mathf.Abs(swipeDistance) >= 100)
                        {
                           
                            if (swipeDistance > 0 )
                            { 
                                panelGameObject.SetActive(true);
                                                         
                            }
                            else
                            {
                                
                                  panelGameObject.SetActive(false);
                            }
                        }

           } 

        }
    }
        /// <summary>
        /// Deinitializes this Object
        /// </summary>
        private void OnDestroy()
        {
            btnConnect.onClick.RemoveListener(OnClick_Connect);
            if (!TikTokLiveManager.Exists)
                return;
            mgr.OnConnected -= ConnectStatusChange;
            mgr.OnSocialMessage-=OnSocialMassage;
            mgr.OnDisconnected -= ConnectStatusChange;
            mgr.OnJoin-=OnJoin;
            mgr.OnLike -= OnLike;
            mgr.OnChatMessage -= OnComment;
            mgr.OnGift -= OnGift;
            
        }
        #endregion

        #region Private
        /// <summary>
        /// Handler for Connect-Button
        /// </summary>
        private void OnClick_Connect()
        {
            bool connected = mgr.Connected;
            bool connecting = mgr.Connecting;
            if (connected || connecting)
                mgr.DisconnectFromLivestreamAsync();
            else
            {
                if (!string.IsNullOrEmpty(ifRoomId.text))
                    mgr.ConnectToRoomAsync(ifRoomId.text, Debug.LogException);
                else
                    mgr.ConnectToStreamAsync(ifHostId.text, Debug.LogException);
            }
            UpdateStatus();
            Invoke(nameof(UpdateStatus), .5f);
        }
        /// <summary>
        /// Handler for Connection-Events. Updates StatusPanel
        /// </summary>
        private void ConnectStatusChange(TikTokLiveClient sender, bool e) => UpdateStatus();
        /// <summary>
        /// Handler for Gift-Event
        /// </summary>
        private void OnGift(TikTokLiveClient sender, TikTokGift gift)
        {
           // gift.Gift.
            gift.OnStreakFinished+=StreakFinished;
            
        }
         private void StreakFinished(TikTokGift gift, long finalAmount)
        {

            Debug.Log(gift.Sender.NickName +" send  -> "+gift.Gift.Name +"--"+ finalAmount);
           for(int i=1;i<=finalAmount;i++)
           {
           
            CreateFiveBalls(gift,finalAmount);
            Debug.Log("work1");
           }
            
        }

        private void CreateFiveBalls(TikTokGift gift, long finalAmount)
        {

             for(int i=0;i<5;i++)
           {
            var ball= ballFabric.CreateBall();
            ball.name=gift.Sender.NickName;
            ball.NickName=gift.Sender.NickName;
            RequestBallSprite( ball,gift.Sender.AvatarThumbnail);
           Debug.Log("work2");

           }
        }
          /// <summary>
        /// Handler for Join-Event
        /// </summary>
           private void OnJoin(TikTokLiveClient sender, Join join)
        {
            var ball= ballFabric.CreateBall();
            ball.name=join.User.NickName;
            ball.NickName=join.User.NickName;
            RequestBallSprite( ball,join.User.AvatarThumbnail);
           
        }
       
        /// <summary>
        /// Handler for Like-Event
        /// </summary>
        private void OnLike(TikTokLiveClient sender, Like like)
        {

             for(int i=1;i<=like.Count;i++)
           {
           var ball= ballFabric.CreateBall();
            ball.name=like.Sender.NickName;
            ball.NickName=like.Sender.NickName;
            RequestBallSprite( ball,like.Sender.AvatarThumbnail);
           }
            
            
            
        }
         /// <summary>
        /// Handler for SocialMasage
        /// </summary>
        private void OnSocialMassage(TikTokLiveClient sender,SocialMessage val)
        {
            if(val.ShareCount!=0)
            {
               Debug.Log($"{val.Sender.NickName} share Translation {val.ShareCount} times ");
           
            for(int i=1;i<=2;i++)
            {
                 var ball= ballFabric.CreateBall();
            ball.name=val.Sender.NickName;
            ball.NickName=val.Sender.NickName;
            RequestBallSprite( ball,val.Sender.AvatarThumbnail);
            }
               
            }
            if(val.FollowCount!=0)
            {
               Debug.Log($"{val.Sender.NickName} follow  the streamer ");
              for(int i=1;i<=2;i++)
            { 
               var ball= ballFabric.CreateBall();
               ball.name=val.Sender.NickName;
               ball.NickName=val.Sender.NickName;
               RequestBallSprite( ball,val.Sender.AvatarThumbnail);
            }

            }
            
        }
        /// <summary>
        /// Handler for Comment-Event
        /// </summary>
        private void OnComment(TikTokLiveClient sender, Chat comment)
        {
            for(int i=1;i<=2;i++)
            {
            var ball= ballFabric.CreateBall();
            ball.name=comment.Sender.NickName;
            ball.NickName=comment.Sender.NickName;
            RequestBallSprite( ball,comment.Sender.AvatarThumbnail);
            }
            
           
        }
        /// <summary>
        /// Requests Image from TikTokLive-Manager
        /// </summary>
        /// <param name="img">UI-Image used for display</param>
        /// <param name="picture">Data for Image</param>
        private void RequestImage(Image img, Picture picture)
        {
            Dispatcher.RunOnMainThread(() =>
            {
                mgr.RequestSprite(picture, spr =>
                {
                    if (img != null && img.gameObject != null && img.gameObject.activeInHierarchy)
                        img.sprite = spr;
                });
            });
        }
         /// <summary>
        /// Requests Image from TikTokLive-Manager
        /// </summary>
        /// <param name="ball">SpriteRender used for display</param>
        /// <param name="picture">Data for Image</param>
        private void RequestBallSprite(Ball ball, Picture picture)
        {
            
            // Dispatcher.RunOnMainThread(() =>
            // {
                mgr.RequestSprite(picture, spr =>
                {
                    if (ball != null && ball.gameObject != null )
                      {
                         
                         ball.userAvatar.sprite = spr;
                         
                         ballQueue.Enqueue(ball);
                      

                      }  
                });
            // });
        }
        /// <summary>
        /// Updates Status-Panel based on ConnectionState
        /// </summary>
        private void UpdateStatus()
        {    
            if (mgr.Connected)
            {
                panelObjetImage.color=connectedColor;

            }
            else
            {
                panelObjetImage.color=disconectedColor;
            }
            bool connected = mgr.Connected;
            bool connecting = mgr.Connecting;
            txtStatusTitle.text = connected ? "Connected to:" : connecting ? "Connecting to:" : "Connect to:";
            txtStatusHostId.gameObject.SetActive(connecting || connected);
            if (connected || connecting)
                txtStatusHostId.text = string.IsNullOrWhiteSpace(mgr.HostName) ? mgr.RoomId : mgr.HostName;
            ifHostId.gameObject.SetActive(!connected && !connecting);
            ifRoomId.gameObject.SetActive(!connected && !connecting);
            btnConnect.GetComponentInChildren<TMP_Text>().text = connected ? "Disconnect" : connecting ? "Cancel" : "Connect";
        }
        
        #endregion
        #endregion
    }
