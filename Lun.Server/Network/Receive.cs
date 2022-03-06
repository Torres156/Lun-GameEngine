using LiteNetLib;
using LiteNetLib.Utils;
using Lun.Server.Models.Player;
using Lun.Server.Services;
using Lun.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Server.Network
{
    class Receive
    {
        public static void Handle(NetPeer peer, NetDataReader buffer)
        {
            var packetID = (PacketClient) buffer.GetInt();

            switch(packetID)
            {
                case PacketClient.Register       : Register(peer, buffer); break;
                case PacketClient.Login          : Login(peer, buffer); break;
                case PacketClient.CreateCharacter: CreateCharacter(peer, buffer); break;
                case PacketClient.UseCharacter   : UseCharacter(peer, buffer); break;
                case PacketClient.MapAnswer      : MapAnswer(peer, buffer); break;
                case PacketClient.PlayerMovement : PlayerMovement(peer, buffer); break;
            }
        }

        static void PlayerMovement(NetPeer peer, NetDataReader buffer)
        {
            var player = PlayerService.Characters.Find(i => i.Peer == peer);

            player.Direction = (Directions)buffer.GetInt();
            player.Position  = buffer.GetVector2();

            Sender.PlayerMovement(player);
        }

        static void MapAnswer(NetPeer peer, NetDataReader buffer)
        {
            var value = buffer.GetBool();

            if (value)
            { } // SEND MAP DATA
            else
            {
                PlayerService.SendWarpInfo(PlayerService.Characters.Find(i => i.Peer == peer));
            }
        }

        static void UseCharacter(NetPeer peer, NetDataReader buffer)
        {
            var slotid = buffer.GetInt();

            var account   = PlayerService.Accounts.Find(i => i.Peer == peer);
            var character = new Character();

            var reader = ExecuteReader($"SELECT * FROM {TABLE_CHARACTERS} WHERE accountid={account.ID} AND slotid={slotid}");
            if (reader.Read())
            {
                character.AccountID       = account.ID;
                character.CharacterSlotID = slotid;
                character.ID              = reader.GetInt32("id");
                character.Name            = reader.GetString("name");
                character.ClassID         = reader.GetInt32("classid");
                character.SpriteID        = reader.GetInt32("spriteid");
                character.Position        = new Vector2( reader.GetFloat("x"), reader.GetFloat("y"));
            }
            reader.Close();

            character.Peer = peer;
            PlayerService.Characters.Add(character);
            PlayerService.JoinGame(character);
        }

        static void CreateCharacter(NetPeer peer, NetDataReader buffer)
        {
            var slotID   = buffer.GetInt();
            var name     = buffer.GetString();
            var classID  = buffer.GetInt();
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

            ExecuteNonQuery($@"INSERT INTO { TABLE_CHARACTERS } 
            ( accountid, slotid, name, classid, spriteid, x, y ) VALUES
            ({ account.ID }, { slotID }, '{name}', {classID}, { spriteID }, 320, 320); ");

            Sender.AllCharacterAccount(account);
            Sender.Logged(account);
            Sender.Alert(peer, $"The character {name} has been created!");
        }

        static void Login(NetPeer peer, NetDataReader buffer)
        {
            var user = buffer.GetString();
            var pwd  = buffer.GetString();

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
            var accountID       = reader.GetInt32("id");
            var accountName     = reader.GetString("name");
            var accountPassword = reader.GetString("password");
            reader.Close();
            
            // Check if passwords match
            if (pwd != accountPassword)
            {
                Sender.Alert(peer, "Incorrect password!");
                return;
            }

            // Load account data
            var account = new Account();
            account.ID       = accountID;
            account.Peer     = peer;
            account.Name     = accountName;
            account.Password = accountPassword;
            PlayerService.Accounts.Add(account);

            Sender.AllClassData(peer);
            Sender.AllCharacterAccount(account);
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
            account.Name     = user;
            account.Password = pwd;

            ExecuteNonQuery(@$"INSERT INTO { TABLE_ACCOUNTS } 
            (name, password) VALUES
            ('{ user }', '{ pwd }'); ");

            Sender.Alert(peer, $"The {user} account has been created!");
        }
    }
}
