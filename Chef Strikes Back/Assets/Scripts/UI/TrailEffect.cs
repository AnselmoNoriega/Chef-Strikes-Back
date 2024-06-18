using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailEffect : MonoBehaviour
{
    public List<Sprite> trailSprites;
    public GameObject spritePrefab;
    public float spawnInterval = 0.1f;
    public float fadeDuration = 1.0f;

    private List<GameObject> pooledSprites = new List<GameObject>();
    private float timer = 0f;
    private bool isActive = false;  // To track the active state of trail effects

    public GameObject spawnLocationController;

    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                timer = 0f;
                SpawnTrailSprite();
            }
        }
    }

    public void StartTrail()
    {
        isActive = true;
        Debug.Log("Trail effect activated.");
    }

    public void StopTrail()
    {
        isActive = false;
        // Initiate a fade-out for each active sprite instead of deactivating them immediately
        foreach (var sprite in pooledSprites)
        {
            if (sprite.activeInHierarchy)
            {
                StartCoroutine(FadeOutSprite(sprite));
            }
        }
    }

    private GameObject GetPooledSprite()
    {
        foreach (var obj in pooledSprites)
        {
            if (!obj.activeInHierarchy)
            {
                obj.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); // Reset color to fully opaque
                return obj;
            }
        }

        GameObject newObj = Instantiate(spritePrefab, transform.position, Quaternion.identity);
        newObj.SetActive(false);  // Start inactive, activate when needed
        pooledSprites.Add(newObj);
        return newObj;
    }

    private IEnumerator FadeOutSprite(GameObject spriteObj)
    {
        SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();
        float fadeOutDuration = 1.0f; // Duration for the fade-out effect, adjust as needed
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(sr.color.a, 0f, t));
            yield return null;
        }

        spriteObj.SetActive(false);
    }

    private void SpawnTrailSprite()
    {
        if (!isActive)
            return; // Prevent spawning new sprites if the trail is stopped

        GameObject spriteObj = GetPooledSprite();
        spriteObj.transform.position = spawnLocationController ? spawnLocationController.transform.position : transform.position;
        Sprite selectedSprite = trailSprites[Random.Range(0, trailSprites.Count)];
        SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.color = new Color(1f, 1f, 1f, 1f); // Fully visible when spawned
        spriteObj.SetActive(true);
        StartCoroutine(FadeSprite(spriteObj)); // Start fading immediately upon spawn
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }

    private IEnumerator FadeSprite(GameObject spriteObj)
    {
        SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();
        float initialFadeDuration = 0.5f; // Shorter duration for initial fade if necessary
        float elapsed = 0f;

        while (elapsed < initialFadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / initialFadeDuration;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        // Do not deactivate here if `StopTrail` is handling final fade outs
        if (isActive)
        {
            spriteObj.SetActive(false);
        }
    }
}
