using System.Collections.Generic;

public class Blackboard
{
    private Dictionary<string, object> data = new Dictionary<string, object>();
    
    public void SetValue(string key, object value)
    {
        if (data.ContainsKey(key))
            data[key] = value;
        else
            data.Add(key, value);
    }

    public T GetValue<T>(string key)
    {
        if(data.TryGetValue(key, out object value))
            return (T)value;

        throw new KeyNotFoundException($"BlackBoard에 키 {key}가 존재하지 않는다.");
    }

    public bool ContainKey(string key)
    {
        return data.ContainsKey(key);
    }
}

/*
 
if(data.TryGetValue(key, out object value))
{
    // is가 value type, as가 ref type인데
    bool isValueType = typeof(T).IsValueType;

    if(isValueType)
    {
        if(value is T typeValue)
            return typeValue;
    }
    else
    {
        // Error Code CS0413 T랑 as 연산자랑 사용할 수 없다는 내용이다.
        // 그래서 이를 해결하려면 명시적 캐스팅을 사용해야 한다. (T)value 사용
        T targetNode = value as T;
                
    }
}

*/