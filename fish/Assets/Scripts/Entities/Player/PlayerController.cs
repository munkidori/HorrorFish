using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;          
    public float rotationSpeed = 100f;    
    public float acceleration = 10f;      
    public float deceleration = 5f;      

    private Vector3 movementDirection;  
    private Rigidbody rb;                
    private Vector3 currentVelocity;
    private string currentScene;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
       currentScene = SceneManager.GetActiveScene().name;

    }

    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical"); 
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (currentScene == "River")
        {
            movementDirection = transform.forward * vertical;

            if (horizontal != 0f)
            {
                float rotation = horizontal * rotationSpeed * Time.deltaTime;
                transform.Rotate(0f, rotation, 0f);
            }

            Vector3 targetPosition = transform.position + new Vector3(0f, 10f, -10f);
        }
        else if (currentScene == "Hub")
        {
            Vector3 move = transform.right * horizontal + transform.forward * vertical;

            if (move.magnitude > 1f)
                move.Normalize();
            
            rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
        }
    }

    void FixedUpdate()
    {
        if (movementDirection.magnitude > 0.1f) 
            currentVelocity = Vector3.MoveTowards(currentVelocity, movementDirection * moveSpeed, acceleration * Time.fixedDeltaTime);
        else 
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }
}
