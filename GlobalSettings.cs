using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalSettings
{
    public static int renderDist = 500;
    public static int fov = 60;

    public static void Save()
    {
        PlayerPrefs.SetInt("FOV", fov);
        PlayerPrefs.SetInt("RenderDist", renderDist);
    }

    public static void Load()
    {
        fov = PlayerPrefs.GetInt("FOV");
        renderDist = PlayerPrefs.GetInt("RenderDist");
    }
}
