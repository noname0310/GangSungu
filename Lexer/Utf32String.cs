using System;
using System.Runtime.InteropServices;
using System.Text;
using Char32 = System.UInt32;

namespace GangSungu.Lexer;

public ref struct Utf32String
{
    public readonly ReadOnlySpan<Char32> Span;
    private unsafe byte* _str;

    public unsafe Utf32String(ReadOnlySpan<char> str)
    {
        var size = Encoding.UTF32.GetByteCount(str);
        _str = (byte*)Marshal.AllocHGlobal(size);
        
        fixed (char* strPtr = str)
            Encoding.UTF32.GetBytes(strPtr, str.Length, _str, size);
        Span = new(_str, size >> 2);
    }
    public void Dispose()
    {
        unsafe
        {
            if (_str == null)
                return;
            Marshal.FreeHGlobal((IntPtr)_str);
            _str = null;
        }
    }
    public override string ToString()
    {
        unsafe
        {
            return Encoding.UTF32.GetString(_str, Span.Length << 2);
        }
    }
}
