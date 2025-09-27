using UnityEngine;

namespace Assets.Scripts.Car
{
    public class WheelControl : MonoBehaviour
    {
        public Transform WheelGraphics;

        [HideInInspector] public WheelCollider WheelCollider;

        public bool Steerable;
        public bool Motorized;

        Vector3 position;
        Quaternion rotation;

        private void Awake()
        {
            WheelCollider = GetComponent<WheelCollider>();
        }

        private void Update()
        {
            // obtain the position and rotation of the wheel collider
            WheelCollider.GetWorldPose(out position, out rotation);

            // apply the position and rotation to the wheel graphics
            WheelGraphics.position = position;
            WheelGraphics.rotation = rotation;
        }
    }
}
