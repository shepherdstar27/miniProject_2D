using System;
using System.Collections.Generic;

[Serializable] //JsonUtility로 직렬화하려면, Mono를 상속받지 않도록 주의하자!
public class ItemModel
{
    public long ItemUniqueId;
    public string ItemDataId;
    public int ItemStackCount;
}