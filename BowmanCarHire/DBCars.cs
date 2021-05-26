using BowmanCarHire;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBConnect
{
    class DBCars
    {

        public MySqlDataReader GetCars(MySqlConnection connection)
        {
            string sql = "select VehicleRegNo, Make, EngineSize, DateRegistered, RentalPerDay, Available from car where VehicleRegNo is not null;";

            MySqlDataReader reader = null;

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            try
            {
                reader = cmd.ExecuteReader();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception found: " + ex.Message);
            }
            return reader;
        }

        public void Update(MySqlConnection connection, string vehicleRegNo, string make, string engineSize, DateTime dateRegistered, double rentalCost, int available)
        {
            string sql = "UPDATE car SET VehicleRegNo = @vehicleRegNo, Make = @make, EngineSize = @engineSize, DateRegistered = @dateRegistered, " +
            "RentalPerDay = @rentalCost, Available = @available WHERE VehicleRegNo = @vehicleRegNo;";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@vehicleRegNo", vehicleRegNo);
            cmd.Parameters.AddWithValue("@make", make);
            cmd.Parameters.AddWithValue("@engineSize", engineSize);
            cmd.Parameters.AddWithValue("@dateRegistered", dateRegistered);
            cmd.Parameters.AddWithValue("@rentalCost", rentalCost);
            cmd.Parameters.AddWithValue("@available", available);

            try 
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void Insert(MySqlConnection connection, string vehicleRegNo, string make, string engineSize, DateTime dateRegistered, double rentalCost, int available)
        {
            /*string sql = "INSERT INTO car VALUES (VehicleRegNo = @vehicleRegNo, Make = @make, EngineSize = @engineSize, DateRegistered = @dateRegistered, " +
            "RentalPerDay = @rentalCost, Available = @available);";*/

            string sql = "INSERT INTO car VALUES (@vehicleRegNo, @make, @engineSize, @dateRegistered, @rentalCost, @available);";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@vehicleRegNo", vehicleRegNo);
            cmd.Parameters.AddWithValue("@make", make);
            cmd.Parameters.AddWithValue("@engineSize", engineSize);
            cmd.Parameters.AddWithValue("@dateRegistered", dateRegistered);
            cmd.Parameters.AddWithValue("@rentalCost", rentalCost);
            cmd.Parameters.AddWithValue("@available", available);

            try
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Delete(MySqlConnection connection, string vehicleRegNo)
        {
            string sql = "DELETE FROM car WHERE vehicleRegNo = @vehicleRegNo;";

            MySqlCommand cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@vehicleRegNo", vehicleRegNo);

            try
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }


    }
}