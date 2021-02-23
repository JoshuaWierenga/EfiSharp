namespace System
{
    internal readonly struct ConsoleReadKey
    {
        internal readonly char Key;
        internal readonly byte Length;

        public ConsoleReadKey(char key, byte length = 0)
        {
            Key = key;
            Length = length;
        }
    }
}
