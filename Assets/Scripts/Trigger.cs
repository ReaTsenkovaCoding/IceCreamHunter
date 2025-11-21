using UnityEngine;

public class Trigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered!");
    }    
}
