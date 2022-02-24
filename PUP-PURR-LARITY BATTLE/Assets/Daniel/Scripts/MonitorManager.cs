using UnityEngine;

public class MonitorManager : Singleton<MonitorManager>
{
    [SerializeField] private Monitor[] monitors;

    [ContextMenu("TestBlink")]
    public void TestBlink()
    {
        BlinkRepeating(MonitorType.Oil);
    }
    
    public void BlinkOnce(MonitorType type, bool positive = true)
    {
        foreach (var monitor in monitors)
        {
            if (type != monitor.type) continue;

            var index = positive ? 1 : 2;
            monitor.BlinkOnce(index);
        }
    }

    public void BlinkRepeating(MonitorType type, bool positive = true)
    {
        foreach (var monitor in monitors)
        {
            if (type != monitor.type) continue;
            
            var index = positive ? 1 : 2;
            monitor.BlinkRepeating(index);
        }
    }

    [ContextMenu("BlueScreen")]
    public void BlueScreen()
    {
        foreach (var monitor in monitors)
        {
            monitor.BlueScreen();
        }
    }

    [ContextMenu("StopBlueScreen")]
    public void StopBlueScreen()
    {
        foreach (var monitor in monitors)
        {
            monitor.RevertToDefault();
        }
    }
}
