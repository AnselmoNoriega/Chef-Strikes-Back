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
    [SerializeField] private Vector2 _pizzaOffset;

    [Space, Header("Storage Objects")]
    [SerializeField] private GameObject _burger;
    [SerializeField] private Transform _magnet;

    private Dictionary<FoodType, bool> _count = new();
    private Dictionary<FoodType, GameObject> _items = new();
    private Dictionary<FoodType, List<GameObject>> _waitList = new();
    private readonly Dictionary<FoodType, bool> _acceptedFoodByType = new();

    private void Start()
    {
        _waitList = new();
        foreach (var foodType in _acceptedFoodTypes)
        {
            _waitList.Add(foodType.Food, new List<GameObject>());
            _count.Add(foodType.Food, false);
            _acceptedFoodByType.Add(foodType.Food, foodType.IsAllowed);
            _items.Add(foodType.Food, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem && IsAcceptedType(recivedItem.type) && recivedItem.isPickable)
        {
            if (!_count[recivedItem.type])
            {
                _items[recivedItem.type] = recivedItem.gameObject;
                _count[recivedItem.type] = true;
                recivedItem.LaunchedInTable(_magnet);
                recivedItem.isPickable = false;
            }
            else if (_count[recivedItem.type] && !_waitList[recivedItem.type].Contains(recivedItem.gameObject))
            {
                _waitList[recivedItem.type].Add(recivedItem.gameObject);
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
            _waitList[recivedItem.type].Remove(recivedItem.gameObject);
        }
    }

    private bool ItemIsMissing()
    {
        for (int i = 0; i < _acceptedFoodTypes.Count; ++i)
        {
            if (_items[_acceptedFoodTypes[i].Food] == null)
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
            _count[_acceptedFoodTypes[i].Food] = false;
            Destroy(_items[_acceptedFoodTypes[i].Food]);
        }

        ServiceLocator.Get<GameManager>().Score += 5;
        Instantiate(_burger, (Vector2)transform.position + _pizzaOffset, Quaternion.identity);
    }

    private void CheckAvailability(FoodType foodtype)
    {
        if (_items[foodtype] == null && _waitList[foodtype].Count > 0)
        {
            _items[foodtype] = _waitList[foodtype][0];
            _waitList[foodtype].RemoveAt(0);
            _count[foodtype] = true;
            var foodItem = _items[foodtype].GetComponent<Item>();
            foodItem.LaunchedInTable(_magnet);
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
