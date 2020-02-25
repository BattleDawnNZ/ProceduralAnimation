using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeGen : MonoBehaviour {

    public Vector2Int mapSize;
    public float spikeSpacing = 1;
    public GameObject prefab;
    public Vector2 perlinScale = Vector2.one;
    public Vector2 perlinOffset = Vector2.one;

    void Start () {
        for (float y = 0; y < mapSize.y; y += 1f) {
            for (float x = 0; x < mapSize.x; x += 1f) {
                float height = Mathf.PerlinNoise (x * spikeSpacing * perlinScale.x + perlinOffset.x, y * spikeSpacing * perlinScale.y + perlinOffset.y) - 1;
                if (Random.Range (0f, 1f) < 0.1f) {
                    height = -Random.Range (0, 2);
                }
                GameObject go = Instantiate (prefab, new Vector3 (x * spikeSpacing, height, y * spikeSpacing), Quaternion.identity);
            }
        }
    }
}
