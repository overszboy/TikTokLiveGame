using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    private void Awake() {
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        Application.targetFrameRate=-1;
#else
        Debug.unityLogger.logEnabled = false;
         Application.targetFrameRate=-1;;
#endif
    }
}
