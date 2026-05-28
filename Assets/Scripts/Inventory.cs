using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private static Inventory _instance;
    public static Inventory Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<InventorySlotData> GetCargoSlots()
    {
        return GameManager.Inst.PlayerModel.CargoSlots;
    }

    public void AddItemToInventory(string itemId, int count)
    {
        GameManager.Inst.PlayerModel.AddItem(itemId, count);
        RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        if (InventoryUI.Instance != null)
        {
            PlayerModel model = GameManager.Inst.PlayerModel;
            InventoryUI.Instance.RefreshInventoryDisplay(model.CargoSlots);
            InventoryUI.Instance.UpdateSpecialResourceText(model.Gold, model.Fuel, model.Supplies);
        }
    }
}