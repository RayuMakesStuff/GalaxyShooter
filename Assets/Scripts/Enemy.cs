using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")] 
    [SerializeField][Range(2.0f, 8.0f)] private float _speed = 5.0f;

    [Header("Position Data")] 
    private Transform _enemyDestroyPoint;
    
    [Header("Game Objects")]
    private Player _player;

    // ====================================================================
    
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
        _enemyDestroyPoint = GameObject.Find("EnemyDestroyPosition").transform;
    }

    private void NullChecking()
    {
        if (_enemyDestroyPoint == null)
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

        if (transform.position.y < _enemyDestroyPoint.transform.position.y)
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _player.AddScore(10);
            Destroy(this.gameObject);
        }


        if (other.CompareTag("Player"))
        {
            _player.Damage();
            Destroy(this.gameObject);
        }

    }
}
