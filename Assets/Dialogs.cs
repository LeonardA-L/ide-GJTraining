using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogs
{
	public static List<DialogEvent> InitDialogs()
    {
        List<DialogEvent> events = new List<DialogEvent>();

        // Write new dialog events here

        events.Add(new DialogEvent(
            CreateListString("Salut. Active la citerne stp je veux pas mourir de soif."),
            Step0
        ));

        events.Add(new DialogEvent(
            CreateListString("J'attend là gros active la citerne il faut cliquer dessus",
                             "T'abuses grave n'empêche"),
            Step0_Water_Tank_Still_Not_Active
        ));

        // EXAMINONS CET EXEMPLE
        events.Add(new DialogEvent(                                         // Il faut garder cette ligne
            CreateListString("Ah bien la citerne est activée.",             // Ajouter un message
                             "Bon maintenant répare le générateur"),        // *Eventuellement*, ajouter d'autres messages
            Step0_Water_Tank_Active,                                        // Donner une fonction de condition de déclenchement
            End_Of_Step0                                                    // *Eventuellement*, ajouter une fonction d'action à effectuer après le dialogue
        ));

        events.Add(new DialogEvent(
            CreateListString("Ah ben bien joué je pensais pas que tu y arriverais"),
            Step1_Generator_Is_Repaired,
            End_Of_Step1
        ));





        return events;
    }


    // ############# TRIGGER FUNCTIONS ###############


    static bool Step0(ResourceManager _resM)
    {
        return _resM.OnboardingStep == 0    // OnBoardingStep is 0
            && timeIs(5.0f, _resM);         // AND timer is at 5s
    }

    static bool Step0_Water_Tank_Still_Not_Active(ResourceManager _resM)
    {
        return _resM.OnboardingStep == 0     // OnBoardingStep is 0
            && timeIs(10.0f, _resM)          // AND timer is at 10s
            && !isWaterTankActive(_resM);    // AND water tank is NOT active
    }

    static bool Step0_Water_Tank_Active(ResourceManager _resM)
    {
        return _resM.OnboardingStep == 0    // OnBoardingStep is 0
            && timeIs(6.0f, _resM)          // AND timer is at 6s (or more)
            && isWaterTankActive(_resM);    // AND water tank is active
    }

    static bool Step1_Generator_Is_Repaired(ResourceManager _resM)
    {
        return _resM.OnboardingStep == 1                        // OnBoardingStep is 1
            && _resM.GetModuleHealth("electricity") >= 80.0f;   // AND generator health is more than 80%
    }










    // ############# POST DIALOG ACTIONS ###############

    static void End_Of_Step0(ResourceManager _resM)
    {
        Debug.Log("User just did step 0");
        IncrementOnboardingStep(_resM);
    }

    static void End_Of_Step1(ResourceManager _resM)
    {
        Debug.Log("User just did step 1");
        IncrementOnboardingStep(_resM);
        // Example: Decrease number of potatoes
        // Example: Deactivate generator for 10s
        // ...
    }














    // ############# TOOLS ############# don't touch
    private static List<string> CreateListString(params string[] _parts)
    {
        List<string> parts = new List<string>();
        for (int i = 0; i < _parts.Length; i++)
        {
            parts.Add(_parts[i]);
        }
        return parts;
    }

    private static bool timeIs(float _time, ResourceManager _resM)
    {
        return _resM.timer >= _time;
    }

    private static bool isWaterTankActive(ResourceManager _resM)
    {
        return _resM.IsActive("water");
    }
    private static bool isPotatoFieldActive(ResourceManager _resM)
    {
        return _resM.IsActive("potatoes");
    }
    private static bool isGeneratorActive(ResourceManager _resM)
    {
        return _resM.IsActive("electricity");
    }

    private static void IncrementOnboardingStep(ResourceManager _resM)
    {
        _resM.OnboardingStep = _resM.OnboardingStep + 1;
    }
}
