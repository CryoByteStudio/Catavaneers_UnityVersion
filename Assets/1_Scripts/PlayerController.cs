using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MoveStates
    {
        Movement,
        Freeze,
        Dodge
    }

    public enum DodgeDirection
    {
        Forward,
        Backward,
        Left,
        Right,
        none
    }
    [SerializeField] float speed = 0.0f;
    [SerializeField] float dodgeSpeed = 3.15f;
    [SerializeField] float dodgeTime = 0.0f;
    [SerializeField] float straffSensitiviy = 30.0f;

    Vector3 LTumbInput = new Vector3(0,0,0);
    Vector3 RTumbInput = new Vector3(0, 0, 0);
    float leftInputMagnitud = 0.0f;
    float rightinputMagnitud = 0.0f;
    float characterRotation = 0.0f;
    MoveStates states = MoveStates.Movement;
    DodgeDirection dodgeDirection = DodgeDirection.none;
    HealthComp health;
    float weaponWeight = 1;

    bool freeze = false;
    [SerializeField] float reverseValue = 1;
    [SerializeField] float slowValue = 1;


    public string inputHorizontalLeftThumb;
    public string inputVerticalLeftThumb;
    public string inputHorizontalRightThumb;
    public string inputVerticalRightThumb;

    private void Start()
    {
        health = GetComponent<HealthComp>();
    }

    void Update()
    {

        if (!health.IsDead())
        {
            switch(states)
            {
                case MoveStates.Dodge:
                    LTumbInput = Vector3.zero;
                    RTumbInput = Vector3.zero;
                    Dodge();
                    break;
                case MoveStates.Freeze:
                    LTumbInput = Vector3.zero;
                    RTumbInput = Vector3.zero;
                    leftInputMagnitud = 0;
                    GetComponent<Animator>().SetFloat("Walk", leftInputMagnitud);
                    break;
                default:
                    AxisInput();
                    break;
            }
            CharacterMove(weaponWeight, reverseValue, slowValue);
        }
    }
    private void AxisInput()
    {
            LTumbInput = new Vector3(Input.GetAxis(inputHorizontalLeftThumb), 0, Input.GetAxis(inputVerticalLeftThumb));
            RTumbInput = new Vector3(Input.GetAxis(inputHorizontalRightThumb), 0, Input.GetAxis(inputVerticalRightThumb));        
        Rotation();
        Direction();
    }
    private void CharacterMove(float weight, float reverse, float slow)
    {
        leftInputMagnitud = LTumbInput.magnitude;
        float movementFraction = (speed * reverse* leftInputMagnitud)/weight;
        movementFraction = movementFraction / slow;
        GetComponent<Animator>().SetFloat("Y input", Input.GetAxis(inputVerticalLeftThumb));
        GetComponent<Animator>().SetFloat("X input", Input.GetAxis(inputHorizontalLeftThumb));
        transform.position += LTumbInput * Time.deltaTime * movementFraction;
    }
    void Dodge()
    {
        if(dodgeDirection == DodgeDirection.Backward)
        {
            transform.position += Vector3.back * 3.15f*Time.deltaTime;
        }
        if (dodgeDirection == DodgeDirection.Right)
        {
            transform.position += Vector3.right * 3.15f * Time.deltaTime;
        }
        if (dodgeDirection == DodgeDirection.Left)
        {
            transform.position += Vector3.left * 3.15f * Time.deltaTime;
        }
    }
    private void Rotation()
    {
        if (RTumbInput != Vector3.zero)
        {
            characterRotation = Mathf.Atan2(Input.GetAxis(inputHorizontalRightThumb), Input.GetAxis("Vertical Right Thumbstick")) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, characterRotation, 0));
            //GetComponent<Fighter>().UpdateRaycastOrientation(characterRotation);
        }
    }
    //function that tells the animator if players is strafing and the direction
    private void Direction()
    {
        float curAngle = Vector3.Angle(LTumbInput.normalized, transform.forward);

        float clockwise = angleDir(transform.forward, LTumbInput.normalized, Vector3.up);
        if (curAngle < straffSensitiviy)
        {
            GetComponent<Animator>().SetInteger("Strafe", 0);
        }
        if ((curAngle > straffSensitiviy && curAngle < 180 - straffSensitiviy && clockwise < 0))
        {
            GetComponent<Animator>().SetInteger("Strafe", -1);
        }
        if ((curAngle > straffSensitiviy && curAngle < 180- straffSensitiviy && clockwise > 0))
        {
            GetComponent<Animator>().SetInteger("Strafe", 1);
        }
        if ((curAngle > 180 - straffSensitiviy))
        {
            GetComponent<Animator>().SetInteger("Strafe", 0);
        }
    }
    public void SetWeaponWeight(float currentWeapon)
    {
        weaponWeight = currentWeapon;
    }

    //complements the Direction function, tells if the angle of straffing in clockwise or counter clockwise
    public float angleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {

        Vector3 perp = Vector3.Cross(fwd, targetDir);

        float dir = Vector3.Dot(perp, up);

        if (dir > 0.0)
        {
            return 1.0f;
        }
        else if (dir < 0.0)
        {
            return -1.0f;
        }
        else
        {
            return 0.0f;
        }
    }

    public void HitByTrap(float reverseTrapValue, float slowTrapValue, float lastingTime)
    {
        reverseValue = reverseTrapValue;
        slowValue = slowTrapValue;
        StartCoroutine(UndoAfliction(lastingTime));
    }

    private IEnumerator UndoAfliction(float time)
    {
        yield return new WaitForSeconds(time);
        states = MoveStates.Movement;
        reverseValue = 1;
        slowValue = 1;
    }

    public MoveStates GetMoveState()
    {
        return states;
    }
    public void SetMoveState(MoveStates state)
    {
        states = state;
    }
    public void SetFreeze(float lastingTime)
    {
        states = MoveStates.Freeze;
        leftInputMagnitud = 0;
        StartCoroutine(UndoAfliction(lastingTime));
    }
    void EnterRollBackwards()
    {
        Debug.Log("enter dodge");
        states = MoveStates.Dodge;
        dodgeDirection = DodgeDirection.Backward;
    }
    void ExitRollBackwards()
    {
        Debug.Log("exit dodge");
        states = MoveStates.Movement;
        dodgeDirection = DodgeDirection.none;
    }
    void EnterRollLeft()
    {
        Debug.Log("enter dodge");
        states = MoveStates.Dodge;
        dodgeDirection = DodgeDirection.Left;
    }
    void ExitRollLeft()
    {
        Debug.Log("exit dodge");
        states = MoveStates.Movement;
        dodgeDirection = DodgeDirection.none;
    }
    void EnterRollRight()
    {
        Debug.Log("enter dodge");
        states = MoveStates.Dodge;
        dodgeDirection = DodgeDirection.Right;
    }
    void ExitRollRight()
    {
        Debug.Log("exit dodge");
        states = MoveStates.Movement;
        dodgeDirection = DodgeDirection.none;
    }
}
