using System;
using UnityEngine;

public enum SatelliteType {CatDog, BirdFish}
public class Satellite : MonoBehaviour
{
    public SatelliteType Type { get; private set; }
    [SerializeField] private float speed;
    [SerializeField] private Sprite catDogSprite;
    [SerializeField] private Sprite birdFishSprite;
    [SerializeField] private float lifeTime;

    private float _lifeTimer;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _direction;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _lifeTimer = 0;
    }

    private void Update()
    {
        Move();
        UpdateLifeTime();
    }

    private void UpdateLifeTime()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        var position = _transform.position;
        position += (Vector3) _direction * speed * Time.deltaTime;
        _transform.position = position;
    }

    public void SetType(SatelliteType type)
    {
        Type = type;
        switch (type)
        {
            case SatelliteType.CatDog:
                _spriteRenderer.sprite = catDogSprite;        
                break;
            case SatelliteType.BirdFish:
                _spriteRenderer.sprite = birdFishSprite;
                break;
        }
    }
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
