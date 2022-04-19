using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlekGames.Systems
{
    [RequireComponent(typeof(Rigidbody))]
    public class hoverCraft : MonoBehaviour
    {

        #region values
        private enum InputT { axes, other };

        private enum liftType { world, local};

        [Tooltip("what input should the script use? axes - use axes other - asighn input from other script by function parseInput")]
        [SerializeField] private InputT inputType;

        [Tooltip("world - Vector3.up ; local - transform.up")]
        [SerializeField] private liftType liftTypeSpace;

        [Tooltip("center of the vehicle")]
        [SerializeField] private Transform center;

        [Header("forward forces")]

        [Tooltip("forward of the vehicle")]
        [SerializeField] private Transform forwardVector;

        [Tooltip("speed of vehicle that is applyied forward when going forward")]
        [SerializeField] private float forwardSpeed = 5000;

        [Tooltip("speed of vehicle that is applyied backwards when going backwards")]
        [SerializeField] private float backwardsSpeed = 2000;

        [Tooltip("the maxinum velocity.magditud for the hoovercraft to apply forces forward/backwards")]
        [SerializeField] private float maxSpeed = 20;

        [Tooltip("forceMode of forces forward/backwards")]
        [SerializeField] private ForceMode forwardForceMode = ForceMode.Acceleration;

        [Header("angular forces")]

        [Tooltip("torque applyied to rotate the veicle")]
        [SerializeField] private float angularSpeed = 750;

        [Tooltip("the maxinum angularVelocity.magditud for the hoovercraft to apply Torque left/right")]
        [SerializeField] private float maxAngularSpeed = 12;

        [Tooltip("forceMode of Torques left/right")]
        [SerializeField] private ForceMode angularForceMode = ForceMode.Acceleration;


        [Header("main ground detection & actions")]

        [Tooltip("distance that the veicle will detect the ground from groundDetector")]
        [SerializeField] private float groundDetectionDistance = 2.4f;

        [Tooltip("a point from which the ground will be detected")]
        [SerializeField] private Transform groundDetector;

        [Tooltip("the layer that the ground  is on")]
        [SerializeField] private LayerMask groundLayer;

        [Tooltip("desired height for the hoovercraft to be on (will be a little lover)")]
        [SerializeField] private float hoverHeight = 1.8f;

        [Header("max slopes")]

        [Tooltip("the maximum normal diffrence accepable to float with out moving sideways")]
        [SerializeField] [Range(0.1f, 180)] private float maxNormalAngle = 40;

        [Tooltip("force used to push veicle of hill with slope over maxNormal angle. Keep in mind that tis force is checked and possibly added foreach hoverPoint")]
        [SerializeField] private float normalFixForce = 5000;

        [Tooltip("ForceMode used for normalFixForce")]
        [SerializeField] private ForceMode normalFixForceMode = ForceMode.Force;

        [Header("additional forces")]

        [Tooltip("the force that is applyied for the vehicle when it is grounded up.")]
        [SerializeField] private float additionalGroundedForceUp = 3000;

        [Tooltip("the force that is applyied for the vehicle when it is not grounded up")]
        [SerializeField] private float additionalUnGroundedForceUp = -1000;

        [Header("Jumping")]

        [Tooltip("the force of jump")]
        [SerializeField] private float jumpForce = 10000;

        [Tooltip("forceMode of the jump")]
        [SerializeField] private ForceMode jumpForceMode = ForceMode.Acceleration;
        [Space]
        [SerializeField] private flythingie[] hoverPoints;

        private Vector3 Thrust;
        private bool grounded;

        private Vector2 inputMovement;
        private bool inputJump;

        private Rigidbody rb;

        #endregion

        #region updates

        void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>(); //can be setup to not be null using rigidbodySetup function
                rb.centerOfMass = center.localPosition;
            }

        }

        private void Update()
        {
            gatherInput();
            calculateThrusts();
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            checkIfGrounded();
            addForces();
            hover();
        }

        #endregion

        #region values update

        private void gatherInput()
        {
            if (inputType == InputT.axes)
            {
                inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                inputJump = Input.GetKey(KeyCode.Space);
            }
        }

        private void calculateThrusts() //done in update, couse there is no point in doing it in fixedUpdate
        {
            Thrust = Vector3.zero;

            if (inputMovement.y > 0)
                Thrust.z = inputMovement.y * forwardSpeed;
            else if (inputMovement.y < 0)
                Thrust.z = inputMovement.y * backwardsSpeed;

            if (inputMovement.x != 0) Thrust.x = inputMovement.x;

            if (inputJump && grounded)
            {
                Debug.Log("want jump");
                Thrust.y = jumpForce;
            }
        }
        #endregion

        #region physics

        private void checkIfGrounded()
        {
            if (Physics.Raycast(groundDetector.position, -groundDetector.up, groundDetectionDistance, groundLayer)) grounded = true;
            else if (Physics.Raycast(groundDetector.position, Vector3.down, groundDetectionDistance, groundLayer)) grounded = true;
            else grounded = false;
        }

        private void addForces()
        {
            if (rb.velocity.magnitude <= maxSpeed && Thrust.z != 0)
            {
                rb.AddForce(forwardVector.forward * Thrust.z * Time.deltaTime, forwardForceMode);
                if (Thrust.y != 0 && grounded) rb.AddForce(center.up * Thrust.y * Time.deltaTime, jumpForceMode);
            }

            if (rb.angularVelocity.magnitude <= maxAngularSpeed && Thrust.x != 0)
                rb.AddRelativeTorque(Vector3.up * Thrust.x * angularSpeed * Time.deltaTime, angularForceMode);

            if (jumpForce != 0)
            {
                Debug.Log("jumping");
                rb.AddForce(center.up * Thrust.y * Time.deltaTime, jumpForceMode);
            }

            if (grounded && additionalGroundedForceUp != 0) rb.AddForce(Vector3.up * additionalGroundedForceUp * Time.deltaTime);
            else if (!grounded && additionalUnGroundedForceUp != 0) rb.AddForce(Vector3.up * additionalUnGroundedForceUp * Time.deltaTime);
        }


        private void hover()
        {
            for (int i = 0; i < hoverPoints.Length; i++)
            {
                Vector3 levelingPos = hoverPoints[i].point.position + hoverPoints[i].point.up * hoverPoints[i].upLevelingOffset;

                if (Physics.Raycast(hoverPoints[i].point.position, -hoverPoints[i].point.up, out RaycastHit hitInfo, hoverHeight, groundLayer))
                {
                    Vector3 updir = (liftTypeSpace == liftType.world) ? Vector3.up : transform.up;

                    rb.AddForceAtPosition(updir * hoverPoints[i].liftForce * (1 - (hitInfo.distance / hoverHeight)) * Time.deltaTime, hoverPoints[i].point.position, hoverPoints[i].liftForceMode);


                    if(Vector3.Angle(hitInfo.normal, Vector3.up) > maxNormalAngle)
                    {
                        Vector3 dir = new Vector3(hitInfo.normal.x, 0, hitInfo.normal.z).normalized; //just get the XZ dir, when there is Y the hoverCraft chitters to much, due to it moving up and down
                        rb.AddForce(dir * normalFixForce * Time.deltaTime,  normalFixForceMode); // just addForce, not at position. if at position hoverCraft will spin too much
                    }
                }
                else if ((hoverPoints[i].WhenToLevel == flythingie.lw.WhenVeicleGroundedAndThisNot && grounded) || hoverPoints[i].WhenToLevel == flythingie.lw.whenThisNotGrounded)
                {
                    levelVeicle(i, levelingPos);
                }

                if (hoverPoints[i].WhenToLevel == flythingie.lw.always)
                {
                    levelVeicle(i, levelingPos);
                }

            }
        }

        private void levelVeicle(int index, Vector3 levelingPos)
        {
            float multiplyier = 1;
            if (Vector3.Distance(Vector3.down, forwardVector.up) < Vector3.Distance(Vector3.up, forwardVector.up)) multiplyier = -1;

            if (hoverPoints[index].point.position.y - hoverPoints[index].levelingHeightDiffrenceTolerance > center.position.y)
            {
                rb.AddForceAtPosition(Vector3.up * -hoverPoints[index].levelingForce * multiplyier * Time.deltaTime, levelingPos, hoverPoints[index].levelForceMode);
            }
            else if (hoverPoints[index].point.position.y + hoverPoints[index].levelingHeightDiffrenceTolerance < center.position.y)
            {
                rb.AddForceAtPosition(Vector3.up * hoverPoints[index].levelingForce * multiplyier * Time.deltaTime, levelingPos, hoverPoints[index].levelForceMode);
            }
        }

        #endregion

        #region input parse

        public void changeYMoveInput(float to)
        {
            if (to > 1) to = 1;
            if (to < -1) to = -1;

            inputMovement.y = to;
        }

        public void changeXMoveInput(float to)
        {
            if (to > 1) to = 1;
            if (to < -1) to = -1;

            inputMovement.x = to;
        }

        public void changeJumpInput(bool to) => inputJump = to;

        public void parseInput(Vector2 move, bool jump)
        {
            inputMovement = move;
            inputJump = jump;
        }

        #endregion

        #region easySetup

        [ContextMenu("set up hover point from 0")]
        public void setUpHoverPoint()
        {
            for (int i = 1; i < hoverPoints.Length; i++) //copy from 0 to others
            {
                Transform p = (hoverPoints[i].point != null)? hoverPoints[i].point: null;
                hoverPoints[i] = hoverPoints[0];
                if(p != null) hoverPoints[i].point = p;
            }
        }

        [ContextMenu("find hover points from hoverPoint 0")]
        public bool findHoverPointFromPoint(bool castErroes)
        {
            if (hoverPoints[0].point == null)
            {
                if(castErroes) Debug.LogError("no hoover point selected in slot 1 of hoverPoints in obj: " + transform.name);
                return false;
            }

            Transform parent = hoverPoints[0].point.parent;

            flythingie hoverCopy = hoverPoints[0];

            hoverPoints = new flythingie[hoverPoints[0].point.parent.childCount];

            for (int i = 0; i < hoverPoints.Length; i++) //paste settings from 1 to others, andd after assighn apropporiate child
            {
                hoverPoints[i] = hoverCopy;
                hoverPoints[i].point = parent.GetChild(i);
            }

            return true;
        }

        [ContextMenu("rigidbodySetup")]
        public void rigidbodySetup()
        {
            rb = GetComponent<Rigidbody>();

            rb.drag = 2.4f;
            rb.angularDrag = 4;
            rb.mass = 20;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.centerOfMass = center.localPosition;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        [ContextMenu("valuesDeafultSetup")]
        public void valuesSetup()
        {
            forwardSpeed = 5000;
            backwardsSpeed = 2000;
            maxSpeed = 20;
            forwardForceMode = ForceMode.Acceleration;
            angularSpeed = 750;
            maxAngularSpeed = 12;
            angularForceMode = ForceMode.Acceleration;
            groundDetectionDistance = 2.4f;
            hoverHeight = 1.8f;
            maxNormalAngle = 40;
            normalFixForce = 5000;
            additionalGroundedForceUp = 3000;
            additionalUnGroundedForceUp = -1000;
            jumpForce = 10000;
            jumpForceMode = ForceMode.Acceleration;

            Transform p = null;

            if(hoverPoints.Length > 0)
            {
                if (hoverPoints[0].point != null) p = hoverPoints[0].point;
            }

            hoverPoints = new flythingie[8];

            hoverPoints[0].point = p;
            hoverPoints[0].liftForce = 25000;
            hoverPoints[0].liftForceMode = ForceMode.Force;
            hoverPoints[0].WhenToLevel = flythingie.lw.whenThisNotGrounded;
            hoverPoints[0].levelingForce = 1000;
            hoverPoints[0].upLevelingOffset = 0.5f;
            hoverPoints[0].levelForceMode = ForceMode.Force;

            if(!findHoverPointFromPoint(false))
            {
                setUpHoverPoint();
            }
        }

        #endregion

        #region classes

        [System.Serializable]
        public struct flythingie
        {

            [Tooltip("position of the hoverpoint")]
            public Transform point;

            [Tooltip("force used to lift the veicle on this point")]
            public float liftForce;

            [Tooltip("the forceMode of force used to lift the veicle")]
            public ForceMode liftForceMode;

            [Tooltip("the conditions for the vehicle to start leveling (proces of making v3 up to vehicle up)")]
            public lw WhenToLevel;

            [Tooltip("the force of leveling")]
            public float levelingForce;

            [Tooltip("the ofset from the point up, for better leveling")]
            public float upLevelingOffset;

            [Tooltip("the tolerance of diffrent height when leveling")]
            public float levelingHeightDiffrenceTolerance;

            [Tooltip("forceMode used for leveling")]
            public ForceMode levelForceMode;

            public enum lw { whenThisNotGrounded, WhenVeicleGroundedAndThisNot, always };
        }

        #endregion
    }
}
