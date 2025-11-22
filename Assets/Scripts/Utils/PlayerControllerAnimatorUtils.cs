using UnityEngine;

public class PlayerControllerAnimatorUtils : MonoBehaviour
{
    PlayerController playerController;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    public void SitIntoCar()
    {
        playerController.SitIntoCar();
    }

    public void DisableAgent()
    {
        playerController.DisableAgent();
    }

    public void SetColliderTrigger()
    {
        playerController.SetColliderTrigger();
    }
    public void SetColliderSolid()
    {
        playerController.SetColliderSolid();
    }

    public void ExitFromCar()
    {
        playerController.ExitFromCar();
    }
}
