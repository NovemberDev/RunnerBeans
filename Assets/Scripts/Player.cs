using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public float Gravity = 10.0f;

    public float MovementSpeed = 1.0f;

    public float turnSpeed = 2.0f;

    public float HorizontalMovementSpeed = 10.0f;

    private CharacterController controller;

    private Ground currentGround;

    private Vector3 direction = Vector3.zero;

    public Vector3 horizontalInput = Vector3.zero;

    private bool rotateHandled, isJumping = false;

    private RaycastHit groundRay, collisionRay;

    private Quaternion desiredRotation;

    private Animator anim;

    private float jumpAccel = 0.0f;

    private float gameStartTimeOut = 2.0f;

    private bool gameOver = false;

    public int Score = 0;

    private Vector2 touchOrigin = -Vector2.one;

    void Start ()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        desiredRotation = controller.transform.rotation;
    }

    private void FixedUpdate()
    {
        Score += (int)(controller.velocity.sqrMagnitude * 0.01f);
        GameManager.Instance.SetScoreText(Score);
    }

    void Update ()
    {
        if(gameOver)
        {
            return;
        }

#if UNITY_STANDALONE || UNITY_EDITOR_WIN
        horizontalInput.x = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            jumpAccel = 0.51f;
            isJumping = true;
        }

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        if (Input.touchCount > 0) {
            Touch myTouch = Input.touches[0];

            if (myTouch.phase == TouchPhase.Began)
            {
                touchOrigin = myTouch.position;
            }
            else if (myTouch.phase == TouchPhase.Moved || myTouch.phase == TouchPhase.Stationary && touchOrigin != myTouch.position)
            {
                Vector2 touchResult = myTouch.position - touchOrigin;

                if (Mathf.Abs(touchResult.x) > Mathf.Abs(touchResult.y))
                {
                    horizontalInput.x = touchResult.x > 0 ? 1 : -1;
                }
                else 
                {
                    if((touchResult.y > 0 ? 1 : -1) > 0 && !isJumping) 
                    {
                        jumpAccel = 0.51f;
                        isJumping = true;
                    }
                }
            }
            else if (myTouch.phase == TouchPhase.Ended) 
            {
                horizontalInput.x = 0;
            }
        }

#endif
        direction = transform.forward * Time.deltaTime * MovementSpeed * 2.0f;

        direction.y -= Gravity * Time.deltaTime;

        direction.y += jumpAccel;

        MovementSpeed += Time.deltaTime * 0.03f;

        turnSpeed += Time.deltaTime * 0.03f;

        HorizontalMovementSpeed += Time.deltaTime * 0.01f;
         
        if(HorizontalMovementSpeed > 15.0f)
        {
            HorizontalMovementSpeed = 15.0f;
        }

        if (turnSpeed > 5.0f)
        {
            turnSpeed = 5.0f;
        }

        if(MovementSpeed > 50.0f)
        {
            MovementSpeed = 50.0f;
        }

        GameManager.Instance.SetSpeedText(MovementSpeed);

        if (jumpAccel >= 0.0f)
        {
            jumpAccel -= Time.deltaTime;
        }

        if(gameStartTimeOut > -0.1f)
        {
            gameStartTimeOut -= Time.deltaTime;
        }

        if (Physics.Raycast(transform.position, -Vector3.up, out groundRay, 2.0f))
        {
            if (currentGround != groundRay.collider.GetComponentInParent<Ground>())
            {
                currentGround = groundRay.collider.GetComponentInParent<Ground>();
                GameManager.Instance.SpawnGround();
                rotateHandled = false;
                isJumping = false;
            }
        }

        if(Physics.Raycast(transform.position - Vector3.up, transform.forward, out collisionRay, 2.0f))
        {
            if(controller.velocity.sqrMagnitude <= 2.0f && gameStartTimeOut < 0.0f)
            {
                anim.SetTrigger("crash");
                gameOver = true;
                GameManager.Instance.GameOver();
            }
        }

        if (currentGround != null && !rotateHandled && currentGround.GroundType != GroundType.Forward)
        {
            if(horizontalInput.x > 0.1f)
            {
                desiredRotation = Quaternion.Euler(desiredRotation.eulerAngles + new Vector3(0, 90, 0));
                rotateHandled = true;
            }
            else if (horizontalInput.x < -0.1f)
            {
                desiredRotation = Quaternion.Euler(desiredRotation.eulerAngles + new Vector3(0, -90, 0));
                rotateHandled = true;
            }
        }
        else
        {
            direction += (Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * horizontalInput) * HorizontalMovementSpeed / 5.0f;
        }

        controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, desiredRotation, turnSpeed * Time.deltaTime);

        controller.Move(direction);

        anim.SetBool("jumping", isJumping);
        anim.SetFloat("speed", controller.velocity.sqrMagnitude);

        if (transform.position.y <= 5.0f)
        {
            Debug.Log("Lose condition detected " + transform.position.y);
            gameOver = true;
            GameManager.Instance.GameOver();
        }

        if (controller.velocity.y <= -1.0f)
        {
            anim.SetBool("jumping", true);
        }
            // TODO: Loss condition on velocity almost 0
            // Debug.Log(controller.velocity);
    }

    public void Jump()
    {
        if(!isJumping)
        {
            jumpAccel = 0.51f;
            isJumping = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position - Vector3.up, transform.forward);
    }
}
