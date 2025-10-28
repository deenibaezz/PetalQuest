using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedHUD : MonoBehaviour
{
    public TMP_Text seedText;
    void Update(){
        if (seedText) seedText.text = $"Seeds: {GameManager.I.SeedsCollectedThisLevel}";
    }
}