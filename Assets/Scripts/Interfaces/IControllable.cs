using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IControllable
    {
        void Move(Vector2 direction);
        void MoveCanceled();
        void Interact();
    }
}