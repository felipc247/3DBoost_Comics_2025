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
            _animator.SetTrigger(_owner.EnterCarSettings.AnimationTrigger);
            _timePassed = 0;
        }

        public override void OnUpdate()
        {
            // TODO: SE HO RAGGIUNTO IL PIVOT, ALLORA RUOTARE IL PERSONAGGIO FINO AL DRIVE PIVOT DELLA MACCHINA
            // SENZA CAMBIARE STATO

            _owner.transform.rotation = new(_owner.transform.rotation.x, Mathf.Lerp(_owner.transform.rotation.y, _owner.CurrentCar.DrivePivot.rotation.y, 5 * Time.deltaTime), _owner.transform.rotation.z, _owner.transform.rotation.w);
            if (_owner.transform.rotation != _owner.CurrentCar.DrivePivot.rotation)
            {
                Debug.Log("Sto girando...");
                return; // se non sono ancora girato, esco
            }

            _timePassed += Time.deltaTime;
            if (_timePassed >= _clipLength)
            {
                _owner.SetDrive();
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
