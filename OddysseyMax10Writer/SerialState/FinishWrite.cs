using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddysseyMax10Writer.SerialState
{
    class FinishWrite : ISerialState
    {
        private static FinishWrite mInstance;
        private FinishWrite()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new FinishWrite();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            if (s == TargetState.FINISH_WRITE) return this;
            else if (s == TargetState.MENU)
            {
                return WaitContinue.GetInstance().Exec(rw, s);

            }
            return null;
        }
    }
}