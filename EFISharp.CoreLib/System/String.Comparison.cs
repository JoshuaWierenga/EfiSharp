namespace System
{
#pragma warning disable 660,661
    public partial class String
#pragma warning restore 660,661
    {
        // Determines whether two Strings match.
        public static unsafe bool Equals(string a, string b)
        {
            if (a is null || b is null || a.Length != b.Length)
            {
                return false;
            }

            //TODO Use EFI_UNICODE_COLLATION_PROTOCOL?
            fixed (char* pA = a, pB = b)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    if (pA[i] != pB[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        public static bool operator ==(string? a, string? b) => string.Equals(a, b);

        public static bool operator !=(string? a, string? b) => !string.Equals(a, b);
    }
}