using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Character
{
    public class WalkCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        float _timePassed;
        readonly Rigidbody _rigidbody;
        public WalkCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
            _rigidbody = _owner.GetComponent<Rigidbody>();
        }

        public override void OnStart()
        {
            _rigidbody.isKinematic = false;

            _animator.SetTrigger(_owner.WalkSettings.AnimationTrigger);
            _timePassed = 0;
        }

        public override void OnUpdate()
        {
            if (_owner.Direction == Vector3.zero)
            {
                _owner.SetIdle();
                return;
            }

            // get the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            // gradually rotate to target rotation
            _owner.transform.rotation = Quaternion.RotateTowards(
                _owner.transform.rotation,
                targetRotation,
                _owner.RotationSpeed * Time.deltaTime);

            //Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            //_rigidbody.MoveRotation(Quaternion.RotateTowards(_owner.transform.rotation, targetRotation, _owner.RotationSpeed * Time.deltaTime));
        }

        private Vector3 moveDirection;

        public override void OnFixedUpdate()
        {
            // get the movement direction relative to the camera
            moveDirection = _owner.MainCamera.transform.TransformDirection(_owner.Direction).normalized;
            // null the camera tilt
            moveDirection.y = 0;

            Vector3 velocity = _owner.MaxSpeed * Time.fixedDeltaTime * moveDirection;
            velocity.y = _rigidbody.linearVelocity.y;
            _rigidbody.linearVelocity = velocity;



            //_rigidbody.AddForce(_owner.Acceleration * Time.fixedDeltaTime * characterDir);

            //_owner.transform.forward = moveDirection;
            //Vector3.RotateTowards(
            //    _owner.transform.forward,
            //    characterDir,
            //    _owner.RotationSpeed * Time.fixedDeltaTime,
            //    0);


            _rigidbody.linearVelocity =
                new Vector3(
                    Mathf.Clamp(_rigidbody.linearVelocity.x, -_owner.MaxSpeed, _owner.MaxSpeed),
                    _rigidbody.linearVelocity.y,
                    Mathf.Clamp(_rigidbody.linearVelocity.z, -_owner.MaxSpeed, _owner.MaxSpeed));
        }

        public override void OnEnd()
        {

        }
    }
}
