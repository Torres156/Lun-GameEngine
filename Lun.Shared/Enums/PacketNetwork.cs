using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Shared.Enums
{
    public enum PacketClient
    {
        Register,
        Login,
        CreateCharacter,
        UseCharacter,
        MapAnswer,
        PlayerMovement,        
    }

    public enum PacketServer
    {
        Alert,
        Logged,
        AllClassData,
        AllCharacterAccount,
        MyCharacterData,
        ChangeToGameplay,
        MapCheck,
        PlayerData,
        PlayerRemove,
        PlayerMovement,
    }
}
