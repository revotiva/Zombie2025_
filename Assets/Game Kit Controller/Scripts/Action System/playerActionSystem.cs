﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class playerActionSystem : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public string actionActiveAnimatorName = "Action Active";
    public string actionIDAnimatorName = "Action ID";

    public string actionActiveUpperBodyAnimatorName = "Action Active Upper Body";

    public string horizontalAnimatorName = "Horizontal Action";
    public string verticalAnimatorName = "Vertical Action";

    public string rawHorizontalAnimatorName = "Raw Horizontal Action";
    public string rawVerticalAnimatorName = "Raw Vertical Action";

    public string lastHorizontalDirectionAnimatorName = "Last Horizontal Direction";
    public string lastVerticalDirectionAnimatorName = "Last Vertical Direction";

    public string disableHasExitTimeAnimatorName = "Disable Has Exit Time State";

    public float inputLerpSpeed = 0.1f;

    public int actionsLayerIndex;

    public bool customActionStatesEnabled = true;

    public bool changeCameraViewToThirdPersonOnActionOnAnyAction;

    [Space]
    [Header ("Action State Info Settings")]
    [Space]

    public List<actionStateInfo> actionStateInfoList = new List<actionStateInfo> ();

    [Space]
    [Header ("Custom Action State Info Settings")]
    [Space]

    public int currentCustomActionCategoryID = 0;

    [Space]
    [Space]

    [TextArea (3, 20)]
    public string explanation = "After adding, removing or modifying the info of the custom actions, press the button to UPDATE ACTION LIST " +
                                                  "on the bottom of this component.";

    [Space]
    [Space]

    public List<customActionStateCategoryInfo> customActionStateCategoryInfoList = new List<customActionStateCategoryInfo> ();

    [Space]
    [Header ("Player Input To Pause During Action Settings")]
    [Space]

    public bool pauseInputListDuringActionActiveAlways = true;
    public List<inputToPauseOnActionIfo> inputToPauseOnActionIfoList = new List<inputToPauseOnActionIfo> ();

    [Space]
    [Header ("Other Settings")]
    [Space]

    public bool setInitialActionSystem;
    public actionSystem initialActionSystemToSet;

    public bool useInitialCustomActionName;
    public string initialCustomActionName;

    [Space]

    public bool checkIfActionTakesLongerDurationToResume;

    public float maxWaitToResumeIfAnimationTakesLongerDuration = 0.8f;

    [Space]
    [Header ("Events Settings")]
    [Space]

    public UnityEvent eventOnStartAction;
    public UnityEvent eventOnEndAction;

    [Space]

    public bool useEventOnActionDetected;
    public UnityEvent eventOnActionDetected;

    [Space]

    public UnityEvent eventBeforeStartAction;
    public UnityEvent eventAfterEndAction;

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;

    public bool showDebugPrintCustomActionActivated;

    public bool actionFound;
    public bool actionActive;

    public bool playingAnimation;
    public int currentActionInfoIndex;

    public bool waitingForNextPlayerInput;

    public bool actionMovementInputActive;

    public float horizontalInput;
    public float verticalInput;

    public int rawHorizontalInput;
    public int rawVerticalInput;

    public Vector2 axisValues;
    public Vector2 rawAxisValues;

    public bool currentAnimationChecked;

    public bool startAction;

    public bool carryingWeaponsPreviously;
    public bool usingPowersPreviosly;

    public bool aimingWeaponsPrevously;

    public bool checkConditionsActive;

    public string currentCustomActionName;
    public bool customActionActive;

    public bool actionFoundWaitingToPlayerResume;

    public Vector3 currentWalkDirection;
    public bool walkingToDirectionActive;

    public string currentActionName;

    public bool ignoreInputChangeActive;

    public float currentActionDuration;

    [Space]
    [Space]

    public bool rotatingTowardFacingDirection;
    public bool movingPlayerToPositionTargetActive;

    public bool usePositionToAdjustPlayerActive;

    public bool waitToPlayAnimationAfterAdjustPositionActive;

    [Space]
    [Space]

    public actionSystem.actionInfo currentActionInfo;
    public Transform currentActionSystemTransform;

    public GameObject currentActionSystemGameObject;

    public actionSystem actionFoundWaitingResume;

    public List<actionSystem> actionSystemListStoredToPlay = new List<actionSystem> ();

    public List<customActionStateInfoDictionary> customActionStateInfoDictionaryList = new List<customActionStateInfoDictionary> ();

    [Space]
    [Header ("Gizmo Settings")]
    [Space]

    public bool showGizmo;

    [Space]
    [Header ("Components")]
    [Space]

    public Transform playerTransform;
    public Animator mainAnimator;
    public playerController mainPlayerController;
    public playerCamera mainPlayerCamera;
    public Collider mainCollider;
    public Rigidbody mainRigidbody;
    public headBob mainHeadbob;
    public headTrack mainHeadTrack;
    public playerInputManager mainPlayerInputManager;
    public IKFootSystem mainIKFootSystem;
    public handsOnSurfaceIKSystem mainHandsOnSurfaceIKSystem;
    public remoteEventSystem mainRemoteEventSystem;
    public menuPause pauseManager;
    public playerWeaponsManager mainPlayerWeaponsManager;
    public otherPowers mainOtherPowers;
    public usingDevicesSystem mainUsingDevicesSystem;
    public playerNavMeshSystem mainPlayerNavMeshSystem;
    public AINavMesh mainAINavmesh;
    public findObjectivesSystem mainFindObjectivesSystem;
    public health mainHealth;

    public sharedActionSystem mainSharedActionSystem;
    public sharedActionButtonActivator mainSharedActionButtonActivator;
    public sharedActionSystemRemoteActivator mainSharedActionSystemRemoteActivator;


    int actionActiveAnimatorID;
    int actionIDAnimatorID;

    int actionActiveUpperBodyAnimatorID;

    int disableHasExitTimeAnimatorID;

    actionSystem currentActionSystem;

    float lastTimeAnimationPlayed;

    float currentAngleWithTarget;
    float currentDistanceToTarget;

    Coroutine animationCoroutine;

    Coroutine playerWalkCoroutine;

    Coroutine objectParentCoroutine;

    float lastTimePlayerOnGround;

    bool playerOnGround;

    int horizontalAnimatorID;
    int verticalAnimatorID;

    int rawHorizontalAnimatorID;
    int rawVerticalAnimatorID;

    int lastHorizontalDirectionAnimatorID;
    int lastVerticalDirectionAnimatorID;

    int lastHorizontalDirection;
    int lastVerticalDirection;

    bool hudDisabledDuringAction;

    bool pausePlayerActivated;

    bool pausePlayerInputDuringWalk;

    string previousCameraStateName = "";

    string currentActionCategoryActive = "";

    bool usingAINavmesh;

    bool strafeModeActivePreviously;

    bool firstPersonViewPreviouslyActive;

    bool fullBodyAwarenessPreviusolyActive;

    bool useExtraFollowTransformPositionOffsetActiveFBAPreviouslyActive;

    bool ignorePlayerRotationToCameraOnFBA;

    bool playerCameraTransformFollowsPlayerTransformRotationOnFBA;

    bool ignoreHorizontalCameraRotationOnFBA;

    bool ignoreVerticalCameraRotationOnFBA;


    bool setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive;

    bool newHeadTrackTargetUsed;

    bool dropGrabbedObjectsOnActionActivatedPreviously;

    bool initialStateChecked;

    bool actionActivatedOnFirstPerson;

    bool eventInfoListActivatedOnFirstPerson;

    bool isFirstPersonActive;

    bool readInputValuesForActionSystemActiveState;

    bool playerCrouchingPreviously;

    Coroutine disableHasExitTimeCoroutine;

    Coroutine movePlayerCoroutine;
    bool movePlayerOnDirectionActive;
    bool navmeshUsedOnAction;

    bool navmeshPreviouslyActive;
    Coroutine eventInfoListCoroutine;

    Coroutine killOrEnterOnRagdollStateCoroutine;

    bool activateCustomActionsPaused;
    bool pauseInputListChecked;

    Coroutine movePlayerToTargetCoroutine;

    void Awake ()
    {
        actionActiveAnimatorID = Animator.StringToHash (actionActiveAnimatorName);
        actionIDAnimatorID = Animator.StringToHash (actionIDAnimatorName);

        actionActiveUpperBodyAnimatorID = Animator.StringToHash (actionActiveUpperBodyAnimatorName);

        horizontalAnimatorID = Animator.StringToHash (horizontalAnimatorName);
        verticalAnimatorID = Animator.StringToHash (verticalAnimatorName);

        lastHorizontalDirectionAnimatorID = Animator.StringToHash (lastHorizontalDirectionAnimatorName);
        lastVerticalDirectionAnimatorID = Animator.StringToHash (lastVerticalDirectionAnimatorName);

        rawHorizontalAnimatorID = Animator.StringToHash (rawHorizontalAnimatorName);
        rawVerticalAnimatorID = Animator.StringToHash (rawVerticalAnimatorName);

        disableHasExitTimeAnimatorID = Animator.StringToHash (disableHasExitTimeAnimatorName);
    }

    void Start ()
    {
        pausePlayerActivated = false;

        usingAINavmesh = mainAINavmesh != null;

        if (customActionStateInfoDictionaryList.Count == 0) {
            updateActionList (true);
        }

        if (usingAINavmesh) {
            if (mainFindObjectivesSystem == null) {
                mainFindObjectivesSystem = playerTransform.gameObject.GetComponent<findObjectivesSystem> ();
            }
        }
    }

    void FixedUpdate ()
    {
        if (!initialStateChecked) {
            if (setInitialActionSystem) {
                if (useInitialCustomActionName) {
                    activateCustomAction (initialCustomActionName);
                } else {
                    initialActionSystemToSet.activateCustomAction (playerTransform.gameObject);
                }
            }

            initialStateChecked = true;
        }

        if (actionFound) {

            if (actionMovementInputActive) {
                getInputValuesForActionSystem ();
            }

            if (!actionActive) {
                if (checkConditionsActive) {
                    if (checkConditionsToPlayAnimation ()) {
                        playAnimation ();

                        checkConditionsActive = false;
                    }
                }
            }

            if (actionActive) {
                if (currentActionInfo.stopActionIfPlayerIsOnAir) {
                    playerOnGround = mainPlayerController.isPlayerOnGround ();

                    if (playerOnGround) {
                        lastTimePlayerOnGround = Time.time;
                    }

                    if (!playerOnGround) {
                        if (Time.time > lastTimePlayerOnGround + currentActionInfo.delayToStopActionIfPlayerIsOnAir) {
                            stopAllActions ();
                        }
                    }
                }

                if (checkIfActionTakesLongerDurationToResume) {
                    if (currentActionDuration != 0 && actionActive) {
                        if (Time.time > lastTimeAnimationPlayed + currentActionDuration + maxWaitToResumeIfAnimationTakesLongerDuration) {
                            if (!movePlayerOnDirectionActive &&
                                !walkingToDirectionActive &&
                                !movingPlayerToPositionTargetActive &&
                                currentActionSystem.actionInfoList.Count == 1) {

                                if (showDebugPrint) {
                                    print ("current action finished but character not resuming state, forcing stop to resume");
                                }

                                stopAllActions ();

                                return;
                            }
                        }
                    }
                }
            }

            if (actionActivatedOnFirstPerson) {
                updateOnAnimatorMove ();
            }
        }

        if (readInputValuesForActionSystemActiveState) {
            getInputValuesForActionSystem ();
        }
    }

    public float getCurrentActionDuration ()
    {
        if (!actionActive || currentActionInfo == null) {
            return 0;
        }

        float currentAnimationDuration = 0;

        if (actionActivatedOnFirstPerson) {
            currentAnimationDuration = currentActionInfo.actionDurationOnFirstPerson;
        } else {
            currentAnimationDuration = currentActionInfo.animationDuration;
        }

        float animationDuration = currentAnimationDuration / currentActionInfo.animationSpeed;

        float mainAnimatorSpeed = mainAnimator.speed;

        if (mainAnimatorSpeed < 1) {
            animationDuration /= mainAnimatorSpeed;
        }

        return animationDuration + currentActionInfo.delayToPlayAnimation;
    }

    public void setReadInputValuesForActionSystemActiveState (bool state)
    {
        readInputValuesForActionSystemActiveState = state;
    }

    public void getInputValuesForActionSystem ()
    {
        axisValues = mainPlayerInputManager.getPlayerMovementAxis ();

        horizontalInput = axisValues.x;
        verticalInput = axisValues.y;

        rawAxisValues = mainPlayerInputManager.getPlayerRawMovementAxis ();

        rawHorizontalInput = (int)rawAxisValues.x;
        rawVerticalInput = (int)rawAxisValues.y;

        if (rawHorizontalInput != 0) {
            if (horizontalInput < -0.01f) {
                lastHorizontalDirection = -1;
            } else {
                lastHorizontalDirection = 1;
            }
        }

        if (rawVerticalInput != 0) {
            if (verticalInput < -0.01f) {
                lastVerticalDirection = -1;
            } else {
                lastVerticalDirection = 1;
            }
        }

        updateAnimatorParemeters ();
    }

    public void updateAnimatorParemeters ()
    {
        if (currentCustomActionCategoryID == 0) {
            mainAnimator.SetFloat (horizontalAnimatorID, horizontalInput, inputLerpSpeed, Time.fixedDeltaTime);
            mainAnimator.SetFloat (verticalAnimatorID, verticalInput, inputLerpSpeed, Time.fixedDeltaTime);

            mainAnimator.SetInteger (lastHorizontalDirectionAnimatorID, lastHorizontalDirection);
            mainAnimator.SetInteger (lastVerticalDirectionAnimatorID, lastVerticalDirection);

            mainAnimator.SetInteger (rawHorizontalAnimatorID, rawHorizontalInput);
            mainAnimator.SetInteger (rawVerticalAnimatorID, rawVerticalInput);
        }
    }

    public void setPlayerActionActive (actionSystem newActionSystem)
    {
        if (actionActive) {
            if (showDebugPrint) {
                print ("action active, checking state for new action " + newActionSystem.name);
            }

            if (currentActionSystem != null && currentActionSystem == newActionSystem) {
                if (showDebugPrint) {
                    print (newActionSystem.name + " is already the current action system, avoiding to activate it again");
                }

                return;
            }

            bool currentActionCanbeStopped = newActionSystem.canStopPreviousAction;

            if (!currentActionCanbeStopped) {
                if (newActionSystem.canInterruptOtherActionActive && currentActionSystem != null) {
                    if (newActionSystem.useCategoryToCheckInterrupt) {
                        if (newActionSystem.actionCategoryListToInterrupt.Contains (currentActionSystem.categoryName)) {
                            newActionSystem.eventOnInterrupOtherActionActive.Invoke ();

                            if (currentActionSystem.useEventOnInterruptedAction) {
                                currentActionSystem.eventOnInterruptedAction.Invoke ();
                            }

                            currentActionCanbeStopped = true;

                            if (showDebugPrint) {
                                print ("action " + newActionSystem.name + " can force stop " + currentActionSystem.getCurrentactionInfo ().Name
                                + " by category " + currentActionSystem.categoryName);
                            }
                        }
                    } else {
                        if (newActionSystem.actionListToInterrupt.Contains (currentActionSystem.getCurrentactionInfo ().Name)) {
                            newActionSystem.eventOnInterrupOtherActionActive.Invoke ();

                            currentActionCanbeStopped = true;
                        }
                    }
                }
            }

            if (!currentActionCanbeStopped) {
                actionFoundWaitingToPlayerResume = true;

                actionFoundWaitingResume = newActionSystem;

                if (showDebugPrint) {
                    print ("current Action Can't be Stopped " + currentActionName);
                }

                return;
            } else {
                stopCheckActionEventInfoList ();

                stopCheckKillOrEnterOnRagdollState ();
            }
        }

        axisValues = Vector2.zero;

        horizontalInput = 0;
        verticalInput = 0;

        rawAxisValues = Vector2.zero;

        rawHorizontalInput = 0;
        rawVerticalInput = 0;

        updateAnimatorParemeters ();

        currentActionSystem = newActionSystem;

        if (currentActionSystem.clearAddActionToListStoredToPlay) {
            clearActionSystemListStoredToPlay ();
        }

        if (currentActionSystem.addActionToListStoredToPlay) {

            if (!actionSystemListStoredToPlay.Contains (currentActionSystem)) {
                actionSystemListStoredToPlay.Add (currentActionSystem);
            }
        }

        if (newActionSystem != null) {
            currentActionSystemGameObject = newActionSystem.gameObject;
        }

        currentActionInfo = currentActionSystem.getCurrentactionInfo ();

        currentActionName = currentActionInfo.Name;

        if (showDebugPrint) {
            print ("Assigning new current action info " + currentActionName);
        }

        currentActionInfoIndex = currentActionSystem.getCurrentActionInfoIndex ();

        currentActionSystemTransform = currentActionSystem.actionTransform;

        setActionFoundState (true);

        checkConditionsActive = false;

        bool animationCanBeplayed = false;

        bool canPlayAnimationCheck = canPlayAnimation ();

        if (showDebugPrint) {
            print ("Result of check if can play animation " + canPlayAnimationCheck);
        }

        if (!currentActionInfo.animationTriggeredByExternalEvent && !currentActionInfo.useInteractionButtonToActivateAnimation && canPlayAnimationCheck) {
            if (currentActionInfo.checkConditionsToStartActionOnUpdate) {
                checkConditionsActive = true;

                return;
            }

            playAnimation ();

            animationCanBeplayed = true;
        }

        if (useEventOnActionDetected) {
            eventOnActionDetected.Invoke ();
        }

        if (showDebugPrint) {
            if (animationCanBeplayed) {
                print ("Animation can be played " + currentActionName);
            } else {
                print ("Animation can't be played " + currentActionName);
            }
        }
    }

    public bool checkConditionsToPlayAnimation ()
    {
        if (actionFound && currentActionInfo != null) {
            if (currentActionInfo.checkConditionsToStartActionOnUpdate) {
                if (currentActionInfo.playerMovingToStartAction) {
                    if (!mainPlayerController.isPlayerMoving (0.4f)) {
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    public void setPlayerActionDeactivate (actionSystem newActionSystem)
    {
        if (!actionActive) {
            checkEmptyActionSystemListStored ();

            if (actionSystemListStoredToPlay.Count > 0) {
                if (actionSystemListStoredToPlay.Contains (newActionSystem)) {
                    actionSystemListStoredToPlay.Remove (newActionSystem);
                }
            }

            if (actionFoundWaitingToPlayerResume && actionFoundWaitingResume == newActionSystem) {
                removeActionFoundWaitingToPlayerResume ();
            }

            setActionFoundState (false);
        }
    }

    public void setPlayerActionDeactivate ()
    {
        if (!actionActive) {
            setActionFoundState (false);
        }
    }

    void setActionFoundState (bool state)
    {
        actionFound = state;
    }

    public void playAnimation ()
    {
        if (!currentActionSystem.actionsCanBeUsedOnFirstPerson) {
            if (mainPlayerController.isPlayerOnFirstPerson () && !currentActionSystem.changeCameraViewToThirdPersonOnAction && !changeCameraViewToThirdPersonOnActionOnAnyAction) {
                if (showDebugPrint) {
                    print ("The action system is only used on third person for now. In the next update, the actions will be done in first person as well");
                }

                return;
            }
        }

        if (currentActionInfo.setPreviousCrouchStateOnActionEnd) {
            playerCrouchingPreviously = mainPlayerController.isCrouching ();
        }

        if (currentActionInfo.getUpIfPlayerCrouching) {
            mainPlayerController.setCrouchState (false);
        }

        eventBeforeStartAction.Invoke ();

        currentActionSystem.checkEventBeforeStartAction ();

        currentActionSystem.checkSendCurrentPlayerOnEvent (playerTransform.gameObject);

        navmeshUsedOnAction = false;

        if (currentActionInfo.usePlayerWalkTarget && !currentActionInfo.useWalkAtTheEndOfAction) {
            setPlayerWalkState (false);
        } else {
            activateAnimation ();
        }

        if (usingAINavmesh) {
            if (currentActionInfo.pauseAIOnActionStart) {
                mainAINavmesh.pauseAI (true);

                mainFindObjectivesSystem.checkPauseOrResumePatrolStateDuringActionActive (true);
            }

            if (currentActionInfo.setMoveNavMeshPaused) {
                mainAINavmesh.setMoveNavMeshPausedDuringActionState (true);
            }
        }

        if (currentActionSystem != null) {
            currentActionSystem.checkEventOnStartAction ();
        }
    }

    public void stopPlayAnimationCoroutine ()
    {
        if (animationCoroutine != null) {
            StopCoroutine (animationCoroutine);
        }
    }

    public void activateAnimation ()
    {
        playingAnimation = false;

        currentActionDuration = 0;

        stopPlayAnimationCoroutine ();

        animationCoroutine = StartCoroutine (playAnimationCoroutine ());

        pausePlayer ();

        currentAnimationChecked = false;

        if (showDebugPrint) {
            print (currentAnimationChecked);
        }

        waitingForNextPlayerInput = false;

        if (currentActionInfo.useMovingPlayerToPositionTarget) {
            movePlayerToPositionTarget (currentActionInfo.matchTargetTransform);
        }
    }

    IEnumerator playAnimationCoroutine ()
    {
        if (showDebugPrint) {
            print ("start coroutine " + playerTransform.name + " " + Time.time);
        }

        if (showDebugPrint) {
            print ("check to face direction " + Time.time);
        }

        bool isFullBodyAwarenessActive = mainPlayerCamera.isFullBodyAwarenessActive ();

        bool canSetPlayerFacingDirectionResult = true;

        if (isFullBodyAwarenessActive && currentActionInfo.ignoreSetPlayerFacingDirectionOnFBA) {
            canSetPlayerFacingDirectionResult = false;
        }

        if (currentActionInfo.setPlayerFacingDirection && canSetPlayerFacingDirectionResult) {
            if (currentActionInfo.adjustRotationAtOnce) {
                playerTransform.rotation = currentActionInfo.playerFacingDirectionTransform.rotation;

                if (isFullBodyAwarenessActive) {
                    mainPlayerCamera.transform.rotation = playerTransform.rotation;
                }
            } else {

                rotatingTowardFacingDirection = true;

                bool previousStrafeMode = mainPlayerController.isStrafeModeActive ();

                mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (false);

                bool targetReached = false;

                Transform currentFacingDirectionTransform = currentActionInfo.playerFacingDirectionTransform;

                if (currentFacingDirectionTransform != null) {
                    if (currentActionInfo.adjustFacingDirectionBasedOnPlayerPosition) {
                        currentFacingDirectionTransform.position = playerTransform.position;

                        currentFacingDirectionTransform.LookAt (currentActionInfo.facingDirectionPositionTransform);
                    }

                    int c = 0;

                    while (!targetReached) {

                        float turnAmount = 0;

                        float angle = Vector3.SignedAngle (playerTransform.forward, currentFacingDirectionTransform.forward, playerTransform.up);

                        if (Mathf.Abs (angle) > currentActionInfo.minRotationAngle) {
                            turnAmount = angle * Mathf.Deg2Rad;

                            turnAmount = Mathf.Clamp (turnAmount, -currentActionInfo.maxRotationAmount, currentActionInfo.maxRotationAmount);
                        } else {
                            turnAmount = 0;
                        }

                        float turnAmountToApply = turnAmount;
                        if (turnAmountToApply < 0) {
                            if (turnAmountToApply > -currentActionInfo.minRotationAmount) {
                                turnAmountToApply = -currentActionInfo.minRotationAmount;
                            }
                        } else {
                            if (turnAmountToApply < currentActionInfo.minRotationAmount) {
                                turnAmountToApply = currentActionInfo.minRotationAmount;
                            }
                        }

                        if (isFullBodyAwarenessActive) {
                            mainPlayerCamera.transform.rotation = playerTransform.rotation;
                        }

                        mainPlayerController.setOverrideTurnAmount (turnAmountToApply, true);

                        if (turnAmount == 0) {
                            targetReached = true;
                        }

                        c++;

                        if (c >= 100000) {
                            targetReached = true;

                            if (showDebugPrint) {
                                print ("too much loops, ending rotation");
                            }
                        }

                        yield return null;
                    }

                    mainPlayerController.setOverrideTurnAmount (0, false);

                    if (!currentActionInfo.pauseStrafeState) {
                        mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (previousStrafeMode);
                    }
                }

                rotatingTowardFacingDirection = false;
            }
        }

        if (showDebugPrint) {
            print ("check to adjust position " + Time.time);
        }

        usePositionToAdjustPlayerActive = false;

        bool canUsePositionToAdjustPlayerResult = true;

        if (currentActionInfo.usePositionToAdjustPlayerOnlyOnFBA && !isFullBodyAwarenessActive) {
            canUsePositionToAdjustPlayerResult = false;
        }

        if (canUsePositionToAdjustPlayerResult &&
            currentActionInfo.usePositionToAdjustPlayer &&
            currentActionInfo.positionToAdjustPlayer != null) {

            if (currentActionInfo.useRaycastToAdjustPositionToAdjustPlayer) {

                RaycastHit hit;

                if (Physics.Raycast (currentActionInfo.positionToAdjustPlayer.position + Vector3.up, -Vector3.up, out hit, 2, currentActionInfo.layerForRaycast)) {
                    currentActionInfo.positionToAdjustPlayer.position = hit.point + 0.01f * Vector3.up;
                }
            }

            usePositionToAdjustPlayerActive = true;

            Transform targetTransform = currentActionInfo.positionToAdjustPlayer;

            float dist = GKC_Utils.distance (playerTransform.position, targetTransform.position);

            if (dist <= 0) {
                dist = 1;
            }

            float duration = dist / currentActionInfo.adjustPlayerPositionSpeed;

            float t = 0;

            Vector3 targetPosition = targetTransform.position;
            Quaternion targetRotation = targetTransform.rotation;

            float movementTimer = 0;

            bool targetReached = false;

            float angleDifference = 0;
            float positionDifference = 0;

            while (!targetReached) {
                t += Time.deltaTime / duration;

                if (currentActionInfo.userLerpToAdjustPosition) {
                    playerTransform.position = Vector3.Lerp (playerTransform.position, targetPosition, t);
                    playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, targetRotation, t);
                } else {
                    playerTransform.position = Vector3.Slerp (playerTransform.position, targetPosition, t);
                    playerTransform.rotation = Quaternion.Slerp (playerTransform.rotation, targetRotation, t);
                }

                if (isFullBodyAwarenessActive) {
                    mainPlayerCamera.transform.rotation = playerTransform.rotation;
                }

                angleDifference = Quaternion.Angle (playerTransform.rotation, targetRotation);
                positionDifference = GKC_Utils.distance (playerTransform.position, targetPosition);

                movementTimer += Time.deltaTime;

                if (positionDifference < 0.01f && angleDifference < 0.2f) {
                    targetReached = true;
                }

                if (movementTimer > (duration + 0.3f)) {
                    targetReached = true;
                }

                yield return null;
            }

            usePositionToAdjustPlayerActive = false;
        }

        if (waitToPlayAnimationAfterAdjustPositionActive) {
            while (waitToPlayAnimationAfterAdjustPositionActive) {
                yield return null;
            }
        }

        if (currentActionInfo.movePlayerOnDirection) {
            stopMovePlayerOnDirectionCorotuine ();

            movePlayerCoroutine = StartCoroutine (movePlayerOnDirectionCoroutine ());
        }

        if (showDebugPrint) {
            print ("continue with coroutine " + Time.time);
        }

        //		if (currentActionInfo.delayToPlayAnimation > 0) {
        //			lastTimeAnimationPlayed = Time.time;
        //		}

        startAction = false;

        if (showDebugPrint) {
            print (playerTransform + " activating delay " + currentActionInfo.delayToPlayAnimation);
        }

        if (currentActionInfo.delayToPlayAnimation > 0) {
            WaitForSeconds delay = new WaitForSeconds (currentActionInfo.delayToPlayAnimation);

            yield return delay;
        }

        if (!currentActionInfo.ignoreAnimationTransitionCheck) {
            float timer = 0;

            bool canContinue = false;

            while (!canContinue) {

                timer += Time.deltaTime;

                canContinue = !mainAnimator.IsInTransition (actionsLayerIndex);

                if (timer > 5) {
                    canContinue = true;
                }

                yield return null;
            }
        }

        if (showDebugPrint) {
            print (playerTransform.name + " set last time animation played for " + currentActionInfo.actionName + " " + Time.time);
        }

        stopCheckActionEventInfoList ();

        checkActionEventInfoList ();

        checkSetObjectParent ();

        stopCheckKillOrEnterOnRagdollState ();

        checkKillOrEnterOnRagdollState ();

        startAction = false;

        lastTimeAnimationPlayed = Time.time;

        currentActionDuration = getCurrentActionDuration ();

        startAction = true;

        checkSetActionState ();

        if (currentActionInfo.useRaycastToAdjustMatchTransform) {
            RaycastHit hit;

            if (Physics.Raycast (currentActionInfo.matchTargetTransform.position + Vector3.up, -Vector3.up, out hit, 2, currentActionInfo.layerForRaycast)) {
                currentActionInfo.matchTargetTransform.position = hit.point + 0.05f * Vector3.up;
            }
        }

        if (currentActionInfo.disableAnyStateConfiguredWithExitTime) {
            checkDisableHasExitTimeAnimator ();
        }

        //		if (mainAINavmesh == null) {
        //			print (currentActionInfo.actionID);
        //		}

        if (currentActionInfo.useActionID) {
            mainAnimator.SetInteger (actionIDAnimatorID, currentActionInfo.actionID);
        } else {
            mainAnimator.SetInteger (actionIDAnimatorID, 0);
        }

        if (currentActionSystem.animationUsedOnUpperBody) {
            mainAnimator.SetBool (actionActiveUpperBodyAnimatorID, true);

            if (currentActionSystem.disableRegularActionActiveState) {
                mainAnimator.SetBool (actionActiveAnimatorID, false);
            }
        } else {
            mainAnimator.SetBool (actionActiveAnimatorID, true);
        }

        if (currentActionInfo.useActionName) {
            if (currentActionInfo.useCrossFadeAnimation) {
                mainAnimator.CrossFadeInFixedTime (currentActionInfo.actionName, 0.1f);
            } else {
                mainAnimator.Play (currentActionInfo.actionName);
            }

            if (showDebugPrintCustomActionActivated) {
                print (currentActionInfo.actionName);
            }
        }

        playingAnimation = true;

        if (currentActionInfo.useMovementInput) {
            setActionMovementInputActiveState (true);
        }
    }

    void checkDisableHasExitTimeAnimator ()
    {
        if (disableHasExitTimeCoroutine != null) {
            StopCoroutine (disableHasExitTimeCoroutine);
        }

        disableHasExitTimeCoroutine = StartCoroutine (checkDisablehasExitTimeAnimatorCoroutine ());
    }

    IEnumerator checkDisablehasExitTimeAnimatorCoroutine ()
    {
        mainAnimator.SetBool (disableHasExitTimeAnimatorID, true);

        WaitForSeconds delay = new WaitForSeconds (0.1f);

        yield return delay;

        mainAnimator.SetBool (disableHasExitTimeAnimatorID, false);
    }

    void stopMovePlayerOnDirectionCorotuine ()
    {
        if (movePlayerCoroutine != null) {
            StopCoroutine (movePlayerCoroutine);
        }

        movePlayerOnDirectionActive = false;
    }

    IEnumerator movePlayerOnDirectionCoroutine ()
    {
        if (currentActionInfo.movePlayerOnDirection) {
            movePlayerOnDirectionActive = true;

            Vector3 targetPosition = playerTransform.position;

            RaycastHit hit;

            Vector3 raycastPosition = targetPosition + playerTransform.up;

            float movementDistance = currentActionInfo.movePlayerOnDirectionRaycastDistance;

            Vector3 movePlayerDirection = currentActionInfo.movePlayerDirection;


            float currentSurfaceHitAngle = mainPlayerController.getCurrentSurfaceHitAngle ();

            if (Mathf.Abs (currentSurfaceHitAngle) > 1) {

                if (Physics.Raycast (targetPosition, -playerTransform.up, out hit, 3, currentActionInfo.movePlayerOnDirectionLayermask)) {
                    currentSurfaceHitAngle = Vector3.SignedAngle (playerTransform.up, hit.normal, playerTransform.right);
                }

                movePlayerDirection = Quaternion.Euler (currentSurfaceHitAngle, 0, 0) * movePlayerDirection;
            }

            Vector3 movementDirection = playerTransform.TransformDirection (movePlayerDirection);

            if (showDebugPrint) {
                print (currentSurfaceHitAngle);
                print (movementDirection);
            }

            if (showGizmo) {
                Debug.DrawRay (raycastPosition, movementDirection * movementDistance, Color.white, 5);
            }

            //			Debug.DrawRay (raycastPosition, playerTransform.TransformDirection (currentActionInfo.movePlayerDirection) * movementDistance, Color.white, 5);

            //			print (playerTransform.TransformDirection (currentActionInfo.movePlayerDirection));


            if (Physics.Raycast (raycastPosition, movementDirection, out hit,
                    movementDistance, currentActionInfo.movePlayerOnDirectionLayermask)) {
                movementDistance = hit.distance - 0.6f;
            }

            targetPosition += movementDistance * movementDirection;

            float dist = GKC_Utils.distance (playerTransform.position, targetPosition);

            float duration = dist / currentActionInfo.movePlayerOnDirectionSpeed;

            float t = 0;

            float movementTimer = 0;

            bool targetReached = false;

            float positionDifference = 0;

            while (!targetReached) {
                t += Time.deltaTime / duration;

                if (currentActionInfo.usePhysicsForceOnMovePlayer) {
                    mainRigidbody.position = Vector3.MoveTowards (mainRigidbody.position, targetPosition, Time.deltaTime * currentActionInfo.physicsForceOnMovePlayer);

                    mainPlayerController.setCurrentVelocityValue (mainRigidbody.linearVelocity);

                    //					mainPlayerController.setCurrentVelocityValue (movementDirection * currentActionInfo.physicsForceOnMovePlayer);

                    if (t >= currentActionInfo.physicsForceOnMovePlayerDuration) {
                        targetReached = true;
                    }

                    if (currentActionInfo.checkIfPositionReachedOnPhysicsForceOnMovePlayer) {
                        positionDifference = GKC_Utils.distance (playerTransform.position, targetPosition);

                        if (positionDifference < 0.01f) {
                            targetReached = true;
                        }
                    }
                } else {
                    playerTransform.position = Vector3.Lerp (playerTransform.position, targetPosition, t);

                    positionDifference = GKC_Utils.distance (playerTransform.position, targetPosition);

                    movementTimer += Time.deltaTime;

                    if (positionDifference < 0.01f || movementTimer > (duration + 0.3f)) {
                        targetReached = true;
                    }
                }

                yield return null;
            }

            movePlayerOnDirectionActive = false;
        }
    }

    public void checkSetActionState ()
    {
        if (currentActionInfo.setActionState) {

            for (int i = 0; i < actionStateInfoList.Count; i++) {
                if (actionStateInfoList [i].Name.Equals (currentActionInfo.actionStateName)) {
                    if (currentActionInfo.actionStateToConfigure) {
                        actionStateInfoList [i].eventToActivateState.Invoke ();
                    } else {
                        actionStateInfoList [i].eventToDeactivateState.Invoke ();
                    }
                }
            }
        }
    }

    public bool isRotatingTowardFacingDirection ()
    {
        return rotatingTowardFacingDirection;
    }

    public bool isPlayerWalkingToDirectionActive ()
    {
        return walkingToDirectionActive;
    }

    public bool isActionActive ()
    {
        return actionActive;
    }

    public bool isPlayerMovingOn3dWorld ()
    {
        return mainPlayerController.isPlayerMovingOn3dWorld ();
    }

    public bool isUsePositionToAdjustPlayerActive ()
    {
        return usePositionToAdjustPlayerActive;
    }

    public void setWaitToPlayAnimationAfterAdjustPositionActiveState (bool state)
    {
        waitToPlayAnimationAfterAdjustPositionActive = state;
    }

    public void stopSetPlayerWalkState ()
    {
        if (playerWalkCoroutine != null) {
            StopCoroutine (playerWalkCoroutine);
        }

        walkingToDirectionActive = false;

        rotatingTowardFacingDirection = false;
    }

    public void setPlayerWalkState (bool playAnimationAtEnd)
    {
        stopSetPlayerWalkState ();

        playerWalkCoroutine = StartCoroutine (setPlayerWalkStateCoroutine (playAnimationAtEnd));
    }

    IEnumerator setPlayerWalkStateCoroutine (bool playAnimationAtEnd)
    {
        if (currentActionInfo.useRaycastToAdjustTargetTransform) {
            RaycastHit hit;

            if (Physics.Raycast (currentActionInfo.playerWalkTarget.position + Vector3.up, -Vector3.up, out hit, 2, currentActionInfo.layerForRaycast)) {
                currentActionInfo.playerWalkTarget.position = hit.point + 0.05f * Vector3.up;
            }
        }

        float timer = Time.time;

        bool targetReached = false;

        if (playAnimationAtEnd) {
            playingAnimation = false;

            mainPlayerController.setGravityForcePuase (false);

            mainPlayerController.setCheckOnGroungPausedState (false);
        }

        if (currentActionInfo.pausePlayerInputDuringWalk && !pausePlayerInputDuringWalk) {
            pausePlayerInputDuringWalk = true;

            checkIfPauseInputListDuringActionActive (true);

        }

        if (mainPlayerCamera.isFullBodyAwarenessActive ()) {

            if (currentActionInfo.ignorePlayerRotationToCameraOnFBA) {
                mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (true);

                ignorePlayerRotationToCameraOnFBA = true;

                mainPlayerController.setFullBodyAwarenessActiveStateExternally (false);
            }

            if (currentActionSystem.keepCameraRotationToHeadOnFullBodyAwareness) {
                if (!currentActionSystem.ignoreChangeToThirdPerson) {
                    mainPlayerCamera.setPivotCameraTransformParentCurrentTransformToFollow ();

                    if (currentActionInfo.setCustomHeadTrackTargetToLook) {
                        float newAngle = mainPlayerCamera.pivotCameraTransform.localEulerAngles.x;

                        if (newAngle > 180) {
                            newAngle -= 360;
                        }

                        mainPlayerCamera.setLookAngleValue (new Vector2 (0, newAngle));

                        if (currentActionInfo.useCustomHeadTrackTolook) {
                            mainHeadTrack.setCustomHeadTrackToLookInfoByName (currentActionInfo.customHeadTrackToLookName);
                        } else {
                            mainHeadTrack.setNewCameraTargetToLookAndAdjustDirectionToPrevious (currentActionInfo.customHeadTrackTargetToLook);
                        }

                        newHeadTrackTargetUsed = true;
                    }

                    setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = true;
                }
            }

            if (currentActionInfo.resetCameraRotationForwardOnFBA) {
                mainPlayerCamera.setPlayerCameraTransformRotation (playerTransform.rotation);
                mainPlayerCamera.setLookAngleValue (Vector2.zero);
            }

            bool pauseHeadTrackResult = currentActionInfo.pauseHeadTrack;

            if (currentActionInfo.usCustomPauseHeadTrackOnFBA) {
                pauseHeadTrackResult = currentActionInfo.customPauseHeadTrackOnFBA;
            }

            if (pauseHeadTrackResult) {
                if (mainHeadTrack != null) {
                    mainHeadTrack.setSmoothHeadTrackDisableState (true);
                }

                mainPlayerController.setHeadTrackCanBeUsedState (false);
            }
        }

        if (currentActionInfo.resetPlayerMovementInput) {
            mainPlayerController.changeScriptState (true);
        } else {
            mainPlayerController.setCanMoveState (true);
        }

        mainPlayerController.setUpdate2_5dClampedPositionPausedState (true);

        bool previousStrafeMode = mainPlayerController.isStrafeModeActive ();

        if (currentActionInfo.playerWalkTarget) {
            if (usingAINavmesh) {
                mainAINavmesh.enableCustomNavMeshSpeed (currentActionInfo.maxWalkSpeed);
            } else {
                if (!navmeshUsedOnAction) {
                    navmeshPreviouslyActive = mainPlayerNavMeshSystem.isPlayerNavMeshEnabled ();

                    navmeshUsedOnAction = true;
                }

                mainPlayerNavMeshSystem.setShowCursorPausedState (true);

                mainPlayerNavMeshSystem.setUsingPlayerNavmeshExternallyState (true);
                mainPlayerNavMeshSystem.setPlayerNavMeshEnabledState (true);
                mainPlayerNavMeshSystem.enableCustomNavMeshSpeed (currentActionInfo.maxWalkSpeed);
            }

            mainPlayerCamera.setPlayerNavMeshEnabledState (false);

            if (usingAINavmesh) {
                mainAINavmesh.pauseAI (false);

                if (currentActionInfo.setMoveNavMeshPaused) {
                    mainAINavmesh.setMoveNavMeshPausedDuringActionState (false);
                }

                mainAINavmesh.setTarget (currentActionInfo.playerWalkTarget);
                mainAINavmesh.setTargetType (false, true);

                mainAINavmesh.enableCustomMinDistance (0.22f);
            } else {
                mainPlayerNavMeshSystem.checkRaycastPositionWithVector3 (currentActionInfo.playerWalkTarget.position);

                mainPlayerNavMeshSystem.enableCustomMinDistance (0.09f);
            }

            mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (false);
        }

        bool dynamicObstacleDetectionChecked = false;
        bool dynamicObstacleActiveChecked = false;

        while (!targetReached) {
            if (currentActionInfo.playerWalkTarget) {
                walkingToDirectionActive = true;

                WaitForSeconds delay = new WaitForSeconds (0.6f);

                yield return delay;

                currentDistanceToTarget = GKC_Utils.distance (currentActionInfo.playerWalkTarget.position, playerTransform.position);

                if (currentDistanceToTarget < 2) {
                    if (!dynamicObstacleDetectionChecked) {
                        if (currentActionInfo.activateDynamicObstacleDetection) {
                            if (usingAINavmesh) {
                                mainAINavmesh.setUseDynamicObstacleDetectionState (false);
                            } else {
                                mainPlayerNavMeshSystem.setUseDynamicObstacleDetectionState (false);
                            }
                        }

                        dynamicObstacleDetectionChecked = true;
                    }
                } else {
                    if (!dynamicObstacleActiveChecked) {
                        if (currentActionInfo.activateDynamicObstacleDetection) {
                            if (usingAINavmesh) {
                                mainAINavmesh.setUseDynamicObstacleDetectionState (true);
                            } else {
                                mainPlayerNavMeshSystem.setUseDynamicObstacleDetectionState (true);
                            }
                        }

                        dynamicObstacleActiveChecked = true;
                    }
                }

                if (usingAINavmesh) {
                    //					print (mainAINavmesh.isFollowingTarget ());

                    currentWalkDirection = mainAINavmesh.getCurrentMovementDirection ();

                    if (!mainAINavmesh.isFollowingTarget ()) {
                        targetReached = true;
                    }
                } else {
                    currentWalkDirection = mainPlayerNavMeshSystem.getCurrentMovementDirection ();

                    if (!mainPlayerNavMeshSystem.isFollowingTarget ()) {
                        targetReached = true;
                    }
                }
            } else {
                targetReached = true;
            }

            yield return null;
        }

        if (!currentActionInfo.pauseStrafeState) {
            mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (previousStrafeMode);
        }

        if (currentActionInfo.playerWalkTarget) {
            if (usingAINavmesh) {
                mainAINavmesh.removeTarget ();

                mainAINavmesh.disableCustomNavMeshSpeed ();

                mainAINavmesh.disableCustomMinDistance ();

                mainAINavmesh.pauseAI (true);

                mainFindObjectivesSystem.checkPauseOrResumePatrolStateDuringActionActive (true);

                if (currentActionInfo.setMoveNavMeshPaused) {
                    mainAINavmesh.setMoveNavMeshPausedDuringActionState (true);
                }
            } else {
                mainPlayerNavMeshSystem.setPlayerNavMeshEnabledState (false);

                mainPlayerNavMeshSystem.setUsingPlayerNavmeshExternallyState (false);

                mainPlayerNavMeshSystem.disableCustomNavMeshSpeed ();

                mainPlayerNavMeshSystem.disableCustomMinDistance ();

                mainPlayerNavMeshSystem.setShowCursorPausedState (false);
            }
        }

        walkingToDirectionActive = false;

        if (currentActionInfo.resetPlayerMovementInput) {
            mainPlayerController.changeScriptState (false);
        } else {
            mainPlayerController.setCanMoveState (false);
            mainPlayerController.resetPlayerControllerInput ();
        }

        if (playAnimationAtEnd) {
            resumePlayer ();

            if (currentActionInfo.resetActionIndexAfterComplete) {
                currentActionInfoIndex = 0;

                currentActionInfo = currentActionSystem.actionInfoList [currentActionInfoIndex];

                currentActionName = currentActionInfo.Name;

                currentActionSystem.resetCurrentActionInfoIndex ();
            }

            checkDestroyAction ();

            checkRemoveAction ();
        } else {
            activateAnimation ();
        }
    }

    public void OnAnimatorMove ()
    {
        if (actionActivatedOnFirstPerson) {

        } else {
            updateOnAnimatorMove ();
        }
    }

    void updateOnAnimatorMove ()
    {
        if (actionActive) {

            bool matchTargetActive = false;

            if (currentActionInfo.useActionID) {
                if (currentActionInfo.removeActionIDValueImmediately) {
                    if (Time.time > lastTimeAnimationPlayed + 0.1f) {
                        mainAnimator.SetInteger (actionIDAnimatorID, 0);
                    }
                }

                if (mainAnimator.GetCurrentAnimatorStateInfo (actionsLayerIndex).IsName (currentActionInfo.actionName)) {
                    mainAnimator.SetInteger (actionIDAnimatorID, 0);
                }
            }

            if (currentActionInfo.actionUsesMovement) {
                if (currentActionInfo.adjustRotationDuringMovement) {
                    playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, currentActionInfo.playerTargetTransform.rotation,
                        currentActionInfo.matchPlayerRotationSpeed * Time.fixedDeltaTime);

                    if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.transform.rotation = playerTransform.rotation;
                    }
                }
            } else {
                if (playingAnimation) {
                    if (currentActionInfo.adjustPlayerPositionRotationDuring) {
                        matchTargetActive = true;
                    }
                }

                if (movingPlayerToPositionTargetActive) {

                } else {
                    if (matchTargetActive) {
                        if (currentActionInfo.matchTargetTransform != null) {
                            matchTarget (currentActionInfo.matchTargetTransform.position,
                                currentActionInfo.matchTargetTransform.rotation,
                                currentActionInfo.mainAvatarTarget,
                                new MatchTargetWeightMask (currentActionInfo.matchTargetPositionWeightMask, currentActionInfo.matchTargetRotationWeightMask),
                                currentActionInfo.matchStartValue,
                                currentActionInfo.matchEndValue);
                        }

                        if (currentActionInfo.matchTargetRotationWeightMask == 0) {
                            if (currentActionInfo.matchPlayerRotation) {
                                playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, currentActionInfo.playerTargetTransform.rotation,
                                    currentActionInfo.matchPlayerRotationSpeed * Time.fixedDeltaTime);
                            } else {
                                playerTransform.rotation = mainAnimator.rootRotation;
                            }
                        }
                    }

                    if (playingAnimation) {
                        if (!actionActivatedOnFirstPerson) {
                            playerTransform.position = mainAnimator.rootPosition;
                        }
                    }
                }
            }

            if (!currentAnimationChecked && startAction) {
                float currentAnimationDuration = 0;

                if (actionActivatedOnFirstPerson) {
                    currentAnimationDuration = currentActionInfo.actionDurationOnFirstPerson;
                } else {
                    currentAnimationDuration = currentActionInfo.animationDuration;
                }

                float animationDuration = currentAnimationDuration / currentActionInfo.animationSpeed;

                float mainAnimatorSpeed = mainAnimator.speed;

                if (mainAnimatorSpeed <= 0) {
                    lastTimeAnimationPlayed += Time.deltaTime;
                } else if (mainAnimatorSpeed < 1) {
                    animationDuration /= mainAnimatorSpeed;
                }

                if (Time.time > lastTimeAnimationPlayed + (animationDuration + currentActionInfo.delayToPlayAnimation)) {

                    if (showDebugPrint) {
                        print ("Animation duration " + ((Time.time) - (lastTimeAnimationPlayed + animationDuration + currentActionInfo.delayToPlayAnimation)));
                    }
                    //					print (Time.time + " " + lastTimeAnimationPlayed + " " + (animationDuration + currentActionInfo.delayToPlayAnimation));

                    currentAnimationChecked = true;

                    startAction = false;

                    if (currentActionInfo.resumePlayerAfterAction) {
                        if (showDebugPrint) {
                            print ("resume player action action state reached, checking next state");
                        }

                        if (currentActionInfo.usePlayerWalkTarget && currentActionInfo.useWalkAtTheEndOfAction) {
                            setPlayerWalkState (true);
                        } else {
                            resumePlayer ();

                            if (currentActionInfo.resetActionIndexAfterComplete) {
                                currentActionInfoIndex = 0;

                                currentActionInfo = currentActionSystem.actionInfoList [currentActionInfoIndex];

                                currentActionName = currentActionInfo.Name;

                                currentActionSystem.resetCurrentActionInfoIndex ();
                            }

                            checkDestroyAction ();

                            checkRemoveAction ();
                        }
                    } else {

                        if (currentActionInfo.increaseActionIndexAfterComplete) {
                            currentActionInfoIndex++;
                        }

                        if (currentActionInfo.waitForNextPlayerInteractionButtonPress) {
                            waitingForNextPlayerInput = true;

                        } else {
                            if (!currentActionInfo.stayInState) {
                                currentActionInfo = currentActionSystem.actionInfoList [currentActionInfoIndex];

                                currentActionName = currentActionInfo.Name;

                                playAnimation ();
                            }
                        }
                    }

                    //					print ("final");
                }
            }
        }
    }

    public void matchTarget (Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
    {
        if (mainAnimator.isMatchingTarget || mainAnimator.IsInTransition (actionsLayerIndex)) {
            return;
        }

        float normalizeTime = Mathf.Repeat (mainAnimator.GetCurrentAnimatorStateInfo (actionsLayerIndex).normalizedTime, 1f);

        if (normalizeTime > normalisedEndTime) {
            return;
        }

        mainAnimator.MatchTarget (matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
    }

    public void forcePlayCurrentAnimation (int newCurrentActionInfoIndex)
    {
        currentActionInfoIndex = newCurrentActionInfoIndex;

        waitingForNextPlayerInput = true;

        playCurrentAnimation ();
    }

    public void playCurrentAnimation ()
    {
        if (actionFound) {

            bool canPlayAnimationCheck = canPlayAnimation ();

            if (showDebugPrint) {
                print ("Result of check if can play animation " + canPlayAnimationCheck);
            }

            if ((!actionActive || waitingForNextPlayerInput) &&
                (currentActionInfo.useInteractionButtonToActivateAnimation || currentActionInfo.animationTriggeredByExternalEvent || usingAINavmesh) &&
                canPlayAnimationCheck) {

                if (waitingForNextPlayerInput) {
                    if (actionActive) {
                        if (!pauseInputListChecked) {
                            checkIfPauseInputListDuringActionActive (false);
                        }

                        pausePlayerActivated = false;
                    }

                    pauseInputListChecked = false;

                    currentActionInfo = currentActionSystem.actionInfoList [currentActionInfoIndex];

                    currentActionName = currentActionInfo.Name;

                    //					if (actionActive) {
                    //						checkIfPauseInputListDuringActionActive (true);
                    //					}
                }

                if (currentActionInfo.checkConditionsToStartActionOnUpdate) {
                    checkConditionsActive = true;

                    return;
                }

                //				if (mainAINavmesh == null) {
                //					print ("play current animation " + currentActionInfo.actionName);
                //				}

                playAnimation ();
            }
        }
    }

    public bool canPlayAnimation ()
    {
        if (!mainPlayerController.canPlayerRagdollMove ()) {
            if (showDebugPrint) {
                print ("can't play animation for ragdoll");
            }

            return false;
        }

        if (!mainPlayerController.isPlayerOnGround () && currentActionInfo.checkIfPlayerOnGround) {
            if (showDebugPrint) {
                print ("can't play animation for ground");
            }

            return false;
        }

        if (currentActionInfo.checkPlayerToNotCrouch) {
            if (mainPlayerController.isCrouching ()) {
                if (showDebugPrint) {
                    print ("can't play animation for crouch");
                }

                return false;
            }
        }

        if (currentActionInfo.checkIfPlayerCanGetUpFromCrouch) {
            if (!mainPlayerController.playerCanGetUpFromCrouch ()) {
                if (showDebugPrint) {
                    print ("can't play animation for not getting up");
                }

                return false;
            }
        }

        bool checkDistanceResult = false;

        if (currentActionSystem.useMinDistanceToActivateAction) {
            if (usingAINavmesh) {
                if (currentActionInfo.checkUseMinDistanceToActivateAction) {
                    checkDistanceResult = true;
                }
            } else {
                checkDistanceResult = true;
            }
        }

        if (checkDistanceResult) {
            currentDistanceToTarget = GKC_Utils.distance (currentActionSystemTransform.position, playerTransform.position);

            if (currentDistanceToTarget > currentActionSystem.minDistanceToActivateAction) {
                if (showDebugPrint) {
                    print ("can't play animation for distance");
                }

                return false;
            }
        }

        bool checkAngleResult = false;

        if (currentActionSystem.useMinAngleToActivateAction) {
            if (usingAINavmesh) {
                if (currentActionInfo.checkUseMinAngleToActivateAction) {
                    checkAngleResult = true;
                }
            } else {
                checkAngleResult = true;
            }
        }

        if (checkAngleResult) {
            currentAngleWithTarget = Vector3.SignedAngle (playerTransform.forward, currentActionSystemTransform.forward, playerTransform.up);

            if (Mathf.Abs (currentAngleWithTarget) > currentActionSystem.minAngleToActivateAction) {
                if (currentActionSystem.checkOppositeAngle) {
                    currentAngleWithTarget = Mathf.Abs (currentAngleWithTarget) - 180;

                    if (Mathf.Abs (currentAngleWithTarget) > currentActionSystem.minAngleToActivateAction) {
                        if (showDebugPrint) {
                            print ("can't play animation for angle");
                        }

                        return false;
                    }
                } else {
                    if (showDebugPrint) {
                        print ("can't play animation for angle");
                    }

                    return false;
                }
            }
        }

        if (showDebugPrint) {
            print (playerTransform.name + " can play animation " + currentActionInfo.Name);
        }

        return true;
    }

    public void stopCheckActionEventInfoList ()
    {
        if (eventInfoListCoroutine != null) {
            //			print ("stop coroutine " + playerTransform.name);
            StopCoroutine (eventInfoListCoroutine);
        }
    }

    public void checkActionEventInfoList ()
    {
        if (currentActionInfo != null && currentActionInfo.useEventInfoList) {

            stopCheckActionEventInfoList ();

            //			print ("start coroutine " + playerTransform.name);

            eventInfoListCoroutine = StartCoroutine (checkActionEventInfoListCoroutine ());
        }
    }

    IEnumerator checkActionEventInfoListCoroutine ()
    {
        List<actionSystem.eventInfo> eventInfoList = currentActionInfo.eventInfoList;

        eventInfoListActivatedOnFirstPerson = false;

        if (actionActivatedOnFirstPerson) {
            eventInfoList = currentActionInfo.firstPersonEventInfoList;

            eventInfoListActivatedOnFirstPerson = false;
        }

        for (int i = 0; i < eventInfoList.Count; i++) {
            eventInfoList [i].eventTriggered = false;
        }

        actionSystem.eventInfo currentEventInfo;

        bool useAnimationSpeedOnDelay = currentActionInfo.useAnimationSpeedOnDelay;

        float mainAnimatorSpeed = mainAnimator.speed;

        float animationSpeed = currentActionInfo.animationSpeed;

        if (currentActionInfo.useAccumulativeDelay) {
            for (int i = 0; i < eventInfoList.Count; i++) {

                currentEventInfo = eventInfoList [i];

                float delayToActivate = currentEventInfo.delayToActivate;

                if (useAnimationSpeedOnDelay) {
                    delayToActivate = delayToActivate / animationSpeed;

                    if (mainAnimatorSpeed < 1) {
                        delayToActivate /= mainAnimatorSpeed;
                    }
                }

                WaitForSeconds delay = new WaitForSeconds (delayToActivate);

                yield return delay;

                currentEventInfo.eventToUse.Invoke ();

                if (currentEventInfo.useRemoteEvent) {
                    if (currentEventInfo.useRemoteEventNameList) {
                        for (int j = 0; j < currentEventInfo.remoteEventNameList.Count; j++) {
                            mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventNameList [j]);
                        }
                    } else {
                        mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventName);
                    }
                }

                if (currentEventInfo.sendCurrentPlayerOnEvent) {
                    currentEventInfo.eventToSendCurrentPlayer.Invoke (playerTransform.gameObject);
                }

                if (currentActionInfo == null) {
                    i = eventInfoList.Count - 1;
                }
            }
        } else {
            int numberOfEvents = eventInfoList.Count;

            int numberOfEventsTriggered = 0;

            float timer = Time.time;

            bool allEventsTriggered = false;

            while (!allEventsTriggered) {
                if (currentActionInfo == null) {
                    allEventsTriggered = true;

                } else {
                    for (int i = 0; i < eventInfoList.Count; i++) {

                        currentEventInfo = eventInfoList [i];

                        if (!currentEventInfo.eventTriggered) {
                            float delayToActivate = currentEventInfo.delayToActivate;

                            if (useAnimationSpeedOnDelay) {
                                delayToActivate = delayToActivate / animationSpeed;

                                if (mainAnimatorSpeed < 1) {
                                    delayToActivate /= mainAnimatorSpeed;
                                }
                            }

                            if (Time.time > timer + delayToActivate) {
                                currentEventInfo.eventToUse.Invoke ();

                                if (currentEventInfo.useRemoteEvent) {
                                    if (currentEventInfo.useRemoteEventNameList) {
                                        for (int j = 0; j < currentEventInfo.remoteEventNameList.Count; j++) {
                                            mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventNameList [j]);
                                        }
                                    } else {
                                        mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventName);
                                    }
                                }

                                if (currentEventInfo.sendCurrentPlayerOnEvent) {
                                    currentEventInfo.eventToSendCurrentPlayer.Invoke (playerTransform.gameObject);
                                }

                                currentEventInfo.eventTriggered = true;

                                numberOfEventsTriggered++;

                                if (numberOfEvents == numberOfEventsTriggered) {
                                    allEventsTriggered = true;
                                }
                            }
                        }
                    }
                }

                yield return null;
            }
        }
    }


    public void stopCheckKillOrEnterOnRagdollState ()
    {
        if (killOrEnterOnRagdollStateCoroutine != null) {
            StopCoroutine (killOrEnterOnRagdollStateCoroutine);
        }
    }

    public void checkKillOrEnterOnRagdollState ()
    {
        if (currentActionInfo != null && (currentActionInfo.killCharacter || currentActionInfo.activateCharacterRagdoll)) {

            stopCheckKillOrEnterOnRagdollState ();

            killOrEnterOnRagdollStateCoroutine = StartCoroutine (checkKillOrEnterOnRagdollStateCoroutine ());
        }
    }

    IEnumerator checkKillOrEnterOnRagdollStateCoroutine ()
    {
        if (currentActionInfo.activateCharacterRagdoll) {
            if (currentActionInfo.delatyToActivateCharacterRagdoll > 0) {
                WaitForSeconds delay = new WaitForSeconds (currentActionInfo.delatyToActivateCharacterRagdoll);

                yield return delay;
            }

            bool setRagdollStateWithForceResult = false;

            float ragdollStateDuration = 0;

            if (currentActionInfo.ragdollStateDuration > 0) {
                if (currentActionInfo.addForceToRagdoll) {
                    ragdollStateDuration = currentActionInfo.ragdollStateDuration;

                    setRagdollStateWithForceResult = true;
                } else {
                    mainHealth.pushCharacterWithoutForceXAmountOfTime (currentActionInfo.ragdollStateDuration);
                }
            } else {
                if (currentActionInfo.addForceToRagdoll) {
                    setRagdollStateWithForceResult = true;
                } else {
                    mainHealth.pushCharacterWithoutForce ();
                }
            }

            if (setRagdollStateWithForceResult) {
                Vector3 ragdollForce = playerTransform.forward;

                if (currentActionInfo.useLocalCharacterRagdollForceDirection) {
                    Vector3 localCharacterRagdollForceDirection = currentActionInfo.localCharacterRagdollForceDirection;

                    ragdollForce = localCharacterRagdollForceDirection.x * playerTransform.right +
                        localCharacterRagdollForceDirection.y * playerTransform.up +
                        localCharacterRagdollForceDirection.z * playerTransform.forward;
                }

                if (currentActionInfo.useDirectionForRagdoll) {
                    ragdollForce = currentActionInfo.directionForRagdollTransform.forward;
                }

                ragdollForce *= currentActionInfo.ragdollForce;

                mainHealth.pushFullCharacterXAmountOfTime (ragdollStateDuration, ragdollForce);
            }

            yield return null;
        }

        if (currentActionInfo.killCharacter) {
            if (currentActionInfo.delayToKillCharacter > 0) {
                WaitForSeconds delay = new WaitForSeconds (currentActionInfo.delayToKillCharacter);

                yield return delay;
            }

            mainHealth.killByButtonWithoutRagdollForce ();

            yield return null;
        }

        yield return null;
    }

    public void stopCheckSetObjectParent ()
    {
        if (objectParentCoroutine != null) {
            StopCoroutine (objectParentCoroutine);
        }
    }

    public void checkSetObjectParent ()
    {
        if (!currentActionInfo.setObjectParent) {
            return;
        }

        stopCheckSetObjectParent ();

        objectParentCoroutine = StartCoroutine (checkSetObjectParentCoroutine ());
    }


    IEnumerator checkSetObjectParentCoroutine ()
    {
        WaitForSeconds delay = new WaitForSeconds (currentActionInfo.waitTimeToParentObject);

        yield return delay;

        Transform targetTransform = currentActionInfo.bonePositionReference;

        Transform currentObject = currentActionInfo.objectToSetParent;

        Vector3 objectToSetParentOriginalPosition = currentObject.localPosition;
        Vector3 objectToSetParentOriginalRotation = currentObject.localEulerAngles;

        Transform objectToSetParentOriginalParent = currentObject.parent;

        currentActionSystem.setObjectToSetParentInfo (currentObject,
            objectToSetParentOriginalPosition, objectToSetParentOriginalRotation,
            objectToSetParentOriginalParent);

        Transform currentParent = mainAnimator.GetBoneTransform (currentActionInfo.boneParent);

        if (currentActionInfo.useMountPoint) {
            Transform currentMountPoint = GKC_Utils.getMountPointTransformByName (currentActionInfo.mountPointName, playerTransform);

            if (currentMountPoint != null) {
                currentParent = currentMountPoint;
            }
        }

        currentObject.SetParent (currentParent);

        float dist = GKC_Utils.distance (currentObject.localPosition, targetTransform.localPosition);

        float duration = dist / currentActionInfo.setObjectParentSpeed;

        float t = 0;

        Vector3 pos = targetTransform.localPosition;
        Quaternion rot = targetTransform.localRotation;

        float movementTimer = 0;

        bool targetReached = false;

        float angleDifference;

        while (!targetReached) {
            t += Time.deltaTime / duration;
            currentObject.localPosition = Vector3.Slerp (currentObject.localPosition, pos, t);
            currentObject.localRotation = Quaternion.Slerp (currentObject.localRotation, rot, t);

            angleDifference = Quaternion.Angle (currentObject.localRotation, rot);

            movementTimer += Time.deltaTime;

            if ((GKC_Utils.distance (currentObject.localPosition, pos) < 0.01f && angleDifference < 0.2f) || movementTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }

        if (currentActionInfo.resetObjectPositionAndParentAfterAction) {
            currentActionSystem.checkResetObjectPositionAndParentAfterAction ();
        }
    }

    public void setActionMovementInputActiveState (bool state)
    {
        actionMovementInputActive = state;
    }

    public void setLastHorizontalDirectionValue (int newValue)
    {
        lastHorizontalDirection = newValue;
    }

    public void setLastVerticalDirectionValue (int newValue)
    {
        lastVerticalDirection = newValue;
    }

    public void updateLastDirectionValues ()
    {
        mainAnimator.SetInteger (lastHorizontalDirectionAnimatorID, lastHorizontalDirection);
        mainAnimator.SetInteger (lastVerticalDirectionAnimatorID, lastVerticalDirection);
    }

    void getCustomActionByName (string actionName, ref int categoryIndex, ref int actionIndex)
    {
        int currentActionIndex = customActionStateInfoDictionaryList.FindIndex (s => s.Name.ToLower () == actionName);

        if (currentActionIndex > -1) {
            customActionStateInfoDictionary currentCustomActionStateInfoDictionary = customActionStateInfoDictionaryList [currentActionIndex];

            categoryIndex = currentCustomActionStateInfoDictionary.categoryIndex;

            actionIndex = currentCustomActionStateInfoDictionary.actionIndex;
        }
    }

    public void forceActivateCustomAction (string actionName)
    {
        waitingForNextPlayerInput = true;

        activateCustomAction (actionName);
    }

    public void setActivateCustomActionsPausedState (bool state)
    {
        activateCustomActionsPaused = state;
    }

    public void activateCustomActionByCategoryName (string categoryName)
    {
        int categoryIndex = -1;
        int actionIndex = -1;

        for (int i = 0; i < customActionStateCategoryInfoList.Count; i++) {

            for (int j = 0; j < customActionStateCategoryInfoList [i].actionStateInfoList.Count; j++) {
                if (customActionStateCategoryInfoList [i].actionStateInfoList [j].categoryName.Equals (categoryName)) {
                    if (categoryIndex == -1) {
                        categoryIndex = i;

                        actionIndex = j;
                    }

                    int randomProbability = Random.Range (0, 100);

                    if (randomProbability > 50) {
                        categoryIndex = i;

                        actionIndex = j;
                    }
                }
            }
        }

        if (categoryIndex > -1 && actionIndex > -1) {
            activateCustomAction (customActionStateCategoryInfoList [categoryIndex].actionStateInfoList [actionIndex].Name);
        }
    }

    public void activateCustomAction (string actionName)
    {
        if (!customActionStatesEnabled) {
            return;
        }

        //		if (mainAINavmesh == null) {
        //			print (actionName + "  " + activateCustomActionsPaused);
        //		}

        //		print (actionName);

        if (activateCustomActionsPaused) {
            return;
        }

        if (showDebugPrint) {
            print ("check action to play " + actionName);
        }

        actionName = actionName.ToLower ();

        int categoryIndex = -1;
        int actionIndex = -1;

        getCustomActionByName (actionName, ref categoryIndex, ref actionIndex);

        int currentCategoryIndex = categoryIndex;

        int currentActionIndex = actionIndex;

        if (currentActionIndex > -1) {
            customActionStateInfo newActionToUse = customActionStateCategoryInfoList [currentCategoryIndex].actionStateInfoList [currentActionIndex];

            if (newActionToUse.stateEnabled) {

                bool canActivateState = true;

                if (newActionToUse.checkLockedCameraState && mainPlayerController.isLockedCameraStateActive () != newActionToUse.lockedCameraState) {
                    canActivateState = false;
                }

                if (newActionToUse.checkAimingState && mainPlayerController.isPlayerAiming () != newActionToUse.aimingState) {
                    canActivateState = false;
                }

                if (currentCustomActionCategoryID > 0) {
                    if (newActionToUse.useCustomActionCategoryIDInfoList) {
                        if (!newActionToUse.canBeUsedOnAnyCustomActionCategoryID) {
                            int currentCategoryActionIndex = newActionToUse.customActionCategoryIDInfoList.FindIndex (s => s.categoryID == currentCustomActionCategoryID);

                            if (currentCategoryActionIndex == -1) {
                                print ("custom action category is " + currentCustomActionCategoryID + " action not found for " + newActionToUse.Name);

                                canActivateState = false;
                            }
                        }
                    } else {
                        if (showDebugPrint) {
                            print ("no custom action category id configured for " + newActionToUse.Name);
                        }

                        canActivateState = false;
                    }
                }

                if (canActivateState) {
                    checkIfSpawnActionSystem (newActionToUse);

                    newActionToUse.mainActionSystem.categoryName = newActionToUse.categoryName;

                    currentActionCategoryActive = newActionToUse.categoryName;

                    if (newActionToUse.useProbabilityToActivateAction) {
                        newActionToUse.mainActionSystem.useProbabilityToActivateAction = true;
                        newActionToUse.mainActionSystem.probablityToActivateAction = newActionToUse.probablityToActivateAction;

                        float currentProbablity = Random.Range (0, 100);

                        if ((newActionToUse.probablityToActivateAction * 100) < currentProbablity) {
                            return;
                        }
                    }

                    if (newActionToUse.useEventOnInterruptedAction) {
                        newActionToUse.mainActionSystem.useEventOnInterruptedAction = true;
                        newActionToUse.mainActionSystem.eventOnInterruptedAction = newActionToUse.eventOnInterruptedAction;
                    }

                    if (newActionToUse.canInterruptOtherActionActive) {
                        newActionToUse.mainActionSystem.canInterruptOtherActionActive = true;
                        newActionToUse.mainActionSystem.useCategoryToCheckInterrupt = newActionToUse.useCategoryToCheckInterrupt;

                        if (newActionToUse.mainActionSystem.useCategoryToCheckInterrupt) {
                            newActionToUse.mainActionSystem.actionCategoryListToInterrupt = newActionToUse.actionCategoryListToInterrupt;
                        } else {
                            newActionToUse.mainActionSystem.actionListToInterrupt = newActionToUse.actionListToInterrupt;
                        }

                        newActionToUse.mainActionSystem.eventOnInterrupOtherActionActive = newActionToUse.eventOnInterrupOtherActionActive;
                    }

                    if (showDebugPrint) {
                        print ("action can be played " + newActionToUse.Name + " " + mainPlayerController.name);
                    }

                    bool canForceToPlayCustomAction = false;

                    if (newActionToUse.mainActionSystem.canForceToPlayCustomAction) {
                        canForceToPlayCustomAction = true;
                    }

                    if (!canForceToPlayCustomAction || newActionToUse.canForceInterruptOtherActionActive) {
                        if (newActionToUse.canInterruptOtherActionActive && actionActive && currentActionSystem != null) {
                            if (newActionToUse.mainActionSystem.useCategoryToCheckInterrupt) {
                                if (newActionToUse.actionCategoryListToInterrupt.Contains (currentActionSystem.categoryName)) {
                                    if (showDebugPrint) {
                                        print ("action " + newActionToUse.Name + " can force stop " + currentActionSystem.getCurrentactionInfo ().Name +
                                        " by category " + currentActionSystem.categoryName);
                                    }

                                    newActionToUse.eventOnInterrupOtherActionActive.Invoke ();

                                    if (currentActionSystem.useEventOnInterruptedAction) {
                                        currentActionSystem.eventOnInterruptedAction.Invoke ();
                                    }

                                    canForceToPlayCustomAction = true;
                                }
                            } else {
                                if (newActionToUse.actionListToInterrupt.Contains (currentActionSystem.getCurrentactionInfo ().Name)) {
                                    newActionToUse.eventOnInterrupOtherActionActive.Invoke ();

                                    canForceToPlayCustomAction = true;
                                }
                            }
                        }
                    }

                    if (canForceToPlayCustomAction) {
                        if (actionActive) {
                            checkIfPauseInputListDuringActionActive (false);

                            pausePlayerActivated = false;

                            pauseInputListChecked = true;
                        }
                        waitingForNextPlayerInput = true;
                    }

                    checkCustomActionToActivate (currentCategoryIndex, currentActionIndex);

                    playCurrentAnimation ();

                    //					print ("activate custom action " + newActionToUse.Name);
                } else {
                    if (showDebugPrint) {
                        print ("action can not be activated " + actionName);
                    }
                }
            } else {
                if (showDebugPrint) {
                    print ("action not enabled for " + actionName);
                }
            }
        } else {
            if (showDebugPrint) {
                print ("action not found for " + actionName);
            }
        }
    }

    public void checkCustomActionToActivate (int currentCategoryIndex, int currentActionIndex)
    {
        customActionStateInfo currentCustomActionStateInfo = customActionStateCategoryInfoList [currentCategoryIndex].actionStateInfoList [currentActionIndex];

        if (showDebugPrint) {
            print ("current custom action to check " + currentCustomActionStateInfo.Name);
        }

        int currentCategoryActionIndex = -1;

        bool useCustomActionCategoryIDInfoList = currentCustomActionStateInfo.useCustomActionCategoryIDInfoList;

        if (currentCustomActionCategoryID > 0) {
            if (useCustomActionCategoryIDInfoList) {
                if (currentCustomActionStateInfo.canBeUsedOnAnyCustomActionCategoryID) {
                    useCustomActionCategoryIDInfoList = false;
                } else {
                    currentCategoryActionIndex = currentCustomActionStateInfo.customActionCategoryIDInfoList.FindIndex (s => s.categoryID == currentCustomActionCategoryID);
                }
            }
        }

        if (currentCustomActionStateInfo.useRandomActionSystemList) {
            int randomActionIndex = 0;

            int randomActionSystemListCount = currentCustomActionStateInfo.randomActionSystemList.Count;

            if (currentCategoryActionIndex > -1) {
                randomActionSystemListCount = currentCustomActionStateInfo.customActionCategoryIDInfoList [currentCategoryActionIndex].randomActionSystemList.Count;
            }

            if (currentCustomActionStateInfo.followActionsOrder && !useCustomActionCategoryIDInfoList) {
                randomActionIndex = currentCustomActionStateInfo.currentActionIndex;

                currentCustomActionStateInfo.currentActionIndex++;

                if (currentCustomActionStateInfo.currentActionIndex >= randomActionSystemListCount) {
                    currentCustomActionStateInfo.currentActionIndex = 0;
                }
            } else {
                randomActionIndex = Random.Range (0, randomActionSystemListCount);
            }

            if (currentCategoryActionIndex > -1) {
                currentCustomActionStateInfo.customActionCategoryIDInfoList [currentCategoryActionIndex].randomActionSystemList [randomActionIndex].activateCustomAction (playerTransform.gameObject);
            } else {
                currentCustomActionStateInfo.randomActionSystemList [randomActionIndex].activateCustomAction (playerTransform.gameObject);
            }
        } else {
            if (showDebugPrint) {
                print ("Trigger Activate Custom Action function on action system " + currentCustomActionStateInfo.Name);
            }

            bool activateRegularAction = false;

            if (currentCustomActionStateInfo.useActionOnCrouch) {
                if (mainPlayerController.isCrouching () && mainPlayerController.isPlayerOnGround ()) {
                    currentCustomActionStateInfo.mainActionSystemOnCrouch.activateCustomAction (playerTransform.gameObject);
                } else {
                    if (mainPlayerController.isPlayerOnGround ()) {
                        activateRegularAction = true;
                    }
                }
            }

            if (currentCustomActionStateInfo.useActionOnAir) {
                if (mainPlayerController.isPlayerOnGround ()) {
                    activateRegularAction = true;
                } else {
                    currentCustomActionStateInfo.mainActionSystemOnAir.activateCustomAction (playerTransform.gameObject);
                }
            } else {
                activateRegularAction = true;
            }

            if (activateRegularAction) {
                if (currentCategoryActionIndex > -1) {

                    activateRegularAction = false;

                    currentCustomActionStateInfo.customActionCategoryIDInfoList [currentCategoryActionIndex].mainActionSystem.activateCustomAction (playerTransform.gameObject);
                }

                if (activateRegularAction) {
                    checkIfSpawnActionSystem (currentCustomActionStateInfo);

                    currentCustomActionStateInfo.mainActionSystem.activateCustomAction (playerTransform.gameObject);
                }
            }
        }

        currentCustomActionName = currentCustomActionStateInfo.Name;

        customActionActive = true;
    }

    bool checkIfActionToStopIsCurrentlyActive;

    public void stopCustomActionIfCurrentlyActive (string actionName)
    {
        checkIfActionToStopIsCurrentlyActive = true;

        stopCustomAction (actionName);

        checkIfActionToStopIsCurrentlyActive = false;
    }

    public void stopCustomAction (string actionName)
    {
        if (!customActionStatesEnabled) {
            return;
        }

        bool checkingIfActionToStopIsOnStoredList = false;

        if (!actionActive) {
            checkingIfActionToStopIsOnStoredList = true;

            //			return;
        }

        actionName = actionName.ToLower ();

        int categoryIndex = -1;
        int actionIndex = -1;

        getCustomActionByName (actionName, ref categoryIndex, ref actionIndex);

        int currentCategoryIndex = categoryIndex;

        int currentActionIndex = actionIndex;

        if (currentActionIndex > -1) {
            customActionStateInfo newActionToUse = customActionStateCategoryInfoList [currentCategoryIndex].actionStateInfoList [currentActionIndex];

            if (newActionToUse.stateEnabled) {

                checkIfSpawnActionSystem (newActionToUse);

                if (checkIfActionToStopIsCurrentlyActive) {
                    if (currentActionSystem != null && newActionToUse.mainActionSystem != currentActionSystem) {
                        print ("trying to stop an action that is not currently playing, cancelling " + newActionToUse.Name);

                        return;

                    }
                }

                //				print ("action to stop " + newActionToUse.Name);

                bool canActivateState = true;

                if (newActionToUse.checkLockedCameraState && mainPlayerController.isLockedCameraStateActive () != newActionToUse.lockedCameraState) {
                    canActivateState = false;
                }

                if (newActionToUse.checkAimingState && mainPlayerController.isPlayerAiming () != newActionToUse.aimingState) {
                    canActivateState = false;
                }

                if (canActivateState) {
                    bool actionFoundOnStoredList = false;

                    checkEmptyActionSystemListStored ();

                    if (actionSystemListStoredToPlay.Count > 0) {
                        if (actionSystemListStoredToPlay.Contains (newActionToUse.mainActionSystem)) {
                            actionFoundOnStoredList = true;
                        }
                    }

                    if (checkingIfActionToStopIsOnStoredList) {
                        if (actionFoundOnStoredList) {
                            setPlayerActionActive (newActionToUse.mainActionSystem);
                        } else {
                            return;
                        }
                    }

                    checkCustomActionToActivate (currentCategoryIndex, currentActionIndex);

                    if (actionFoundOnStoredList) {
                        actionSystemListStoredToPlay.Remove (newActionToUse.mainActionSystem);
                    }

                    resumePlayer ();

                    checkIfEventOnStopAction ();

                    checkDestroyAction ();

                    checkRemoveAction ();

                    //					print (newActionToUse.Name + " stopped, resumed");
                }
            }
        }
    }

    void checkEmptyActionSystemListStored ()
    {
        if (actionSystemListStoredToPlay.Count > 0) {
            for (int k = actionSystemListStoredToPlay.Count - 1; k >= 0; k--) {
                if (actionSystemListStoredToPlay [k] == null) {
                    actionSystemListStoredToPlay.RemoveAt (k);
                }

            }
        }
    }

    bool shootingWeaponBeforeStartingActionActive;
    public void pausePlayer ()
    {
        if (showDebugPrint) {
            print ("Pause player " + mainPlayerController.name);
        }

        actionActive = true;

        //		if (currentActionInfo.stopCurrentMeleeAttackInProcess) {
        //			GKC_Utils.disableCurrentAttackInProcess (playerTransform.gameObject);
        //		}

        if (currentActionInfo.keepMeleeWeaponGrabbed) {
            GKC_Utils.keepMeleeWeaponGrabbed (playerTransform.gameObject);
        }

        if (currentActionInfo.dropGrabbedObjectsOnAction) {
            if (currentActionInfo.dropOnlyIfNotGrabbedPhysically) {
                GKC_Utils.dropObjectIfNotGrabbedPhysically (playerTransform.gameObject, currentActionInfo.dropIfGrabbedPhysicallyWithIK);
            } else {
                GKC_Utils.dropObject (playerTransform.gameObject);
            }

            GKC_Utils.checkIfKeepGrabbedObjectDuringAction (playerTransform.gameObject, currentActionInfo.keepGrabbedObjectOnActionIfNotDropped, true);

            if (!pausePlayerActivated) {
                dropGrabbedObjectsOnActionActivatedPreviously = true;
            }
        }

        eventOnStartAction.Invoke ();

        bool pauseHeadTrackResult = currentActionInfo.pauseHeadTrack;

        if (currentActionInfo.usCustomPauseHeadTrackOnFBA && mainPlayerCamera.isFullBodyAwarenessActive ()) {
            pauseHeadTrackResult = currentActionInfo.customPauseHeadTrackOnFBA;
        }

        if (pauseHeadTrackResult) {
            if (mainHeadTrack != null) {
                mainHeadTrack.setSmoothHeadTrackDisableState (true);
            }

            mainPlayerController.setHeadTrackCanBeUsedState (false);
        }

        if (currentActionInfo.actionUsesMovement) {
            mainPlayerController.setActionActiveWithMovementState (true);
        }

        if (currentActionInfo.setNoFrictionOnCollider) {
            mainPlayerController.setPhysicMaterialAssigmentPausedState (true);

            mainPlayerController.setZeroFrictionMaterial ();
        }

        if (currentActionInfo.forceRootMotionDuringAction) {
            mainPlayerController.setApplyRootMotionAlwaysActiveState (true);
        }

        if (currentActionSystem.enableAnimatorLayerOnAction) {
            mainPlayerController.enableAnimatorLayerWeight (currentActionSystem.animatorLayerToEnableName);
        }

        if (!pausePlayerActivated) {
            isFirstPersonActive = mainPlayerCamera.isFirstPersonActive ();

            actionActivatedOnFirstPerson = false;

            firstPersonViewPreviouslyActive = false;

            bool canActiveChangeToThirdPersonView = false;

            if (currentActionSystem.changeCameraViewToThirdPersonOnAction || changeCameraViewToThirdPersonOnActionOnAnyAction) {
                canActiveChangeToThirdPersonView = true;
            }

            if (currentActionSystem.actionsCanBeUsedOnFirstPerson) {
                if (currentActionSystem.ignoreChangeToThirdPerson) {
                    canActiveChangeToThirdPersonView = false;

                    if (isFirstPersonActive) {
                        actionActivatedOnFirstPerson = true;
                    }
                }
            }

            if (canActiveChangeToThirdPersonView) {
                if (isFirstPersonActive) {
                    mainPlayerCamera.changeCameraToThirdOrFirstView ();

                    firstPersonViewPreviouslyActive = true;
                }
            }
        }

        if (setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive) {
            mainPlayerCamera.setPivotCameraTransformOriginalParent ();

            mainPlayerCamera.resetPivotCameraTransformLocalRotation ();

            if (newHeadTrackTargetUsed) {
                mainHeadTrack.setOriginalCameraTargetToLook ();
            }
        }

        if (fullBodyAwarenessPreviusolyActive) {
            if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                mainPlayerCamera.changeCameraToThirdOrFirstView ();
            }
        }

        setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = false;

        newHeadTrackTargetUsed = false;

        fullBodyAwarenessPreviusolyActive = false;

        if (!isFirstPersonActive) {
            if (currentActionSystem.changeCameraViewToThirdPersonOnActionOnFullBodyAwareness) {
                if (!currentActionSystem.ignoreChangeToThirdPerson) {
                    if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.changeCameraToThirdOrFirstView ();

                        fullBodyAwarenessPreviusolyActive = true;
                    }
                }
            }

            if (currentActionSystem.keepCameraRotationToHeadOnFullBodyAwareness) {
                if (!currentActionSystem.ignoreChangeToThirdPerson) {
                    if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.setPivotCameraTransformParentCurrentTransformToFollow ();

                        if (currentActionInfo.setCustomHeadTrackTargetToLook) {
                            float newAngle = mainPlayerCamera.pivotCameraTransform.localEulerAngles.x;

                            if (newAngle > 180) {
                                newAngle -= 360;
                            }

                            mainPlayerCamera.setLookAngleValue (new Vector2 (0, newAngle));

                            if (currentActionInfo.useCustomHeadTrackTolook) {
                                mainHeadTrack.setCustomHeadTrackToLookInfoByName (currentActionInfo.customHeadTrackToLookName);
                            } else {
                                mainHeadTrack.setNewCameraTargetToLookAndAdjustDirectionToPrevious (currentActionInfo.customHeadTrackTargetToLook);
                            }

                            newHeadTrackTargetUsed = true;
                        }

                        setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = true;
                    }
                }
            }

            if (currentActionInfo.useExtraFollowTransformPositionOffsetActiveFBA) {
                if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                    mainPlayerCamera.setExtraFollowTransformPositionOffsetFBA (currentActionInfo.currentExtraFollowTransformPositionOffsetFBA);

                    useExtraFollowTransformPositionOffsetActiveFBAPreviouslyActive = true;
                }
            }

            if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                if (currentActionInfo.playerCameraTransformFollowsPlayerTransformRotationOnFBA) {
                    mainPlayerCamera.setPlayerCameraTransformFollowsPlayerTransformRotationOnFBAState (true);

                    playerCameraTransformFollowsPlayerTransformRotationOnFBA = true;
                }

                if (currentActionInfo.ignorePlayerRotationToCameraOnFBA) {
                    mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (true);

                    ignorePlayerRotationToCameraOnFBA = true;
                }

                if (currentActionInfo.ignoreHorizontalCameraRotationOnFBA) {
                    mainPlayerCamera.setIgnoreHorizontalCameraRotationOnFBAState (true);

                    ignoreHorizontalCameraRotationOnFBA = true;
                }

                if (currentActionInfo.ignoreVerticalCameraRotationOnFBA) {
                    mainPlayerCamera.setIgnoreVerticalCameraRotationOnFBAState (true);

                    ignoreVerticalCameraRotationOnFBA = true;
                }
            }
        }

        //Set input state
        if (currentActionInfo.pausePlayerActionsInput) {
            mainPlayerController.setPlayerActionsInputEnabledState (false);
        }

        if (currentActionInfo.pausePlayerCameraActionsInput) {
            mainPlayerCamera.setCameraActionsInputEnabledState (false);
        }

        if (currentActionInfo.pausePlayerCameraViewChange) {
            mainPlayerCamera.setPausePlayerCameraViewChangeState (true);
        }

        if (currentActionInfo.pausePlayerCameraMouseWheel) {
            mainPlayerCamera.setMoveCameraPositionWithMouseWheelActiveState (false);
        }

        if (currentActionInfo.pausePlayerCameraRotationInput) {
            mainPlayerCamera.changeCameraRotationState (false);
        }

        if (currentActionInfo.pausePlayerMovementInput) {
            if (currentActionInfo.resetPlayerMovementInput) {
                mainPlayerController.changeScriptState (false);
            } else {
                if (currentActionInfo.resetPlayerMovementInputSmoothly) {
                    mainPlayerController.smoothChangeScriptState (false);
                } else {
                    mainPlayerController.setCanMoveState (false);

                    if (currentActionInfo.removePlayerMovementInputValues) {
                        mainPlayerController.resetPlayerControllerInput ();
                    }
                }

                mainPlayerController.resetOtherInputFields ();
            }
        }

        if (currentActionInfo.pauseInteractionButton) {
            if (mainUsingDevicesSystem != null) {
                mainUsingDevicesSystem.setUseDeviceButtonEnabledState (false);
            }
        }

        if (currentActionInfo.actionCanHappenOnAir) {
            mainPlayerController.setActionCanHappenOnAirState (true);
        }

        mainPlayerController.setActionActiveState (true);

        if (currentActionInfo.disablePlayerGravity) {
            float currentAnimationDuration = 0;

            if (actionActivatedOnFirstPerson) {
                currentAnimationDuration = currentActionInfo.actionDurationOnFirstPerson;
            } else {
                currentAnimationDuration = currentActionInfo.animationDuration;
            }

            float animationDuration = currentAnimationDuration / currentActionInfo.animationSpeed;

            mainPlayerController.overrideOnGroundAnimatorValue (animationDuration + 0.5f);

            mainPlayerController.setGravityForcePuase (true);

            mainPlayerController.setCheckOnGroungPausedState (true);

            mainPlayerController.setPlayerVelocityToZero ();

            mainPlayerController.setPlayerOnGroundAnimatorStateOnOverrideOnGround (!currentActionInfo.disablePlayerOnGroundState);
        }

        if (mainHeadbob) {
            if (currentActionInfo.pauseHeadBob) {
                mainHeadbob.stopAllHeadbobMovements ();
            }

            if (currentActionInfo.disableHeadBob) {
                mainHeadbob.playOrPauseHeadBob (false);
            }

            if (currentActionInfo.pauseHeadBob) {
                mainPlayerCamera.stopShakeCamera ();
            }
        }

        if (currentActionInfo.disablePlayerCollider) {
            mainCollider.isTrigger = true;
        }

        if (currentActionInfo.disablePlayerColliderComponent) {
            mainCollider.enabled = false;
        }

        if (currentActionInfo.ignoreCameraDirectionOnMovement) {
            mainPlayerController.setIgnoreCameraDirectionOnMovementState (true);
        }

        if (currentActionInfo.ignoreCameraDirectionOnStrafeMovement) {
            mainPlayerController.setIgnoreCameraDirectionOnStrafeMovementState (true);
        }

        if (currentActionInfo.disableIKOnFeet) {
            if (mainIKFootSystem != null) {
                mainIKFootSystem.setIKFootPausedState (true);
            }
        }

        if (currentActionInfo.disableIKOnHands) {
            if (mainHandsOnSurfaceIKSystem != null) {
                mainHandsOnSurfaceIKSystem.setSmoothBusyDisableActiveState (true);
                mainHandsOnSurfaceIKSystem.setAdjustHandsPausedState (true);
            }
        }

        if (!pausePlayerActivated) {

            if (currentActionInfo.pauseStrafeState) {

                strafeModeActivePreviously = mainPlayerController.isStrafeModeActive ();

                if (usingAINavmesh) {
                    strafeModeActivePreviously = mainPlayerController.isLookAlwaysInCameraDirectionActive ();
                }

                mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (false);
            }

            if (currentActionInfo.disableHUDOnAction) {
                if (pauseManager) {
                    pauseManager.setIgnoreChangeHUDElementsState (true);

                    pauseManager.enableOrDisablePlayerHUD (false);

                    pauseManager.enableOrDisableDynamicElementsOnScreen (false);

                    hudDisabledDuringAction = true;
                }
            }

            carryingWeaponsPreviously = mainPlayerWeaponsManager.isPlayerCarringWeapon ();

            aimingWeaponsPrevously = mainPlayerWeaponsManager.isAimingWeapons ();

            shootingWeaponBeforeStartingActionActive = false;

            if (carryingWeaponsPreviously) {
                if (lastTimeWeaponsKept == 0 || Time.time > lastTimeWeaponsKept + 0.3f) {
                    bool stopShootingWeaponActivated = false;

                    if (currentActionInfo.stopShootOnFireWeapons) {
                        stopShootingWeaponActivated = mainPlayerWeaponsManager.stopShootingFireWeaponIfActive ();
                    }

                    shootingWeaponBeforeStartingActionActive = mainPlayerWeaponsManager.isCharacterShooting () &&
                        mainPlayerWeaponsManager.isUsingFreeFireMode ();

                    if (aimingWeaponsPrevously) {
                        if (currentActionInfo.stopAimOnFireWeapons) {
                            mainPlayerWeaponsManager.setAimWeaponState (false);
                        }
                    }

                    if (currentActionInfo.keepWeaponsDuringAction) {
                        if (showDebugPrint) {
                            print ("check to keep weapons");
                        }

                        mainPlayerWeaponsManager.checkIfDisableCurrentWeapon ();

                        mainPlayerWeaponsManager.resetWeaponHandIKWeight ();
                    } else if (currentActionInfo.disableIKWeaponsDuringAction) {
                        if (!stopShootingWeaponActivated) {
                            mainPlayerWeaponsManager.stopShootingFireWeaponIfActive ();
                        }

                        mainPlayerWeaponsManager.enableOrDisableIKOnWeaponsDuringAction (false);
                    }

                    lastTimeWeaponsKept = Time.time;
                }
            }

            if (currentActionInfo.hideMeleWeaponMeshOnAction) {
                GKC_Utils.enableOrDisableMeleeWeaponMeshActiveState (playerTransform.gameObject, false);
            }

            usingPowersPreviosly = mainOtherPowers.isAimingPower ();

            if (isFirstPersonActive) {
                usingPowersPreviosly = false;
            }

            if (usingPowersPreviosly) {
                mainOtherPowers.enableOrDisableIKOnPowersDuringAction (false);
            }

            if (!pausePlayerInputDuringWalk) {
                checkIfPauseInputListDuringActionActive (true);
            }

            if (currentActionInfo.useNewCameraStateOnActionStart) {
                if (currentActionInfo.newCameraStateNameOnActionStart != "") {
                    previousCameraStateName = "";

                    if (currentActionInfo.setPreviousCameraStateOnActionEnd) {
                        previousCameraStateName = mainPlayerCamera.getCurrentStateName ();
                    }

                    if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.setCameraStateExternally (currentActionInfo.newCameraStateNameOnActionStart);
                    }
                }
            }

            if (currentActionInfo.setInvincibilityStateActive) {
                mainHealth.setInvincibleStateDurationWithoutDisableDamageOverTime (currentActionInfo.invincibilityStateDuration);

                if (currentActionInfo.checkEventsOnTemporalInvincibilityActive) {
                    mainHealth.setCheckEventsOnTemporalInvincibilityActiveState (true);
                }
            }

            if (currentActionInfo.disableDamageReactionDuringAction) {
                mainHealth.setDamageReactionPausedState (true);
            }

            if (!currentActionInfo.setHealthOnActionEnd) {
                if (currentActionInfo.addHealthAmountOnAction) {
                    mainHealth.addHealth (currentActionInfo.healthAmountToAdd);
                }

                if (currentActionInfo.removeHealthAmountOnAction) {
                    bool isDamageReactionPaused = mainHealth.isDamageReactionPaused ();

                    mainHealth.setDamageReactionPausedState (true);

                    mainHealth.takeHealth (currentActionInfo.healthAmountToRemove);

                    mainHealth.setDamageReactionPausedState (isDamageReactionPaused);
                }
            }

            pausePlayerActivated = true;

            mainPlayerController.setAllowDownVelocityDuringActionState (currentActionInfo.allowDownVelocityDuringAction);

            if (currentActionSystem.setPlayerParentDuringActionActive) {
                mainPlayerController.setPlayerAndCameraParent (currentActionSystem.playerParentDuringAction);
            }

            if (currentActionInfo.ignorePivotCameraCollision) {
                mainPlayerCamera.setIgnorePivotCameraCollisionActiveState (true);
            }

            mainPlayerController.setUpdate2_5dClampedPositionPausedState (true);
        }

        activateCustomActionsPaused = currentActionInfo.pauseActivationOfOtherCustomActions;
    }

    public void clearActionSystemListStoredToPlay ()
    {
        actionSystemListStoredToPlay.Clear ();
    }

    public void stopAllActions ()
    {
        if (actionActive || walkingToDirectionActive || rotatingTowardFacingDirection) {
            clearActionSystemListStoredToPlay ();

            stopCheckActionEventInfoList ();

            stopCheckKillOrEnterOnRagdollState ();

            if (customActionActive) {
                stopCustomAction (currentCustomActionName);
            } else {
                resumePlayer ();

                checkIfEventOnStopAction ();

                checkDestroyAction ();

                checkRemoveAction ();
            }

            stopCheckSetObjectParent ();

            stopPlayAnimationCoroutine ();

            stopSetPlayerWalkState ();

            stopCheckSetObjectParent ();

            stopMovePlayerOnDirectionCorotuine ();
        }
    }

    float lastTimeWeaponsKept = 0;

    public void resetAllActionStates ()
    {
        actionActive = false;

        setActionFoundState (false);

        currentActionCategoryActive = "";

        setActionMovementInputActiveState (false);

        currentAnimationChecked = false;

        waitingForNextPlayerInput = false;

        playingAnimation = false;

        startAction = false;

        carryingWeaponsPreviously = false;

        usingPowersPreviosly = false;

        pausePlayerActivated = false;

        pausePlayerInputDuringWalk = false;

        previousCameraStateName = "";

        customActionActive = false;

        removeActionFoundWaitingToPlayerResume ();

        currentActionSystem = null;

        currentActionInfo = null;

        currentActionName = "";

        currentActionSystemGameObject = null;

        currentActionSystemTransform = null;

        dropGrabbedObjectsOnActionActivatedPreviously = false;

        usePositionToAdjustPlayerActive = false;

        waitToPlayAnimationAfterAdjustPositionActive = false;
    }

    public void checkIfPauseInputListDuringActionActive (bool state)
    {
        if ((pauseInputListDuringActionActiveAlways || currentActionInfo.pauseInputListDuringActionActive) && !currentActionInfo.ignorePauseInputListDuringAction) {
            if (mainPlayerInputManager != null) {

                if (currentActionInfo.pauseCustomInputListDuringActionActive) {
                    //					print ("input " + currentActionInfo.actionName + " " + state);
                    checkInputListToPauseDuringAction (currentActionInfo.customInputToPauseOnActionInfoList, state);
                } else {
                    checkInputListToPauseDuringAction (inputToPauseOnActionIfoList, state);
                }
            }
        }
    }

    public void checkInputListToPauseDuringAction (List<inputToPauseOnActionIfo> inputList, bool state)
    {
        if (ignoreInputChangeActive) {
            return;
        }

        int inputListCount = inputList.Count;

        for (int i = 0; i < inputListCount; i++) {

            if (state) {
                inputList [i].previousActiveState = mainPlayerInputManager.setPlayerInputMultiAxesStateAndGetPreviousState (false, inputList [i].inputName);
            } else {
                if (inputList [i].previousActiveState) {
                    mainPlayerInputManager.setPlayerInputMultiAxesState (inputList [i].previousActiveState, inputList [i].inputName);
                }
            }
        }
    }

    public void updateCurrentInputList ()
    {
        if (ignoreInputChangeActive) {
            return;
        }

        if (mainPlayerInputManager != null) {
            int inputToPauseOnActionIfoListCount = inputToPauseOnActionIfoList.Count;

            for (int i = 0; i < inputToPauseOnActionIfoListCount; i++) {
                inputToPauseOnActionIfoList [i].previousActiveState = mainPlayerInputManager.getPlayerInputMultiAxesState (inputToPauseOnActionIfoList [i].inputName);
            }
        }
    }

    public void setIgnoreInputChangeActiveState (bool state)
    {
        ignoreInputChangeActive = state;
    }

    public void resumePlayer ()
    {
        if (showDebugPrint) {
            print ("RESUMING");

            print ("resume player " + mainPlayerController.name);
        }

        actionActive = false;

        currentActionCategoryActive = "";

        if (currentActionInfo.dropGrabbedObjectsOnAction) {
            GKC_Utils.checkIfKeepGrabbedObjectDuringAction (playerTransform.gameObject, currentActionInfo.keepGrabbedObjectOnActionIfNotDropped, false);
        } else {
            if (dropGrabbedObjectsOnActionActivatedPreviously) {
                GKC_Utils.disableKeepGrabbedObjectStateAfterAction (playerTransform.gameObject);

                if (showDebugPrint) {
                    print ("carrying weapon previously and it was set as kept, setting the state back to regular value");
                }
            }
        }

        eventOnEndAction.Invoke ();

        bool playerIsAlive = !mainPlayerController.isPlayerDead ();

        if (mainHeadTrack != null) {
            mainHeadTrack.setSmoothHeadTrackDisableState (false);
        }

        if (!mainPlayerController.isRagdollCurrentlyActive ()) {
            mainPlayerController.setHeadTrackCanBeUsedState (true);
        }

        mainPlayerController.setActionActiveWithMovementState (false);

        mainPlayerController.setPhysicMaterialAssigmentPausedState (false);

        if (!currentActionInfo.ignoreSetLastTimeFallingOnActionEnd) {
            mainPlayerController.setLastTimeFalling ();
        }

        //Set input state
        if (currentActionInfo.pausePlayerActionsInput) {
            mainPlayerController.setPlayerActionsInputEnabledState (true);
        }

        if (currentActionInfo.pausePlayerCameraActionsInput) {
            mainPlayerCamera.setCameraActionsInputEnabledState (true);
        }

        if (currentActionInfo.pausePlayerCameraViewChange) {
            mainPlayerCamera.setPausePlayerCameraViewChangeState (false);
        }

        if (currentActionInfo.pausePlayerCameraMouseWheel) {
            mainPlayerCamera.setOriginalMoveCameraPositionWithMouseWheelActiveState ();
        }

        if (currentActionInfo.resetCameraRotationForwardOnFBA) {
            if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                mainPlayerCamera.setPlayerCameraTransformRotation (playerTransform.rotation);
                mainPlayerCamera.setLookAngleValue (Vector2.zero);
            }
        }

        if (playerIsAlive) {
            if (currentActionInfo.pausePlayerCameraRotationInput) {
                mainPlayerCamera.changeCameraRotationState (true);
            }

            if (currentActionInfo.pausePlayerMovementInput) {
                if (currentActionInfo.resetPlayerMovementInput) {
                    mainPlayerController.changeScriptState (true);
                } else {
                    mainPlayerController.setCanMoveState (true);
                }
            }

            if (currentActionInfo.enablePlayerCanMoveAfterAction) {
                mainPlayerController.setCanMoveState (true);
            }
        }

        if (currentActionInfo.pauseInteractionButton) {
            if (mainUsingDevicesSystem != null) {
                mainUsingDevicesSystem.setUseDeviceButtonEnabledState (true);
            }
        }

        mainPlayerController.setApplyRootMotionAlwaysActiveState (false);

        mainPlayerController.setActionActiveState (false);

        mainPlayerController.setActionCanHappenOnAirState (false);

        mainPlayerController.setGravityForcePuase (false);

        mainPlayerController.setCheckOnGroungPausedState (false);

        mainCollider.isTrigger = false;

        if (currentActionInfo.disablePlayerColliderComponent) {
            if (currentActionInfo.enablePlayerColliderComponentOnActionEnd) {
                mainCollider.enabled = true;
            }
        }

        if (currentActionInfo.reloadMainColliderOnCharacterOnActionEnd) {
            mainPlayerController.reactivateColliderIfPossible ();
        }

        mainAnimator.SetInteger (actionIDAnimatorID, 0);

        if (currentActionSystem.animationUsedOnUpperBody) {
            mainAnimator.SetBool (actionActiveUpperBodyAnimatorID, false);

            if (currentActionSystem.disableRegularActionActiveStateOnEnd) {
                mainAnimator.SetBool (actionActiveAnimatorID, false);
            }
        } else {
            mainAnimator.SetBool (actionActiveAnimatorID, false);
        }

        if (mainHeadbob) {
            if (currentActionInfo.pauseHeadBob || currentActionInfo.disableHeadBob) {
                mainHeadbob.pauseHeadBodWithDelay (0.5f);

                mainHeadbob.playOrPauseHeadBob (true);
            }
        }

        mainPlayerController.setIgnoreCameraDirectionOnMovementState (false);

        mainPlayerController.setIgnoreCameraDirectionOnStrafeMovementState (false);

        setActionMovementInputActiveState (false);

        currentAnimationChecked = false;

        waitingForNextPlayerInput = false;

        if (currentActionInfo.disableIKOnFeet) {
            if (mainIKFootSystem != null) {
                mainIKFootSystem.setIKFootPausedState (false);
            }
        }

        if (currentActionInfo.disableIKOnHands) {
            if (mainHandsOnSurfaceIKSystem != null) {
                mainHandsOnSurfaceIKSystem.setSmoothBusyDisableActiveState (false);
                mainHandsOnSurfaceIKSystem.setAdjustHandsPausedState (false);
            }
        }

        if (currentActionInfo.ignorePivotCameraCollision) {
            mainPlayerCamera.setIgnorePivotCameraCollisionActiveState (false);
        }

        if (useExtraFollowTransformPositionOffsetActiveFBAPreviouslyActive) {
            mainPlayerCamera.setExtraFollowTransformPositionOffsetFBA (Vector3.zero);
        }

        if (playerCameraTransformFollowsPlayerTransformRotationOnFBA) {
            mainPlayerCamera.setPlayerCameraTransformFollowsPlayerTransformRotationOnFBAState (false);
        }

        if (ignorePlayerRotationToCameraOnFBA) {
            mainPlayerCamera.setIgnorePlayerRotationToCameraOnFBAState (false);

            if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
                mainPlayerController.setFullBodyAwarenessActiveStateExternally (true);
            }
        }

        if (ignoreHorizontalCameraRotationOnFBA) {
            mainPlayerCamera.setIgnoreHorizontalCameraRotationOnFBAState (false);
        }

        if (ignoreVerticalCameraRotationOnFBA) {
            mainPlayerCamera.setIgnoreVerticalCameraRotationOnFBAState (false);
        }

        playingAnimation = false;

        startAction = false;

        actionSystem temporalCurrentActionSystem = currentActionSystem;

        currentActionSystem.checkEventOnEndAction ();

        checkSetActionState ();

        if (hudDisabledDuringAction) {
            if (pauseManager) {
                pauseManager.setIgnoreChangeHUDElementsState (false);

                pauseManager.enableOrDisablePlayerHUD (true);

                pauseManager.enableOrDisableDynamicElementsOnScreen (true);
            }

            hudDisabledDuringAction = false;
        }

        bool isRagdollCurrentlyActive = mainPlayerController.isRagdollCurrentlyActive ();

        if (playerIsAlive) {
            if (!currentActionSystem.activateCustomActionAfterActionComplete) {
                if (carryingWeaponsPreviously) {
                    if (!isRagdollCurrentlyActive) {
                        if (currentActionInfo.drawWeaponsAfterAction) {
                            mainPlayerWeaponsManager.checkIfDrawWeaponWithAnimationAfterAction ();

                            mainPlayerWeaponsManager.checkIfDrawSingleOrDualWeapon ();
                        } else if (currentActionInfo.disableIKWeaponsDuringAction) {
                            mainPlayerWeaponsManager.enableOrDisableIKOnWeaponsDuringAction (true);
                        }

                        if (shootingWeaponBeforeStartingActionActive) {
                            if (mainPlayerWeaponsManager.isPlayerCarringWeapon () &&
                                mainPlayerWeaponsManager.isAimingWeapons ()) {
                                mainPlayerWeaponsManager.enableFreeFireModeState ();

                                mainPlayerWeaponsManager.setAimingWeaponFromShootingState (true);

                                mainPlayerWeaponsManager.setLastTimeFired ();

                                if (showDebugPrint) {
                                    print ("resume free fire mode state");
                                }
                            }
                        }
                    }

                    carryingWeaponsPreviously = false;
                }

                if (usingPowersPreviosly) {
                    mainOtherPowers.enableOrDisableIKOnPowersDuringAction (true);

                    usingPowersPreviosly = false;
                }

                if (currentActionInfo.hideMeleWeaponMeshOnAction) {
                    GKC_Utils.enableOrDisableMeleeWeaponMeshActiveState (playerTransform.gameObject, true);
                }
            }

            if (currentActionInfo.pauseStrafeState) {
                mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (strafeModeActivePreviously);
            }
        } else {
            mainPlayerWeaponsManager.setActionActiveState (false);

            carryingWeaponsPreviously = false;

            mainOtherPowers.setActionActiveState (false);

            usingPowersPreviosly = false;
        }

        if (mainPlayerCamera.isFullBodyAwarenessActive ()) {
            mainPlayerController.activateOrDeactivateStrafeModeDuringActionSystem (true);

            mainPlayerController.setFullBodyAwarenessActiveStateExternally (true);
        }

        pausePlayerActivated = false;

        checkIfPauseInputListDuringActionActive (false);

        pausePlayerInputDuringWalk = false;

        strafeModeActivePreviously = false;

        updateCurrentInputList ();

        if (playerIsAlive) {
            if (currentActionInfo.useNewCameraStateOnActionStart) {
                if (currentActionInfo.setPreviousCameraStateOnActionEnd && previousCameraStateName != "") {
                    if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.setCameraStateExternally (previousCameraStateName);
                    }

                    previousCameraStateName = "";

                    if (!mainPlayerCamera.isFirstPersonActive () && mainPlayerCamera.isCameraTypeFree ()) {
                        if (mainPlayerWeaponsManager.isAimingWeapons ()) {
                            mainPlayerCamera.activateAiming ();
                        }
                    }
                }

                if (currentActionInfo.useNewCameraStateOnActionEnd && currentActionInfo.newCameraStateNameOnActionEnd != "") {
                    if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                        mainPlayerCamera.setCameraStateExternally (currentActionInfo.newCameraStateNameOnActionEnd);
                    }
                }
            }

            if (currentActionSystem.changeCameraViewToThirdPersonOnAction || changeCameraViewToThirdPersonOnActionOnAnyAction) {
                if (firstPersonViewPreviouslyActive) {
                    mainPlayerCamera.changeCameraToThirdOrFirstView ();
                }
            }

            if (fullBodyAwarenessPreviusolyActive) {
                if (!mainPlayerCamera.isFullBodyAwarenessActive ()) {
                    mainPlayerCamera.changeCameraToThirdOrFirstView ();
                }
            }

            if (setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive) {
                mainPlayerCamera.setPivotCameraTransformOriginalParent ();

                mainPlayerCamera.resetPivotCameraTransformLocalRotation ();

                if (newHeadTrackTargetUsed) {
                    mainHeadTrack.setOriginalCameraTargetToLook ();
                }
            }
        } else {
            previousCameraStateName = "";
        }

        customActionActive = false;

        firstPersonViewPreviouslyActive = false;

        fullBodyAwarenessPreviusolyActive = false;

        useExtraFollowTransformPositionOffsetActiveFBAPreviouslyActive = false;

        ignorePlayerRotationToCameraOnFBA = false;

        playerCameraTransformFollowsPlayerTransformRotationOnFBA = false;

        ignoreHorizontalCameraRotationOnFBA = false;

        ignoreVerticalCameraRotationOnFBA = false;

        setPivotCameraTransformParentCurrentTransformToFollowPreviouslyActive = false;

        newHeadTrackTargetUsed = false;

        actionActivatedOnFirstPerson = false;

        isFirstPersonActive = false;

        if (playerIsAlive) {
            if (actionFoundWaitingToPlayerResume) {
                if (showDebugPrint) {
                    print ("action waiting for resume to being activated");
                }

                if (actionFoundWaitingResume) {
                    setPlayerActionActive (actionFoundWaitingResume);
                }

                removeActionFoundWaitingToPlayerResume ();
            }
        } else {
            removeActionFoundWaitingToPlayerResume ();
        }

        mainPlayerController.setOverrideTurnAmount (0, false);

        //		print ("resuming from action " + currentActionInfo.Name + " " + currentActionInfo.resumeAIOnActionEnd + " " + playerIsAlive + " " + usingAINavmesh);

        if (playerIsAlive) {
            if (currentActionInfo.crouchOnActionEnd) {
                mainPlayerController.setCrouchState (true);
            } else {
                if (currentActionInfo.setPreviousCrouchStateOnActionEnd) {
                    mainPlayerController.setCrouchState (playerCrouchingPreviously);
                }
            }

            if (usingAINavmesh) {
                if (currentActionInfo.resumeAIOnActionEnd && !isRagdollCurrentlyActive) {
                    mainAINavmesh.pauseAI (false);

                    if (currentActionInfo.assignPartnerOnActionEnd && mainAINavmesh.partnerLocated) {
                        mainAINavmesh.setTarget (mainAINavmesh.getCurrentPartner ());

                        mainAINavmesh.setTargetType (true, false);
                    } else {
                        mainFindObjectivesSystem.checkPauseOrResumePatrolStateDuringActionActive (false);
                    }
                }

                if (currentActionInfo.setMoveNavMeshPaused) {
                    mainAINavmesh.setMoveNavMeshPausedDuringActionState (false);
                }
            } else {
                if (mainPlayerNavMeshSystem != null) {
                    if (navmeshUsedOnAction) {

                        //						print ("navmesh used on action");

                        if (mainPlayerNavMeshSystem.isUsingPlayerNavmeshExternallyActive () ||
                            mainPlayerNavMeshSystem.isPlayerNavMeshEnabled ()) {

                            mainPlayerNavMeshSystem.setPlayerNavMeshEnabledState (false);

                            mainPlayerNavMeshSystem.setUsingPlayerNavmeshExternallyState (false);

                            mainPlayerNavMeshSystem.disableCustomNavMeshSpeed ();

                            mainPlayerNavMeshSystem.disableCustomMinDistance ();

                            mainPlayerNavMeshSystem.setShowCursorPausedState (false);
                        }
                    }

                    if (navmeshPreviouslyActive) {

                        print ("navmesh previously active");
                        mainPlayerNavMeshSystem.setPlayerNavMeshEnabledState (true);
                    }
                }
            }

            if (currentActionInfo.activateDynamicObstacleDetection) {
                if (usingAINavmesh) {
                    mainAINavmesh.setOriginalUseDynamicObstacleDetection ();
                } else {
                    mainPlayerNavMeshSystem.setOriginalUseDynamicObstacleDetection ();
                }
            }

            if (currentActionInfo.drawMeleeWeaponGrabbedOnActionEnd) {
                GKC_Utils.drawMeleeWeaponGrabbedCheckingAnimationDelay (playerTransform.gameObject);
            }
        }

        mainPlayerController.setExtraCharacterVelocity (Vector3.zero);

        activateCustomActionsPaused = false;

        navmeshPreviouslyActive = false;
        navmeshUsedOnAction = false;

        if (temporalCurrentActionSystem.useEventAfterResumePlayer) {
            StartCoroutine (checkEventAfterResumePlayerCoroutine (temporalCurrentActionSystem));
        }

        mainPlayerController.setAllowDownVelocityDuringActionState (false);

        dropGrabbedObjectsOnActionActivatedPreviously = false;

        if (movePlayerOnDirectionActive) {
            stopMovePlayerOnDirectionCorotuine ();
        }

        mainHealth.setDamageReactionPausedState (false);

        if (currentActionSystem.setPlayerParentDuringActionActive) {
            mainPlayerController.setPlayerAndCameraParent (null);
        }

        eventAfterEndAction.Invoke ();

        currentActionDuration = 0;

        usePositionToAdjustPlayerActive = false;

        waitToPlayAnimationAfterAdjustPositionActive = false;

        if (currentActionInfo.setHealthOnActionEnd) {
            if (currentActionInfo.addHealthAmountOnAction) {
                mainHealth.addHealth (currentActionInfo.healthAmountToAdd);
            }

            if (currentActionInfo.removeHealthAmountOnAction) {
                bool isDamageReactionPaused = mainHealth.isDamageReactionPaused ();

                mainHealth.setDamageReactionPausedState (true);

                mainHealth.takeHealth (currentActionInfo.healthAmountToRemove);

                mainHealth.setDamageReactionPausedState (isDamageReactionPaused);
            }
        }

        mainPlayerController.setUpdate2_5dClampedPositionPausedState (false);
    }

    IEnumerator checkEventAfterResumePlayerCoroutine (actionSystem temporalCurrentActionSystem)
    {
        WaitForSeconds delay = new WaitForSeconds (0.1f);

        yield return delay;

        temporalCurrentActionSystem.checkEventAfterResumePlayer ();
    }

    public void removeActionFoundWaitingToPlayerResume ()
    {
        actionFoundWaitingToPlayerResume = false;
        actionFoundWaitingResume = null;
    }

    public void checkDestroyAction ()
    {
        if (currentActionInfo != null && currentActionInfo.destroyActionOnEnd) {
            currentActionSystem.destroyAction ();

            setPlayerActionDeactivate ();

            currentActionSystem = null;

            currentActionInfo = null;

            currentActionName = "";

            currentActionSystemGameObject = null;
        }
    }

    public void checkRemoveAction ()
    {
        if (currentActionInfo != null && currentActionInfo.removeActionOnEnd) {
            currentActionSystem.removePlayerFromList (playerTransform.gameObject);

            if (showDebugPrint) {
                print ("remove action finished " + currentActionInfo.Name + " " + mainPlayerController.name);
            }

            setPlayerActionDeactivate ();

            bool activateCustomActionAfterActionComplete = currentActionSystem.activateCustomActionAfterActionComplete;
            string customActionToActiveAfterActionComplete = currentActionSystem.customActionToActiveAfterActionComplete;

            currentActionSystem = null;

            currentActionInfo = null;

            currentActionName = "";

            currentActionSystemGameObject = null;

            stopPlayAnimationCoroutine ();

            if (activateCustomActionAfterActionComplete) {
                activateCustomAction (customActionToActiveAfterActionComplete);
            } else {

                checkEmptyActionSystemListStored ();

                if (actionSystemListStoredToPlay.Count > 0) {
                    if (actionSystemListStoredToPlay [0].addActionToListStoredToPlay) {
                        if (actionSystemListStoredToPlay [0].playActionAutomaticallyIfStoredAtEnd) {
                            activateCustomAction (actionSystemListStoredToPlay [0].actionInfoList [0].Name);
                        } else {
                            setPlayerActionActive (actionSystemListStoredToPlay [0]);
                        }
                    }
                }
            }
        }
    }

    public void checkIfEventOnStopAction ()
    {
        if (currentActionInfo != null) {

            if (currentActionInfo.useEventIfActionStopped) {
                currentActionInfo.eventIfActionStopped.Invoke ();
            }

            if (currentActionInfo.resumeAIOnActionEnd) {
                if (mainAINavmesh != null) {
                    mainAINavmesh.pauseAI (false);

                    mainFindObjectivesSystem.checkPauseOrResumePatrolStateDuringActionActive (false);
                }

                if (currentActionInfo.setMoveNavMeshPaused) {
                    mainAINavmesh.setMoveNavMeshPausedDuringActionState (false);
                }
            }

            List<actionSystem.eventInfo> eventInfoList = currentActionInfo.eventInfoList;

            if (eventInfoListActivatedOnFirstPerson) {
                eventInfoList = currentActionInfo.firstPersonEventInfoList;

                eventInfoListActivatedOnFirstPerson = false;
            }

            actionSystem.eventInfo currentEventInfo;

            for (int i = 0; i < eventInfoList.Count; i++) {

                currentEventInfo = eventInfoList [i];

                if (currentEventInfo.callThisEventIfActionStopped) {

                    currentEventInfo.eventToUse.Invoke ();

                    if (currentEventInfo.useRemoteEvent) {
                        if (currentEventInfo.useRemoteEventNameList) {
                            for (int j = 0; j < currentEventInfo.remoteEventNameList.Count; j++) {
                                mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventNameList [j]);
                            }
                        } else {
                            mainRemoteEventSystem.callRemoteEvent (currentEventInfo.remoteEventName);
                        }
                    }

                    if (currentEventInfo.sendCurrentPlayerOnEvent) {
                        currentEventInfo.eventToSendCurrentPlayer.Invoke (playerTransform.gameObject);
                    }
                }
            }
        }
    }

    public void enableCustomActionByName (string actionName)
    {
        enableOrDisableCustomActionByName (actionName, true);
    }

    public void disableCustomActionByName (string actionName)
    {
        enableOrDisableCustomActionByName (actionName, false);
    }

    public void enableOrDisableCustomActionByName (string actionName, bool state)
    {
        int categoryIndex = -1;
        int actionIndex = -1;

        actionName = actionName.ToLower ();

        getCustomActionByName (actionName, ref categoryIndex, ref actionIndex);

        int currentCategoryIndex = categoryIndex;

        int currentActionIndex = actionIndex;

        if (currentActionIndex > -1) {
            customActionStateCategoryInfoList [currentCategoryIndex].actionStateInfoList [currentActionIndex].stateEnabled = state;
        }
    }

    public void setCustomActionTransform (string actionName, Transform newTransform)
    {
        int categoryIndex = -1;
        int actionIndex = -1;

        getCustomActionByName (actionName, ref categoryIndex, ref actionIndex);

        int currentCategoryIndex = categoryIndex;

        int currentActionIndex = actionIndex;

        if (currentActionIndex > -1) {
            customActionStateInfo newActionToUse = customActionStateCategoryInfoList [currentCategoryIndex].actionStateInfoList [currentActionIndex];

            if (newActionToUse.stateEnabled) {
                int currentCategoryActionIndex = -1;

                if (currentCustomActionCategoryID > 0 && newActionToUse.useCustomActionCategoryIDInfoList) {
                    if (!newActionToUse.canBeUsedOnAnyCustomActionCategoryID) {
                        currentCategoryActionIndex = newActionToUse.customActionCategoryIDInfoList.FindIndex (s => s.categoryID == currentCustomActionCategoryID);
                    }
                }

                if (newActionToUse.useRandomActionSystemList) {
                    if (currentCategoryActionIndex > -1) {

                        int randomActionSystemListCount = newActionToUse.customActionCategoryIDInfoList [currentCategoryActionIndex].randomActionSystemList.Count;

                        for (int j = 0; j < randomActionSystemListCount; j++) {
                            newActionToUse.customActionCategoryIDInfoList [currentCategoryActionIndex].randomActionSystemList [j].setCustomActionTransform (newTransform);
                        }
                    } else {
                        for (int j = 0; j < newActionToUse.randomActionSystemList.Count; j++) {
                            newActionToUse.randomActionSystemList [j].setCustomActionTransform (newTransform);
                        }
                    }
                } else {
                    if (currentCategoryActionIndex > -1) {
                        newActionToUse.customActionCategoryIDInfoList [currentCategoryActionIndex].mainActionSystem.setCustomActionTransform (newTransform);
                    } else {
                        checkIfSpawnActionSystem (newActionToUse);

                        newActionToUse.mainActionSystem.setCustomActionTransform (newTransform);
                    }
                }

                return;
            }
        }
    }

    public string getCurrentActionName ()
    {
        return currentActionName;
    }

    public string getCurrentActionCategoryActive ()
    {
        return currentActionCategoryActive;
    }

    public actionSystem getCurrentActionSystem ()
    {
        return currentActionSystem;
    }

    public void setNewActionIDExternally (int newID)
    {
        mainAnimator.SetInteger (actionIDAnimatorID, newID);
    }

    public void setCurrentCustomActionCategoryID (int newValue)
    {
        currentCustomActionCategoryID = newValue;
    }

    public void checkChangeOfView (bool isFirstPerson)
    {
        if (isFirstPerson) {

        } else {
            mainPlayerController.resetAnimator ();
        }
    }

    //INPUT FOR ACTION SYSTEM
    public void inputPlayCurrentAnimation ()
    {
        if (currentActionSystemGameObject != null && mainUsingDevicesSystem.existInDeviceList (currentActionSystemGameObject) &&
            mainUsingDevicesSystem.isCurrentDeviceToUseFound (currentActionSystemGameObject)) {

            if (showDebugPrint) {
                print ("pressing button to play current animation");
            }

            playCurrentAnimation ();
        } else {
            if (showDebugPrint) {
                print ("object not found or is not the current one to use in the using devices system");
            }
        }
    }

    public void inputPlayCurrentAnimationWithoutCheckingIfExistsOnaDeviceList ()
    {
        if (currentActionSystemGameObject != null) {

            if (showDebugPrint) {
                print ("pressing button to play current animation without checking for devices list");
            }

            playCurrentAnimation ();
        } else {
            if (showDebugPrint) {
                print ("object not found or is not the current one to use in the using devices system");
            }
        }
    }

    public void inputCheckIfResumePlayerOnJump ()
    {
        if (actionFound && actionActive) {
            if (currentActionInfo.jumpCanResumePlayer) {
                stopAllActions ();

                //				mainPlayerController.inputJump ();

                mainPlayerController.setJumpActive (true);
            }
        }
    }

    public sharedActionSystem getSharedActionSystem ()
    {
        return mainSharedActionSystem;
    }

    public sharedActionButtonActivator getSharedActionButtonActivator ()
    {
        return mainSharedActionButtonActivator;
    }

    public sharedActionSystemRemoteActivator getSharedActionSystemRemoteActivator ()
    {
        return mainSharedActionSystemRemoteActivator;
    }

    public bool isSharedActionActive ()
    {
        if (mainSharedActionSystem != null) {
            return mainSharedActionSystem.isSharedActionActive ();
        }

        return false;
    }

    public bool isPlayerAlive ()
    {
        return !mainPlayerController.isPlayerDead ();
    }

    //EDITOR FUNCTIONS
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

    //draw the pivot and the final positions of every door
    void DrawGizmos ()
    {
        if (showGizmo) {
            if (walkingToDirectionActive) {
                GKC_Utils.drawGizmoArrow (playerTransform.position + playerTransform.up, 3 * currentWalkDirection, Color.yellow, 2, 20);
            }
        }
    }

    public bool addNewActionFromEditor (actionSystem newActionSystem, string newActionCategoryName, string actionSystemName,
                                        bool updateActionListActive, string actionCategory)
    {
        int currentActionIndex = customActionStateCategoryInfoList.FindIndex (s => s.Name.Equals (newActionCategoryName));

        if (currentActionIndex <= -1) {
            currentActionIndex = customActionStateCategoryInfoList.FindIndex (s => s.Name.Equals ("Others"));
        }

        if (currentActionIndex > -1) {
            customActionStateCategoryInfo currentCustomActionStateCategoryInfo = customActionStateCategoryInfoList [currentActionIndex];

            customActionStateInfo newCustomActionStateInfo = new customActionStateInfo ();

            newCustomActionStateInfo.Name = actionSystemName;
            newCustomActionStateInfo.mainActionSystem = newActionSystem;

            newCustomActionStateInfo.categoryName = actionCategory;

            currentCustomActionStateCategoryInfo.actionStateInfoList.Add (newCustomActionStateInfo);

            if (updateActionListActive) {
                updateActionList (false);
            }

            return true;
        }

        return false;
    }

    public void setNewInfoOnAction (string newActionCategoryName, string originalActionSystemName, string actionSystemName, float newDuration, float newSpeed)
    {
        print ("setting new info on action");

        int currentCategoryIndex = customActionStateCategoryInfoList.FindIndex (s => s.Name.Equals (newActionCategoryName));

        if (currentCategoryIndex > -1) {
            customActionStateCategoryInfo currentCustomActionStateCategoryInfo = customActionStateCategoryInfoList [currentCategoryIndex];

            int currentActionIndex = currentCustomActionStateCategoryInfo.actionStateInfoList.FindIndex (s => s.Name.Equals (originalActionSystemName));

            print ("category found " + newActionCategoryName);

            if (currentActionIndex > -1) {

                print ("action found " + originalActionSystemName);

                customActionStateInfo currentcustomActionStateInfo = currentCustomActionStateCategoryInfo.actionStateInfoList [currentActionIndex];

                currentcustomActionStateInfo.Name = actionSystemName;

                currentcustomActionStateInfo.mainActionSystem.addNewActionFromEditor (actionSystemName, newDuration, newSpeed, false, 0, actionSystemName);

                updateActionList (false);

                GKC_Utils.updateDirtyScene ("Updating Character Action System ", gameObject);
            }
        }
    }

    public void updateActionList (bool updatingListIngame)
    {
        customActionStateInfoDictionaryList.Clear ();

        for (int i = 0; i < customActionStateCategoryInfoList.Count; i++) {

            if (!updatingListIngame) {
                print ("Category " + customActionStateCategoryInfoList [i].Name);

                print ("\n\n");
            }

            for (int j = 0; j < customActionStateCategoryInfoList [i].actionStateInfoList.Count; j++) {
                customActionStateInfoDictionary newCustomActionStateInfoDictionary = new customActionStateInfoDictionary ();

                newCustomActionStateInfoDictionary.Name = customActionStateCategoryInfoList [i].actionStateInfoList [j].Name;

                newCustomActionStateInfoDictionary.categoryIndex = i;
                newCustomActionStateInfoDictionary.actionIndex = j;

                customActionStateInfoDictionaryList.Add (newCustomActionStateInfoDictionary);

                if (!updatingListIngame) {
                    print ("Action " + newCustomActionStateInfoDictionary.Name);
                }
            }

            if (!updatingListIngame) {
                print ("\n\n");
                print ("\n\n");
            }
        }

        if (!updatingListIngame) {
            print ("\n\n");
            print ("Custom Action List Updated with a total of " + customActionStateInfoDictionaryList.Count + " actions configured");
        }

        if (!updatingListIngame) {
            updateComponent ();

            GKC_Utils.updateDirtyScene ("Updating Character Action System ", gameObject);
        }
    }

    public void checkActions ()
    {
        int actionsCounter = 0;

        for (int j = 0; j < customActionStateCategoryInfoList.Count; j++) {

            for (int k = 0; k < customActionStateCategoryInfoList [j].actionStateInfoList.Count; k++) {
                actionsCounter++;
            }
        }

        for (int j = 0; j < customActionStateCategoryInfoList.Count; j++) {

            for (int k = 0; k < customActionStateCategoryInfoList [j].actionStateInfoList.Count; k++) {
                if (checkDuplicated (customActionStateCategoryInfoList [j].actionStateInfoList [k].Name) > 1) {
                    print (customActionStateCategoryInfoList [j].actionStateInfoList [k].Name + " duplicated");
                }
            }
        }


        print ("actions counter " + actionsCounter);
    }

    int checkDuplicated (string actionName)
    {
        int counter = 0;
        for (int j = 0; j < customActionStateCategoryInfoList.Count; j++) {

            for (int k = 0; k < customActionStateCategoryInfoList [j].actionStateInfoList.Count; k++) {
                if (customActionStateCategoryInfoList [j].actionStateInfoList [k].Name == actionName) {
                    counter++;
                }
            }
        }

        return counter;
    }

    public bool checkActionSystemAlreadyExists (string actionName)
    {
        if (checkDuplicated (actionName) > 0) {
            return true;
        }

        return false;
    }

    public void movePlayerToPositionTarget (Transform targetTransform)
    {
        stopMovePlayerToPositionTargetCoroutine ();

        movePlayerToTargetCoroutine = StartCoroutine (movePlayerToPositionTargetCoroutine (targetTransform));
    }

    public void stopMovePlayerToPositionTargetCoroutine ()
    {
        if (movePlayerToTargetCoroutine != null) {
            StopCoroutine (movePlayerToTargetCoroutine);
        }

        movingPlayerToPositionTargetActive = false;
    }

    IEnumerator movePlayerToPositionTargetCoroutine (Transform targetTransform)
    {
        movingPlayerToPositionTargetActive = true;

        float dist = GKC_Utils.distance (playerTransform.position, targetTransform.position);

        float duration = dist / currentActionInfo.movingPlayerToPositionTargetSpeed;

        float translateTimer = 0;

        float teleportTimer = 0;

        bool targetReached = false;

        float positionDifference = 0;


        if (currentActionInfo.movingPlayerToPositionTargetDelay > 0) {
            WaitForSeconds delay = new WaitForSeconds (currentActionInfo.movingPlayerToPositionTargetDelay);

            yield return delay;
        }

        while (!targetReached) {
            translateTimer += Time.deltaTime / duration;

            playerTransform.position = Vector3.Lerp (playerTransform.position, targetTransform.position, translateTimer);

            teleportTimer += Time.deltaTime;

            positionDifference = GKC_Utils.distance (playerTransform.position, targetTransform.position);

            if ((positionDifference < 0.07f) || teleportTimer > (duration + 1)) {
                targetReached = true;
            }

            yield return null;
        }

        movingPlayerToPositionTargetActive = false;
    }

    public void checkIfSpawnActionSystem (customActionStateInfo customActionStateInfoToCheck)
    {
        if (customActionStateInfoToCheck.useActionSystemPrefab &&

            (!customActionStateInfoToCheck.actionSystemPrefabSpawned || customActionStateInfoToCheck.mainActionSystem == null)) {
            GameObject newActionSystemToSpawn = GameObject.Instantiate (customActionStateInfoToCheck.actionSystemPrefab, transform);

            customActionStateInfoToCheck.mainActionSystem = newActionSystemToSpawn.GetComponent<actionSystem> ();

            customActionStateInfoToCheck.actionSystemPrefabSpawned = true;
        }
    }

    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);
    }

    [System.Serializable]
    public class actionStateInfo
    {
        public string Name;

        public bool stateActive;

        public UnityEvent eventToActivateState;
        public UnityEvent eventToDeactivateState;
    }

    [System.Serializable]
    public class customActionStateInfo
    {
        [Header ("Main Settings")]
        [Space]

        public string Name;

        public string categoryName;

        public bool stateEnabled = true;

        public actionSystem mainActionSystem;

        [Space]

        public bool useActionSystemPrefab;
        public GameObject actionSystemPrefab;

        [HideInInspector] public bool actionSystemPrefabSpawned;

        [Space]
        [Header ("Random Action Settings")]
        [Space]

        public bool useRandomActionSystemList;
        public List<actionSystem> randomActionSystemList = new List<actionSystem> ();
        public bool followActionsOrder;
        [HideInInspector] public int currentActionIndex;

        [Space]
        [Header ("Action On Air Settings")]
        [Space]

        public bool useActionOnAir;
        public actionSystem mainActionSystemOnAir;

        [Space]
        [Header ("Action On Crouch Settings")]
        [Space]

        public bool useActionOnCrouch;
        public actionSystem mainActionSystemOnCrouch;

        [Space]
        [Header ("Interrupt Other Actions Settings")]
        [Space]

        public bool canInterruptOtherActionActive;
        public List<string> actionListToInterrupt = new List<string> ();
        public bool useCategoryToCheckInterrupt;
        public List<string> actionCategoryListToInterrupt = new List<string> ();
        public UnityEvent eventOnInterrupOtherActionActive;

        public bool canForceInterruptOtherActionActive;

        public bool useEventOnInterruptedAction;
        public UnityEvent eventOnInterruptedAction;

        [Space]
        [Header ("Other Settings")]
        [Space]

        public bool useProbabilityToActivateAction;
        [Range (0, 1)] public float probablityToActivateAction;

        public bool checkLockedCameraState;
        public bool lockedCameraState;

        public bool checkAimingState;
        public bool aimingState;

        [Space]
        [Header ("Category ID Settings")]
        [Space]

        public bool useCustomActionCategoryIDInfoList;

        public bool canBeUsedOnAnyCustomActionCategoryID;

        public List<customActionCategoryIDInfo> customActionCategoryIDInfoList = new List<customActionCategoryIDInfo> ();
    }

    [System.Serializable]
    public class inputToPauseOnActionIfo
    {
        public string inputName;
        public bool previousActiveState;
    }


    [System.Serializable]
    public class customActionStateCategoryInfo
    {
        public string Name;

        [Space]

        public List<customActionStateInfo> actionStateInfoList = new List<customActionStateInfo> ();
    }

    [System.Serializable]
    public class customActionStateInfoDictionary
    {
        public string Name;

        [Space]

        public int categoryIndex;
        public int actionIndex;
    }

    [System.Serializable]
    public class customActionCategoryIDInfo
    {
        public int categoryID;

        public actionSystem mainActionSystem;

        public List<actionSystem> randomActionSystemList = new List<actionSystem> ();
    }
}