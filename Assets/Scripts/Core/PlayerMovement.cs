using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core
{
    public class PlayerMovement: MonoBehaviour
    {
        [SerializeField] private Animator animatorComponent;
        [SerializeField] private GameObject  playerGameObject;
        public float moveSpeed;
        public const float RunningThreshold = 0.5f;
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode runKey = KeyCode.LeftShift;
        
        private float _runningTime;
        private CharacterController _characterController;
        private float horizontalInputX;
        private float horizontalInputY;
        private Vector3 _verticalMovement;
        private static readonly int Bool1 = Animator.StringToHash("bool1");
        private static readonly int Bool2 = Animator.StringToHash("bool2");
        private static readonly int LBool = Animator.StringToHash("lbool");
        private static readonly int LBool2 = Animator.StringToHash("lbool2");
        private static readonly int RBool = Animator.StringToHash("rbool");
        private static readonly int RBool2 = Animator.StringToHash("rbool2");
        private static readonly int CrouchBool = Animator.StringToHash("Agacha");
        private static readonly int Walk2Run = Animator.StringToHash("walk2run");


        private void Awake()
        {
            playerGameObject = GameObject.FindWithTag("Player");
            _characterController = playerGameObject.GetComponent<CharacterController>();
        }

        private void Update()
        {
            DiagonalMovement();
            ForwardMovement();
            LeftMovement();
            RightMovement();
            CrouchMovement();
            RunMovement();
        }

        private void DiagonalMovement()
        {
            horizontalInputX = Input.GetAxis("Horizontal") * 100;
            horizontalInputY = Input.GetAxis("Vertical") * 100;
        }

        private void ForwardMovement()
        {
            if (Input.GetKeyDown(upKey))
            {
                animatorComponent.SetBool(Bool1, true);
                animatorComponent.SetBool(Bool2, true);
                horizontalInputX *= 100;
                // tempo = 0;
            }
            if (Input.GetKeyUp(upKey))
            {
                animatorComponent.SetBool(Bool1, false);
                animatorComponent.SetBool(Bool2, false);
            }
        }

        private void LeftMovement()
        {
            if (Input.GetKeyDown(leftKey))
            {
                animatorComponent.SetBool(LBool, true);
                animatorComponent.SetBool(LBool2, false);
            }
            if(Input.GetKeyUp(leftKey))
            {
                animatorComponent.SetBool(LBool, false);
                animatorComponent.SetBool(LBool2, true);
            }
        }

        private void RightMovement()
        {
            if(Input.GetKeyDown(rightKey))
            {
                animatorComponent.SetBool(RBool, true);
                animatorComponent.SetBool(RBool2, false);
            }
            if(Input.GetKeyUp(rightKey))
            {
                animatorComponent.SetBool(RBool, false);
                animatorComponent.SetBool(RBool2, true);
            }
        }

        private void CrouchMovement()
        {
            if (Input.GetKeyDown(downKey)) {
                animatorComponent.SetBool(CrouchBool, true);
                // mainCollider.enabled = false;
            }
        
            if (Input.GetKeyUp(downKey)) {
                animatorComponent.SetBool(CrouchBool, false);
                // mainCollider.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            var currentTransform = _characterController.transform.forward;
            var movementValueIncrease = horizontalInputY * (2f * Time.deltaTime);
            _verticalMovement = currentTransform * movementValueIncrease;
        
            var rotateValueIncrease = horizontalInputX * 2f * Time.deltaTime * Vector3.up;
            _characterController.transform.Rotate(rotateValueIncrease);

            if (!Input.GetKeyDown(upKey) && !Input.GetKeyDown(runKey)) return;
            var moveIncrease = moveSpeed * Time.deltaTime * _verticalMovement;
            _characterController.Move(moveIncrease);
        }
        
        private void RunMovement()
        {
            RunningTime();
            RunMechanics();
        }

        private void RunningTime()
        {
            if (Input.GetKey(runKey)) {
                _runningTime += Time.deltaTime;
            }
            else
            {
                _runningTime = 0;
                animatorComponent.SetBool(Walk2Run, false);
            }
        }

        private void RunMechanics()
        {
            if (_runningTime is >= RunningThreshold)
            {
                animatorComponent.SetBool(Walk2Run, true);
            }
        }
    }
}
