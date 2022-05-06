using Unity.Entities;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;

[BurstCompile]
public struct Boid_Shader_Control : ISystem
{
    EntityQuery bcd_ShaderQuery;

    public void OnCreate(ref SystemState state)
    {
        bcd_ShaderQuery = state.GetEntityQuery(typeof(Boid_FrequencyData), typeof(Boid_IntensityData), ComponentType.ReadOnly<Boid_ComponentData>());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ComponentTypeHandle<Boid_ComponentData> bcd = state.GetComponentTypeHandle<Boid_ComponentData>();
        ComponentTypeHandle<Boid_FrequencyData> boidFrequencyData = state.GetComponentTypeHandle<Boid_FrequencyData>();
        ComponentTypeHandle<Boid_IntensityData> boidIntensityData = state.GetComponentTypeHandle<Boid_IntensityData>();
        ShaderJob sj = new ShaderJob
        {
            boidFrequencyData = boidFrequencyData,
            boidIntensityData = boidIntensityData,
            bcd = bcd,
        };
        state.Dependency = sj.ScheduleParallel(bcd_ShaderQuery, state.Dependency);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    struct ShaderJob : IJobEntityBatch
    {
        public ComponentTypeHandle<Boid_FrequencyData> boidFrequencyData;
        public ComponentTypeHandle<Boid_IntensityData> boidIntensityData;
        public ComponentTypeHandle<Boid_ComponentData> bcd;

        public void Execute(ArchetypeChunk chunk, int batchIndex)
        {
            NativeArray<Boid_FrequencyData> chunkFrequencyData = chunk.GetNativeArray(boidFrequencyData);
            NativeArray<Boid_IntensityData> chunkIntensityData = chunk.GetNativeArray(boidIntensityData);
            NativeArray<Boid_ComponentData> bcdData = chunk.GetNativeArray(bcd);
            for (int i = 0; i < chunk.Count; i++)
            {
                chunkFrequencyData[i] = new Boid_FrequencyData
                {
                    value = math.length(bcdData[i].velocity)
                };
                chunkIntensityData[i] = new Boid_IntensityData
                {
                    value =  math.lerp(0.4f, 1, bcdData[i].boidsInCell)
                };
            }
        }
    }
}
