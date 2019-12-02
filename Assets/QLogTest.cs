using UnityEngine;

public class QLogTest : MonoBehaviour
{

    void Start()
    {
        QLog.Sample("QLog.Sample");
        QLog.Log("QLog.Log");
        QLog.LogEditor("QLog.LogEditor");
        QLog.LogError("QLog.LogError");
        QLog.LogErrorEditor("QLog.LogErrorEditor");
        QLog.LogWarning("QLog.LogWarning");
        QLog.LogWarningEditor("QLog.LogWarningEditor");
    }

}
