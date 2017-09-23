using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalClinic.Models
{
    public class DentalClinicContext
    {
        public string ConnectionString { get; set; }

        public DentalClinicContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<PatientProfile> SearchPatientProfile(string name)
        {
            var profiles = new List<PatientProfile>();
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                var query = string.Format("SELECT * FROM patientprofile where name like '%{0}%' or nameen like '%{0}%'", name);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profiles.Add(new PatientProfile()
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            NameEn = reader.GetString("NameEn"),
                            Gender = reader.GetInt32("Gender"),
                            Phone = reader.GetString("Phone"),
                            Address = reader.GetString("Address"),
                            Email = reader.GetString("Email"),
                            CreatedOn = reader.GetDateTime("CreatedOn"),
                            UpdatedOn = reader.GetDateTime("UpdatedOn")

                        });
                    }
                }
            }
            return profiles;
        }
    }
}
