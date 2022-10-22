using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Today : MonoBehaviour
{
    Text text;
    public bool  isFrist;
    private DateTime _dateTime;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    void OnEnable()
    {
        if (!isFrist)
        {
            _dateTime = DateTime.Now;
            text.text = _dateTime.ToString("yy-MM-dd");
        }
        else
        {
            _dateTime = DateTime.Now;
            text.text = _dateTime.ToString("yy-MM") + "-01";
        }
    }

}
