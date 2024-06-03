
using ImGuiNET;
using SonicFramework.PlayerStates;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace SonicFramework.IMGUI
{
    public class IMGUIMenu : MonoBehaviour
    {
        [SerializeField] private Vector2 defaultPos;
        [SerializeField] private Vector2 defaultSize;
        [SerializeField] private float fpsDelay = 0.2f;
        
        [Inject] private Settings settings;
        [Inject] private SonicCameraConfig config;
        [Inject] private PlayerMovementConfig movementConfig;
        [Inject] private PlayerBase player;
        
        private int ringPickupType;
        
        private int fps;
        private float fpsTimer;

        private int selectedScene;
        private string[] scenes;

        private void Start()
        {
            fpsTimer = fpsDelay;
            
            scenes = new string[SceneManager.sceneCountInBuildSettings];
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            }

            defaultPos = new Vector2(Screen.width - 515, defaultPos.y);
            
            ringPickupType = (int)settings.ringPickupType;
        }

        private void OnEnable()
        {
            ImGuiUn.Layout += Layout;
        }

        private void OnDisable()
        {
            ImGuiUn.Layout -= Layout;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                Framework.BlockCursor();
            }

            if (fpsTimer > 0)
            {
                fpsTimer -= Time.unscaledDeltaTime;
            }
            
            if (fpsTimer <= 0)
            {
                fps = (int)(1f / Time.unscaledDeltaTime);
                fpsTimer = fpsDelay;
            }
        }

        private void Layout()
        {
            ImGui.SetNextWindowPos(defaultPos, ImGuiCond.Once);
            ImGui.SetNextWindowSize(defaultSize, ImGuiCond.Once);
            
            ImGui.Begin("Sonic Raster Framework");
            
            if (ImGui.CollapsingHeader("Player Info"))
            {
                ImGui.Text($"FPS: {Mathf.Round(fps)}");
                ImGui.Text($"Player State: {player.fsm.CurrentState.ToString().Replace("SonicFramework.PlayerStates.", "")}");
                ImGui.Text($"Last Player State: {player.fsm.LastState.ToString().Replace("SonicFramework.PlayerStates.", "")}");
                ImGui.Text($"Camera State: {player.Camera.fsm.CurrentState.ToString().Replace("SonicFramework.CameraStates.", "")}");
                ImGui.Text(FallWarning());
                ImGui.Text($"Speed: {player.fsm.Velocity.magnitude:00.0}");
                ImGui.Text($"Velocity: {player.fsm.Velocity}");
                ImGui.Text($"Local Velocity: {player.fsm.LocalVelocity}");
                
                ImGui.Spacing();
                ImGui.Text("Player Flags");
                
                foreach (var flag in player.Flags.List)
                {
                    string flagName = flag.ToString();
                    
                    for (int i = 0; i < flagName.Length; i++)
                    {
                        if (char.IsUpper(flagName[i]))
                        {
                            flagName = flagName.Insert(i, " ");
                            i++;
                        }
                    }
                    
                    ImGui.Text(flagName);
                }
            }
            
            if (ImGui.CollapsingHeader("Settings"))
            {
                if (settings)
                {
                    ImGui.Checkbox("Enable Camera Leaning", ref settings.enableCameraLeaning);
                    ImGui.SliderInt("Ring Pickup Type", ref ringPickupType, 0, 1, "Frontiers | Unleashed");
                    settings.ringPickupType = (RingPickupType)ringPickupType;
                }
                else
                {
                    ImGui.TextColored(Color.red, "Assign settings config in the inspector");
                }

                if (config)
                {
                    ImGui.SliderFloat("Sensitivity", ref config.sens, 0.1f, 5);
                    ImGui.SliderFloat("Min Distance", ref config.minDistance, 0.5f, 5);
                    ImGui.SliderFloat("Max Distance", ref config.maxDistance, 0.5f, 5);
                    ImGui.SliderFloat("Min Fov", ref config.minFov, 30, 75);
                    ImGui.SliderFloat("Max Fov", ref config.maxFov, 75, 150);
                    ImGui.SliderFloat3("Look Offset", ref config.lookOffset, -1, 1);
                    ImGui.SliderFloat("Max Lean", ref config.maxLean, 0.2f, 1f);
                    ImGui.SliderFloat("Homing Velocity Keep", ref movementConfig.homingVelocityKeep, 0f, 1f);
                }
                else
                {
                    ImGui.TextColored(Color.red, "Assign camera config in the inspector");
                }
            }
            
            if (ImGui.CollapsingHeader("Scenes"))
            {
                ImGui.ListBox("Scenes", ref selectedScene, scenes, scenes.Length);
            
                if (ImGui.Button("Load Selected Scene"))
                {
                    SceneManager.LoadScene(selectedScene);
                }
            
                if (ImGui.Button("Restart Current Scene"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }

            if (ImGui.Button("Add 5000 score"))
            {
                player.stage.dataContainer.Score += 5000;
            }
            
            ImGui.End();
        }

        private string FallWarning()
        {
            string c = "Ground Angle: 0";
            if (player.fsm.CurrentState is StateGround state)
            {
                c = $"Ground Angle: {Mathf.Round(state.groundAngle)}";

                if (player.fsm.Velocity.magnitude < 30 && state.groundAngle > 85f)
                {
                    c += " (You are going to fall lol)";
                }
            }

            return c;
        }
    }
}