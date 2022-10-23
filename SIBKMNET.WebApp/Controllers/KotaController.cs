using Microsoft.AspNetCore.Mvc;
using SIBKMNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SIBKMNET.WebApp.Controllers
{
    public class KotaController : Controller
    {
        SqlConnection sqlConnection;

        string connectionString = "Data Source=DESKTOP-6TUDK2G;Initial Catalog=SIBKMNET;" +
            "User ID=sibkmnet;Password=1234567890;Connect Timeout=30;";

        //Get All
        //GET
        public IActionResult Index()
        {
            string query = "SELECT * FROM Kota";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            List<Kota> Perkotaan = new List<Kota>();

            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Kota kota = new Kota();
                            kota.Id = Convert.ToInt32(sqlDataReader[0]);
                            kota.Name = sqlDataReader[1].ToString();
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                            Perkotaan.Add(kota);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(Perkotaan);
            
        }

        // GET BY ID
        // GET
        public IActionResult GetById(int Id)
        {
            string query = "SELECT * FROM Kota WHERE Id = @id";

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = Id;
            List<Kota> Perkotaan = new List<Kota>();

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add(sqlParameter);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Kota kota = new Kota();
                            kota.Id = Convert.ToInt32(sqlDataReader[0]);
                            kota.Name = sqlDataReader[1].ToString();
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                            Perkotaan.Add(kota);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return View(Perkotaan);
        }

        // CREATE
        // GET
        public IActionResult Create()
        {
            return View();
        }
        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Kota kota)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = kota.Name;


                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    sqlCommand.CommandText = "INSERT INTO Kota (Name) VALUES (@name)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");
        }

        // UPDATE
        // GET
        public IActionResult Update(Kota kota)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
                List<Kota> Perkotaan = new List<Kota>();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@id";
                sqlParameter.Value = kota.Id;

                SqlParameter sqlParameter2 = new SqlParameter();
                sqlParameter2.ParameterName = "@update";
                sqlParameter2.Value = kota.Name;




                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.Parameters.Add(sqlParameter2);


                try
                {
                    sqlCommand.CommandText = "UPDATE Kota SET Name = (@update)" + "WHERE Id = (@id)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");
        }
        // POST


        // DELETE
        // GET
        public IActionResult Delete(Kota kota)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = kota.Name;

                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    sqlCommand.CommandText = "DELETE From Kota " + "WHERE (Name) = (@name)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
