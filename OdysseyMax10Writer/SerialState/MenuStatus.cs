using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdysseyWriter.SerialState
{
    public class MenuState : ISerialState
    {
        private static MenuState mInstance;
        private MenuState()
        {

        }
        public static ISerialState GetInstance()
        {
            if (mInstance == null) mInstance = new MenuState();
            return mInstance;
        }

        public ISerialState Exec(ISerialReadWrite rw, TargetState s)
        {
            if (s == TargetState.MENU) return this;
            else if (s == TargetState.WRITE)
            {
                rw.WriteLine("5");
                String str;
                while (!(str = rw.ReadLine()).StartsWith("Binary option box is checked before pressing OK.")) ;
                rw.TryReadLine();
                return InitWrite.GetInstance();
            }
            else if(s == TargetState.PROGRAM)
            {
                rw.WriteLine("6");
                String str;
                while (!(str = rw.ReadLine()).StartsWith("Enter two digit Personality number")) ;
                return InitProgram.GetInstance();
            }

            return null;
        }
    }
}
