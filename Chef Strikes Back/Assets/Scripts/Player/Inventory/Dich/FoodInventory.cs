using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodInventory : MonoBehaviour
{
    //[Header("Objects")]
    //[SerializeField]
    //private FoodItem emptyItem;
    //[SerializeField]
    //private GameObject burger;
    //[SerializeField]
    //private GameObject hotDog;

    //[Space, Header("Cooking Items")]
    //[SerializeField]
    //private FoodItem[] foodItems;
    //[SerializeField]
    //private FoodItem[] foodCount;


    // Update is called once per frame
    //void Update()
    //{
    //    CountItems();
    //    CheckIfFoodIsReady();
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    for (int i = 0; i < foodItems.Length; i++)
    //    {
    //        if (foodItems[i] == emptyItem)
    //        {
    //            switch (collision.tag)
    //            {
    //                case "BurgerBun":
    //                    foodItems[i] = foodCount[0]; return;
    //                    break;
    //                case "Meat":
    //                    foodItems[i] = foodCount[1]; return;
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }
    //    }
    //}

    //private void CountItems()
    //{
    //    TempFunction();
    //    for (int i = 0; i < foodItems.Length; i++)
    //    {
    //        switch (foodItems[i].foodType)
    //        {
    //            case FoodType.BURGERBUN:
    //                AddFoodAvailability(0, i, true);
    //                break;
    //            case FoodType.MEAT:
    //                AddFoodAvailability(1, i, true);
    //                break;
    //            case FoodType.TOMATOE:
    //                AddFoodAvailability(2, i, true);
    //                break;
    //            case FoodType.LETTUCE:
    //                AddFoodAvailability(3, i, true);
    //                break;
    //            case FoodType.HOTDOGBUN:
    //                AddFoodAvailability(4, i, true);
    //                break;
    //            case FoodType.SAUSAGE:
    //                AddFoodAvailability(5, i, true);
    //                break;
    //            default: break;
    //        }
    //    }
    //}

    //private void AddFoodAvailability(int index, int myItemIndex, bool isAdding)
    //{
    //    if (!foodCount[index].isCounted && isAdding)
    //    {
    //        foodCount[index].isCounted = true;
    //        foodItems[myItemIndex].isCounted = true;
    //    }
    //    if (foodCount[index].isCounted && !isAdding)
    //    {
    //        foodCount[index].isCounted = false;
    //        foodItems[myItemIndex] = emptyItem;
    //    }
    //}

    //private FoodType CheckIfFoodIsReady()
    //{
    //    if (foodCount[0].isCounted && foodCount[1].isCounted && foodCount[2].isCounted && foodCount[3].isCounted)
    //    {
    //        DeliteItemsFor(FoodType.BURGER);
    //        Instantiate(burger);
    //        return FoodType.BURGER;
    //    }

    //    if (foodCount[4].isCounted && foodCount[5].isCounted)
    //    {
    //        DeliteItemsFor(FoodType.HOTDOG);
    //        Instantiate(hotDog);
    //        return FoodType.HOTDOG;
    //    }

    //    return FoodType.NONE;

    //}

    //private void DeliteItemsFor(FoodType foodType)
    //{
    //    if (foodType == FoodType.BURGER)
    //    {
    //        for (int i = 0; i < foodItems.Length; i++)
    //        {
    //            switch (foodItems[i].foodType)
    //            {
    //                case FoodType.BURGERBUN:
    //                    AddFoodAvailability(0, i, false);
    //                    break;
    //                case FoodType.MEAT:
    //                    AddFoodAvailability(1, i, false);
    //                    break;
    //                case FoodType.TOMATOE:
    //                    AddFoodAvailability(2, i, false);
    //                    break;
    //                case FoodType.LETTUCE:
    //                    AddFoodAvailability(3, i, false);
    //                    break;
    //                default: break;
    //            }
    //        }
    //    }
    //    else if (foodType == FoodType.HOTDOG)
    //    {
    //        for (int i = 0; i < foodItems.Length; i++)
    //        {
    //            switch (foodItems[i].foodType)
    //            {
    //                case FoodType.HOTDOGBUN:
    //                    AddFoodAvailability(4, i, false);
    //                    break;
    //                case FoodType.SAUSAGE:
    //                    AddFoodAvailability(5, i, false);
    //                    break;
    //                default: break;
    //            }
    //        }
    //    }

    //}


//    private void TempFunction()
//    {
//        for (int i = 0; i < foodCount.Length; i++)
//        {
//            foodCount[i].isCounted = false;
//        }
//    }
}

//public enum FoodType
//{
//    NONE,
//    BURGERBUN,
//    HOTDOGBUN,
//    LETTUCE,
//    TOMATOE,
//    MEAT,
//    SAUSAGE,
//    BURGER,
//    HOTDOG
//}
