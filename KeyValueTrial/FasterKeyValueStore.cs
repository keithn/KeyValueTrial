using System;
using FASTER.core;

namespace KeyValueTrial
{
    class FasterKeyValueStore : IValueStore, IDisposable
    {
        private ClientSession<string, string, string, string, Empty, IFunctions<string, string, string, string, Empty>>
            _session;

        private FasterKV<string, string> _store;

        public FasterKeyValueStore()
        {
            var log = Devices.CreateLogDevice(@"hlog.log");
            var objlog = Devices.CreateLogDevice(@"hlog.obj.log");

            var logSettings = new LogSettings
            {
                LogDevice = log,
                ObjectLogDevice = objlog,
            };

            _store = new FasterKV<string, string>(
                size: 1L << 20,
                logSettings: logSettings,
                checkpointSettings: new CheckpointSettings()
                {
                    CheckpointDir = @"." ,
                }
            );

            try
            {
                _store.Recover();
            }
            catch (Exception e)
            {
                // first recover with no checkpoint spits the dummy
            }

            _session = _store.NewSession(new SimpleFunctions<string, string>());
        }

        public void Put(string key, string value)
        {
            _session.Upsert(ref key, ref value);
        }

        public string Get(string key)
        {
            var value = "";
            var status = _session.Read(ref key, ref value);
            if(status != Status.OK) Console.WriteLine(status);
            return value;
        }

        public void Clear()
        {
        }

        public void Begin()
        {
        }

        public void End()
        {
            var x = _store.TakeFullCheckpoint(out var savedGuid, CheckpointType.Snapshot);
        }
        


        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}