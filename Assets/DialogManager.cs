using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    public GameObject window;
    public Text text;
    public ResourceManager resM;
    private int idx = 0;
    private List<string> parts;
    private bool active = false;
    private List<DialogEvent> events;
    private DialogEvent currentDialog;

    // Use this for initialization
    void Start () {
        window.SetActive(false);
        idx = 0;
        active = false;
        events = Dialogs.InitDialogs();
    }
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetMouseButtonDown(0))
        {
            Next();
        }
        if (!active)
        {
            for (int i = 0; i < events.Count; i++)
            {
                DialogEvent ev = events[i];
                if(!ev.isDone() && ev.shouldTrigger(resM))
                {
                    StartDialog(ev);
                    break;
                }
            }
        }
    }

    public void StartDialog(DialogEvent _de)
    {
        _de.StartThis();
        currentDialog = _de;
        parts = _de.parts;
        resM.Pause();
        window.SetActive(true);
        idx = -1;
        active = true;
        Next();
    }

    public void Next()
    {
        Debug.Log("Next in queue");
        idx++;
        if(idx >= parts.Count)
        {
            EndDialog();
            return;
        }
        text.text = parts[idx];
    }

    public void EndDialog()
    {
        window.SetActive(false);
        idx = 0;
        parts = null;
        active = false;
        if(currentDialog.postHook != null)
        {
            currentDialog.postHook(resM);
        }
        resM.EndDialog();
    }
}
