using UnityEngine;

public enum SatelliteType {CatDog, BirdFish}
public class Satellite : MonoBehaviour
{
    public SatelliteType Type { get; private set; }
    [SerializeField] private float speed;
    [SerializeField] private Sprite catDogSprite;
    [SerializeField] private Sprite birdFishSprite;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject explosion;

    private SpriteRenderer _spriteRenderer;
    private float _lifeTimer;
    private Vector2 _direction;
    private Transform _transform;
    
    private void Awake()
    {
        _transform = transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(Vector2 startPosition, Vector2 direction, SatelliteType type)
    {
        _transform.position = startPosition;
        _direction = direction;
        SetType(type);
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
            OnExpiration();
            gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        var position = _transform.position;
        position += (Vector3) _direction * speed * Time.deltaTime;
        _transform.position = position;
    }

    private void SetType(SatelliteType type)
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
   
    private void OnExpiration()
    {
        if (Type == SatelliteType.BirdFish)
        {
            ScoreManager.instance.UpdateCatDogBar(-1, 0);
        }
        else
        {
            ScoreManager.instance.UpdateCatDogBar(2, -1);
        }
    }
    
    public void Hit()
    {
        switch (Type)
        {
            case SatelliteType.CatDog:
                ScoreManager.instance.UpdateCatDogBar(-10, 10);
                MonitorManager.Instance.BlinkOnce(MonitorType.CatDog, false);
                MonitorManager.Instance.BlinkOnce(MonitorType.BirdFish);
                break;
            case SatelliteType.BirdFish:
                ScoreManager.instance.UpdateCatDogBar(20, -15);
                MonitorManager.Instance.BlinkOnce(MonitorType.CatDog);
                MonitorManager.Instance.BlinkOnce(MonitorType.BirdFish, false);
                break;
        }

        Instantiate(explosion, _transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
