using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    [SerializeField] int density;

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    [SerializeField, Range(0, 1)] float rotateTowardsNormal;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;

    [SerializeField] Material[] baseMats;
    [SerializeField] Material[] additionalMats;

    public void GenerateFromPrefab(int biomeIdx)
    {
        Clear();

        LayerMask terrainMask = 1 << 3;

        for(int i = 0; i < density; i++)
        {
            float sampleX = Random.Range(xRange.x, xRange.y);
            float sampleZ = Random.Range(zRange.x, zRange.y);
            Vector3 rayStart = new Vector3(sampleX, maxHeight, sampleZ);

            if(!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity, terrainMask, QueryTriggerInteraction.Ignore))
                continue;
            
            if(hit.point.y < minHeight)
                continue;

            GameObject prefabInstance = (GameObject) Instantiate(prefabs[(int) Random.Range(0f, prefabs.Length)], transform);
            prefabInstance.transform.position = hit.point;
            prefabInstance.transform.Rotate(Vector3.up, Random.Range(rotationRange.x, rotationRange.y), Space.Self);
            prefabInstance.transform.localScale = new Vector3(
              Random.Range(minScale.x, maxScale.x),
              Random.Range(minScale.y, maxScale.y),
              Random.Range(minScale.z, maxScale.z)  
            );
            prefabInstance.GetComponent<PrefabTextures>().ChangeMaterial(baseMats[biomeIdx], additionalMats[biomeIdx]);
        }
    }

    public void Clear()
    {
        while(transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
}
