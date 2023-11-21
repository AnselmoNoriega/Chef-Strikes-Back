using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationTable : MonoBehaviour
{
    [Serializable]
    public struct AllowedFood
    {
        public FoodType Food;
        public bool IsAllowed;
    }

    [Header("Storage Info")]
    [SerializeField] private List<AllowedFood> _acceptedFoodTypes = new();

    [Space, Header("Storage Objects")]
    [SerializeField] private GameObject burger;
    [SerializeField] private Transform magnet;

    private Dictionary<FoodType, bool> count = new();
    private Dictionary<FoodType, GameObject> items = new();
    private Dictionary<FoodType, List<GameObject>> waitList = new();
    private readonly Dictionary<FoodType, bool> _acceptedFoodByType = new();

    private void Start()
    {
        waitList = new();
        foreach (var foodType in _acceptedFoodTypes)
        {
            waitList.Add(foodType.Food, new List<GameObject>());
            count.Add(foodType.Food, false);
            _acceptedFoodByType.Add(foodType.Food, foodType.IsAllowed);
            items.Add(foodType.Food, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem && IsAcceptedType(recivedItem.type) && recivedItem.isPickable)
        {
            if (!count[recivedItem.type])
            {
                items[recivedItem.type] = recivedItem.gameObject;
                count[recivedItem.type] = true;
                recivedItem.LaunchedInTable(magnet);
                recivedItem.isPickable = false;
            }
            else if (count[recivedItem.type] && !waitList[recivedItem.type].Contains(recivedItem.gameObject))
            {
                waitList[recivedItem.type].Add(recivedItem.gameObject);
            }

            if (!ItemIsMissing())
            {
                StartCoroutine(GiveMeBurger());
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _acceptedFoodTypes.Count; ++i)
        {
            CheckAvailability(_acceptedFoodTypes[i].Food);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem)
        {
            waitList[recivedItem.type].Remove(recivedItem.gameObject);
        }
    }

    private bool ItemIsMissing()
    {
        for (int i = 0; i < _acceptedFoodTypes.Count; ++i)
        {
            if (items[_acceptedFoodTypes[i].Food] == null)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator GiveMeBurger()
    {
        yield return new WaitForSeconds(1);

        for (int i = 0; i < _acceptedFoodTypes.Count; ++i)
        {
            Destroy(items[_acceptedFoodTypes[i].Food]);
        }

        Instantiate(burger, transform.position, Quaternion.identity);
    }

    private void CheckAvailability(FoodType foodtype)
    {
        if (items[foodtype] == null && waitList[foodtype].Count > 0)
        {
            items[foodtype] = waitList[foodtype][0];
            waitList[foodtype].RemoveAt(0);
            count[foodtype] = true;
            var foodItem = items[foodtype].GetComponent<Item>();
            foodItem.LaunchedInTable(magnet);
            foodItem.isPickable = false;
        }
    }

    private bool IsAcceptedType(FoodType type)
    {
        if (_acceptedFoodByType.ContainsKey(type))
        {
            return _acceptedFoodByType[type];
        }
        return false;
    }
}
