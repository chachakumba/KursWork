using System;
using System.Collections.Generic;
using System.IO;

namespace EntityContext //Contains base classes and saving methods
{
    public class Context
    {
        static string savePath = AppDomain.CurrentDomain.BaseDirectory + @"save.bgg";
        public static void SaveBinar(Save save)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream file = File.Create(savePath);
            bf.Serialize(file, save);
            file.Close();
        }
        public static void DeleteSave()
        {
            File.Delete(savePath);
        }
        public static Save LoadSaveBinary()
        {
            if (
            File.Exists(savePath))
            {
                FileStream file = File.Open(savePath, FileMode.Open);
                try
                {
                    System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var save = bf.Deserialize(file);
                    file.Close();
                    return (Save)save;
                }
                catch (Exception e)
                {
                    file.Close();
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        static void Main(string[] args) { }
    }
}
