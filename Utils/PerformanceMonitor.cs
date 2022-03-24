using System;
using System.Diagnostics;
using System.Threading;

namespace HostCord.Utils;

public class PerformanceMonitor
{
    private static PerformanceMonitor instance;
    
    private Thread monitor;
    private bool isRunning = false;
    private int pollingIntervalMS;
    private Process mainProcess;

    public float cpuUsage { get; private set; } = .0f;
    public float ramUsage { get; private set; } = .0f;
    
    //                         One second by default ↓
    private PerformanceMonitor(int pollingIntervalS = 1)
    {
        mainProcess = Process.GetCurrentProcess();
        monitor = new Thread(getData);
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
        monitor.Start();
    }

    public void stop()
    {
        if (!isRunning)
            return;

        isRunning = false;
        Console.WriteLine("Stopping monitor");
        monitor.Join();
        Console.WriteLine("Monitor Stopped");
    }

    private void getData()
    {
        while (isRunning)
        {
            cpuUsage = (float) Math.Round(getCPUUsage());
            ramUsage = (float) Math.Round(getRAMUsage());
            Thread.Sleep(pollingIntervalMS / 2);
        }
    }

    private float getCPUUsage()
    {
        var counter = new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
        counter.NextValue();
        Thread.Sleep(pollingIntervalMS/2);
        var sample2 = counter.NextValue();
        return sample2;
    }
    
    private float getRAMUsage()
    {
        return new PerformanceCounter("Process", "Private Bytes", mainProcess.ProcessName).NextValue()/1024/1024;
    }
}