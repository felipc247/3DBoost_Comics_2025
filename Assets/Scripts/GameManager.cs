using Assets.Scripts.Interfaces;
using DesignPatterns.Generics;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    IControllable _currentControllable;
    private InputSystem_Actions _inputs;

    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera carCamera;

    public override void Awake()
    {
        base.Awake();

        SetControllable(FindFirstObjectByType<PlayerController>());

        _inputs = new InputSystem_Actions();

        _inputs.Player.Move.performed += Move_performed;
        _inputs.Player.Move.canceled += Move_canceled;
        _inputs.Player.Interact.performed += Interact_performed;

        _inputs.Enable();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _currentControllable.Interact();
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _currentControllable.MoveCanceled();
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        _currentControllable.Move(obj.ReadValue<Vector2>());
    }

    public void SetControllable(IControllable controllable)
    {
        _currentControllable = controllable;
    }

    public void SwitchToPlayerCamera()
    { 
        carCamera.Priority = 0;
        playerCamera.Priority = 10;
    }

    public void SwitchToCarCamera()
    { 
        playerCamera.Priority = 0;
        carCamera.Priority = 10;
    }
}
