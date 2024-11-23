using UnityEngine;

public class LazerBullet : MonoBehaviour
{
    public GameObject impactEffectPrefab;
    public float damage = 15f;             

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (impactEffectPrefab != null)
        {
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);
        }

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.Instance.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
