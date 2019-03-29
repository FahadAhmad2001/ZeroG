using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FrameworkZeroG.CustomUnitySerializable
{

    public class SerializableStuff
    {
        public SerializableVector3 ConvertVectorSerializable(Vector3 vector)
        {
            return new SerializableVector3(vector.x, vector.y, vector.z);
        }
        public Vector3 ConvertVectorOriginal(SerializableVector3 vector)
        {
            return new Vector3(vector.x, vector.y, vector.z);
        }
        public SerializableQuaternion ConvertQuaternionSerializable(Quaternion quaternion)
        {
            return new SerializableQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
        public Quaternion ConvertQuaternionOriginal(SerializableQuaternion quaternion)
        {
            return new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
        }
    }
    [Serializable]
    public struct SerializableVector3
    {
        public float x;
        public float y;
        public float z;
        public SerializableVector3(float valX, float valY, float valZ)
        {
            x = valX;
            y = valY;
            z = valZ;
        }
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("[{0}, {1}, {2}]", x, y, z);
        }
    }
    [Serializable]
    public struct SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;
        public SerializableQuaternion(float rX, float rY, float rZ, float rW)
        {
            x = rX;
            y = rY;
            z = rZ;
            w = rW;
        }
        public override string ToString()
        {
            return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
        }
    }
}
