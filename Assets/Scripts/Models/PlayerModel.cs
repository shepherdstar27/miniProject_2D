using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] //JsonUtility로 직렬화하려면, Mono를 상속받지 않도록 주의하자!
public class PlayerModel
{
    public string PlayerName;
    public int PlayerTotalExp;
    public string LastMapDataId;
    public Vector3 LastMapPosition;

    public List<ItemModel> ItemList = new List<ItemModel>();
}