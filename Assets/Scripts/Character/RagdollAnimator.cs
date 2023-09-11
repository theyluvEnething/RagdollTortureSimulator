using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Debug = System.Diagnostics.Debug;

public class RagdollAnimator : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Transform IdlePoint;
    [SerializeField] private string _StandUpClipName;
    [SerializeField] private float _timeToResetBones;

    [Header("DEBUG")]
    [SerializeField] public RagdollState _currentState;
    [SerializeField] private float _timeSinceKnocked;
    public enum RagdollState
    {
        Idle,
        Ragdoll,
        StandingUp,
        Walking,
        ResettingBones
    }
    private class BoneTransform
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
    private BoneTransform[] _standUpBoneTransform;
    private BoneTransform[] _ragdollBoneTransform;
    private Transform[] _bones;
    private Transform _hipsBone;
    private Transform _head;
    private float _elapsedResetBonesTimes;
    private Rigidbody[] _rbs;

    private Collider animatorCollider;
    private Animator animator;
    private RagdollEnabler ragdollEnabler;
    private bool isBored = false;
    private int groundLayerMask;
    const float minWaitTime = 7f, maxWaitTime = 15f;

    void Start()
    {
        animator = GetComponent<Animator>();
        animatorCollider = GetComponent<Collider>();
        StartCoroutine(RecursiveRandomWait());
        ragdollEnabler = GetComponent<RagdollEnabler>();
        _hipsBone = animator.GetBoneTransform(HumanBodyBones.Hips);
        _head = animator.GetBoneTransform(HumanBodyBones.Head);
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        _rbs = transform.GetComponentsInChildren<Rigidbody>();
        _bones = _hipsBone.GetComponentsInChildren<Transform>();
        _standUpBoneTransform = new BoneTransform[_bones.Length];
        _ragdollBoneTransform = new BoneTransform[_bones.Length];
        for (int i= 0; i < _bones.Length; i++)
        {
            _standUpBoneTransform[i] = new BoneTransform();
            _ragdollBoneTransform[i] = new BoneTransform();
        }
        PopulateAnimationStartBoneTransforms(_StandUpClipName, _standUpBoneTransform);

        ragdollEnabler.EnableAnimator();
    }
    void Update()
    {
        switch (_currentState)
        {
            case RagdollState.Idle:
                break;
            case RagdollState.Ragdoll:
                RagdollBehaviour();
                break;
            case RagdollState.ResettingBones:
                ResettingBonesBehaviour();
                break;
            case RagdollState.StandingUp:
                StandingUpBehaviour();
                break;
            case RagdollState.Walking:
                WalkingBehaviour();
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Background") && !collision.transform.CompareTag("Untagged"))
        {
            PushRagdoll();

            if (collision.rigidbody)
            {
                foreach (Rigidbody _rb in _rbs)
                    _rb.AddForce(collision.rigidbody.velocity * 5f, ForceMode.Force);
            }
        }
    }
    public void PushRagdoll()
    {
        _timeSinceKnocked = 0f;
        ragdollEnabler.EnableRagdoll();
        _currentState = RagdollAnimator.RagdollState.Ragdoll;
    }
    private void RagdollBehaviour()
    {
        if (_timeSinceKnocked >= 2.5f)
        {
            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransforms(_ragdollBoneTransform);

            _currentState = RagdollState.ResettingBones;
            _elapsedResetBonesTimes = 0f;
        }

        if (Physics.Raycast(_head.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayerMask))
        {
            if (hit.distance <= 1.5f) {
                _timeSinceKnocked += Time.deltaTime;
            } else {
                _timeSinceKnocked = 0f;   
            }
        }
    }
    private void AlignPositionToHips()
    {
        Vector3 originalHitPosition = _hipsBone.position;
        transform.position = _hipsBone.position;

        Vector3 positionOffset = _standUpBoneTransform[0].Position;
        positionOffset.y = 0.5f;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

        _hipsBone.position = originalHitPosition;
    }
    private void AlignRotationToHips()
    {
        Vector3 originalHitPosition = _hipsBone.position;
        Quaternion originalHitRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up * -1f;
        desiredDirection.y = 0f;
        desiredDirection.x += 180f;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHitPosition;
        _hipsBone.rotation = originalHitRotation;
    }
    private void PopulateBoneTransforms(BoneTransform[] boneTransforms)
    {
        for (int i = 0; i < _bones.Length; i++)
        {
            boneTransforms[i].Position = _bones[i].localPosition;
            boneTransforms[i].Rotation = _bones[i].localRotation;
        }
    }
    private void PopulateAnimationStartBoneTransforms(string clipName, BoneTransform[] boneTransform)
    {
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                clip.SampleAnimation(gameObject, 0);
                PopulateBoneTransforms(_standUpBoneTransform);
                break;
            }
        }
    }
    private IEnumerator RecursiveRandomWait()
    {
        float waitTime = Random.Range(minWaitTime, maxWaitTime);

        // Wait for the random time
        yield return new WaitForSeconds(waitTime);

        isBored = true;
        SetBoredAnimation(isBored);

        yield return new WaitForSeconds(2f);
        isBored = false;
        SetBoredAnimation(isBored);

        StartCoroutine(RecursiveRandomWait());
    }
    private void ResettingBonesBehaviour()
    {
        _timeSinceKnocked = 0f;
        _elapsedResetBonesTimes += Time.deltaTime;
        float elapsedPercentage = _elapsedResetBonesTimes / _timeToResetBones;

        for (int i = 0; i < _bones.Length; i++)
        {
            _bones[i].localPosition = Vector3.Lerp(
                _ragdollBoneTransform[i].Position,
                _standUpBoneTransform[i].Position,
                elapsedPercentage);

            _bones[i].localRotation = Quaternion.Lerp(
                _ragdollBoneTransform[i].Rotation,
                _standUpBoneTransform[i].Rotation,
                elapsedPercentage);
        }
        if (elapsedPercentage >= 1)
        {
            _currentState = RagdollState.StandingUp;
            ragdollEnabler.EnableAnimator();
            animator.Play("Stand Up");
        }
    }
    private void StandingUpBehaviour()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == _StandUpClipName) {
            //animatorCollider.enabled = false;
        }
        else
        {
            animatorCollider.enabled = true;
            _currentState = RagdollState.Walking;
        }
    }
    private void WalkingBehaviour()
    {
        Vector2 TransformSnapshot = new Vector2(transform.position.x, transform.position.z);
        Vector2 IdlePointSnapshot = new Vector2(IdlePoint.position.x, IdlePoint.position.z);
        if (Vector2.Distance(TransformSnapshot, IdlePointSnapshot) > 0.5f)
        {
            Vector3 lookAtDirection = IdlePoint.position - transform.position;
            lookAtDirection.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookAtDirection);
            transform.position = Vector3.Lerp(transform.position, IdlePoint.position, 0.01f);
        } else
        {
            StartCoroutine(RotateCharacterRoutine(IdlePoint));
            SetReachedDestination(true);
            _currentState = RagdollState.Idle;
            Debug.WriteLine("Reached Destination");
            StartCoroutine(ResetReachedDestination());
        }
    }
    private IEnumerator RotateCharacterRoutine(Transform targetPoint)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, IdlePoint.localRotation, 0.1f);

        Vector3 directionToTarget = targetPoint.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        float angularDifference = Quaternion.Angle(transform.rotation, targetRotation);

        // Compare the angular difference with the similarity threshold
        if (angularDifference < 1f)
        {
            Debug.WriteLine("Finished turning");
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(RotateCharacterRoutine(targetPoint));
    }
    private IEnumerator ResetReachedDestination()
    {
        yield return new WaitForSeconds(1f);
        SetReachedDestination(false);
    }
    void SetBoredAnimation(bool bored)
    {
        isBored = bored;
        animator.SetBool("isBored", bored);
    }
    void SetReachedDestination(bool reachedDestination)
    {
        animator.SetBool("reachedDestination", reachedDestination);
    }
}
