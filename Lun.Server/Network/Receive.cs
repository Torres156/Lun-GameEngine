using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Server.Models.Player;
using Lun.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Network
{
    class Receive
    {
        enum Packet
        {
            Register,
            Login,
        }

        public static void Handle(NetPeer peer, NetDataReader buffer)
        {
            var packetID = (Packet) buffer.GetInt();

            switch(packetID)
            {
                case Packet.Register: Register(peer, buffer); break;
                case Packet.Login: Login(peer, buffer); break;
            }
        }

        static void Login(NetPeer peer, NetDataReader buffer)
        {
            var user = buffer.GetString();
            var pwd = buffer.GetString();

            // Verify the username is used
            if (!DatabaseManager.Accounts.Any(i => i.Name == user))
            {
                Sender.Alert(peer, $"{user} is not exist!");
                return;
            }

            // Check if any account is logged in
            if (PlayerService.Accounts.Any(i => i.Name.Equals(user, StringComparison.OrdinalIgnoreCase)))
            {
                var find = PlayerService.Accounts.Find(i => i.Name.Equals(user, StringComparison.OrdinalIgnoreCase));
                find.Peer.Disconnect(); // Disconnect the found account
                Sender.Alert(peer, $"{user} is already logged in!");
                return;
            }

            var accountData = DatabaseManager.Accounts.First(i => i.Name.Equals(user, StringComparison.OrdinalIgnoreCase));
            
            // Check if passwords match
            if (pwd != accountData.Password)
            {
                Sender.Alert(peer, "Incorrect password!");
                return;
            }

            accountData.Peer = peer;
            PlayerService.Accounts.Add(accountData);
            Sender.Logged(accountData);
        }

        static void Register(NetPeer peer, NetDataReader buffer)
        {
            var user = buffer.GetString();
            var pwd = buffer.GetString();

            // Verify the username is used
            if (DatabaseManager.Accounts.Any(i => i.Name.Equals(user, StringComparison.OrdinalIgnoreCase)))
            {
                Sender.Alert(peer, $"{user} is already in use!");
                return;
            }

            var account = new Account();
            account.Name = user;
            account.Password = pwd;
            DatabaseManager.Accounts.Add(account);
            DatabaseManager.SaveChanges();

            Sender.Alert(peer, $"The {user} account has been created!");
        }
    }
}
