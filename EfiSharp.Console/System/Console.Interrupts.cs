using EfiSharp;

namespace System
{
    public static partial class Console
    {
        private static partial class Buffer
        {
            private static unsafe class Interrupts
            {
                private static void* _backspaceInterruptHandle;
                private static void* _tabInterruptHandle;

                private static void* _enterInterruptHandle;

                private static void* _spaceInterruptHandle;

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

                private static void* _upperAInterruptHandle;
                private static void* _upperBInterruptHandle;
                private static void* _upperCInterruptHandle;
                private static void* _upperDInterruptHandle;
                private static void* _upperEInterruptHandle;
                private static void* _upperFInterruptHandle;
                private static void* _upperGInterruptHandle;
                private static void* _upperHInterruptHandle;
                private static void* _upperIInterruptHandle;
                private static void* _upperJInterruptHandle;
                private static void* _upperKInterruptHandle;
                private static void* _upperLInterruptHandle;
                private static void* _upperMInterruptHandle;
                private static void* _upperNInterruptHandle;
                private static void* _upperOInterruptHandle;
                private static void* _upperPInterruptHandle;
                private static void* _upperQInterruptHandle;
                private static void* _upperRInterruptHandle;
                private static void* _upperSInterruptHandle;
                private static void* _upperTInterruptHandle;
                private static void* _upperUInterruptHandle;
                private static void* _upperVInterruptHandle;
                private static void* _upperWInterruptHandle;
                private static void* _upperXInterruptHandle;
                private static void* _upperYInterruptHandle;
                private static void* _upperZInterruptHandle;

                internal static void InitInterrupts()
                {
                    UefiApplication.In->RegisterKeyNotify(
                        new EFI_KEY_DATA(new EFI_INPUT_KEY('\b'), new EFI_KEY_STATE()),
                        &MainInterrupt, out _backspaceInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(
                        new EFI_KEY_DATA(new EFI_INPUT_KEY('\t'), new EFI_KEY_STATE()),
                        &MainInterrupt, out _tabInterruptHandle);

                    UefiApplication.In->RegisterKeyNotify(
                        new EFI_KEY_DATA(new EFI_INPUT_KEY('\r'), new EFI_KEY_STATE()),
                        &EnterInterrupt, out _enterInterruptHandle);

                    UefiApplication.In->RegisterKeyNotify(
                        new EFI_KEY_DATA(new EFI_INPUT_KEY(' '), new EFI_KEY_STATE()),
                        &MainInterrupt, out _spaceInterruptHandle);

                    //TODO Add remaining interrupts
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('a'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerAInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('b'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerBInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('c'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerCInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('d'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerDInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('e'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerEInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('f'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerFInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('g'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerGInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('h'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerHInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('i'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerIInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('j'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerJInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('k'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerKInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('l'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerLInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('m'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerMInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('n'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerNInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('o'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerOInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('p'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerPInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('q'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerQInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('r'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerRInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('s'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerSInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('t'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerTInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('u'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerUInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('v'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerVInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('w'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerWInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('x'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerXInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('y'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerYInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('z'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _lowerZInterruptHandle);


                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('A'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperAInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('B'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperBInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('C'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperCInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('D'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperDInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('E'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperEInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('F'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperFInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('G'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperGInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('H'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperHInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('I'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperIInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('J'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperJInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('K'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperKInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('L'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperLInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('M'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperMInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('N'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperNInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('O'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperOInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('P'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperPInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('Q'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperQInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('R'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperRInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('S'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperSInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('T'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperTInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('U'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperUInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('V'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperVInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('W'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperWInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('X'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperXInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('Y'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperYInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('Z'), new EFI_KEY_STATE()),
                    &MainInterrupt, out _upperZInterruptHandle);

                    Console.WriteLine("Interrupts init!");
                }

                private static EFI_STATUS EnterInterrupt(EFI_KEY_DATA* enter)
                {
                    PushBack(*enter);
                    PushBack(new EFI_KEY_DATA(new EFI_INPUT_KEY('\n'), enter->KeyState));
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS MainInterrupt(EFI_KEY_DATA* key)
                {
                    PushBack(*key);
                    return EFI_STATUS.EFI_SUCCESS;
                }
            }
        }
    }
}
