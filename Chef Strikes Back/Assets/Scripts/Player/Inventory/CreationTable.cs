using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class CreationTable : MonoBehaviour
{
    [Header("Storage Info")]
    [SerializeField]
    private bool[] count;
    [SerializeField]
    private List<GameObject> items;
    private List<GameObject> waitList;

    [Space, Header("Storage Objects")]
    [SerializeField]
    private GameObject burger;
    [SerializeField]
    private Transform magnet;

    private void Start()
    {
        waitList = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem != null)
        {
            if (count[(int)recivedItem.type] == false)
            {
                items[(int)recivedItem.type] = recivedItem.gameObject;
                count[(int)recivedItem.type] = true;
                recivedItem.LaunchedInTable(magnet);
            }
            else if(!items.Contains(recivedItem.gameObject) && !waitList.Contains(recivedItem.gameObject))
            {
                waitList.Add(recivedItem.gameObject);
            }

            if(!ItemIsMissing())
            {
                GiveMeBurger();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem != null)
        {
            if (recivedItem.gameObject == items[(int)recivedItem.type])
            {
                items[(int)recivedItem.type] = null;
                count[(int)recivedItem.type] = false;
                recivedItem.LaunchedInTable(null);
            }
            else
            {
                waitList.Remove(recivedItem.gameObject);
            }
        }
    }

    private bool ItemIsMissing()
    {
        bool itemMissing = false;

        for (int i = 0; i < 4; i++)
        {
            itemMissing |= items[i] == null;
        }

        return itemMissing;
    }

    private void GiveMeBurger()
    {
        for (int i = 0; i < 4; i++)
        {
            Destroy(items[i]);
        }

        Instantiate(burger, transform.position, Quaternion.identity);
    }
}
