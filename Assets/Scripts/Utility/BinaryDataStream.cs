using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class BinaryDataStream
{
    public static void Save<T>(T SerializableObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);
        
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Create);

        try
        {
            formatter.Serialize(fileStream, SerializableObject);
        }
        catch (SerializationException e)
        {
            Debug.Log("Save failed. Error: " + e.Message);
        }
        finally
        {
            if (fileStream != null)
            {
                fileStream.Close();
            }
        }
    }

    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/" + fileName + ".dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            try
            {
                return (T)formatter.Deserialize(fileStream);
            }
            catch (SerializationException e)
            {
                Debug.Log("Read failed. Error: " + e.Message);
                return default(T);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        return default(T);
    }

    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/" + fileName + ".dat";
        return File.Exists(path);
    }
}
