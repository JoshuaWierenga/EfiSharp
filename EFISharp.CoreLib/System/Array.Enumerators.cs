// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Collections;

namespace System
{
    //TODO Add Clone
    internal sealed class ArrayEnumerator : IEnumerator//, ICloneable
    {
        private readonly Array _array;
        private nint _index;

        internal ArrayEnumerator(Array array)
        {
            _array = array;
            _index = -1;
        }

        //TODO Add MemberwiseClone
        /*public object Clone()
        {
            return MemberwiseClone();
        }*/

        public bool MoveNext()
        {
            nint index = _index + 1;
            if ((nuint)index >= _array.NativeLength)
            {
                _index = (nint)_array.NativeLength;
                return false;
            }
            _index = index;
            return true;
        }

        
        public object? Current
        {
            get
            {
                nint index = _index;
                Array array = _array;

                if ((nuint)index >= array.NativeLength)
                {
                    if (index < 0)
                    {
                        ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumNotStarted();
                    }
                    else
                    {
                        ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumEnded();
                    }
                }

                return array.InternalGetValue(index);
            }
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}
