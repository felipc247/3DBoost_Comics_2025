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

    public void SitCompleted()
    {
        playerController.SitCompleted();
    }

    public void SetColliderTrigger()
    {
        playerController.SetCollliderTrigger();
    }

    public void SetColliderSolid()
    {
        playerController.SetColliderSolid();
    }
}
