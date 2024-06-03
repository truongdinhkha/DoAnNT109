using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Threading;
using System.Security.Cryptography;

namespace Server_review
{
    class Server 
    {
        private TcpListener serverListener;
        private const int Port_Number = 1234;
       string ConnectionString = @"Data Source=DINH-KHA\SQLEXPRESS;Initial Catalog=doan;Integrated Security=True";
        private List<TcpClient> ListClient = new List<TcpClient>();

        public void StartListening()
        {
            try
            {
                serverListener = new TcpListener(IPAddress.Any, Port_Number);
                serverListener.Start();

                Console.WriteLine("Server started. Listening for incoming connections...");

                while (true)
                {
                    TcpClient client = serverListener.AcceptTcpClient();
                    ListClient.Add(client);

                    // Sử dụng luồng mới để xử lý kết nối từ người dùng
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string requestData = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                //string[] requestParts = requestData.Split(',');
                string[] requestParts = requestData.Split('/');
                string requestType = requestParts[0];
                string userData = requestParts[1];
                Console.WriteLine("Request: " + requestType);
                Console.WriteLine("Data: " + userData);

                bool isSuccess = false;
                string response = string.Empty;

                if (requestType == "REGISTER")
                {
                    isSuccess = RegisterUser(requestParts[1], requestParts[2]);
                    response = isSuccess ? "REGISTER_SUCCESS" : "REGISTER_FAILURE";
                }
                else if (requestType == "LOGIN")
                {
                    isSuccess = LoginUser(requestParts[1], requestParts[2]);
                    response = isSuccess ? "LOGIN_SUCCESS" : "LOGIN_FAILURE";
                }
                else if (requestType == "Chat_user")
                {

                    
                
                   

                }
                else if (requestType == "New_Post")
                {
                 
                    isSuccess = new_post(requestParts[1], requestParts[2], requestParts[3], Int32.Parse(requestParts[4]), requestParts[5]);

                    response = isSuccess ? "POST_SUCCESS" : "POST_FAILURE";
                   
                }
                byte[] responseBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling client request: " + ex.Message);
            }
        }
      
        private void handle(TcpClient client, string endPoint)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true && client.Connected)
            {
                int byteReads = 0;

                try
                {
                    byteReads = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {

                    break;
                }

                if (byteReads == 0)
                {

                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, byteReads);

            }
        }
       
      
        private bool new_post(string Name_Company,string title,string content,int Star,string ID_Nguoi_Dung)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    ID_Nguoi_Dung = "kha";
                    DateTime t = DateTime.Now;
                    string sqlstring = $"Insert into Post(Name_Company,Title,Content,Star,Date,Name_Nguoi_Viet) values('{Name_Company}',' {title} ','{content}', {Star} ,'{t}', '{ID_Nguoi_Dung}' )";
                    SqlCommand command = new SqlCommand(sqlstring, connection);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"Insert successfully.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Insert unsuccessfully.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }

        }
      

        private bool RegisterUser(string username, string password)
        {
            try
            {
                // Thực hiện kết nối và thao tác với cơ sở dữ liệu SQL Server
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
                    string checkQuery = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}'";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    int existingUserCount = (int)checkCommand.ExecuteScalar();

                    if (existingUserCount > 0)
                    {
                        Console.WriteLine($"User {username} already exists.");
                        return false;
                    }

                    // Thêm người dùng vào bảng Users
                    string insertQuery = $"INSERT INTO Users (Username, Password) VALUES ('{username}', '{ComputeMD5Hash(password)}')";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"User {username} registered successfully.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to register user {username}.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error registering user: " + ex.Message);
                return false;
            }
        }

        private bool LoginUser(string username, string password)
        {
            try
            {

                // Thực hiện kết nối và thao tác với cơ sở dữ liệu SQL Server
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu không
                    string checkQuery = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}' AND Password = '{ComputeMD5Hash(password)}'";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    int userCount = (int)checkCommand.ExecuteScalar();

                    if (userCount > 0)
                    {
                        Console.WriteLine($"User {username} logged in successfully.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to log in user {username}.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error logging in user: " + ex.Message);
                return false;
            }
        }
        //hàm băm chuỗi sử dụng thuật toán MD5 hash
        public static string ComputeMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                // Chuyển đổi chuỗi vào thành mảng byte
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // Băm mảng byte
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Chuyển đổi mảng byte thành chuỗi hexa
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        /*
        //chuyen nhan tin nhan
        private List<Industry> GetIndustries()
        {
            List<Industry> industries = new List<Industry>();

            try
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.Open();

                    string query = "SELECT * FROM Industries";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int industryID = reader.GetInt32(0);
                        string industryName = reader.GetString(1);
                        Console.WriteLine("Data: " + industryID);

                        Industry industry = new Industry(industryID, industryName);
                        industries.Add(industry);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving industries: " + ex.Message);
            }

            return industries;
        }

        /*
        private List<Company> GetCompaniesByIndustry(int industryID)
        {
            List<Company> companies = new List<Company>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = $"SELECT [CompanyID], [CompanyName], [IndustryID], [Address], [ReviewCount], [AverageRating] FROM Companies WHERE [IndustryID] = @IndustryID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IndustryID", industryID);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int companyID = reader.GetInt32(0);
                        string companyName = reader.GetString(1);
                        int industryID1 = reader.GetInt32(2);
                        string address = reader.GetString(3);
                        int reviewCount = reader.GetInt32(4);
                        double averageRating = reader.GetDouble(5);

                        Company company = new Company(companyID, companyName, industryID, address, reviewCount, averageRating);
                        companies.Add(company);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving companies: " + ex.Message);
            }

            return companies;
        }
        */
       
    }

 

    public class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartListening();
            
        }
    }
}
