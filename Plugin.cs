using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Revive;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string PluginGuid = "maxenterme.Revive";
    private const string PluginName = "Revive";
    private const string PluginVersion = "1.0.3";

    internal static Plugin Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;

    internal static ConfigEntry<KeyCode> ReviveKey = null!;
    internal static ConfigEntry<int> ReviveHealth = null!;
    internal static ConfigEntry<bool> UseHPCost = null!;
    internal static ConfigEntry<int> HPCost = null!;
    internal static ConfigEntry<int> InvincibilityDuration = null!;
    internal static ConfigEntry<bool> UseMaxDistance = null!;
    internal static ConfigEntry<float> MaxDistance = null!;

    private void Awake()
    {
        Instance = this;

        ReviveKey = Config.Bind(
            "Settings", "ReviveKey", KeyCode.F1,
            "Key to revive all dead players (host only)."
        );
        ReviveHealth = Config.Bind(
            "Settings", "ReviveHealth", 50,
            new ConfigDescription("HP to restore on revive", new AcceptableValueRange<int>(1, 100))
        );
        UseHPCost = Config.Bind(
            "Settings", "UseHPCost", false,
            "Deduct HP from the player who triggers the revive."
        );
        HPCost = Config.Bind(
            "Settings", "HPCost", 25,
            new ConfigDescription("HP cost to the reviver (if UseHPCost is enabled)", new AcceptableValueRange<int>(1, 100))
        );
        InvincibilityDuration = Config.Bind(
            "Settings", "InvincibilityDuration", 3,
            new ConfigDescription("Seconds of invincibility after revive", new AcceptableValueRange<int>(0, 30))
        );
        UseMaxDistance = Config.Bind(
            "Settings", "UseMaxDistance", false,
            "Only revive dead players within MaxDistance of the reviver."
        );
        MaxDistance = Config.Bind(
            "Settings", "MaxDistance", 5f,
            new ConfigDescription("Max distance to revive (if UseMaxDistance is enabled)", new AcceptableValueRange<float>(1f, 50f))
        );

        new Harmony(PluginGuid).PatchAll(typeof(Plugin).Assembly);
        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded!");
    }
}
