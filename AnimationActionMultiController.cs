using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationState
{
    public string stateName;
    public float duration;
}

public class AnimationActionMultiController : MonoBehaviour
{
    public Animator animator;
    public List<string> stateNames = new List<string>();
    public List<AnimationState> animationSequence = new List<AnimationState>();
    public int selectedStateIndex = 0;

    private float elapsedTime = 0f;
    private int currentStateIndex = 0;

    void OnValidate()
    {
        UpdateStateNames();
    }

    void UpdateStateNames()
    {
        stateNames.Clear();
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            foreach (var state in animator.runtimeAnimatorController.animationClips)
            {
                stateNames.Add(state.name);
            }
        }

        // Update available states in animationSequence
        foreach (var animState in animationSequence)
        {
            if (!stateNames.Contains(animState.stateName))
            {
                animState.stateName = "";
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        if (animationSequence.Count > 0 && animator != null)
        {
            StartCoroutine(PlayAnimationSequence());
        }
    }

    IEnumerator PlayAnimationSequence()
    {
        while (true)
        {
            if (animationSequence.Count == 0) yield break;

            AnimationState currentState = animationSequence[currentStateIndex];
            if (!string.IsNullOrEmpty(currentState.stateName))
            {
                animator.Play(currentState.stateName);
            }

            yield return new WaitForSeconds(currentState.duration);

            currentStateIndex = (currentStateIndex + 1) % animationSequence.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("isWalking", true);
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Elapsed Time: " + elapsedTime.ToString("F2") + " seconds");
    }
}
