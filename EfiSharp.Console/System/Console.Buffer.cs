using EfiSharp;

namespace System
{
    public static partial class Console
    {
        //Circular Deque from https://github.com/LeoVen/C-Macro-Collections/blob/dba782ccd6254436e8fd72fb9342dabaaa7d3f2e/src/cmc_deque.h 
        //TODO Make non static struct? Use c style instantiation?
        private static unsafe partial class Buffer
        {
            private static EFI_KEY_DATA* buffer;
            //sizeof(EFI_KEY_DATA) = 9 bytes => sizeof(_inputBuffer) = 4.608kb
            private const ushort Capacity = 512;
            private static ushort _count;
            private static uint _front;
            private static uint _back;

            //TODO Add static constructor
            private static void Init()
            {
                if (buffer == null) return;

                InitBuffer();
                //Interrupts.InitInterrupts();
            }

            //TODO Move to static constructor
            private static void InitBuffer()
            {
                EFI_KEY_DATA* newBuffer = stackalloc EFI_KEY_DATA[Capacity];
                buffer = newBuffer;

                _count = 0;
                _front = 0;
                _back = 0;

                Console.WriteLine("Buffer init!");
            }


            // Collection Allocation and Deallocation
            //private static Buffer New(int capacity, f_val) { }

            //private static Buffer NewCustom(int capacity, f_val, alloc, callbacks) { }

            //private static void Clear() { }

            //private static void Free() { }

            //private static void Customise(alloc, callbacks) { }

            // Collection Input and Output
            private static bool PushFront(EFI_KEY_DATA value)
            {
                if (Full())
                {
                    return false;
                }

                _front = _front == 0 ? Capacity - 1 : _front - 1;
                buffer[_front] = value;

                _count++;

                return true;
            }

            private static bool PushBack(EFI_KEY_DATA value)
            {
                if (Full())
                {
                    return false;
                }

                buffer[_back] = value;

                _back = _back == Capacity - 1 ? 0 : _back + 1;

                _count++;

                return true;
            }

            internal static bool PopFront()
            {
                if (Empty())
                {
                    return false;
                }

                buffer[_front].Dispose();

                _front = _front == Capacity - 1 ? 0 : _front + 1;

                _count--;

                return true;
            }

            private static bool PopBack()
            {
                if (Empty())
                {
                    return false;
                }

                _back = _back == 0 ? Capacity - 1 : _back - 1;

                buffer[_back].Dispose();

                _count--;

                return true;
            }

            // Element Access
            internal static bool Front(out EFI_KEY_DATA value)
            {
                if (Empty())
                {
                    value = new EFI_KEY_DATA();
                    value.Dispose();
                    return false;
                }

                value = buffer[_front];
                return true;
            }

            private static bool Back(out EFI_KEY_DATA value)
            {
                if (Empty())
                {
                    value = new EFI_KEY_DATA();
                    value.Dispose();
                    return false;
                }

                value = buffer[_back == 0 ? Capacity - 1 : _back - 1];
                return true;
            }

            // Collection State
            //Only compares the key, not the key state 
            internal static bool Contains(EFI_KEY_DATA value)
            {
                for (uint i = _front, j = 0; j < _count; j++)
                {
                    if (buffer[i].Key.UnicodeChar == value.Key.UnicodeChar)
                    {
                        return true;
                    }

                    i = (i + 1) % Capacity;
                }

                return false;
            }

            private static bool Empty() => _count == 0;

            private static bool Full() => _count == Capacity;

            //private static int Count() => count;

            //private static int Capacity() => capacity;

            //private static int Flag() { }

            // Collection Utility
            //private static bool Resize(int capacity) { }

            //private static Buffer CopyOf() { } 

            //private static bool Equals(Buffer) { }



            internal static void Fill()
            {
                Init();

                EFI_KEY_DATA keyData;
                while (UefiApplication.In->ReadKeyStrokeEx(out keyData) == EFI_STATUS.EFI_SUCCESS && PushBack(keyData))
                {
                    keyData.Dispose();
                }
                keyData.Dispose();
            }
        }
    }
}
