using System;

[Serializable]
public class InventorySlot
{
    public int itemID;     // 어떤 아이템인가? (ItemData의 ID 참조)
    public int itemCount;  // 몇 개 가지고 있는가?

    public InventorySlot(int id, int count)
    {
        this.itemID = id;
        this.itemCount = count;
    }
}