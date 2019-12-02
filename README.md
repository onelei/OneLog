# Unity Debug Dll 编写

## Debug.Log存在的问题

在Unity调试代码的时候，我们经常要用到Debug.Log函数。

```
 UnityEngine.Debug.Log(message);
```

平时开发的时候需要打印Log，但是出包的时候需要屏蔽掉对应的Log打印。这时候有人或许说了，Unity提供了UNITY_DDITOR编辑器下的宏，我们调用Log函数的时候可以通过添加宏来控制显示。示例如下

```
 #if UNITY_EDITOR
 UnityEngine.Debug.Log(message);
 #endif
```

然后每个地方都这样编写比较麻烦，我们封装一个函数来调用

```
public static void Log(object message)
{
 #if UNITY_EDITOR
 UnityEngine.Debug.Log(message);
 #endif
}
```

想法很好，但是这样打印Log之后，我们在Unity的Console窗口双击该打印信息，就会直接跳转到封装好的Log函数，而并不是跳转到我们设置的打印的位置。

## 探究Debug.Log函数

我们通过Unity的Debug.Log函数跟踪过去，发现了这样一个有意思的写法

![QQ截图20191202205850](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202205850.png)

![QQ截图20191202205911](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202205911.png)

函数上方有一个Conditional属性。我们查看它的含义发现

![QQ截图20191202210253](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202210253.png)

当定义了UNITY_ASSERTIONS宏的时候，该函数才会被调用。它其实就相当于如下函数

```
[Conditional("UNITY_ASSERTIONS")]
public static void Assert(bool condition, string message, Object context);
```

相当于

```
#if UNITY_ASSERTIONS
public static void Assert(bool condition, string message, Object context)
{
 
}
#endif
```

该函数被宏包起来了。

## 由Conditional启发

通过Conditional启发了QLog的诞生。我们通过Conditional编写一个专门用来调试使用的dll文件。然后在Unity里面直接调用即可。这样就可以避免了上面提到的，双击会跳转到同一个函数的问题。

打开VS，选择文件，新建，项目。选择.net framework类库，注意框架要选择.net 3.5，因为Unity里面最低版本是3.5

![QQ截图20191202211512](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211512.png)

![QQ截图20191202211035](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211035.png)

然后添加UnityEngine的dll引用

![QQ截图20191202211106](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211106.png)



![QQ截图20191202211132](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211132.png)

![QQ截图20191202211157](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211157.png)

接下来只需要在Program.cs文件编写代码如下

```
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

```

我们查看QLog的属性，然后确认如下

![QQ截图20191202211949](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202211949.png)

接下来选择release版本，点击生成即可

![QQ截图20191202212053](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202212053.png)

![QQ截图20191202212204](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202212204.png)

我们将生成好的QLog.dll放入Unity的Plugins文件夹下面即可

![QQ截图20191202212300](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202212300.png)

接下来，我们编写如下测试案例

```
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
```

### 在不开启QLog宏的时候打印如下

![QQ截图20191202212517](https://github.com/onelei/RedDotManager/blob/master/Images/QQ截图20191202212517.png)

只有6条打印信息，其中QLog.Sample并没有打印出来。

### 在开启QLog宏的时候打印如下

![QQ截图2019120221243https://github.com/onelei/RedDotManager/blob/master/Images/es\QQ截图20191202212434.png)

发现当QLog宏开启的时候，QLog.Sample被打印了出来。



### DLL工程

“QLog”文件夹



### Unity工程

“Assets”文件夹

