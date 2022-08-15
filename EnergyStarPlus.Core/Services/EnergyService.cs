using EnergyStar.Interop;

namespace EnergyStar;

public class EnergyService
{
    private static readonly CancellationTokenSource cts = new();
    static readonly object locker = new();
    private static bool Running = true;
    public static void StopService()
    {
        cts.Cancel();
        HookManager.UnsubscribeWindowEvents();
        lock (locker)
        {
            Running = false;
            Monitor.Pulse(locker);
        }
    }
    static async void HouseKeepingThreadProc()
    {
        Console.WriteLine("House keeping thread started.");
        while (!cts.IsCancellationRequested)
        {
            try
            {
                var houseKeepingTimer = new PeriodicTimer(TimeSpan.FromMinutes(5));
                await houseKeepingTimer.WaitForNextTickAsync(cts.Token);
                EnergyManager.ThrottleAllUserBackgroundProcesses();
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    public static void Service()
    {
        HookManager.SubscribeToWindowEvents();
        EnergyManager.ThrottleAllUserBackgroundProcesses();

        Thread houseKeepingThread = new(new ThreadStart(HouseKeepingThreadProc));
        houseKeepingThread.Start();  // Wait for the thread to finish.

        while (Running)
        {
            lock (locker)
            {
                Monitor.Wait(locker);
            }
        }
        Running = true;
    }
}
