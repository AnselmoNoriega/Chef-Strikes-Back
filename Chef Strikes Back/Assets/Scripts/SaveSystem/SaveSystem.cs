using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour, IGameModule
{
    private string _filePath = null;

    public IEnumerator LoadModule()
    {
        _filePath = Application.persistentDataPath + "/Saves";
        ServiceLocator.Register<SaveSystem>(this);
        yield break;
    }

    public void Save<T>(T data, string saveName)
    {
        if(!Directory.Exists(_filePath))
        {
            Directory.CreateDirectory(_filePath);
        }

        BinaryFormatter formatter = new BinaryFormatter();

        string savePath = _filePath + "/" + saveName;

        FileStream fileStream = new FileStream(savePath, FileMode.Create);
        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public T Load<T>(string saveName)
    {
        string savePath = _filePath + "/" + saveName;

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
