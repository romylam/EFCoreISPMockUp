using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public interface IMakeTransact
    {
        Transact MakeTransact();
    }
    public class MakeGeneralTransact : IMakeTransact
    {
        public Transact MakeTransact()
        {
            Transact buyHouse = new GeneralTransact();
            return buyHouse;
        }
    }
    public class MakeTradingTransact : IMakeTransact
    {
        public Transact MakeTransact()
        {
            Transact rentHouse = new TradingTransact();
            return rentHouse;
        }
    }
}
