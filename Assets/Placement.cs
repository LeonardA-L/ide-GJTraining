using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placement : MonoBehaviour {
    public float id = 0;
    public Text text;
    public ResourceModel res;
    public ResourceModel fuel;
    public bool activated = false;
    private static int gameClock;
    private float lastTime;
	// Use this for initialization
	void Start () {
        lastTime = Time.time;

    }
    public void Init(ResourceModel _resource, ResourceModel _fuelResource, int _gameClock)
    {
        Debug.Log(id + " " + _resource.name + " " +_fuelResource);
        gameClock = _gameClock;
        res = _resource;
        fuel = _fuelResource;
        transform.position = new Vector3(0, (id) * 2, 0);
        // Update text position
        text.rectTransform.anchoredPosition = new Vector3(100, (id + 1) * -30.0f, 0);

    }

    private void Update()
    {
        text.text = res.name + " " + res.amount;

        if ((Time.time - lastTime) > gameClock)
        {
            lastTime = Time.time;
            Tick();
        }
    }

    private void Tick()
    {
        res.amount -= res.decay;

        if(activated && fuel.amount > 0)
        {
            res.amount += res.efficiency;
            fuel.amount -= 1;
        }

        if (res.amount < 0.0f)
        {
            res.amount = 0.0f;
        }
    }
}
