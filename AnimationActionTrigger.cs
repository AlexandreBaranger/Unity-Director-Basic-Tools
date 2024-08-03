using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationTrigger
{
    public string triggerName;
    public float duration;
    public Vector3 direction; // Direction of movement
    public float speed; // Speed of movement
}

public class AnimationActionTrigger : MonoBehaviour
{
    public Animator animator;
    public List<string> triggerNames = new List<string>();
    public List<AnimationTrigger> animationSequence = new List<AnimationTrigger>();
    public int selectedTriggerIndex = 0;

    private float elapsedTime = 0f;
    private int currentTriggerIndex = 0;

    void OnValidate()
    {
        UpdateTriggerNames();
    }

    void UpdateTriggerNames()
    {
        triggerNames.Clear();
        if (animator != null)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    triggerNames.Add(parameter.name);
                }
            }
        }

        // Update available triggers in animationSequence
        foreach (var animTrigger in animationSequence)
        {
            if (!triggerNames.Contains(animTrigger.triggerName))
            {
                animTrigger.triggerName = "";
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

            AnimationTrigger currentTrigger = animationSequence[currentTriggerIndex];
            if (!string.IsNullOrEmpty(currentTrigger.triggerName))
            {
                animator.SetTrigger(currentTrigger.triggerName);
            }

            // Move the GameObject in the specified direction
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + currentTrigger.direction.normalized * currentTrigger.speed * currentTrigger.duration;
            float elapsedTime = 0f;

            while (elapsedTime < currentTrigger.duration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / currentTrigger.duration);
                transform.rotation = Quaternion.LookRotation(currentTrigger.direction);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentTriggerIndex = (currentTriggerIndex + 1) % animationSequence.Count;
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("walk");
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Elapsed Time: " + elapsedTime.ToString("F2") + " seconds");
    }
}
