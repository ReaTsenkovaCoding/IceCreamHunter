using UnityEngine;

public class CustomersSpawner : MonoBehaviour
{
    [SerializeField] GameObject customerPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int customersToSpawn = 9;
    void Start()
    {
        int count = Mathf.Min(customersToSpawn, spawnPoints.Length);

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
            Instantiate(customerPrefab, p.position, Quaternion.identity);

        }
        
        
    }

}
