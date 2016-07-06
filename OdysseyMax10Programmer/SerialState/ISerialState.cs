using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    public interface ISerialState
    {

        ISerialState Exec(ISerialReadWrite rw, TargetState s);
    }
    public enum TargetState
    {
        MENU, WRITE, FINISH_WRITE, PROGRAM, FINISH_PROGRAM
    }
}
