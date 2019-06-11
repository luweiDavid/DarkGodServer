



using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Protocol
{
    public class SerializeMgr
    {
        public static byte[] BinarySerialize(System.Object obj) {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(ms, obj);
                byte[] byteArr = ms.ToArray();

                PECommonTool.Log(">binary length >"+byteArr.Length.ToString());
                return byteArr;
            }
            catch (System.Exception e)
            {
                PECommonTool.Log("class to binary failed :" + e.Message);
            }
            finally {
                if (ms != null) {
                    ms.Dispose();
                } 
            }
            return new byte[0];
        }


        public static T[] DeserializeBinary<T>(byte[] rawData) {
            MemoryStream ms = null;
            T[] arr = null;
            PECommonTool.Log("===DeserializeBinary====  " + rawData.Length);
            try
            {
                BinaryFormatter bFormatter = new BinaryFormatter();
                ms = new MemoryStream(rawData);
                ms.Position = 0;
                arr = (T[])bFormatter.Deserialize(ms);
            }
            catch (System.Exception e)
            {
                PECommonTool.Log("deserialize failed :" + e.Message);
            }
            finally {
                if (ms != null) {
                    ms.Dispose();
                }
            }
           
            return arr;
        }
    }
}
