using UnityEngine;
public class BoostSpawner : MonoBehaviour
{
    [SerializeField] GameObject boostPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] int boostsToSpawn = 5;

    void Start()
    {
        // how many boosts we’ll actually spawn (can’t exceed spawn points count)
        int count = Mathf.Min(boostsToSpawn, spawnPoints.Length); 

        // indices will hold: [0, 1, 2, ..., n-1]
        int[] indices = new int[spawnPoints.Length]; // create an array to hold references to spawn point indices, to ensure no duplicates
        for(int i = 0; i < indices.Length; i++) // fill the array with numbers from 1 to the length of spawnPoints
        {
            indices[i] = i; 
        }

        for(int i = 0; i < count; i++) 
        {
            int r = Random.Range(i, indices.Length); // get random index from i to the end
            (indices[i],indices[r]) = (indices[r], indices[i]); // swap the index with the random number

            Transform p = spawnPoints[indices[i]]; // get the spawn point at the shuffled index
            Instantiate(boostPrefab, p.position, Quaternion.identity);
            //create a copy of boostPrefab at the position p, with no rotation
            //we create a copy so that we actually spawn something in the game world. A prefab is just a template, not an actual object in the scene.
        }
    }
}
