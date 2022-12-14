using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Attributes
{
    
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHp = 100f;
        public float currentHp;
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            //
        }
        [SerializeField] private bool dead;
        [SerializeField] private bool godMode;
        [SerializeField] private TakeDamageEvent takeDamage;
        // Start is called before the first frame update
        public void Start()
        {
            currentHp = maxHp;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            if (!godMode)
            {
                var newHp = Mathf.Max(currentHp - damage, 0);
                currentHp = newHp;
            }

            if (currentHp == 0)
            {
                Die();
                // AwardExperience(instigator);
            }
            // else
            // {
            //     takeDamage.Invoke(damage);
            // }
        }

        private float GetHp()
        {
            return currentHp;
        }
        
        public float GetPercentage()
        {
            var maximumHp = maxHp;
            var ratio = currentHp / maximumHp;
            return ratio;
        }

        private void Die()
        {
            if (dead)
            {
                return;
            }
            dead = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
