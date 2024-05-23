using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SonicFramework.UI
{
    public class UIFlash : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        private EventTrigger eventTrigger;
        private Tween fadeTween;
        
        private Button button;

        private void Awake()
        {
            eventTrigger = GetComponentInChildren<EventTrigger>();
            button = GetComponentInChildren<Button>();
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
            fadeTween = transform.DOScale(1.3f, 0.2f);
        }

        public void OnDeselect()
        {
            fadeTween?.Kill();
            fadeTween = transform.DOScale(1.0f, 0.2f);
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