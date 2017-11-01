﻿using System.Collections;
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

    // Use this for initialization
    void Start () {
        window.SetActive(false);
        idx = 0;
        active = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (active && Input.GetMouseButtonDown(0))
        {
            Next();
        }
    }

    public void StartDialog(List<string> _parts)
    {
        parts = _parts;
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

        resM.EndDialog();
    }
}
