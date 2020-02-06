using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Crisis.Messages;
using Crisis.Database;
using Crisis.Database.Model;

namespace Crisis.Network
{
    /// <summary>
    /// Most interaction should happen through Clients. Use this class sparingly.
    /// </summary>
    static class Server
    {
        private static readonly Telepathy.Server server = new Telepathy.Server();

        private static readonly ConcurrentDictionary<int, Client> clients = new ConcurrentDictionary<int, Client>();
        public static IReadOnlyDictionary<int, Client> Clients => clients;

        private static IDatabaseHandler _db = new LocalJsonDatabase();

        public static void Start()
        {
            server.Start(Configuration.Port);
            Task.Run(DequeueLoop);
        }

        public static void Send(Client destination, Message msg)
        {
            server.Send(destination.ID, msg.Serialize());
        }

        public static void Disconnect(Client client)
        {
            server.Disconnect(client.ID);
            clients.TryRemove(client.ID, out Client _);
        }

        public static bool RegisterUser(UserModel model)
        {
            // Probably change to int return, for [already registered, db error, invalid model]
            if (_db.addUser(model))
                return true;
            return false;
        }

        public static void Stop() => server.Stop();

        private static void DequeueLoop()
        {
            while (server.Active)
            {
                try
                {
                    while (server.GetNextMessage(out Telepathy.Message msg))
                    {
                        switch (msg.eventType)
                        {
                            case Telepathy.EventType.Connected:
                                clients.TryAdd(msg.connectionId, new Client(msg.connectionId));
                                break;
                            case Telepathy.EventType.Data:
                                if (Message.TryInfer(msg.data, out Message inferred))
                                {
                                    clients[msg.connectionId].Receive(inferred);
                                }
                                break;
                            case Telepathy.EventType.Disconnected:
                                clients[msg.connectionId].Disconnect();
                                break;
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
        }

    }
}
