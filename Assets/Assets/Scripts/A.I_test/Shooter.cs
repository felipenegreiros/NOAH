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
        public Quaternion desiredAngle;
        public float horizontalForce = 2.11f;
        public float verticalForce = 0.22f;

        private void Awake()
        {
            gameObjectTag = gameObject.tag;
            animatorComponent = gameObject.GetComponent<Animator>();
            if (!IsNpc()) return;
            playerObject = GameObject.FindGameObjectWithTag("Player");
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
            timeSinceLastAttack += Time.deltaTime;
            if (IsPlayer())
            {
                PlayerShootingAnimation();
            }
            else
            {
                // EnemyShootingAnimation();
            }
        }

        private Quaternion GetHorizontalAngleBetweenShooterAndPlayer()
        {
            var playerPosition = playerObject.transform.position;
            var shooterPosition = transform.position;
            var lookPos = playerPosition - shooterPosition;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            return rotation;
        }

        private void LookAtPlayer()
        {
            desiredAngle = GetHorizontalAngleBetweenShooterAndPlayer();
            var t = transform;
            t.rotation = Quaternion.Slerp(t.rotation, desiredAngle, Time.deltaTime * 10);
            // t.eulerAngles = new Vector3(90, transform.eulerAngles.y, t.eulerAngles.z);
            // transform.rotation = Quaternion.Euler(90, desiredAngle, 0);
        }

        public void EnemyShootingAnimation()
        {
            Debug.Log("Shooting!");
            LookAtPlayer();
            if (timeSinceLastAttack < 1)
            {
                timeSinceLastAttack += Time.deltaTime;
                // ;DisableShootingAnimation();
                return;
            }
            // EnableShootingAnimation();
            Invoke(nameof(PlayerShootProjectile), .4f);
            timeSinceLastAttack = 0;
            // Invoke(nameof(DisableShootingAnimation), .4f);
        }

        private void PlayerShootingAnimation()
        {
            if (Input.GetKeyDown(shootKey))
            {
                EnableShootingAnimation();
                Invoke(nameof(PlayerShootProjectile), 0.4f);
            }
            else
            {
                DisableShootingAnimation();
            }
        }

        private void EnableShootingAnimation()
        {
            animatorComponent.SetBool(Shoot1, true);
        }

        private void DisableShootingAnimation()
        {
            animatorComponent.SetBool(Shoot1, false);
        }
        
        
        private void PlayerShootProjectile() {
            var bulletGameObject = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
            bulletGameObject.transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, 0);
            var bulletComponent = bulletGameObject.GetComponent<Bullet>();
            bulletComponent.transform.forward = transform.forward;
            bulletComponent.Push(transform);
        }

        private void EnemyShootProjectile()
        {
            if(timeSinceLastAttack <= 1)
            {
                return;
            }
            var projectile = Instantiate(bullet, transform.position, transform.rotation);
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward*horizontalForce, ForceMode.Impulse);
            rb.AddForce(transform.up*verticalForce, ForceMode.Impulse);
            var bulletComponent = projectile.GetComponent<Bullet>();
            bulletComponent.SetOriginalInstigator(gameObject);
            timeSinceLastAttack = 0;
        }

        public void ShootAtPlayer()
        {
            LookAtPlayer();
            EnemyShootProjectile();
        }
    }
}
