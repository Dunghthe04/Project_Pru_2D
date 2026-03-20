using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public float spawnInterval = 10f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnArrow();
        }
    }

    void SpawnArrow()
    {
        if (spawnPoints.Count == 0) return;

        Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject arrow = Instantiate(arrowPrefab, point.position, point.rotation);

        Vector2 dir = point.right;

        arrow.GetComponent<ArrowProjectile>().Init(dir);
    }
}