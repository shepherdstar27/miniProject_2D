using System;
using System.Collections.Generic;

[Serializable]
public class PlayerModel
{
    public string PlayerName;
    public int PlayerTotalExp;
    public List<ItemModel> ItemList = new List<ItemModel>();
}