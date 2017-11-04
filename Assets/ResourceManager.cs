using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ResourceManager : MonoBehaviour {
    string gameDataFileName = "gamedata.json";
    public GameDataModel data;
    public int frame;
    private float lastTime;
    public bool timeRuns = false;

    public Text timerText;
    public DialogManager dialogManager;
    private int onboardingStep = 0;

    public float timer = 0;
    private Equipment ductTape;
    private List<Placement> resources;

    public int OnboardingStep
    {
        get
        {
            return onboardingStep;
        }

        set
        {
            onboardingStep = value;
        }
    }

    // Use this for initialization
    void Start ()
    {
        resources = new List<Placement>();
        lastTime = 0;
        frame = 0;
        data = LoadGameData();
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
            resources.Add(placement);
        }

        GameObject duct = (GameObject)GameObject.Instantiate(Resources.Load("Equipment"));
        duct.transform.position = new Vector3(0, 0, 0);
        Equipment equipment = duct.GetComponent<Equipment>();
        equipment.id = i;
        equipment.Init(data.ductTape, this);
        ductTape = equipment;
        timeRuns = true;
    }
	
	// Update is called once per frame
	void Update () {
        if(timeRuns)
        {
            timer += Time.deltaTime;
            frame++;

            /*if(timer >= 5.0f && !didDialog1) {
                didDialog1 = true;
                List<string> parts = new List<string>();
                parts.Add("Salut les Seagulls, j'espère que vous aimez bien le proto allez salut.");
                StartDialog(parts);
            }

            if (timer >= 10.0f && !didDialog2)
            {
                didDialog2 = true;
                List<string> parts = new List<string>();
                parts.Add("Ma soeur s'est faite mordre par un élan une fois.");
                parts.Add("C'est dangereux les élans.");
                StartDialog(parts);
            }*/
        }

        if ((timer - lastTime) > data.gameClock)
        {
            lastTime = Time.time;
            Tick();
        }

        timerText.text = "T:" + timer.ToString("0.00") + "s";
    }

    public void ToggleTime()
    {
        timeRuns = !timeRuns;
    }

    public void Pause()
    {
        timeRuns = false;
    }

    public void Play()
    {
        timeRuns = true;
    }

    private void Tick()
    {
        if (timeRuns)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                resources[i].Tick();
            }
            ductTape.Tick();
        }
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
    /*
    private void StartDialog(List<string> parts)
    {
        Pause();
        dialogManager.StartDialog(parts);
    }

    private void StartDialog(string singlePart)
    {
        List<string> parts = new List<string>();
        parts.Add(singlePart);
        StartDialog(parts);
    }
    */
    public void EndDialog()
    {
        Play();
    }

    public bool IsActive(string name)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if(resources[i].res.name == name)
            {
                return resources[i].activated;
            }
        }
        return false;
    }

    public float GetModuleHealth(string name)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].res.name == name)
            {
                return resources[i].moduleHealth;
            }
        }
        return -1.0f;
    }
}
