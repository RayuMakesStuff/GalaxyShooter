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
            Debug.LogError("Cannot start because of an invalid configured timer! " +
                           "Please adjust the time values in the SpawnManager within the Editor" +
                           " and re-launch the application!");
        }

        else
        {
            StartSpawning();
        }
    }

    private void NullChecking()
    {
        if (_leftEnemyBorder == null)
        {
            Debug.LogError("'_leftEnemyBorder' is NULL! Have you named your GameObject 'LeftEnemyBorder'?");
        }

        if (_rightEnemyBorder == null)
        {
            Debug.LogError("'_rightEnemyBorder' is NULL! Have you named your GameObject 'RightEnemyBorder'?");
        }
        
        if (_topEnemyBorder == null)
        {
            Debug.LogError("'_topEnemyBorder' is NULL! Have you named your GameObject 'TopEnemyBorder'?");
        }
        
        if (_player == null)
        {
            Debug.LogError("'_player' is NULL! Have you named your GameObject 'Player'?");
        }
    }

    private void StartSpawning()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }

    private IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(_leftEnemyBorder.transform.position.x, _rightEnemyBorder.transform.position.x);
            Instantiate(_enemyPrefab, new Vector3(randomX, _topEnemyBorder.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_minimumTimeToWait, _maximumTimeToWait));
        }
    }    
    private IEnumerator PowerUpSpawnRoutine()
    {
        while (_stopSpawning == false)
        {
            float randomX = Random.Range(_leftEnemyBorder.transform.position.x, _rightEnemyBorder.transform.position.x);
            Instantiate(_tripleShotPowerUpPrefab, new Vector3(randomX, _topEnemyBorder.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_minimumTimeToWait, _maximumTimeToWait));
        }
    }
    
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
