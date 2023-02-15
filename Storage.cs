using module_14;
using System.Collections.Generic;
using AccountLibrary;

namespace module_14
{
    internal class Storage
    {
        public List<Client> ClientsDB { get; set; } //база клиентов
        public List<Account<int, int, object, object>> AccountsDB { get; set; } //база счетов с параметризация типов данных

        private Storage()
        {
            ClientsDB = new List<Client>(); //создание новой базы клиентов
            AccountsDB = new List<Account<int, int, object, object>>(); //создание новой базы счетов

            ClientsDB.Add(new Client(1, "Петров", "Дмитрий", "Иванович"));
            AccountsDB.Add(new Account<int, int, object, object>(1, 1, "521534", 10000));
            AccountsDB.Add(new Account<int, int, object, object>(2, 1, "826493", 50));
            AccountsDB.Add(new Account<int, int, object, object>(3, 1, "906562", 9000));

            ClientsDB.Add(new Client(2, "Васильев", "Пётр", "Дмитриевич"));
            AccountsDB.Add(new Account<int, int, object, object>(4, 2, "165934", 12000));
            AccountsDB.Add(new Account<int, int, object, object>(5, 2, "381950", 185000));
            AccountsDB.Add(new Account<int, int, object, object>(6, 2, "194735", 300));
        }

        public static Storage CreateStorage() //создание репозитория с базами
        {
            return new Storage();
        }

        public void AccountAdd(int id, int clientID, object accountNumber, object accountSumm) //метод добавления нового счёта в базу
        {
            AccountsDB.Add(new Account<int, int, object, object>(id, clientID, accountNumber, accountSumm));
        }


    }


}
