using System;
using System.Collections.Generic;

[Serializable]
public class PortData
{
    // 규칙 반영: JSON의 "Id", "Name", "Description", "Damage"와 완벽히 일치
    public string Id;          // 항구 고유 ID
    public string Name;        // 항구 이름
    public string Description; // 항구 설명

}

[Serializable]
public class PortTable
{
    public List<PortData> Data;
}