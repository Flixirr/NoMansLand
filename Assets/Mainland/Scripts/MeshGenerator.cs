using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    public int mapWidth = 20;
    public int mapHeight = 20;
    public int seed;
    public float scale; 
    public int octaves; 
    [Range(0,1)]
    public float persistance; 
    public float lacunarity; 
    public Vector2 offset;
    public float heightScale = 1f;
    public AnimationCurve heighCurve;
    public float heightMultiplier = 1f;

    [Range(0,1)]
    public float falloffStart;
    [Range(0,1)]
    public float falloffEnd;
    public Vector2Int falloffSize;

    public Material[] terrainTypes;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    int biomeIdx;

    [SerializeField] VegetationGenerator TreesGen;

    void Start()
    {
        mesh = new Mesh();
        seed = (int)Random.Range(0f, 100000f);
        GetComponent<MeshFilter>().mesh = mesh;
        biomeIdx = (int)Random.Range(0f, terrainTypes.Length);
        GetComponent<MeshRenderer>().material = terrainTypes[biomeIdx] as Material;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GenerateVegetation();
    }

    void CreateShape()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth + 1, mapHeight + 1, seed, scale, octaves, persistance, lacunarity, offset);
        vertices = new Vector3[(mapWidth + 1) * (mapHeight + 1)];
        float[,] falloffMap = Noise.FalloffMap(falloffSize, falloffStart, falloffEnd);

        for (int i = 0, z = 0; z <= mapHeight; z++)
        {
            for (int x = 0; x <= mapWidth; x++)
            {
                float heightAtCurrentLevel = heighCurve.Evaluate(Mathf.Clamp(noiseMap[x, z] - falloffMap[x, z], 0f, 1f)) * heightMultiplier;
                vertices[i] = new Vector3(x, heightAtCurrentLevel, z);
                i++;
            }
        }
        
        triangles = new int[mapWidth * mapHeight * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < mapHeight; z++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + mapWidth + 1;
                triangles[tris + 2] = vert + 1;

                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + mapWidth + 1;
                triangles[tris + 5] = vert + mapWidth + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];
        for (int i = 0, z = 0; z <= mapHeight; z++)
        {
            for (int x = 0; x <= mapWidth; x++)
            {
                uvs[i] = new Vector2((float)x / mapWidth, (float)z / mapHeight);
                i++;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        
    }

    void GenerateVegetation()
    {
        TreesGen.GenerateFromPrefab(biomeIdx);
    }

    void OnValidate()
    {
        if(mapWidth < 1) 
        {
            mapWidth = 1;
        }
        if(mapHeight < 1) 
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
        if(heightMultiplier < 1f) 
        {
            heightMultiplier = 1f;
        }
    }
}