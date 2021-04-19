using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField][Range(0.0f, 8.0f)] private float _speed = 2.5f;
    [SerializeField] private int _powerUpID;
    
    private Transform _powerUpDestroyPoint;
    private Player _player;


    // ======================================================
    private void Start()
    {
        FindGameObjects();
        NullChecking();
    }
    
    private void Update()
    {
        Movement();
    }

    private void FindGameObjects()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _powerUpDestroyPoint = GameObject.Find("EnemyDestroyPosition").transform;
    }

    private void NullChecking()
    {
        if (_powerUpDestroyPoint == null)
        {
            Debug.LogError("'_enemyDestroyPoint' is NULL! Have you named your GameObject 'EnemyDestroyPosition'?");
        }

        if (_player == null)
        {
            Debug.LogError("'_player' is NULL! Have you named your GameObject 'Player'?");
        }
    }
    
    private void Movement()
    {
        transform.Translate(Vector3.down * (_speed * Time.deltaTime));

        if (transform.position.y < _powerUpDestroyPoint.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            
            switch (_powerUpID)
            {
                case 0:
                    _player.ActivateTripleShot();
                    break;
                case 1:
                    _player.ActivateSpeedBoost();
                    break;
                case 2:
                    _player.ActivateShield();
                    break;
                default:
                    Debug.LogWarning("Invalid powerup ID!");
                    break;
            }
        }
    }
}
