using EfiSharp;

namespace System
{
    public static partial class Console
    {
        private static partial class Buffer
        {
            private static unsafe class Interrupts
            {
                private static void* _enterInterruptHandle;

                private static void* _lowerAInterruptHandle;
                private static void* _lowerBInterruptHandle;
                private static void* _lowerCInterruptHandle;
                private static void* _lowerDInterruptHandle;
                private static void* _lowerEInterruptHandle;
                private static void* _lowerFInterruptHandle;
                private static void* _lowerGInterruptHandle;
                private static void* _lowerHInterruptHandle;
                private static void* _lowerIInterruptHandle;
                private static void* _lowerJInterruptHandle;
                private static void* _lowerKInterruptHandle;
                private static void* _lowerLInterruptHandle;
                private static void* _lowerMInterruptHandle;
                private static void* _lowerNInterruptHandle;
                private static void* _lowerOInterruptHandle;
                private static void* _lowerPInterruptHandle;
                private static void* _lowerQInterruptHandle;
                private static void* _lowerRInterruptHandle;
                private static void* _lowerSInterruptHandle;
                private static void* _lowerTInterruptHandle;
                private static void* _lowerUInterruptHandle;
                private static void* _lowerVInterruptHandle;
                private static void* _lowerWInterruptHandle;
                private static void* _lowerXInterruptHandle;
                private static void* _lowerYInterruptHandle;
                private static void* _lowerZInterruptHandle;

                internal static void InitInterrupts()
                {
                    UefiApplication.In->RegisterKeyNotify(
                        new EFI_KEY_DATA(new EFI_INPUT_KEY((char)ConsoleKey.Enter), new EFI_KEY_STATE()),
                        &EnterInterrupt, out _enterInterruptHandle);

                    //TODO Add remaining interrupts
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('a'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerAInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('b'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerBInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('c'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerCInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('d'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerDInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('e'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerEInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('f'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerFInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('g'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerGInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('h'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerHInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('i'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerIInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('j'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerJInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('k'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerKInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('l'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerLInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('m'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerMInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('n'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerNInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('o'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerOInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('p'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerPInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('q'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerQInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('r'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerRInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('s'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerSInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('t'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerTInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('u'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerUInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('v'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerVInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('w'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerWInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('x'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerXInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('y'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerYInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('z'), new EFI_KEY_STATE()),
                    &LowerLetterInterrupt, out _lowerZInterruptHandle);

                    Console.WriteLine("Interrupts init!");
                }

                private static EFI_STATUS EnterInterrupt(EFI_KEY_DATA* enter)
                {
                    PushBack(*enter);
                    PushBack(new EFI_KEY_DATA(new EFI_INPUT_KEY('\n'), enter->KeyState));
                    return EFI_STATUS.EFI_SUCCESS;
                }

                //TODO Replace all interrupt functions with one function since I believe the same interrupt function can be registered multiple times 
                private static EFI_STATUS LowerLetterInterrupt(EFI_KEY_DATA* letter)
                {
                    PushBack(*letter);
                    return EFI_STATUS.EFI_SUCCESS;
                }
            }
        }
    }
}
