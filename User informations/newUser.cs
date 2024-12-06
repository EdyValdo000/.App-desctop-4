using App_desctop_4.Data_base;

namespace App_desctop_4.User_informations
{
    public class newUser
    {
        public string userName { get; set; }
        public string password { get; set; }

        dataBase database = new dataBase();
        private string _userName;
        private string _password;

        public void newCount()
        {
            _userName = userName;
            _password = password;
            userName = null;
            password = null;
            database.InsertUser(_userName, _password);
        }
    }
}
