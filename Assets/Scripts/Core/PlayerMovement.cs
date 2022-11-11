using UnityEngine;

namespace Core
{
    public class PlayerMovement: MonoBehaviour
    {
        [SerializeField] private Animator animatorComponent;
        public float inX;
        public KeyCode upKey = KeyCode.W;
        public KeyCode downKey = KeyCode.S;
        public KeyCode leftKey = KeyCode.A;
        public KeyCode rightKey = KeyCode.D;
        private static readonly int Bool1 = Animator.StringToHash("bool1");
        private static readonly int Bool2 = Animator.StringToHash("bool2");
        private static readonly int LBool = Animator.StringToHash("lbool");
        private static readonly int LBool2 = Animator.StringToHash("lbool2");
        private static readonly int RBool = Animator.StringToHash("rbool");
        private static readonly int RBool2 = Animator.StringToHash("rbool2");
        private static readonly int CrouchBool = Animator.StringToHash("Agacha");

        private void Update()
        {
            ForwardMovement();
            LeftMovement();
            RightMovement();
            CrouchMovement();
        }

        private void ForwardMovement()
        {
            if (Input.GetKeyDown(upKey)) {
                animatorComponent.SetBool(Bool1, true);
                animatorComponent.SetBool(Bool2, true);
                inX = inX * 100;
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
            if (Input.GetKeyDown(leftKey)) {
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
        
    }
}
