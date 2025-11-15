using Assets.Scripts.Extensions;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class WalkAgentCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        float _timePassed;
        readonly Rigidbody _rigidbody;
        public WalkAgentCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
            _rigidbody = _owner.GetComponent<Rigidbody>();
        }

        public override void OnStart()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;

            _rigidbody.isKinematic = true;

            _animator.SetTrigger(_owner.WalkAgentSettings.AnimationTrigger);
            _timePassed = 0;
            _owner.SetAgentDestination(_owner.CurrentCar.AccessPivot);
        }

        public override void OnUpdate()
        {
            if (_owner.Agent.IsAgentStopped(0.5f))
            {
                _owner.transform.rotation = 
                    Quaternion.RotateTowards(
                        _owner.transform.rotation, _owner.CurrentCar.AccessPivot.rotation, _owner.RotationSpeed * Time.deltaTime);

                if (Quaternion.Angle(_owner.transform.rotation, _owner.CurrentCar.AccessPivot.rotation) > _owner.MinimumStoppingRotationAngle)
                {
                    return;
                }

                _owner.transform.rotation = _owner.CurrentCar.AccessPivot.rotation;
                _owner.SetEnterCar();
                return;
            }
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnEnd()
        {

        }
    }
}
