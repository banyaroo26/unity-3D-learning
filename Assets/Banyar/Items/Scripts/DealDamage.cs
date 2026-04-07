using UnityEngine;
using Banyar.Enemy;

namespace Banyar.Items 
{
    public class DealDamage : MonoBehaviour
    {
        [SerializeField]protected float damageAmount = 0f;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Hit");
                HealthSystem healthSystem = other.GetComponent<HealthSystem>();
                if (healthSystem != null)
                {
                    healthSystem.TakeDamage(damageAmount);
                }
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Deal Damage Started");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}