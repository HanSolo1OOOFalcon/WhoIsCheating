﻿using System;
using BepInEx;
using UnityEngine;
using Utilla;
using WhoIsTalking;

namespace WhoIsCheating
{
    // This is your mod's main class.
    [BepInDependency("Graze.WhoIsTalking")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        Plugin()
        {
            HarmonyPatches.ApplyHarmonyPatches();
	    ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("cheese is gouda", PluginInfo.Name);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }
}
