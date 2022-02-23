using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SatelliteManager : Singleton<SatelliteManager>
{
    [SerializeField] private int catDogRngWeight = 1;
    [SerializeField] private int birdFishRngWeight = 1;
    [SerializeField] private float timeBetweenSpawns;

    [SerializeField] private float satelliteDirOffset = 0.1f;
    [SerializeField] private ObjectPool satellitePool;

    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    [ContextMenu("StartSpawning")]
    public void StartSatelliteSpawning()
    {
        InvokeRepeating(nameof(SpawnSatellite), 0, timeBetweenSpawns);
    }

    [ContextMenu("StopSpawning")]
    public void StopSatelliteSpawning()
    {
        CancelInvoke(nameof(SpawnSatellite));
    }

    private void SpawnSatellite()
    {
        var randomPosition = GetRandomPositionOutOfView();
        var direction = GetCrossScreenDirection(randomPosition);
        var type = GetRandomType();

        var newSatellite = satellitePool.GetObject();
        newSatellite.SetActive(true);

        newSatellite.GetComponent<Satellite>().Init(randomPosition, direction, type);
    }

    private SatelliteType GetRandomType()
    {
        var totalRngWeight = catDogRngWeight + birdFishRngWeight;
        var rand = Random.Range(0, totalRngWeight);

        return rand < catDogRngWeight ? SatelliteType.CatDog : SatelliteType.BirdFish;
    }

    private Vector2 GetCrossScreenDirection(Vector2 startPosition)
    {
        var xOffset = Random.Range(-satelliteDirOffset, satelliteDirOffset);
        var yOffset = Random.Range(-satelliteDirOffset, satelliteDirOffset);

        var centerOffset = new Vector2(0.5f + xOffset, 0.5f + yOffset);
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

        var pos = _camera.ViewportToWorldPoint(new Vector2(xDir, yDir));
        return pos;
    }
}