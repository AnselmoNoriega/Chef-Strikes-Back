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
    [SerializeField] private TrailRenderer _trailRenderer;

    private AudioManager _audioManager;

    private void Start()
    {
        _isBeingDrag = false;
        IsPickable = true;
        IsServed = false;
        InitializeTrailRenderer();

        // Initialize the audio manager
        _audioManager = ServiceLocator.Get<AudioManager>();
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

        _collisionCount++;

        if (_collisionCount == 2)
        {
            _collisionCount = 0;

            if (collision.contactCount > 0)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 collisionPoint = contact.point;
                TriggerParticles(collisionPoint, contact.normal);
            }
            else
            {
                Debug.LogError("No contact points available for collision.");
            }

            // Play bounce sound based on the food type
            PlayBounceSound();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.contactCount > 0)
            {
                ContactPoint2D contact = collision.GetContact(0);
                Vector2 collisionPoint = contact.point;
                TriggerParticles(collisionPoint, contact.normal);

                // Play bounce sound based on the food type
                PlayBounceSound();
            }
        }
    }

    private void PlayBounceSound()
    {
        int randomIndex = Random.Range(0, 3); // Random index between 0 and 2
        string soundClipName = $"{Type}Bounce_{randomIndex:D2}"; // Formatted as "TypeBounce_00" to "TypeBounce_02"
        _audioManager.PlaySource(soundClipName);
        Debug.Log($"Playing sound: {soundClipName}");
    }

    private void TriggerParticles(Vector2 position, Vector3 normal)
    {
        if (CollisionParticles != null)
        {
            ParticleSystem instantiatedParticles = Instantiate(CollisionParticles, position, Quaternion.identity);
            instantiatedParticles.transform.rotation = Quaternion.FromToRotation(Vector3.forward, normal);
            instantiatedParticles.Play();
            Destroy(instantiatedParticles.gameObject, instantiatedParticles.main.duration + 2.0f);
        }
        else
        {
            Debug.LogError("CollisionParticles prefab is not assigned.");
        }
    }

    private void InitializeTrailRenderer()
    {
        if (_trailRenderer == null)
        {
            _trailRenderer = gameObject.AddComponent<TrailRenderer>();
        }

        _trailRenderer.time = 0.2f;  // Trail length in seconds
        _trailRenderer.startWidth = 0.2f;  // Start width of the trail
        _trailRenderer.endWidth = 0.01f;  // End width of the trail
        _trailRenderer.material = new Material(Shader.Find("Sprites/Default"));  // Assign a basic material

        // Set the color of the trail based on the food type
        switch (Type)
        {
            case FoodType.Tomato:
                _trailRenderer.startColor = new Color(tomatoColor.r, tomatoColor.g, tomatoColor.b, 0.5f); // Fully opaque start color
                _trailRenderer.endColor = new Color(tomatoColor.r, tomatoColor.g, tomatoColor.b, 0f); // Transparent end color
                break;
            case FoodType.Dough:
                _trailRenderer.startColor = new Color(doughColor.r, doughColor.g, doughColor.b, 0.5f); // Fully opaque start color
                _trailRenderer.endColor = new Color(doughColor.r, doughColor.g, doughColor.b, 0f); // Transparent end color
                break;
            case FoodType.Cheese:
                _trailRenderer.startColor = new Color(cheeseColor.r, cheeseColor.g, cheeseColor.b, 0.5f); // Fully opaque start color
                _trailRenderer.endColor = new Color(cheeseColor.r, cheeseColor.g, cheeseColor.b, 0f); // Transparent end color
                break;
            default:
                _trailRenderer.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Fully opaque white start color
                _trailRenderer.endColor = new Color(1.0f, 1.0f, 1.0f, 0f); // Transparent white end color
                break;
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
    None
}
