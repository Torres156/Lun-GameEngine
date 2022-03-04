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
            CreateCharacter,
        }

        public static void Handle(NetPeer peer, NetDataReader buffer)
        {
            var packetID = (Packet) buffer.GetInt();

            switch(packetID)
            {
                case Packet.Register: Register(peer, buffer); break;
                case Packet.Login: Login(peer, buffer); break;
                case Packet.CreateCharacter: CreateCharacter(peer, buffer); break;
            }
        }

        static void CreateCharacter(NetPeer peer, NetDataReader buffer)
        {
            var slotID = buffer.GetInt();
            var name = buffer.GetString();
            var classID = buffer.GetInt();
            var spriteID = buffer.GetInt();


            // Check if name is using
            if (CheckDataExists(TABLE_CHARACTERS, "name", name))
            {
                Sender.Alert(peer, $"{name} is already in use!");
                return;
            }
            
            var account = PlayerService.Accounts.FirstOrDefault(i => i.Peer == peer);
            if (account == null)
            {
                // DEBUG
                return;
            }

            ExecuteNonQuery($@"INSERT INTO {TABLE_CHARACTERS} 
(accountid, name, classid, spriteid) VALUES
({account.ID},'{name}', {classID}, {spriteID});");
            
            Sender.Logged(account);
            Sender.Alert(peer, $"The character {name} has been created!");
        }

        static void Login(NetPeer peer, NetDataReader buffer)
        {
            var user = buffer.GetString();
            var pwd = buffer.GetString();

            // Verify the username is used
            if (!CheckDataExists(TABLE_ACCOUNTS, "name", user))
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

            var reader = ExecuteReader($"SELECT * FROM {TABLE_ACCOUNTS} WHERE name='{user}';");
            reader.Read();
            var accountID = reader.GetInt32(0);
            var accountName = reader.GetString(1);
            var accountPassword = reader.GetString(2);
            reader.Close();
            
            // Check if passwords match
            if (pwd != accountPassword)
            {
                Sender.Alert(peer, "Incorrect password!");
                return;
            }

            var account = new Account();
            account.ID = accountID;
            account.Peer = peer;
            account.Name = accountName;
            account.Password = accountPassword;
            PlayerService.Accounts.Add(account);

            Sender.AllClassData(peer);
            Sender.Logged(account);
        }

        static void Register(NetPeer peer, NetDataReader buffer)
        {
            var user = buffer.GetString();
            var pwd = buffer.GetString();

            // Verify the username is used
            if (CheckDataExists(TABLE_ACCOUNTS,"name", user))
            {
                Sender.Alert(peer, $"{user} is already in use!");
                return;
            }

            var account = new Account();
            account.Name = user;
            account.Password = pwd;

            ExecuteNonQuery(@$"INSERT INTO {TABLE_ACCOUNTS} 
(name, password) VALUES
('{user}', '{pwd}');");

            Sender.Alert(peer, $"The {user} account has been created!");
        }
    }
}
