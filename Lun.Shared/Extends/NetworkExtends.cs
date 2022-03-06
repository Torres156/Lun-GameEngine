using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lun.Shared.Extends
{
    public static class NetworkExtends
    {
        public static void Put(this NetDataWriter obj, Vector2 v)
        {
            obj.Put(v.x);
            obj.Put(v.y);
        }

        public static Vector2 GetVector2(this NetDataReader obj)
            => new Vector2(obj.GetFloat(), obj.GetFloat());

        public static void Put(this NetDataWriter obj, Color c)
            => obj.Put(c.ToInteger());

        public static void GetColor(this NetDataReader obj)
            => new Color(obj.GetUInt());
    }
}
