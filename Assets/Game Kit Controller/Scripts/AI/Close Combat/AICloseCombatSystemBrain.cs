﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;

[System.Serializable]
public class AICloseCombatSystemBrain : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public bool combatSystemEnabled;

    public bool controlledByAI;

    [Space]
    [Header ("Combat Settings")]
    [Space]

    public bool makeFullCombos;
    public bool useWaitTimeBetweenFullCombos;
    public float minRandomTimeBetweenFullCombos;
    public float maxRandomTimeBetweenFullCombos;

    [Space]

    public bool useRandomTimeBetweenAttacks;
    public float minRandomTimeBetweenAttacks;
    public float maxRandomTimeBetweenAttacks;

    public float minTimeBetweenAttacks;

    [Space]
    [Space]

    public bool useRandomAttack;
    public bool useAlwaysSameAttackType;
    public string sameAttackTypeName;
    public bool alternateAttackTypes;

    [Space]

    public float maxTimeToResetAttacksState;

    [Space]

    public bool checkToForceAttackIfNotUsedLongTimeAgo;

    public float maxWaitTimeToForceAttackIfNotUsedLongTimeAgo = 10;

    [Space]
    [Header ("Block Attack Settings")]
    [Space]

    public bool blockEnabled;

    public Vector2 randomBlockWaitTime;
    public Vector2 randomBlockDuration;

    [Space]
    [Header ("Roll/Dodge Settings")]
    [Space]

    public bool rollEnabled;

    public Vector2 randomRollWaitTime;

    public List<Vector2> rollMovementDirectionList = new List<Vector2> ();

    [Space]
    [Header ("Random Walk Settings")]
    [Space]

    public bool randomWalkEnabled;

    public Vector2 randomWalkWaitTime;
    public Vector2 randomWalkDuration;
    public Vector2 randomWalkRadius;

    [Space]

    public bool ignoreRandomWalkInFrontDirectionEnabled = true;

    [Space]
    [Header ("Jump To Target Settings")]
    [Space]

    public bool jumpToTargetOnRangeDistance;
    public Vector2 rangeDistanceToJumpToTarget;
    public bool avoidAttackOnAir = true;
    public float minTimeToJumpAgain = 4;
    public bool useJumpProbability;
    [Range (0, 100)] public float jumpProbability;

    [Space]

    public bool setNewJumpForce;
    public float newJumpForce;
    public bool setNewJumpForceMinDistance;
    public float newJumpForceMinDistance;

    [Space]
    [Header ("Other Settings")]
    [Space]

    public bool changeWaitToActivateAttackEnabled = true;

    public bool activateRandomWalkIfWaitToActivateAttackActive;

    public bool checkIfAttackInProcessBeforeCallingNextAttack = true;

    public bool useWaitToActivateAttackActiveCounter;

    [Space]

    public bool checkVerticalDistanceToTarget;
    public float maxVerticalDistanceToTarget;

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;
    public bool closeCombatSystemActive;
    public bool waitingForAttackActive;

    public string currentComboNameSelected;

    public float currentRandomTimeToAttack;

    public bool waitingForFullComboComplete;

    public float currentMinWaitTimeToNextAttack;

    public bool blockActive;
    public bool waitingBlockActive;

    public bool walkActive;
    public bool waitingWalkActive;

    public bool waitingRollActive;

    public bool canUseAttackActive;

    public bool insideMinDistanceToAttack;

    public bool canActivateAttack;

    public bool attackStatePaused;

    public bool debugPauseAttackActivation;

    public bool behaviorStatesPaused;

    public bool waitToActivateAttackActive;

    public int waitToActivateAttackActiveCounter = 0;

    public bool turnBasedCombatActionActive;

    public bool ignoreOtherActionsToForceAttackActive;

    [Space]
    [Header ("Events Settings")]
    [Space]

    public bool useEventsOnStateChange;
    public UnityEvent evenOnStateEnabled;
    public UnityEvent eventOnStateDisabled;

    [Space]

    public bool useEventIfTargetCantBeReachedButVisible;
    public UnityEvent eventIfTargetCantBeReachedButVisible;

    [Space]

    public eventParameters.eventToCallWithString eventToChangeAttackMode;

    [Space]
    [Header ("Components")]
    [Space]

    public closeCombatSystem mainCloseCombatSystem;
    public findObjectivesSystem mainFindObjectivesSystem;
    public dashSystem mainDashSystem;


    float lastTimeAttack;

    float currentRandomTimeToFullCombo;

    int currentAttackTypeToAlternateIndex;

    closeCombatSystem.combatTypeInfo currentComboInProcess;

    closeCombatSystem.combatTypeInfo temporalComboToCheck;

    bool checkComboCompleteState;


    float lastTimeBlockActive;

    float currentBlockWaitTime;

    float currentBlockDuration;

    float lastTimeBlockWaitActive;

    float lastTimeRollActive;

    float lastTimeWaitRollActive;

    float currentRollWaitTime;


    float lastTimeWaitWalkActive;
    float currentWalkTime;
    float lastTimeWalkActive;

    float currentWalkDuration;
    float currentWalkRadius;

    bool rollCoolDownActive;

    float currentPauseAttackStateDuration;
    float lastTimeAttackPauseWithDuration;

    float originalMinRandomTimeBetweenAttacks;
    float originalMaxRandomTimeBetweenAttacks;

    float originalMinTimeBetweenAttacks;

    float randomWaitTime;

    bool originalRandomWalkEnabled;

    float lastTimeJump;

    bool eventIfTargetCantBeReachedButVisibleActivated;


    void Start ()
    {
        originalMinRandomTimeBetweenAttacks = minRandomTimeBetweenAttacks;
        originalMaxRandomTimeBetweenAttacks = maxRandomTimeBetweenAttacks;

        originalMinTimeBetweenAttacks = minTimeBetweenAttacks;

        originalRandomWalkEnabled = randomWalkEnabled;
    }

    public void updateAI ()
    {
        if (closeCombatSystemActive) {

            if (walkActive) {
                if (Time.time > lastTimeWalkActive + 1) {
                    if (Time.time > lastTimeWalkActive + currentWalkDuration ||
                        mainFindObjectivesSystem.getRemainingPathDistanceToTarget () < 0.5f ||
                        mainFindObjectivesSystem.getCurrentDistanceToTarget () < 0.5f) {

                        resetRandomWalkState ();

                        print ("walk position reached");
                    }
                }
            }

            if (jumpToTargetOnRangeDistance) {
                checkJumpState ();
            }

            if (!insideMinDistanceToAttack) {
                if (mainFindObjectivesSystem.isOnSpotted ()) {
                    if (!mainFindObjectivesSystem.getCanReachCurrentTargetValue ()) {
                        bool checkIfTargetVisible = mainFindObjectivesSystem.checkIfTargetVisible (0);

                        if (checkIfTargetVisible) {
                            if (showDebugPrint) {
                                print ("target is visible but can't be reached");
                            }

                            if (useEventIfTargetCantBeReachedButVisible) {
                                if (!eventIfTargetCantBeReachedButVisibleActivated) {
                                    eventIfTargetCantBeReachedButVisible.Invoke ();

                                    eventIfTargetCantBeReachedButVisibleActivated = true;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void checkJumpState ()
    {
        if (mainFindObjectivesSystem.isActionActive ()) {
            return;
        }

        if (!insideMinDistanceToAttack &&
            mainFindObjectivesSystem.isAIOnGround () &&
            mainFindObjectivesSystem.isOnSpotted () &&
            (lastTimeJump == 0 || Time.time > minTimeToJumpAgain + lastTimeJump)) {

            float distanceToTarget = mainFindObjectivesSystem.getDistanceToTarget ();

            if (distanceToTarget > rangeDistanceToJumpToTarget.x && distanceToTarget < rangeDistanceToJumpToTarget.y) {
                bool activateJumpResult = true;

                if (useJumpProbability) {
                    float currentProbability = Random.Range (0, 100);

                    if (currentProbability > jumpProbability) {
                        useJumpProbability = false;
                    }
                }

                if (activateJumpResult) {
                    bool setNewJumpForceResult = setNewJumpForce;

                    if (setNewJumpForceMinDistance) {
                        if (distanceToTarget < newJumpForceMinDistance) {
                            setNewJumpForceResult = false;
                        }
                    }

                    if (setNewJumpForceResult) {
                        mainFindObjectivesSystem.setJumpPower (newJumpForce);

                        mainFindObjectivesSystem.activateJumpExternally (0);
                    } else {
                        mainFindObjectivesSystem.setOriginalJumpPower ();

                        mainFindObjectivesSystem.activateJumpExternally (distanceToTarget);
                    }
                }

                lastTimeJump = Time.time;
            }
        }
    }

    public void updateMainCloseCombatAttack (bool newCanUseAttackActiveState)
    {
        canUseAttackActive = newCanUseAttackActiveState;
    }

    public void updateMainCloseCombatBehavior ()
    {
        if (!closeCombatSystemActive) {
            return;
        }

        if (behaviorStatesPaused) {
            return;
        }

        if (mainFindObjectivesSystem.isAIPaused ()) {
            return;
        }

        if (checkToForceAttackIfNotUsedLongTimeAgo) {
            if (lastTimeAttack > 0 && !ignoreOtherActionsToForceAttackActive) {
                if (Time.time > maxWaitTimeToForceAttackIfNotUsedLongTimeAgo + lastTimeAttack) {
                    ignoreOtherActionsToForceAttackActive = true;

                    resetRandomWalkState ();

                    resetRollState ();

                    resetBlockState ();

                    if (showDebugPrint) {
                        print ("too much time since last attack active, reseting other states" +
                        " to force an attack");
                    }
                }
            }
        }

        checkWalkState ();

        if (walkActive) {
            return;
        }

        checkBlockState ();

        if (blockEnabled) {
            if (rollEnabled) {
                if (Time.time < lastTimeRollActive + 0.3f) {
                    return;
                }
            }
        }

        if (blockActive) {
            return;
        }

        if (blockEnabled) {
            if (rollEnabled) {
                if (Time.time < lastTimeBlockActive + 0.3f) {
                    return;
                }
            }
        }

        checkRollState ();

        if (rollEnabled) {
            if (Time.time < lastTimeRollActive + 1) {
                return;
            }
        }

        if (!insideMinDistanceToAttack || !canUseAttackActive) {
            return;
        }

        if (checkVerticalDistanceToTarget) {
            float verticalDistanceToCurrentEnemyPosition = mainFindObjectivesSystem.getVerticalDistanceToCurrentEnemyPosition ();

            if (verticalDistanceToCurrentEnemyPosition != 0) {
                if (verticalDistanceToCurrentEnemyPosition > maxVerticalDistanceToTarget) {
                    return;
                }
            }
        }

        if (currentPauseAttackStateDuration > 0) {
            if (Time.time > currentPauseAttackStateDuration + lastTimeAttackPauseWithDuration) {

                attackStatePaused = false;

                currentPauseAttackStateDuration = 0;
            } else {
                return;
            }
        }

        if (jumpToTargetOnRangeDistance && avoidAttackOnAir) {
            if (!mainFindObjectivesSystem.isAIOnGround ()) {
                return;
            }
        }

        if (waitToActivateAttackActive) {
            if (activateRandomWalkIfWaitToActivateAttackActive) {
                if (!walkActive) {
                    currentWalkTime = Random.Range (randomWalkWaitTime.x, randomWalkWaitTime.y);

                    currentWalkDuration = Random.Range (randomWalkDuration.x, randomWalkDuration.y);

                    currentWalkRadius = Random.Range (randomWalkRadius.x, randomWalkRadius.y);

                    mainFindObjectivesSystem.setignoreRandomWalkInFrontDirectionEnabledState (ignoreRandomWalkInFrontDirectionEnabled);

                    mainFindObjectivesSystem.setRandomWalkPositionState (currentWalkRadius);

                    lastTimeWalkActive = Time.time;

                    walkActive = true;

                    mainFindObjectivesSystem.setStrafeModeActive (true);
                }
            }

            return;
        }

        AICloseCombatAttack ();
    }

    //AI brain for close combat system
    public void AICloseCombatAttack ()
    {
        if (Time.time > maxTimeToResetAttacksState + lastTimeAttack) {
            if (showDebugPrint) {
                print ("Reset attacks info after max wait time");

                waitingForAttackActive = false;

                waitingForFullComboComplete = false;

                currentMinWaitTimeToNextAttack = 0;
            }
        }

        //		print (insideMinDistanceToAttack + " " + canUseAttackActive);

        if (currentRandomTimeToFullCombo > 0) {
            if (Time.time < currentRandomTimeToFullCombo + lastTimeAttack) {
                return;
            } else {
                currentRandomTimeToFullCombo = 0;
            }
        }

        bool canDoAttack = false;

        if (useRandomTimeBetweenAttacks) {
            if (!waitingForAttackActive) {
                currentRandomTimeToAttack = Random.Range (minRandomTimeBetweenAttacks, maxRandomTimeBetweenAttacks);

                waitingForAttackActive = true;
            }
        } else {
            currentRandomTimeToAttack = minTimeBetweenAttacks;
        }

        if (currentMinWaitTimeToNextAttack > 0) {
            currentRandomTimeToAttack = currentMinWaitTimeToNextAttack;
        }

        canActivateAttack = (Time.time > currentRandomTimeToAttack + lastTimeAttack);

        if (canActivateAttack) {
            if (showDebugPrint) {
                print ("Time since last attack " + currentRandomTimeToAttack + " " + (Time.time - currentRandomTimeToAttack - lastTimeAttack));
            }

            canDoAttack = true;
        }

        int numberOfAttacks = mainCloseCombatSystem.combatTypeInfoList.Count;

        if (makeFullCombos) {
            if (!waitingForFullComboComplete) {

                waitingForFullComboComplete = true;

                bool attackInfoAssigned = false;

                int combatTypeID = mainCloseCombatSystem.getCombatTypeID ();

                int loop = 0;

                while (!attackInfoAssigned) {
                    if (useAlwaysSameAttackType) {
                        currentComboInProcess = mainCloseCombatSystem.getCombatTypeInfoByName (sameAttackTypeName);

                    } else {
                        currentComboInProcess = mainCloseCombatSystem.combatTypeInfoList [Random.Range (0, numberOfAttacks)];
                    }

                    if (currentComboInProcess.combatTypeID == combatTypeID) {
                        attackInfoAssigned = true;
                    } else {
                        loop++;

                        if (loop >= 100) {
                            attackInfoAssigned = true;

                            return;
                        }
                    }
                }

                currentComboNameSelected = currentComboInProcess.Name;

                if (showDebugPrint) {
                    print ("Combo selected " + currentComboNameSelected);
                }
            }
        }

        if (canDoAttack) {
            if (checkIfAttackInProcessBeforeCallingNextAttack) {
                if (!mainCloseCombatSystem.canActivateAttack ()) {
                    if (showDebugPrint) {
                        print ("attack in process, cancelling check of attack");
                    }

                    return;
                }
            }

            if (checkComboCompleteState) {
                waitingForFullComboComplete = false;

                if (showDebugPrint) {
                    print ("CURRENT COMBO " + currentComboInProcess.Name + " COMPLETED\n\n");
                }

                currentMinWaitTimeToNextAttack = 0;

                if (useWaitTimeBetweenFullCombos) {

                    currentRandomTimeToFullCombo = Random.Range (minRandomTimeBetweenFullCombos, maxRandomTimeBetweenFullCombos);
                }

                checkComboCompleteState = false;

                return;
            }

            if (waitingForFullComboComplete) {

                if (currentComboInProcess.currentAttackIndex > 0 && currentComboInProcess.currentAttackIndex >= currentComboInProcess.combatAttackInfoList.Count) {
                    checkComboCompleteState = true;

                    if (showDebugPrint) {
                        print ("Combo complete");
                    }
                } else {
                    if (showDebugPrint) {
                        print ("Current Attack index: " + currentComboInProcess.currentAttackIndex);
                    }

                    if (currentComboInProcess.currentAttackIndex < currentComboInProcess.combatAttackInfoList.Count) {
                        float minTimeToPlayNextAttack =
                             currentComboInProcess.combatAttackInfoList [currentComboInProcess.currentAttackIndex].minTimeToPlayNextAttack;

                        if (minTimeToPlayNextAttack > 0) {
                            currentMinWaitTimeToNextAttack = minTimeToPlayNextAttack + 0.08f;
                        }
                    }

                    activateAttack (currentComboNameSelected);

                    if (showDebugPrint) {
                        print ("Next attack will wait " + currentMinWaitTimeToNextAttack + " seconds");
                    }

                    lastTimeAttack = Time.time;

                    ignoreOtherActionsToForceAttackActive = false;

                    if (useRandomTimeBetweenAttacks) {
                        waitingForAttackActive = false;
                    }
                }
            } else {
                if (useRandomAttack) {
                    bool attackInfoAssigned = false;

                    int combatTypeID = mainCloseCombatSystem.getCombatTypeID ();

                    int loop = 0;

                    while (!attackInfoAssigned) {
                        temporalComboToCheck = mainCloseCombatSystem.combatTypeInfoList [Random.Range (0, numberOfAttacks)];

                        if (temporalComboToCheck.combatTypeID == combatTypeID) {
                            activateAttack (temporalComboToCheck.Name);

                            attackInfoAssigned = true;
                        } else {
                            loop++;

                            if (loop >= 100) {
                                attackInfoAssigned = true;

                                return;
                            }
                        }

                    }
                } else if (useAlwaysSameAttackType) {
                    activateAttack (sameAttackTypeName);

                } else if (alternateAttackTypes) {
                    bool attackInfoAssigned = false;

                    int combatTypeID = mainCloseCombatSystem.getCombatTypeID ();

                    int loop = 0;

                    while (!attackInfoAssigned) {
                        temporalComboToCheck = mainCloseCombatSystem.combatTypeInfoList [currentAttackTypeToAlternateIndex];

                        if (temporalComboToCheck.combatTypeID == combatTypeID) {
                            attackInfoAssigned = true;
                        } else {
                            loop++;

                            if (loop >= 100) {
                                attackInfoAssigned = true;

                                return;
                            }

                            currentAttackTypeToAlternateIndex++;

                            if (currentAttackTypeToAlternateIndex == numberOfAttacks) {
                                currentAttackTypeToAlternateIndex = 0;
                            }
                        }
                    }

                    activateAttack (mainCloseCombatSystem.combatTypeInfoList [currentAttackTypeToAlternateIndex].Name);

                    currentAttackTypeToAlternateIndex++;

                    if (currentAttackTypeToAlternateIndex == numberOfAttacks) {
                        currentAttackTypeToAlternateIndex = 0;
                    }
                }

                lastTimeAttack = Time.time;

                ignoreOtherActionsToForceAttackActive = false;

                if (useRandomTimeBetweenAttacks) {
                    waitingForAttackActive = false;
                }
            }
        }
    }

    void activateAttack (string attackName)
    {
        if (showDebugPrint) {
            print ("Attack to use from BRAIN" + attackName);
            print ("\n");
        }

        if (debugPauseAttackActivation) {
            return;
        }

        mainCloseCombatSystem.useAttack (attackName);
    }

    public void resetRandomWalkState ()
    {
        if (waitingWalkActive || walkActive) {
            mainFindObjectivesSystem.setRandomWalkPositionState (0);

            waitingWalkActive = false;

            walkActive = false;

            lastTimeWalkActive = Time.time;

            if (!mainCloseCombatSystem.useStrafeMode) {
                mainFindObjectivesSystem.setStrafeModeActive (false);

                mainFindObjectivesSystem.playerControllerManager.activateOrDeactivateStrafeMode (false);
            }
        }
    }

    public void resetBlockState ()
    {
        waitingBlockActive = false;

        if (blockActive) {
            mainCloseCombatSystem.setBlockStateWithoutInputCheck (false);

            if (showDebugPrint) {
                print ("Stop Block");
            }
        }

        blockActive = false;

        lastTimeBlockActive = Time.time;
    }

    public void resetRollState ()
    {
        waitingRollActive = false;

        lastTimeRollActive = Time.time;
    }

    public void resetAttackState ()
    {

    }

    public void resetStates ()
    {
        resetRandomWalkState ();

        resetBlockState ();

        resetRollState ();

        resetAttackState ();
    }

    public void checkIfResetStatsOnRandomWalk ()
    {
        if (walkActive) {
            resetStates ();
        }
    }

    public void checkBlockState ()
    {
        if (ignoreOtherActionsToForceAttackActive) {
            return;
        }

        if (blockEnabled) {
            updateBlockState ();
        }
    }

    public void updateBlockState ()
    {
        if (rollEnabled) {
            if (Time.time < lastTimeRollActive + 1) {
                return;
            }
        }

        if (walkActive) {
            return;
        }

        if (mainCloseCombatSystem.isAttackInProcess ()) {
            return;
        }

        if (!insideMinDistanceToAttack) {
            if (blockActive) {
                if (Time.time > lastTimeBlockActive + 0.5f) {
                    resetBlockState ();

                    lastTimeBlockActive = 0;
                }
            }

            return;
        }

        if (waitingBlockActive) {
            if (blockActive) {
                if (Time.time > lastTimeBlockActive + currentBlockDuration) {
                    //					print (Time.time + "  " + lastTimeBlockActive + " " + currentBlockDuration + " " +
                    //					(lastTimeBlockActive + currentBlockDuration));

                    resetBlockState ();
                }
            } else {
                if (Time.time > lastTimeBlockWaitActive + currentBlockWaitTime) {
                    lastTimeBlockActive = Time.time;

                    blockActive = true;

                    mainCloseCombatSystem.setBlockStateWithoutInputCheck (true);

                    if (showDebugPrint) {
                        print ("Start Block");
                    }
                }
            }
        } else {
            if (Time.time > lastTimeBlockActive + randomWaitTime) {
                currentBlockWaitTime = Random.Range (randomBlockWaitTime.x, randomBlockWaitTime.y);

                currentBlockDuration = Random.Range (randomBlockDuration.x, randomBlockDuration.y);

                lastTimeBlockWaitActive = Time.time;

                waitingBlockActive = true;

                blockActive = false;

                randomWaitTime = Random.Range (0.1f, 0.5f);
            }
        }
    }

    public void checkRollState ()
    {
        if (ignoreOtherActionsToForceAttackActive) {
            return;
        }

        if (rollEnabled) {

            if (blockActive) {
                return;
            }

            if (walkActive) {
                return;
            }

            if (mainCloseCombatSystem.isAttackInProcess ()) {
                return;
            }

            if (!insideMinDistanceToAttack) {
                resetRollState ();

                lastTimeRollActive = 0;

                return;
            }

            if (waitingRollActive) {
                if (Time.time > lastTimeWaitRollActive + currentRollWaitTime) {

                    int randomRollMovementDirection = Random.Range (0, rollMovementDirectionList.Count);

                    Vector3 dashDirection = rollMovementDirectionList [randomRollMovementDirection];

                    if (mainFindObjectivesSystem.isLocked2_5dModeActive ()) {
                        randomRollMovementDirection = Random.Range (0, 2);

                        if (randomRollMovementDirection == 0) {
                            dashDirection = Vector3.forward;
                        } else {
                            dashDirection = -Vector3.forward;
                        }
                    }

                    mainDashSystem.activateDashStateWithCustomDirection (dashDirection);

                    resetRollState ();
                }
            } else {
                currentRollWaitTime = Random.Range (randomRollWaitTime.x, randomRollWaitTime.y);

                lastTimeWaitRollActive = Time.time;

                waitingRollActive = true;
            }
        }
    }

    public void checkWalkState ()
    {
        if (ignoreOtherActionsToForceAttackActive) {
            return;
        }

        if (randomWalkEnabled) {

            if (blockActive) {
                return;
            }

            if (mainCloseCombatSystem.isAttackInProcess ()) {
                return;
            }

            rollCoolDownActive = Time.time < lastTimeRollActive + 0.7f;

            if (rollCoolDownActive) {
                return;
            }

            if (waitingWalkActive) {
                if (!walkActive) {

                    if (Time.time > lastTimeWaitWalkActive + currentWalkTime) {
                        mainFindObjectivesSystem.setignoreRandomWalkInFrontDirectionEnabledState (ignoreRandomWalkInFrontDirectionEnabled);

                        mainFindObjectivesSystem.setRandomWalkPositionState (currentWalkRadius);

                        lastTimeWalkActive = Time.time;

                        walkActive = true;

                        mainFindObjectivesSystem.setStrafeModeActive (true);
                    }
                }
            } else {
                currentWalkTime = Random.Range (randomWalkWaitTime.x, randomWalkWaitTime.y);

                lastTimeWaitWalkActive = Time.time;

                waitingWalkActive = true;

                currentWalkDuration = Random.Range (randomWalkDuration.x, randomWalkDuration.y);

                currentWalkRadius = Random.Range (randomWalkRadius.x, randomWalkRadius.y);

                walkActive = false;

                if (!mainCloseCombatSystem.useStrafeMode) {
                    mainFindObjectivesSystem.setStrafeModeActive (false);

                    mainFindObjectivesSystem.playerControllerManager.activateOrDeactivateStrafeMode (false);
                }
            }
        }
    }

    public void updateInsideMinDistance (bool newInsideMinDistanceToAttack)
    {
        insideMinDistanceToAttack = newInsideMinDistanceToAttack;
    }

    public void setCloseCombatSystemActiveState (bool state)
    {
        if (closeCombatSystemActive == state) {
            return;
        }

        closeCombatSystemActive = state;

        waitingForFullComboComplete = false;

        checkEventsOnStateChange (closeCombatSystemActive);

        eventIfTargetCantBeReachedButVisibleActivated = false;

        if (closeCombatSystemActive) {
            if (mainCloseCombatSystem.useStrafeMode) {
                mainFindObjectivesSystem.setStrafeModeActive (true);
            }
        }

        lastTimeAttack = 0;

        waitToActivateAttackActive = false;

        waitToActivateAttackActiveCounter = 0;
    }

    public void checkEventsOnStateChange (bool state)
    {
        if (useEventsOnStateChange) {
            if (state) {
                evenOnStateEnabled.Invoke ();
            } else {
                eventOnStateDisabled.Invoke ();
            }
        }
    }

    public void startOverride ()
    {
        overrideTurretControlState (true);
    }

    public void stopOverride ()
    {
        overrideTurretControlState (false);
    }

    public void overrideTurretControlState (bool state)
    {
        controlledByAI = !state;
    }

    public void setBlockEnabledState (bool state)
    {
        blockEnabled = state;

        if (!blockEnabled) {
            resetBlockState ();
        }
    }

    public void setRollEnabledState (bool state)
    {
        rollEnabled = state;

        if (!rollEnabled) {
            resetRollState ();
        }
    }

    public void pauseAttackDuringXTime (float newDuration)
    {
        currentPauseAttackStateDuration = newDuration;

        lastTimeAttackPauseWithDuration = Time.time;

        attackStatePaused = true;
    }

    public void setWaitToActivateAttackActiveState (bool state)
    {
        if (!changeWaitToActivateAttackEnabled) {
            return;
        }

        if (useWaitToActivateAttackActiveCounter) {
            if (state) {
                waitToActivateAttackActiveCounter++;
            } else {
                waitToActivateAttackActiveCounter--;

                if (waitToActivateAttackActiveCounter < 0) {
                    waitToActivateAttackActiveCounter = 0;
                }
            }

            if (waitToActivateAttackActiveCounter > 0) {
                if (waitToActivateAttackActive) {
                    return;
                } else {
                    state = true;
                }
            } else {
                if (!waitToActivateAttackActive) {
                    return;
                } else {
                    state = false;
                }
            }
        }

        waitToActivateAttackActive = state;

        if (activateRandomWalkIfWaitToActivateAttackActive) {
            if (state) {
                currentWalkTime = Random.Range (randomWalkWaitTime.x, randomWalkWaitTime.y);

                currentWalkDuration = Random.Range (randomWalkDuration.x, randomWalkDuration.y);

                currentWalkRadius = Random.Range (randomWalkRadius.x, randomWalkRadius.y);

                mainFindObjectivesSystem.setignoreRandomWalkInFrontDirectionEnabledState (ignoreRandomWalkInFrontDirectionEnabled);

                mainFindObjectivesSystem.setRandomWalkPositionState (currentWalkRadius);

                lastTimeWalkActive = Time.time;

                walkActive = true;

                mainFindObjectivesSystem.setStrafeModeActive (true);
            } else {
                if (walkActive) {
                    resetRandomWalkState ();
                }
            }
        }
    }

    public void setUseRandomWalkEnabledState (bool state)
    {
        randomWalkEnabled = state;
    }

    public void setOriginalUseRandomWalkEnabledState ()
    {
        setUseRandomWalkEnabledState (originalRandomWalkEnabled);
    }

    public void resetBehaviorStates ()
    {
        resetStates ();

        waitingForAttackActive = false;

        waitingForFullComboComplete = false;

        currentMinWaitTimeToNextAttack = 0;

        checkComboCompleteState = false;

        currentRandomTimeToFullCombo = 0;

        insideMinDistanceToAttack = false;
    }

    public void setMinRandomTimeBetweenAttacks (float newValue)
    {
        minRandomTimeBetweenAttacks = newValue;
    }

    public void setMaxRandomTimeBetweenAttacks (float newValue)
    {
        maxRandomTimeBetweenAttacks = newValue;
    }

    public void setOriginalMinRandomTimeBetweenAttacks ()
    {
        setMinRandomTimeBetweenAttacks (originalMinRandomTimeBetweenAttacks);
    }

    public void setOriginalMaxRandomTimeBetweenAttacks ()
    {
        setMaxRandomTimeBetweenAttacks (originalMaxRandomTimeBetweenAttacks);
    }

    public void setMinTimeBetweenAttacks (float newValue)
    {
        minTimeBetweenAttacks = newValue;
    }

    public void setOriginalMinTimeBetweenAttacks ()
    {
        setMinTimeBetweenAttacks (originalMinTimeBetweenAttacks);
    }

    public void setBehaviorStatesPausedState (bool state)
    {
        behaviorStatesPaused = state;
    }

    public bool isAIBehaviorAttackInProcess ()
    {
        return mainCloseCombatSystem.isAttackInProcess ();
    }

    public void stopCurrentAttackInProcess ()
    {
        if (isAIBehaviorAttackInProcess ()) {
            mainCloseCombatSystem.disableCurrentAttackInProcess ();
        }
    }

    public void setTurnBasedCombatActionActiveState (bool state)
    {
        turnBasedCombatActionActive = state;
    }

    public void checkAIBehaviorStateOnCharacterSpawn ()
    {

    }

    public void checkAIBehaviorStateOnCharacterDespawn ()
    {

    }

    public void setMaxWaitTimeToForceAttackIfNotUsedLongTimeAgoValue (float newValue)
    {
        maxWaitTimeToForceAttackIfNotUsedLongTimeAgo = newValue;
    }

    public void setCheckToForceAttackIfNotUsedLongTimeAgoState (bool state)
    {
        checkToForceAttackIfNotUsedLongTimeAgo = state;
    }

    public void changeCurrentAttackMode (string newModeName)
    {
        changeAttackMode (newModeName);
    }

    void changeAttackMode (string newModeName)
    {
        if (!closeCombatSystemActive) {
            return;
        }

        stopAttackState ();

        eventToChangeAttackMode.Invoke (newModeName);
    }

    public void stopAttackState ()
    {
        if (!closeCombatSystemActive) {
            return;
        }

        bool playerIsBusy = mainCloseCombatSystem.isAttackInProcess ();

        if (playerIsBusy) {
            //print ("player is busy");

            resetBehaviorStates ();

            mainCloseCombatSystem.disableCurrentAttackInProcess ();
        }
    }
}