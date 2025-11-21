using UnityEngine;

public class PackageSpawner : MonoBehaviour
{
    [SerializeField] GameObject packagePrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int packagesToSpawn = 9;
    void Start()
    {
        
        int count = Mathf.Min(packagesToSpawn, spawnPoints.Length);

        int[] indices = new int [spawnPoints.Length];

        for(int i = 0; i < indices.Length; i++)
        {
            indices[i] = i;
        }

        for(int i = 0; i < count; i++)
        {
            int r = Random.Range(i, indices.Length);
            (indices[i], indices[r]) = (indices[r], indices[i]);

            Transform p = spawnPoints[indices[i]];
            Instantiate(packagePrefab, p.position, Quaternion.identity);
        }


    }
}
