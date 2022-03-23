using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIC
{
    public delegate void AnimationCallback();
    public static class AnimationClipsController
    {
        public static void OnComplete(this CharacterAnimatorController characterAnimatorController, AnimationCallback action = null) 
        {
            characterAnimatorController.animationCallback = action;
        }
        
    }
}