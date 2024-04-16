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
    }

    public void StopTrail()
    {
        isActive = false;
        // Optionally reset all active trail sprites
        foreach (var sprite in pooledSprites)
        {
            sprite.SetActive(false);
        }
    }

    private GameObject GetPooledSprite()
    {
        foreach (var obj in pooledSprites)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        // Instantiating a new object if none are available in the pool
        GameObject newObj = Instantiate(spritePrefab, transform.position, Quaternion.identity);
        newObj.SetActive(false); // Start inactive, activate when needed
        pooledSprites.Add(newObj);
        return newObj;
    }

    private IEnumerator FadeSprite(GameObject spriteObj)
    {
        SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();
        Color initialColor = sr.color;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        spriteObj.SetActive(false);
    }

    private void SpawnTrailSprite()
    {
        GameObject spriteObj = GetPooledSprite();
        spriteObj.transform.position = spawnLocationController ? spawnLocationController.transform.position : transform.position;
        Sprite selectedSprite = trailSprites[Random.Range(0, trailSprites.Count)];
        SpriteRenderer sr = spriteObj.GetComponent<SpriteRenderer>();
        sr.sprite = selectedSprite;
        sr.sortingLayerName = "Foreground"; // Ensure this is a visible layer
        sr.color = new Color(1f, 1f, 1f, 1f); // Ensure full visibility
        spriteObj.SetActive(true);
        StartCoroutine(FadeSprite(spriteObj));
    }
}
