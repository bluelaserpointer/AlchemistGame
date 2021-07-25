using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class AnimationStopper : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    public bool IsPlaying => animator.GetBool("Play");
    public UnityEvent animationStopped;
    public void Stop()
    {
        animator.SetBool("Play", false);
    }
    public void Play()
    {
        animator.SetBool("Play", true);
    }
}
