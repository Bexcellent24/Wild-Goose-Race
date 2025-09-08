using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIRacerAnimator : MonoBehaviour
{
    [SerializeField ]private Animator animator;
    [SerializeField] private NavMeshAgent agent; 
    [SerializeField] private float walkThreshold = 5f;
    [SerializeField] private float jogThreshold = 10f;
    [SerializeField] private float checkInterval = 0.5f;
    
    private Coroutine runningAnimationCoroutine;
    private string currentAnimation = "";

    private void Start()
    {
        ChangeAnimation(this.gameObject, GooseStateType.Running);
    }
    private void OnEnable()
    {
        GooseStateTrigger.OnAIRacerTrigger += ChangeAnimation;
    }

    private void OnDisable()
    {
        GooseStateTrigger.OnAIRacerTrigger -= ChangeAnimation;
    }

    private void ChangeAnimation(GameObject racer, GooseStateType stateType)
    {
        
        if (racer != this.gameObject) return;

        // Stop any ongoing speed-based animation logic
        if (runningAnimationCoroutine != null)
        {
            StopCoroutine(runningAnimationCoroutine);
            runningAnimationCoroutine = null;
        }

        switch (stateType)
        {
            case GooseStateType.Running:
                runningAnimationCoroutine = StartCoroutine(UpdateRunningAnimation());
                break;

            case GooseStateType.Swimming:
                PlayAnimation("Swimming");
                break;

            case GooseStateType.Flying:
                PlayAnimation("Flying");
                break;

            default:
                PlayAnimation("IdleStill");
                break;
        }
    }
    
    private void PlayAnimation(string name)
    {
        if (currentAnimation == name) return;
        currentAnimation = name;
        animator.Play(name);
    }
    
    private IEnumerator UpdateRunningAnimation()
    {
        while (true)
        {
            float speed = agent.velocity.magnitude;
            string animToPlay;
            if (speed < walkThreshold)
            {
                animToPlay = "Walking";
            }
            else if (speed < jogThreshold)
            {
                animToPlay = "Jogging";
            }
            else
            {
                animToPlay = "Running";
            }
            PlayAnimation(animToPlay);

            yield return new WaitForSeconds(checkInterval);
        }
    }

}