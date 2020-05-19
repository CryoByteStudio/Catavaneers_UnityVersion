using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCont : MonoBehaviour
{
    public void GetInput()
    {
       // m_horizontalInput = Input.GetAxis("Horizontal");
       // m_verticalInput = Input.GetAxis("Vertical");
    }

    private void Steer()
    {
        m_steeringAngle = maxAngle * m_horizontalInput;
        RF.steerAngle = m_steeringAngle;
        LF.steerAngle = m_steeringAngle;

    }

    private void Acelerate()
    {
        RF.motorTorque = m_verticalInput * force;
        LF.motorTorque = m_verticalInput * force;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(RF, RFT);
        UpdateWheelPose(LF, LFT);
        UpdateWheelPose(RB, RBT);
        UpdateWheelPose(LB, LBT);
    }

    private void UpdateWheelPose(WheelCollider _collider,Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    
    {
        GetInput();
        Steer();
        Acelerate ();
        UpdateWheelPoses();

    }







    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider RF, LF;
    public WheelCollider RB, LB;
    public Transform RFT, LFT;
    public Transform RBT, LBT;
    public float maxAngle = 30;
    public float force = 50;



   
}