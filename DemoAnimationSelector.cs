using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AnimationStep
{
    public int index;
    public float startTime;
    public float duration;
}

public class DemoAnimationSelector : MonoBehaviour
{
    public Animator m_Animator = null;

    [SerializeField]
    public List<AnimationStep> animationSteps = new List<AnimationStep>();

    private float elapsedTime = 0f;

    void Start()
    {
        // Initialisation de l'Animator
        m_Animator = GetComponent<Animator>();

        // Démarre la coroutine pour gérer les animations
        StartCoroutine(PlayAnimations());
    }

    void Update()
    {
        // Met à jour le temps écoulé
        elapsedTime += Time.deltaTime;
    }

    IEnumerator PlayAnimations()
    {
        foreach (var step in animationSteps)
        {
            // Attends jusqu'au moment de début de l'animation
            yield return new WaitForSeconds(step.startTime - elapsedTime);

            // Change l'animation
            SwitchAnimation(step.index);

            // Attends la durée de l'animation
            yield return new WaitForSeconds(step.duration);
        }
    }

    public void SwitchAnimation(int index)
    {
        if (!m_Animator)
            m_Animator = GetComponent<Animator>();

        if (m_Animator)
            m_Animator.SetInteger("Mode", index);
    }
}
