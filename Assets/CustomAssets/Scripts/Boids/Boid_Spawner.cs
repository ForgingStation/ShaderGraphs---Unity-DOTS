using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using Unity.Transforms;

public class Boid_Spawner : MonoBehaviour
{
    public int boidsPerInterval;
    public int boidsToSpawn;
    public float interval;
    public float cohesionBias;
    public float separationBias;
    public float alignmentBias;
    public float targetBias;
    public float perceptionRadius;
    public float3 target;
    public float minSpeed;
    public float maxSpeed;
    public float step;
    public int cellSize;
    public GameObject prefabToSpawn;

    private EntityManager entitymanager;
    private float elapsedTime;
    private int totalSpawnedBoids;
    private float3 currentPosition;
    private BlobAssetStore bas;
    private GameObjectConversionSettings gocs;
    private Entity convertedEntity;

    private void Start()
    {
        totalSpawnedBoids = 0;
        entitymanager = World.DefaultGameObjectInjectionWorld.EntityManager;
        currentPosition = this.transform.position;
        bas = new BlobAssetStore();
        gocs = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, bas);
        convertedEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefabToSpawn, gocs);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime = 0;
            for (int i = 0; i <= boidsPerInterval; i++)
            {
                if (totalSpawnedBoids == boidsToSpawn)
                {
                    break;
                }
                Entity e;
                if (i == 0)
                {
                    e = convertedEntity;
                }
                else
                {
                    e = entitymanager.Instantiate(convertedEntity);
                }

                entitymanager.AddComponentData(e, new Translation
                {
                    Value = currentPosition
                });
                entitymanager.AddComponentData(e, new Boid_ComponentData
                {
                    velocity = math.normalize(UnityEngine.Random.insideUnitSphere) * maxSpeed,
                    perceptionRadius = perceptionRadius,
                    speed = UnityEngine.Random.Range(minSpeed,maxSpeed),
                    step = step,
                    cohesionBias = cohesionBias,
                    separationBias = separationBias,
                    alignmentBias = alignmentBias,
                    target = target,
                    targetBias = targetBias,
                    cellSize = cellSize
                });
                entitymanager.AddComponentData(e, new Boid_FrequencyData{});
                entitymanager.AddComponentData(e, new Boid_IntensityData { });
                totalSpawnedBoids++;
            }
        }
    }
}