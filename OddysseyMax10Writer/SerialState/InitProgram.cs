using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddysseyMax10Writer.SerialState
{
    class InitProgram : ISerialState
    {
        private static InitProgram mInstance;
        private InitProgram()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new InitProgram();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            if (s == TargetState.PROGRAM) return this;
            else if (s == TargetState.FINISH_PROGRAM)
            {
                String str;
                while (!(str = rw.ReadLine()).StartsWith("Please choose the above options"));
                rw.TryReadLine(500);
                rw.WriteLine("0");

                while (!(str = rw.ReadLine()).StartsWith("Complete")) ;

                return FinishWrite.GetInstance();

            }

            return null;
        }
    }
}
