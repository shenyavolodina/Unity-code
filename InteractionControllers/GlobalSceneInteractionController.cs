using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIC
{
    public class GlobalSceneInteractionController : MonoBehaviour
    {
        public InteractiveObject[] interactiveObjects = null;

        public static bool isGlobalLockInteractive = false;

        public void DisableSceneInteraction()
        {

            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                interactiveObjects[i].SetInteractive(false);
            }
            isGlobalLockInteractive = true;
        }

        public void EnableSceneInteraction()
        {
            isGlobalLockInteractive = false;
            for (int i = 0; i < interactiveObjects.Length; i++)
            {
                interactiveObjects[i].SetInteractive(true);
            }
        }
    }
}