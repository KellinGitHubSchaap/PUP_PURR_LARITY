using System;
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

    public void HandleBreakdown(MachineType type)
    {
        PlayerGunner.canShoot = false;
        switch (type)
        {
            case MachineType.Oil:
                BlinkRepeating(MonitorType.Oil);
                break;
            case MachineType.Systems:
                BlueScreen();
                break;
            case MachineType.Oxygen:
                BlinkRepeating(MonitorType.Oxygen);
                break;
        }
    }

    public void HandleFix(MachineType type)
    {
        PlayerGunner.canShoot = true;
        switch (type)
        {
            case MachineType.Oil:
                foreach (var monitor in monitors)
                {
                    if (monitor.type != MonitorType.Oil) continue;
                    monitor.CancelBlink();
                }
                break;
            case MachineType.Systems:
                StopBlueScreen();
                break;
            case MachineType.Oxygen:
                foreach (var monitor in monitors)
                {
                    if (monitor.type != MonitorType.Oxygen) continue;
                    monitor.CancelBlink();
                }
                break;
        }
    }
}
