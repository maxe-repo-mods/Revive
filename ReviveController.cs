using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Revive;

[HarmonyPatch]
public static class ReviveController
{
    [HarmonyPatch(typeof(RoundDirector), "Update")]
    [HarmonyPostfix]
    private static void RoundDirector_Update_Postfix()
    {
        try
        {
            if (!SemiFunc.IsMasterClientOrSingleplayer()) return;
            if (!SemiFunc.RunIsLevel()) return;
            if (!Input.GetKeyDown(Plugin.ReviveKey.Value)) return;

            ReviveAllDeadPlayers();
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"Revive Update error: {e}");
        }
    }

    private static void ReviveAllDeadPlayers()
    {
        List<PlayerAvatar> players = SemiFunc.PlayerGetAll();
        if (players == null) return;

        PlayerAvatar? localPlayer = null;
        if (Plugin.UseHPCost.Value || Plugin.UseMaxDistance.Value)
            localPlayer = PlayerAvatar.instance;

        int revivedCount = 0;

        foreach (var player in players)
        {
            if (player == null) continue;
            if (!player.deadSet) continue;

            if (Plugin.UseMaxDistance.Value && localPlayer != null)
            {
                float dist = Vector3.Distance(localPlayer.transform.position, player.transform.position);
                if (dist > Plugin.MaxDistance.Value)
                {
                    Plugin.Logger.LogInfo($"Skipping {player.playerName}: too far ({dist:F1}m > {Plugin.MaxDistance.Value}m)");
                    continue;
                }
            }

            if (Plugin.UseHPCost.Value && localPlayer != null && revivedCount == 0)
            {
                int currentHP = localPlayer.playerHealth.health;
                if (currentHP <= Plugin.HPCost.Value)
                {
                    Plugin.Logger.LogWarning("Not enough HP to revive.");
                    return;
                }
            }

            player.Revive(false);
            player.playerHealth.HealOther(Plugin.ReviveHealth.Value, true);

            if (Plugin.InvincibilityDuration.Value > 0)
                player.playerHealth.InvincibleSet((float)Plugin.InvincibilityDuration.Value);

            revivedCount++;
            Plugin.Logger.LogInfo($"Revived player: {player.playerName}");
        }

        if (Plugin.UseHPCost.Value && localPlayer != null && revivedCount > 0)
        {
            int damage = Plugin.HPCost.Value * revivedCount;
            int currentHP = localPlayer.playerHealth.health;
            int newHP = Mathf.Max(1, currentHP - damage);
            int healAmount = newHP - currentHP;
            if (healAmount < 0)
                localPlayer.playerHealth.HealOther(healAmount, true);
        }

        if (revivedCount > 0)
            Plugin.Logger.LogInfo($"Revived {revivedCount} player(s).");
    }
}
