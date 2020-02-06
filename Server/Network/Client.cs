using System;
using Crisis.Database.Model;
using Crisis.Messages;
using Crisis.Messages.Client;
using Crisis.Messages.Server;

namespace Crisis.Network
{
    class Client
    {
        public int ID { get; }
        /// <summary>
        /// While the client is not authed it can only receive auth and register messages, the rest will be dropped.
        /// </summary>
        public bool Authed { get; private set; }
        public bool Connected { get; private set; } = true;
        public Character Character { get; private set; }

        public Client(int id)
        {
            ID = id;
        }

        public void Receive(Message msg)
        {
            if (!Connected) return;

            if (msg is AuthMessage authmsg)
            {
                if (Authed) //Someone tried to auth twice?
                {
                    return;
                }

                Character = new Character
                {
                    Client = this,
                    Name = authmsg.Mail
                };
                Send(new AuthConfirmMessage());
                Send(new GMChangedMessage { IsGM = true });
                Send(new TimeTurnMessage { Time = DateTime.UtcNow, TurnEnd = DateTime.UtcNow.AddHours(1) });
                Authed = true;
                return;
            }
            else if (msg is RegisterMessage)
            {
                RegisterMessage regMsg = (RegisterMessage)msg;

                UserModel registeredUser = new UserModel(regMsg.Mail, regMsg.Password);
                if(Server.RegisterUser(registeredUser))
                    Send(new RegisterResponeMessage { Response = RegisterResponse.Ok }); //TODO: Get a db
                else
                    Send(new RegisterResponeMessage { Response = RegisterResponse.NameTaken });
            }

            if (!Authed) return;

            if (msg is SpeechMessage speechmsg)
            {
                Character.Speak(speechmsg.Text);
            }
        }

        public void Send(Message msg)
        {
            Server.Send(this, msg);
        }

        public void Disconnect()
        {
            if (!Connected)
            {
                return;
            }
            Connected = false;

            if (Character != null)
            {
                Character.Client = null;
                Character = null;
            }
            Server.Disconnect(this);
        }
    }
}
