using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GardenManager : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject[] flowerPrefabs;  // multiple flower prefabs
    public Rect spawnArea = new Rect(-8f, -3f, 16f, 6f); // x, y, width, height

    [Header("UI / Navigation")]
    public string mainMenuScene = "MainMenu";

    void Start()
    {
        int n = Mathf.Max(0, GameManager.I.SeedsCollectedThisLevel);

        for (int i = 0; i < n; i++)
        {
            float x = Random.Range(spawnArea.xMin, spawnArea.xMax);
            float y = Random.Range(spawnArea.yMin, spawnArea.yMax);

            // pick a random flower from the array
            GameObject randomFlower = flowerPrefabs[Random.Range(0, flowerPrefabs.Length)];

            Instantiate(randomFlower, new Vector3(x, y, 0f), Quaternion.identity);
        }

        // reset for next level
        GameManager.I.SeedsCollectedThisLevel = 0;
        GameManager.I.HasWateringCan = false;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
