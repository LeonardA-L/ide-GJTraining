using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogEvent
{
    public delegate bool TriggerTest(ResourceManager resM);
    public delegate void PostHook(ResourceManager resM);
    public TriggerTest shouldTrigger;
    public PostHook postHook;
    public List<string> parts;
    private bool done = false;

	public DialogEvent(List<string> _parts, TriggerTest _shouldTrigger, PostHook _postHook = null)
	{
        parts = _parts;
        shouldTrigger = _shouldTrigger;
        postHook = _postHook;
    }

    public void StartThis()
    {
        done = true;
    }

    public bool isDone()
    {
        return done;
    }
}
