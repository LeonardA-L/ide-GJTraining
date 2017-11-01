using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {
    public float id = 0;
    public Text text;
    public ResourceModel res;
    private static int gameClock;
    private float lastTime;

    private ResourceManager manager = null;
    // Use this for initialization
    void Start () {
        lastTime = Time.time;

    }
    public void Init(ResourceModel _resource, ResourceManager resM)
    {
        gameClock = resM.data.gameClock;
        res = _resource;
        manager = resM;
        transform.position = new Vector3(0, (id) * 2, 0);
        // Update text position
        text.rectTransform.anchoredPosition = new Vector3(20, (id + 1) * -30.0f, 0);
    }

    private void Update()
    {
        text.text = res.name + ": " + res.amount.ToString("0.00");

        if ((Time.time - lastTime) > gameClock)
        {
            lastTime = Time.time;
            Tick();
        }
    }

    public void Tick()
    {

    }
}
