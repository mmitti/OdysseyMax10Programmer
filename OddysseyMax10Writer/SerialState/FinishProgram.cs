using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    class FinishProgram : ISerialState
    {
        private static FinishProgram mInstance;
        private FinishProgram()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new FinishProgram();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            if (s == TargetState.FINISH_PROGRAM) return this;
            else if (s == TargetState.MENU)
            {
                return WaitContinue.GetInstance().Exec(rw, s);

            }
            return null;
        }
    }
}