using birds_game.Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Trashcan : MonoBehaviour
{
    [SerializeField] private float _maxSpawnTime = 5f;
    [SerializeField] private float _minSpawnTime = 2f;
    [SerializeField] private float _spawnTimer = 5f;
    [SerializeField] private List<GameObject> _trashPrefabs;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private CircleCollider2D _spawnRange;

    private Rigidbody2D _rigidbody2D;

    private bool _kicked = false;
    private bool _isInSpawnRange = false;
    
    private float _currentTimer;
    void Start()
    {
        _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0 && _isInSpawnRange && !_kicked)
        {
            var prefabIndex = Random.Range(0, _trashPrefabs.Count);
            var trash = Instantiate(_trashPrefabs[prefabIndex], _spawnPosition.position, Quaternion.identity);
            var trashRigidbody = trash.GetComponent<Rigidbody2D>();
            var force = new Vector2(-1, 0.5f);
            trashRigidbody.AddForce( force * 5f, ForceMode2D.Impulse);
            _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var birdLayer = LayerMask.NameToLayer("Player");
        
        if (col.gameObject.layer == birdLayer)
        {
            _isInSpawnRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        var birdLayer = LayerMask.NameToLayer("Player");
        
        if (col.gameObject.layer == birdLayer)
        {
            _isInSpawnRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var birdLayer = LayerMask.NameToLayer("Player");
        
        if (collision.collider.gameObject.layer == birdLayer && !_kicked)
        {
            _kicked = true;
            GameManager.Instance.TakeDamage();
        };
    }
}
