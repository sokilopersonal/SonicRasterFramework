using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SonicFramework.UI
{
    public enum ButtonAnimationType
    {
        None,
        Right,
        Scale
    }
    
    public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private ButtonAnimationType _animationType = ButtonAnimationType.Right;
        
        private EventTrigger eventTrigger;
        private Tween fadeTween;
        
        private Button button;
        private RectTransform downTransform;

        private float initX;

        private void Awake()
        {
            eventTrigger = GetComponentInChildren<EventTrigger>();
            button = GetComponentInChildren<Button>();
            downTransform = transform.GetChild(0).GetComponent<RectTransform>();

            initX = downTransform.rect.x;
            Debug.Log(initX);
        }

        private void OnEnable()
        {
            var entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Select
            };
            entry.callback.AddListener(OnSelect);
            
            eventTrigger.triggers.Add(entry);
            
            var entry2 = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Deselect
            };
            entry2.callback.AddListener(OnDeselect);
            
            eventTrigger.triggers.Add(entry2);
        }

        private void OnDisable()
        {
            eventTrigger.triggers.Clear();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            button.Select();
            button.OnSelect(eventData);
            OnSelect();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnDeselect();
        }

        public void OnSelect()
        {
            fadeTween?.Kill();
            
            switch (_animationType)
            {
                case ButtonAnimationType.None:
                    break;
                case ButtonAnimationType.Right:
                    fadeTween = downTransform.DOLocalMoveX(initX + 75, 0.5f).SetEase(Ease.OutExpo);
                    break;
                case ButtonAnimationType.Scale:
                    fadeTween = downTransform.DOScale(1.3f, 0.5f).SetEase(Ease.OutExpo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnDeselect()
        {
            fadeTween?.Kill();

            switch (_animationType)
            {
                case ButtonAnimationType.None:
                    break;
                case ButtonAnimationType.Right:
                    fadeTween = downTransform.DOLocalMoveX(initX, 0.5f).SetEase(Ease.OutExpo);
                    break;
                case ButtonAnimationType.Scale:
                    fadeTween = downTransform.DOScale(1f, 0.5f).SetEase(Ease.OutExpo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public void OnSelect(BaseEventData eventData)
        {
            OnSelect();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            OnDeselect();
        }
    }
}