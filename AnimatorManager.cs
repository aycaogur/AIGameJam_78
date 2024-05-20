using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;
    int horizontal;
    int vertical;
    
    public delegate void AnimationEnd(string animationName);
    public event AnimationEnd OnAnimationEnd;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public void PlayTargetAnimation(string targetAnimation,bool isInteracting)
    {
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnimation, 0.2f);
        StartCoroutine(WaitForAnimationToEnd(targetAnimation));
    }

    private IEnumerator WaitForAnimationToEnd(string animationName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        while (stateInfo.normalizedTime < 1.0f)
        {
            yield return null;
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        }

        OnAnimationEnd?.Invoke(animationName);
    }

    public void UpdateAnimatorValues(float horizontalMovement,float verticalMovement,bool isFastRunning)
    {
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if(horizontalMovement>0 && horizontalMovement<0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if(horizontalMovement> 0.55f)
        {
            snappedHorizontal = 1;
        }
        else if(horizontalMovement<0&&horizontalMovement>-0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if(horizontalMovement<-0.55f)
        {
            snappedHorizontal = -1;
        }
        else
        {
            snappedHorizontal = 0;
        }
        #endregion
        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 0.55f)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }
        #endregion

        if (isFastRunning)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }

        anim.SetFloat(horizontal,snappedHorizontal,0.1f,Time.deltaTime);
        anim.SetFloat(vertical,snappedVertical,0.1f,Time.deltaTime);
    }
    private void OnAnimatorMove()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
        {
            OnAnimationEnd?.Invoke(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        }
    }
}
