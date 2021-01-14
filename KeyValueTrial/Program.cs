using System;
using System.Diagnostics;

namespace KeyValueTrial
{
    class Program
    {
        static void Main(string[] args)
        {
            var keybase = "alongerkeytotestwith";
            var examplepayload = "abdefghijklmnopqrstuvwxyz";
            
            // add remove the following to vary the size of the payload....
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            examplepayload = examplepayload + examplepayload;
            
            using var sqliteStore = new SqlLiteKeyValueStore();
            Console.WriteLine("Sqlite");
            RunTest(sqliteStore, examplepayload, keybase);
            
            Console.WriteLine("FASTER");
            using var fasterStore = new FasterKeyValueStore();
            RunTest(fasterStore, examplepayload, keybase);
        }

        private static void RunTest(IValueStore store, string payload, string keybase)
        {
            store.Clear();
            var sw = new Stopwatch();

            var count = 10000;
            Console.WriteLine($"Payload size: {payload.Length}");
            sw.Start();


            store.Begin();
            for (int i = 0; i < count; i++)
            {
                store.Put($"{keybase}{i}", payload);
            }

            store.End();


            sw.Stop();
            Console.WriteLine($"Puts Took {sw.Elapsed.TotalSeconds}s");
            sw.Restart();
            for (int i = 0; i < count; i++)
            {
                var g = store.Get($"{keybase}{i}");
            }


            sw.Stop();

            Console.WriteLine($"Gets Took {sw.Elapsed.TotalSeconds}s");
        }
    }
}