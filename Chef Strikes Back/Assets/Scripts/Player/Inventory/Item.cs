using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Item : MonoBehaviour
{
    [Header("Item Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] public Collider2D tgrCollider;
    [SerializeField] public Collider2D childCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Space, Header("Item Info")]
    public FoodType Type;
    public bool IsServed;
    public bool IsPickable;

    [Space, Header("Throw Info")]
    [SerializeField] private float _timeReduction;
    private float _time;
    private Vector2 _acceleration;

    [Space, Header("Movement in table")]
    [SerializeField] private float _magnetSmoodTime;
    private Transform _magnetPos;
    private bool _isBeingDrag;

    [Space, Header("Particles and Colors")]
    [SerializeField] private ParticleSystem CollisionParticles;
    [SerializeField] private Color tomatoColor = Color.red;
    [SerializeField] private Color doughColor = Color.white;
    [SerializeField] private Color cheeseColor = Color.yellow;
    [SerializeField] private ParticleSystem pizzaParticlesPrefab;
    [SerializeField] private ParticleSystem spaghettiParticlesPrefab;
    [SerializeField] private Color[] finishedFoodColors;  // Array of colors for finished foods

    private void Start()
    {
        _isBeingDrag = false;
        IsPickable = true;
        IsServed = false;
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            transform.localPosition = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (_time >= 0)
        {
            _rb.velocity += _acceleration * Time.deltaTime;
            _time -= Time.deltaTime;

            CheckTime();
        }

        MagnetToTable();
        DraggingFood();
    }

    public void Throw(Vector2 velocity, Vector2 acceleration, float time)
    {
        _time = time * _timeReduction;
        _rb.velocity = velocity;
        _acceleration = acceleration;
        IsPickable = true;  // Assuming the item cannot be picked while it's being thrown

        SetParticleColor(); // Set the particle color based on the food type
    }

    private void CheckTime()
    {
        if (_time <= 0)
        {
            _rb.velocity = Vector2.zero;
            _rb.rotation = 0;
            _rb.angularVelocity = 0;
        }
    }

    private void MagnetToTable()
    {
        _isBeingDrag = _magnetPos != null;
    }

    public void LaunchedInTable(Transform table)
    {
        _magnetPos = table;
        _rb.isKinematic = true;
        _rb.freezeRotation = true;
    }

    public void DraggingFood()
    {
        if (_isBeingDrag)
        {
            var tempVelocity = _rb.velocity;
            transform.position = Vector2.SmoothDamp(transform.position, _magnetPos.position, ref tempVelocity, _magnetSmoodTime);
            _isBeingDrag = .2 <= Vector3.Distance(transform.position, _magnetPos.position);
        }
    }

    private void SetParticleColor()
    {
        ParticleSystem.MainModule mainModule = CollisionParticles.main;  // Get the main module
        switch (Type)
        {
            case FoodType.Tomato:
                mainModule.startColor = tomatoColor;
                break;
            case FoodType.Dough:
                mainModule.startColor = doughColor;
                break;
            case FoodType.Cheese:
                mainModule.startColor = cheeseColor;
                break;
            case FoodType.Pizza:
                // Use a specific particle prefab for pizza
                InstantiateParticle(pizzaParticlesPrefab);
                return;  // Exit the method to avoid playing the default particle system
            case FoodType.Spaghetti:
                // Use a specific particle prefab for spaghetti
                InstantiateParticle(spaghettiParticlesPrefab);
                return;  // Exit the method to avoid playing the default particle system
            default:
                mainModule.startColor = Color.white;  // Default color if none of the above
                break;
        }
        CollisionParticles.Play();
    }

    private void InstantiateParticle(ParticleSystem prefab)
    {
        if (prefab != null)
        {
            ParticleSystem instantiatedParticles = Instantiate(prefab, transform.position, Quaternion.identity);
            instantiatedParticles.Play();
            Destroy(instantiatedParticles.gameObject, instantiatedParticles.main.duration + 2.0f);
        }
        else
        {
            Debug.LogError("Particle prefab is not assigned.");
        }
    }

    public void CollidersState(bool state)
    {
        tgrCollider.enabled = state;
        childCollider.enabled = state;
        
    }

    public SpriteRenderer GetHighlight()
    {
        return _spriteRenderer;
    }

    private int _collisionCount = 0;
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Increase collision count on each collision
        _collisionCount++;
        if (_collisionCount == 2) // Assuming you want to trigger on every second collision
        {
            // Reset the collision count
            _collisionCount = 0;

            // Check if there are contact points and the CollisionParticles system is assigned
            if (collision.contactCount > 0 && CollisionParticles != null)
            {
                // Get the first contact point
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 collisionPoint = contact.point;

                // Instantiate the particle system at the collision point
                ParticleSystem instantiatedParticles = Instantiate(CollisionParticles, collisionPoint, Quaternion.identity);

                // Optionally, align the particle system with the collision normal
                instantiatedParticles.transform.rotation = Quaternion.FromToRotation(Vector3.forward, contact.normal);

                // Play the particle system
                instantiatedParticles.Play();

                // Destroy the particle system after it has finished
                Destroy(instantiatedParticles.gameObject, instantiatedParticles.main.duration + 2.0f);
            }
            else
            {
                Debug.LogError("CollisionParticles prefab is not assigned or no contact points available.");
            }
        }
    }
}

public enum FoodType
{
    Pizza,
    Spaghetti,
    Dough,
    Tomato,
    Cheese,
}
