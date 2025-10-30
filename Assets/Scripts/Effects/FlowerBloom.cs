using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBloom : MonoBehaviour
{
    public float growTime = 0.5f;
    public float finalScale = 1f;
    public float randomStartDelayMax = 0.4f;

    SpriteRenderer sr;
    Vector3 targetScale;
    float timer;
    float delay;

    void Awake()
    {
        // get a renderer on this object or any child
        sr = GetComponent<SpriteRenderer>();
        if (!sr) sr = GetComponentInChildren<SpriteRenderer>();

        targetScale = transform.localScale;

        transform.localScale = Vector3.zero;
        if (sr){
            var c = sr.color; c.a = 0f; sr.color = c;
        }

        delay = Random.Range(0f, randomStartDelayMax);
    }

    void Update()
    {
        if (delay > 0f){ delay -= Time.deltaTime; return; }

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / growTime);

        transform.localScale = Vector3.Lerp(Vector3.zero, targetScale * finalScale, t);

        if (sr){
            Color c = sr.color; c.a = Mathf.Lerp(0f, 1f, t); sr.color = c;
        }
    }
}