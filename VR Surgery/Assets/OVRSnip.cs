
using System.Collections.Generic;
using UnityEngine;

public class OVRSnip : MonoBehaviour
{
    // Grip trigger thresholds for picking up objects, with some hysteresis.
    public float grabBegin = 0.55f;
    public float grabEnd = 0.35f;
    public GameObject slicerComponent;
    public Animator animator;
    // Demonstrates parenting the held object to the hand's transform when grabbed.
    // When false, the grabbed object is moved every FixedUpdate using MovePosition.
    // Note that MovePosition is required for proper physics simulation. If you set this to true, you can
    // easily observe broken physics simulation by, for example, moving the bottom cube of a stacked
    // tower and noting a complete loss of friction.
    [SerializeField]
    protected bool m_parentHeldObject = false;

    // If true, this script will move the hand to the transform specified by m_parentTransform, using MovePosition in
    // Update. This allows correct physics behavior, at the cost of some latency. In this usage scenario, you
    // should NOT parent the hand to the hand anchor.
    // (If m_moveHandPosition is false, this script will NOT update the game object's position.
    // The hand gameObject can simply be attached to the hand anchor, which updates position in LateUpdate,
    // gaining us a few ms of reduced latency.)
    [SerializeField]
    protected bool m_moveHandPosition = false;

    // Child/attached transforms of the grabber, indicating where to snap held objects to (if you snap them).
    // Also used for ranking grab targets in case of multiple candidates.
    [SerializeField]
    protected Transform m_gripTransform = null;

    // Should be OVRInput.Controller.LTouch or OVRInput.Controller.RTouch.
    [SerializeField]
    protected OVRInput.Controller m_controller;

    // You can set this explicitly in the inspector if you're using m_moveHandPosition.
    // Otherwise, you should typically leave this null and simply parent the hand to the hand anchor
    // in your scene, using Unity's inspector.
    [SerializeField]
    protected Transform m_parentTransform;

    [SerializeField]
    protected GameObject m_player;

    protected bool m_grabVolumeEnabled = true;
    protected Vector3 m_lastPos;
    protected Quaternion m_lastRot;
    protected Quaternion m_anchorOffsetRotation;
    protected Vector3 m_anchorOffsetPosition;
    protected float m_prevFlex;
    protected OVRGrabbable m_grabbedObj = null;
    protected Vector3 m_grabbedObjectPosOff;
    protected Quaternion m_grabbedObjectRotOff;
    protected Dictionary<OVRGrabbable, int> m_grabCandidates = new Dictionary<OVRGrabbable, int>();
    protected bool m_operatingWithoutOVRCameraRig = true;

    /// <summary>
    /// The currently grabbed object.
    /// </summary>
    public OVRGrabbable grabbedObject
    {
        get { return m_grabbedObj; }
    }

    public void ForceRelease(OVRGrabbable grabbable)
    {
        bool canRelease = (
            (m_grabbedObj != null) &&
            (m_grabbedObj == grabbable)
        );
        if (canRelease)
        {
            GrabEnd();
        }
    }

    protected virtual void Awake()
    {
        m_anchorOffsetPosition = transform.localPosition;
        m_anchorOffsetRotation = transform.localRotation;

        if (!m_moveHandPosition)
        {
            // If we are being used with an OVRCameraRig, let it drive input updates, which may come from Update or FixedUpdate.
            OVRCameraRig rig = transform.GetComponentInParent<OVRCameraRig>();
            if (rig != null)
            {
                rig.UpdatedAnchors += (r) => { OnUpdatedAnchors(); };
                m_operatingWithoutOVRCameraRig = false;
            }
        }
    }

    protected virtual void Start()
    {
        m_lastPos = transform.position;
        m_lastRot = transform.rotation;
        if (m_parentTransform == null)
        {
            m_parentTransform = gameObject.transform;
        }

        // We're going to setup the player collision to ignore the hand collision.
        SetPlayerIgnoreCollision(gameObject, true);
    }

    // Using Update instead of FixedUpdate. Doing this in FixedUpdate causes visible judder even with
    // somewhat high tick rates, because variable numbers of ticks per frame will give hand poses of
    // varying recency. We want a single hand pose sampled at the same time each frame.
    // Note that this can lead to its own side effects. For example, if m_parentHeldObject is false, the
    // grabbed objects will be moved with MovePosition. If this is called in Update while the physics
    // tick rate is dramatically different from the application frame rate, other objects touched by
    // the held object will see an incorrect velocity (because the move will occur over the time of the
    // physics tick, not the render tick), and will respond to the incorrect velocity with potentially
    // visible artifacts.
    virtual public void Update()
    {
        if (m_operatingWithoutOVRCameraRig)
        {
            OnUpdatedAnchors();
        }
    }

    // Hands follow the touch anchors by calling MovePosition each frame to reach the anchor.
    // This is done instead of parenting to achieve workable physics. If you don't require physics on
    // your hands or held objects, you may wish to switch to parenting.
    void OnUpdatedAnchors()
    {
        Vector3 destPos = m_parentTransform.TransformPoint(m_anchorOffsetPosition);
        Quaternion destRot = m_parentTransform.rotation * m_anchorOffsetRotation;

        if (m_moveHandPosition)
        {
            GetComponent<Rigidbody>().MovePosition(destPos);
            GetComponent<Rigidbody>().MoveRotation(destRot);
        }

        m_lastPos = transform.position;
        m_lastRot = transform.rotation;

        float prevFlex = m_prevFlex;
        // Update values from inputs
        m_prevFlex = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, m_controller);

        CheckForGrabOrRelease(prevFlex);
    }

    protected void CheckForGrabOrRelease(float prevFlex)
    {
        if ((m_prevFlex >= grabBegin) && (prevFlex < grabBegin) || Input.GetKeyDown(KeyCode.A))
        {
            GrabBegin();
        }
        else if ((m_prevFlex <= grabEnd) && (prevFlex > grabEnd) || Input.GetKeyUp(KeyCode.A))
        {
            GrabEnd();
        }
    }

    protected void GrabBegin()
    {
        slicerComponent.SetActive(true);
        animator.SetBool("cut", true);
    }

    protected void GrabEnd()
    {
        slicerComponent.SetActive(false);
        animator.SetBool("cut", false);
    }

    protected void SetPlayerIgnoreCollision(GameObject grabbable, bool ignore)
    {
        if (m_player != null)
        {
            Collider[] playerColliders = m_player.GetComponentsInChildren<Collider>();
            foreach (Collider pc in playerColliders)
            {
                Collider[] colliders = grabbable.GetComponentsInChildren<Collider>();
                foreach (Collider c in colliders)
                {
                    if (!c.isTrigger && !pc.isTrigger)
                        Physics.IgnoreCollision(c, pc, ignore);
                }
            }
        }
    }

}
