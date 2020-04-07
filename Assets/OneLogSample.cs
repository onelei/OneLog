using UnityEngine;

namespace One
{
    public class OneLogSample : MonoBehaviour
    {

        void Start()
        {
            One.Debug.Sample("QLog.Sample");
            One.Debug.Log("QLog.Log");
            One.Debug.LogEditor("QLog.LogEditor");
            One.Debug.LogError("QLog.LogError");
            One.Debug.LogErrorEditor("QLog.LogErrorEditor");
            One.Debug.LogWarning("QLog.LogWarning");
            One.Debug.LogWarningEditor("QLog.LogWarningEditor");
        }

    }
}
