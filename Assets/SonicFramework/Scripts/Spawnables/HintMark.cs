using DG.Tweening;
using FMODUnity;
using SonicFramework.Events;
using UnityEngine;

namespace SonicFramework
{
    public class HintMark : PlayerContactable
    {
        [SerializeField, TextArea(2, 4)] private string hint;
        [SerializeField] private Transform model;

        [SerializeField] private EventReference soundReference;

        private float delayTimer;

        private void Update()
        {
            if (delayTimer > 0)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                delayTimer = 0;
            }
        }

        public override void OnContact()
        {
            base.OnContact();

            if (delayTimer == 0)
            {
                model.DOLocalMoveY(1f, 0.45f).SetEase(Ease.OutSine).onComplete = () =>
                {
                    model.DOLocalMoveY(0, 1.25f).SetEase(Ease.OutBounce);
                };

                delayTimer = 5f;
                RuntimeManager.PlayOneShot(soundReference);
                
                GlobalSpawnablesEvents.OnHintTriggered?.Invoke(hint);
            }
        }
    }
}