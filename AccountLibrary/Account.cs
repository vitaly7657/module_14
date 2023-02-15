using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AccountLibrary
{
    public class Account<T1, T2, T3, T4> //создание параметризованного класса
    {
        public T1 ID { get; set; }
        public T2 ClientID { get; set; }
        public T3 AccountNumber { get; set; }
        public T4 AccountSumm { get; set; }


        public Account(T1 ID, T2 ClientID, T3 AccountNumber, T4 AccountSumm)
        {
            this.ID = ID;
            this.ClientID = ClientID;
            this.AccountNumber = AccountNumber;
            this.AccountSumm = AccountSumm;

        }

        public Account(int AccountSumm)
        {
            this.AccountNumber = AccountNumber;

        }


        public override string ToString() //формат вывода счёта
        {
            return $"{ID} {ClientID} {AccountNumber} {AccountSumm}";
        }



    }
}
