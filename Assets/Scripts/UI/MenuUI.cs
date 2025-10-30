using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    [SerializeField] string dayScene = "Level_Day";
    [SerializeField] string nightScene = "Level_Night";

    public void PlayDay()  { Time.timeScale = 1f; SceneManager.LoadScene(dayScene); }
    public void PlayNight(){ Time.timeScale = 1f; SceneManager.LoadScene(nightScene); }
       
}