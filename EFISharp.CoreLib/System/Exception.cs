﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

using System.Diagnostics;

namespace System
{
    [Serializable]
    //TODO Add ISerializable
    [System.Runtime.CompilerServices.TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
    public partial class Exception// : ISerializable
    {
        private protected const string InnerExceptionPrefix = " ---> ";

        public Exception()
        {
            _HResult = HResults.COR_E_EXCEPTION;
        }

        //TODO Support nullable
        //public Exception(string? message)
        public Exception(string message)
            : this()
        {
            _message = message;
        }

        // Creates a new Exception.  All derived classes should
        // provide this constructor.
        // Note: the stack trace is not started until the exception
        // is thrown
        //
        //TODO Support nullable
        //public Exception(string? message, Exception? innerException)
        public Exception(string message, Exception innerException)
            : this()
        {
            _message = message;
            _innerException = innerException;
        }

        //TODO Add SerializationInfo, StreamingContext and IDictionary
        /*protected Exception(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            _message = info.GetString("Message"); // Do not rename (binary serialization)
            _data = (IDictionary?)(info.GetValueNoThrow("Data", typeof(IDictionary))); // Do not rename (binary serialization)
            _innerException = (Exception?)(info.GetValue("InnerException", typeof(Exception))); // Do not rename (binary serialization)
            _helpURL = info.GetString("HelpURL"); // Do not rename (binary serialization)
            _stackTraceString = info.GetString("StackTraceString"); // Do not rename (binary serialization)
            _remoteStackTraceString = info.GetString("RemoteStackTraceString"); // Do not rename (binary serialization)
            _HResult = info.GetInt32("HResult"); // Do not rename (binary serialization)
            _source = info.GetString("Source"); // Do not rename (binary serialization)

            RestoreRemoteStackTrace(info, context);
        }*/

        //TODO Add SR.Format, GetClassName and generate the full SR file
        //public virtual string Message => _message ?? SR.Format(SR.Exception_WasThrown, GetClassName());
        public virtual string Message => _message;

        //TODO Add IDictionary and CreateDataContainer
        //public virtual IDictionary Data => _data ??= CreateDataContainer();

        //TODO Add Type.ToString
        //private string GetClassName() => GetType().ToString();

        // Retrieves the lowest exception (inner most) for the given Exception.
        // This will traverse exceptions using the innerException property.
        public virtual Exception GetBaseException()
        {
            //TODO Support nullable
            //Exception? inner = InnerException;
            Exception inner = InnerException;
            Exception back = this;

            while (inner != null)
            {
                back = inner;
                inner = inner.InnerException;
            }

            return back;
        }

        //TODO Support nullable
        //public Exception? InnerException => _innerException;
        public Exception InnerException => _innerException;

        // Sets the help link for this exception.
        // This should be in a URL/URN form, such as:
        // "file:///C:/Applications/Bazzal/help.html#ErrorNum42"
        //TODO Fix nullable
        //public virtual string? HelpLink
        public virtual string HelpLink
        {
            get => _helpURL;
            set => _helpURL = value;
        }

        //TODO Fix nullable
        //public virtual string? Source
        public virtual string Source
        {
            get => _source ??= CreateSourceName();
            set => _source = value;
        }


        //TODO Add SerializationInfo, StreamingContext, IDictionary, GetClassName and SerializationStackTraceString
        /*public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            if (_source == null)
            {
                _source = Source; // Set the Source information correctly before serialization
            }

            info.AddValue("ClassName", GetClassName(), typeof(string)); // Do not rename (binary serialization)
            info.AddValue("Message", _message, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("Data", _data, typeof(IDictionary)); // Do not rename (binary serialization)
            info.AddValue("InnerException", _innerException, typeof(Exception)); // Do not rename (binary serialization)
            info.AddValue("HelpURL", _helpURL, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("StackTraceString", SerializationStackTraceString, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("RemoteStackTraceString", _remoteStackTraceString, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("RemoteStackIndex", 0, typeof(int)); // Do not rename (binary serialization)
            info.AddValue("ExceptionMethod", null, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("HResult", _HResult); // Do not rename (binary serialization)
            info.AddValue("Source", _source, typeof(string)); // Do not rename (binary serialization)
            info.AddValue("WatsonBuckets", SerializationWatsonBuckets, typeof(byte[])); // Do not rename (binary serialization)
        }*/

        //TODO Add GetClassName, StackTrace, Environment Span<T> and String.AsSpan
        /*public override string ToString()
        {
            string className = GetClassName();
            string? message = Message;
            string innerExceptionString = _innerException?.ToString() ?? "";
            string endOfInnerExceptionResource = SR.Exception_EndOfInnerExceptionStack;
            string? stackTrace = StackTrace;

            // Calculate result string length
            int length = className.Length;
            checked
            {
                if (!string.IsNullOrEmpty(message))
                {
                    length += 2 + message.Length;
                }
                if (_innerException != null)
                {
                    length += Environment.NewLineConst.Length + InnerExceptionPrefix.Length + innerExceptionString.Length + Environment.NewLineConst.Length + 3 + endOfInnerExceptionResource.Length;
                }
                if (stackTrace != null)
                {
                    length += Environment.NewLineConst.Length + stackTrace.Length;
                }
            }

            // Create the string
            string result = string.FastAllocateString(length);
            Span<char> resultSpan = new Span<char>(ref result.GetRawStringData(), result.Length);

            // Fill it in
            Write(className, ref resultSpan);
            if (!string.IsNullOrEmpty(message))
            {
                Write(": ", ref resultSpan);
                Write(message, ref resultSpan);
            }
            if (_innerException != null)
            {
                Write(Environment.NewLineConst, ref resultSpan);
                Write(InnerExceptionPrefix, ref resultSpan);
                Write(innerExceptionString, ref resultSpan);
                Write(Environment.NewLineConst, ref resultSpan);
                Write("   ", ref resultSpan);
                Write(endOfInnerExceptionResource, ref resultSpan);
            }
            if (stackTrace != null)
            {
                Write(Environment.NewLineConst, ref resultSpan);
                Write(stackTrace, ref resultSpan);
            }
            Debug.Assert(resultSpan.Length == 0);

            // Return it
            return result;

            static void Write(string source, ref Span<char> dest)
            {
                source.CopyTo(dest);
                dest = dest.Slice(source.Length);
            }
        }*/

        //TODO Add EventHandler, SafeSerializationEventArgs and PlatformNotSupportedException(String)
        /* [Obsolete(Obsoletions.BinaryFormatterMessage, DiagnosticId = Obsoletions.BinaryFormatterDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        protected event EventHandler<SafeSerializationEventArgs>? SerializeObjectState
        {
            add { throw new PlatformNotSupportedException(SR.PlatformNotSupported_SecureBinarySerialization); }
            remove { throw new PlatformNotSupportedException(SR.PlatformNotSupported_SecureBinarySerialization); }
        }*/

        public int HResult
        {
            get => _HResult;
            set => _HResult = value;
        }

        // this method is required so Object.GetType is not made virtual by the compiler
        // _Exception.GetType()
        //TODO Object.GetType
        //public new Type GetType() => base.GetType();

        //Add SerializationInfo and StreamingContext
        //partial void RestoreRemoteStackTrace(SerializationInfo info, StreamingContext context);

        // Returns the stack trace as a string.  If no stack trace is
        // available, null is returned.
        //TODO Add StackTrace
        /*public virtual string? StackTrace
        {
            get
            {
                string? stackTraceString = _stackTraceString;
                string? remoteStackTraceString = _remoteStackTraceString;

                // if no stack trace, try to get one
                if (stackTraceString != null)
                {
                    return remoteStackTraceString + stackTraceString;
                }
                if (!HasBeenThrown)
                {
                    return remoteStackTraceString;
                }

                return remoteStackTraceString + GetStackTrace();
            }
        }

        private string GetStackTrace()
        {
            // Do not include a trailing newline for backwards compatibility
            return new StackTrace(this, fNeedFileInfo: true).ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
        }*/

        [StackTraceHidden]
        internal void SetCurrentStackTrace()
        {
            //if (!CanSetRemoteStackTrace())
            {
                return; // early-exit
            }

            //TODO Add CanSetRemoteStackTrace, StringBuilder, StackTrace, SR.Exception_EndStackTraceFromPreviousThrow and _remoteStackTraceString
            // Store the current stack trace into the "remote" stack trace, which was originally introduced to support
            // remoting of exceptions cross app-domain boundaries, and is thus concatenated into Exception.StackTrace
            // when it's retrieved.
            /*var sb = new StringBuilder(256);
            new StackTrace(fNeedFileInfo: true).ToString(System.Diagnostics.StackTrace.TraceFormat.TrailingNewLine, sb);
            sb.AppendLine(SR.Exception_EndStackTraceFromPreviousThrow);
            _remoteStackTraceString = sb.ToString();*/
        }

        //TODO Add CanSetRemoveStackTrace, _remoteStackTraceString, Environment and SR.Exception_EndStackTraceFromPreviousThrow
        /*internal void SetRemoteStackTrace(string stackTrace)
        {
            if (!CanSetRemoteStackTrace())
            {
                return; // early-exit
            }

            // Store the provided text into the "remote" stack trace, following the same format SetCurrentStackTrace
            // would have generated.
            _remoteStackTraceString = stackTrace + Environment.NewLineConst + SR.Exception_EndStackTraceFromPreviousThrow + Environment.NewLineConst;
        }*/

        //TODO Add GetStackTrace
        /*private string? SerializationStackTraceString
        {
            get
            {
                string? stackTraceString = _stackTraceString;

                if (stackTraceString == null && HasBeenThrown)
                {
                    stackTraceString = GetStackTrace();
                }

                return stackTraceString;
            }
        }*/
    }
}
