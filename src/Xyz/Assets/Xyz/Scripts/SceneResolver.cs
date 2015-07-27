using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SceneResolver
{
    public enum Scene
    {
        Unknown,
        Title,
        Intro,
        Main
    }

    private readonly static Dictionary<Scene, string> SceneMap = new Dictionary<Scene, string>
    {
        {Scene.Title, "Title"},
        {Scene.Intro, "Intro"},
        {Scene.Main, "Main"}
    };


    public static string GetSceneName(Scene scene)
    {
        string val;
        SceneMap.TryGetValue(scene, out val);
        return val;
    }

}
