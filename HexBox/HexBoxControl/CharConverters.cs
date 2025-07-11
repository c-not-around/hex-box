using System.Text;


namespace HexBoxControl
{
    public interface ICharConverter
    {
        char ToChar(byte data);

        byte ToByte(char c);
    }

    public class AnsiCharConverter : ICharConverter
    {
        public virtual char ToChar(byte data)
        {
            char c = (char)data;
            return (c < '!') || ('\x7e' < c && c < '\xa1') || (c == '\xad') ? '\0' : c;
        }

        public virtual byte ToByte(char c) => (byte)c;

        public override string ToString() => "ANSI";
    }

    public class AsciiCharConverter : ICharConverter
    {
        private Encoding _Encoding = Encoding.ASCII;

        public virtual char ToChar(byte data)
        {
            char? c = _Encoding.GetChars(new byte[1]{data})[0];
            return (c == null) || (c < '!') || (c == '\x7f') ? '\0' : (char)c;
        }

        public virtual byte ToByte(char c)
        {
            byte? b = _Encoding.GetBytes(new char[1]{c})[0];
            return (b != null) ? (byte)b : (byte)0;
        }

        public override string ToString() => "ASCII";
    }

    public class Utf8CharConverter : ICharConverter
    {
        private Encoding _Encoding = Encoding.UTF8;

        public virtual char ToChar(byte data)
        {
            char? c = _Encoding.GetChars(new byte[1]{data})[0];
            return (c == null) || (c < '!') || (c == '\x7f') ? '\0' : (char)c;
        }

        public virtual byte ToByte(char c)
        {
            byte? b = _Encoding.GetBytes(new char[1]{c})[0];
            return (b != null) ? (byte)b : (byte)0;
        }

        public override string ToString() => "UTF-8";
    }

    public class Cp1251CharConverter : ICharConverter
    {
        private Encoding _Encoding = Encoding.GetEncoding(1251);

        public virtual char ToChar(byte data)
        {
            char? c = _Encoding.GetChars(new byte[1]{data})[0];
            return (c == null) || (c < '!') || (c == '\x7f') || (c == '\xa0') || (c == '\xad') ? '\0' : (char)c;
        }

        public virtual byte ToByte(char c)
        {
            byte? b = _Encoding.GetBytes(new char[1]{c})[0];
            return (b != null) ? (byte)b : (byte)0;
        }

        public override string ToString() => "CP-1251";
    }
}