using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Character
{
    public class WalkAgentCharacterState : State
    {
        readonly PlayerController _owner;
        readonly Animator _animator;
        float _timePassed;
        public WalkAgentCharacterState(PlayerController owner, Animator animator)
        {
            _owner = owner;
            _animator = animator;
        }

        public override void OnStart()
        {
            _animator.SetTrigger(_owner.WalkAgentSettings.AnimationTrigger);
            _timePassed = 0;
            _owner.SetAgentDestination(_owner.CurrentCar.AccessPivot);
            //NavMeshPath path = new();
            //_owner.Agent.CalculatePath(_owner.CurrentCar.AccessPivot.position, path);
            //_owner.Agent.SetPath(path);
        }

        public override void OnUpdate()
        {
            // se l'utente preme spazio, allora annulla l'operazione
            // TODO:...

            if (!_owner.Agent.pathPending)
            {
                if (_owner.Agent.remainingDistance <= _owner.Agent.stoppingDistance)
                {
                    if (!_owner.Agent.hasPath || _owner.Agent.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("Path completo verso la macchina.");
                        // ho raggiunto la destinazione

                        // TODO: QUI FARE IN MODO CHE L'OWNER SI GIRI FINO ALLA STESSA ROTAZIONE DELL'ACCESSPIVOT DELLA MACCHINA
                        // SENZA USCIRE DA QUESTO STATO.

                        _owner.transform.rotation = new(_owner.transform.rotation.x, Mathf.Lerp(_owner.transform.rotation.y, _owner.CurrentCar.AccessPivot.rotation.y, 5 * Time.deltaTime), _owner.transform.rotation.z, _owner.transform.rotation.w);
                        if (_owner.transform.rotation != _owner.CurrentCar.AccessPivot.rotation)
                        {
                            Debug.Log("Sto girando...");
                            return; // se non sono ancora girato, esco
                        }


                        // QUANDO SONO ABBASTANZA GIRATO, ENTRO IN MACCHINA
                        _owner.SetEnterCar();
                        return;
                    }
                }
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
