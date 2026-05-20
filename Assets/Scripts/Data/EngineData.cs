using System;
using System.Collections.Generic;

[Serializable]
public class EngineData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 엔진 고유 ID
    public string Name;        // 엔진 이름
    public string Description; // 엔진 설명 (도감용)
    public float MoveSpeed; // 이동속도
    public float RotateSpeed; // 배의 회전 속도
    public float Acceleration; // 배의 관성
    public string IconPath; // 엔진 표기용 Icon

}

[Serializable]
public class EngineTable
{
    public List<EngineData> Data;
}