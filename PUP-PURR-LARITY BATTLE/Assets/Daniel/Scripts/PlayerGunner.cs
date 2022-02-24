using UnityEngine;

public class PlayerGunner : MonoBehaviour
{
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private Texture2D cursor;
    [SerializeField] private float cooldownTime;

    private float _cooldownTimer;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
            _cooldownTimer = cooldownTime;

            Debug.Log("Dog Shot");
        }
    }

    private void Shoot()
    {
        var bullet = bulletPool.GetObject();
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().Init( _camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnEnable()
    {
        Cursor.SetCursor(cursor, Vector2.one * 50, CursorMode.ForceSoftware);
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
