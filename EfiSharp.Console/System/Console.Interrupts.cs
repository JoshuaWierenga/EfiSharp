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
                        new EFI_KEY_DATA(new EFI_INPUT_KEY((char) ConsoleKey.Enter), new EFI_KEY_STATE()),
                        &EnterInterrupt, out _enterInterruptHandle);

                    //TODO Add remaining interrupts
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('a'), new EFI_KEY_STATE()), 
                    &LowerAInterrupt, out _lowerAInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('b'), new EFI_KEY_STATE()),
                    &LowerBInterrupt, out _lowerBInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('c'), new EFI_KEY_STATE()),
                    &LowerCInterrupt, out _lowerCInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('d'), new EFI_KEY_STATE()),
                    &LowerDInterrupt, out _lowerDInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('e'), new EFI_KEY_STATE()),
                    &LowerEInterrupt, out _lowerEInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('f'), new EFI_KEY_STATE()),
                    &LowerFInterrupt, out _lowerFInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('g'), new EFI_KEY_STATE()),
                    &LowerGInterrupt, out _lowerGInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('h'), new EFI_KEY_STATE()),
                    &LowerHInterrupt, out _lowerHInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('i'), new EFI_KEY_STATE()),
                    &LowerIInterrupt, out _lowerIInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('j'), new EFI_KEY_STATE()),
                    &LowerJInterrupt, out _lowerJInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('k'), new EFI_KEY_STATE()),
                    &LowerKInterrupt, out _lowerKInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('l'), new EFI_KEY_STATE()),
                    &LowerLInterrupt, out _lowerLInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('m'), new EFI_KEY_STATE()),
                    &LowerMInterrupt, out _lowerMInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('n'), new EFI_KEY_STATE()),
                    &LowerNInterrupt, out _lowerNInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('o'), new EFI_KEY_STATE()),
                    &LowerOInterrupt, out _lowerOInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('p'), new EFI_KEY_STATE()),
                    &LowerPInterrupt, out _lowerPInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('q'), new EFI_KEY_STATE()),
                    &LowerQInterrupt, out _lowerQInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('r'), new EFI_KEY_STATE()),
                    &LowerRInterrupt, out _lowerRInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('s'), new EFI_KEY_STATE()),
                    &LowerSInterrupt, out _lowerSInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('t'), new EFI_KEY_STATE()),
                    &LowerTInterrupt, out _lowerTInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('u'), new EFI_KEY_STATE()),
                    &LowerUInterrupt, out _lowerUInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('v'), new EFI_KEY_STATE()),
                    &LowerVInterrupt, out _lowerVInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('w'), new EFI_KEY_STATE()),
                    &LowerWInterrupt, out _lowerWInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('x'), new EFI_KEY_STATE()),
                    &LowerXInterrupt, out _lowerXInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('y'), new EFI_KEY_STATE()),
                    &LowerYInterrupt, out _lowerYInterruptHandle);
                    UefiApplication.In->RegisterKeyNotify(new EFI_KEY_DATA(new EFI_INPUT_KEY('z'), new EFI_KEY_STATE()),
                    &LowerZInterrupt, out _lowerZInterruptHandle);

                    Console.WriteLine("Interrupts init!");
                }

                private static EFI_STATUS EnterInterrupt(EFI_KEY_DATA* enter)
                {
                    PushBack(*enter);
                    PushBack(new EFI_KEY_DATA(new EFI_INPUT_KEY('\n'), enter->KeyState));
                    return EFI_STATUS.EFI_SUCCESS;
                }

                //TODO Replace all interrupt functions with one function since I believe the same interrupt function can be registered multiple times 
                private static EFI_STATUS LowerAInterrupt(EFI_KEY_DATA* a)
                {
                    PushBack(*a);
                    return EFI_STATUS.EFI_SUCCESS;
                }
                private static EFI_STATUS LowerBInterrupt(EFI_KEY_DATA* b)
                {
                    PushBack(*b);
                    return EFI_STATUS.EFI_SUCCESS;
                }
                private static EFI_STATUS LowerCInterrupt(EFI_KEY_DATA* c)
                {
                    PushBack(*c);
                    return EFI_STATUS.EFI_SUCCESS;
                }
                private static EFI_STATUS LowerDInterrupt(EFI_KEY_DATA* d)
                {
                    PushBack(*d);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerEInterrupt(EFI_KEY_DATA* e)
                {
                    PushBack(*e);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerFInterrupt(EFI_KEY_DATA* f)
                {
                    PushBack(*f);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerGInterrupt(EFI_KEY_DATA* g)
                {
                    PushBack(*g);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerHInterrupt(EFI_KEY_DATA* h)
                {
                    PushBack(*h);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerIInterrupt(EFI_KEY_DATA* i)
                {
                    PushBack(*i);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerJInterrupt(EFI_KEY_DATA* j)
                {
                    PushBack(*j);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerKInterrupt(EFI_KEY_DATA* k)
                {
                    PushBack(*k);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerLInterrupt(EFI_KEY_DATA* l)
                {
                    PushBack(*l);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerMInterrupt(EFI_KEY_DATA* m)
                {
                    PushBack(*m);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerNInterrupt(EFI_KEY_DATA* n)
                {
                    PushBack(*n);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerOInterrupt(EFI_KEY_DATA* o)
                {
                    PushBack(*o);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerPInterrupt(EFI_KEY_DATA* p)
                {
                    PushBack(*p);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerQInterrupt(EFI_KEY_DATA* q)
                {
                    PushBack(*q);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerRInterrupt(EFI_KEY_DATA* r)
                {
                    PushBack(*r);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerSInterrupt(EFI_KEY_DATA* s)
                {
                    PushBack(*s);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerTInterrupt(EFI_KEY_DATA* t)
                {
                    PushBack(*t);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerUInterrupt(EFI_KEY_DATA* u)
                {
                    PushBack(*u);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerVInterrupt(EFI_KEY_DATA* v)
                {
                    PushBack(*v);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerWInterrupt(EFI_KEY_DATA* w)
                {
                    PushBack(*w);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerXInterrupt(EFI_KEY_DATA* x)
                {
                    PushBack(*x);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerYInterrupt(EFI_KEY_DATA* y)
                {
                    PushBack(*y);
                    return EFI_STATUS.EFI_SUCCESS;
                }

                private static EFI_STATUS LowerZInterrupt(EFI_KEY_DATA* z)
                {
                    PushBack(*z);
                    return EFI_STATUS.EFI_SUCCESS;
                }
            }
        }
    }
}