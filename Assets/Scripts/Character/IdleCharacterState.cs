using UnityEngine;

namespace Assets.Scripts.Character
{
    public class IdleCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        float _timePassed;
        public IdleCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
        }

        public override void OnStart()
        {
            _animator.SetTrigger(_owner.IdleSettings.AnimationTrigger);
            _timePassed = 0;
        }

        public override void OnUpdate()
        {
            if (_owner.InteractionRequest)
            {
                _owner.SetNearestCar();
                if (_owner.CurrentCar != null)
                {
                    _owner.SetWalkAgent();
                    return;
                }
            }

            if (_owner.Direction != Vector3.zero)
            {
                _owner.SetWalk();
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
