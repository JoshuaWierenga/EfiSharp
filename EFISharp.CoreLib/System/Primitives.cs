using System.Runtime.CompilerServices;

namespace System
{
    public abstract class ValueType
    {
    }

    public abstract class Enum : ValueType
    {
    }

    public struct Boolean
    {
    }

    public struct Char
    {
    }

    public struct Byte 
    {  
    }

    public struct Int32
    {
    }

    public struct Int64
    {
    }

    public unsafe struct IntPtr
    {
        private void* _value;
        public IntPtr(void* value) { _value = value; }

        [Intrinsic]
        public static unsafe explicit operator void*(IntPtr value)
        {
            return value._value;
        }
    }


    //TODO Move
   
    public struct UInt32 { }
    
    public struct UInt64 { }
}
