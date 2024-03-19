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
    [Serializable]
    public struct FoodImages
    {
        public FoodType Type;
        public GameObject Object;
    }

    [Header("Storage Info")]
    [SerializeField] private List<AllowedFood> _acceptedFoodTypes = new();
    [SerializeField] private Transform _foodOffset;
    private CircleCollider2D _circleCollider2D;
    private int _spawnFoodOffset = 20;

    [Space, Header("Storage Objects")]
    [SerializeField] private GameObject _burger;
    [SerializeField] private Transform _magnet;
    [SerializeField] private List<FoodImages> _foodImagesInspector;

    private Dictionary<FoodType, bool> _count = new();
    private Dictionary<FoodType, GameObject> _items = new();
    private Dictionary<FoodType, List<GameObject>> _waitList = new();
    private Dictionary<FoodType, bool> _acceptedFoodByType = new();
    private Dictionary<FoodType, GameObject> _foodSprites = new();

    [Header("TableStat")]
    private bool _isLocked = false;
    [SerializeField] Transform standPoint;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer _lockedRedCross;

    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _waitList = new();
        foreach (var foodType in _acceptedFoodTypes)
        {
            _waitList.Add(foodType.Food, new List<GameObject>());
            _count.Add(foodType.Food, false);
            _acceptedFoodByType.Add(foodType.Food, foodType.IsAllowed);
            _items.Add(foodType.Food, null);
        }

        foreach(var foodSprite in _foodImagesInspector)
        {
            _foodSprites.Add(foodSprite.Type, foodSprite.Object);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isLocked)
        {
            return;
        }

        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem && IsAcceptedType(recivedItem.Type) && recivedItem.IsPickable)
        {
            if (!_count[recivedItem.Type])
            {
                _items[recivedItem.Type] = recivedItem.gameObject;
                _count[recivedItem.Type] = true;
                recivedItem.LaunchedInTable(_magnet);
                recivedItem.IsPickable = false;
                StartCoroutine(IngredientSpriteActive(recivedItem));
            }
            else if (_count[recivedItem.Type] && !_waitList[recivedItem.Type].Contains(recivedItem.gameObject))
            {
                _waitList[recivedItem.Type].Add(recivedItem.gameObject);
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
        if (_isLocked)
        {
            _circleCollider2D.enabled = true;
        }
        else
        {
            _circleCollider2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Item recivedItem = collision.GetComponent<Item>();

        if (recivedItem && _waitList.ContainsKey(recivedItem.Type))
        {
            _waitList[recivedItem.Type].Remove(recivedItem.gameObject);
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
            _foodSprites[_acceptedFoodTypes[i].Food].SetActive(false);
            Destroy(_items[_acceptedFoodTypes[i].Food]);
        }
        Debug.Log(_spawnFoodOffset);
        Vector2 randomOffset = new Vector2(UnityEngine.Random.Range(-_spawnFoodOffset, _spawnFoodOffset), UnityEngine.Random.Range(-_spawnFoodOffset, _spawnFoodOffset));
        Debug.Log(randomOffset);
        randomOffset /= 100;
        Debug.Log(randomOffset);
        Instantiate(_burger, (Vector2)_foodOffset.position + randomOffset, Quaternion.identity);
    }

    private IEnumerator IngredientSpriteActive(Item item)
    {
        yield return new WaitForSeconds(0.2f);

        _foodSprites[item.Type].SetActive(true);
        item.gameObject.SetActive(false);
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
            foodItem.IsPickable = false;
            StartCoroutine(IngredientSpriteActive(foodItem));
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

    public Vector3 CombinerPos()
    {
        return standPoint.position;
    }

    public void Lock()
    {
        _lockedRedCross.enabled = true;
        _isLocked = true;
    }

    public void Unlock()
    {
        _lockedRedCross.enabled = false;
        _isLocked = false;
    }

    public bool GetIsLocked()
    {
        return _isLocked;
    }
}
