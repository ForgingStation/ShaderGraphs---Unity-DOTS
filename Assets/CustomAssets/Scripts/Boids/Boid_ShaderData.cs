using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

public struct Boid_ShaderData : IComponentData
{

}

[MaterialProperty("_bfrequency", MaterialPropertyFormat.Float)]
public struct Boid_FrequencyData : IComponentData
{
    public float value;
}

[MaterialProperty("_bEIntensity", MaterialPropertyFormat.Float)]
public struct Boid_IntensityData : IComponentData
{
    public float value;
}
