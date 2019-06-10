



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


        public static List<T> DeserializeBinary<T>(byte[] rawData) {
            MemoryStream ms = null;
            List<T> _list = null;
            try
            {
                BinaryFormatter bFormatter = new BinaryFormatter(); 
                ms = new MemoryStream(rawData);
                ms.Position = 0;
                _list = (List<T>)bFormatter.Deserialize(ms);

                ms.Dispose();
            }
            catch (System.Exception e)
            { 
                PECommonTool.Log("deserialize failed :" + e.Message);
            } 
            return _list;
        }
    }
}
