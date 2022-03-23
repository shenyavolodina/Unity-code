using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UnityIC
{
    //The class for checking the end of an animation by calling the OnComplete method.
    //Example : characterAnimatorController.SetAnimation(AnimatorParameters.Idle).OnComplete(()=>Debug.Log("AnimationComplete"));
    public class CharacterAnimatorController : MonoBehaviour
    {
        public AnimatorParameters parameter = AnimatorParameters.Idle;
        public AnimationCallback animationCallback = null;

        private static CharacterAnimatorController m_characterAnimatorController = null;
        private Animator animator = null;
        private List<AnimatorClipInfo> _animatorClipInfos = new List<AnimatorClipInfo>();
        private float _cureentClipLength = 0f;
        private float _countOfClips = 0f;
        private void Awake()
        {
            m_characterAnimatorController = this;
        }

        private void OnEnable()
        {
            animator = GetComponentInChildren<Animator>();
            SetAnimation(parameter);
        }
        public static void InvokeCallback()
        {
            m_characterAnimatorController.animationCallback?.Invoke();
        }

        private void Update()
        {
            if (animator)
            {
                _animatorClipInfos = animator.GetCurrentAnimatorClipInfo(0).ToList();
                if (_animatorClipInfos.Count > 0)
                {
                    _cureentClipLength = _animatorClipInfos[0].clip.length;
                }
            }
        }

        public CharacterAnimatorController SetAnimation(AnimatorParameters newParameter, int countOfClips = 1)
        {
            StopCoroutine(WaitForFinishingAnimation());
            animationCallback = null;
            animator.SetBool(parameter.ToString(), false);
            _countOfClips = countOfClips;
            parameter = newParameter;
            animator.SetBool(parameter.ToString(), true);
            StartCoroutine(WaitForFinishingAnimation());
            return this;
        }

        private IEnumerator WaitForFinishingAnimation()
        {
            float _resultingLength = 0f;
            _cureentClipLength = 0f;
            yield return new WaitUntil(() => _cureentClipLength > 0);
            if (animationCallback != null)
            {
                _resultingLength = _cureentClipLength * _countOfClips;
                yield return new WaitForSeconds(_resultingLength);
                m_characterAnimatorController.OnComplete(animationCallback);
                InvokeCallback();
            }
        }
    }

    public enum AnimatorParameters
    {
        None = 0,
        Idle = 1,
        Walk = 2,
    }
}
