// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// Changes made by Joshua Wierenga.

namespace System.Reflection
{
    public class MethodBody
    {
        protected MethodBody() { }
        public virtual int LocalSignatureMetadataToken => 0;
        //TODO Add IList<T> and LocalVariableInfo
        //public virtual IList<LocalVariableInfo> LocalVariables => throw new ArgumentNullException("array");
        public virtual int MaxStackSize => 0;
        public virtual bool InitLocals => false;
        public virtual byte[]? GetILAsByteArray() => null;
        //TODO Add IList<T> and ExceptionHandlingClause
        //public virtual IList<ExceptionHandlingClause> ExceptionHandlingClauses => throw new ArgumentNullException("array");
    }
}