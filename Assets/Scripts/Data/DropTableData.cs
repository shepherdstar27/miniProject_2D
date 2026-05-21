using System;
using System.Collections.Generic;

// 엑셀 및 JSON과 철자가 완벽히 일치해야 하는 데이터 원본 구조체
[Serializable]
public class DropTableData
{
    public string Id;             // 드랍테이블 고유 번호
    public string Drop_Item_List;        // 드랍테이블 목록
    public string Drop_Chance_List; // 드랍테이블 백분율 목록
    public string Min_Count; // 드랍테이블 최소값 목록
    public string Max_Count; // 드랍테이블 최대값 목록


// =================================================================================
// 텍스트 데이터를 순정 정수/실수 배열로 실시간 변환하는 단독 함수들
// =================================================================================
// 아이템 ID 문자열 목록을 가볍게 쪼개어 배열로 반환하는 함수
public string[] GetItemArray()
{
    if (string.IsNullOrEmpty(Drop_Item_List) == true) return new string[0];
    return Drop_Item_List.Split(',');
}

// 확률 텍스트 리스트를 순수 실수(float) 배열로 변환하는 정석 파싱 함수
public float[] GetChanceArray()
{
    if (string.IsNullOrEmpty(Drop_Chance_List) == true) return new float[0];

    string[] splitStrings = Drop_Chance_List.Split(',');
    float[] floatResults = new float[splitStrings.Length];

    for (int i = 0; i < splitStrings.Length; i++)
    {
        floatResults[i] = float.Parse(splitStrings[i]);
    }
    return floatResults;
}

// 최소 수량 텍스트 리스트를 정수(int) 배열로 변환하는 함수
public int[] GetMinCountArray()
{
    if (string.IsNullOrEmpty(Min_Count) == true) return new int[0];

    string[] splitStrings = Min_Count.Split(',');
    int[] intResults = new int[splitStrings.Length];

    for (int i = 0; i < splitStrings.Length; i++)
    {
        intResults[i] = int.Parse(splitStrings[i]);
    }
    return intResults;
}

// 최대 수량 텍스트 리스트를 정수(int) 배열로 변환하는 함수
public int[] GetMaxCountArray()
{
    if (string.IsNullOrEmpty(Max_Count) == true) return new int[0];

    string[] splitStrings = Max_Count.Split(',');
    int[] intResults = new int[splitStrings.Length];

    for (int i = 0; i < splitStrings.Length; i++)
    {
        intResults[i] = int.Parse(splitStrings[i]);
    }
    return intResults;
}
}





// DropTableData가 로드할 때 사용할 컨테이너 클래스
[Serializable]
public class DropTable
{
    public List<DropTableData> Data;
}