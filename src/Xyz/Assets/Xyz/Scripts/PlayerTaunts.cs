using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class PlayerTaunts
{
    private static List<string> _taunts = new List<string>()
    {
        "I only want to help!",
        "Yes! Chase me!",
        "Prove your mediocrity!",
        "If you cannot catch me how can we be safe?",
        "I only want to you to be safe!",
        "I respect your position!",
        "New hires probably won't decrease your wages!",
        "I just want the world to see my message!",
        "I'm just a normal guy!"
    };

    public static string GetRandomTaunt()
    {
        return _taunts.GetRandom();
    }
}
