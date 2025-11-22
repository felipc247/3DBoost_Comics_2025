using UnityEngine;

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
            //_owner.transform.rotation =
            //    Quaternion.RotateTowards(
            //    _owner.transform.rotation,
            //    _owner.MainCamera.transform.rotation,
            //    _owner.RotationSpeed * Time.deltaTime);

            if (_owner.Direction == Vector3.zero)
            {
                _owner.SetIdle();
                return;
            }
        }

        public override void OnFixedUpdate()
        {
            Vector3 characterDir = _owner.MainCamera.transform.TransformDirection(_owner.Direction);
            characterDir.y = 0;
            _rigidbody.AddForce(_owner.Acceleration * Time.fixedDeltaTime * characterDir);

            _owner.transform.forward = characterDir;
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
