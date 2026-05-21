using System;
using System.Collections.Generic;

[Serializable]
public class ShipData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 몬스터 고유 ID
    public string Name;        // 몬스터 이름
    public string Description; // 몬스터 설명 (도감용)
    public float Max_HP; // 체력
    public int Cargo; // 화물칸
    public string PrefabPath; // 프리팹으로 구현

}

[Serializable]
public class ShipTable
{
    public List<ShipData> Data;
}