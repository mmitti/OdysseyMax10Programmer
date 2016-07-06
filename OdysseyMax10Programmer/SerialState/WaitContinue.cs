using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    public class WaitContinue : ISerialState
    {
        private static WaitContinue mInstance;
        private WaitContinue()
        {
        }
        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            string last = "";
            string str;
            do
            {
                while ((str = rw.TryReadLine()) != null)
                {
                    if (!String.IsNullOrWhiteSpace(str)) last = str;
                }
                if (last.StartsWith("--Press Enter"))
                {
                    rw.WriteLine();
                }
            } while (!last.StartsWith("S3 if using the MAX 10 board - when LED turns"));

            rw.TryReadLine(50);
            return MenuState.GetInstance().Exec(rw, s);

        }

        public static ISerialState GetInstance()
        {
            if(mInstance == null) mInstance = new WaitContinue();
            return mInstance;
        }
    }
}
