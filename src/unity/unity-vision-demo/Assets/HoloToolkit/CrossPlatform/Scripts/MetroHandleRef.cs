// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_METRO && !UNITY_EDITOR

using System;

namespace System.Runtime.InteropServices
{
    [ComVisible (true)]
    public struct HandleRef
    {
        object wrapper;
        IntPtr handle;