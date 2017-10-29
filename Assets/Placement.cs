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

    public TextMesh whatText;

    public Animator animCube;
    public Animator animHealth;
    public Transform healthTransform;

    public float moduleHealth = 100.0f;
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
        text.rectTransform.anchoredPosition = new Vector3(120, (id + 1) * -30.0f, 0);

        whatText.text = _resource.name + "(" + _fuelResource.name + "->" + _resource.name + ")";
    }

    public void OnClick()
    {
        activated = !activated;
        animCube.SetBool("activated", activated);
    }

    private void Update()
    {
        text.text = res.name + ": " + res.amount.ToString("0.00") + " | health: " + moduleHealth.ToString("0.00");

        if ((Time.time - lastTime) > gameClock)
        {
            lastTime = Time.time;
            Tick();
        }

        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == gameObject)
            {
                //OnClick();
            }
        }

        healthTransform.localScale = new Vector3(0.2f, Mathf.Max(moduleHealth / 100.0f, 0.2f), 0.2f);
        animHealth.SetFloat("health", moduleHealth);
        
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

        moduleHealth -= res.damageRate;
        if(moduleHealth < 0.0f)
        {
            moduleHealth = 0.0f;
            activated = false;
            animCube.SetBool("activated", activated);
        }
    }
}
