using System;

[Serializable]
public class ProductRecord
{
    // Date, SubjectName, Receiving, Release, CompanyName, ReceivingTime, Gugo
    public string Date;
    public string SubjectName;
    public string Receiving;     // ���ڿ�(��ǥ ���� ����), �ʿ�� int ��ȯ
    public string Release;
    public string CompanyName;
    public string ReceivingTime; // "HH:mm" ��
    public string Gugo;          // ���
}

public enum FilterOp { Contains, Equals, StartsWith, EndsWith }

[Serializable]
public struct FilterCondition
{
    public string field; // "Date","SubjectName","CompanyName" ��
    public FilterOp op;
    public string value;
}
