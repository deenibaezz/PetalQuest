using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    void Start(){
        GameManager.I.SeedsCollectedThisLevel = 0;
        GameManager.I.HasWateringCan = false;
    }
}
