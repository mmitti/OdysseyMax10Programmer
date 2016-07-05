using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    public class InitialState : ISerialState
    {
        private static InitialState mInstance;
        private InitialState()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new InitialState();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            rw.WriteLine("");
            return WaitContinue.GetInstance().Exec(rw, s);
        }
    }
}
