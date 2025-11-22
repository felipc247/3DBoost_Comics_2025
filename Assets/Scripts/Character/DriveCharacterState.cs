using UnityEngine;

namespace Assets.Scripts.Character
{
    public class DriveCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        float _timePassed;
        public DriveCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
        }

        public override void OnStart()
        {
            GameManager.Instance.SwitchToCarCamera();
            GameManager.Instance.SetControllable(_owner.CurrentCar);
            _owner.transform.SetParent(_owner.CurrentCar.transform);
            _animator.SetTrigger(_owner.DriveSettings.AnimationTrigger);
            _timePassed = 0;
        }

        public override void OnUpdate()
        {

        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnEnd()
        {
            GameManager.Instance.SwitchToPlayerCamera();
        }
    }
}
