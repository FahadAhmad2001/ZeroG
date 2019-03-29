using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FrameworkZeroG.CustomUnitySerializable
{
    public class ConvertCustomTypes
    {
        public static SerializableQuaternion ConvertQuaternionSerializable(Quaternion quaternion)
        {
            SerializableStuff converter = new SerializableStuff();
            return converter.ConvertQuaternionSerializable(quaternion);
        }
        public static Quaternion ConvertQuaternionOriginal(SerializableQuaternion quaternion)
        {
            SerializableStuff converter = new SerializableStuff();
            return converter.ConvertQuaternionOriginal(quaternion);
        }
        public static SerializableVector3 ConvertVectorSerializable(Vector3 vector)
        {
            SerializableStuff converter = new SerializableStuff();
            return converter.ConvertVectorSerializable(vector);
        }
        public static Vector3 ConvertVectorOriginal(SerializableVector3 vector)
        {
            SerializableStuff converter = new SerializableStuff();
            return converter.ConvertVectorOriginal(vector);
        }
    }
}
