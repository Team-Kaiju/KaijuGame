using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GlobalSettings
{
    public static GlobalSettings instance = new GlobalSettings();

    public int renderDistance = 500;
    public int fov = 10;

    private GlobalSettings()
    {
        Load();
    }

    public void Save()
    {
        FileStream fs = new FileStream("game_settings.kkdat", FileMode.OpenOrCreate);
        BinaryWriter bw = new BinaryWriter(fs);

        Dictionary<string, int> dict = new Dictionary<string, int>();

        bw.Write(dict.Count);

        foreach(KeyValuePair<string, int> entry in dict)
        {
            bw.Write(entry.Key);
            bw.Write(entry.Value);
        }
    }

    public void Load()
    {
        FileStream fs = new FileStream("game_settings.kkdat", FileMode.OpenOrCreate);
        BinaryReader br = new BinaryReader(fs);

        Dictionary<string, int> dict = new Dictionary<string, int>();

        for (int i = 0; i < br.ReadInt32(); i++)
        {
            dict.Add(br.ReadString(), br.ReadInt32());
        }
    }
}
