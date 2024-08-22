using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;
    public int scoreUntilBouncerSpawn;
    public float scoreNeededMultiplier = 1.5f;
    public float scoreNeededDeteriationMin;
    public float scoreNeededDeteriationMax;
    public float scoreNeededMultiplierMin;
    public int maxBouncers = 7;
    [Header("Spawn Ranges")]
    public float xMinSpawnRange;
    public float xMaxSpawnRange, ySpawnMinRange, ySpawnMaxRange, zSpawnMinRange, zSpawnMaxRange;
    [Header("Bouncer Spawning")]
    public GameObject bouncer;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= scoreUntilBouncerSpawn)
        {
            scoreUntilBouncerSpawn = (int)Mathf.Ceil(scoreUntilBouncerSpawn * scoreNeededMultiplier);
            scoreNeededMultiplier -= Random.Range(scoreNeededDeteriationMin, scoreNeededDeteriationMax);
            scoreNeededMultiplier = Mathf.Clamp(scoreNeededMultiplier, scoreNeededMultiplierMin, scoreNeededMultiplier);
            SpawnNewBouncer();
        }
    }

    public void SpawnNewBouncer()
    {
        if (FindObjectsOfType<Bouncer>().Length >= maxBouncers)
        {
            return;
        }
        GameObject spawnedBouncer = Instantiate(bouncer);
        spawnedBouncer.transform.position = new Vector3(Random.Range(xMinSpawnRange, xMaxSpawnRange), Random.Range(ySpawnMinRange, ySpawnMaxRange), Random.Range(zSpawnMinRange, zSpawnMaxRange));
    }
}
