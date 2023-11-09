using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public AudioSource source;
    public AudioClip shove;

    [SerializeField]
    [Range(0, 3)]
    private float maxSpeed = 1.0f;
    [SerializeField]
    private float acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        source.clip = shove;
    }

    private void FixedUpdate()
    {
        if (MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb2d.velocity = oldMovementInput * currentSpeed;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            source.clip = shove;
            source.Play();
        }
           
    }

}
