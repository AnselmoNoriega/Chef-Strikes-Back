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
    private List<List<GameObject>> waitList;

    [Space, Header("Storage Objects")]
    [SerializeField]
    private GameObject burger;
    [SerializeField]
    private Transform magnet;

    private void Start()
    {
        waitList = new List<List<GameObject>>();
        for (int i = 0; i < 3; ++i)
        {
            waitList.Add(new List<GameObject>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem != null && (int)recivedItem.type < 3)
        {
            if (!count[(int)recivedItem.type])
            {
                items[(int)recivedItem.type] = recivedItem.gameObject;
                count[(int)recivedItem.type] = true;
                recivedItem.LaunchedInTable(magnet);
            }
            else if (!items.Contains(recivedItem.gameObject) && !waitList[(int)recivedItem.type].Contains(recivedItem.gameObject))
            {
                waitList[(int)recivedItem.type].Add(recivedItem.gameObject);
            }

            if (!ItemIsMissing())
            {
                StartCoroutine(GiveMeBurger());
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 3; ++i)
        {
            CheckAvailability(i);
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
                waitList[(int)recivedItem.type].Remove(recivedItem.gameObject);
            }
        }
    }

    private bool ItemIsMissing()
    {
        bool itemMissing = false;

        for (int i = 0; i < 3; i++)
        {
            itemMissing |= items[i] == null;
        }

        return itemMissing;
    }

    private IEnumerator GiveMeBurger()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < 3; i++)
        {
            Destroy(items[i]);
        }

        Instantiate(burger, transform.position, Quaternion.identity);
    }

    private void CheckAvailability(int num)
    {
        if (items[num] == null && waitList[num].Count > 0)
        {
            items[num] = waitList[num][0];
            waitList[num].RemoveAt(0);
            count[num] = true;
            var foodItem = items[num].GetComponent<Item>();
            foodItem.LaunchedInTable(magnet);
        }
    }
}
