using System;
using DG.Tweening;
using UnityEngine;

namespace SonicFramework
{
    public class JumpBallModel : MonoBehaviour
    {
        [SerializeField] private ParticleSystem twirl;
        [SerializeField] private Vector3 enterPunch;
        [SerializeField] private Vector3 outerPunch;
        [SerializeField] private float duration;
        [SerializeField] private int vibrato;
        [SerializeField] private float elasticity;

        private Tween t;
        private Tween t2;

        private void OnEnable()
        {
            twirl.Play();
        }

        private void OnDisable()
        {
            twirl.Stop();   
        }

        private void Update()
        {
            var main = twirl.main;
            main.startSizeXMultiplier = transform.localScale.x;
            main.startSizeYMultiplier = transform.localScale.y;
            main.startSizeZMultiplier = transform.localScale.z;
        }

        public void Animate()
        {
            transform.localScale = Vector3.one;

            t?.Kill();
            t = transform.DOPunchScale(enterPunch, duration, vibrato, elasticity);
            t.onComplete += () => t = transform.DOScale(Vector3.one, 0.2f);

            t2?.Kill();
            t2 = transform.DOPunchScale(outerPunch, duration, vibrato, elasticity);
            t2.onComplete += () => t2 = transform.DOScale(Vector3.one, 0.2f);
        }
    }
}
