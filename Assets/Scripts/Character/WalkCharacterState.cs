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
            if (_owner.Direction == Vector3.zero)
            {
                _owner.SetIdle();
                return;
            }
        }

        public override void OnFixedUpdate()
        {
            _rigidbody.AddForce(_owner.Acceleration * Time.fixedDeltaTime * _owner.Direction);
            
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
