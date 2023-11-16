using UnityEngine;


public class Chair : MonoBehaviour
{
    public bool seatAvaliable = true;

    [SerializeField]
    private GameObject SpawnPointForBadAI = null;

    [SerializeField]
    private Table table;
    public AI ai;

    private SpriteRenderer chairSprite;
    private PolygonCollider2D chairCollider;

    private void Start()
    {
        chairSprite = GetComponent<SpriteRenderer>();
        chairCollider = GetComponent<PolygonCollider2D>();
    }

    public void FreeChair()
    {
        ai.transform.position = SpawnPointForBadAI.transform.position;
        ai.isLeaving = true;
        ai = null;
        seatAvaliable = true;
        chairCollider.enabled = true;
        chairSprite.enabled = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Customer")
            && collision.gameObject.GetComponent<AI>().state == AIState.Good)
        {
            ai = collision.gameObject.GetComponent<AI>();
            ai.rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            ai.isSit = true;
            ai.transform.position = transform.position;
            table.AddCostumer(this);
            seatAvaliable = false;
            chairSprite.enabled = false;
            table.plateSprite.enabled = true;
        }
    }
}
