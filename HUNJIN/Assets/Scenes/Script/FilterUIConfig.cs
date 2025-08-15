using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FilterUIConfig", menuName = "Inventory/Filter UI Config")]
public class FilterUIConfig : ScriptableObject
{
    [Header("���� ������ �ʵ�")]
    public string[] fields = new string[] {
        "Date","SubjectName","CompanyName","Release","Receiving","ReceivingTime","Gugo"
    };

    [Header("������ ǥ�� ���ڿ�(���� �߿�)")]
    public string[] operators_ko = new string[] { "����", "����", "����", "����" };
}
