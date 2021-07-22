using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class AnimationStopper : MonoBehaviour
{
    Animator animator;
    public bool IsPlaying => animator.GetBool("Play");
    public UnityEvent animationStopped;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Stop()
    {
        animator.SetBool("Play", false);
    }
    public void Play()
    {
        animator.SetBool("Play", true);
    }
}
