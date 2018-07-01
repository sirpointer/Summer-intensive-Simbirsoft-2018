using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace SocialMediaInformationAggregator.DatabaseInteraction
{
    public static class PeopleFromDb
    {
        public static string connectionString;
        public static void ConnectToDatabase()
        {
            string dataDirectory = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory); //Переопределяем |DataDirectory|, директория, откуда загружается база данных
            connectionString = "Data Source=LAPTOP-8FE5V0OM\\SQLEXPRESS;Initial Catalog=SMIA;Integrated Security=True";
        }

        private static List<FindPeople.PersonInformation> GetFoindPeopleFromNetworks(string login, string TableName)
        {
            ConnectToDatabase();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = String.Format("SELECT * FROM " + TableName+ " WHERE UserLogin=@a");
            List<FindPeople.PersonInformation> list = new List<FindPeople.PersonInformation>();
            using (SqlCommand comm = new SqlCommand(query, conn))
            {
                comm.Parameters.AddWithValue("@a", login);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FindPeople.PersonInformation hum = new FindPeople.PersonInformation();
                        hum.Name = reader.GetValue(1).ToString();
                        hum.LastName = reader.GetValue(2).ToString();
                        hum.YearOfBirth = (int)reader.GetValue(3);

                        List<string> lst = new List<string>();
                        lst.Add(reader.GetValue(4).ToString());
                        hum.Cities = SeparateElements(lst);
                        lst.Clear();
                        lst.Add(reader.GetValue(5).ToString());
                        hum.Education = SeparateElements(lst);

                        list.Add(hum);
                    }
                }
            }
            return list;
        }

        public static List<FindPeople.PersonInformation> GetFoundPeople(string login)
        {
            List<FindPeople.PersonInformation> PersonsFromVK = GetFoindPeopleFromNetworks(login, "FoundPersonsVK");
            List<FindPeople.PersonInformation> PersonsFromOK = GetFoindPeopleFromNetworks(login, "FoundPersonOK");
            List<FindPeople.PersonInformation> CommonPersons = PersonsFromVK.Concat(PersonsFromOK).ToList();
            return CommonPersons;
        }

        private static List<string> GetRowInformFromDb(string login, int k)
        {
            List<string> informFromVK = GetRowInformFromDbNetwork(login, "FoundPersonsVK", k);
            List<string> informFromOK = GetRowInformFromDbNetwork(login, "FoundPersonsOK", k);
            List<string> CommonInform = informFromVK.Concat(informFromOK).Distinct().ToList();
            return CommonInform;
        }

        private static List<string> GetRowInformFromDbNetwork(string login, string TableName, int k)
        {
            /// возвращает список значений определенного столбца таблицы для заданного пользователя
            ConnectToDatabase();
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string query = String.Format("SELECT * FROM "+TableName+" WHERE UserLogin=@a");
            List<string> list = new List<string>();
            using (SqlCommand comm = new SqlCommand(query, conn))
            {
                comm.Parameters.AddWithValue("@a", login);
                SqlDataReader reader = comm.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(reader.GetValue(k).ToString());
                    }
                }
                return list;
            }
        }


        public static List<string> LastNameFoundPeople(string login, int count = -1, string startWith = "")
        {
            List<string> lastNamesList = new List<string>();
            if (!String.IsNullOrWhiteSpace(login))
            {
                if (count != -1 && startWith != "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 2).Where(a => a.StartsWith(startWith)).OrderBy(a => a).Take(count);
                    lastNamesList = buf.ToList();
                }
                else if (count != -1 && startWith == "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 2).OrderBy(a => a).Take(count);
                    lastNamesList = buf.ToList();
                }
                else if (count == -1 && startWith != "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 2).Where(a => a.StartsWith(startWith)).OrderBy(a => a);
                    lastNamesList = buf.ToList();
                }
                else if (count == -1 && startWith == "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 2).OrderBy(a => a);
                    lastNamesList = buf.ToList();
                }
                return lastNamesList;
            }
            else
            {
                throw new Exception("Введено некорректное значение!");
            }
        }

        public static List<string> FirstNameFoundPeople(string login, int count = -1, string startWith = "")
        {
            List<string> firstNamesList = new List<string>();
            if (!String.IsNullOrWhiteSpace(login))
            {
                if (count != -1 && startWith != "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 1).Where(a => a.StartsWith(startWith)).OrderBy(a => a).Take(count);
                    firstNamesList = buf.ToList();
                }
                else if (count == -1 && startWith != "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 1).Where(a => a.StartsWith(startWith)).OrderBy(a => a);
                    firstNamesList = buf.ToList();
                }
                else if (count == -1 && startWith == "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 1).OrderBy(a => a);
                    firstNamesList = buf.ToList();
                }
                else if (count != -1 && startWith == "")
                {
                    IEnumerable<string> buf = GetRowInformFromDb(login, 1).OrderBy(a => a).Take(count);
                    firstNamesList = buf.ToList();
                }
                return firstNamesList;
            }
            else
            {
                throw new Exception("Введено некорректное значение!");
            }

        }

        public static List<string> SeparateElements(List<string> mas)
        {
            List<string> elements = new List<string>();
            foreach (string a in mas)
            {
                string[] trySort = a.Split(',');
                foreach (string p in trySort)
                {
                    elements.Add(p.TrimStart(' '));
                }
            }
            elements = elements.Distinct().ToList();
            return elements;
        }
        public static List<string> CityFoundPeople(string login, int count = -1, string startWith = "")
        {
            List<string> cities = new List<string>();
            if (!String.IsNullOrWhiteSpace(login) && (count > 0 || count == -1))
            {
                if (count > 0 && startWith != "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 4);
                    cities = SeparateElements(AllElements).Where(a => a.StartsWith(startWith)).OrderBy(a => a).Take(count).ToList();
                }
                else if (count == -1 && startWith != "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 4);
                    cities = SeparateElements(AllElements).ToList().Where(a=>a.StartsWith(startWith)).OrderBy(a => a).ToList();
                }
                else if (count == -1 && startWith == "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 4);
                    cities = SeparateElements(AllElements).OrderBy(a => a).ToList();
                }
                else if (count > -1 && startWith == "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 4);
                    cities = SeparateElements(AllElements).OrderBy(a => a).Take(count).ToList();
                }
                return cities;
            }
            else
            {
                throw new Exception("Введено некорректное значение или не указан логин!");
            }
        }

        public static List<string> EducationFoundPeople(string login, int count = -1, string startWith = "")
        {
            List<string> education = new List<string>();

            if (!String.IsNullOrWhiteSpace(login) && (count > 0 || count == -1))
            {
                if (count > 0 && startWith != "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 5);
                    education = SeparateElements(AllElements).Select(a=>a).Where(a => a.StartsWith(startWith)).OrderBy(a => a).Take(count).ToList();
                }
                else if (count == -1 && startWith != "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 5);
                    IEnumerable<string> res = SeparateElements(AllElements).Select(a => a).Where(a => a.StartsWith(startWith)).OrderBy(a => a);
                    education = res.ToList();
                }
                else if (count == -1 && startWith == "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 5);
                    IEnumerable<string> res = SeparateElements(AllElements).OrderBy(a => a);
                    education = res.ToList();
                }
                else if (count > 1 && startWith == "")
                {
                    List<string> AllElements = GetRowInformFromDb(login, 5);
                    education = SeparateElements(AllElements).OrderBy(a => a).Take(count).ToList();
                }
                return education;
            }
            else
            {
                throw new Exception("Введено некорректное значение или не указан логин!");
            }
        }

        public static void AddFoundPerson(FindPeople.PersonInformation persVK = null, FindPeople.PersonInformation persOK = null)
        ///Vk = true, значит, ссылка из ВК, в противном случае - из ОК
        {
            if (persVK != null || persOK != null)
            {
                if (persVK != null)
                {
                    InsertIntoNetwork(persVK, "FoundPersonsVK");
                }
                if (persOK != null)
                {
                    InsertIntoNetwork(persOK, "FoundPersonsOK");
                }
            }
        }

        private static void InsertIntoNetwork(FindPeople.PersonInformation pers, string tableName)
        {
            if (!String.IsNullOrEmpty(pers.Name)
                && !String.IsNullOrEmpty(pers.LastName)
                && !String.IsNullOrEmpty(pers.YearOfBirth.ToString()))
            {
                int buf = -1;
                List<FindPeople.PersonInformation> alreadyExist = GetFoundPeople(App.CurrentUserLogin);
                foreach (var k in alreadyExist)
                {
                    if (k == pers)
                    {
                        buf = 100;
                    }
                }
                if (buf == -1)
                {
                    ConnectToDatabase();
                    SqlConnection conn = new SqlConnection(connectionString);
                    conn.Open();
                    string query = String.Format("INSERT INTO " + tableName +
                        " (UserLogin, FirstName, LastName, BirthData, Cities, " +
                        "Education, LinkVK) " +
                        "VALUES (@login, @name, @lastName, @birthData, @cities," +
                        "@education, @linkVK)");
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        string cities = "";
                        string education = "";
                        comm.Parameters.AddWithValue("@login", App.CurrentUserLogin);
                        comm.Parameters.AddWithValue("@name", pers.Name);
                        comm.Parameters.AddWithValue("@lastName", pers.LastName);
                        comm.Parameters.AddWithValue("@birthData", pers.YearOfBirth);
                        cities = MakeCityRow(pers);
                        education = MakeEducationRow(pers);
                        comm.Parameters.AddWithValue("@cities", cities);
                        comm.Parameters.AddWithValue("@education", education);
                        comm.Parameters.AddWithValue("@linkVK", pers.SocialNetwork);
                        comm.ExecuteNonQuery();
                    }
                }
                else
                {
                    throw new Exception("Данная запистьв базе уже существует!");
                }
            }
            else
            {
                throw new Exception("Обязательные поля не заполнены!");
            }
        }
        private static string MakeEducationRow(FindPeople.PersonInformation pers)
        {
            string education = "";
            foreach (string a in pers.Education)
            {
                education += a + ",";
            }
            if (!String.IsNullOrEmpty(education))
            {
                education.Remove(education.Length - 1, 1);
            }
            return education;
        }

        private static string MakeCityRow(FindPeople.PersonInformation pers)
        {
            string cities = "";
            foreach (string a in pers.Education)
            {
                cities += a + ",";
            }
            if (!String.IsNullOrEmpty(cities))
            {
                cities.Remove(cities.Length - 1, 1);
            }
            return cities;
        }
    }
}
