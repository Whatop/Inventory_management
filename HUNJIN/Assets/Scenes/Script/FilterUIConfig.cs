using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilterUIConfig", menuName = "Inventory/Filter UI Config")]
public class FilterUIConfig : ScriptableObject
{
    [Header("필터 가능한 필드")]
    public string[] fields = new string[] {
        "Date","SubjectName","CompanyName","Release","Receiving","ReceivingTime","Gugo"
    };

    [Header("연산자 표시 문자열(순서 중요)")]
    public string[] operators_ko = new string[] { "포함", "같음", "시작", "끝남" };
}
