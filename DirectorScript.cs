using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DirectorScript : MonoBehaviour
{
    [System.Serializable]
    public class AnimationClipInfo
    {
        public Animator animator;
        public AnimationClip animationClip;
        public float startTime;
    }

    [System.Serializable]
    public class CameraInfo
    {
        public Camera camera;
        public float startTime;
        public float duration;
    }

    public enum TransitionType { Cut, JumpCut, FadeInOut }

    [System.Serializable]
    public class TransitionInfo
    {
        public TransitionType transitionType;
        public float duration; // Only for fade in/out
    }

    [System.Serializable]
    public class CameraMovement
    {
        public Camera camera;
        public Vector3 targetPosition;
        public Quaternion targetRotation;
        public float startTime;
        public float duration;
    }

    public List<AnimationClipInfo> animations = new List<AnimationClipInfo>();
    public List<CameraInfo> cameras = new List<CameraInfo>();
    public List<TransitionInfo> transitions = new List<TransitionInfo>();
    public List<CameraMovement> cameraMovements = new List<CameraMovement>();

    // Variables pour suivre l'état actuel
    public string currentAnimation = "None";
    public string currentCamera = "None";
    public string currentMovement = "None";
    public float currentTime = 0f;

    private void Start()
    {
        if (animations != null && animations.Count > 0)
        {
            StartCoroutine(HandleAnimations());
        }
        if (cameras != null && cameras.Count > 0)
        {
            StartCoroutine(HandleCameras());
        }
        if (cameraMovements != null && cameraMovements.Count > 0)
        {
            StartCoroutine(HandleCameraMovements());
        }
    }

    private IEnumerator HandleAnimations()
    {
        foreach (var anim in animations)
        {
            if (anim != null && anim.animator != null && anim.animationClip != null)
            {
                currentAnimation = anim.animationClip.name;
                currentTime = anim.startTime;
                yield return new WaitForSeconds(anim.startTime);
                anim.animator.Play(anim.animationClip.name);
                currentAnimation = "Playing: " + anim.animationClip.name;
            }
        }
        currentAnimation = "None";
    }

    private IEnumerator HandleCameras()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != null && cameras[i].camera != null)
            {
                currentCamera = cameras[i].camera.name;
                currentTime = cameras[i].startTime;
                yield return new WaitForSeconds(cameras[i].startTime);
                cameras[i].camera.enabled = true;

                if (i > 0 && cameras[i - 1] != null && cameras[i - 1].camera != null && transitions[i - 1] != null)
                {
                    var previousCamera = cameras[i - 1].camera;
                    var transition = transitions[i - 1];

                    switch (transition.transitionType)
                    {
                        case TransitionType.Cut:
                            previousCamera.enabled = false;
                            break;
                        case TransitionType.JumpCut:
                            yield return new WaitForSeconds(0.1f);
                            previousCamera.enabled = false;
                            break;
                        case TransitionType.FadeInOut:
                            var previousCameraFade = previousCamera.GetComponent<CameraFade>();
                            var currentCameraFade = cameras[i].camera.GetComponent<CameraFade>();
                            if (previousCameraFade != null && currentCameraFade != null)
                            {
                                yield return StartCoroutine(previousCameraFade.FadeOut(transition.duration / 2));
                                yield return StartCoroutine(currentCameraFade.FadeIn(transition.duration / 2));
                            }
                            previousCamera.enabled = false;
                            break;
                    }
                }

                yield return new WaitForSeconds(cameras[i].duration);
                currentCamera = "None";
                Debug.Log($"Deactivating camera {cameras[i].camera.name} after {cameras[i].duration} seconds");
                cameras[i].camera.enabled = false;
            }
        }
    }

    private IEnumerator HandleCameraMovements()
    {
        foreach (var move in cameraMovements)
        {
            if (move != null && move.camera != null)
            {
                currentMovement = move.camera.name;
                currentTime = move.startTime;
                yield return new WaitForSeconds(move.startTime);
                StartCoroutine(MoveCamera(move));
            }
        }
        currentMovement = "None";
    }

    private IEnumerator MoveCamera(CameraMovement move)
    {
        float elapsedTime = 0;
        Vector3 initialPosition = move.camera.transform.position;
        Quaternion initialRotation = move.camera.transform.rotation;

        while (elapsedTime < move.duration)
        {
            move.camera.transform.position = Vector3.Lerp(initialPosition, move.targetPosition, elapsedTime / move.duration);
            move.camera.transform.rotation = Quaternion.Lerp(initialRotation, move.targetRotation, elapsedTime / move.duration);
            elapsedTime += Time.deltaTime;
            currentTime = elapsedTime;
            yield return null;
        }

        move.camera.transform.position = move.targetPosition;
        move.camera.transform.rotation = move.targetRotation;
    }
}
