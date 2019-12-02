using UnityEngine;
using System.Diagnostics;

public class QLog
{
    #region Conditional
    [Conditional("QLog")]
    public static void Sample(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogEditor(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarningEditor(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogErrorEditor(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    #endregion

    #region Unity API

    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    public static void LogWarning(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    #endregion
}
