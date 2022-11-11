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
        // public Quaternion angle;
        public float playerAngle;
        public float bottleAngle;
        public Quaternion desiredAngle;

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
            t.eulerAngles = new Vector3(90, transform.eulerAngles.y, t.eulerAngles.z);
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
            Invoke(nameof(ShootProjectile), .4f);
            timeSinceLastAttack = 0;
            // Invoke(nameof(DisableShootingAnimation), .4f);
        }

        private void PlayerShootingAnimation()
        {
            if (Input.GetKeyDown(shootKey))
            {
                EnableShootingAnimation();
                Invoke(nameof(ShootProjectile), 0.4f);
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
        
        
        private void ShootProjectile() {
            var bulletGameObject = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
            bulletGameObject.transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, 0);
            var bulletComponent = bulletGameObject.GetComponent<Bullet>();
            // Make transform.forward face towards the player
            bulletComponent.transform.forward = transform.forward;
            bulletComponent.Push(transform);
        }
    }
}
