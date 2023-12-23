using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventBuss 
{
     public static UnityEvent OnRaundOver = new UnityEvent();
      public static UnityEvent OnRaundStart = new UnityEvent();
}
