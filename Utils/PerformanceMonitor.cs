using System;
using System.Diagnostics;
using System.Threading;

namespace HostCord.Utils;

public class PerformanceMonitor
{
    private static PerformanceMonitor instance;
    
    private Thread pcMonitor;
    private Thread networkMonitor;
    private bool isRunning = false;
    private int pollingIntervalMS;
    private string processName;

    public float cpuUsage { get; private set; } = .0f;
    public float ramUsage { get; private set; } = .0f;

    //                         One second by default ↓
    private PerformanceMonitor(int pollingIntervalS = 2)
    {
        processName = getProcessName();
        pcMonitor = new Thread(getPcData);
        pollingIntervalMS = pollingIntervalS * 1000;
    }

    public static PerformanceMonitor getInstance()
    {
        if (instance == null)
            instance = new PerformanceMonitor();

        return instance;
    }

    public void start()
    {
        if (isRunning)
            return;

        isRunning = true;
        pcMonitor.Start();
    }

    public void stop()
    {
        if (!isRunning)
            return;

        isRunning = false;
        pcMonitor.Join();
    }

    private void getPcData()
    {
        while (isRunning)
        {
            cpuUsage = (float) Math.Round(getCPUUsage());
            ramUsage = (float) Math.Round(getRAMUsage());
            Thread.Sleep(pollingIntervalMS);
        }
    }

    private float getCPUUsage()
    {
        var counter = new PerformanceCounter("Process", "% Processor Time", processName, true);
        counter.NextValue();
        Thread.Sleep(pollingIntervalMS);
        return counter.NextValue();
    }
    
    private float getRAMUsage()
    {
        return new PerformanceCounter("Process", "Private Bytes", processName).NextValue()/1024/1024;
    }

    private string getProcessName()
    {
        var process = Process.GetCurrentProcess();

        foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
        {
            if (instance.StartsWith(process.ProcessName))
            {
                using (var processId = new PerformanceCounter("Process", "ID Process", instance, true))
                {
                    if (process.Id == (int)processId.RawValue)
                    {
                        return instance;
                    }
                }
            }
        }

        return "";
    }
}