using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIC
{
     public class GlobalSceneInteractionController : MonoBehaviour
        {
            public static bool isGlobalLockInteractive = false;

            public static event Action OnEnableSceneInteraction = null;
            public static event Action OnDisableSceneInteraction = null;

            public void DisableSceneInteraction()
            {
                OnDisableSceneInteraction?.Invoke();
                isGlobalLockInteractive = true;
            }

            public void EnableSceneInteraction()
            {
                isGlobalLockInteractive = false;
                OnEnableSceneInteraction?.Invoke();
            }
        }
}
