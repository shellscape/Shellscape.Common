﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using Microsoft.WindowsAPI.Internal;

namespace Microsoft.WindowsAPI.Shell
{
    /// <summary>
    /// Represents the base class for all search-related classes.
    /// </summary>
    public class ShellSearchCollection : ShellContainer
    {
        internal ShellSearchCollection() { }

        internal ShellSearchCollection(IShellItem2 shellItem) : base(shellItem) { }
    }
}
