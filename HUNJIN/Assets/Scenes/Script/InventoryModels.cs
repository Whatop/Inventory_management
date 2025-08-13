using System;

[Serializable]
public class ProductRecord
{
    // Date, SubjectName, Receiving, Release, CompanyName, ReceivingTime, Gugo
    public string Date;
    public string SubjectName;
    public string Receiving;     // 문자열(쉼표 제거 전제), 필요시 int 변환
    public string Release;
    public string CompanyName;
    public string ReceivingTime; // "HH:mm" 등
    public string Gugo;          // 비고
}

public enum FilterOp { Contains, Equals, StartsWith, EndsWith }

[Serializable]
public struct FilterCondition
{
    public string field; // "Date","SubjectName","CompanyName" 등
    public FilterOp op;
    public string value;
}
