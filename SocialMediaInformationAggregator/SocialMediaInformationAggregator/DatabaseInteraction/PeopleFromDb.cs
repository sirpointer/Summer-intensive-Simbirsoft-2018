using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace SocialMediaInformationAggregator.DatabaseInteraction
{
    public static class PeopleFromDb
    {
        public static string connectionString;
        public static List<FindPeople.PersonInformation> GetFoundPeople(string login)
        {
            connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB; AttachDbFilename = C:\\Users\\user\\Desktop\\Social - media - information - aggregator - BranchToShow\\SocialMediaInformationAggregator\\SocialMediaInformationAggregator\\AppData\\SmiaDb.mdf;Integrated Security = True";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = String.Format("SELECT * FROM FoundPeople WHERE Login='@a'");
            using (SqlCommand comm = new SqlCommand(query, conn))
            {
                comm.Parameters.AddWithValue("@a", login);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        FindPeople.PersonInformation hum = new FindPeople.PersonInformation();
                        hum.Name = reader.GetValue(2).ToString();
                        hum.LastName = reader.GetValue(3).ToString();
                      //  hum.
                    }
                }
            }
                throw new NotImplementedException();
        }

        public static List<string> LastNameFoundPeople(string login, int count=-1)
        {
            if (String.IsNullOrWhiteSpace(login) || count<1)
            {
                throw new Exception("ошибка");
            }
            if (count==-1)
            {
                throw new Exception("вывести все");
            }
            throw new NotImplementedException();
        }

        public static List<string> FirstNameFoundPeople(string login, int count = -1)
        {
            throw new NotImplementedException();
        }

        public static List<string> CityFoundPeople(string login, int count = -1)
        {
            throw new NotImplementedException();
        }

        public static List<string> EducationFoundPeople(string login, int count = -1)
        {
            throw new NotImplementedException();
        }

        public static void AddFoundPerson(FindPeople.PersonInformation pers)
        {
            throw new NotImplementedException();
        }

    }
}
