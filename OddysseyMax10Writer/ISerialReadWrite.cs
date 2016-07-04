using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter
{
    public interface ISerialReadWrite
    {
        void WriteLine(string s = "");
        string ReadLine();
        string TryReadLine(int ms = 1000);
    }
}
