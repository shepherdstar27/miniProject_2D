using System;
using System.Collections.Generic;

[Serializable]
public class WeaponData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 무기 고유 ID
    public string Name;        // 무기 이름
    public string Description; // 무기 설명
    public int Damage;         // 무기 데미지 (JSON 상 숫자로 들어오므로 int)
}

[Serializable]
public class WeaponTable
{
    public List<WeaponData> Data;
}