using UnityEngine;

public class Item : MonoBehaviour
{
    public Rigidbody2D rb;
    public ItemType type;

    [SerializeField]
    private float timereduction;

    private float time;
    private Vector2 acceleration;
    private Vector2 handPosition;

    [Space, Header("Movment in table"), SerializeField]
    private float magnetSmoodTime;
    private Transform magnetPos;
    private bool isBeingDrag;
    public bool isPickable;

    public bool isServed;

    private void Start()
    {
        handPosition = new Vector2(0, 0.7f);
        isBeingDrag = false;
        isPickable = true;
        isServed = false;
    }

    private void FixedUpdate()
    {

        if (transform.parent != null)
        {
            transform.localPosition = handPosition;
        }

        if (time >= 0)
        {
            rb.velocity += acceleration * Time.deltaTime;
            time -= Time.deltaTime;

            Checktime();
        }

        MagnetToTable();

        DraggingFood();
    }

    public void Throw(Vector2 velocity, Vector2 acceleration, float time)
    {
        this.time = time * timereduction;
        rb.velocity = velocity;
        this.acceleration = acceleration;
        isPickable = true;
    }

    private void Checktime()
    {
        if (time <= 0)
        {
            rb.velocity = Vector2.zero;
            rb.rotation = 0;
            rb.angularVelocity = 0;
        }
    }

    private void MagnetToTable()
    {
        isBeingDrag = magnetPos != null;
    }

    public void LaunchedInTable(Transform table)
    {
        magnetPos = table;
    }

    public void DraggingFood()
    {
        if (isBeingDrag)
        {
            var temp = rb.velocity;
            transform.position = Vector2.SmoothDamp(transform.position, magnetPos.position, ref temp, magnetSmoodTime);
            isBeingDrag = .2 <= Vector3.Distance(transform.position, magnetPos.position);
        }
    }

    public void DestoyItem()
    {
        Destroy(gameObject);
    }

}

public enum ItemType
{
    BurgerBun,
    Meat,
    Lettuce,
    Burger
}
