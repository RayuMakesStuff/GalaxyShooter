using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Position Data")]
    private Transform _leftEnemyBorder;
    private Transform _rightEnemyBorder;
    private Transform _topEnemyBorder;

    [Header("Game Objects")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _tripleShotPowerUpPrefab;
    private Player _player;

    [Header("Debug Booleans")]
    private bool _stopSpawning;

    [Header("Enemy Spawn Timer")]
    [SerializeField][Range(1, 5)] private int _minimumTimeToWait = 2;
    [SerializeField][Range(1, 8)] private int _maximumTimeToWait = 6;

    [Header("Power Ups")]
    [SerializeField] private GameObject[] _powerUps;

    // ========================================================================================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
        CheckSpawnCooldownTime();
    }

    private void FindGameObjects()
    {
        _leftEnemyBorder = GameObject.Find("EnemyBorder_left").transform;
        _rightEnemyBorder = GameObject.Find("EnemyBorder_right").transform;
        _topEnemyBorder = GameObject.Find("EnemySpawnPosition").transform;
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void CheckSpawnCooldownTime()
    {
        if (_maximumTimeToWait < _minimumTimeToWait)
        {
            _maximumTimeToWait = _minimumTimeToWait + 1;
            Debug.LogWarning("Time values changed due to an invalid configured timer!");
        }
    }

    private void NullChecking()
    {
        if (_leftEnemyBorder == null)
            Debug.LogError("'_leftEnemyBorder' is NULL! Have you named your GameObject 'LeftEnemyBorder'?");

        if (_rightEnemyBorder == null)
            Debug.LogError("'_rightEnemyBorder' is NULL! Have you named your GameObject 'RightEnemyBorder'?");

        if (_topEnemyBorder == null)
            Debug.LogError("'_topEnemyBorder' is NULL! Have you named your GameObject 'TopEnemyBorder'?");

        if (_player == null)
            Debug.LogError("'_player' is NULL! Have you named your GameObject 'Player'?");
        
    }

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(_leftEnemyBorder.transform.position.x, _rightEnemyBorder.transform.position.x);
            Instantiate(_enemyPrefab, new Vector3(randomX, _topEnemyBorder.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_minimumTimeToWait, _maximumTimeToWait));
        }
    }    
    private IEnumerator PowerUpSpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while (_stopSpawning == false)
        {
            SelectRandomPowerUp();

            yield return new WaitForSeconds(Random.Range(10.0f, 30.0f));
        }
    }

    private void SelectRandomPowerUp()
    {
        float randomX = Random.Range(_leftEnemyBorder.transform.position.x, _rightEnemyBorder.transform.position.x);
        
        int total = 0;

        int[] powerUpLootTable =
        {
           45, // Ammo+
           25, // Speed
           15, // Shield
           10, // TripleShot
           5   // HP+
        };

        int[] powerUpReward =
        {
            3, // Ammo+
            1, // Speed
            2, // Shield
            0, // TripleShot
            4  // HP+
        };

        foreach (var item in powerUpLootTable)
        {
            total += item;
        }

        int randomNumber = Random.Range(0, total + 1);
        var i = 0;

        foreach (var weight in powerUpLootTable)
        {
            if (randomNumber <= weight)
            {
                var powerUpToSpawn = powerUpReward[i];
                Instantiate(_powerUps[powerUpToSpawn], new Vector3(randomX, _topEnemyBorder.position.y,0),  Quaternion.identity);
                return;
            }

            i++;
            randomNumber -= weight;
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
