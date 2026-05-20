using System;
using System.Collections.Generic;

[Serializable]
public class EnemyData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 몬스터 고유 ID
    public string Name;        // 몬스터 이름
    public string Description; // 몬스터 설명 (도감용)
    public string Engine_Id; // 적의 소유 엔진
    public string Weapon_Id; // 적의 소유 무기
    public float HP; // 적의 체력
    public string IconPath; // 몬스터 표기용 Icon
    public string PrefabPath; // 몬스터를 우선 프리팹으로 구현

}

[Serializable]
public class EnemyTable
{
    public List<EnemyData> Data;
}