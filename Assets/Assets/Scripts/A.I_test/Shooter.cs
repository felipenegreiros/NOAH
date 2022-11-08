using System;
using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Shooter: MonoBehaviour
    {
        public KeyCode shootKey = KeyCode.J;
        public string gameObjectTag;
        public GameObject playerObject;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletPoint;
        [SerializeField] private Animator animatorComponent;
        private static readonly int Shoot1 = Animator.StringToHash("shoot");
        public float timeSinceLastAttack;

        private void Awake()
        {
            gameObjectTag = gameObject.tag;
            animatorComponent = gameObject.GetComponent<Animator>();  
            if (IsNpc())
            {
                playerObject = GameObject.FindGameObjectWithTag("Player");
            }
        }

        private bool IsPlayer()
        {
            return gameObjectTag == "Player";
        }

        private bool IsNpc()
        {
            return gameObjectTag != "Player";
        }

        private void Update()
        {
            if (IsPlayer())
            {
                ShootingAnimation();
            }
            else
            {
                if (timeSinceLastAttack < 1)
                {
                    timeSinceLastAttack += Time.deltaTime;
                    animatorComponent.SetBool(Shoot1, false);
                    return;
                }
                animatorComponent.SetBool(Shoot1, true);
                Invoke(nameof(ShootProjectile), .4f);
                timeSinceLastAttack = 0;
            }
        }

        private void ShootingAnimation()
        {
            if (Input.GetKeyDown(shootKey))
            {
                animatorComponent.SetBool(Shoot1, true);
                Invoke(nameof(ShootProjectile), 0.4f);
            }
            else
            {
                animatorComponent.SetBool(Shoot1, false);
            }
        }
        
        private void ShootProjectile() {
            var bulletGameObject = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
            var bulletComponent = bulletGameObject.GetComponent<Bullet>();
            bulletComponent.Push(transform);
        }
    }
}
