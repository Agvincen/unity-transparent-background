using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimation : MonoBehaviour
{
    private Animator animator;
    public List<string> animations = new List<string>();

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
        if (Input.GetMouseButtonDown(0)) //Left Click
        {
            string anim = animations[Random.Range(0, animations.Count)];
            animator.Play(anim);
            Debug.Log($"Playing {anim} animation");
        }
    }
}
