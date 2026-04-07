using UnityEngine;

namespace Banyar.Enemy 
{
    public class HealthSystem : MonoBehaviour
    {
        private float Health = 100f;

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0f)
            {
                Die();
            }
        }

        void Die()
        {
            Debug.Log("Die");
            gameObject.SetActive(false);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Debug.Log("Health System Started");
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}