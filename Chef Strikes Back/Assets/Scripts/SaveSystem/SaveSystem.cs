using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public void Save<T>(T data, string savePath)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream fileStream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public T Load<T>(string savePath)
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            try
            {
                T loadedData = (T)formatter.Deserialize(stream);
                stream.Close();

                return loadedData;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message + " = ERROR LOADING SAVE DATA");
                stream.Close();
                return default(T);
            }
        }
        else
        {
            Debug.Log("Save File Not Found");
            return default(T);
        }
    }
}
