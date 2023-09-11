using JetBrains.Annotations;
using TMPro.Examples;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Animator Animator;
    [SerializeField] public Collider AnimatorCollider;
    [SerializeField] private Transform RagdollRoot;
    [SerializeField] private Rigidbody[] Rigidbodies;

    [Header("ATTRIBUTES")]
    [SerializeField] public bool StartRagdoll = false;

    private bool isRagdoll;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;
    private RagdollAnimator ragdollAnimator;

    [Header("Properties")]
    public float massMultiplier = 1.0f;
    public float dragMultiplier = 0.5f;
    public float angularDragMultiplier = 0.5f;


    private void Awake()
    {
        SetRagdollStiffness();
        isRagdoll = !StartRagdoll;
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        ragdollAnimator = GetComponent<RagdollAnimator>();
        if (StartRagdoll) EnableRagdoll();
        else EnableAnimator();


    }

    public void LateUpdate()
    {
        if (StartRagdoll && !isRagdoll)
        {
            EnableRagdoll();
        }
        else if (!StartRagdoll && isRagdoll)
        {
            EnableAnimator();
        }
    }
    public void EnableRagdoll()
    {
        isRagdoll = true;
        StartRagdoll = true;
        Animator.enabled = false;
            foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            Vector3 bounce = new Vector3(0f, 2f, 0f);
            rigidbody.AddForce(0f, 2f, 0f, ForceMode.Impulse);
        }
        AnimatorCollider.enabled = false;
        if (ragdollAnimator) ragdollAnimator._currentState = RagdollAnimator.RagdollState.Ragdoll;
    }

    public void EnableAnimator()
    {
        isRagdoll = false;
        StartRagdoll = false;
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }

        foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
        AnimatorCollider.enabled = true;
    }

    private void SetRagdollStiffness()
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.mass *= massMultiplier;
            rigidbody.drag *= dragMultiplier;
            rigidbody.angularDrag *= angularDragMultiplier;
        }
    }
}
