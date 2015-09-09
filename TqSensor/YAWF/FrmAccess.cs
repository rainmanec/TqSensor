using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace YAWF
{
    public partial class FrmAccess : Form
    {
        public FrmAccess()
        {
            InitializeComponent();
        }

        private void FrmAccess_Load(object sender, EventArgs e)
        {
            string sql = "insert into record(tp, v1, v2) values (@p_tp, @p_v1, @p_v2)";
            string path = "Data Source =F:\\mydata.db";

            SQLiteParameter p_tp = Util.NewSQLiteParameter("@p_tp", DbType.DateTime, DateTime.Now);
            SQLiteParameter p_v1 = Util.NewSQLiteParameter("@p_v1", DbType.Decimal, 2.23);
            SQLiteParameter p_v2 = Util.NewSQLiteParameter("@p_v2", DbType.Decimal, 232.234);

            SQLiteHelper.ConnectionString = path;
            //int affect = SQLiteHelper.ExecuteNonQuery(sql, p_tp, p_v1, p_v2);
            //MessageBox.Show(affect.ToString());


        }
    }
}
