using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System.Net;
using UnityEngine;

public class DroneNetworkManager : MonoBehaviour
{
    [SerializeField]
    string ServerHost = "localhost";

    [SerializeField]
    int serverPort = 9000;

    TcpListener _server;

    List<TcpClient> _clients = new List<TcpClient>();

    // Start is called before the first frame update
    void Start()
    {
        _server = new TcpListener(IPAddress.Parse("0.0.0.0"), serverPort);
        _server.Start();
        Task.Factory.StartNew(() => AcceptListener());
    }

    async Task AcceptListener()
    {
        while (true)
        {
            TcpClient client = await _server.AcceptTcpClientAsync();

            _clients.Add(client);

            Debug.Log("Client Connected");
            Debug.Log("Server Listening on: " + serverPort);
            Debug.Log("Server Listening on: " + serverPort);

            using (NetworkStream stream = client.GetStream())
            {
                // Client now communicating through stream
                Receive(stream);
            }
        }
    }

    public async Task Send(IPacket packet)
    {
        string json = JsonUtility.ToJson(packet);

        byte[] buffer = Encoding.UTF8.GetBytes(json);

        foreach  (TcpClient client in _clients)
        {
            await client.GetStream().WriteAsync(buffer, 0, buffer.Length);
        }
    }

    async Task Receive(NetworkStream stream)
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            await stream.ReadAsync(buffer, 0, buffer.Length, CancellationToken.None);

            string receivedData = Encoding.UTF8.GetString(buffer);
            dynamic packet = JsonUtility.FromJson<dynamic>(receivedData);

            if (packet.Type == null)
            {
                continue;
            }

            // Maybe for later use
            //Task.Factory.StartNew(() => Process(IPPacketInformation));
        }
    }

}
