using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using App_desctop_4.Data_base;

namespace App_desctop_4.Status
{
    public class lampStatus
    {
        public string lamp1Statu { get; set; }
        public string lamp2Statu { get; set; }
        public string lamp3Statu { get; set; }
        public string lamp4Statu { get; set; }
        public string timeLampChange { get; set; }

        public void insertDataBase()
        {
            dataBase dataBase = new dataBase();
            dataBase.InsertLampStatus(timeLampChange, lamp1Statu, lamp2Statu, lamp3Statu, lamp4Statu);
        }

        public void loadTable(DataGrid grid)
        {
            dataBase dataBase = new dataBase();
            dataBase.LoadLampStatusIntoDataGrid(grid);
        }
    }
}
