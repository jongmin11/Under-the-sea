using System;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"array\":" + json + "}";
        return JsonUtility.FromJson<Wrapper<T>>(newJson).array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}