using Assets.Scripts.Car;
using Assets.Scripts.Character;
using Assets.Scripts.Enums;
using Assets.Scripts.Interfaces;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour, IControllable
{
    [Header("State Machine Settings")]
    public CommonStateSettings IdleSettings;
    public CommonStateSettings WalkSettings;
    public CommonStateSettings EnterCarSettings;
    public CommonStateSettings DriveSettings;
    public CommonStateSettings ExitCarSettings;
    public CommonStateSettings WalkAgentSettings;

    [Header("Car Check Settings")]
    [SerializeField] float radius;
    [SerializeField] Vector3 direction;
    [SerializeField] float maxDistance;
    [SerializeField] LayerMask carMask;

    GenericStateMachine<CharacterStateEnum> stateMachine;
    Animator _animator;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    private bool _interactionRequest;
    public bool InteractionRequest => _interactionRequest;

    private CarControl _currentCar;
    public CarControl CurrentCar => _currentCar;

    public float RotationSpeed = 30f;
    public float MinimumStoppingRotationAngle = 5f;

    public Vector3 Direction = Vector3.zero;
    public float Acceleration = 100f;
    public float MaxSpeed = 5f;

    Collider _capsuleCollider;

    Camera _mainCamera;
    public Camera MainCamera => _mainCamera;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _capsuleCollider = GetComponent<Collider>();

        _agent.enabled = false;

        _mainCamera = Camera.main;

        stateMachine = new();
        stateMachine.RegisterState(CharacterStateEnum.Idle, new IdleCharacterState(this, _animator));
        stateMachine.RegisterState(CharacterStateEnum.Walk, new WalkCharacterState(this, _animator));
        stateMachine.RegisterState(CharacterStateEnum.EnterCar, new EnterCarCharacterState(this, _animator));
        stateMachine.RegisterState(CharacterStateEnum.Drive, new DriveCharacterState(this, _animator));
        stateMachine.RegisterState(CharacterStateEnum.ExitCar, new ExitCarCharacterState(this, _animator));
        stateMachine.RegisterState(CharacterStateEnum.WalkAgent, new WalkAgentCharacterState(this, _animator));

        SetIdle();
    }

    #region STATE MACHINE SETTERS
    [ContextMenu("Set Idle")]
    public void SetIdle()
    {
        stateMachine.SetState(CharacterStateEnum.Idle);
    }
    [ContextMenu("Set Walk")]
    public void SetWalk()
    {
        stateMachine.SetState(CharacterStateEnum.Walk);
    }
    [ContextMenu("Set Enter Car")]
    public void SetEnterCar()
    {
        stateMachine.SetState(CharacterStateEnum.EnterCar);
    }
    [ContextMenu("Set Drive")]
    public void SetDrive()
    {
        stateMachine.SetState(CharacterStateEnum.Drive);
    }
    [ContextMenu("Set Exit Car")]
    public void SetExitCar()
    {
        stateMachine.SetState(CharacterStateEnum.ExitCar);
    }
    [ContextMenu("Set Walk Agent")]
    public void SetWalkAgent()
    {
        stateMachine.SetState(CharacterStateEnum.WalkAgent);
    }
    #endregion

    private void Update()
    {
        stateMachine.OnUpdate();

        ReleaseAllRequests();
    }

    private void FixedUpdate()
    {
        stateMachine.OnFixedUpdate();
    }

    public void SetNearestCar()
    {
        RaycastHit[] results = new RaycastHit[4];

        if (Physics.SphereCastNonAlloc(
            transform.position,
            radius,
            direction,
            results,
            maxDistance,
            carMask) > 0)
        {
            // abbiamo beccato almeno 1 macchina su 4 disponibilità
            var carSelected = results
                .Where(r => r.collider != null)
                .Select(r => r.collider.GetComponentInParent<CarControl>())
                .Where(c => c.HasNoDriver())
                .OrderBy(cc => Vector3.Distance(transform.position, cc.AccessPivot.position))
                .FirstOrDefault();

            if (carSelected)
            {
                carSelected.SetDriver(this);
                _currentCar = carSelected;
            }
        }
    }

    public void SetAgentDestination(Transform destination)
    {
        if (!_agent.enabled)
            _agent.enabled = true;

        _agent.SetDestination(destination.position);
    }

    public void Move(Vector2 direction)
    {
        Direction = new Vector3(direction.x, 0, direction.y);
    }

    public void MoveCanceled()
    {
        Direction = Vector3.zero;
    }

    /// <summary>
    /// Called with Button E from GameManager
    /// </summary>
    public void Interact()
    {
        _interactionRequest = true;
    }

    /// <summary>
    /// Called from a State of the StateMachine
    /// </summary>
    public void ReleaseInteraction()
    {
        _interactionRequest = false;
    }

    private void ReleaseAllRequests()
    {
        ReleaseInteraction();
    }

    /// <summary>
    /// Called from external behavior used in Animator
    /// </summary>
    public void SitIntoCar()
    {
        SetAgentDestination(CurrentCar.DrivePivot);

        // TODO: sarebbe da creare uno stato per gestire
        // SOLO il fatto di aver raggiunto la posizione di seduta
        // senza quindi cambiare l'animazione in corso
    }

    /// <summary>
    /// Called from external behavior used in Animator
    /// </summary>
    public void DisableAgent()
    {
        _agent.enabled = false;
    }

    public void ExitFromCar()
    {
        SetAgentDestination(CurrentCar.ExitPivot);
    }

    public void SetColliderTrigger()
    {
        _capsuleCollider.isTrigger = true;
    }

    public void SetColliderSolid()
    {
        _capsuleCollider.isTrigger = false;
    }

    internal void ForgetCar()
    {
        transform.SetParent(null);
        _currentCar = null;
    }
}

[Serializable]
public class CommonStateSettings
{
    [Tooltip("Trigger per attivare l'animazione")]
    public string AnimationTrigger;
    [Tooltip("Animazione corrispondente nell'animator")]
    public AnimationClip Clip;
}