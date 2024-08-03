using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedAnimatorControllerChange : MonoBehaviour
{
    [System.Serializable]
    public class AnimationTrigger
    {
        public string triggerName;
        public float duration;
        public Vector3 direction; // Direction of movement
        public float speed; // Speed of movement
    }

    [System.Serializable]
    public class AnimatorControllerEvent
    {
        public float delay; // Temps avant que le changement de contr�leur commence
        public RuntimeAnimatorController animatorController; // Le contr�leur d'animation � appliquer
        public float duration; // Dur�e pendant laquelle ce contr�leur est actif
        public List<AnimationTrigger> animationSequence = new List<AnimationTrigger>();

        
        public bool isCompleted = false; // Indicateur si l'�v�nement est termin�
        
        public float startTime = 0.0f; // Temps de d�but de l'�v�nement
    }

    [Header("Animator Controller Events")]
    public List<AnimatorControllerEvent> animatorControllerEvents = new List<AnimatorControllerEvent>();

    private float elapsedTime = 0.0f;
    private Animator animator;
    private int currentEventIndex = -1;
    private int currentTriggerIndex = 0;
    private Coroutine currentCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Temps �coul� depuis le d�but
        elapsedTime += Time.deltaTime;

        for (int i = 0; i < animatorControllerEvents.Count; i++)
        {
            var controllerEvent = animatorControllerEvents[i];

            if (!controllerEvent.isCompleted)
            {
                // Commencer l'�v�nement apr�s le d�lai d�fini
                if (elapsedTime >= controllerEvent.delay && controllerEvent.startTime == 0.0f)
                {
                    controllerEvent.startTime = elapsedTime;
                    animator.runtimeAnimatorController = controllerEvent.animatorController;
                    currentEventIndex = i;

                    // D�marrer la s�quence d'animation
                    if (controllerEvent.animationSequence.Count > 0)
                    {
                        currentCoroutine = StartCoroutine(PlayAnimationSequence(controllerEvent.animationSequence));
                    }
                }

                // Marquer l'�v�nement comme compl�t� apr�s la dur�e d�finie
                if (controllerEvent.startTime != 0.0f && elapsedTime >= controllerEvent.startTime + controllerEvent.duration)
                {
                    controllerEvent.isCompleted = true;

                    // Arr�ter la s�quence d'animation
                    if (currentCoroutine != null)
                    {
                        StopCoroutine(currentCoroutine);
                    }

                    // Passer au prochain contr�leur si disponible
                    if (i + 1 < animatorControllerEvents.Count)
                    {
                        animatorControllerEvents[i + 1].delay = elapsedTime - controllerEvent.startTime;
                    }
                }
            }
        }
    }

    IEnumerator PlayAnimationSequence(List<AnimationTrigger> animationSequence)
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

    // Pour r�initialiser le script (optionnel)
    public void ResetAnimatorControllers()
    {
        elapsedTime = 0.0f;
        foreach (var controllerEvent in animatorControllerEvents)
        {
            controllerEvent.isCompleted = false;
            controllerEvent.startTime = 0.0f;
        }
        currentEventIndex = -1;
        currentTriggerIndex = 0;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }
}
