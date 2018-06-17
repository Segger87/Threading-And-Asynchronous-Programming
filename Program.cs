using System;
using System.Threading;

public class DataStore { public int Value { get; set; } }
class Program
{
	static void Main(string[] args)
	{
		var p = new Program();

		p.ConcurrencyTest();
		Console.ReadLine();
	}

	private DataStore store = new DataStore();
	private DataStore store2 = new DataStore();

	public void ConcurrencyTest()
	{
		var thread1 = new Thread(IncrementTheValue);
		var thread2 = new Thread(OccasionalyDeadLock);

		thread1.Start();
		thread2.Start();

		thread1.Join(); // Wait for the thread to finish executing
		thread2.Join();

		//will only return 1 as only 1 thread is incrementing the value
		Console.WriteLine($"Final value: {store.Value}");
	}
	private void IncrementTheValue()
	{
		//lock forces one thread to wait for the other thread to complete before continuing - below is an example of how to create a deadlock (THIS IS BAD)
		lock (store)
		{
			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("First Instance of DataStore is Locked");
			Thread.Sleep(1000);

			lock (store2)
			{
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.WriteLine("Second Instance of DataStore is Locked");
				Console.ResetColor();
			}
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("Second Instance of DataStore is Unlocked");
		}
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.WriteLine("First Instance of DataStore is Unlocked");
		Console.ResetColor();
		store.Value++;
	}

	private void OccasionalyDeadLock()
	{
		lock (store2)
		{
			lock (store)
			{
				Console.WriteLine("NO DEAD LOCK TODAY BRO, LUL!");
			}
		}
	}
}



