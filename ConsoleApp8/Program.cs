// See https://aka.ms/new-console-template for more information
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

class Program
{

    public static void Main()
    {
        var t = new Test(true);
        ref var a = ref t.GetReference();
        Console.WriteLine(a);
    }
}


struct Test
{
    private readonly nint _baseAddress;

    public Test(bool dummy)
    {
        _baseAddress = Native.VirtualAlloc(0, 1000, AllocationType.Commit, MemoryProtection.ReadWrite);
        if (_baseAddress == 0) throw new Exception();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public readonly ref byte GetReference() => ref Unsafe.AddByteOffset(ref Unsafe.NullRef<byte>(), _baseAddress);
}

static class Native
{
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern nint VirtualAlloc(nint lpAddress, nint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);
}

[Flags]
enum AllocationType
{
    Commit = 0x1000,
    Reserve = 0x2000,
    Decommit = 0x4000,
    Release = 0x8000,
    Reset = 0x80000,
    Physical = 0x400000,
    TopDown = 0x100000,
    WriteWatch = 0x200000,
    LargePages = 0x20000000
}

[Flags]
enum MemoryProtection
{
    Execute = 0x10,
    ExecuteRead = 0x20,
    ExecuteReadWrite = 0x40,
    ExecuteWriteCopy = 0x80,
    NoAccess = 0x01,
    ReadOnly = 0x02,
    ReadWrite = 0x04,
    WriteCopy = 0x08,
    GuardModifierflag = 0x100,
    NoCacheModifierflag = 0x200,
    WriteCombineModifierflag = 0x400
}
