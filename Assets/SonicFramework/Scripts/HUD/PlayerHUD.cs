
using System;
using System.Collections;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using Lean.Pool;
using SonicFramework.Events;
using SonicFramework.HUD;
using SonicFramework.PlayerStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Debug = UnityEngine.Debug;
using State = SonicFramework.StateMachine.State;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace SonicFramework
{
    public class PlayerHUD : MonoBehaviour
    {
        [Inject] private Settings settings;
        [Inject] private PlayerInputService playerInputService;
        [Inject] private Stage stage;
        
        [Header("Homing Reticle HUD")]
        [SerializeField] private Transform homingReticle;
        private Animator homingReticleAnimator;
        
        [Header("Time HUD")]
        [SerializeField] private TMP_Text timeCountText;
        
        [Header("Ring HUD")]
        [SerializeField] private Transform endTransform;
        [SerializeField] private RingOnHUD ringPrefab;
        [SerializeField] private TMP_Text ringCountText;

        [Header("Level HUD")]
        [SerializeField] private TMP_Text speedLevelText;
        [SerializeField] private TMP_Text speedLevelAnimatePrefab;
        
        [Header("Score HUD")]
        [SerializeField] private TMP_Text scoreCountText;
        
        [Header("Speed Meter HUD")]
        [SerializeField] private Image speedMeter;
        [SerializeField] private float interpolationSpeed = 12f;
        private TMP_Text speedLevelTextAnimate;
        private float speedMeterValue;
        
        [Header("Parallax HUD")]
        public Transform parent;
        [SerializeField] private float parallaxForce;
        [SerializeField] private float maxParallax;
        
        [Header("Debug HUD")]
        [SerializeField] private TMP_Text debugText;
        [SerializeField] private TMP_Text versionText;

        [Header("Hint Mark")]
        [SerializeField] private CanvasGroup hintGroup;
        [SerializeField] private TMP_Text hintText;
        private Coroutine hintAppear;
        private Tween hintFade;
        private Tween hintTextFade;

        [Header("Result HUD")] 
        [SerializeField] private Transform resultParent;
        [SerializeField] private Transform resultTimeParent;
        [SerializeField] private Transform resultRingsParent;
        [SerializeField] private Transform resultScoreParent;
        [SerializeField] private Transform resultRankParent;
        [SerializeField] private Transform resultTotalParent;
        [SerializeField] private EventReference resultReference;
        [SerializeField] private EventReference resultShowReference;
        [SerializeField] private EventReference resultTotalReference;
        [SerializeField] private EventReference resultRankReference;
        [SerializeField] private Image resultFlash;
        public event Action OnHUDResult; 
        private EventInstance resultInstance;
        private EventInstance resultRankInstance;
        
        private Vector3 startPosition;
        private Camera cam;
        private bool enableRingsOnHud;

        private PlayerBase player;
        private bool subscribed;

        private void Awake()
        {
            player ??= FindFirstObjectByType<PlayerBase>();
        }

        private void Start()
        {
            hintGroup.alpha = 0;
            
            startPosition = parent.position;
            homingReticleAnimator ??= homingReticle.gameObject.GetComponent<Animator>();
            cam = Camera.main;

            bool condition = Debug.isDebugBuild && !Application.isEditor;
            debugText.enabled = condition;
            versionText.enabled = condition;
            
            versionText.text = $"Version: {Application.version}";
        }
        
        private void OnEnable()
        {
            GlobalSpawnablesEvents.OnRingCollected += UpdateRingCountText;
            GlobalSpawnablesEvents.OnRingWorldCollected += PlaceRingOnUI;
            GlobalSpawnablesEvents.OnOneHundredRingsCollected += OnUpgrade;
            GlobalSpawnablesEvents.OnScoreChanged += UpdateScoreCountText;
            
            GlobalSpawnablesEvents.OnHintTriggered += HintTriggered;
            GlobalSpawnablesEvents.OnGoal += OnGoal;
        }

        private void OnDisable()
        {
            GlobalSpawnablesEvents.OnRingCollected -= UpdateRingCountText;
            GlobalSpawnablesEvents.OnRingWorldCollected -= PlaceRingOnUI;
            GlobalSpawnablesEvents.OnOneHundredRingsCollected -= OnUpgrade;
            GlobalSpawnablesEvents.OnScoreChanged -= UpdateScoreCountText;
            
            GlobalSpawnablesEvents.OnHintTriggered -= HintTriggered;
            GlobalSpawnablesEvents.OnGoal -= OnGoal;
            player.fsm.OnStateEnter -= StartResult;
        }

        private void Update()
        {
            player ??= FindFirstObjectByType<PlayerBase>();

            if (player && !subscribed)
            {
                player.fsm.OnStateEnter += StartResult;
                subscribed = true;
            }
            
            enableRingsOnHud = settings.ringPickupType == RingPickupType.Unleashed;
            
            Parallax();
            SpeedMeter();
            HomingReticle();
            TimeHUD();
            SpeedUpgradeHUD();
        }

        private void TimeHUD()
        {
            timeCountText.text = Framework.GetTimeInString(player.stage.dataContainer.Time);
        }

        private void SpeedUpgradeHUD()
        {
            if (player.RingUpgrade.speedLevel < 5)
            {
                speedLevelText.text = $"Lvl. {player.RingUpgrade.speedLevel}";
            }
            else if (player.RingUpgrade.speedLevel == 5)
            {
                speedLevelText.text = "MAX";
            }

            if (speedLevelTextAnimate)
            {
                if (player.RingUpgrade.speedLevel < 5)
                {
                    speedLevelTextAnimate.text = $"Lvl. {player.RingUpgrade.speedLevel}";
                }
                else if (player.RingUpgrade.speedLevel == 5)
                {
                    speedLevelTextAnimate.text = "MAX";
                }
            }
        }

        private async void OnGoal()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1f), DelayType.Realtime);

            resultFlash.DOFade(1f, 0.35f);
        }

        private void OnUpgrade()
        {
            if (player.RingUpgrade.speedLevel < 5)
            {
                if (speedLevelTextAnimate) Destroy(speedLevelTextAnimate.gameObject);
                
                speedLevelTextAnimate = Instantiate(speedLevelAnimatePrefab, speedLevelText.rectTransform.position, Quaternion.identity, speedLevelText.transform.parent);
            
                Destroy(speedLevelTextAnimate.gameObject, 5f);
            }
        }

        private void HomingReticle()
        {
            if (player.fsm.CurrentState is StateAir air)
            {
                if (air.target != null)
                {
                    homingReticle.gameObject.SetActive(true);
                    homingReticleAnimator.Play("Open");
                    Vector3 pos = cam.WorldToScreenPoint(air.target.position);
                    homingReticle.position = pos;
                }
                else
                {
                    homingReticle.gameObject.SetActive(false);
                }
            }
            else
            {
                homingReticle.gameObject.SetActive(false);
            }
        }

        private void SpeedMeter()
        {
            if (player.fsm.CurrentState is not StateSpinDashCharge state)
            {
                speedMeterValue = Mathf.Lerp(speedMeterValue,
                    player.fsm.Velocity.magnitude / ((StateMove)player.fsm.CurrentState).maxSpeed,
                    interpolationSpeed * Time.deltaTime);
                speedMeter.fillAmount = speedMeterValue;
            }
            else
            {
                speedMeterValue = Mathf.Lerp(speedMeterValue, state.chargeTimer / state.spinDashMaxChargeTime,
                    interpolationSpeed * Time.deltaTime);
                speedMeter.fillAmount = speedMeterValue;
            }
        }

        private void Parallax()
        {
            Vector2 mouseInput = player.fsm.LocalVelocity;

            mouseInput.x = Mathf.Clamp(mouseInput.x, -maxParallax, maxParallax);
            mouseInput.y = Mathf.Clamp(mouseInput.y, -maxParallax, maxParallax);

            float posX = startPosition.x + mouseInput.x * parallaxForce;
            float posY = startPosition.y + mouseInput.y * parallaxForce;

            parent.position = Vector3.Lerp(parent.position, new Vector3(posX, posY, startPosition.z),
                4 * Time.deltaTime);
        }

        private void UpdateRingCountText(int value)
        {
            ringCountText.text = value.ToString("D3");
        }

        private void UpdateScoreCountText(int value)
        {
            //scoreCountText.text = value.ToString("D6");
            scoreCountText.text = value.ToString();
        }

        private void HintTriggered(string obj)
        {
            if (hintAppear != null)
            {
                hintFade?.Kill();
                hintTextFade?.Kill();
                
                hintGroup.alpha = 0f;
                hintText.text = "";
                StopCoroutine(hintAppear);
                hintAppear = null;
                StartCoroutine(OnHintTriggered(obj));
            }
            else
            {
                hintAppear = StartCoroutine(OnHintTriggered(obj));
            }
        }

        private IEnumerator OnHintTriggered(string obj)
        {
            hintTextFade = hintText.DOFade(0f, 0f);
            hintText.text = obj;
            hintTextFade = hintText.DOFade(1f, 0.5f);
            hintFade = hintGroup.DOFade(1f, 0.5f);

            yield return new WaitForSeconds(5f);
            hintFade = hintGroup.DOFade(0f, 1.5f);
        }

        private void PlaceRingOnUI(GameObject obj)
        {
            if (enableRingsOnHud)
            {
                var ring = LeanPool.Spawn(ringPrefab, transform);
                ring.Initialize(endTransform.position);
                
                ring.transform.position = cam.WorldToScreenPoint(obj.transform.position);
            }
        }

        private void StartResult(State state)
        {
            if (state is StateGoal)
            {
                resultInstance = RuntimeManager.CreateInstance(resultReference);
                resultInstance.start();

                resultRankInstance = RuntimeManager.CreateInstance(resultRankReference);
                
                StartCoroutine(ShowResult());
            }
        }

        private IEnumerator ShowResult()
        {
            resultFlash.DOFade(0f, 0.35f);
            
            var time = resultTimeParent.GetComponentInChildren<TMP_Text>();
            var rings = resultRingsParent.GetComponentInChildren<TMP_Text>();
            var score = resultScoreParent.GetComponentInChildren<TMP_Text>();
            var rankTitle = resultRankParent.GetComponentInChildren<TMP_Text>();
            var total = resultTotalParent.GetComponentInChildren<TMP_Text>();

            var rank = stage.GetRank();
            resultRankInstance.setParameterByNameWithLabel("Rank", rank.ToString());
            
            rankTitle.text = $"Rank: {rank.ToString()}";
            time.text = $"Time: {Framework.GetTimeInString(stage.dataContainer.Time)}";
            rings.text = $"Rings: {stage.dataContainer.RingCount}";
            score.text = $"Score: {stage.dataContainer.Score:D6}";
            total.text = $"Total: {stage.dataContainer.EndScore}";

            yield return new WaitForSeconds(0.25f);

            float d = 1f;
            resultTimeParent?.DOLocalMoveX(0, d);
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(resultShowReference);
            yield return new WaitForSeconds(0.5f);
            resultRingsParent?.DOLocalMoveX(0, d);
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(resultShowReference);
            yield return new WaitForSeconds(0.5f);
            resultScoreParent?.DOLocalMoveX(0, d);
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(resultShowReference);
            yield return new WaitForSeconds(0.5f);
            resultTotalParent?.DOLocalMoveX(0, d);
            yield return new WaitForSeconds(0.1f);
            RuntimeManager.PlayOneShot(resultTotalReference);

            yield return new WaitForSeconds(1.35f);
            resultRankParent?.DOLocalMoveX(0, 0.45f);
            resultRankInstance.start();
            
            OnHUDResult?.Invoke();
        }

        private void OnDestroy()
        {
            resultInstance.stop();
            resultRankInstance.stop();
        }
    }
}