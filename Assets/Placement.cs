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
    public bool repairing = false;
    private static int gameClock;

    public TextMesh whatText;

    public Animator animCube;
    public Animator animHealth;
    public Transform healthTransform = null;

    public float moduleHealth = 100.0f;
    private float efficiencyModifier = 1;
    private ResourceManager manager = null;
    // Use this for initialization
    void Start () {

    }
    public void Init(ResourceModel _resource, ResourceModel _fuelResource, ResourceManager resM)
    {
        Debug.Log(id + " " + _resource.name + " " +_fuelResource);
        gameClock = resM.data.gameClock;
        res = _resource;
        fuel = _fuelResource;
        manager = resM;
        transform.position = new Vector3(0, (id) * 2, 0);
        // Update text position
        text.rectTransform.anchoredPosition = new Vector3(20, (id + 1) * -30.0f, 0);

        whatText.text = _resource.name + "(" + _fuelResource.name + "->" + _resource.name + ")";
    }

    public void OnClick(string name)
    {
        if(name == "mod")
        {
            activated = !activated;
            animCube.SetBool("activated", activated);
        }
        else if (name =="health") {
            repairing = true;
        }
    }

    private void Update()
    {
        text.text = res.name + ": " + res.amount.ToString("0.00") + " | health: " + moduleHealth.ToString("0.00") + " | efficiency: " + (res.efficiency * efficiencyModifier);

        healthTransform.localScale = new Vector3(0.2f, Mathf.Max(moduleHealth / 100.0f, 0.2f), 0.2f);
        animHealth.SetFloat("health", moduleHealth);

        efficiencyModifier = 1.0f;
        for (int i = 0; i < manager.data.moduleHealthThresholds.Length; i++)
        {
            ModuleHealthThreshold thr = manager.data.moduleHealthThresholds[i];
            if(moduleHealth <= thr.threshold)
            {
                efficiencyModifier = thr.modifier;
                break;
            }
        }
        
        if(!Input.GetMouseButton(0))
        {
            repairing = false;
        }

    }

    public void Tick()
    {
        if (moduleHealth < 0.0f)
        {
            moduleHealth = 0.0f;
            activated = false;
            animCube.SetBool("activated", activated);
        }

        res.amount -= res.decay;

        if(activated && fuel.amount > 0)
        {
            res.amount += res.efficiency * efficiencyModifier;
            fuel.amount -= 1;
        }

        if (res.amount < 0.0f)
        {
            res.amount = 0.0f;
        }

        if(activated)
        {
            moduleHealth -= res.damageRate;
        }

        if (repairing && manager.data.ductTape.amount >= manager.data.ductTape.efficiency)
        {
            moduleHealth += manager.data.ductTape.efficiency;
            manager.data.ductTape.amount -= 1;

            if (manager.data.ductTape.amount < 0.0f)
            {
                manager.data.ductTape.amount = 0.0f;
            }
        }
    }
}
