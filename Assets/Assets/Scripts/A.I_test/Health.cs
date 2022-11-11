using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Assets.Scripts.A.I_test
{
    
    public class Health : MonoBehaviour
    {
        [SerializeField] private float hp;
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            //
        }
        [SerializeField] private bool dead;
        [SerializeField] private bool godMode;
        [SerializeField] private TakeDamageEvent takeDamage;
        // Start is called before the first frame update
        void Start()
        {
        
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            if (!godMode)
            {
                var newHp = Mathf.Max(hp - damage, 0);
                hp = newHp;
            }

            if (hp == 0)
            {
                Die();
                // AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        private float GetHp()
        {
            return hp;
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
