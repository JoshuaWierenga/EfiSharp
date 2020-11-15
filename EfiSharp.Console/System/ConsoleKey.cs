namespace System
{
    //TODO Add more keys for Console.ReadKey
    public enum ConsoleKey
    {
        Backspace = 0x8,
        Tab = 0x9,

        Enter = 0xD,

        Spacebar = 0x20,

        D0 = 0x30,  // 0 through 9
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        //Key chars currently match my keyboard but since I have used ConsoleKey within ReadKey and not the chars shown
        //here, ReadKey should return the same ConsoleKey as the regular dotnet implementation, even for other keyboards
        //This is currently not true, hopefully it is once I switch to an array based version of ReadKey.
        Oem1 = 0xBA, // ; and :
        OemPlus = 0xBB, // = and +
        OemComma = 0xBC, // , and <
        OemMinus = 0xBD, // - and _
        OemPeriod = 0xBE, // . and >
        Oem2 = 0xBF, // / and ? 
        Oem3 = 0xC0, // ` and ~
        Oem4 = 0xDB, // [ and {
        Oem5 = 0xDC, // \ and |
        Oem6 = 0xDD, // ] and }
        Oem7 = 0xDE, // ' and "
    }
}
