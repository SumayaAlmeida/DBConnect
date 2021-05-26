using DBConnect;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BowmanCarHire
{
    public partial class CarHire : Form
    {
        List<Car> list = null;

        DBConnection db = null;

        DBCars dbCars = null;

        MySqlDataReader reader = null;

        int count = 1;

        bool clickToClear = true;


        // METHODS///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        internal void DisplayCar(Car car)
        {
            textBoxRegNo.Text = car.vehicleRegNo;
            textBoxMake.Text = car.make;
            textBoxEngineSize.Text = car.engineSize;
            textBoxDateRegistered.Text = car.dateRegistered.ToString();
            textBoxRentalCost.Text = car.rentalCost.ToString();
            if (car.available == 1)
            {
                checkBoxAvailable.Checked = true;
            }
            else
            {
                checkBoxAvailable.Checked = false;
            }
            textBoxCount.Text = (count).ToString() + " of " + list.Count();

        }

        internal void UpdateList(Car car)
        {
            
            car.vehicleRegNo = textBoxRegNo.Text;
            car.make = textBoxMake.Text;
            car.engineSize = textBoxEngineSize.Text;
            car.dateRegistered = DateTime.Parse(textBoxDateRegistered.Text);
            car.rentalCost = Double.Parse(textBoxRentalCost.Text);
            car.available = Available(checkBoxAvailable);

        }

        internal int Available(CheckBox check)
        {
            int available;
            if (check.Checked)
            {
                available = 1;
            }
            else
            {
                available = 0;
            }

                return available;
        }

        public void DisableButtons()
        {
            buttonDelete.Enabled = false;
            buttonUpdate.Enabled = false;
            buttonFirst.Enabled = false;
            buttonNext.Enabled = false;
            buttonPrevious.Enabled = false;
            buttonLast.Enabled = false;
            buttonPrint.Enabled = false;
        }

        public void EnableButtons()
        {
            buttonDelete.Enabled = true;
            buttonUpdate.Enabled = true;
            buttonFirst.Enabled = true;
            buttonNext.Enabled = true;
            buttonPrevious.Enabled = true;
            buttonLast.Enabled = true;
            buttonPrint.Enabled = true;
        }

        public void MakeDbConnection()
        {
            if (db.Connect())
            {
                MessageBox.Show("Connected to MySql Server");
            }

        }

        public void LoadCars()
        {
            dbCars = new DBCars();

            reader = dbCars.GetCars(this.db.Connection);

            while (reader.Read())
            {
                Car car = new Car();

                car.vehicleRegNo = reader.GetString(0);
                car.make = reader.GetString(1);
                car.engineSize = reader.GetString(2);
                car.dateRegistered = reader.GetDateTime(3);
                car.rentalCost = reader.GetDouble(4);
                car.available = reader.GetInt32(5);

                list.Add(car);
            }
        }

        //VALIDATION METHODS//////////////////////////////////////////////////////////////////////////////////////

        private bool ValidateRegNo(string vehicleRegNo)
        {
            if (vehicleRegNo.Length > 0 && vehicleRegNo.Length <= 10)
            {
                return true;
            }

            MessageBox.Show("Vehicle Registration No. must be greater than 0 characters, but no more than 10", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private bool ValidateMake(string make)
        {
            if (make.Length > 0 && make.Length <= 50)
            {
                return true;
            }

            MessageBox.Show("Make must be greater than 0 characters, but no more than 50", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private bool ValidateEngineSize(string engineSize)
        {
            if (engineSize.Length > 0 && engineSize.Length <= 10)
            {
                return true;
            }

            MessageBox.Show("Engine size must be greater than 0 characters, but no more than 10", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private Car CreateCarFromForm()
        {
            string vehicleRegNo = this.textBoxRegNo.Text;
            if (ValidateRegNo(vehicleRegNo) == false)
            {
                return null;
            }

            string make = this.textBoxMake.Text;
            if (ValidateMake(make) == false)
            {
                return null;
            }

            string engineSize = this.textBoxEngineSize.Text;
            if (ValidateEngineSize(engineSize) == false)
            {
                return null;
            }

            DateTime dateRegistered;

            if (DateTime.TryParse(this.textBoxDateRegistered.Text, out dateRegistered) == false)
            {
                MessageBox.Show("Please enter a date in the format YYYY-MM-DD", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            string rentalCostSubstring = "";
            if (this.textBoxRentalCost.Text.Length > 0)
            {
                rentalCostSubstring = this.textBoxRentalCost.Text.Substring(1);
            }

            double rentalPerDay;

            if (double.TryParse(rentalCostSubstring, out rentalPerDay) == false)
            {
                MessageBox.Show("Please enter a valid rental cost.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            int available = 0;
            if (this.checkBoxAvailable.Enabled)
            {
                available = 1;
            }

            Car car = new Car(vehicleRegNo, make, engineSize, dateRegistered, rentalPerDay, available);

            return car;
        }


        //EVENT HANDLERS///////////////////////////////////////////////////////////////////////////////////////////
        public CarHire()
        {
            InitializeComponent();
        }

        private void CarHire_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.db != null)
            {
                this.db.Close();
            }
        }

        private void CarHire_Load(object sender, EventArgs e)
        {
            
            this.list = new List<Car>();
            this.db = new DBConnection("localhost", "hire", "csharp", "password");

            ToolTip toolTip = new ToolTip();

            toolTip.SetToolTip(this.textBoxRegNo, "Enter the registration number of the vehicle.");
            toolTip.SetToolTip(this.textBoxMake, "Enter the make of the vehicle.");
            toolTip.SetToolTip(this.textBoxEngineSize, "Enter the size of the engine");

            MakeDbConnection();

            LoadCars();
            DisplayCar(list[count-1]);
            reader.Close();
                
        }//LOAD

        private void buttonFirst_Click(object sender, EventArgs e)
        {
            count = 1;
            DisplayCar(list[count-1]);
        }

        private void buttonLast_Click(object sender, EventArgs e)
        {
            count = list.Count();
            DisplayCar(list[count-1]);

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            count++;

            if (count <= list.Count())
            {
                DisplayCar(list[count - 1]);
            }
            else
            {
                count = list.Count();
                DisplayCar(list[count - 1]);
            }

        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            count--;

            if (count >= 1)
            {
                DisplayCar(list[count - 1]);
            }
            else
            {
                count = 1;
                DisplayCar(list[count - 1]);
            }


        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            
            System.Windows.Forms.Application.ExitThread();

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            UpdateList(list[count - 1]);
            DBCars dbCars = new DBCars();
            dbCars.Update(this.db.Connection, list[count - 1].vehicleRegNo, list[count - 1].make, list[count - 1].engineSize, list[count - 1].dateRegistered,
                list[count - 1].rentalCost, list[count - 1].available);

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DBCars dbCars = new DBCars();

            dbCars.Delete(this.db.Connection, list[count - 1].vehicleRegNo);

            list.RemoveAt(count - 1);
            DisplayCar(list[count - 2]);

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (clickToClear)
            {
                textBoxRegNo.Text = "";
                textBoxMake.Text = "";
                textBoxEngineSize.Text = "";
                textBoxDateRegistered.Text = "";
                textBoxRentalCost.Text = "";
                checkBoxAvailable.Checked = false;
                buttonAdd.Text = "&Save";
                clickToClear = false;

                DisableButtons(); 

            }
            else
            {
                //Car car = new Car(textBoxRegNo.Text, textBoxMake.Text, textBoxEngineSize.Text, DateTime.Parse(textBoxDateRegistered.Text),
                //Double.Parse(textBoxRentalCost.Text), Available(checkBoxAvailable));

                Car car = CreateCarFromForm();

                list.Add(car);

                DBCars dbCars = new DBCars();
                dbCars.Insert(this.db.Connection, car.vehicleRegNo, car.make, car.engineSize, car.dateRegistered,
                    car.rentalCost, car.available);

                DisplayCar(list[list.Count - 1]);
                buttonAdd.Text = "&Add";
                clickToClear = true;
                EnableButtons();
            }

           
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DisplayCar(list[count - 1]);
            buttonAdd.Text = "&Save";
            clickToClear = false;

        }

        private void buttonPrint_Click(object sender, EventArgs e)
        {

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string printList = "RegNo      Make        Size      DateRegisted   CostPerDay   Available\r\n" +
                                 "----------------------------------------------------------------------------\r\n";

            foreach (Car c in list)
            {
                printList += c.ToString();
            }
            e.Graphics.DrawString(printList, new Font("Courier New", 10, FontStyle.Regular), Brushes.Black, new PointF(100, 100));
        }

        
    }//CLASS

}//NAMESPACE

