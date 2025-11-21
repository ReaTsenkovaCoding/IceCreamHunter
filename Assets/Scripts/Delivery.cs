using TMPro;
using UnityEngine;

public class Delivery : MonoBehaviour
{
  bool hasPackage;
  [SerializeField] float destroyDelay = 0.5f;
  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Package") && !hasPackage)
    {
      Debug.Log("Picked up package");
      hasPackage = true;
      GetComponent <ParticleSystem>().Play();
      Destroy(collision.gameObject, destroyDelay);
      
    }

    if (collision.CompareTag("Customer") && hasPackage)
    {
      Debug.Log("Delivered package");
      hasPackage = false;
      GetComponent<ParticleSystem>().Stop();
      Destroy(collision.gameObject, destroyDelay);
    }
  }
}
