using System;
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
    [SerializeField] float straffSensitiviy = 30.0f;
    public Animator animator = null;
    public float dodgeCoolDown = 2;
   Vector3 LTumbInput = new Vector3(0,0,0);
    Vector3 RTumbInput = new Vector3(0, 0, 0);
    float leftInputMagnitud = 0.0f;
    float rightinputMagnitud = 0.0f;
    float characterRotation = 0.0f;
    MoveStates states = MoveStates.Movement;
    [SerializeField] DodgeDirection dodgeDirection = DodgeDirection.none;
    HealthComp health;
    float weaponWeight = 1;
    float timeSinceLastDodge = -Mathf.Infinity;
    private bool isDodging;
    public bool IsDodging => isDodging;

    bool freeze = false;
    [SerializeField] float reverseValue = 1;
    [SerializeField] float slowValue = 1;

    public Rigidbody rb;
    public string inputHorizontalLeftThumb;
    public string inputVerticalLeftThumb;
    public string inputHorizontalRightThumb;
    public string inputVerticalRightThumb;
    public string dodgeButton;
    public float xmin= -35f;
    public float xmax= 49f;
    public float zmin= -10f;
    public float zmax= 41f;
    public float ymin = -2f;
    public float ymax = 30f;
    public float constraintbuffer=2f;

    public ParticleSystem FreezeEffect;
    public ParticleSystem DamageEffect;
    public ParticleSystem SlowEffect;
    public ParticleSystem ReverseEffect;
    private void Start()
    {
        health = GetComponent<HealthComp>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //constraints for the player
        if (transform.position.y < ymin)
        {
           
            transform.position = new Vector3(transform.position.x, ymin + constraintbuffer, transform.position.z);
           // rb.velocity = Vector3.zero;
        }
        if (transform.position.y > ymax)
        {
            
            transform.position = new Vector3(transform.position.x, ymax - constraintbuffer, transform.position.z);
           // rb.velocity = Vector3.zero;
        }
        if (transform.position.x < xmin)
        {
           
            transform.position = new Vector3(xmin + constraintbuffer, transform.position.y, transform.position.z);
           // rb.velocity = Vector3.zero;
        }
        if (transform.position.x >xmax)
        {
            
            transform.position = new Vector3(xmax - constraintbuffer, transform.position.y, transform.position.z);
           // rb.velocity = Vector3.zero;
        }
        if (transform.position.z <zmin )
        {
          
            transform.position = new Vector3(transform.position.x, transform.position.y, zmin + constraintbuffer);
          //  rb.velocity = Vector3.zero;
        }
        if (transform.position.z > zmax)
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y, zmax- constraintbuffer);
            //rb.velocity = Vector3.zero;
        }

        if (!health.IsDead())
        {
            switch(states)
            {
                case MoveStates.Dodge:
                    LTumbInput = Vector3.zero;
                    RTumbInput = Vector3.zero;
                    Dodge(); 
                    isDodging = true;
                    break;
                case MoveStates.Freeze:
                    LTumbInput = Vector3.zero;
                    RTumbInput = Vector3.zero;
                    leftInputMagnitud = 0;
                    animator.SetFloat("Walk", leftInputMagnitud);
                    break;
                default:
                    AxisInput();
                    isDodging = false;
                    break;
            }
            CharacterMove(weaponWeight, reverseValue, slowValue);
            if (Input.GetAxis(dodgeButton) != 0 && timeSinceLastDodge < Time.time)
            {
                states = MoveStates.Dodge;
                timeSinceLastDodge = Time.time + dodgeCoolDown;               
            }
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
        if (dodgeDirection == DodgeDirection.Backward) animator.SetFloat("Walk", -leftInputMagnitud);
        else animator.SetFloat("Walk", leftInputMagnitud);

        transform.position += LTumbInput * Time.deltaTime * movementFraction;
    }
    void Dodge()
    {
        switch (dodgeDirection)
        {
            case DodgeDirection.Forward:
                transform.Translate(Vector3.forward * dodgeSpeed * Time.deltaTime, Space.Self);
                animator.SetInteger("Roll",2);
                break;
            case DodgeDirection.Backward:
                transform.Translate(Vector3.back * dodgeSpeed * Time.deltaTime, Space.Self);
                animator.SetInteger("Roll", 1);
                break;
            case DodgeDirection.Left:
                transform.Translate(Vector3.left * dodgeSpeed * Time.deltaTime, Space.Self);
                animator.SetInteger("Roll", 3);
                break;
            case DodgeDirection.Right:
                transform.Translate(Vector3.right * dodgeSpeed * Time.deltaTime, Space.Self);
                animator.SetInteger("Roll", 4);
                break;
            default:
                break;
        }
    }

    public void ResetValue()
    {
        animator.SetInteger("Roll", 0);
        states = MoveStates.Movement;
    }
    private void Rotation()
    {
        if (RTumbInput != Vector3.zero)
        {
            characterRotation = Mathf.Atan2(Input.GetAxis(inputHorizontalRightThumb), Input.GetAxis(inputVerticalRightThumb)) * Mathf.Rad2Deg;
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
            animator.SetFloat("Strafe", 0);
            dodgeDirection = DodgeDirection.Forward;
        }
        if ((curAngle > straffSensitiviy && curAngle < 180 - straffSensitiviy && clockwise < 0))
        {
            animator.SetFloat("Strafe", -1);
            dodgeDirection = DodgeDirection.Left;
        }
        if ((curAngle > straffSensitiviy && curAngle < 180- straffSensitiviy && clockwise > 0))
        {
            animator.SetFloat("Strafe", 1);
            dodgeDirection = DodgeDirection.Right;
        }
        if ((curAngle > 180 - straffSensitiviy))
        {
            animator.SetFloat("Strafe", 0);
            dodgeDirection = DodgeDirection.Backward;
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

    // ANIMATION EVENTS
    private void FootL()
    {
        //TODO Add Particle FX
    }

    private void FootR()
    {
        //TODO Add Particle FX
    }
    //
}
