using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class OnAnimationEnd : MonoBehaviour
{
    UnityEvent AnimationFinished;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartAnimation(string AnimationName)
    {
        anim.Play(AnimationName);
        StartCoroutine(AnimationProgress(AnimationName));
    }

    IEnumerator AnimationProgress(string AnimationName)
    {
        bool animFin = false;
        while (animFin == false)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
            {
                animFin = true;
                AnimationFinished.Invoke();
            }
            yield return new WaitForEndOfFrame();
        }
    }

}
