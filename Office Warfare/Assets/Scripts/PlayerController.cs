using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed, gravityModifier, jumpPower;
    public CharacterController charCon;
    public Transform camTransform;
    public float mouseSensitivity;
    public Animator animator;

    private float verticalVelocity;
    private Vector3 moveInput;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private bool canJump;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        charCon = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        float yStore = moveInput.y;

        Vector3 vertMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();
        moveInput = moveInput * moveSpeed;
        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;

        if (charCon.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * gravityModifier * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpPower; // Apply jump force
            }
        }
        else
        {
            verticalVelocity += Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        moveInput.y = verticalVelocity; // Apply jump and gravity
        charCon.Move(moveInput * Time.deltaTime);

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGround).Length  >0 ;
        

        //Jumping
        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpPower;
        }


        charCon.Move(moveInput * Time.deltaTime);


        //Cam rotation
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTransform.rotation = Quaternion.Euler(camTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        
    }

}
