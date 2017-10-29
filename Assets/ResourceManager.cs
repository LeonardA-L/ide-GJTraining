using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceManager : MonoBehaviour {
    string gameDataFileName = "gamedata.json";
    public GameDataModel data;
    public float gameClock;
    public int frame;

    // Use this for initialization
    void Start () {
        gameClock = 0.0f;
        frame = 0;
        data = LoadGameData();
        Debug.Log(data.resources.Length);
        int i = 0;
        for (; i < data.resources.Length; i++)
        {
            int prevI = i - 1;
            if(prevI < 0)
            {
                prevI += data.resources.Length;
            }
            GameObject res = (GameObject)GameObject.Instantiate(Resources.Load("Module"));
            res.transform.position = new Vector3(0, 0, 0);
            Placement placement = res.GetComponent<Placement>();
            placement.id = i;
            placement.Init(data.resources[i], data.resources[prevI], this);
        }

        GameObject duct = (GameObject)GameObject.Instantiate(Resources.Load("Equipment"));
        duct.transform.position = new Vector3(0, 0, 0);
        Equipment equipment = duct.GetComponent<Equipment>();
        equipment.id = i;
        equipment.Init(data.ductTape, this);
    }
	
	// Update is called once per frame
	void Update () {
        gameClock += Time.deltaTime;
        frame++;

    }

    private GameDataModel LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            GameDataModel loadedData = JsonUtility.FromJson<GameDataModel>(dataAsJson);

            return loadedData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
            throw new System.Exception("Cannot load data");
        }
    }
}
