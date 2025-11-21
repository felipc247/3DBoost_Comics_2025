using UnityEngine;

namespace Assets.Scripts.Character
{
    public class EnterCarCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        readonly float _clipLength;
        float _timePassed;

        public EnterCarCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
            _clipLength = _owner.EnterCarSettings.Clip.length;
        }

        public override void OnStart()
        {
            _owner.SetColliderTrigger();
            _owner.CurrentCar.Driver = _owner;
            _owner.transform.SetParent(_owner.CurrentCar.transform);
            _animator.SetTrigger(_owner.EnterCarSettings.AnimationTrigger);
            _timePassed = 0;
        }

        public override void OnUpdate()
        {
            _timePassed += Time.deltaTime;

            if (_owner.transform.rotation != _owner.CurrentCar.DrivePivot.rotation)
            {
                _owner.transform.rotation =
                    Quaternion.RotateTowards(
                        _owner.transform.rotation,
                        _owner.CurrentCar.DrivePivot.rotation,
                        _owner.RotationSpeed * Time.deltaTime);

                if (Quaternion.Angle(_owner.transform.rotation, _owner.CurrentCar.DrivePivot.rotation) > _owner.MinimumStoppingRotationAngle)
                {
                    return;
                }

                _owner.transform.rotation = _owner.CurrentCar.DrivePivot.rotation;
            }

            if (_timePassed >= _clipLength)
            {
                _owner.SetDrive();
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
