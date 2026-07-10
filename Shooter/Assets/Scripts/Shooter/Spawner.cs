using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    void Start()
    {
        StartCoroutine(SpawnOnTimer());
    }


    IEnumerator SpawnOnTimer()
    {
        yield return new WaitForSeconds(5f);
        Vector3 spawnRange = new Vector3(Random.Range(-15, 15), 0, Random.Range(-5, 5));
        Instantiate(enemyPrefab, spawnRange, Quaternion.identity);
        StartCoroutine(SpawnOnTimer());
        Debug.Log("Spawn End");
    }
}
