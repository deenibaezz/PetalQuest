using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsHUD : MonoBehaviour
{
    public PlayerHealth player;
    public Image[] hearts; // size 3

    void Update(){
        if (!player) return;
        for (int i = 0; i < hearts.Length; i++){
            bool filled = i < player.currentHearts;
            hearts[i].enabled = filled; // hide empty hearts
        }
    }
}