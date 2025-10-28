using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public int SeedsCollectedThisLevel;
    public int MinSeedsToWin = 25;

    void Awake(){
        if (I != null && I != this){ Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }
}