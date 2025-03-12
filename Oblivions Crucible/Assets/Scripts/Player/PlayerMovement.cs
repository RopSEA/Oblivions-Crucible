using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] 
    private float BASE_SPEED = 5;
    public StaminaBar stamina;
    private Rigidbody2D rb;
    float currentSpeed;
    private Vector3 slideDir;
    private float slideSpeed;
    public float cooldown;
    private float lastDodge;

    private State state;
    private enum State
    {
        Normal,
        Roll
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = BASE_SPEED;
        lastDodge = Time.time;
        state = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Normal:
                move();
                dodgeRoll();
                break;

            case State.Roll:
                dodgeSlide();
                break;
        }
       

    }

    public Vector3 getSlideDir()
    {
        return slideDir;
    }

    private void dodgeRoll()
    {
        if (Input.GetKeyDown("space"))
        {

            int can = stamina.UseStamina(20);
            if (Time.time - lastDodge < cooldown || can == -1)
            {
                return;
            }

            lastDodge = Time.time;
            state = State.Roll;
            slideSpeed = 35.5f;
        }
    }

    private void dodgeSlide()
    {

        transform.position += slideDir * slideSpeed * Time.deltaTime;
        slideSpeed -= slideSpeed * 10f * Time.deltaTime;
        if (slideSpeed < 5f)
        {
            state = State.Normal;
        }
    }
    
    private void move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, vertical, 0).normalized;
        if (horizontal == 0 && vertical == 0)
        {
            rb.velocity = Vector2.zero;
            slideDir = new Vector3(1, 0, 0).normalized;
        }
        else
        {
            slideDir = dir.normalized;
        }
        
        rb.velocity = dir * currentSpeed;
    }
    
    
    public IEnumerator SpeedChange(float newSpeed, float timeInSecs)
    {
        currentSpeed = newSpeed;
        yield return new WaitForSeconds(timeInSecs);
        currentSpeed = BASE_SPEED;
    }

}

