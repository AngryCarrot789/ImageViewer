﻿using System;
using System.Runtime.InteropServices;

namespace ImageViewer.RecycleBin
{
    /// <summary>
    /// SHFILEOPSTRUCT for SHFileOperation from COM
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.U4)]
        public FileOperationType wFunc;
        public string pFrom;
        public string pTo;
        public FileOperationFlags fFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public string lpszProgressTitle;
    }
}
