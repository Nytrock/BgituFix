using System.IO;
using UnityEngine;

public class FileManager {
    private readonly string _dataPath = Application.persistentDataPath;
    private readonly string _filePath;
    private const string _fileExtension = "nyt";

    public FileManager(string fileName) {
        _filePath = _dataPath + "/" + string.Concat(fileName, ".", _fileExtension);
    }

    public void Save<TData>(TData data) {
        File.WriteAllText(_filePath, JsonUtility.ToJson(data));
    }

    public TData Load<TData>() where TData : class {
        if (!IsFileExists())
            return null;
        return JsonUtility.FromJson<TData>(File.ReadAllText(_filePath));
    }

    public void Delete() {
        if (!IsFileExists())
            return;

        File.Delete(_filePath);
    }

    public bool IsFileExists() {
        return File.Exists(_filePath);
    }
}
