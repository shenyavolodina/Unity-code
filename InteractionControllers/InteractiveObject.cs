using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIC
{
        public class InteractiveObject : MonoBehaviour
        {
            public bool isInteractive = true;
            public bool isGlobalLockInteractive = false;

            public void SetInteractive(bool flag)
            {
                if (GlobalSceneInteractionController.isGlobalLockInteractive)
                    return;
                isInteractive = flag;
            }
        }
}