using System.Linq;
using UnityEngine;

public class PlayerGunner : MonoBehaviour
{
    public static bool canShoot = true;
    
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private float cooldownTime;
    [SerializeField] private Transform crosshair;
    
    private float _cooldownTimer;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        crosshair.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
        
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            return;
        }

        if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
        if (!canShoot) return;
        Shoot();
        _cooldownTimer = cooldownTime;
    }

    private void Shoot()
    {
        SoundManager.instance.PlayShootSound();
        var bullet = bulletPool.GetObject();
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().Init( _camera.ScreenToWorldPoint(Input.mousePosition));
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}
