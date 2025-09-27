using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Car
{
    public class CarControl : MonoBehaviour
    {
        [Header("Car Settings")]
        [SerializeField] float motorTorque = 2000f;
        [SerializeField] float brakeTorque = 2000f;
        [SerializeField] float maxSpeed = 20f;
        [SerializeField] float steeringRange = 30f;
        [SerializeField] float steeringRangeAtMaxSpeed = 10f;
        [SerializeField] float centreOfGravityOffset = -1f;

        [Header("Wheel Settings")]
        [SerializeField] WheelControl[] wheels;

        Rigidbody rigidBody;

        InputSystem_Actions inputActions;
        Vector2 inputVector;
        bool inputCanceled;
        private void Awake()
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.Move.performed += Move_performed;
            inputActions.Player.Move.canceled += Move_canceled;
        }

        private void Move_canceled(InputAction.CallbackContext obj)
        {
            inputVector = Vector3.Dot(transform.forward, rigidBody.linearVelocity) > 0 
                ? Vector2.down 
                : Vector2.up;
            inputCanceled = true;
        }

        private void Move_performed(InputAction.CallbackContext obj)
        {
            inputVector = obj.ReadValue<Vector2>();
            inputCanceled = false;
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }
        private void Update()
        {
            if (inputCanceled && rigidBody.linearVelocity.magnitude < 0.1f)
            {
                inputVector = Vector2.zero;
                inputCanceled = false;
            }
        }

        private void Start()
        {
            rigidBody = GetComponent<Rigidbody>();

            // Lower the center of gravity for better stability
            rigidBody.centerOfMass += new Vector3(0, centreOfGravityOffset, 0);
        }

        private void FixedUpdate()
        {
            // Get the current input values
            float verticalInput = inputVector.y;
            float horizontalInput = inputVector.x;

            // Calculate the current speed along the forward direction
            float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.linearVelocity);
            // Adjust steering sensitivity based on speed
            float speedFactor = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(forwardSpeed));

            // Calculate and apply motor and steering values to each wheel
            float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
            float currentSteeringRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

            bool isAccelerating = 
                Mathf.Sign(verticalInput) == Mathf.Sign(forwardSpeed);

            foreach (var wheel in wheels)
            {
                // apply steering to wheel that supports steering
                if (wheel.Steerable)
                {
                    wheel.WheelCollider.steerAngle = 
                        horizontalInput * currentSteeringRange;
                }

                if (isAccelerating)
                {
                    // apply motor torque to wheel that supports motor
                    if (wheel.Motorized)
                    {
                        wheel.WheelCollider.motorTorque = 
                            verticalInput * currentMotorTorque;
                    }
                    // release brake torque when accelerating
                    wheel.WheelCollider.brakeTorque = 0f;
                }
                else
                {
                    // apply brake torque when not accelerating
                    wheel.WheelCollider.motorTorque = 0f;
                    wheel.WheelCollider.brakeTorque = 
                        Mathf.Abs(verticalInput) * brakeTorque;
                }
            }
        }
    }
}
