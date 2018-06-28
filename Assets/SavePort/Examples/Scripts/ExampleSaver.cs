using SavePort.Saving;
using UnityEngine;

public class ExampleSaver : MonoBehaviour {

    public string fileName;

    public void SaveGame() {
        SaveManager.SaveContainers(fileName);
    }

    public void LoadGame() {
        SaveManager.LoadContainers(fileName);
    }
}
