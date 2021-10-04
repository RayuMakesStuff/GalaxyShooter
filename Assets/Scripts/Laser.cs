using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("Laser Properties")] 
    [SerializeField][Range(2.0f, 10.0f)] private float _playerLaserSpeed = 6.0f;
    [SerializeField][Range(2.0f, 10.0f)] private float _enemyLaserSpeed = 3.5f;

    [Header("Position Data")]
    private Transform _laserDestroyPosition;
    private Transform _enemyLaserDestroyPosition;
    
    private bool _isEnemyLaser = false;
    
    // ===============================================

    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }

    private void Update()
    {
        if (_isEnemyLaser == true)
        {
            MoveDown();
        }

        else
        {
            MoveUp();
        }
    }

    private void FindGameObjects()
    {
        _laserDestroyPosition = GameObject.Find("LaserDestroyPosition").transform;
        _enemyLaserDestroyPosition = GameObject.Find("EnemyLaserDestroyPosition").transform;
    }

    private void NullChecking()
    {
        if (_laserDestroyPosition == null)
        {
            Debug.LogError("'_laserDestroyPosition' is NULL! " + 
                           "Have you named the GameObject 'LaserDestroyPosition'?");
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * (_playerLaserSpeed * Time.deltaTime));

        if (transform.position.y > _laserDestroyPosition.transform.position.y)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }
    
    private void MoveDown()
    {
        transform.Translate(Vector3.down * (_enemyLaserSpeed * Time.deltaTime));

        if (transform.position.y < _enemyLaserDestroyPosition.transform.position.y)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);

            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
