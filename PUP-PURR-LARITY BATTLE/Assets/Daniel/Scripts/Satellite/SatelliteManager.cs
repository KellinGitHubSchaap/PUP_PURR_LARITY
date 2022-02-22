using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SatelliteManager : Singleton<SatelliteManager>
{
    [SerializeField] private float satelliteDirOffset = -0.1f;
    
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Start()
    {
        SpawnSatellite();
    }

    [ContextMenu("SpawnSatellite")]
    public void SpawnSatellite()
    {
        var newSatellite = SatellitePool.SharedInstance.GetSatellite();
        newSatellite.SetActive(true);
        
        var randomPosition = GetRandomPositionOutOfView();
        newSatellite.transform.position = randomPosition;
        
        var satellite = newSatellite.GetComponent<Satellite>();
        satellite.SetDirection(GetCrossScreenDirection(randomPosition));
    }

    private Vector2 GetCrossScreenDirection(Vector2 startPosition)
    {
        var xOffset = Random.Range(-satelliteDirOffset, satelliteDirOffset);
        var yOffset = Random.Range(-satelliteDirOffset, satelliteDirOffset);

        var centerOffset = new Vector2(0.5f + xOffset, 0.5f + xOffset);
        var direction = ((Vector2) _camera.ViewportToWorldPoint(centerOffset) - startPosition).normalized;

        return direction;
    }

    private Vector2 GetRandomPositionOutOfView()
    {
        int xViewport = Random.Range(-5, 15);
        int yViewport;
        if (xViewport is < 0 or > 10)
        {
            yViewport = Random.Range(-5, 15);
        }
        else
        {
            var range = Enumerable.Range(-5, 20).Where(i => i is < 0 or > 10).ToArray();
            yViewport = range.ElementAt(Random.Range(0, range.Count() - 1));
        }

        var xDir = xViewport * 0.1f;
        var yDir = yViewport * 0.1f;
        
        var pos =  _camera.ViewportToWorldPoint(new Vector2(xDir, yDir));
        return pos;
    }
}
