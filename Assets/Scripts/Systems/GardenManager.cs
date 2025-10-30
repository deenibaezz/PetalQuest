using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GardenManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text resultsText;

    [Header("Spawn")]
    public GameObject[] flowerPrefabs;  // multiple flower prefabs
    public Rect spawnArea = new Rect(-8f, -3f, 16f, 6f); // x, y, width, height

    [Header("UI / Navigation")]
    public string mainMenuScene = "MainMenu";

    [Header("SFX")]
    public AudioClip sfxGardenTada;
    [Range(0f,1f)] public float volTada = 0.85f;
    public AudioClip sfxFlowerPop;
    [Range(0f,1f)] public float volPop = 0.35f;

    void Start()
    {
        int n = Mathf.Max(0, GameManager.I.SeedsCollectedThisLevel);

        // update headline
        if (resultsText){
            resultsText.text = $"You collected {n} seed{(n == 1 ? "" : "s")}: Flowers Blooming!";
        }

        // play arrival sting
        Sfx2D.Play(sfxGardenTada, volTada);

        if (flowerPrefabs == null || flowerPrefabs.Length == 0) return;
        StartCoroutine(SpawnSequential(n));
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

            Sfx2D.Play(sfxFlowerPop, volPop); // pop per flower

            yield return new WaitForSeconds(interval);
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
