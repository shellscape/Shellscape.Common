﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPI.Shell
{
    [ComImport,
    Guid(ShellIIDGuid.IShellLibrary),
    CoClass(typeof(ShellLibraryCoClass))]
    internal interface INativeShellLibrary : IShellLibrary
    {
    }

    [ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(ShellCLSIDGuid.ShellLibrary)]
    internal class ShellLibraryCoClass
    {
    }
}
