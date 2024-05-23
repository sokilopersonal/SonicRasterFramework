using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

namespace SonicFramework.UI
{
    public class Pause : MonoBehaviour
    {
        private Canvas canvas;
        private Tween fadeTween;

        [HideInInspector] public bool active;

        [SerializeField] private CanvasGroup main;
        [SerializeField] private Button mainButton;
        
        [Inject] private PlayerInputService playerInput;
        [Inject] private MenuInputService menuInput;
        
        private List<Button> allButtons = new List<Button>();

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            
            allButtons = new List<Button>(GetComponentsInChildren<Button>(true));
            
            foreach (Button button in allButtons)
            {
                button.interactable = false;
            }
            
            canvas.enabled = false;
            main.alpha = 0f;
        }

        private void Update()
        {
            if (menuInput.WasPressed(InputButton.Pause) && !active)
            {
                Enable();
            }
        }

        public void Enable()
        {
            if (!active)
            {
                fadeTween?.Kill();
                canvas.enabled = true;
                mainButton.Select();
                
                foreach (Button button in allButtons)
                {
                    button.interactable = true;
                }
                
                fadeTween = main.DOFade(1f, 0.5f).SetUpdate(UpdateType.Normal);
                playerInput.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
                active = true;
            }
            else
            {
                fadeTween?.Kill();
                fadeTween = main.DOFade(0f, 0.5f).SetUpdate(UpdateType.Normal);
                EventSystem.current.SetSelectedGameObject(null);
                
                foreach (Button button in allButtons)
                {
                    button.interactable = false;
                }
                
                fadeTween.onComplete = () => canvas.enabled = false;
                playerInput.enabled = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                active = false;
            }
        }

        public void Select(GameObject go)
        {
            EventSystem.current.SetSelectedGameObject(go);
        }

        public void Quit()
        {
#if !UNITY_EDITOR
            Application.Quit();
#else
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}