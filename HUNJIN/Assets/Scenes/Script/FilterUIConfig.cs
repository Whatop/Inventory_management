using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilterUIConfig", menuName = "Inventory/Filter UI Config")]
public class FilterUIConfig : ScriptableObject
{
    [Header("필터 가능한 필드 (키, 순서 고정)")]
    public string[] fields = new string[] {
        "Date","SubjectName","CompanyName","Release","Receiving","ReceivingTime","Gugo"
    };

    [Header("필드 표시 문자열 (한국어, fields와 동일 순서)")]
    public string[] fieldLabels_ko = new string[] {
        "날짜",     // Date
        "모델명",   // SubjectName
        "거래처",   // CompanyName
        "출고",     // Release
        "입고",     // Receiving
        "시간",     // ReceivingTime
        "규격"      // Gugo
    };

    [Header("연산자 표시 문자열(순서 중요)")]
    public string[] operators_ko = new string[] { "포함", "같음", "시작", "끝남" };

}
