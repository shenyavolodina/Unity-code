using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIC
{
         public class InteractiveObject : MonoBehaviour
        {
            public bool isInteractive = true;

            public virtual void OnEnable()
            {
                GlobalSceneInteractionController.OnEnableSceneInteraction += SetInteractiveTrue;
                GlobalSceneInteractionController.OnDisableSceneInteraction += SetInteractiveFalse;
            }

            public virtual void OnDisable()
            {
                GlobalSceneInteractionController.OnEnableSceneInteraction -= SetInteractiveTrue;
                GlobalSceneInteractionController.OnDisableSceneInteraction -= SetInteractiveFalse;
            }

            public void SetInteractive(bool flag)
            {
                if (GlobalSceneInteractionController.isGlobalLockInteractive)
                    return;
                isInteractive = flag;
            }

            public void SetInteractiveTrue()
            {
                if (GlobalSceneInteractionController.isGlobalLockInteractive)
                    return;
                isInteractive = true;
            }

            public void SetInteractiveFalse()
            {
                if (GlobalSceneInteractionController.isGlobalLockInteractive)
                    return;
                isInteractive = false;
            }
        }
}
