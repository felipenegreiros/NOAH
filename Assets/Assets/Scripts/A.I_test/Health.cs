using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.A.I_test
{
    
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _hp;
        [Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            //
        }
        [SerializeField] private bool _dead;
        [SerializeField] private bool _godMode;
        [SerializeField] private TakeDamageEvent _takeDamage;
        // Start is called before the first frame update
        void Start()
        {
        
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            if (!_godMode)
            {
                _hp = Mathf.Max(_hp - damage, 0);
            }

            if (_hp == 0)
            {
                Die();
                // AwardExperience(instigator);
            }
            else
            {
                _takeDamage.Invoke(damage);
            }
        }

        private float GetHp()
        {
            return _hp;
        }

        private void Die()
        {
            if (_dead)
            {
                return;
            }
            _dead = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
