using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Item : MonoBehaviour
{
    [Header("Item Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] public Collider2D tgrCollider;
    [SerializeField] public Collider2D childCollider;

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

            Checktime();
        }

        MagnetToTable();

        DraggingFood();
    }

    public void Throw(Vector2 velocity, Vector2 acceleration, float time)
    {
        _time = time * _timeReduction;
        _rb.velocity = velocity;
        _acceleration = acceleration;
        IsPickable = true;
    }

    private void Checktime()
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
            var temp = _rb.velocity;
            transform.position = Vector2.SmoothDamp(transform.position, _magnetPos.position, ref temp, _magnetSmoodTime);
            _isBeingDrag = .2 <= Vector3.Distance(transform.position, _magnetPos.position);
        }
    }

    public void CollidersState(bool state)
    {
        tgrCollider.enabled = state;
        childCollider.enabled = state;
    }
}

public enum FoodType
{
    Pizza,
    Spaghetti,
    Dough,
    Tomatoe,
    Cheese,
}
