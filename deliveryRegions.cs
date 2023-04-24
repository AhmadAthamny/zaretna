using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ZaretnaPanel
{
    public partial class deliveryRegions : Form
    {
        public List<int> deletedRegions = new List<int>();
        private string LastValue;

        // adding region
        int a_DeliveryPrice, a_regionID;
        string a_A_NAME, a_H_NAME;
        bool a_addingNew = false;
        LoadingForm LDFRM;
        public deliveryRegions()
        {
            InitializeComponent();
            SetupTable();
        }
        public void SetupTable()
        {
            if (regionsTable.Rows.Count == 0)
            {
                regionsTable.Columns.Add("regionID", "#");
                regionsTable.Columns.Add("A_NAME", "אישור בערבית");
                regionsTable.Columns.Add("H_NAME", "איזור בעברית");
                regionsTable.Columns.Add("D_PRICE", "מחיר משלוח");
            }

            regionsTable.Columns[0].ReadOnly = true;

            regionsTable.Columns[0].FillWeight = 10;
            regionsTable.Columns[1].FillWeight = 35;
            regionsTable.Columns[2].FillWeight = 35;
            regionsTable.Columns[3].FillWeight = 20;

            regionsTable.Columns[0].HeaderText = "#";
            regionsTable.Columns[1].HeaderText = "אישור בערבית";
            regionsTable.Columns[2].HeaderText = "איזור בעברית";
            regionsTable.Columns[3].HeaderText = "מחיר משלוח";


            DataTable dt = new API_CLASS().API_QUERY("SELECT ID, A_NAME, H_NAME, DELIVERY_PRICE FROM regions");
            foreach (DataRow row in dt.Rows)
            {
                regionsTable.Rows.Add(int.Parse(row.Field<string>("ID")), row.Field<string>("A_NAME"), row.Field<string>("H_NAME"), row.Field<string>("DELIVERY_PRICE"));
            }

            regionsTable.Columns[0].Visible = false;

            regionsTable.DefaultCellStyle.NullValue = null;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool foundNew = false;
            foreach(DataGridViewRow row in regionsTable.Rows)
            {
                if(int.Parse(row.Cells[0].Value.ToString()) == -2)
                {
                    regionsTable.ClearSelection();
                    regionsTable.CurrentCell = row.Cells[1];
                    foundNew = true;
                    break;
                }
            }
            if (!foundNew)
            {
                regionsTable.ClearSelection();
                regionsTable.Rows.Add(-2, "إسم البلدة بالعربية", "שם איזור בעברית", "מחיר משלוח");
                regionsTable.Rows[regionsTable.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                regionsTable.CurrentCell = regionsTable.Rows[regionsTable.Rows.Count - 1].Cells[1];
            }
        }

        private void regionsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {  
            // It works, but commented because it's not needed rn.
            /*if(!regionsTable.Rows[e.RowIndex].IsNewRow)
            {
                if (regionsTable.Rows[e.RowIndex].Cells[0].Value != null && regionsTable.Rows[e.RowIndex].Cells[0].Value != DBNull.Value)
                {
                    RemoveRegionForm RRF = new RemoveRegionForm(this, Convert.ToInt32(regionsTable.Rows[e.RowIndex].Cells[0].Value), e.RowIndex);
                    RRF.ShowDialog(this);
                }
            }*/
        }

        private void regionsTable_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int i = e.RowIndex;
            if(e.RowIndex != -1)
            {
                if(int.Parse(regionsTable.Rows[i].Cells[0].Value.ToString()) != -2)
                {
                    this.LastValue = regionsTable.Rows[i].Cells[e.ColumnIndex].Value.ToString();
                }
            }
        }

        private void regionsTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i != -1)
            {
                if (regionsTable.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != this.LastValue)
                {
                    // adding region
                    if (int.Parse(regionsTable.Rows[i].Cells[0].Value.ToString()) == -2)
                    {
                        if (regionsTable.Rows[i].Cells[1].Value != null && regionsTable.Rows[i].Cells[1].Value != DBNull.Value
                           && regionsTable.Rows[i].Cells[2].Value != null && regionsTable.Rows[i].Cells[2].Value != DBNull.Value
                           && regionsTable.Rows[i].Cells[3].Value != null && regionsTable.Rows[i].Cells[3].Value != DBNull.Value
                        )
                        {
                            if (!string.IsNullOrWhiteSpace(regionsTable.Rows[i].Cells[1].Value.ToString())
                                && !string.IsNullOrWhiteSpace(regionsTable.Rows[i].Cells[2].Value.ToString())
                                && !string.IsNullOrWhiteSpace(regionsTable.Rows[i].Cells[3].Value.ToString())
                            )
                            {
                                int deliveryPrice = -1;
                                bool isInt = false;
                                isInt = int.TryParse(regionsTable.Rows[i].Cells[3].Value.ToString(), out deliveryPrice);
                                if (isInt && deliveryPrice > 0)
                                {
                                    if (regionsTable.Rows[i].Cells[1].Value.ToString().Length > 0
                                        && regionsTable.Rows[i].Cells[2].Value.ToString().Length > 0
                                    )
                                    {
                                        this.a_addingNew = true;
                                        this.a_DeliveryPrice = deliveryPrice;
                                        this.a_A_NAME = MySqlHelper.EscapeString(regionsTable.Rows[i].Cells[1].Value.ToString());
                                        this.a_H_NAME = MySqlHelper.EscapeString(regionsTable.Rows[i].Cells[2].Value.ToString());
                                        regionsTable.ReadOnly = true;
                                        this.LDFRM = new LoadingForm();
                                        this.LDFRM.label1.Text = "הוספת איזור משלוח";
                                        this.LDFRM.Show();
                                        this.worker_addRegion.RunWorkerAsync();
                                    }
                                }
                            }
                        }
                    }
                    else // updating region
                    {
                        if (e.ColumnIndex == 3)
                        {
                            int tmpIntValue;
                            bool isInt = int.TryParse(regionsTable.Rows[i].Cells[e.ColumnIndex].Value.ToString(), out tmpIntValue);
                            if (!isInt || tmpIntValue < 0)
                            {
                                regionsTable.Rows[i].Cells[e.ColumnIndex].Value = LastValue;
                            }
                            else
                            {
                                regionsTable.Rows[i].Cells[e.ColumnIndex].Value = tmpIntValue;

                                this.a_regionID = int.Parse(regionsTable.Rows[i].Cells[0].Value.ToString());
                                this.a_DeliveryPrice = tmpIntValue;
                                this.a_A_NAME = null;
                                this.a_H_NAME = null;
                                this.a_addingNew = false;
                                regionsTable.ReadOnly = true;
                                LDFRM = new LoadingForm();
                                LDFRM.label1.Text = "עדכון מחיר משלוח";
                                LDFRM.Show();
                                this.worker_addRegion.RunWorkerAsync();
                            }
                        }
                        else if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                        {
                            if (regionsTable.Rows[i].Cells[e.ColumnIndex].Value == null || regionsTable.Rows[i].Cells[e.ColumnIndex].Value == DBNull.Value)
                                regionsTable.Rows[i].Cells[e.ColumnIndex].Value = this.LastValue;
                            else
                            {
                                this.a_addingNew = false;
                                this.a_regionID = int.Parse(regionsTable.Rows[i].Cells[0].Value.ToString());
                                this.a_A_NAME = e.ColumnIndex == 1 ? MySqlHelper.EscapeString(regionsTable.Rows[i].Cells[1].Value.ToString()) : null;
                                this.a_H_NAME = e.ColumnIndex == 2 ? MySqlHelper.EscapeString(regionsTable.Rows[i].Cells[2].Value.ToString()) : null;

                                regionsTable.ReadOnly = true;
                                LDFRM = new LoadingForm();
                                LDFRM.label1.Text = "עדכון שם איזור";
                                LDFRM.Show();
                                this.worker_addRegion.RunWorkerAsync();
                            }
                        }
                    }
                }
            }
        }

        private void worker_addRegion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.a_addingNew)
            {
                foreach (DataGridViewRow row in regionsTable.Rows)
                {
                    if (int.Parse(row.Cells[0].Value.ToString()) == -2)
                    {
                        row.Cells[0].Value = this.a_regionID;
                        row.DefaultCellStyle.BackColor = Color.White;
                        break;
                    }
                }
            }
            this.regionsTable.ReadOnly = false;
            this.LDFRM.Close();
        }

        private void deliveryRegions_Load(object sender, EventArgs e)
        {
            regionsTable.DoubleBuffered(true);
        }

        private void worker_addRegion_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.a_addingNew)
            {
                string query = string.Format("INSERT INTO regions (A_NAME, H_NAME, DELIVERY_PRICE) VALUES ('{0}', '{1}', {2})", this.a_A_NAME, this.a_H_NAME, this.a_DeliveryPrice);
                this.a_regionID = new API_CLASS().API_QUERY_EXECUTE(query);
            }
            else
            {
                string query = string.Format("UPDATE regions SET ");
                if (this.a_A_NAME != null) query += string.Format("A_NAME='{0}'", this.a_A_NAME);
                else if (this.a_H_NAME != null) query += string.Format("H_NAME='{0}'", this.a_H_NAME);
                else query += string.Format("DELIVERY_PRICE={0}", this.a_DeliveryPrice);
                query += " WHERE ID=" + this.a_regionID;
                new API_CLASS().API_QUERY_EXECUTE(query);
            }
        }
    }
}
