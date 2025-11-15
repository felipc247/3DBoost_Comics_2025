using UnityEngine;

namespace Assets.Scripts.Character
{
    public class ExitCarCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        readonly float _clipLength;
        float _timePassed;
        public ExitCarCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
            _clipLength = _owner.ExitCarSettings.Clip.length;
        }

        public override void OnStart()
        {
            _animator.SetTrigger(_owner.ExitCarSettings.AnimationTrigger);
            GameManager.Instance.SwitchToPlayerCamera();
            _timePassed = 0;
        }

        public override void OnUpdate()
        {
            _timePassed += Time.deltaTime;
            if (_timePassed >= _clipLength)
            {
                _owner.SetIdle();
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
