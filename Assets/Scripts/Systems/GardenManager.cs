using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GardenManager : MonoBehaviour
{
    [Header("Spawn")]
    [Header("UI")]
    public TMP_Text resultsText;
    public GameObject[] flowerPrefabs;  // multiple flower prefabs
    public Rect spawnArea = new Rect(-8f, -3f, 16f, 6f); // x, y, width, height

    [Header("UI / Navigation")]
    public string mainMenuScene = "MainMenu";

    void Start()
    {
    int n = Mathf.Max(0, GameManager.I.SeedsCollectedThisLevel);
    if (flowerPrefabs == null || flowerPrefabs.Length == 0) return;

    StartCoroutine(SpawnSequential(n));

    // update TMP line
    if (resultsText)
    {
        int count = GameManager.I.SeedsCollectedThisLevel;
        resultsText.text = $"You collected {count} seed{(count == 1 ? "" : "s")}: Flowers Blooming!";
    }
}

IEnumerator SpawnSequential(int n)
{
    float interval = 0.12f;
    for (int i = 0; i < n; i++)
    {
        Vector2 pos = new Vector2(
            Random.Range(spawnArea.xMin, spawnArea.xMax),
            Random.Range(spawnArea.yMin, spawnArea.yMax)
        );

        GameObject prefab = flowerPrefabs[Random.Range(0, flowerPrefabs.Length)];
        Instantiate(prefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);

        yield return new WaitForSeconds(interval);
    }

    GameManager.I.SeedsCollectedThisLevel = 0;
    GameManager.I.HasWateringCan = false;
}

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}