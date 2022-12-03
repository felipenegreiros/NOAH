using System;
using Assets.Scripts.A.I_test;
using Control;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour
    {
        private Aggro aggroComponent;
        private GameObject playerGameObject;

        private void Awake()
        {
            aggroComponent = GetComponent<Aggro>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
