namespace module_14
{
    internal class Client
    {
        public object id;
        public object surname;
        public object name;
        public object patronymic;

        public Client(object ID, object Surname, object Name, object Patronymic)
        {
            this.id = ID;
            this.surname = Surname;
            this.name = Name;
            this.patronymic = Patronymic;
        }
        public object ID
        {
            get { return id; }
        }

        public object Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public object Name
        {
            get { return name; }
            set { name = value; }
        }
        public object Patronymic
        {
            get { return patronymic; }
            set { patronymic = value; }
        }

        public override string ToString() //формат вывода клиента
        {
            return $"{this.ID} {this.Surname} {this.Name} {this.Patronymic}";
        }
    }
}
