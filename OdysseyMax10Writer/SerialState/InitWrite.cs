using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    class InitWrite : ISerialState
    {
        private static InitWrite mInstance;
        private InitWrite()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new InitWrite();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            if (s == TargetState.WRITE) return this;
            else if (s == TargetState.FINISH_WRITE)
            {
                String str;
                while (!(str = rw.ReadLine()).StartsWith("Enter two digit Personality number"));
                
                return FinishWrite.GetInstance();

            }
            else throw new Exception("Bad Status");

            return null;
        }
    }
}
