using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    private Animator animator;
    public List<string> animations = new List<string>();
    public float prevAnimTime = 0f;
    private int lastAnimPlayed = 0;

    void Awake()
    {
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();

        //Load animations
        animations.Clear();
        foreach(AnimationClip ac in animator.runtimeAnimatorController.animationClips)
            animations.Add(ac.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && lastAnimPlayed == 0) //Left Click
        {
            PlayAnim(Random.Range(1, animations.Count));
            prevAnimTime = 0;
        }
        else
        {
            float animLoop = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f);
            if (animLoop < prevAnimTime)
            {
                PlayAnim(0);
            }
            prevAnimTime = animLoop;
        }
    }
    void PlayAnim(int index)
    {
        string anim = animations[index];
        animator.Play(anim);
        Debug.Log($"Playing {anim} animation");
        lastAnimPlayed = index;
    }
}
