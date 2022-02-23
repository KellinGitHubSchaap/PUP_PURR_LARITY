using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private SpriteRenderer sprite;

    private float _lifeTimer;
    private bool _canHit = true;
    private Transform _transform;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _transform = transform;
    }

    public void Init(Vector2 position)
    {
        _collider.enabled = false;
        _canHit = true;
        _lifeTimer = 0;
        _transform.localScale = Vector3.one;
        _transform.localPosition = position;
        sprite.sortingOrder = 1;
    }

    private void Update()
    {
        if (_lifeTimer < lifeTime)
        {
            _lifeTimer += Time.deltaTime;
            _transform.localScale -= Vector3.one / lifeTime * Time.deltaTime;

            if (_lifeTimer > lifeTime / 2 && _canHit)
            {
                _collider.enabled = true;
                Invoke(nameof(GoPast), 0.05f);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void GoPast()
    {
        _canHit = false;
        sprite.sortingOrder = -1;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Satellite") || !_canHit) return;

        col.GetComponent<Satellite>().Hit();
        gameObject.SetActive(false);
    }
}