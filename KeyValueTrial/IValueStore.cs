namespace KeyValueTrial
{
    internal interface IValueStore
    {
        void Put(string key, string value);
        string Get(string key);
        void Clear();
        void Begin();
        void End();
    }
}