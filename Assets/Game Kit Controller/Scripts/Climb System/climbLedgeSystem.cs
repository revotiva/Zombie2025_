﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class climbLedgeSystem : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public bool climbLedgeActive;
    public bool useHangFromLedgeIcon;

    public bool useFixedDeviceIconPosition;

    public bool keepWeaponsOnLedgeDetected;
    public bool drawWeaponsAfterClimbLedgeIfPreviouslyCarried;

    [Space]
    [Header ("Climb Animator Settings")]
    [Space]

    public int climbLedgeActionID = 1;
    public string holdOnLedgeActionName = "Hold On Ledge";

    public string actionActiveAnimatorName = "Action Active";
    public string actionIDAnimatorName = "Action ID";

    public float matchStartValue;
    public float matchEndValue;
    public Vector3 matchMaskValue;

    public float matchMaskRotationValue;

    public int baseLayerIndex = 0;

    [Space]
    [Header ("Raycast Ledge Detection Settings")]
    [Space]

    public float climbLedgeRayForwardDistance = 1;
    public float climbLedgeRayDownDistance = 1;

    public LayerMask layerMaskToCheck;

    [Space]
    [Header ("Other Settings")]
    [Space]

    public bool onlyGrabLedgeIfMovingForward;

    public float minWaitToClimbAfterHoldOnLedge = 0.6f;

    public float adjustToHoldOnLedgePositionSpeed = 3;
    public float adjustToHoldOnLedgeRotationSpeed = 10;

    public Vector3 holdOnLedgeOffset;
    public Vector3 holdOnLedgeOffsetFBA = new Vector3 (0, -1.65f, -0.8f);

    [Space]

    public Vector3 climbLedgeTargetPositionOffsetThirdPerson;
    public Vector3 climdLedgeTargetPositionOffsetFirstPerson;
    public Vector3 climdLedgeTargetPositionOffsetFirstFBA = new Vector3 (0, 0.64f, 0.15f);

    [Space]

    public float handOffset = 0.2f;

    public float timeToClimbLedgeThirdPerson = 2;
    public float timeToClimbLedgeFirstPerson = 1;

    public float climbLedgeSpeedFirstPerson = 1;

    public bool climbIfSurfaceFoundBelowPlayer;

    public bool checkForLedgeZonesActive = true;

    [Space]
    [Header ("Ledge Below Check Settings")]
    [Space]

    public bool checkForHangFromLedgeOnGround;
    public bool checkLedgeZoneDetectedByRaycast;

    public float raycastRadiusToCheckSurfaceBelowPlayer;

    public float checkForHangFromLedgeOnGroundRaycastDistance;
    public bool onlyHangFromLedgeIfPlayerIsNotMoving;

    public float timeToCancelHangFromLedgeIfNotFound = 3;
    public bool canCancelHangFromLedge;

    public bool hasToLookAtLedgePositionOnFirstPerson;
    public bool useMaxDistanceToCameraCenter;
    public float maxDistanceToCameraCenter;

    [Space]
    [Header ("Auto Climb Ledge Settings")]
    [Space]

    public bool autoClimbInThirdPerson;
    public bool autoClimbInFirstPerson;

    [Space]
    [Header ("Jump Settings")]
    [Space]

    public bool canJumpWhenHoldLedge;
    public float jumpForceWhenHoldLedge = 10;
    public ForceMode jumpForceMode;

    [Space]
    [Header ("Grab Surface On Air Settings")]
    [Space]

    public bool canGrabToSurfaceOnAirInputEnabled;
    public bool canGrabAnySurfaceOnAir;
    public bool useGrabSurfaceAmountLimit;
    public int grabSurfaceAmountLimit;
    public int currentGrabSurfaceAmount;

    [Space]

    public bool useSurfacesToCheckTagsOnGrabSurfaceByInput;
    public List<string> surfacesToCheckTagsGrabSurfaceByInputList = new List<string> ();

    [Space]

    public bool checkMoveInputDirectionOnJump;

    public float jumpRotationSpeedThirdPerson = 1;
    public float jumpRotationSpeedFirstPerson = 0.5f;

    public float jumpForceWhenHoldLedgeOnOppositeDirection = 15;

    [Space]
    [Header ("Event Settings")]
    [Space]

    public bool useEvents;
    public UnityEvent eventOnLedgeGrabbed;
    public UnityEvent eventOnLedgeLose;
    public UnityEvent eventOnLedgeClimbed;
    public UnityEvent eventOnStartLedgeClimb;
    public UnityEvent eventOnLedgeJump;

    [Space]
    [Header ("Gizmo Settings")]
    [Space]

    public bool showGizmo;
    public Color gizmoColor;
    public float gizmoRadius;

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;

    public bool avoidPlayerGrabLedge;

    public bool ledgeZoneFound;

    public bool activateClimbAction;
    public bool canStartToClimbLedge;
    public bool climbingLedge;
    public bool canUseClimbLedge;

    public bool canClimbCurrentLedgeZone = true;

    public bool stopGrabLedge;

    public float directionAngle;

    public bool surfaceToHangOnGroundFound;
    public bool movingTowardSurfaceToHang;
    public bool previouslyMovingTowardSurfaceToHang;

    public bool onAirWhileSearchingLedgeToHang;

    public bool ledgeZoneCloseEnoughToScreenCenter;
    public float currentDistanceToTarget;

    public bool canCheckForHangFromLedgeOnGround = true;

    public bool climbLedgeActionActivated;
    public bool loseLedgeActionActivated;

    public bool grabbingSurface;

    public bool climbLedgeActionPaused;

    public bool otherClimbActionActive;

    [Space]
    [Header ("Climb Elements")]
    [Space]

    public Transform playerTransform;

    public Transform climbLedgeRayPosition;
    public Transform climbLedgeRayDownPosition;
    public Transform holdLedgeZone;
    public Transform climbLedgZone;
    public Transform hangFromLedgeOnGroundRaycastPosition;

    public GameObject hangFromLedgeIcon;
    public RectTransform hangFromLedgeIconRectTransform;

    [Space]
    [Header ("Components")]
    [Space]

    public playerController playerControllerManager;
    public playerInputManager playerInput;
    public playerWeaponsManager weaponsManager;
    public otherPowers powersManager;
    public headBob headBobManager;
    public gravitySystem gravityManager;
    public Camera mainCamera;
    public playerCamera mainPlayerCamera;
    public Rigidbody mainRigidbody;
    public Animator animator;
    public Collider MainCollider;
    public usingDevicesSystem usingDevicesManager;


    bool canGrabAnySurfaceOnAirActive = true;

    bool groundChecked;

    float lastTimeOnGround;

    Vector3 surfaceFoundNormal;

    RaycastHit hit;

    Coroutine holdOnLedgeCoroutine;
    Coroutine climbLedgeOnFirstPersonCoroutine;

    float lastTimeHoldOnEdge;

    float lastTimeLedgeChecked;

    bool playerOnGround;
    bool carryingWeaponsPreviously;

    bool pauseAllPlayerDownForces;

    bool wallRunningActive;

    bool freeClimbActive;

    bool swimModeActive;

    bool flyModeActive;

    bool waitingForMinTimeToCheckForLedge;

    Vector3 currentRayPosition;
    Vector3 currentRayDirection;
    Vector3 playerTargetRotation;

    bool firstPersonActive;

    float lastTimeClimbLedge;

    float originalClimbLedgeRayForwardDistance;
    float originalClimbLedgeRayDownDistance;

    Transform previousParent;
    parentAssignedSystem currentParentAssignedSystem;

    Ray ray;
    bool surfaceBelowPlayerDetected;

    Vector3 checkDownRaycastOffset;

    GameObject currentSurfaceFound;
    GameObject previousSurfaceFound;

    GameObject currentSurfaceFoundBelow;
    GameObject previousSurfaceFoundBelow;

    bool stopHangFromLedgeAction;

    float lastTimeVerticalInputPressed;
    bool movementInputPressed;
    bool movementInputReleased;

    float lastTimeMovingTowardSurfaceToHang;

    float lastTimeLedgeToHangFound;

    Vector3 surfaceToHangNormal;

    Vector3 screenPoint;

    Vector2 iconPosition2d;
    bool usingScreenSpaceCamera;

    Vector2 mainCanvasSizeDelta;
    Vector2 halfMainCanvasSizeDelta;
    bool targetOnScreen;
    Vector3 currentIconPosition;
    Vector3 originalDeviceIconPosition;

    Vector3 lastMovementDirection;

    Vector2 rawMovementAxis;

    Coroutine rotateToLedgeOnFirstPersonCoroutine;
    bool rotationToLedgeOnFirstPersonActive;
    bool playerRotatedToLedgeOnFirstPerson;

    bool originalOnlyHangFromLedgeIfPlayerIsNotMovingValue;

    int actionActiveAnimatorID;
    int actionIDAnimatorID;

    float screenWidth;
    float screenHeight;

    Coroutine climbLedgeActionPausedCoroutine;

    bool grabToSurfaceInputPaused;

    bool callingActivateGrabToSurfaceActionExternally;

    Transform newLedgeTransformToGrabAutomatically;

    Vector3 currentDirectionToRotatePlayer;


    void Start ()
    {
        actionActiveAnimatorID = Animator.StringToHash (actionActiveAnimatorName);
        actionIDAnimatorID = Animator.StringToHash (actionIDAnimatorName);

        climbLedgZone.SetParent (null);
        holdLedgeZone.SetParent (null);

        originalClimbLedgeRayForwardDistance = climbLedgeRayForwardDistance;
        originalClimbLedgeRayDownDistance = climbLedgeRayDownDistance;

        mainCanvasSizeDelta = mainPlayerCamera.getMainCanvasSizeDelta ();
        halfMainCanvasSizeDelta = mainCanvasSizeDelta * 0.5f;
        usingScreenSpaceCamera = mainPlayerCamera.isUsingScreenSpaceCamera ();

        originalDeviceIconPosition = hangFromLedgeIconRectTransform.position;

        originalOnlyHangFromLedgeIfPlayerIsNotMovingValue = onlyHangFromLedgeIfPlayerIsNotMoving;

        if (playerTransform == null) {
            playerTransform = transform;
        }

        if (mainCamera == null) {
            mainCamera = mainPlayerCamera.getMainCamera ();
        }
    }

    void FixedUpdate ()
    {
        if (!climbLedgeActive) {
            return;
        }

        if (avoidPlayerGrabLedge) {
            return;
        }

        if (!canUseClimbLedge) {
            return;
        }

        if (climbLedgeActionPaused) {
            return;
        }

        if (otherClimbActionActive) {
            return;
        }

        if (playerControllerManager.isIgnoreExternalActionsActiveState ()) {
            return;
        }

        pauseAllPlayerDownForces = playerControllerManager.isPauseAllPlayerDownForcesActive ();

        if (pauseAllPlayerDownForces) {
            return;
        }

        wallRunningActive = playerControllerManager.isWallRunningActive ();

        swimModeActive = playerControllerManager.isSwimModeActive ();

        freeClimbActive = playerControllerManager.isFreeClimbActive ();

        flyModeActive = playerControllerManager.isFlyingActive ();

        if (lastTimeClimbLedge == 0) {
            waitingForMinTimeToCheckForLedge = false;
        } else {
            waitingForMinTimeToCheckForLedge = (Time.time < lastTimeClimbLedge + 0.7f);
        }

        if (wallRunningActive) {
            return;
        }

        if (swimModeActive) {
            return;
        }

        if (freeClimbActive) {
            return;
        }

        if (flyModeActive) {
            return;
        }

        if (waitingForMinTimeToCheckForLedge) {
            return;
        }

        //the auto hang from ledge is active and in process
        if (checkForHangFromLedgeOnGround) {

            //override player input and make him move toward the position to hang from the ledge detected
            if (movingTowardSurfaceToHang) {
                if (playerOnGround) {
                    playerInput.overrideInputValues (new Vector2 (0, -1), true);

                    if (onAirWhileSearchingLedgeToHang) {
                        stopHangFromLedge ();
                    }
                } else {

                    onAirWhileSearchingLedgeToHang = true;

                    if (!climbingLedge) {
                        if (firstPersonActive) {
                            if (!playerRotatedToLedgeOnFirstPerson) {
                                rotateTowardLedgeOnFirstPerson ();
                            }

                            if (!rotationToLedgeOnFirstPersonActive) {
                                playerInput.overrideInputValues (new Vector2 (0, 1), true);
                            }
                        } else {
                            playerInput.overrideInputValues (new Vector2 (0, 1), true);
                        }
                    } else {
                        stopHangFromLedge ();
                    }
                }

                //if the auto hang from ledge time is over of the player moves while he is moving automatically, cancel the auto hang action
                if (Time.time > lastTimeMovingTowardSurfaceToHang + timeToCancelHangFromLedgeIfNotFound ||
                    (canCancelHangFromLedge && onlyHangFromLedgeIfPlayerIsNotMoving && Time.time > lastTimeMovingTowardSurfaceToHang + 0.5f && playerInput.getAuxRawMovementAxis () != Vector2.zero)) {
                    stopHangFromLedge ();
                }
            } else {
                //if the player only hangs from the ledge when he is not moving and he is on the air, cancel the auto hang action
                if (onlyHangFromLedgeIfPlayerIsNotMoving && surfaceToHangOnGroundFound) {
                    if (!playerOnGround) {
                        stopHangFromLedge ();
                        return;
                    }
                }
            }
        }
    }

    void Update ()
    {
        if (climbLedgeActive) {
            pauseAllPlayerDownForces = playerControllerManager.isPauseAllPlayerDownForcesActive ();

            if (pauseAllPlayerDownForces ||
                wallRunningActive ||
                swimModeActive ||
                freeClimbActive ||
                flyModeActive ||
                waitingForMinTimeToCheckForLedge ||
                playerControllerManager.isIgnoreExternalActionsActiveState () ||
                climbLedgeActionPaused) {

                if (surfaceToHangOnGroundFound) {
                    stopHangFromLedge ();
                }

                return;
            }

            playerOnGround = playerControllerManager.isPlayerOnGround ();

            firstPersonActive = playerControllerManager.isPlayerOnFirstPerson ();

            if (groundChecked != playerOnGround) {
                groundChecked = playerOnGround;

                if (playerOnGround) {
                    currentGrabSurfaceAmount = 0;
                }
            }
        }

        //if the player is climbing a ledge, then
        if (climbLedgeActive && climbingLedge) {

            //match animation with player controller position
            if (!firstPersonActive) {
                if (playerControllerManager.getCurrentNormal () == Vector3.up) {
                    if (Time.time < lastTimeClimbLedge + timeToClimbLedgeThirdPerson) {

                        MatchTarget (climbLedgZone.position + handOffset * playerTransform.forward,
                            climbLedgZone.rotation,
                            AvatarTarget.RightHand,
                            new MatchTargetWeightMask (matchMaskValue, matchMaskRotationValue),
                            matchStartValue,
                            matchEndValue);
                    }
                }
            }

            //check to disable the climb action and resume the player state once the ledge is climbed, with different values according to camera view, first or third person
            if (activateClimbAction) {
                if (canStartToClimbLedge) {
                    if (firstPersonActive) {
                        if (Time.time > (lastTimeClimbLedge + timeToClimbLedgeFirstPerson)) {
                            resumePlayer ();

                            return;
                        }
                    } else {
                        if (Time.time > (lastTimeClimbLedge + timeToClimbLedgeThirdPerson)) {
                            resumePlayer ();

                            return;
                        }
                    }
                }
            } else {
                //prevent the player to rotate due to current idle animation
                if (!firstPersonActive) {
                    playerTransform.eulerAngles = playerTargetRotation;
                }
            }

            //if the player is hanging from the ledge, then
            if (!activateClimbAction) {

                if (Time.time > lastTimeHoldOnEdge + minWaitToClimbAfterHoldOnLedge) {

                    //if some object is detected below the player, activate automatically the climb to avoid that object from moving through the player
                    if (climbIfSurfaceFoundBelowPlayer) {
                        ray = new Ray (playerTransform.position, -playerTransform.up);

                        if (Physics.SphereCast (ray, raycastRadiusToCheckSurfaceBelowPlayer, raycastRadiusToCheckSurfaceBelowPlayer, layerMaskToCheck)) {
                            surfaceBelowPlayerDetected = true;
                        } else {
                            surfaceBelowPlayerDetected = false;
                        }
                    }

                    //check for movement input, so if the direction of the movement is the same direction of the player forward direction accoding to camera facing direction, then
                    rawMovementAxis = playerInput.getPlayerRawMovementAxis ();

                    if (rawMovementAxis != Vector2.zero) {
                        Vector3 movementDirection = rawMovementAxis.y * playerControllerManager.getCurrentForwardDirection () + rawMovementAxis.x * playerControllerManager.getCurrentRightDirection ();

                        directionAngle = Vector3.SignedAngle (movementDirection, playerTransform.forward, playerTransform.up);

                        if (Mathf.Abs (directionAngle) < 90) {

                            if (canClimbCurrentLedgeZone) {
                                climbLedgeActionActivated = true;
                            }
                        } else {
                            loseLedgeActionActivated = true;
                        }
                    }

                    if (canClimbCurrentLedgeZone) {
                        if (firstPersonActive) {
                            if (autoClimbInFirstPerson) {
                                climbLedgeActionActivated = true;
                            }
                        } else {
                            if (autoClimbInThirdPerson) {
                                climbLedgeActionActivated = true;
                            }
                        }
                    }

                    //climb the ledge
                    if (climbLedgeActionActivated || surfaceBelowPlayerDetected) {
                        startClimbLedgeAction ();
                    } else if (loseLedgeActionActivated) {
                        //lose the ledge
                        startLoseLedgeAction ();
                    }
                }
            }
        }

        //if the option to auto hang from ledge is active and only when the player is not moving, then
        if (playerOnGround && checkForHangFromLedgeOnGround && surfaceToHangOnGroundFound && !movingTowardSurfaceToHang && onlyHangFromLedgeIfPlayerIsNotMoving) {
            //place an UI element which follows the position of the ledge found
            if (useHangFromLedgeIcon) {
                currentIconPosition = holdLedgeZone.position;

                if (!usingScreenSpaceCamera) {
                    screenWidth = Screen.width;
                    screenHeight = Screen.height;
                }

                if (usingScreenSpaceCamera) {
                    screenPoint = mainCamera.WorldToViewportPoint (currentIconPosition);
                    targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                } else {
                    screenPoint = mainCamera.WorldToScreenPoint (currentIconPosition);
                    targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < screenWidth && screenPoint.y > 0 && screenPoint.y < screenHeight;
                }

                if (targetOnScreen) {
                    if (useFixedDeviceIconPosition) {
                        hangFromLedgeIcon.transform.position = originalDeviceIconPosition;
                    } else {
                        if (usingScreenSpaceCamera) {
                            iconPosition2d = new Vector2 ((screenPoint.x * mainCanvasSizeDelta.x) - halfMainCanvasSizeDelta.x,
                                (screenPoint.y * mainCanvasSizeDelta.y) - halfMainCanvasSizeDelta.y);

                            hangFromLedgeIconRectTransform.anchoredPosition = iconPosition2d;
                        } else {
                            hangFromLedgeIcon.transform.position = new Vector3 (screenPoint.x, screenPoint.y, 0);
                        }
                    }
                    enableOrDisableHangFromLedgeIcon (true);
                } else {
                    enableOrDisableHangFromLedgeIcon (false);
                }
            }

            if (hasToLookAtLedgePositionOnFirstPerson && firstPersonActive) {
                if (targetOnScreen) {
                    if (useMaxDistanceToCameraCenter) {

                        currentDistanceToTarget = GKC_Utils.distance (hangFromLedgeIconRectTransform.localPosition, Vector3.zero);
                        if (currentDistanceToTarget < maxDistanceToCameraCenter) {
                            ledgeZoneCloseEnoughToScreenCenter = true;
                        } else {
                            ledgeZoneCloseEnoughToScreenCenter = false;
                        }
                    } else {
                        ledgeZoneCloseEnoughToScreenCenter = true;
                    }
                } else {
                    ledgeZoneCloseEnoughToScreenCenter = false;
                }

                enableOrDisableHangFromLedgeIcon (ledgeZoneCloseEnoughToScreenCenter);
            }
        }

        if (!climbLedgeActive) {
            return;
        }

        if (avoidPlayerGrabLedge) {
            return;
        }

        //check if the player can use the climb ledge system right now
        canUseClimbLedge = !playerControllerManager.isPlayerDead () &&
        !playerControllerManager.isPlayerDriving () &&
        !playerControllerManager.isPlayerRotatingToSurface () &&
        !playerControllerManager.isPlayerOnFFOrZeroGravityModeOn () &&
        !gravityManager.isPlayerSearchingGravitySurface ();

        if (!canUseClimbLedge) {
            return;
        }

        if (playerOnGround) {

            //if the auto hang action check is active, then
            if (checkForHangFromLedgeOnGround) {

                //search a surface in front of the player and in down direction, according to hangFromLedgeOnGroundRaycastPosition transform forward direction
                if (!surfaceToHangOnGroundFound) {
                    //if the player is moving or if he only auto hang from ledge when not moving, to detect it first, then
                    if (Time.time > lastTimeLedgeToHangFound + 1 && (!onlyHangFromLedgeIfPlayerIsNotMoving || playerControllerManager.isPlayerMoving (0))) {
                        //check the ledge in front of him
                        currentRayPosition = hangFromLedgeOnGroundRaycastPosition.position;
                        currentRayDirection = hangFromLedgeOnGroundRaycastPosition.forward;

                        if (!Physics.Raycast (currentRayPosition, currentRayDirection, out hit, checkForHangFromLedgeOnGroundRaycastDistance, layerMaskToCheck)) {
                            //if not surface is found, then
                            if (showGizmo) {
                                Debug.DrawRay (currentRayPosition, checkForHangFromLedgeOnGroundRaycastDistance * currentRayDirection, Color.green);
                            }

                            //search for the closest point surface of that ledge, by lowering the raycast position until a surface is found
                            bool surfaceToLedgeDetected = false;

                            RaycastHit newHit = new RaycastHit ();

                            int numberOfLoops = 0;

                            while (!surfaceToLedgeDetected && numberOfLoops < 100) {
                                if (showGizmo) {
                                    Debug.DrawRay (currentRayPosition, checkForHangFromLedgeOnGroundRaycastDistance * currentRayDirection, Color.blue, 4);
                                }

                                if (Physics.Raycast (currentRayPosition, currentRayDirection, out newHit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                                    hit = newHit;

                                    surfaceToLedgeDetected = true;

                                    holdLedgeZone.position = hit.point + 0.04f * playerTransform.up;
                                } else {
                                    holdLedgeZone.position = hit.point + 0.04f * playerTransform.up;

                                    currentRayPosition -= 0.05f * hangFromLedgeOnGroundRaycastPosition.up;
                                }

                                numberOfLoops++;
                            }

                            //once the ledge is found, then
                            if (surfaceToLedgeDetected) {

                                if (checkLedgeZoneDetectedByRaycast) {
                                    currentSurfaceFoundBelow = hit.collider.gameObject;

                                    if (currentSurfaceFoundBelow != previousSurfaceFoundBelow) {
                                        previousSurfaceFoundBelow = currentSurfaceFoundBelow;

                                        ledgeZoneSystem currentLedgeZoneSystem = currentSurfaceFoundBelow.GetComponent<ledgeZoneSystem> ();

                                        if (currentLedgeZoneSystem != null) {
                                            stopHangFromLedgeAction = currentLedgeZoneSystem.avoidPlayerGrabLedge;

                                            canClimbCurrentLedgeZone = currentLedgeZoneSystem.ledgeZoneCanBeClimbed;

                                            canCheckForHangFromLedgeOnGround = currentLedgeZoneSystem.canCheckForHangFromLedgeOnGround;

                                        } else {
                                            stopHangFromLedgeAction = false;
                                            canClimbCurrentLedgeZone = true;
                                            canCheckForHangFromLedgeOnGround = true;
                                        }
                                    }

                                    if (!canCheckForHangFromLedgeOnGround) {
                                        return;
                                    }

                                    //the current surface can't be grabbed, so stop the rest of checkings until a new surface is found
                                    if (stopHangFromLedgeAction) {
                                        return;
                                    }
                                }

                                //launch a raycast to get the position of the wall below the ledge, to get also the normal of that surface, so the player can move and rotate toward that direction
                                currentRayPosition = holdLedgeZone.position + 1.4f * playerTransform.forward - playerTransform.up;

                                currentRayDirection = -playerTransform.forward;

                                if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, checkForHangFromLedgeOnGroundRaycastDistance, layerMaskToCheck)) {

                                    //once that surface is found, check that there is enough room in the ledge for the player to hang there, for example an space of an unit is not enough, using the
                                    //regular height of the player, 2 units, as measure
                                    currentRayPosition = holdLedgeZone.position + 0.5f * hit.normal;
                                    currentRayDirection = -playerTransform.up;

                                    RaycastHit newAirHit = new RaycastHit ();

                                    if (!Physics.Raycast (currentRayPosition, currentRayDirection, out newAirHit, 2, layerMaskToCheck)) {

                                        surfaceToHangNormal = hit.normal;
                                        holdLedgeZone.rotation = Quaternion.LookRotation (-surfaceToHangNormal, playerTransform.up);

                                        lastTimeLedgeToHangFound = Time.time;

                                        surfaceToHangOnGroundFound = true;

                                        holdLedgeZone.position += 0.5f * hit.normal;
                                    }
                                }
                            }
                        } else {
                            if (showGizmo) {
                                Debug.DrawRay (currentRayPosition, hit.distance * currentRayDirection, Color.white);
                            }
                        }
                    }
                } else {
                    //once a possible ledge to hang is found, then 
                    if (!movingTowardSurfaceToHang) {
                        //if the player needs to being close to the ledge to detect it and then press the movement input again to activate the auto hang, then
                        if (onlyHangFromLedgeIfPlayerIsNotMoving) {
                            //use a delay to avoid different input signal
                            if (Time.time > lastTimeLedgeToHangFound + 0.5f) {

                                if (!hasToLookAtLedgePositionOnFirstPerson || (!firstPersonActive || ledgeZoneCloseEnoughToScreenCenter)) {
                                    //check the direction of the input and check how much time is pressed, using only a quick press
                                    Vector3 movementDirection = playerControllerManager.getMoveInputDirection ();

                                    if (Mathf.Abs (movementDirection.magnitude) > 0) {
                                        if (!movementInputPressed) {
                                            lastTimeVerticalInputPressed = Time.time;
                                            movementInputPressed = true;
                                        }

                                        lastMovementDirection = movementDirection;
                                    } else {
                                        if (movementInputPressed) {
                                            movementInputReleased = true;
                                        }
                                    }

                                    //once the input is quickly pressed
                                    if (movementInputPressed && movementInputReleased) {
                                        //if the amount of time to press the input is correct
                                        if (Time.time < lastTimeVerticalInputPressed + 0.4f) {

                                            //check the direction of the input according to camera direction and player forward direction
                                            Vector3 ledgeDirection = holdLedgeZone.position - playerTransform.position;

                                            directionAngle = Vector3.SignedAngle (lastMovementDirection, ledgeDirection, playerTransform.up);

                                            if (Mathf.Abs (directionAngle) < 90) {
                                                //orientation is correct, proceed to auto hang
                                                lastTimeMovingTowardSurfaceToHang = Time.time;
                                                movingTowardSurfaceToHang = true;

                                                previouslyMovingTowardSurfaceToHang = true;

                                                playerControllerManager.overrideMainCameraTransformDirection (holdLedgeZone.transform, true);

                                                if (useHangFromLedgeIcon) {
                                                    enableOrDisableHangFromLedgeIcon (false);
                                                }
                                            } else {
                                                stopHangFromLedge ();
                                            }
                                        } else {
                                            //else, stop the auto hang
                                            stopHangFromLedge ();
                                        }
                                    }
                                }
                            }
                        } else {
                            //else, the player only needs to get close to the ledge and the system will automatically activate the auto hang
                            lastTimeMovingTowardSurfaceToHang = Time.time;

                            movingTowardSurfaceToHang = true;

                            previouslyMovingTowardSurfaceToHang = true;

                            playerControllerManager.overrideMainCameraTransformDirection (holdLedgeZone.transform, true);
                        }
                    }
                }
            }

            return;
        }

        //check if the player only grabs a ledge when he is pressing the movement input
        if (onlyGrabLedgeIfMovingForward && !previouslyMovingTowardSurfaceToHang) {
            if (playerControllerManager.isPlayerMoving (0)) {
                return;
            }
        }

        //check for a surface to climb in front of the player when he is on the air
        if (!climbingLedge && Time.time > lastTimeLedgeChecked + 0.6f) {
            currentRayPosition = climbLedgeRayPosition.position;
            currentRayDirection = climbLedgeRayPosition.forward;

            //check surfaces in front of him
            if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayForwardDistance, layerMaskToCheck)) {
                if (hit.rigidbody != null) {
                    return;
                }

                //check if there is a ledge zone system attached in the current surface in front of him, to get the values configured on it, so maybe that surface can be grabbed or climbed
                if (checkLedgeZoneDetectedByRaycast) {
                    currentSurfaceFound = hit.collider.gameObject;

                    if (currentSurfaceFound != previousSurfaceFound) {
                        previousSurfaceFound = currentSurfaceFound;

                        ledgeZoneSystem currentLedgeZoneSystem = currentSurfaceFound.GetComponent<ledgeZoneSystem> ();

                        if (currentLedgeZoneSystem != null) {
                            stopGrabLedge = currentLedgeZoneSystem.avoidPlayerGrabLedge;

                            canClimbCurrentLedgeZone = currentLedgeZoneSystem.ledgeZoneCanBeClimbed;
                            canCheckForHangFromLedgeOnGround = currentLedgeZoneSystem.canCheckForHangFromLedgeOnGround;
                        } else {
                            stopHangFromLedgeAction = false;
                            canClimbCurrentLedgeZone = true;
                            canCheckForHangFromLedgeOnGround = true;
                        }
                    }

                    //the current surface can't be grabbed, so stop the rest of checkings until a new surface is found
                    if (stopGrabLedge) {
                        return;
                    }
                }

                if (showGizmo) {
                    Debug.DrawRay (currentRayPosition, climbLedgeRayForwardDistance * currentRayDirection, Color.blue);
                }

                surfaceFoundNormal = hit.normal;

                //if a surface is located in front of the player, checks that there is enough space above it, by checking that a second raycast doesn't found a surface
                currentRayPosition = climbLedgeRayPosition.position + 0.3f * climbLedgeRayPosition.up;
                currentRayDirection = climbLedgeRayPosition.forward;

                if (!Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayForwardDistance, layerMaskToCheck)) {
                    if (showGizmo) {
                        Debug.DrawRay (currentRayPosition, climbLedgeRayForwardDistance * currentRayDirection, Color.blue);
                    }

                    //check then the ledge surface
                    currentRayPosition = climbLedgeRayDownPosition.position;
                    currentRayDirection = -climbLedgeRayDownPosition.up;

                    if (ledgeZoneFound) {
                        currentRayPosition +=
                            checkDownRaycastOffset.x * playerTransform.right +
                        checkDownRaycastOffset.y * playerTransform.up +
                        checkDownRaycastOffset.z * playerTransform.forward;
                    }

                    if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                        if (showGizmo) {
                            Debug.DrawRay (currentRayPosition, climbLedgeRayDownDistance * currentRayDirection, Color.yellow);
                        }

                        activateGrabLedge ();
                    } else {
                        if (showGizmo) {
                            Debug.DrawRay (currentRayPosition, climbLedgeRayDownDistance * currentRayDirection, Color.white);
                        }
                    }
                } else {
                    if (showGizmo) {
                        Debug.DrawRay (currentRayPosition, climbLedgeRayForwardDistance * currentRayDirection, Color.red);
                    }
                }
            } else {
                if (showGizmo) {
                    Debug.DrawRay (currentRayPosition, climbLedgeRayForwardDistance * currentRayDirection, Color.red);
                }

                if (checkLedgeZoneDetectedByRaycast) {
                    currentSurfaceFound = null;

                    previousSurfaceFound = null;

                    stopGrabLedge = false;
                }
            }
        }
    }

    public void enableOrDisableHangFromLedgeIcon (bool state)
    {
        if (hangFromLedgeIcon.activeSelf != state) {
            hangFromLedgeIcon.SetActive (state);
        }
    }

    //once the player climbs a ledge or lose it or jumps above it, resume player components and state
    public void resumePlayer ()
    {
        stopAdjustHoldOnLedgeCoroutine ();

        stopClimbLedgeOnFirstPersonMovementCoroutine ();

        if (currentParentAssignedSystem != null) {
            if (previousParent != null) {
                playerControllerManager.setPlayerAndCameraParent (previousParent);
            } else {
                playerControllerManager.setPlayerAndCameraParent (null);
            }

            holdLedgeZone.SetParent (null);
            climbLedgZone.SetParent (null);

            currentParentAssignedSystem = null;
        }

        surfaceBelowPlayerDetected = false;

        lastTimeLedgeChecked = Time.time;

        climbingLedge = false;

        activateClimbAction = false;

        climbLedgeActionActivated = false;
        loseLedgeActionActivated = false;

        previouslyMovingTowardSurfaceToHang = false;

        playerControllerManager.setLastTimeFalling ();

        playerControllerManager.changeScriptState (true);

        playerControllerManager.setHeadTrackCanBeUsedState (true);

        playerControllerManager.setApplyRootMotionAlwaysActiveState (false);

        playerControllerManager.setActionActiveState (false);

        playerControllerManager.setGravityForcePuase (false);

        playerControllerManager.setCheckOnGroungPausedState (false);

        playerControllerManager.setFootStepManagerState (false);

        MainCollider.isTrigger = false;

        mainRigidbody.isKinematic = false;

        if (!firstPersonActive) {
            animator.SetInteger (actionIDAnimatorID, 0);
            animator.SetBool (actionActiveAnimatorID, false);
        }

        headBobManager.pauseHeadBodWithDelay (0.5f);
        headBobManager.playOrPauseHeadBob (true);

        if (carryingWeaponsPreviously && drawWeaponsAfterClimbLedgeIfPreviouslyCarried) {
            weaponsManager.checkIfDrawSingleOrDualWeapon ();
        }

        checkEventOnLedgeClimbed ();

        grabbingSurface = false;

        mainPlayerCamera.setOriginalMoveCameraPositionWithMouseWheelActiveState ();

        if (showDebugPrint) {
            print ("resume player");
        }

        playerControllerManager.setUpdate2_5dClampedPositionPausedState (false);

        if (setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive) {
            mainPlayerCamera.setPivotCameraTransformOriginalParent ();

            mainPlayerCamera.resetPivotCameraTransformLocalRotation ();

            mainPlayerCamera.setPlayerCameraTransformFollowsPlayerTransformRotationOnFBAState (false);
        }

        setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = false;

        playerControllerManager.setGrabbingToSurfaceActiveState (false);

        playerControllerManager.setCurrentCustomActionCategoryID (0);

        playerControllerManager.setAdjustingToSurfaceToGrabInProcessState (false);
    }

    //function called to climb a ledge
    public void startClimbLedgeAction ()
    {
        if (!canClimbCurrentLedgeZone) {
            return;
        }

        playerControllerManager.setApplyRootMotionAlwaysActiveState (true);

        stopAdjustHoldOnLedgeCoroutine ();

        playerControllerManager.setAdjustingToSurfaceToGrabInProcessState (true);

        playerControllerManager.overrideOnGroundAnimatorValue (timeToClimbLedgeThirdPerson + 0.5f);

        playerControllerManager.setFootStepManagerState (false);

        lastTimeClimbLedge = Time.time;

        activateClimbAction = true;

        if (!firstPersonActive) {
            animator.SetInteger (actionIDAnimatorID, climbLedgeActionID);
        }

        canStartToClimbLedge = true;

        if (firstPersonActive) {
            climbLedgeOnFirstPersonMovement ();
        } else {
            mainRigidbody.isKinematic = false;

            if (playerControllerManager.getCurrentNormal () != Vector3.up) {
                climbLedgZone.position -= 0.5f * playerTransform.forward - 0.15f * playerTransform.up;

                activatingManualMatchTargetPosition = true;

                climbLedgeOnFirstPersonMovement ();
            }
        }

        checkEventOnStartLedgeClimb ();
    }

    bool activatingManualMatchTargetPosition;

    //function called to lose a ledge
    public void startLoseLedgeAction ()
    {
        if (!climbingLedge) {
            return;
        }

        activateClimbAction = true;

        playerControllerManager.currentVelocity = 10 * (-playerTransform.up);

        resumePlayer ();

        checkEventOnLedgeLose ();
    }

    public void loseAnyGrabbedSurface ()
    {
        if (grabbingSurface) {
            resumePlayer ();
        }
    }

    //match target animation function
    public void MatchTarget (Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
    {
        if (animator.isMatchingTarget) {
            return;
        }

        if (animator.IsInTransition (baseLayerIndex)) {
            return;
        }

        float normalizeTime = Mathf.Repeat (animator.GetCurrentAnimatorStateInfo (baseLayerIndex).normalizedTime, 1f);

        if (normalizeTime > normalisedEndTime) {
            return;
        }

        animator.MatchTarget (matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
    }

    //adjust player to the current surface found in that edge, for position and rotation, using a corutine
    public void adjustHoldOnLedge ()
    {
        stopAdjustHoldOnLedgeCoroutine ();

        holdOnLedgeCoroutine = StartCoroutine (adjustHoldOnLedgeCoroutine ());
    }

    public void stopAdjustHoldOnLedgeCoroutine ()
    {
        if (holdOnLedgeCoroutine != null) {
            StopCoroutine (holdOnLedgeCoroutine);
        }

        playerControllerManager.setAdjustingToSurfaceToGrabInProcessState (false);
    }

    IEnumerator adjustHoldOnLedgeCoroutine ()
    {
        //		holdLedgeZone.rotation = Quaternion.LookRotation (-surfaceFoundNormal, playerTransform.up);

        //		float angleWithPlayer = Vector3.SignedAngle (playerTransform.right, holdLedgeZone.right, playerTransform.up);
        //
        //		print (angleWithPlayer);

        //		holdLedgeZone.Rotate (angleWithPlayer, 0, 0);

        playerControllerManager.setAdjustingToSurfaceToGrabInProcessState (true);


        Vector3 currentPlayerRotation = playerTransform.eulerAngles;

        float angleWithSurface = Vector3.SignedAngle (-surfaceFoundNormal, playerTransform.forward, playerTransform.up);

        Vector3 currentTargetRotation = playerTransform.eulerAngles - angleWithSurface * playerTransform.up;


        holdLedgeZone.eulerAngles = currentTargetRotation;

        Vector3 currentHoldOnLedgeOffset = holdOnLedgeOffset;

        if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
            currentHoldOnLedgeOffset = holdOnLedgeOffsetFBA;
        }

        holdLedgeZone.position +=
            currentHoldOnLedgeOffset.x * holdLedgeZone.right +
            currentHoldOnLedgeOffset.y * playerTransform.up +
            currentHoldOnLedgeOffset.z * holdLedgeZone.forward;

        //		Quaternion lookRotationTarget = Quaternion.LookRotation (-surfaceFoundNormal, playerTransform.up);

        if (showDebugPrint) {
            print (angleWithSurface + " " + (-surfaceFoundNormal) + " " + currentTargetRotation);
        }

        playerTargetRotation = currentTargetRotation;

        float dist = GKC_Utils.distance (playerTransform.position, holdLedgeZone.position);
        float duration = dist / adjustToHoldOnLedgePositionSpeed;

        float translateTimer = 0;
        float rotateTimer = 0;

        float teleportTimer = 0;

        float normalAngle = 0;

        float positionDifference = 0;

        bool targetReached = false;

        while (!targetReached) {
            translateTimer += Time.deltaTime / duration;

            playerTransform.position = Vector3.Lerp (playerTransform.position, holdLedgeZone.position, translateTimer);

            rotateTimer += Time.deltaTime * adjustToHoldOnLedgeRotationSpeed;

            playerTransform.eulerAngles = Vector3.Lerp (currentPlayerRotation, playerTargetRotation, rotateTimer);

            teleportTimer += Time.deltaTime;

            normalAngle = Mathf.Abs (Vector3.SignedAngle (playerTransform.forward, -surfaceFoundNormal, playerTransform.up));

            positionDifference = GKC_Utils.distance (playerTransform.position, holdLedgeZone.position);

            if ((positionDifference < 0.01f && normalAngle < 1) || teleportTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }

        playerControllerManager.setAdjustingToSurfaceToGrabInProcessState (false);
    }

    //climg ledge in first person is not driven by root motion and animation, so the movement is managed by translation using a coroutine
    public void climbLedgeOnFirstPersonMovement ()
    {
        stopClimbLedgeOnFirstPersonMovementCoroutine ();

        climbLedgeOnFirstPersonCoroutine = StartCoroutine (climbLedgeOnFirstPersonMovementCoroutine ());
    }

    public void stopClimbLedgeOnFirstPersonMovementCoroutine ()
    {
        if (climbLedgeOnFirstPersonCoroutine != null) {
            StopCoroutine (climbLedgeOnFirstPersonCoroutine);
        }
    }

    IEnumerator climbLedgeOnFirstPersonMovementCoroutine ()
    {
        float dist = GKC_Utils.distance (playerTransform.position, climbLedgZone.position);
        float duration = dist / climbLedgeSpeedFirstPerson;

        if (activatingManualMatchTargetPosition) {
            duration = dist / (climbLedgeSpeedFirstPerson / 4);

            activatingManualMatchTargetPosition = false;
        }

        float translateTimer = 0;

        float teleportTimer = 0;

        bool targetReached = false;

        float positionDifference = 0;

        while (!targetReached) {
            translateTimer += Time.deltaTime / duration;

            playerTransform.position = Vector3.Lerp (playerTransform.position, climbLedgZone.position, translateTimer);

            teleportTimer += Time.deltaTime;

            positionDifference = GKC_Utils.distance (playerTransform.position, climbLedgZone.position);

            if ((positionDifference < 0.03f) || teleportTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }
    }

    //when the option to auto hang is active and the player is on first person, the player rotation is adjusted by the camera rotation, looking in the same direction, so the rotation to face the ledge
    //can't be done by input in the player, so instead, the camera is rotated with a coroutine to face the wall of the ledge
    public void rotateTowardLedgeOnFirstPerson ()
    {
        stopClimbLedgeOnFirstPersonMovementCoroutine ();

        rotateToLedgeOnFirstPersonCoroutine = StartCoroutine (rotateTowardLedgeOnFirstPersonCoroutine ());
    }

    public void stopRotateTowardLedgeOnFirstPersonCoroutine ()
    {
        if (rotateToLedgeOnFirstPersonCoroutine != null) {
            StopCoroutine (rotateToLedgeOnFirstPersonCoroutine);
        }
    }

    IEnumerator rotateTowardLedgeOnFirstPersonCoroutine ()
    {
        rotationToLedgeOnFirstPersonActive = true;
        playerRotatedToLedgeOnFirstPerson = true;

        Vector3 currentPlayerRotation = playerTransform.eulerAngles;

        Quaternion targetRotation = Quaternion.LookRotation (-surfaceToHangNormal, playerTransform.up);

        playerTargetRotation = targetRotation.eulerAngles;

        float rotateTimer = 0;

        float normalAngle = 0;

        bool targetReached = false;

        while (!targetReached) {
            rotateTimer += Time.deltaTime * adjustToHoldOnLedgeRotationSpeed;

            mainPlayerCamera.transform.eulerAngles = Vector3.Lerp (currentPlayerRotation, playerTargetRotation, rotateTimer);

            normalAngle = Vector3.SignedAngle (mainPlayerCamera.transform.forward, -surfaceToHangNormal, playerTransform.up);

            if (Mathf.Abs (normalAngle) < 1) {
                targetReached = true;
                rotationToLedgeOnFirstPersonActive = false;
            }

            yield return null;
        }
    }

    //set if a ledge zone system has been detected by trigger
    public void setLedgeZoneFoundState (bool state)
    {
        if (!checkForLedgeZonesActive) {
            return;
        }

        ledgeZoneFound = state;
    }

    //configure new raycast distance for the current ledge zones, usually to make these raycast shorter, for more little ledges in a wall instead of the top part
    public void setNewRaycastDistance (float forwardDistance, float downDistance, Vector3 newCheckDownRaycastOffset)
    {
        if (!checkForLedgeZonesActive) {
            return;
        }

        climbLedgeRayForwardDistance = forwardDistance;
        climbLedgeRayDownDistance = downDistance;
        checkDownRaycastOffset = newCheckDownRaycastOffset;
    }

    //set original distance on raycast
    public void setOriginalRaycastDistance ()
    {
        if (!checkForLedgeZonesActive) {
            return;
        }

        setNewRaycastDistance (originalClimbLedgeRayForwardDistance, originalClimbLedgeRayDownDistance, Vector3.zero);
    }

    //configure if the system can climb a surface right now from a trigger detection on a ledge zone system configured in the level
    public void setCanClimbCurrentLedgeZoneState (bool state)
    {
        canClimbCurrentLedgeZone = state;
    }

    public void setCanGrabAnySurfaceOnAirActiveState (bool state)
    {
        canGrabAnySurfaceOnAirActive = state;
    }

    public void setCanGrabAnySurfaceOnAirState (bool state)
    {
        canGrabAnySurfaceOnAir = state;
    }

    //function called on jump action input by event, so if the player is hanging from a ledge, he can jump from it
    public void inputJumpOnHoldLedge ()
    {
        if (climbLedgeActive && canJumpWhenHoldLedge && (climbingLedge || grabbingSurface) && !activateClimbAction) {
            resumePlayer ();

            bool jumpUpResult = true;

            if (checkMoveInputDirectionOnJump) {
                rawMovementAxis = playerInput.getPlayerRawMovementAxis ();

                if (rawMovementAxis != Vector2.zero) {
                    Vector3 movementDirection = rawMovementAxis.y * playerControllerManager.getCurrentForwardDirection () + rawMovementAxis.x * playerControllerManager.getCurrentRightDirection ();

                    directionAngle = Vector3.SignedAngle (movementDirection, playerTransform.forward, playerTransform.up);

                    if (Mathf.Abs (directionAngle) > 90) {
                        if (showDebugPrint) {
                            print ("pressing in opposite direction to grabbed surface, jumping in opposite direction");
                        }

                        jumpUpResult = false;
                    }
                }

                if (!jumpUpResult) {
                    currentDirectionToRotatePlayer = -playerTransform.forward;

                    Vector3 jumpDirection = playerTransform.forward;

                    bool isFirstPersonActive = mainPlayerCamera.isFirstPersonActive ();

                    if (isFirstPersonActive) {
                        float angleWithCamera = Vector3.SignedAngle (playerTransform.forward, mainPlayerCamera.transform.forward, playerTransform.up);

                        if (Mathf.Abs (angleWithCamera) > 45) {
                            currentDirectionToRotatePlayer = mainPlayerCamera.transform.forward;
                        }
                    }

                    Vector3 totalJumpForce = jumpForceWhenHoldLedgeOnOppositeDirection * (currentDirectionToRotatePlayer + 0.5f * playerTransform.up);

                    playerControllerManager.useJumpPlatform (totalJumpForce, jumpForceMode);

                    rotateCharacterOnJump ();
                }
            }

            if (jumpUpResult) {
                playerControllerManager.useJumpPlatform (jumpForceWhenHoldLedge * playerTransform.up, jumpForceMode);
            }

            checkEventOnLedgeJump ();
        }
    }

    public void inputGrabSurface ()
    {
        if (!canGrabToSurfaceOnAirInputEnabled) {
            return;
        }

        activateGrabToSurfaceAction ();
    }

    public void activateGrabToSurfaceAction ()
    {
        if (canGrabToSurface ()) {

            if (grabToSurfaceInputPaused) {
                return;
            }

            if (!canGrabAnySurfaceOnAirActive) {
                return;
            }

            if (climbLedgeActionPaused) {
                return;
            }

            if (otherClimbActionActive) {
                return;
            }

            if (usingDevicesManager.hasDeviceToUse ()) {
                return;
            }

            enableOrDisableGrabToSurfaceState ();
        }
    }

    public void activateGrabToSurfaceActionExternally (bool state, Transform newLedgeTransform)
    {
        if (state) {
            if (grabbingSurface) {
                if (newLedgeTransform != null && newLedgeTransform == newLedgeTransformToGrabAutomatically) {
                    if (showDebugPrint) {
                        print ("trying to grab to already current surface, ignoring");
                    }

                    return;
                }
            } else {
                if (Time.time < lastTimeLedgeChecked + 0.2f) {
                    if (showDebugPrint) {
                        print ("trying to grab before proper wait time, ignoring");
                    }

                    return;
                }
            }
        }

        callingActivateGrabToSurfaceActionExternally = state;

        if (showDebugPrint) {
            print ("callingActivateGrabToSurfaceActionExternally " + state);
        }

        newLedgeTransformToGrabAutomatically = newLedgeTransform;

        if (callingActivateGrabToSurfaceActionExternally) {
            if (playerControllerManager.isActionActive ()) {
                playerControllerManager.stopAllActionsOnActionSystem ();
            }

            activateGrabToSurfaceAction ();

            callingActivateGrabToSurfaceActionExternally = false;
        }
    }

    public bool canGrabToSurface ()
    {
        if (!canGrabAnySurfaceOnAir) {
            return false;
        }

        if (playerOnGround) {
            return false;
        }

        if (climbingLedge) {
            return false;
        }

        if (pauseAllPlayerDownForces) {
            return false;
        }

        if (wallRunningActive) {
            return false;
        }

        if (freeClimbActive) {
            return false;
        }

        if (swimModeActive) {
            return false;
        }

        if (flyModeActive) {
            return false;
        }

        if (waitingForMinTimeToCheckForLedge) {
            return false;
        }

        return true;
    }

    public void enableOrDisableGrabToSurfaceState ()
    {
        if (grabbingSurface) {
            resumePlayer ();

            return;
        } else {
            if (callingActivateGrabToSurfaceActionExternally) {
                if (playerControllerManager.isPlayerOnGround ()) {
                    callingActivateGrabToSurfaceActionExternally = false;

                    return;
                }

                holdLedgeZone.position = newLedgeTransformToGrabAutomatically.position;
                holdLedgeZone.rotation = newLedgeTransformToGrabAutomatically.rotation;

                //check surfaces in front of him
                surfaceFoundNormal = -newLedgeTransformToGrabAutomatically.forward;

                grabbingSurface = true;

                climbingLedge = false;

                //ground above the ledge is found, so pause the necessary components in the player and activate the grab ledge action

                stopHangFromLedge ();

                setPlayerPauseState ();

                playerControllerManager.setGrabbingToSurfaceActiveState (true);

                callingActivateGrabToSurfaceActionExternally = false;
            } else {
                if (Time.time < playerControllerManager.getLastTimeFalling () + 0.35f) {

                    return;
                }

                if (useGrabSurfaceAmountLimit) {
                    currentGrabSurfaceAmount++;

                    if (currentGrabSurfaceAmount > grabSurfaceAmountLimit) {
                        return;
                    }
                }

                currentRayPosition = climbLedgeRayPosition.position;
                currentRayDirection = climbLedgeRayPosition.forward;

                //check surfaces in front of him
                if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayForwardDistance, layerMaskToCheck)) {
                    if (hit.collider.GetComponent<Rigidbody> ()) {
                        return;
                    } else {
                        surfaceFoundNormal = hit.normal;
                    }
                } else {
                    return;
                }

                if (useSurfacesToCheckTagsOnGrabSurfaceByInput) {
                    if (!surfacesToCheckTagsGrabSurfaceByInputList.Contains (hit.collider.tag)) {
                        return;
                    }
                }

                grabbingSurface = true;

                climbingLedge = false;

                //ground above the ledge is found, so pause the necessary components in the player and activate the grab ledge action

                stopHangFromLedge ();

                setPlayerPauseState ();

                playerControllerManager.setGrabbingToSurfaceActiveState (true);

                //lower the raycast to get the exact edge of the ledge in front of the player
                if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance * 2, layerMaskToCheck)) {
                    holdLedgeZone.position = hit.point + 0.08f * playerTransform.up;

                    if (showGizmo) {
                        Debug.DrawRay (holdLedgeZone.position, climbLedgeRayDownDistance * 2 * hit.normal, Color.black, 10);
                    }
                }
            }

            //adjust the player rotation and position to the ledge found
            adjustHoldOnLedge ();

            canStartToClimbLedge = false;

            lastTimeHoldOnEdge = Time.time;

            //keep player weapons if he was 
            if (keepWeaponsOnLedgeDetected) {
                carryingWeaponsPreviously = weaponsManager.isPlayerCarringWeapon ();

                if (carryingWeaponsPreviously) {
                    weaponsManager.checkIfDisableCurrentWeapon ();
                }
            }

            if (powersManager.isAimingPower ()) {
                powersManager.useAimMode ();
            }

            //call ledge grabbed event
            checkEventOnLedgeGrabbed ();
        }
    }

    public void setPlayerPauseState ()
    {
        playerControllerManager.changeScriptState (false);

        playerControllerManager.setHeadTrackCanBeUsedState (false);

        playerControllerManager.setActionActiveState (true);

        playerControllerManager.setGravityForcePuase (true);

        playerControllerManager.setPlayerOnGroundAnimatorStateOnOverrideOnGround (true);

        playerControllerManager.setCheckOnGroungPausedState (true);

        playerControllerManager.setPlayerVelocityToZero ();

        playerControllerManager.setFootStepManagerState (true);

        mainRigidbody.isKinematic = true;

        playerControllerManager.currentVelocity = Vector3.zero;

        headBobManager.stopAllHeadbobMovements ();
        headBobManager.stopBobTransform ();
        headBobManager.playOrPauseHeadBob (false);

        MainCollider.isTrigger = true;

        mainPlayerCamera.stopShakeCamera ();

        firstPersonActive = playerControllerManager.isPlayerOnFirstPerson ();

        if (!firstPersonActive) {
            animator.SetBool (actionActiveAnimatorID, true);
            animator.CrossFadeInFixedTime (holdOnLedgeActionName, 0.1f);
        }

        currentRayPosition = climbLedgeRayPosition.position;
        currentRayDirection = climbLedgeRayPosition.forward;

        //if a ledge zone is found, check if maybe is a moving platform, to make the player a child of that object while he is grabbed to it
        if (ledgeZoneFound) {

            currentParentAssignedSystem = hit.collider.GetComponent<parentAssignedSystem> ();

            if (currentParentAssignedSystem != null) {
                if (playerControllerManager.isPlayerSetAsChildOfParent ()) {
                    previousParent = playerTransform.parent;
                } else {
                    previousParent = null;
                }

                Transform newParent = currentParentAssignedSystem.getAssignedParent ().transform;

                playerControllerManager.setPlayerAndCameraParent (newParent);

                holdLedgeZone.SetParent (newParent);
                climbLedgZone.SetParent (newParent);
            }
        }

        mainPlayerCamera.setMoveCameraPositionWithMouseWheelActiveState (false);

        playerControllerManager.setUpdate2_5dClampedPositionPausedState (true);

        playerControllerManager.setJumpsAmountValue (0);

        setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = false;

        bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

        if (isFullBodyAwarenessActive) {
            mainPlayerCamera.setPivotCameraTransformParentCurrentTransformToFollow ();

            mainPlayerCamera.setPlayerCameraTransformFollowsPlayerTransformRotationOnFBAState (true);

            setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = true;
        }

        playerControllerManager.setCurrentCustomActionCategoryID (10700);
    }

    bool setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive;

    public void activateGrabLedge ()
    {
        climbingLedge = true;

        //ground above the ledge is found, so pause the necessary components in the player and activate the grab ledge action

        stopHangFromLedge ();

        setPlayerPauseState ();


        bool surfaceRotatedForward = false;

        bool checkProperSurface = false;

        currentRayPosition = climbLedgeRayPosition.position + climbLedgeRayPosition.up + 1.2f * climbLedgeRayPosition.forward;
        currentRayDirection = -climbLedgeRayPosition.up;

        if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance + 0.5f, layerMaskToCheck)) {
            float angleWithSurface = Vector3.SignedAngle (hit.normal, playerTransform.up, playerTransform.right);

            if (Mathf.Abs (angleWithSurface) > 4) {

                if (angleWithSurface < 0) {
                    surfaceRotatedForward = true;
                }

                if (showDebugPrint) {
                    print ("Angle on top " + angleWithSurface + " " + hit.normal);
                    print ("surfaceRotatedForward " + surfaceRotatedForward);
                }

                checkProperSurface = true;
            }
        }

        if (checkProperSurface) {
            if (surfaceRotatedForward) {
                currentRayPosition = climbLedgeRayPosition.position + climbLedgeRayPosition.up + 1.2f * climbLedgeRayPosition.forward;

                currentRayDirection = -climbLedgeRayPosition.forward - 0.1f * climbLedgeRayPosition.up;
            } else {
                currentRayPosition = climbLedgeRayPosition.position - climbLedgeRayPosition.forward + 0.7f * climbLedgeRayPosition.up;

                currentRayDirection = -climbLedgeRayDownPosition.up;
            }
        } else {
            currentRayPosition = climbLedgeRayPosition.position - climbLedgeRayPosition.up * 0.65f;

            currentRayDirection = climbLedgeRayPosition.forward + 0.05f * climbLedgeRayPosition.up;
        }

        //lower the raycast to get the exact edge of the ledge in front of the player
        bool surfaceToLedgeDetected = false;

        int numberOfLoops = 0;

        bool firstSurfaceContactDetected = false;

        while (!surfaceToLedgeDetected) {

            if (checkProperSurface) {
                if (surfaceRotatedForward) {
                    if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                        holdLedgeZone.position = hit.point + 0.08f * playerTransform.up;

                        if (showGizmo) {
                            Debug.DrawLine (currentRayPosition, hit.point, Color.black, 5);
                        }

                        surfaceToLedgeDetected = true;
                    } else {
                        currentRayPosition -= 0.05f * playerTransform.up;

                        if (showGizmo) {
                            Debug.DrawLine (currentRayPosition, currentRayPosition + 3 * currentRayDirection, Color.white, 6);
                        }
                    }
                } else {
                    if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                        holdLedgeZone.position = hit.point + 0.08f * playerTransform.up;

                        if (showGizmo) {
                            Debug.DrawLine (currentRayPosition, hit.point, Color.black, 5);
                        }

                        surfaceToLedgeDetected = true;
                    } else {
                        currentRayPosition += 0.05f * playerTransform.forward;

                        if (showGizmo) {
                            Debug.DrawLine (currentRayPosition, currentRayPosition + 3 * currentRayDirection, Color.white, 6);
                        }
                    }
                }
            } else {
                if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                    holdLedgeZone.position = hit.point + 0.08f * playerTransform.up;

                    currentRayPosition += 0.05f * playerTransform.up;

                    if (showGizmo) {
                        Debug.DrawLine (currentRayPosition, hit.point, Color.black, 5);
                    }

                    firstSurfaceContactDetected = true;
                } else {
                    if (firstSurfaceContactDetected) {
                        surfaceToLedgeDetected = true;
                    } else {
                        currentRayPosition += 0.05f * playerTransform.up;

                        if (showDebugPrint) {
                            print ("no surface found on first check raycast, waiting for first contact");
                        }
                    }

                    if (showGizmo) {
                        Debug.DrawLine (currentRayPosition, currentRayPosition + 3 * currentRayDirection, Color.white, 6);
                    }
                }
            }

            numberOfLoops++;

            if (numberOfLoops > 100) {
                if (!surfaceToLedgeDetected) {
                    print ("WARNING; NO HOLD LEDGE ZONE FOUND, USING DEFAULT ONE TO ADJUST PLAYER");

                }

                surfaceToLedgeDetected = true;
            }
        }


        if (checkProperSurface) {
            currentRayPosition = playerTransform.position + 0.5f * playerTransform.up;
            currentRayDirection = climbLedgeRayPosition.forward;

            if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance + 0.5f, layerMaskToCheck)) {
                surfaceFoundNormal = hit.normal;
            }
        }


        //lower the raycast to get the exact edge of the ledge in the surface above that ledge
        currentRayPosition = climbLedgeRayPosition.position + 0.4f * playerTransform.up;
        currentRayDirection = -climbLedgeRayDownPosition.up;

        surfaceToLedgeDetected = false;
        numberOfLoops = 0;

        while (!surfaceToLedgeDetected) {

            if (Physics.Raycast (currentRayPosition, currentRayDirection, out hit, climbLedgeRayDownDistance, layerMaskToCheck)) {
                climbLedgZone.position = hit.point;

                Vector3 currentOffset = climbLedgeTargetPositionOffsetThirdPerson;

                if (firstPersonActive) {
                    currentOffset = climdLedgeTargetPositionOffsetFirstPerson;
                } else {
                    if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        currentOffset = climdLedgeTargetPositionOffsetFirstFBA;
                    }
                }

                climbLedgZone.position += currentOffset.x * playerTransform.right +
                currentOffset.y * playerTransform.up +
                currentOffset.z * playerTransform.forward;

                surfaceToLedgeDetected = true;
            } else {
                currentRayPosition += 0.05f * playerTransform.forward;
            }

            numberOfLoops++;

            if (numberOfLoops > 100) {
                surfaceToLedgeDetected = true;
            }
        }

        //adjust the player rotation and position to the ledge found
        adjustHoldOnLedge ();

        canStartToClimbLedge = false;

        lastTimeHoldOnEdge = Time.time;

        //keep player weapons if he was 
        if (keepWeaponsOnLedgeDetected) {
            carryingWeaponsPreviously = weaponsManager.isUsingWeapons ();

            if (carryingWeaponsPreviously) {
                weaponsManager.checkIfDisableCurrentWeapon ();
            }
        }

        if (powersManager.isAimingPower ()) {
            powersManager.useAimMode ();
        }

        //call ledge grabbed event
        checkEventOnLedgeGrabbed ();
    }

    public void setClimbLedgeActiveState (bool state)
    {
        climbLedgeActive = state;
    }

    public void setClimbLedgeActiveStateFromEditor (bool state)
    {
        setClimbLedgeActiveState (state);

        updateComponent ();
    }

    //events for grab, climb and lose ledge
    public void checkEventOnLedgeGrabbed ()
    {
        if (useEvents) {
            eventOnLedgeGrabbed.Invoke ();
        }
    }

    public void checkEventOnLedgeLose ()
    {
        if (useEvents) {
            eventOnLedgeLose.Invoke ();
        }
    }

    public void checkEventOnLedgeClimbed ()
    {
        if (useEvents) {
            eventOnLedgeClimbed.Invoke ();
        }
    }

    public void checkEventOnStartLedgeClimb ()
    {
        if (useEvents) {
            eventOnStartLedgeClimb.Invoke ();
        }
    }

    public void checkEventOnLedgeJump ()
    {
        if (useEvents) {
            eventOnLedgeJump.Invoke ();
        }
    }

    //a ledge zone system configured in a trigger is configured to avoid the player to grab a ledge
    public void setAvoidPlayerGrabLedgeValue (bool state)
    {
        avoidPlayerGrabLedge = state;
    }

    //stop the auto hang from ledge action
    public void stopHangFromLedge ()
    {
        playerInput.overrideInputValues (Vector2.zero, false);
        playerControllerManager.overrideMainCameraTransformDirection (null, false);
        movingTowardSurfaceToHang = false;
        surfaceToHangOnGroundFound = false;

        lastTimeVerticalInputPressed = 0;
        movementInputPressed = false;
        movementInputReleased = false;

        onAirWhileSearchingLedgeToHang = false;

        rotationToLedgeOnFirstPersonActive = false;
        playerRotatedToLedgeOnFirstPerson = false;

        if (useHangFromLedgeIcon) {
            enableOrDisableHangFromLedgeIcon (false);
        }
    }

    public void setCanCheckForHangFromLedgeOnGroundState (bool state)
    {
        canCheckForHangFromLedgeOnGround = state;
    }

    public void setOnlyHangFromLedgeIfPlayerIsNotMovingValue (bool state)
    {
        onlyHangFromLedgeIfPlayerIsNotMoving = state;
    }

    public void setOnlyHangFromLedgeIfPlayerIsNotMovingOriginalValue ()
    {
        onlyHangFromLedgeIfPlayerIsNotMoving = originalOnlyHangFromLedgeIfPlayerIsNotMovingValue;
    }

    public void setClimbLedgeActionPausedState (bool state)
    {
        climbLedgeActionPaused = state;
    }

    public void setOtherClimbActionActiveState (bool state)
    {
        otherClimbActionActive = state;
    }

    public void setGrabToSurfaceInputPausedState (bool state)
    {
        grabToSurfaceInputPaused = state;
    }

    public void stopSetClimbLedgeActionPausedStateWithDurationCoroutine ()
    {
        if (climbLedgeActionPausedCoroutine != null) {
            StopCoroutine (climbLedgeActionPausedCoroutine);
        }
    }

    public void setClimbLedgeActionPausedStateWithDuration (float pauseDuration)
    {
        stopSetClimbLedgeActionPausedStateWithDurationCoroutine ();

        climbLedgeActionPausedCoroutine = StartCoroutine (setClimbLedgeActionPausedStateWithDurationCoroutine (pauseDuration));
    }

    IEnumerator setClimbLedgeActionPausedStateWithDurationCoroutine (float pauseDuration)
    {
        setClimbLedgeActionPausedState (true);

        avoidPlayerGrabLedge = true;

        yield return new WaitForSeconds (pauseDuration);

        setClimbLedgeActionPausedState (false);

        avoidPlayerGrabLedge = false;
    }

    Coroutine jumpCoroutine;

    public void rotateCharacterOnJump ()
    {
        stopRotateCharacterOnJumpCoroutine ();

        jumpCoroutine = StartCoroutine (rotateCharacterOnJumpCoroutine ());
    }

    void stopRotateCharacterOnJumpCoroutine ()
    {
        if (jumpCoroutine != null) {
            StopCoroutine (jumpCoroutine);
        }
    }

    public IEnumerator rotateCharacterOnJumpCoroutine ()
    {
        bool targetReached = false;

        float movementTimer = 0;

        float t = 0;

        float duration = 0;

        bool isFirstPersonActive = mainPlayerCamera.isFirstPersonActive ();

        if (isFirstPersonActive) {
            duration = 0.5f / jumpRotationSpeedFirstPerson;
        } else {
            duration = 0.5f / jumpRotationSpeedThirdPerson;
        }

        float angleDifference = 0;

        bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

        Transform objectToRotate = playerTransform;

        if (isFirstPersonActive || isFullBodyAwarenessActive) {
            objectToRotate = mainPlayerCamera.transform;
        }

        Quaternion targetRotation = Quaternion.LookRotation (currentDirectionToRotatePlayer, objectToRotate.up);

        while (!targetReached) {
            t += Time.deltaTime / duration;

            objectToRotate.rotation = Quaternion.Slerp (objectToRotate.rotation, targetRotation, t);

            angleDifference = Quaternion.Angle (objectToRotate.rotation, targetRotation);

            movementTimer += Time.deltaTime;

            if (angleDifference < 0.2f || movementTimer > (duration + 1)) {
                targetReached = true;
            }
            yield return null;
        }
    }


    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Update Climb Ledge System", gameObject);
    }

    //gizmo settings
    void OnDrawGizmos ()
    {
        if (!showGizmo) {
            return;
        }

        if (GKC_Utils.isCurrentSelectionActiveGameObject (gameObject)) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected ()
    {
        DrawGizmos ();
    }

    void DrawGizmos ()
    {
        if (showGizmo) {
            Gizmos.color = gizmoColor;

            Gizmos.DrawSphere (holdLedgeZone.position, gizmoRadius);
        }
    }
}