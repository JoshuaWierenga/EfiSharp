using EfiSharp;

namespace System
{
    public static partial class Console
    {
        //Circular Deque from https://github.com/LeoVen/C-Macro-Collections/blob/dba782ccd6254436e8fd72fb9342dabaaa7d3f2e/src/cmc_deque.h 
        private static unsafe partial class Buffer
        {
            private static EFI_KEY_DATA* _inputBuffer;
            private static int _inputBufferFront;
            private static int _inputBufferBack;
            private static int _inputBufferCount;
            //sizeof(EFI_KEY_DATA) = 9 bytes => sizeof(_inputBuffer) = 4.608kb
            private const ushort InputBufferCapacity = 512;
            private static int _walkPoint = -1;


            internal static void Init()
            {
                if (_inputBuffer != null) return;

                InitBuffer();
                Interrupts.InitInterrupts();
            }

            //TODO Move to static constructor
            private static void InitBuffer()
            {
                EFI_KEY_DATA* newBuffer = stackalloc EFI_KEY_DATA[InputBufferCapacity];
                _inputBuffer = newBuffer;
                _inputBufferFront = 0;
                _inputBufferBack = 0;
                _inputBufferCount = 0;
                
                Console.WriteLine("Buffer init!");
            }

            private static bool BufferEmpty => _inputBufferCount == 0;

            //TODO Make internal
            private static bool BufferFull => _inputBufferCount == InputBufferCapacity;

            //This returns false if the buffer is full and true otherwise indicating the item was added
            private static bool BufferPushBack(EFI_KEY_DATA item)
            {
                if (BufferFull)
                {
                    return false;
                }

                _inputBuffer[_inputBufferBack] = item;

                _inputBufferBack = _inputBufferBack == InputBufferCapacity - 1 ? 0 : _inputBufferBack + 1;
                _inputBufferCount++;

                return true;
            }

            //This returns false if there are no items in the buffer and true if an item has been returned
            internal static bool BufferPopFront(out EFI_KEY_DATA item)
            {
                if (BufferEmpty)
                {
                    item = new EFI_KEY_DATA();
                    return false;
                }

                item = _inputBuffer[_inputBufferFront];
                
                _inputBufferFront = _inputBufferFront == InputBufferCapacity - 1 ? 0 : _inputBufferFront + 1;
                //Walking is non destructive, note that BufferPushBack and BufferPopBack only check the count and never the front location so we are free to change it
                if (_walkPoint == -1)
                {
                    _inputBufferCount--;
                }

                return true;
            }

            //This returns false if there are no items in the buffer and true if an item has been returned
            private static bool BufferPopBack(out EFI_KEY_DATA item)
            {
                if (BufferEmpty)
                {
                    item = new EFI_KEY_DATA();
                    return false;
                }

                _inputBufferBack = _inputBufferBack == 0 ? InputBufferCapacity - 1 : _inputBufferBack - 1;
                _inputBufferCount--;

                //TODO Fix?
                //Potentially unsafe if there are interrupts since we shrink the array and then retrieve a value out of bounds
                item = _inputBuffer[_inputBufferBack];

                return true;
            }

            //Allows non destructive access to the buffer using BufferPopFront, the back operations work as usual
            internal static bool BeginWalkFront()
            {
                if (_walkPoint != -1)
                {
                    return false;
                }

                _walkPoint = _inputBufferFront;
                return true;
            }

            internal static bool EndWalkFront()
            {
                if (_walkPoint == -1) return false;

                _inputBufferFront = _walkPoint;
                return true;
            }


            //Adds a single key from the efi buffer into the console buffer if one exists and return a bool to indicate if it was successful
            //Only call if InitBuffer has been called already
            /*private static bool AddKey() =>
                UefiApplication.In->ReadKeyStrokeEx(out EFI_KEY_DATA keyData) == EFI_STATUS.EFI_SUCCESS &&
                //We cannot process keys like backspace and enter here since read and readkey handle them quite differently so just add them as is
                BufferPushBack(keyData);

            //TODO Make internal
            private static void FillBuffer()
            {
                InitBuffer();

                while (AddKey()) { }
            }

            //TODO Make internal
            //Returns a key from the console buffer, if the buffer is empty then execution will pause until the efi buffer has a key
            private static void GetKey(out EFI_KEY_DATA keyData)
            {
                InitBuffer();

                if (BufferPopFront(out keyData)) return;

                UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->_waitForKeyEx, out _);
                UefiApplication.In->ReadKeyStrokeEx(out keyData);
            }

            internal static void GetKeyTemp(out EFI_KEY_DATA keyData)
            {
                UefiApplication.SystemTable->BootServices->WaitForEvent(UefiApplication.In->_waitForKeyEx, out _);
                UefiApplication.In->ReadKeyStrokeEx(out keyData);
            }*/
        }
    }
}