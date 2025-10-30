using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBloom : MonoBehaviour
{
    public float growTime = 0.5f; // how long the animation lasts
    public float finalScale = 1f; // 100% size
    float delay;

    SpriteRenderer sr;
    Vector3 targetScale;
    float timer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    targetScale = transform.localScale;

    transform.localScale = Vector3.zero;
    if (sr) sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);

    // small random delay so they don't all start at once
    delay = Random.Range(0f, 0.4f);
}

    void Update()
    {
        if (delay > 0f) { delay -= Time.deltaTime; return; }

    timer += Time.deltaTime;
    float t = Mathf.Clamp01(timer / growTime);

    transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);

    if (sr)
    {
        Color c = sr.color;
        c.a = Mathf.Lerp(0f, 1f, t);
        sr.color = c;
    }
}
}