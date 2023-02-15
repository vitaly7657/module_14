using module_14;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace module_14
{
    class AmountAccount //класс муппирования счетов перевода средств
    {
        public int SummAccount { get; set; }
        public static AmountAccount operator +(AmountAccount a, AmountAccount b) //перегрузка операции суммирования
        {
            return new AmountAccount() { SummAccount = a.SummAccount + b.SummAccount };
        }
    }

    public static class HelpWindow //класс для метода расширения окна помощи через кнопку [?]
    {
        public static void ShowHelpWindow(this string a)
        {
            MessageBox.Show(a);
        }

    }

    public partial class MainWindow : Window
    {
        public static event Action<string, string> AddAccountNotification; //создание делегата события на добавление счёта
        public static event Action<string, string> CloseAccountNotification; //создание делегата события на закрытие счёта
        public static event Action<string, string, string, string> TransferAccountNotification; //создание делегата события на перевод средств между счетами
        public static event Action<string, string, string> DepositAccountNotification; //создание делегата события на пополнение счёта
        static List<string> notificationList = new List<string>(); //создание списка уведомлений

        public void NotificationListFill() //метод обновления списка уведомлений
        {
            lb_journal.Items.Refresh();
        }

        public static void AddAccountInfo(string user, string accountNumber) //метод уведомлений при создании счёта
        {
            string messageItem = $"{DateTime.Now} | {user} открыл счёт {accountNumber}";
            MessageBox.Show(messageItem);
            notificationList.Add(messageItem);

        }

        public static void CloseAccountInfo(string user, string accountNumber) //метод уведомлений при закрытии счёта
        {
            string messageItem = $"{DateTime.Now} | {user} закрыл счёт {accountNumber}";
            MessageBox.Show(messageItem);
            notificationList.Add(messageItem);
        }

        public static void TransferAccountInfo(string user, string summ, string accountFrom, string accountTo) //метод уведомлений при переводе средств между счетами
        {
            string messageItem = $"{DateTime.Now} | {user} перевёл сумму {summ} рублей со счёта {accountFrom} на счёт {accountTo}";
            MessageBox.Show(messageItem);
            notificationList.Add(messageItem);
        }

        public static void DepositAccountInfo(string user, string depositAccount, string depositSumm) //метод уведомлений при пополнении счёта
        {
            string messageItem = $"{DateTime.Now} | {user} пополнил счёт {depositAccount} на {depositSumm} рублей";
            MessageBox.Show(messageItem);
            notificationList.Add(messageItem);
        }

        Storage clientsData;

        public void SelectedFill() //метод выбора счёта
        {
            if (lv_accounts.SelectedItem != null)
            {
                string[] selectedNumberAcc = (lv_accounts.SelectedItem.ToString()).Split(' '); //разделение строки данных выбанного в lv_accounts счёта на части через разделитель пробел
                string numberAcc = selectedNumberAcc[2]; //номер выбранного счёта
                tb_selected_acc.Text = numberAcc; //номер выбранного счёта записать в tb_selected_acc
                tb_deposit_acc.Text = numberAcc; //номер выбранного счёта записать в tb_selected_acc
            }
        }

        public void AccountsFill() //метод заполнения lv_accounts
        {
            string[] clientListID = (lv_clients.SelectedItem.ToString()).Split(' '); //разделение строки данных выбанного в lv_clients клиента на части через разделитель пробел
            int clID = Convert.ToInt32(clientListID[0]); //извлечение номера клиента из массива                       
            lv_accounts.ItemsSource = clientsData.AccountsDB.FindAll(a => a.ClientID == clID); //заполнение lv_accounts на основе AccountsDB с данным номером клиента
        }

        public void ButtonsStatusChange(bool buttonStatus) //метод группового изменения состояния кнопок в зависимости от пользователя
        {
            btn_del_acc.IsEnabled = buttonStatus;
            btn_deposit_acc.IsEnabled = buttonStatus;
            btn_add_acc.IsEnabled = buttonStatus;
            btn_transfer.IsEnabled = buttonStatus;
            tb_deposit_summ.IsEnabled = buttonStatus;
            tb_new_acc.IsEnabled = buttonStatus;
        }

        public void SourceManage() //метод объявления объектам на форме источников данных
        {
            lv_clients.ItemsSource = clientsData.ClientsDB; //заполнение lv_clients из базы клиентов
            lv_clients.SelectedIndex = 0;
            cb_from_client.ItemsSource = clientsData.ClientsDB; //источник данных для cb_from_client
            cb_to_client.ItemsSource = clientsData.ClientsDB; //источник данных для cb_to_client
        }

        static string currentUser; //статическая переменная текущего пользователя

        public MainWindow()
        {
            InitializeComponent();
            clientsData = Storage.CreateStorage(); //создание хранилища
            ButtonsStatusChange(false); //состояние кнопок по умолчанию
            lv_clients.Visibility = Visibility.Hidden; //состояние таблицы с клиентами по умолчанию
            lv_accounts.Visibility = Visibility.Hidden; //состояние таблицы со счетами по умолчанию
            tb_selected_acc.IsEnabled = false;
            tb_deposit_acc.IsEnabled = false;
            AddAccountNotification += AddAccountInfo;
            CloseAccountNotification += CloseAccountInfo;
            TransferAccountNotification += TransferAccountInfo;
            DepositAccountNotification += DepositAccountInfo;
            lb_journal.ItemsSource = notificationList;
        }

        private void lv_clients_SelectionChanged(object sender, SelectionChangedEventArgs e) //действие при выборе строчки в lv_clients
        {
            AccountsFill();
        }

        private void lv_accounts_SelectionChanged(object sender, SelectionChangedEventArgs e) //действие при выборе строчки в lv_accounts
        {
            SelectedFill();
        }

        private void btn_del_acc_Click(object sender, RoutedEventArgs e) //кнопка удаления счёта
        {
            if (tb_selected_acc.Text != "выбранный счёт")
            {
                object[] selectedNumberAcc = (lv_accounts.SelectedItem.ToString()).Split(' '); //разделение строки данных выбранного в lv_accounts счёт через разделитель пробел
                int deleteAccID = Convert.ToInt32(selectedNumberAcc[0]); //извленение ID счёта через первый аргумент
                var deleteAcc = clientsData.AccountsDB.FirstOrDefault(p => p.ID == deleteAccID); //поиск в AccountsDB счёта с данным ID
                clientsData.AccountsDB.Remove(deleteAcc); //удаление счёта
                AccountsFill();
                CloseAccountNotification?.Invoke(currentUser, tb_selected_acc.Text);
                NotificationListFill();
            }
            else
            {
                MessageBox.Show("Выберете счёт из списка!"); //вывод окна уведомления о неверных данных
            }
        }

        private void btn_add_acc_Click(object sender, RoutedEventArgs e) //кнопка добавления счёта
        {
            string newAccount = tb_new_acc.Text;
            int Num;
            bool isNum = int.TryParse(newAccount, out Num);

            try
            {
                if (newAccount == "") //проверка заполнения поля нового счёта
                {
                    throw new EmptyFieldException("не заполнено поле счёта, заполните его");
                }

                if (!isNum) //проверка на счёта на тип int
                {
                    throw new WrongTypeException("счёт должен быть числом");
                }

                if (newAccount.Length != 6) //проверка длины счёта
                {
                    throw new WrongLengthException("длина счёта должна быть 6 символов");
                }


                else
                {
                    object maxAccID = clientsData.AccountsDB.Max(point => point.ID); //поиск в базе AccountsDB максимального ID
                    object[] clientListID = (lv_clients.SelectedItem.ToString()).Split(' '); //разделение строки данных выбранного в lv_clients счёта на части через разделитель пробел
                    int clID = Convert.ToInt32(clientListID[0]);
                    clientsData.AccountAdd(Convert.ToInt32(maxAccID) + 1, clID, tb_new_acc.Text, 0); //добавление нового счёта
                    AccountsFill();
                    AddAccountNotification?.Invoke(currentUser, tb_new_acc.Text);
                    NotificationListFill();
                }
            }

            catch (EmptyFieldException k)
            {
                MessageBox.Show($"Ошибка: {k.ExceptionMessage}"); // окно ошибки о пустом поле
            }

            catch (WrongTypeException k)
            {
                MessageBox.Show($"Ошибка: {k.ExceptionMessage}"); //  окно ошибки о неврном формате данных в поле
            }

            catch (WrongLengthException k)
            {
                MessageBox.Show($"Ошибка: {k.ExceptionMessage}"); //  окно ошибки о неверной длине данных в поле
            }
        }

        private void cb_from_client_SelectionChanged(object sender, SelectionChangedEventArgs e) //действие при выборе в cb_from_client
        {
            string[] clientListID = (cb_from_client.SelectedItem.ToString()).Split(' '); //разделение строки данных выбанного в cb_from_client клиента на части через разделитель пробел
            int clID = Convert.ToInt32(clientListID[0]); //извлечение ID клиента                     
            cb_from_account.ItemsSource = clientsData.AccountsDB.FindAll(a => a.ClientID == clID); //заполнение cb_from_account на основе ID клиента
            cb_from_account.SelectedIndex = 0; //нулевой индекс по умолчанию   
        }

        private void cb_to_client_SelectionChanged(object sender, SelectionChangedEventArgs e) //действие при выборе в cb_to_client
        {
            string[] clientListID = (cb_to_client.SelectedItem.ToString()).Split(' '); //разделение строки данных выбанного в cb_to_client клиента на части через разделитель пробел
            int clID = Convert.ToInt32(clientListID[0]); //извлечение ID клиента                         
            cb_to_account.ItemsSource = clientsData.AccountsDB.FindAll(a => a.ClientID == clID); //заполнение cb_to_account на основе ID клиента
            cb_to_account.SelectedIndex = 0; //нулевой индекс по умолчанию 
        }

        private void btn_transfer_Click(object sender, RoutedEventArgs e) //кнопка перевода средств
        {
            if (cb_from_client.SelectedIndex != -1 && cb_to_client.SelectedIndex != -1 && cb_from_account.SelectedIndex != -1 && cb_to_account.SelectedIndex != -1) //проверка на выбор данных
            {
                object[] tempFrom = (cb_from_account.SelectedItem.ToString()).Split(' '); //извлечение массива из cb_from_account
                string accountFrom = Convert.ToString(tempFrom[2]); //номер счёта от кого
                int summFrom = Convert.ToInt32(tempFrom[3]); //сумма на счёте от кого


                object[] tempTo = (cb_to_account.SelectedItem.ToString()).Split(' '); //извлечение массива из cb_to_account
                string accountTo = Convert.ToString(tempTo[2]); //номер счёта кому
                int summTo = Convert.ToInt32(tempTo[3]); //сума на счёте кому

                //реализация перегрузки операции суммирования классов
                AmountAccount from = new AmountAccount() { SummAccount = summFrom };
                AmountAccount to = new AmountAccount() { SummAccount = summTo };
                AmountAccount amount = from + to;
                tb_transfer_amount.Text = amount.SummAccount.ToString();

                int transferSumm = Convert.ToInt32(tb_transfer_summ.Text); //данные из поля tb_transfer_summ как сумма перевода
                int chechSumm = summFrom - transferSumm; //переменная проверки счедств на счёте
                if (chechSumm < 0) //проверка счедств на счёте
                {
                    MessageBox.Show("Недостаточно средств на счёте для осуществления перевода!");
                }
                else if (chechSumm >= 0) //проверка счедств на счёте
                {
                    summFrom -= transferSumm; //формирование суммы на счёте от кого
                    summTo += transferSumm; //формирование суммы на счёте кому
                    TransferAccountNotification?.Invoke(currentUser, transferSumm.ToString(), accountFrom, accountTo);
                    NotificationListFill();
                }
                clientsData.AccountsDB.FindAll(a => a.AccountNumber.ToString() == accountFrom).ForEach(b => b.AccountSumm = summFrom); //запись полученной суммы на счёт от кого
                clientsData.AccountsDB.FindAll(a => a.AccountNumber.ToString() == accountTo).ForEach(b => b.AccountSumm = summTo); //запись полученной суммы на счёт кому
                AccountsFill();
            }
            else
            {
                MessageBox.Show("Выберете все значения!"); //вывод окна уведомления о неверных данных
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e) //кнопка авторизации
        {
            if (tb_login.Text == "consultant" && tb_password.Text == "consultant") //проверка ввода данных консультанта
            {
                currentUser = "consultant";
                SourceManage();
                ButtonsStatusChange(false);
                lv_clients.Visibility = Visibility.Visible;
                lv_accounts.Visibility = Visibility.Visible;
                tb_current_user.Text = "consultant";

            }
            else if (tb_login.Text == "manager" && tb_password.Text == "manager") //проверка ввода данных менеджера
            {
                currentUser = "manager";
                SourceManage();
                ButtonsStatusChange(true);
                lv_clients.Visibility = Visibility.Visible;
                lv_accounts.Visibility = Visibility.Visible;
                tb_current_user.Text = "manager";


            }
            else
            {
                MessageBox.Show("Логин или пароль неверный!"); //вывод окна уведомления о неверных данных
            }
            tb_login.Text = "";
            tb_password.Text = "";

        }

        private void btn_logout_Click(object sender, RoutedEventArgs e) //кнопка выхода пользователя
        {
            ButtonsStatusChange(false);
            lv_clients.Visibility = Visibility.Hidden;
            lv_accounts.Visibility = Visibility.Hidden;
            tb_current_user.Text = "";
        }

        private void btn_deposit_acc_Click(object sender, RoutedEventArgs e)
        {
            string textDeposit = tb_deposit_summ.Text.Trim();
            int Num;
            bool isNum = int.TryParse(textDeposit, out Num);
            if (isNum) //проверка поля на число
            {
                if (tb_deposit_acc.Text != "выбранный счёт") //проверка поля на выбор счёта
                {
                    int summAdd = Convert.ToInt32(tb_deposit_summ.Text); //сумма пополнения счёта изполя tb_deposit_summ
                    object[] selectedNumberAcc = (lv_accounts.SelectedItem.ToString()).Split(' '); //разделение строки данных выбранного в lv_accounts счёт через разделитель пробел
                    int depositAccID = Convert.ToInt32(selectedNumberAcc[0]); //извлечение ID счёта через первый аргумент
                    object accountSumm = selectedNumberAcc[3];
                    int newAccSumm = Convert.ToInt32(accountSumm) + summAdd;

                    clientsData.AccountsDB.FindAll(a => a.ID == depositAccID).ForEach(b => b.AccountSumm = newAccSumm);

                    AccountsFill();

                    DepositAccountNotification?.Invoke(currentUser, tb_deposit_acc.Text, summAdd.ToString());
                    NotificationListFill();
                }
                else
                {
                    MessageBox.Show("Выберете счёт из списка!"); //вывод окна уведомления о неверных данных
                }

            }
            else if ((textDeposit == "") || (!isNum))
            {
                MessageBox.Show("Введены неверные данные!"); //вывод окна уведомления о неверных данных
            }
        }

        private void btn_help_Click(object sender, RoutedEventArgs e) //кнопка помощи
        {
            //реализация метода расширения для окна помощи через кнопку [?]
            string helpMessage = "Пользователи:\n1. логин: manager, пароль: manager\n2. логин: consultant, пароль: consultant";
            helpMessage.ShowHelpWindow();
        }
    }

}
