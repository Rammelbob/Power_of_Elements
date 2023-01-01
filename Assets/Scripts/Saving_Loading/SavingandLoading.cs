using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingandLoading : MonoBehaviour
{
    private string Savepath => $"{Application.persistentDataPath}/save1.txt";

    [ContextMenu("Save")]
    private void Save()
    {
        var state = LoadFile();
        CaptureState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    private void Load()
    {
        var state = LoadFile();
        RestoreState(state);
    }

    private void SaveFile(object state)
    {
        using (var stream = File.Open(Savepath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private Dictionary<string,object> LoadFile()
    {
        if (!File.Exists(Savepath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(Savepath,FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string,object>)formatter.Deserialize(stream);
        }
    }

    private void CaptureState(Dictionary<string,object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }

    private void RestoreState(Dictionary<string,object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object value))
            {
                saveable.RestoreState(value);
            }
        }
    }
}
