using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server_For_Chat
{

    public partial class Form1 : Form
    {
        TcpListener server;
        private bool running = false;
        Dictionary<TcpClient, string> clients = new Dictionary<TcpClient, string>();
        Dictionary<string, string> participants = new Dictionary<string, string>();
        public Form1()
        {
            InitializeComponent();
            richTextBox1.ReadOnly = true;
            Connect();
        }
        private void Connect()
        {
            if (!running)
            {

                server = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);

                server.Start();
                show($"[{DateTime.Now}] Start listening on port: 5000");
                running = true;


                Task.Run(() =>
                {
                    while (running)
                    {
                        try
                        {
                            TcpClient client = server.AcceptTcpClient();
                            string endPoint = client.Client.RemoteEndPoint.ToString();
                            Task.Run(() => handleClient(client, endPoint));
                            //Task.Run(() => broadcastParticipants());
                        }
                        catch
                        {
                            break;
                        }
                    }
                });
            }
            else
            {
                broadcastMessage("@Server is shutdown", null, null);
                server.Stop();
                running = false;


                show($"[{DateTime.Now}] Server is shutdown");
            }
        }
        private void broadcastMessage(string message, TcpClient client, string user)
        {
            foreach (TcpClient userClient in clients.Keys)
            {
                if (userClient != client)
                {
                    try
                    {
                        byte[] sendMessage = Encoding.UTF8.GetBytes(message);
                        userClient.GetStream().Write(sendMessage, 0, sendMessage.Length);
                    }
                    catch (Exception ex)
                    {
                        show($"[{DateTime.Now}] Error sending message to user {user[1]}: {ex.Message}");
                    }
                }
            }
        }
        private void handleClient(TcpClient client, string endPoint)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024 * 4096];
            string[] user = new string[2];

            byte[] sendMessage = Encoding.UTF8.GetBytes("Welcome to chatroom");
            client.GetStream().Write(sendMessage, 0, sendMessage.Length);

            while (running && client.Connected)
            {
                int byteReads = 0;


                try
                {
                    byteReads = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    show($"[{DateTime.Now}] Error reading from client: {endPoint}");
                    break;
                }

                if (byteReads == 0)
                {
                    show($"[{DateTime.Now}] User {user[1]} has left the room");


                    broadcastMessage($"User {user[1]} has left the room", client, user[1]);
                    break;
                }

                string message = Encoding.UTF8.GetString(buffer, 0, byteReads);

                if (message.Contains("@userName: "))
                {
                    user = message.Split(' ');

                    clients[client] = endPoint;
                    participants[user[1]] = endPoint;

                    show($"[{DateTime.Now}] {endPoint} - User: {user[1]} has connected");

                    broadcastMessage($"User: {user[1]} has connected", client, user[1]);
                }
                else if (message.Contains("@private"))
                {
                    string[] privateUser = message.Split('\\');
                    string privateEndPoint = privateUser[1];

                }
                else
                {
                    show($"[{DateTime.Now}] {endPoint} - User: {user[1]}: {message}");

                    broadcastMessage($"{endPoint} - User: {user[1]}: {message}", client, user[1]);
                }
            }
        }
        private void show(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => show(message)));
            }
            else
            {
                richTextBox1.AppendText(message + Environment.NewLine);
            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
    }
}