using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsTable : MonoBehaviour
{
    [SerializeField]
    private GameObject item;
    public GameObject currentItem;

    private void Start()
    {
        if(currentItem == null)
        {
            currentItem = Instantiate(item, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == currentItem)
        {
            currentItem = Instantiate(item, transform.position, Quaternion.identity);
        }
    }
}
