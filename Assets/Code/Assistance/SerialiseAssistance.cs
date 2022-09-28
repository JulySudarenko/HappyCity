using System;
using UnityEngine;

namespace Code.Assistance
{
    public static class SerialiseAssistance
    {
        public static byte[] SerializeVector3(object obj)
        {
            Vector3 cast = (Vector3)obj;
            byte[] result = new byte[8];
            BitConverter.GetBytes(cast.x).CopyTo(result, 0);
            BitConverter.GetBytes(cast.y).CopyTo(result, 4);
            return result;
        }
        public static object DeserializeVector3(byte[] data)
        {
            var result = new Vector3();
            result.x = BitConverter.ToInt32(data, 0);
            result.y = BitConverter.ToInt32(data, 4);
            return result;
        }
    }
}
