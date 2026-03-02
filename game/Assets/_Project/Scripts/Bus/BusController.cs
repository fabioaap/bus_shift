using UnityEngine;

namespace BusShift.Bus
{
    [RequireComponent(typeof(Rigidbody))]
    public class BusController : MonoBehaviour
    {
        [Header("Wheel Colliders")]
        public WheelCollider FrontLeft;
        public WheelCollider FrontRight;
        public WheelCollider RearLeft;
        public WheelCollider RearRight;

        [Header("Wheel Meshes")]
        public Transform FrontLeftMesh;
        public Transform FrontRightMesh;
        public Transform RearLeftMesh;
        public Transform RearRightMesh;

        [Header("Physics")]
        public float MotorTorque = 4000f;
        public float BrakeTorque = 8000f;
        public float MaxSteerAngle = 25f;
        public float MaxSpeed = 22f; // ~80 km/h

        private Rigidbody _rb;
        private float _inputV, _inputH;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.mass = 5000f;
            _rb.linearDamping = 0.1f;
            _rb.angularDamping = 0.5f;
            _rb.centerOfMass = new Vector3(0f, -0.8f, 0f);
        }

        void Update()
        {
            _inputV = Input.GetAxis("Vertical");
            _inputH = Input.GetAxis("Horizontal");
        }

        void FixedUpdate()
        {
            ApplyMotorTorque();
            ApplySteering();
            ApplyBrakes();
            UpdateWheelMeshes();
        }

        private void ApplyMotorTorque()
        {
            float speed = _rb.linearVelocity.magnitude;
            if (speed < MaxSpeed)
            {
                RearLeft.motorTorque  = _inputV * MotorTorque;
                RearRight.motorTorque = _inputV * MotorTorque;
            }
            else
            {
                RearLeft.motorTorque  = 0f;
                RearRight.motorTorque = 0f;
            }
        }

        private void ApplySteering()
        {
            float steer = _inputH * MaxSteerAngle;
            FrontLeft.steerAngle  = steer;
            FrontRight.steerAngle = steer;
        }

        private void ApplyBrakes()
        {
            if (Mathf.Approximately(_inputV, 0f))
            {
                FrontLeft.brakeTorque  = BrakeTorque * 0.3f;
                FrontRight.brakeTorque = BrakeTorque * 0.3f;
                RearLeft.brakeTorque   = BrakeTorque * 0.3f;
                RearRight.brakeTorque  = BrakeTorque * 0.3f;
            }
            else
            {
                FrontLeft.brakeTorque  = 0f;
                FrontRight.brakeTorque = 0f;
                RearLeft.brakeTorque   = 0f;
                RearRight.brakeTorque  = 0f;
            }
        }

        private void UpdateWheelMeshes()
        {
            UpdateWheelMesh(FrontLeft,  FrontLeftMesh);
            UpdateWheelMesh(FrontRight, FrontRightMesh);
            UpdateWheelMesh(RearLeft,   RearLeftMesh);
            UpdateWheelMesh(RearRight,  RearRightMesh);
        }

        private void UpdateWheelMesh(WheelCollider col, Transform mesh)
        {
            if (mesh == null) return;
            col.GetWorldPose(out Vector3 pos, out Quaternion rot);
            mesh.position = pos;
            mesh.rotation = rot;
        }
    }
}
