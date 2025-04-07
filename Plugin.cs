using BepInEx;
using Photon.Pun;

namespace WhoIsCheating
{
    // This is your mod's main class.
    [BepInDependency("Graze.WhoIsTalking")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();
	        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("cheese is gouda", PluginInfo.Name);
            PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        }
    }
}
