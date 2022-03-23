using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityIC
{
    //The class registers the hit of the ray on the collider. Ignoring the specified layers and Canvas.
    public class RaycastOutput : InteractiveObject
    {
        private static RaycastOutput s_instance = null;
        [SerializeField]
        private LayerMask ignoreLayerMask = new LayerMask();

        [Header("Igore RaycastHit on next UICanvas"), SerializeField]
        private List<GraphicRaycaster> _ignoreCanvas = new List<GraphicRaycaster>();

        private PointerEventData m_PointerEventData = null;

        [Space(20), SerializeField]
        private EventSystem m_EventSystem = null;

        public static event Action<Vector3, Vector3> OnSomewerePointerDown = null;
        public static event Action<Vector3, Vector3> OnSomewerePointerUp = null;

        public static event Action<Vector3, Vector3> OnSomewereTouch = null;
        private bool _buttonDown = true;

        private void Awake()
        {
            s_instance = this;
            SetInteractive(true);
        }

        private void Update()
        {
            if (!isInteractive)
                return;
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0) && _buttonDown)
            {
                var target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                OnSomewerePointerDown?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(target.x, target.y, 100f));
                _buttonDown = false;
            }
            else
            if (Input.GetMouseButtonUp(0) && !_buttonDown)
            {
                var target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
                OnSomewerePointerUp?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(target.x, target.y, 100f));
                _buttonDown = true;
            }
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
                    bool isTouching = Input.touchCount > 0;
                    if (isTouching && !_previousWasTouching)
                    {
                        Touch touch = Input.GetTouch(0);
                        if (touch.phase == TouchPhase.Moved)
                            return;

                        var target = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                        OnSomewereTouch?.Invoke(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector3(target.x, target.y, 100f));
                    }
                    _previousWasTouching = isTouching;
#endif
        }

        public static void OnClick<T>(Vector3 from, Vector3 to, Action<T> onClickAction = null, Action onFailedClick = null)
        {
            RaycastHit2D hit = Physics2D.Raycast(from, to - from, 100, ~(s_instance.ignoreLayerMask.value));
            if (hit.collider == null)
                return;
            if (s_instance.HitUI())
                return;
            if (hit.collider.TryGetComponent(out T comp))
            {
                onClickAction?.Invoke(comp);
            }
            else
            {
                onFailedClick?.Invoke();
            }
        }


        private bool HitUI()
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);

#if UNITY_EDITOR
            m_PointerEventData.position = Input.mousePosition;
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            m_PointerEventData.position = Input.GetTouch(0).position;
#endif

            List<RaycastResult> results = new List<RaycastResult>();
            _ignoreCanvas.ForEach((item) => item.Raycast(m_PointerEventData, results));

            return results.Count > 0;
        }
    }
}
