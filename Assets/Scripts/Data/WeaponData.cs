using System;
using System.Collections.Generic;

[Serializable]
public class WeaponData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 무기 고유 ID
    public string Name;        // 무기 이름
    public string Description; // 무기 설명
    public float Damage;         // 무기 데미지 (JSON 상 숫자로 들어오므로 int)
    public float FireCoolDown;         // 무기 쿨다운
    public float FireRange;         // 무기 사거리
    public string AttackPrefab;         // 무기 공격 유형 프리팹


}

[Serializable]
public class WeaponTable
{
    public List<WeaponData> Data;
}