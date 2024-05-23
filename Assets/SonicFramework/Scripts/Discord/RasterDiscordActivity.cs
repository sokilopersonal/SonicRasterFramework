using System;
using System.Collections;
using Discord;
using SonicFramework.UI;
using UnityEngine;
using Zenject;

namespace SonicFramework.DiscordRPC
{
    public class RasterDiscordActivity : IInitializable, ITickable, ILateDisposable
    {
        private static RasterDiscordActivity _instance;
        
        private Discord.Discord discord;
        private const long APPLICATION_ID = 1234135443272896603;
        
        [Inject] private PlayerBase player;
        [Inject] private StageInfo stageInfo;
        [Inject] private Pause pause;

        public void Initialize()
        {
            try
            {
                discord = new Discord.Discord(APPLICATION_ID, (UInt64)CreateFlags.NoRequireDiscord);
            }
            catch (Exception e)
            {
                Debug.Log($"Discord error: {e}");
            }
        }

        public void Tick()
        {
            if (discord != null)
            {
                string details = $"Score: {player.stage.dataContainer.Score}";
                string state = !pause.active ? $"Time: {Framework.GetTimeInString(player.stage.dataContainer.Time)}" : 
                    $"Time: {Framework.GetTimeInString(player.stage.dataContainer.Time)} (Paused)"; 
                
                var activityManager = discord.GetActivityManager();
                var activity = new Activity
                {
                    Details = details,
                    State = state,
                    Assets = new ActivityAssets
                    {
                        LargeImage = stageInfo.largeImageName,
                        LargeText = $"{stageInfo.stageName} ({player.GetType().Name})"
                    }
                };
            
                activityManager.UpdateActivity(activity, _ => { });
                discord.RunCallbacks();
            }
        }

        public void LateDispose()
        {
            discord?.Dispose();
        }
    }
}