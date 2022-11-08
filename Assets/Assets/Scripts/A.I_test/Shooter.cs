using System;
using UnityEngine;

namespace Assets.Scripts.A.I_test
{
    public class Shooter: MonoBehaviour
    {
        public KeyCode shootKey = KeyCode.J;
        public string gameObjectTag;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletPoint;
        [SerializeField] private Animator animatorComponent;
        private static readonly int Shoot1 = Animator.StringToHash("shoot");

        private void Awake()
        {
            gameObjectTag = gameObject.tag;
            if (gameObjectTag == "Player")
            {
                animatorComponent = gameObject.GetComponent<Animator>();   
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(shootKey))
            {
                animatorComponent.SetBool(Shoot1, true);
                Invoke("Shoot", 0.4f);
            }
            else
            {
                animatorComponent.SetBool(Shoot1, false);
            }
        }
        
        private void Shoot() {
            var bulletGameObject = Instantiate(bullet, bulletPoint.position, Quaternion.identity);
            var bulletComponent = bulletGameObject.GetComponent<Bullet>();
            bulletComponent.Push(transform);
        }
    }
}
