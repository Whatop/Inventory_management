using System;

[Serializable]
public class ApiResponse
{
    public string order;
    public string result; // "OK" or "FAIL"
    public string msg;
    public string value;  // JSON string (optional)
}
