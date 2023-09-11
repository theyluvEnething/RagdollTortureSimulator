using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Reflection;
using System.ComponentModel.Design;
using System.Linq;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [Header("GENERAL")]
    [SerializeField] private int cash = 100;
    [SerializeField] private int gold = 0;
    [SerializeField] private bool[] unlockedItem = new bool[12];
    [SerializeField] private bool blood = false;
    [SerializeField] private int deathCount = 0;

    [HideInInspector] public bool keepItemState;
    public static GameManager Instance { get; private set; }

    [SerializeField] public Item ItemState;

    [SerializeField] InterstitialAdsButton AdsManager;

    public enum Item
    {
        Empty, // Entry State

        /* Item: 1 */
        [Price(1000)]
        ItemChainsaw,

        /* Item: 2 */
        [Price(5000)]
        ItemDiamondAxe,

        /* Item: 3 */
        [Price(35000)]
        ItemMissile,

        /* Item: 4 */
        [Price(50000)]
        ItemRifle,

        /* Item: 5 */
        [Price(70000)]
        ItemExpoRifle,

        /* Item: 6 */
        [Price(15000)]
        ItemGun,

        /* Item: 7 */
        [Price(100000)]
        ItemBazooka,

        /* Item: 8 */
        [Price(150000)]
        ItemTruck,

        /* Item: 9 */
        [Price(10000)]
        ItemDynamite,

        /* Item: 10 */
        [Price(250000)]
        ItemAquarium,

        /* Item: 11 */
        [Price(100000)]
        ItemFlamethrower,

        Null
    }

    void Start()
    {
        if (Instance)
        {
            Debug.LogError("More then one GameManager in scene");
        }
        Instance = this;
        ItemState = Item.Empty;
        this.cash = 100;
        this.gold = 0;
        this.keepItemState = false;
        this.unlockedItem = new bool[12];
        this.blood = false;
        this.deathCount = 0;

        DataPersistenceManager.Instance.LoadGame();
    }

    public void LoadData(GameData data)
    {
        this.cash = data.cash;
        this.gold = data.gold;
        this.blood = data.blood;
        this.deathCount = data.deathCount;

        this.keepItemState = data.keepItemState;
        if (keepItemState)
        {
            this.ItemState = data.ItemState;
        }
        for (int i = 0; i < unlockedItem.Length; i++)
        {
            unlockedItem[i] = data.unlockedItem[i];
        }

        AdsManager = DataManager.Instance.AdsManager;
        if (this.deathCount % 5 == 0)
        {
            AdsManager.ShowAd();
        }
    }
    public void SaveData(GameData data)
    {
        data.cash = this.cash;
        data.gold = this.gold;
        data.blood = this.blood;
        data.deathCount = this.deathCount;

        data.keepItemState = keepItemState;
        data.ItemState = this.ItemState;

        for (int i = 0; i < unlockedItem.Length; i++)
        {
            data.unlockedItem[i] = unlockedItem[i];
        }                
    }
    public int BuyItem(Item item)
    {
        Debug.Log("Item: " + item + " | Price: " + GetPrice(item));
        
        if (!unlockedItem[(int)item])
        {
            if (GetPrice(item) <= this.cash)
            {
                cash -= GetPrice(item);
                unlockedItem[(int)item] = true;
                ItemState = item;
                Debug.Log("<color=green>Sucessfully purchased item. </color>");
                return 1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            ItemState = item;
            Debug.Log("<color=yellow>Item was already bought. </color>");
            return 2;
        }
    }
    public int GetDeathCount()
    {
        return this.deathCount;
    }
    public void IncreaseDeathCount()
    {
        this.deathCount++;
    }
    public void DecraseDeathCount()
    {
        this.deathCount--;
    }
    public bool GetBloodUnlocked()
    {
        return this.blood;
    }
    public void unlockBlood()
    {
        blood = true;
    }
    public void disableBlood()
    {
        blood = false;
    }
    public void addCash(int amount)
    {
        this.cash += amount;
    }
    public void removeCash(int amount)
    {
        this.cash -= amount;
    }
    public void addGold(int amount)
    {
        this.gold += amount;
    }
    public void removeGold(int amount)
    {
        this.gold -= amount;
    }
    public int GetCash()
    {
        return this.cash;
    }    
    public int GetGold()
    {
        return this.gold;
    }
    sealed class PriceAttribute : Attribute
    {
        public int Price { get; }
        public PriceAttribute(int _Price)
        {
            Price = _Price;
        }
    }
    static int GetPrice(Item item)
    {
        FieldInfo field = item.GetType().GetField(item.ToString());
        if (field.GetCustomAttribute(typeof(PriceAttribute)) is PriceAttribute priceAttribute)
        {
            return priceAttribute.Price;
        }
        Debug.LogError("Item has invalid Price.");
        return -1;
    }
}
