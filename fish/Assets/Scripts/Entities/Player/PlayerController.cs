using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;          
    public float rotationSpeed = 100f;    
    public float acceleration = 10f;      
    public float deceleration = 5f;      

    private Vector3 movementDirection;  
    private Rigidbody rb;                
    private Vector3 currentVelocity;     

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float vertical = Input.GetAxisRaw("Vertical"); 
        float horizontal = Input.GetAxisRaw("Horizontal"); 

        movementDirection = transform.forward * vertical;

        if (horizontal != 0f)
        {
            float rotation = horizontal * rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotation, 0f);
        }

        Vector3 targetPosition = transform.position + new Vector3(0f, 10f, -10f);
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
