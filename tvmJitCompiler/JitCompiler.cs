using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace tvmJitCompiler
{
    [Flags]
    internal enum AllocationType
    {
        Commit      = 0x1000,
        Reserve     = 0x2000,
        Decommit    = 0x4000,
        Release     = 0x8000,
        Reset       = 0x80000,
        Physical    = 0x400000,
        TopDown     = 0x100000,
        WriteWatch  = 0x200000,
        LargePages  = 0x20000000
    }


    [Flags]
    internal enum MemoryProtection
    {
        Execute                     = 0x10,
        ExecuteRead                 = 0x20,
        ExecuteReadWrite            = 0x40,
        ExecuteWriteCopy            = 0x80,
        NoAccess                    = 0x01,
        ReadOnly                    = 0x02,
        ReadWrite                   = 0x04,
        WriteCopy                   = 0x08,
        GuardModifierflag           = 0x100,
        NoCacheModifierflag         = 0x200,
        WriteCombineModifierflag    = 0x400
    }

    public sealed class JitCompiler
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateThread(uint lpThreadAttributes, uint dwStackSize, uint lpStartAddress,
            IntPtr param, uint dwCreationFlags, ref uint lpThreadId);

        [DllImport("kernel32.dll")]
        private static extern bool VirtualFree(IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string dllToLoad);

        public void Compile()
        {
            IntPtr address = LoadLibrary("kernel32.dll");

            IntPtr pAddressOfFunctionToCall = GetProcAddress(address, "WriteFile");

            long funcadr = pAddressOfFunctionToCall.ToInt64();

            byte[] program =
                { 0xB8, 0x04, 0x00, 0x00, 0x00, 0xBA, 0x02, 0x00, 0x00, 0x00, 0xF7, 0xE2, 0xE8, 0x01, 0x00, 0x00, 0x00, 0xC3, 0x89, 0xC2, 0xC3 };
            IntPtr alloc = VirtualAlloc(IntPtr.Zero, (uint)program.Length, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);


            //List<byte> program = new List<byte>
            //{ 0xB8, 0x04, 0x00, 0x00, 0x00, 0xBA, 0x02, 0x00, 0x00, 0x00, 0xF7, 0xE2, 0xE8 };

            //long address = alloc.ToInt64();
            //address += 19;
            /*
            byte[] adr = BitConverter.GetBytes(address);

            foreach (byte b in adr) program.Add(b);

            program.Add(0xC3);
            program.Add(0x89);
            program.Add(0xC2);
            program.Add(0xC3);
            */

            Marshal.Copy(program, 0, alloc, program.Length);

            var processorInfo = new ProcessorInfo();
            var unmanagedProcessorInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ProcessorInfo)));
            Marshal.StructureToPtr(processorInfo, unmanagedProcessorInfo, false);

            uint threadId = 0;
            var hThread = CreateThread(0, 0, (uint)alloc.ToInt64(), unmanagedProcessorInfo, 0, ref threadId);
            _ = WaitForSingleObject(hThread, uint.MaxValue);

            Marshal.PtrToStructure(unmanagedProcessorInfo, typeof(ProcessorInfo));
            Marshal.FreeHGlobal(unmanagedProcessorInfo);
            //0xE8, 0x01, 0x00, 0x00, 0x00, 0xC3, 0x89, 0xC2, 0xC3

            CloseHandle(hThread);
            VirtualFree(alloc, 0, (uint)AllocationType.Release);
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct ProcessorInfo
        {
            public readonly uint dwMax;
            public readonly uint id0;
            public readonly uint id1;
            public readonly uint id2;
            public readonly uint dwStandard;
            public readonly uint dwFeature;
            public readonly uint dwExt;
        }


    }
}
