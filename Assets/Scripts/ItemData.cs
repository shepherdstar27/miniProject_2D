using System;
using System.Collections.Generic;

// 엑셀 및 JSON과 철자가 완벽히 일치해야 하는 데이터 원본 구조체
[Serializable]
public class ItemData
{
    public string Id;             // 아이템 고유 번호
    public string Name;        // 아이템 이름
    public string Description; // 아이템 설명
    public string ItemType;        // 아이템 타입
    public string Grade;        // 등급
    public string Price;        // 아이템 가치
    public string MaxStack;        // 아이템 최대 보유
    public string IconPath;    // Resources 폴더 내의 아이콘 이미지 경로
}

// GameDataManager가 로드할 때 사용할 컨테이너 클래스
[Serializable]
public class ItemTable
{
    public List<ItemData> Data;
}