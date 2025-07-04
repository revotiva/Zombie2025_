﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class meleeWeaponsGrabbedManager : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public bool meleeWeaponsGrabbedManagerEnabled = true;

    public bool storeEachGrabbedWeapon;

    public bool storeOnlyOneWeapon;

    public bool canSpawnAnyWeaponStoredInfiniteTimes;

    public bool storePickedWeaponsOnInventory;

    public bool drawWeaponAtStartIfFoundOnInitializingInventory;

    public bool numberKeysToChangeBetweenMeleeWeaponsEnabled = true;

    public bool dropThrownWeaponIfChangingToAnotherWeaponEnabled;

    public bool dropThrownWeaponWhenUsingDeviceEnabled;

    [Space]
    [Header ("Hide Weapon Meshes Settings")]
    [Space]

    public bool overrideOptionToHideWeaponMeshWhenNotUsed;
    public bool hideWeaponMeshWhenNotUsed;

    [Space]
    [Header ("Start Game With Melee Weapon Settings")]
    [Space]

    public bool startGameWithWeapon;

    public bool drawWeaponOnStartEnabled = true;

    public string weaponNameToStartGame;

    [Space]
    [Space]

    public bool startGameWithListOfWeapons;
    public List<string> listOfWeaponsToStart = new List<string> ();

    [Space]

    public bool startWithRandomWeapon;

    public bool startWithRandomWeaponFromListOfWeaponsToStart;

    public bool startWithWeaponOnlyIfAlreadyAvailable;

    public bool drawWeaponAtStartIfConfiguredExternally;

    [Space]
    [Header ("Start Game With Melee Shield Settings")]
    [Space]

    public bool startGameWithShield;
    public string shieldNameToStartGame;

    [Space]
    [Header ("Weapon List")]
    [Space]

    public List<meleeWeaponGrabbedInfo> meleeWeaponGrabbedInfoList = new List<meleeWeaponGrabbedInfo> ();

    public List<meleeWeaponPrefabInfo> meleeWeaponPrefabInfoList = new List<meleeWeaponPrefabInfo> ();

    [Space]
    [Header ("Shield List")]
    [Space]

    public List<shieldPrefabInfo> shieldPrefabInfoList = new List<shieldPrefabInfo> ();

    public List<shieldGrabbedInfo> shieldGrabbedInfoList = new List<shieldGrabbedInfo> ();

    [Space]
    [Header ("Events Settings")]
    [Space]

    public bool useEventsOnStateChange;
    public UnityEvent eventOnStateActive;
    public UnityEvent eventOnStateDeactivate;

    [Space]

    public bool useEventsOnMeleeWeaponEquipped;
    public UnityEvent eventOnMeleeWeaponEquipped;

    [Space]

    public bool useEventOnGrabDropWeapon;
    public UnityEvent eventOnGrabWeapon;
    public UnityEvent eventOnDropWeapon;

    [Space]
    [Header ("Stolen Weapon Settings")]
    [Space]

    public bool useEventOnWeaponStolen;
    public UnityEvent eventOnWeaponStolen;

    [Space]
    [Header ("Save Settings")]
    [Space]

    public bool saveCurrentMeleeWeaponListToSaveFile = true;

    [Space]
    [Header ("IK Settings")]
    [Space]

    public bool useIKOnHands;
    public bool useIKOnHandsOnlyOnFba;

    public IKSystem mainIKSystem;

    public OnAnimatorIKComponent handsOnMeleeWeaponIKSystem;

    [Space]
    [Header ("Debug")]
    [Space]

    public bool showDebugPrint;

    public bool meleeWeaponsGrabbedManagerActive;

    public int currentNumberOfWeaponsAvailable;

    public int currentWeaponIndex = 0;

    public bool equipMeleeWeaponPaused;

    public bool currentMeleeWeaponSheathedOrCarried;

    public bool isLoadingGame;

    public bool carryingObject;

    public bool fullBodyAwarenessActive;

    [Space]
    [Header ("Components")]
    [Space]

    public GameObject playerGameObject;
    public playerInputManager playerInput;
    public grabObjects mainGrabObjects;
    public grabbedObjectMeleeAttackSystem mainGrabbedObjectMeleeAttackSystem;
    public inventoryManager mainInventoryManager;

    bool startWithWeaponChecked;

    bool startWithShieldChecked;

    bool drawMeleeWeaponsPaused;

    shieldGrabbedInfo currentShieldGrabbedInfo;

    Coroutine weaponDrawKeepAnimationCoroutine;

    bool lastDroppeWeaponDurabilityStored;
    float lastDroppedWeaponDurabilityValue;
    string lastDroppedWeaponName;


    public void initializeMeleeManagerValues ()
    {

    }

    void Start ()
    {
        currentNumberOfWeaponsAvailable = getCurrentNumberOfWeaponsAvailable ();

        if (playerGameObject == null) {
            playerGameObject = mainGrabObjects.gameObject;
        }
    }

    public void drawNextWeaponAvailable ()
    {
        getCurrentNumberOfWeaponsAvailable ();

        if (currentNumberOfWeaponsAvailable > 0) {

            checkMeleeWeaponToUse (meleeWeaponGrabbedInfoList [0].Name, startWithWeaponOnlyIfAlreadyAvailable);
        }
    }

    public void drawRandomWeaponFromPrefabList (string previousWeaponName)
    {
        int previousWeaponIndex = -1;

        if (previousWeaponName != "") {
            previousWeaponIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == previousWeaponName);
        }

        int newWeaponIndex = -1;

        if (previousWeaponIndex > -1) {

            bool exit = false;

            int loopCount = 0;

            while (!exit) {
                int randomWeaponIndex = Random.Range (0, meleeWeaponPrefabInfoList.Count);

                if (randomWeaponIndex != previousWeaponIndex) {
                    newWeaponIndex = randomWeaponIndex;

                    exit = true;
                }

                loopCount++;

                if (loopCount > 100) {
                    return;
                }
            }
        } else {
            newWeaponIndex = Random.Range (0, meleeWeaponPrefabInfoList.Count);
        }

        if (newWeaponIndex > -1) {
            checkMeleeWeaponToUse (meleeWeaponPrefabInfoList [newWeaponIndex].Name, false);
        }
    }

    public void inputSetWeaponByIndex (int currentNumberInput)
    {
        if (currentNumberInput > -1) {
            if (currentNumberInput == 0) {
                currentNumberInput = 9;
            } else {
                currentNumberInput--;
            }

            if (meleeWeaponGrabbedInfoList.Count == 0 || currentNumberInput >= meleeWeaponGrabbedInfoList.Count) {
                return;
            }

            if (getCurrentNumberOfWeaponsAvailable () == 0) {
                return;
            }

            if (isGrabObjectsEnabled ()) {
                checkWeaponByNumber (currentNumberInput);
            }
        }
    }

    void Update ()
    {
        if (!startInitialized) {
            startInitialized = true;
        }

        if (meleeWeaponsGrabbedManagerActive) {
            if (!storePickedWeaponsOnInventory) {
                if (numberKeysToChangeBetweenMeleeWeaponsEnabled) {
                    if (!mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget () &&
                        canUseInput ()) {

                        int currentNumberInput = playerInput.checkNumberInput (currentNumberOfWeaponsAvailable + 1);

                        if (currentNumberInput > -1) {
                            inputSetWeaponByIndex (currentNumberInput);
                        }
                    }
                }
            }

            if (!startWithShieldChecked) {
                if (startGameWithShield && isGrabObjectsEnabled ()) {
                    setShieldActiveState (true, shieldNameToStartGame);

                    startWithShieldChecked = true;
                }
            }

            if (!startWithWeaponChecked) {

                if (startGameWithWeapon) {
                    if (isGrabObjectsEnabled ()) {
                        if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                            if (startGameWithListOfWeapons && !isLoadingGame) {
                                setInitialWeaponsList ();
                            }

                            if (startWithRandomWeapon) {
                                if (startWithRandomWeaponFromListOfWeaponsToStart && startGameWithListOfWeapons) {
                                    int randomWeaponIndex = Random.Range (0, meleeWeaponGrabbedInfoList.Count);

                                    weaponNameToStartGame = meleeWeaponGrabbedInfoList [randomWeaponIndex].Name;
                                } else {
                                    int randomWeaponIndex = Random.Range (0, meleeWeaponPrefabInfoList.Count);

                                    weaponNameToStartGame = meleeWeaponPrefabInfoList [randomWeaponIndex].Name;
                                }
                            }

                            if (weaponNameToStartGame != "" && drawWeaponOnStartEnabled) {
                                checkMeleeWeaponToUse (weaponNameToStartGame, startWithWeaponOnlyIfAlreadyAvailable);
                            }
                        }

                        startWithWeaponChecked = true;
                    }
                } else {
                    if (drawWeaponAtStartIfConfiguredExternally) {
                        if (isGrabObjectsEnabled ()) {
                            if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                                if (meleeWeaponGrabbedInfoList.Count > 0) {
                                    checkMeleeWeaponToUse (meleeWeaponGrabbedInfoList [0].Name, false);

                                    if (showDebugPrint) {
                                        print ("checking to draw weapon when starting game and checking for grabbed object list");
                                    }
                                }
                            } else {
                                string currentWeaponName = getCurrentWeaponName ();

                                if (currentWeaponName != null && currentWeaponName != "") {
                                    if (showDebugPrint) {
                                        print ("weapon already carried " + getCurrentWeaponName ());
                                    }

                                    updateQuickAccesSlotOnInventory (currentWeaponName);
                                }
                            }
                        }

                        startWithWeaponChecked = true;
                    }
                }
            }
        }
    }

    bool startInitialized;

    public void resetStartWithWeaponCheckedOnCharacterSpawn ()
    {
        startWithWeaponChecked = false;
    }

    void setInitialWeaponsList ()
    {
        int listOfWeaponsToStartCount = listOfWeaponsToStart.Count;

        for (int i = 0; i < listOfWeaponsToStartCount; i++) {
            int weaponIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == listOfWeaponsToStart [i]);

            if (weaponIndex > -1) {
                meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = meleeWeaponPrefabInfoList [weaponIndex];

                if (currentMeleeWeaponPrefabInfo != null) {
                    int grabbedWeaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == currentMeleeWeaponPrefabInfo.Name);

                    if (grabbedWeaponIndex <= -1) {
                        meleeWeaponGrabbedInfo newMeleeWeaponGrabbedInfo = new meleeWeaponGrabbedInfo ();

                        newMeleeWeaponGrabbedInfo.Name = currentMeleeWeaponPrefabInfo.Name;

                        newMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

                        meleeWeaponGrabbedInfoList.Add (newMeleeWeaponGrabbedInfo);
                    } else {
                        if (showDebugPrint) {
                            print ("melee weapon was already on the initial list");
                        }
                    }
                }
            }
        }

        if (!isGrabObjectsEnabled ()) {
            return;
        }

        int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

        for (int k = 0; k < meleeWeaponGrabbedInfoListCount; k++) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [k];

            currentMeleeWeaponGrabbedInfo.weaponInstantiated = true;

            currentMeleeWeaponGrabbedInfo.carryingWeapon = true;

            meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = getWeaponPrefabByName (currentMeleeWeaponGrabbedInfo.Name);

            if (currentMeleeWeaponPrefabInfo != null) {
                currentMeleeWeaponGrabbedInfo.weaponStored = (GameObject)Instantiate (currentMeleeWeaponPrefabInfo.weaponPrefab, Vector3.up * 1000, Quaternion.identity);

                currentMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                checkObjectMeshToEnableOrDisable (!hideWeaponMeshResult, currentMeleeWeaponGrabbedInfo);

                if (currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
                }
            }
        }
    }

    bool checkIfHideWeaponMeshWhenNotUsed (bool currentValue)
    {
        if (overrideOptionToHideWeaponMeshWhenNotUsed) {
            return hideWeaponMeshWhenNotUsed;
        }

        return currentValue;
    }

    public bool checkMeleeWeaponToUse (string weaponNameToSearch, bool checkIfWeaponNotFound)
    {
        bool canSearchWeapon = true;

        if (showDebugPrint) {
            print (weaponNameToSearch);
        }

        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        if (checkIfWeaponNotFound) {
            if (weaponIndex == -1) {
                canSearchWeapon = false;
            }
        }

        if (canSearchWeapon) {

            if (weaponIndex > -1 && meleeWeaponGrabbedInfoList [weaponIndex].weaponInstantiated) {
                if (showDebugPrint) {
                    print ("check weapon by number");
                }

                checkWeaponByNumber (weaponIndex);

                return true;
            } else {
                if (showDebugPrint) {
                    print ("instantiate new weapon");
                }

                int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == weaponNameToSearch);

                if (weaponPrefabIndex > -1) {
                    GameObject newWeaponToCarry = (GameObject)Instantiate (meleeWeaponPrefabInfoList [weaponPrefabIndex].weaponPrefab, Vector3.up * 1000, Quaternion.identity);

                    grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = newWeaponToCarry.GetComponent<grabPhysicalObjectSystem> ();

                    currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

                    mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                    mainGrabObjects.grabPhysicalObjectExternally (newWeaponToCarry);

                    mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

                    return true;
                }
            }
        }

        return false;
    }

    public void setCheckDrawKeepWeaponAnimationPauseState (bool state)
    {
        mainGrabbedObjectMeleeAttackSystem.setCheckDrawKeepWeaponAnimationPauseState (state);
    }

    public void checkIfDrawWeapon ()
    {
        if (meleeWeaponsGrabbedManagerActive) {
            checkWeaponByNumber (currentWeaponIndex);

            mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (false);
        }
    }

    //Check for the melee draw/keep actions if using animations for those actions
    void activateDrawkeepMeleeWeaponAnimation (meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo, bool useExtraDelay)
    {
        stopActivateDrawkeepMeleeWeaponAnimationCoroutine ();

        weaponDrawKeepAnimationCoroutine = StartCoroutine (activateDrawkeepMeleeWeaponAnimationCoroutine (currentMeleeWeaponGrabbedInfo, useExtraDelay));
    }

    public void stopActivateDrawkeepMeleeWeaponAnimationCoroutine ()
    {
        if (weaponDrawKeepAnimationCoroutine != null) {
            StopCoroutine (weaponDrawKeepAnimationCoroutine);
        }
    }

    IEnumerator activateDrawkeepMeleeWeaponAnimationCoroutine (meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo, bool useExtraDelay)
    {
        bool targetReached = false;

        if (useExtraDelay) {
            yield return new WaitForSeconds (0.3f);
        }

        while (!targetReached) {
            if (!mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isActionActive ()) {
                targetReached = true;
            }


            yield return null;
        }

        confirmToDrawNextWeapon (false, currentMeleeWeaponGrabbedInfo);
    }

    void checkWeaponByNumber (int currentNumberInput)
    {
        if (currentNumberInput >= meleeWeaponGrabbedInfoList.Count || currentNumberInput < 0) {
            return;
        }

        meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentNumberInput];

        bool isCurrentWeapon = currentMeleeWeaponGrabbedInfo.isCurrentWeapon;

        bool previousWeaponIsKept = false;

        bool previousWeaponUseDrawKeepAnimations = false;

        if (!isCurrentWeapon) {

            currentMeleeWeaponGrabbedInfo.isCurrentWeapon = true;

            currentMeleeWeaponGrabbedInfo.carryingWeapon = true;

            currentMeleeWeaponSheathedOrCarried = true;

            currentWeaponIndex = currentNumberInput;

            bool weaponToKeepFound = false;

            for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
                if (k != currentNumberInput) {
                    if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
                        keepWeapon (k);

                        weaponToKeepFound = true;

                        previousWeaponIsKept = true;

                        if (meleeWeaponGrabbedInfoList [k].weaponStored != null) {
                            grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [k].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                            previousWeaponUseDrawKeepAnimations = currentGrabPhysicalObjectMeleeAttackSystem.useDrawKeepWeaponAnimation;
                        }
                    }

                    meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
                }
            }

            if (!weaponToKeepFound && !storeEachGrabbedWeapon) {
                if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                    mainGrabObjects.dropObject ();

                    mainGrabObjects.clearPhysicalObjectToGrabFoundList ();
                }
            }
        }

        bool confirmToDrawNextWeaponResult = true;

        if (previousWeaponIsKept) {
            if (!isCurrentWeapon) {
                if (previousWeaponUseDrawKeepAnimations && !mainGrabbedObjectMeleeAttackSystem.ignoreUseDrawKeepWeaponAnimation) {
                    confirmToDrawNextWeaponResult = false;

                    activateDrawkeepMeleeWeaponAnimation (currentMeleeWeaponGrabbedInfo, true);
                }
            }
        }

        //		print ("confirmToDrawNextWeaponResult result " + confirmToDrawNextWeaponResult);

        if (confirmToDrawNextWeaponResult) {
            confirmToDrawNextWeapon (isCurrentWeapon, currentMeleeWeaponGrabbedInfo);
        }
    }

    void confirmToDrawNextWeapon (bool isCurrentWeapon, meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo)
    {
        if (!isCurrentWeapon) {
            bool weaponInstantiated = false;

            if (!currentMeleeWeaponGrabbedInfo.weaponInstantiated || currentMeleeWeaponGrabbedInfo.canBeSpawnedInfiniteTimes || canSpawnAnyWeaponStoredInfiniteTimes) {
                if (currentMeleeWeaponGrabbedInfo.weaponStored == null) {

                    currentMeleeWeaponGrabbedInfo.weaponInstantiated = true;

                    meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = getWeaponPrefabByName (currentMeleeWeaponGrabbedInfo.Name);

                    if (currentMeleeWeaponPrefabInfo != null) {
                        currentMeleeWeaponGrabbedInfo.weaponStored = (GameObject)Instantiate (currentMeleeWeaponPrefabInfo.weaponPrefab, Vector3.up * 1000, Quaternion.identity);

                        currentMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

                        grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

                        currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

                        mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                        mainGrabObjects.grabPhysicalObjectExternally (currentMeleeWeaponGrabbedInfo.weaponStored);

                        mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

                        weaponInstantiated = true;

                        mainGrabbedObjectMeleeAttackSystem.activateStartDrawAnimation ();

                        //update quick slot here

                        if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                            updateQuickAccesSlotOnInventory (currentMeleeWeaponGrabbedInfo.Name);
                        }
                    } else {
                        print ("WARNING: melee weapon prefab with the name " + currentMeleeWeaponGrabbedInfo.Name + " not found, make sure to configure a weapon" +
                        " with that info.");

                        return;
                    }
                }
            }

            if (!weaponInstantiated) {
                if (!currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
                }

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                if (!hideWeaponMeshResult) {
                    checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
                }

                grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

                currentGrabPhysicalObjectSystem.setCurrentPlayer (playerGameObject);

                mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                mainGrabObjects.grabPhysicalObjectExternally (currentMeleeWeaponGrabbedInfo.weaponStored);

                mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

                mainGrabbedObjectMeleeAttackSystem.activateStartDrawAnimation ();

                //update quick slot here

                if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                    updateQuickAccesSlotOnInventory (currentMeleeWeaponGrabbedInfo.Name);
                }
            }
        }

        mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (true);
    }

    public void enableOrDisableMeleeWeaponsGrabbedManager (bool state)
    {
        if (!meleeWeaponsGrabbedManagerEnabled) {
            return;
        }

        if (meleeWeaponsGrabbedManagerActive == state) {
            return;
        }

        meleeWeaponsGrabbedManagerActive = state;

        if (meleeWeaponsGrabbedManagerActive) {
            if (!drawMeleeWeaponsPaused) {
                bool canDrawWeapon = true;

                if (startGameWithWeapon && !startWithWeaponChecked) {
                    canDrawWeapon = false;
                }

                if (canDrawWeapon) {
                    if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject () && startInitialized) {
                        checkWeaponByNumber (currentWeaponIndex);

                        //						print ("check melee");

                        if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject () && currentWeaponIndex >= 0 && currentWeaponIndex < meleeWeaponGrabbedInfoList.Count) {
                            updateQuickAccesSlotOnInventory (meleeWeaponGrabbedInfoList [currentWeaponIndex].Name);
                        }
                    }
                }
            }
        } else {
            bool currentWeaponIsThrow = isCurrentWeaponThrown ();

            for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
                if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {

                    if (!currentWeaponIsThrow) {
                        keepWeapon (k);
                    }
                }

                meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
            }

            if (currentWeaponIsThrow) {
                mainGrabObjects.dropObject ();
            }
        }

        if (useEventsOnStateChange) {
            if (meleeWeaponsGrabbedManagerActive) {
                eventOnStateActive.Invoke ();
            } else {
                eventOnStateDeactivate.Invoke ();
            }
        }

        if (storePickedWeaponsOnInventory) {
            mainInventoryManager.checkToEnableOrDisableQuickAccessSlotsParentOutOfInventoryFromMeleeWeaponsMode (meleeWeaponsGrabbedManagerActive);
        }

        drawMeleeWeaponsPaused = false;

        if (!meleeWeaponsGrabbedManagerActive) {
            currentMeleeWeaponSheathedOrCarried = false;
        }
    }

    public void setDrawMeleeWeaponsPausedState (bool state)
    {
        drawMeleeWeaponsPaused = state;
    }

    void keepWeapon (int weaponIndex)
    {
        if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
            return;
        }

        if (meleeWeaponGrabbedInfoList.Count > weaponIndex) {

            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                if (showDebugPrint) {
                    print ("Keep current weapon " + currentMeleeWeaponGrabbedInfo.Name);
                }

                mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (true);

                mainGrabObjects.grabbed = true;

                mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);

                mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);

                if (currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
                }

                mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                if (!hideWeaponMeshResult) {
                    checkObjectMeshToEnableOrDisable (true, currentMeleeWeaponGrabbedInfo);
                }

                mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (false);

                bool canUseWeaponKeepAnimation = true;

                if (mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isCustomCharacterControllerActive () ||
                    mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isPlayerDriving () ||
                    mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isUsingDevice ()) {
                    canUseWeaponKeepAnimation = false;
                }

                if (canUseWeaponKeepAnimation) {
                    mainGrabbedObjectMeleeAttackSystem.activateStartKeepAnimation ();
                } else {
                    mainGrabbedObjectMeleeAttackSystem.removeDrawKeepWeaponAnimationInfo ();
                }
            }

            currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;
        }
    }

    public void disableCurrentMeleeWeapon (string weaponName)
    {
        if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
            return;
        }

        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

        if (meleeWeaponGrabbedInfoList.Count > weaponIndex && weaponIndex > -1) {

            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
                if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                    mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                    mainGrabObjects.grabbed = true;

                    mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);

                    mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);

                    if (currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                        currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
                    }

                    mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);

                    bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                    if (!hideWeaponMeshResult) {
                        checkObjectMeshToEnableOrDisable (true, currentMeleeWeaponGrabbedInfo);
                    }
                }

                currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;

                currentMeleeWeaponGrabbedInfo.carryingWeapon = false;
            }
        }
    }

    public void checkObjectMeshToEnableOrDisable (bool enableWeaponMesh, meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo)
    {
        grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

        bool weaponMeshCreated = false;

        if (currentMeleeWeaponGrabbedInfo.weaponMesh == null) {
            GameObject weaponMeshToInstantiate = currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh;

            if (currentGrabPhysicalObjectMeleeAttackSystem.useCustomWeaponMeshToInstantiate) {
                weaponMeshToInstantiate = currentGrabPhysicalObjectMeleeAttackSystem.customWeaponMeshToInstantiate;
            }

            currentMeleeWeaponGrabbedInfo.weaponMesh = (GameObject)Instantiate (weaponMeshToInstantiate, Vector3.zero, Quaternion.identity);

            currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localScale = currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh.transform.localScale;

            weaponMeshCreated = true;
        }

        if (currentMeleeWeaponGrabbedInfo.weaponMesh != null) {
            if (currentMeleeWeaponGrabbedInfo.weaponMesh.activeSelf != enableWeaponMesh) {
                currentMeleeWeaponGrabbedInfo.weaponMesh.SetActive (enableWeaponMesh);

                if (showDebugPrint) {
                    print (enableWeaponMesh + " " + Time.time);
                }
            }

            if (enableWeaponMesh || weaponMeshCreated) {
                grabPhysicalObjectSystem currentGrabPhysicalObjectSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectSystem> ();

                bool setWeaponMeshOnPhysicalWeaponPlace = false;

                if (enableWeaponMesh) {
                    currentMeleeWeaponGrabbedInfo.objectThrown = currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ();

                    if (currentMeleeWeaponGrabbedInfo.objectThrown) {
                        setWeaponMeshOnPhysicalWeaponPlace = true;
                    }
                }

                Transform newParent = null;

                bool useMountPointToKeepObject = currentGrabPhysicalObjectSystem.useMountPointToKeepObject;

                if (useMountPointToKeepObject) {
                    newParent = GKC_Utils.getMountPointTransformByName (currentGrabPhysicalObjectSystem.mountPointTokeepObjectName, playerGameObject.transform);
                }

                if (!useMountPointToKeepObject || newParent == null) {
                    newParent = mainGrabbedObjectMeleeAttackSystem.getCharacterHumanBone (currentGrabPhysicalObjectSystem.boneToKeepObject);
                }

                Vector3 targetPosition = currentGrabPhysicalObjectMeleeAttackSystem.referencePositionToKeepObjectMesh.localPosition;
                Quaternion targetRotation = currentGrabPhysicalObjectMeleeAttackSystem.referencePositionToKeepObjectMesh.localRotation;

                if (enableWeaponMesh) {
                    mainGrabbedObjectMeleeAttackSystem.checkUpdateCustomReferencePositionsToKeepMeshValues (currentGrabPhysicalObjectMeleeAttackSystem);
                }

                if (currentGrabPhysicalObjectMeleeAttackSystem.useCustomReferencePositionToKeepObjectMesh) {
                    targetPosition = currentGrabPhysicalObjectMeleeAttackSystem.getCustomReferenceToKeepObjectMeshPosition ();
                    targetRotation = Quaternion.Euler (currentGrabPhysicalObjectMeleeAttackSystem.getCustomReferenceToKeepObjectMeshEuler ());
                }

                if (setWeaponMeshOnPhysicalWeaponPlace) {
                    Transform lastParentAssigned = currentGrabPhysicalObjectSystem.getLastParentAssigned ();

                    if (lastParentAssigned != null) {
                        currentMeleeWeaponGrabbedInfo.weaponStored.transform.SetParent (lastParentAssigned);
                    }

                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (currentGrabPhysicalObjectMeleeAttackSystem.weaponMesh.transform);

                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localPosition = Vector3.zero;
                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localRotation = Quaternion.identity;

                    newParent = currentGrabPhysicalObjectSystem.transform.parent;

                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (newParent);
                } else {

                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.SetParent (newParent);

                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localPosition = targetPosition;
                    currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localRotation = targetRotation;
                }

                currentMeleeWeaponGrabbedInfo.weaponMesh.transform.localScale = Vector3.one;

                if (!weaponMeshCreated) {
                    if (mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isPlayerOnFirstPerson ()) {
                        enableOrDisableMeleeWeaponMeshOnCharacterBodyByName (false, currentMeleeWeaponGrabbedInfo.Name);
                    }
                }
            }
        }
    }

    public meleeWeaponPrefabInfo getWeaponPrefabByName (string weaponName)
    {
        for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
            if (meleeWeaponPrefabInfoList [k].Name.Equals (weaponName)) {
                return meleeWeaponPrefabInfoList [k];
            }
        }

        return null;
    }

    public bool checkIfCanUseMeleeWeaponPrefabByName (string weaponName)
    {
        for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
            if (meleeWeaponPrefabInfoList [k].Name.Equals (weaponName)) {
                return true;
            }
        }

        return false;
    }

    public grabPhysicalObjectMeleeAttackSystem getWeaponGrabbedByName (string weaponName)
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            if (meleeWeaponGrabbedInfoList [k].Name.Equals (weaponName)) {
                if (meleeWeaponGrabbedInfoList [k].weaponStored != null) {

                    return meleeWeaponGrabbedInfoList [k].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                }
            }
        }

        return null;
    }

    public void checkWeaponToStore (string weaponName, GameObject weaponGameObject)
    {
        if (!storeEachGrabbedWeapon) {
            return;
        }

        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

        if (weaponIndex == -1) {
            meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = getWeaponPrefabByName (weaponName);

            if (currentMeleeWeaponPrefabInfo != null) {
                meleeWeaponGrabbedInfo newMeleeWeaponGrabbedInfo = new meleeWeaponGrabbedInfo ();

                newMeleeWeaponGrabbedInfo.Name = weaponName;
                newMeleeWeaponGrabbedInfo.isCurrentWeapon = true;
                newMeleeWeaponGrabbedInfo.carryingWeapon = true;

                currentMeleeWeaponSheathedOrCarried = true;

                newMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

                newMeleeWeaponGrabbedInfo.weaponStored = weaponGameObject;

                grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = newMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentGrabPhysicalObjectMeleeAttackSystem.hideWeaponMeshWhenNotUsed);

                newMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed = hideWeaponMeshResult;

                newMeleeWeaponGrabbedInfo.weaponInstantiated = true;

                meleeWeaponGrabbedInfoList.Add (newMeleeWeaponGrabbedInfo);

                currentNumberOfWeaponsAvailable = getCurrentNumberOfWeaponsAvailable ();

                currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;

                if (storeOnlyOneWeapon) {
                    for (int i = meleeWeaponGrabbedInfoList.Count - 1; i >= 0; i--) {
                        if (currentWeaponIndex != i) {
                            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [i];

                            if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {

                                hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                                if (!hideWeaponMeshResult) {
                                    checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
                                }

                                currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;

                                if (!currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
                                }
                            }

                            meleeWeaponGrabbedInfoList.RemoveAt (i);
                        }
                    }

                    currentNumberOfWeaponsAvailable = getCurrentNumberOfWeaponsAvailable ();

                    currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;
                }

                if (storePickedWeaponsOnInventory && !equipMeleeWeaponPaused) {
                    if (showDebugPrint) {
                        print ("ADD MELEE WEAPON TO INVENTORY " + weaponName);
                    }

                    mainInventoryManager.addObjectAmountToInventoryByName (weaponName, 1);

                    if (!meleeWeaponsGrabbedManagerActive) {
                        mainInventoryManager.checkQuickAccessSlotToSelectByName (weaponName);

                        //						if (useEventsOnMeleeWeaponEquipped) {
                        //							eventOnMeleeWeaponEquipped.Invoke ();
                        //						}
                    }

                    if (!mainGrabbedObjectMeleeAttackSystem.ignoreUseDrawKeepWeaponAnimation) {
                        meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentWeaponIndex];

                        activateDrawkeepMeleeWeaponAnimation (currentMeleeWeaponGrabbedInfo, false);
                    }
                }

                equipMeleeWeaponPaused = false;
            } else {
                print ("WARNING: melee weapon prefab with the name " + weaponName + " not found, make sure to configure a weapon" +
                " with that info.");
            }
        } else {
            for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
                if (weaponIndex == k) {
                    meleeWeaponGrabbedInfoList [k].isCurrentWeapon = true;
                    meleeWeaponGrabbedInfoList [k].carryingWeapon = true;

                    currentMeleeWeaponSheathedOrCarried = true;
                } else {
                    meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
                }
            }

            currentWeaponIndex = weaponIndex;

            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentWeaponIndex];

            if (currentMeleeWeaponGrabbedInfo.weaponStored != weaponGameObject) {

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                if (!hideWeaponMeshResult) {
                    checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
                }

                currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;

                if (!currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
                }

                currentMeleeWeaponGrabbedInfo.weaponStored = weaponGameObject;
            }
        }
    }

    public void checkToDropWeaponFromList (string weaponName)
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [k];

            if (currentMeleeWeaponGrabbedInfo.Name.Equals (weaponName)) {

                bool objectIsThrown = false;

                bool ignoreRemoveWeaponFromList = false;

                if (storePickedWeaponsOnInventory && mainInventoryManager != null) {
                    if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                        grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem =
                            currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                        if (currentGrabPhysicalObjectMeleeAttackSystem.isObjectThrown ()) {
                            objectIsThrown = true;
                        } else {
                            if (currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                                currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
                            }

                            ignoreRemoveWeaponFromList = true;
                        }
                    }
                }

                if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
                    currentMeleeWeaponSheathedOrCarried = false;
                }

                if (ignoreRemoveWeaponFromList) {
                    currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;
                    currentMeleeWeaponGrabbedInfo.carryingWeapon = false;
                } else {
                    meleeWeaponGrabbedInfoList.RemoveAt (k);
                }

                if (meleeWeaponGrabbedInfoList.Count == 0) {
                    currentMeleeWeaponSheathedOrCarried = false;
                } else {
                    if (getCurrentNumberOfWeaponsAvailable () == 0) {
                        currentMeleeWeaponSheathedOrCarried = false;
                    }
                }

                if (storePickedWeaponsOnInventory && mainInventoryManager != null) {

                    if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
                        if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                            grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                            if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                                lastDroppeWeaponDurabilityStored = true;

                                lastDroppedWeaponName = currentMeleeWeaponGrabbedInfo.Name;

                                lastDroppedWeaponDurabilityValue = currentGrabPhysicalObjectMeleeAttackSystem.getDurabilityAmount ();
                            }
                        }
                    }

                    if (objectIsThrown) {
                        if (showDebugPrint) {
                            print ("object is thrown, remove the weapon from inventory without instantiate a pickup");
                        }

                        mainInventoryManager.dropEquipByName (weaponName, 1, false, false);
                    } else {
                        if (showDebugPrint) {
                            print ("object is not thrown, remove the weapon from inventory and instantiate a pickup");
                        }

                        bool objectLocatedOnInventory = false;

                        if (showDebugPrint) {
                            print (mainInventoryManager.getInventoryObjectAmountByName (weaponName));
                        }

                        int amountOfWeaponOnInventory = mainInventoryManager.getInventoryObjectAmountByName (weaponName);

                        if (amountOfWeaponOnInventory >= 1) {
                            objectLocatedOnInventory = true;
                        }

                        mainInventoryManager.dropEquipByName (weaponName, 1, true, false);

                        if (!objectLocatedOnInventory) {
                            if (!currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                                currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (true);
                            }
                        }

                        //Check here about unequipping melee weapons when carrying more of the same type
                        if (amountOfWeaponOnInventory > 1) {
                            mainInventoryManager.unEquipObjectByName (weaponName);
                        }

                        GameObject pickupDropped = mainInventoryManager.getLastObjectDropped ();

                        if (pickupDropped != null) {
                            Rigidbody currentPickupRigidbody = pickupDropped.GetComponentInChildren<Rigidbody> ();

                            if (currentPickupRigidbody != null) {
                                Vector3 lastDropForceDirection = mainGrabbedObjectMeleeAttackSystem.mainGrabObjects.getThrowDirection ();

                                lastDropForceDirection *= mainGrabbedObjectMeleeAttackSystem.mainGrabObjects.getLastHoldTimer ();

                                currentPickupRigidbody.AddForce (lastDropForceDirection,
                                    mainGrabbedObjectMeleeAttackSystem.mainGrabObjects.powerForceMode);
                            }
                        }
                    }
                }

                return;
            }
        }
    }

    public void disableIsCurrentWeaponStateOnAllWeapons ()
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            meleeWeaponGrabbedInfoList [k].isCurrentWeapon = false;
        }
    }

    public bool isCurrentMeleeWeaponSheathedOrCarried ()
    {
        return currentMeleeWeaponSheathedOrCarried;
    }

    public bool isGrabObjectsEnabled ()
    {
        return mainGrabObjects.isGrabObjectsEnabled ();
    }

    public bool isCarryingRegularPhysicalObject ()
    {
        return mainGrabObjects.isCarryingRegularPhysicalObject ();
    }

    public void enableOrDisableMeleeWeaponMeshesOnCharacterBody (bool state)
    {
        if (mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isUsingGenericModelActive ()) {
            if (state) {
                return;
            }
        }

        int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

        for (int k = 0; k < meleeWeaponGrabbedInfoListCount; k++) {
            if (meleeWeaponGrabbedInfoList [k].carryingWeapon && !meleeWeaponGrabbedInfoList [k].objectThrown) {
                if (meleeWeaponGrabbedInfoList [k].weaponMesh != null) {

                    bool setStateResult = state;

                    if (setStateResult) {
                        if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
                            setStateResult = false;
                        }
                    }

                    bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (meleeWeaponGrabbedInfoList [k].hideWeaponMeshWhenNotUsed);

                    if (hideWeaponMeshResult) {
                        setStateResult = false;
                    }

                    if (meleeWeaponGrabbedInfoList [k].weaponMesh.activeSelf != setStateResult) {
                        meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (setStateResult);

                        if (showDebugPrint) {
                            print ("enableOrDisableMeleeWeaponMeshesOnCharacterBody " + state + " " + meleeWeaponGrabbedInfoList [k].Name);
                        }
                    }
                }
            }
        }
    }

    public void enableOrDisableMeleeWeaponMeshOnCharacterBodyByNameAndInstantiateMesh (bool state, string weaponName)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

        if (meleeWeaponGrabbedInfoList.Count > weaponIndex && weaponIndex > -1) {

            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

            bool setEnableStateResult = state;

            if (setEnableStateResult) {
                if (hideWeaponMeshResult) {
                    setEnableStateResult = false;
                }
            }

            checkObjectMeshToEnableOrDisable (setEnableStateResult, currentMeleeWeaponGrabbedInfo);
        }
    }

    public bool checkIfHideWeaponMeshWhenNotUsedByName (string weaponNameToSearch)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        if (weaponIndex > -1) {
            if (meleeWeaponGrabbedInfoList [weaponIndex].carryingWeapon && meleeWeaponGrabbedInfoList [weaponIndex].weaponMesh != null) {
                return checkIfHideWeaponMeshWhenNotUsed (meleeWeaponGrabbedInfoList [weaponIndex].hideWeaponMeshWhenNotUsed);
            }
        }

        return false;
    }

    public bool checkIfHideShieldMeshWhenNotUsedByName (string shieldNameToSearch)
    {
        int weaponIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldNameToSearch);

        if (weaponIndex > -1) {
            if (shieldGrabbedInfoList [weaponIndex].carryingShield && shieldGrabbedInfoList [weaponIndex].shieldStored != null) {
                return checkIfHideWeaponMeshWhenNotUsed (shieldGrabbedInfoList [weaponIndex].hideWeaponMeshWhenNotUsed);
            }
        }

        return false;
    }

    public void enableOrDisableMeleeWeaponMeshOnCharacterBodyByName (bool state, string weaponNameToSearch)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        if (weaponIndex > -1) {
            if (meleeWeaponGrabbedInfoList [weaponIndex].carryingWeapon && meleeWeaponGrabbedInfoList [weaponIndex].weaponMesh != null) {
                if (meleeWeaponGrabbedInfoList [weaponIndex].weaponMesh.activeSelf != state) {
                    meleeWeaponGrabbedInfoList [weaponIndex].weaponMesh.SetActive (state);

                    if (showDebugPrint) {
                        print ("enableOrDisableMeleeWeaponMeshOnCharacterBodyByName " + state + " " + meleeWeaponGrabbedInfoList [weaponIndex].Name);
                    }
                }
            }
        }
    }

    public void enableOrDisableAllMeleeWeaponMeshesOnCharacterBody (bool state)
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            if (meleeWeaponGrabbedInfoList [k].carryingWeapon && !meleeWeaponGrabbedInfoList [k].objectThrown) {
                if (meleeWeaponGrabbedInfoList [k].weaponMesh != null) {
                    if (state) {
                        if (!meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
                            if (meleeWeaponGrabbedInfoList [k].weaponMesh.activeSelf != state) {
                                meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);

                                if (showDebugPrint) {
                                    print ("enableOrDisableAllMeleeWeaponMeshesOnCharacterBody " + state + " " + meleeWeaponGrabbedInfoList [k].Name);
                                }
                            }
                        }
                    } else {
                        if (meleeWeaponGrabbedInfoList [k].weaponMesh.activeSelf != state) {
                            meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);

                            if (showDebugPrint) {
                                print ("enableOrDisableAllMeleeWeaponMeshesOnCharacterBody " + state + " " + meleeWeaponGrabbedInfoList [k].Name);
                            }
                        }
                    }
                }
            }
        }
    }

    public void enableOrDisableAllMeleeWeaponMeshesOnCharacterBodyCheckingIfHiddingMeshes (bool state)
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            if (meleeWeaponGrabbedInfoList [k].carryingWeapon && !meleeWeaponGrabbedInfoList [k].objectThrown) {
                if (meleeWeaponGrabbedInfoList [k].weaponMesh != null) {
                    if (state) {
                        if (!meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
                            bool weaponMeshActiveResult = state;

                            bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (meleeWeaponGrabbedInfoList [k].hideWeaponMeshWhenNotUsed);

                            if (hideWeaponMeshResult) {
                                weaponMeshActiveResult = false;
                            }

                            if (meleeWeaponGrabbedInfoList [k].weaponMesh.activeSelf != weaponMeshActiveResult) {
                                meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (weaponMeshActiveResult);

                                if (showDebugPrint) {
                                    print ("enableOrDisableAllMeleeWeaponMeshesOnCharacterBody " + weaponMeshActiveResult + " " + meleeWeaponGrabbedInfoList [k].Name);
                                }
                            }
                        }
                    } else {
                        if (meleeWeaponGrabbedInfoList [k].weaponMesh.activeSelf != state) {
                            meleeWeaponGrabbedInfoList [k].weaponMesh.SetActive (state);

                            if (showDebugPrint) {
                                print ("enableOrDisableAllMeleeWeaponMeshesOnCharacterBody " + state + " " + meleeWeaponGrabbedInfoList [k].Name);
                            }
                        }
                    }
                }
            }
        }
    }

    public void enableOrDisableAllMeleeWeaponShieldMeshesOnCharacterBody (bool state)
    {
        int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

        for (int k = 0; k < shieldGrabbedInfoListCount; k++) {
            shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [k];

            if (currentShieldInfo.carryingShield && currentShieldInfo.shieldStored != null) {
                if (state) {
                    if (!currentShieldInfo.isCurrentShield) {
                        if (currentShieldInfo.shieldStored.activeSelf != state) {
                            currentShieldInfo.shieldStored.SetActive (state);

                            if (showDebugPrint) {
                                print (state);
                            }
                        }
                    }
                } else {
                    if (currentShieldInfo.shieldStored.activeSelf != state) {
                        currentShieldInfo.shieldStored.SetActive (state);

                        if (showDebugPrint) {
                            print (state);
                        }
                    }
                }
            }
        }
    }

    public void enableOrDisableAllMeleeWeaponShieldMeshesOnCharacterBodyCheckingIfHiddingMeshes (bool state)
    {
        int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

        for (int k = 0; k < shieldGrabbedInfoListCount; k++) {
            shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [k];

            if (currentShieldInfo.carryingShield && currentShieldInfo.shieldStored != null) {
                if (state) {
                    if (!currentShieldInfo.isCurrentShield) {
                        bool weaponMeshActiveResult = state;

                        bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentShieldInfo.hideWeaponMeshWhenNotUsed);

                        if (hideWeaponMeshResult) {
                            weaponMeshActiveResult = false;
                        }

                        if (currentShieldInfo.shieldStored.activeSelf != weaponMeshActiveResult) {
                            currentShieldInfo.shieldStored.SetActive (weaponMeshActiveResult);

                            if (showDebugPrint) {
                                print (weaponMeshActiveResult);
                            }
                        }
                    }
                } else {
                    if (currentShieldInfo.shieldStored.activeSelf != state) {
                        currentShieldInfo.shieldStored.SetActive (state);

                        if (showDebugPrint) {
                            print (state);
                        }
                    }
                }
            }
        }
    }

    public bool characterIsCarryingWeapon ()
    {
        return meleeWeaponsGrabbedManagerActive && mainGrabbedObjectMeleeAttackSystem.isCarryingObject ();
    }

    public float getLastTimeDrawMeleeWeapon ()
    {
        return mainGrabbedObjectMeleeAttackSystem.getLastTimeDrawMeleeWeapon ();
    }

    //INVENTORY FUNCTIONS
    public bool equipMeleeWeapon (string weaponNameToSearch, bool checkIfWeaponNotFound)
    {
        if (!mainGrabbedObjectMeleeAttackSystem.isGrabbedObjectMeleeAttackEnabled () || !mainGrabbedObjectMeleeAttackSystem.isCanGrabMeleeObjectsEnabled ()) {
            return false;
        }

        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        equipMeleeWeaponPaused = true;

        if (weaponIndex > -1) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
                return true;
            } else {
                if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {

                } else {
                    checkWeaponByNumber (weaponIndex);

                    if (!meleeWeaponsGrabbedManagerActive) {
                        //						mainInventoryManager.checkQuickAccessSlotToSelectByName (weaponNameToSearch);

                        //						if (useEventsOnMeleeWeaponEquipped) {
                        //							eventOnMeleeWeaponEquipped.Invoke ();
                        //						}
                    }

                    updateQuickAccesSlotOnInventory (weaponNameToSearch);

                    return true;
                }
            }
        }

        bool weaponEquippedCorrectly = false;

        bool canSearchWeapon = true;

        if (showDebugPrint) {
            print (weaponNameToSearch);
        }

        weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        if (checkIfWeaponNotFound) {
            if (weaponIndex == -1) {
                canSearchWeapon = false;
            }
        }

        if (canSearchWeapon) {
            if (weaponIndex > -1 && meleeWeaponGrabbedInfoList [weaponIndex].weaponInstantiated) {
                if (showDebugPrint) {
                    print ("check weapon by number");
                }

                weaponEquippedCorrectly = true;
            } else {
                if (showDebugPrint) {
                    print ("instantiate new weapon");
                }

                int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == weaponNameToSearch);

                if (weaponPrefabIndex > -1) {
                    meleeWeaponPrefabInfo currentMeleeWeaponPrefabInfo = meleeWeaponPrefabInfoList [weaponPrefabIndex];

                    GameObject newWeaponToCarry = (GameObject)Instantiate (currentMeleeWeaponPrefabInfo.weaponPrefab, Vector3.up * 1000, Quaternion.identity);

                    meleeWeaponGrabbedInfo newMeleeWeaponGrabbedInfo = new meleeWeaponGrabbedInfo ();
                    newMeleeWeaponGrabbedInfo.Name = weaponNameToSearch;

                    newMeleeWeaponGrabbedInfo.weaponPrefabIndex = currentMeleeWeaponPrefabInfo.weaponPrefabIndex;

                    newMeleeWeaponGrabbedInfo.weaponStored = newWeaponToCarry;

                    newMeleeWeaponGrabbedInfo.carryingWeapon = true;

                    if (newMeleeWeaponGrabbedInfo.weaponStored.activeSelf) {
                        newMeleeWeaponGrabbedInfo.weaponStored.SetActive (false);
                    }

                    newMeleeWeaponGrabbedInfo.weaponInstantiated = true;

                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = newMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentGrabPhysicalObjectMeleeAttackSystem.hideWeaponMeshWhenNotUsed);

                    newMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed = hideWeaponMeshResult;

                    meleeWeaponGrabbedInfoList.Add (newMeleeWeaponGrabbedInfo);

                    weaponEquippedCorrectly = true;
                }
            }
        }


        equipMeleeWeaponPaused = false;

        if (weaponEquippedCorrectly) {

            updateQuickAccesSlotOnInventory (weaponNameToSearch);
        }

        return weaponEquippedCorrectly;
    }

    void updateQuickAccesSlotOnInventory (string weaponNameToSearch)
    {
        if (storePickedWeaponsOnInventory) {
            mainInventoryManager.showWeaponSlotsParentWhenWeaponSelectedByName (weaponNameToSearch);
        }
    }

    public bool unEquipMeleeWeapon (string weaponNameToSearch, bool dropWeaponObject)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponNameToSearch);

        if (weaponIndex > -1) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
                    mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (false);

                    mainGrabObjects.grabbed = true;

                    mainGrabObjects.checkIfDropObject (currentMeleeWeaponGrabbedInfo.weaponStored);
                } else {
                    currentMeleeWeaponGrabbedInfo.weaponStored.transform.position = mainGrabObjects.transform.position + mainGrabObjects.transform.up + mainGrabObjects.transform.forward;
                }

                bool hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (currentMeleeWeaponGrabbedInfo.hideWeaponMeshWhenNotUsed);

                if (!hideWeaponMeshResult) {
                    checkObjectMeshToEnableOrDisable (false, currentMeleeWeaponGrabbedInfo);
                }

                mainGrabObjects.removeCurrentPhysicalObjectToGrabFound (currentMeleeWeaponGrabbedInfo.weaponStored);

                if (currentMeleeWeaponGrabbedInfo.weaponStored.activeSelf != dropWeaponObject) {
                    currentMeleeWeaponGrabbedInfo.weaponStored.SetActive (dropWeaponObject);
                }

                mainGrabbedObjectMeleeAttackSystem.setRemoveWeaponsFromManagerState (true);
            }

            currentMeleeWeaponGrabbedInfo.isCurrentWeapon = false;

            currentMeleeWeaponGrabbedInfo.carryingWeapon = false;

            currentMeleeWeaponSheathedOrCarried = false;

            currentNumberOfWeaponsAvailable = getCurrentNumberOfWeaponsAvailable ();

            currentWeaponIndex = meleeWeaponGrabbedInfoList.Count - 1;

            bool currentWeaponIndexFound = false;

            for (int i = meleeWeaponGrabbedInfoList.Count - 1; i >= 0; i--) {
                if (!currentWeaponIndexFound) {
                    if (meleeWeaponGrabbedInfoList [i].carryingWeapon) {
                        currentWeaponIndex = i;

                        currentWeaponIndexFound = true;
                    }
                }
            }

            if (currentWeaponIndex < 0) {
                currentWeaponIndex = 0;
            }

            return true;
        }

        return false;
    }

    public bool checkWeaponToSelectOnQuickAccessSlots (string weaponName)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

        if (weaponIndex > -1) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [weaponIndex];

            checkWeaponByNumber (weaponIndex);

            mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (false);

            if (currentMeleeWeaponGrabbedInfo.isCurrentWeapon) {
                return true;
            }
        }

        return false;
    }

    public Transform getCurrentGrabbedObjectTransform ()
    {
        if (characterIsCarryingWeapon ()) {
            return mainGrabbedObjectMeleeAttackSystem.getCurrentGrabbedObjectTransform ();
        }

        return null;
    }

    public bool isMeleeWeaponsGrabbedManagerActive ()
    {
        return meleeWeaponsGrabbedManagerActive;
    }

    public string getCurrentWeaponName ()
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            if (meleeWeaponGrabbedInfoList [k].isCurrentWeapon) {
                return meleeWeaponGrabbedInfoList [k].Name;
            }
        }

        return "";
    }

    public string getCurrentShieldName ()
    {
        for (int k = 0; k < shieldGrabbedInfoList.Count; k++) {
            if (shieldGrabbedInfoList [k].isCurrentShield) {
                return shieldGrabbedInfoList [k].Name;
            }
        }

        return "";
    }

    public string getEmptyWeaponToUseOnlyShield ()
    {
        return mainGrabbedObjectMeleeAttackSystem.getEmptyWeaponToUseOnlyShield ();
    }

    public GameObject getCurrentWeaponMeshByName (string weaponName)
    {
        for (int k = 0; k < meleeWeaponGrabbedInfoList.Count; k++) {
            if (meleeWeaponGrabbedInfoList [k].Name.Equals (weaponName)) {
                return meleeWeaponGrabbedInfoList [k].weaponMesh;
            }
        }

        return null;
    }

    public bool isWeaponInGrabbedInfoList (string weaponName)
    {
        int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == weaponName);

        if (weaponIndex > -1) {
            return true;
        }

        return false;
    }

    public string getCurrentWeaponActiveName ()
    {
        if (meleeWeaponGrabbedInfoList.Count > 0 && currentWeaponIndex < meleeWeaponGrabbedInfoList.Count) {
            return meleeWeaponGrabbedInfoList [currentWeaponIndex].Name;
        }

        return "";
    }

    public bool equipShield (string shieldName)
    {
        return setShieldActiveState (true, shieldName);
    }

    public bool unequipeShield (string shieldName)
    {
        bool unequipResult = setShieldActiveState (false, shieldName);

        int shieldInfoIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldName);

        if (shieldInfoIndex > -1) {
            if (shieldGrabbedInfoList [shieldInfoIndex].shieldStored != null) {
                if (shieldGrabbedInfoList [shieldInfoIndex].shieldStored.activeSelf) {
                    shieldGrabbedInfoList [shieldInfoIndex].shieldStored.SetActive (false);
                }

                shieldGrabbedInfoList [shieldInfoIndex].isCurrentShield = false;

                shieldGrabbedInfoList [shieldInfoIndex].carryingShield = false;
            }

            string emptyShieldWeaponName = getEmptyWeaponToUseOnlyShield ();

            int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == emptyShieldWeaponName);

            if (weaponIndex > -1) {
                if (meleeWeaponGrabbedInfoList [weaponIndex].weaponStored != null) {
                    if (meleeWeaponGrabbedInfoList [weaponIndex].weaponStored.activeSelf) {
                        meleeWeaponGrabbedInfoList [weaponIndex].weaponStored.SetActive (false);
                    }

                    meleeWeaponGrabbedInfoList [weaponIndex].isCurrentWeapon = false;

                    meleeWeaponGrabbedInfoList [weaponIndex].carryingWeapon = false;
                }
            }
        }

        return unequipResult;
    }
    //END INVENTORY FUNCTIONS


    //START SHIELD FUNCTIONS
    public bool setShieldActiveState (bool state, string shieldName)
    {
        //		print (shieldName + " " + state);

        int shieldInfoIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldName);

        int shieldPrefabIndex = shieldPrefabInfoList.FindIndex (s => s.Name == shieldName);

        if (shieldInfoIndex == -1) {
            if (shieldPrefabIndex > -1) {

                shieldPrefabInfo currentShieldPrefabInfo = shieldPrefabInfoList [shieldPrefabIndex];

                GameObject newWeaponToCarry = (GameObject)Instantiate (currentShieldPrefabInfo.shieldPrefab, Vector3.up * 1000, Quaternion.identity);

                shieldGrabbedInfo newShieldGrabbedInfo = new shieldGrabbedInfo ();
                newShieldGrabbedInfo.Name = shieldName;

                newShieldGrabbedInfo.shieldInstantiated = true;

                newShieldGrabbedInfo.shieldPrefabIndex = currentShieldPrefabInfo.shieldPrefabIndex;

                newShieldGrabbedInfo.shieldStored = newWeaponToCarry;

                newShieldGrabbedInfo.carryingShield = true;

                newShieldGrabbedInfo.mainMeleeShieldObjectSystem = newWeaponToCarry.GetComponent<meleeShieldObjectSystem> ();

                if (newShieldGrabbedInfo.mainMeleeShieldObjectSystem != null) {
                    newShieldGrabbedInfo.mainMeleeShieldObjectSystem.setCurrentCharacter (playerGameObject);

                    newShieldGrabbedInfo.mainMeleeShieldObjectSystemLocated = true;

                    newShieldGrabbedInfo.shieldCarriedOnRightArm = newShieldGrabbedInfo.mainMeleeShieldObjectSystem.shieldCarriedOnRightArm;
                }

                newShieldGrabbedInfo.equipShieldWhenPickedIfNotShieldEquippedPrevioulsy = currentShieldPrefabInfo.equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;

                bool hideWeaponMeshResult = false;

                if (newShieldGrabbedInfo.mainMeleeShieldObjectSystemLocated) {
                    hideWeaponMeshResult = checkIfHideWeaponMeshWhenNotUsed (newShieldGrabbedInfo.mainMeleeShieldObjectSystem.hideWeaponMeshWhenNotUsed);
                }

                newShieldGrabbedInfo.hideWeaponMeshWhenNotUsed = hideWeaponMeshResult;

                shieldGrabbedInfoList.Add (newShieldGrabbedInfo);

                shieldInfoIndex = shieldGrabbedInfoList.Count - 1;
            }
        }

        if (shieldInfoIndex > -1) {
            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int k = 0; k < shieldGrabbedInfoListCount; k++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [k];

                if (k == shieldInfoIndex) {
                    currentShieldGrabbedInfo = currentShieldInfo;

                    if (!currentShieldGrabbedInfo.isCurrentShield) {
                        if (currentShieldGrabbedInfo.useEventsOnEquipShieldChangeState) {
                            if (state) {
                                currentShieldGrabbedInfo.eventOnUnequippShield.Invoke ();
                            } else {
                                currentShieldGrabbedInfo.eventOnEquipShield.Invoke ();
                            }
                        }
                    }

                    currentShieldGrabbedInfo.isCurrentShield = true;

                    currentShieldGrabbedInfo.carryingShield = true;

                    if (currentShieldGrabbedInfo.shieldStored.activeSelf != state) {
                        currentShieldGrabbedInfo.shieldStored.SetActive (state);
                    }
                } else {
                    if (currentShieldInfo.isCurrentShield) {
                        if (currentShieldInfo.useEventsOnEquipShieldChangeState) {
                            currentShieldInfo.eventOnUnequippShield.Invoke ();
                        }
                    }

                    currentShieldInfo.isCurrentShield = false;

                    if (currentShieldInfo.shieldStored.activeSelf) {
                        currentShieldInfo.shieldStored.SetActive (false);
                    }
                }
            }

            mainGrabbedObjectMeleeAttackSystem.setShieldInfo (currentShieldGrabbedInfo.Name,
                currentShieldGrabbedInfo.shieldStored,
                shieldPrefabInfoList [shieldPrefabIndex].shieldHandMountPointTransformReference,
                shieldPrefabInfoList [shieldPrefabIndex].shieldBackMountPointTransformReference,
                currentShieldGrabbedInfo.shieldCarriedOnRightArm,
                state);

            mainGrabbedObjectMeleeAttackSystem.setShieldActiveState (state);

            if (state) {
                //				string currentShieldName = getCurrentShieldName ();
                //
                //				int weaponIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name.Equals (currentShieldName));
                //
                //				if (weaponIndex > -1) {
                //					
                //					grabPhysicalObjectMeleeAttackSystem currentShieldAttackSystem = meleeWeaponPrefabInfoList [weaponIndex].weaponPrefab.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();
                //
                //					if (currentShieldAttackSystem != null) {
                //						mainGrabbedObjectMeleeAttackSystem.setShieldProtectionValues (currentShieldAttackSystem.blockDamageProtectionAmount, currentShieldAttackSystem.reducedBlockDamageProtectionAmount);
                //					}
                //				}

                if (currentShieldGrabbedInfo.mainMeleeShieldObjectSystem != null) {
                    mainGrabbedObjectMeleeAttackSystem.setShieldProtectionValues (
                        currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.blockDamageProtectionAmount,
                        currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.reducedBlockDamageProtectionAmount,
                        currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.useMaxBlockRangeAngle,
                        currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.maxBlockRangeAngle);
                }
            }

            return true;
        }

        return false;
    }

    public void checkEquipShieldIfNotCarryingPreviously ()
    {
        if (!mainGrabbedObjectMeleeAttackSystem.carryingShield &&
            (meleeWeaponsGrabbedManagerActive || mainGrabbedObjectMeleeAttackSystem.shieldCanBeUsedWithoutMeleeWeapon)) {

            string lastInventoryObjectPickedName = mainInventoryManager.getLastInventoryObjectPickedName ();

            if (lastInventoryObjectPickedName != "") {
                for (int k = 0; k < shieldPrefabInfoList.Count; k++) {
                    if (shieldPrefabInfoList [k].equipShieldWhenPickedIfNotShieldEquippedPrevioulsy &&
                        shieldPrefabInfoList [k].Name.Equals (lastInventoryObjectPickedName)) {

                        mainInventoryManager.equipObjectByName (lastInventoryObjectPickedName);

                        return;
                    }
                }
            }
        }
    }

    public void toggleDrawOrSheatheShield (string shieldName)
    {
        int shieldInfoIndex = shieldGrabbedInfoList.FindIndex (s => s.Name == shieldName);

        bool shieldState = false;

        if (shieldInfoIndex > -1) {
            //			if (meleeWeaponsGrabbedManagerActive) {
            if (shieldGrabbedInfoList [shieldInfoIndex].isCurrentShield) {
                shieldState = !mainGrabbedObjectMeleeAttackSystem.shieldActive;
            } else {
                shieldState = true;
            }
            //			} else {
            //				print ("trying to draw the shield when not melee mode active, sending signal to quick access slots");
            //
            //				mainInventoryManager.changeToMeleeWeapons (mainGrabbedObjectMeleeAttackSystem.getEmptyWeaponToUseOnlyShield ());
            //			}
        } else {
            shieldState = true;
        }

        if (shieldState) {
            currentMeleeWeaponSheathedOrCarried = false;
        }

        drawOrSheatheShield (shieldState, shieldName);

        //		print (shieldState + " " + shieldName);
    }

    public void drawOrSheatheShield (bool state, string shieldName)
    {
        setShieldActiveState (true, shieldName);

        mainGrabbedObjectMeleeAttackSystem.drawOrSheatheShield (state);

        if (state) {
            if (!mainGrabbedObjectMeleeAttackSystem.shieldActive && mainGrabbedObjectMeleeAttackSystem.carryingShield) {
                mainGrabbedObjectMeleeAttackSystem.setShieldParentState (false);
            }
        } else {
            mainGrabbedObjectMeleeAttackSystem.setShieldActiveFieldValueDirectly (false);
        }
    }
    //END SHIELD FUNCTIONS

    public void checkToKeepWeapon ()
    {
        keepWeaponExternally (true);
    }

    public void checkToKeepWeaponWithoutCheckingInputActive ()
    {
        keepWeaponExternally (false);
    }

    void keepWeaponExternally (bool checkIfInputActive)
    {
        if (!meleeWeaponsGrabbedManagerActive) {
            return;
        }

        if (!isGrabObjectsEnabled ()) {
            return;
        }

        if ((mainGrabbedObjectMeleeAttackSystem.canUseWeaponsInput () || !checkIfInputActive) &&
            !mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget ()) {

            if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                keepWeapon (currentWeaponIndex);
            }
        }
    }

    public void setStartGameWithWeaponState (bool state)
    {
        startGameWithWeapon = state;
    }

    public void setStartGameWithWeaponStateFromEditor (bool state)
    {
        setStartGameWithWeaponState (state);

        updateComponent ();
    }

    public void setWeaponNameToStartGame (string newName)
    {
        weaponNameToStartGame = newName;
    }

    public void updateDurabilityAmountStateOnAllObjects ()
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].weaponStored != null && meleeWeaponGrabbedInfoList [i].carryingWeapon) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        currentGrabPhysicalObjectMeleeAttackSystem.updateDurabilityAmountState ();
                    }
                }
            }
        }

        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int i = 0; i < shieldGrabbedInfoListCount; i++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [i];

                if (currentShieldInfo.shieldStored != null && currentShieldInfo.carryingShield) {

                    if (currentShieldInfo.mainMeleeShieldObjectSystem != null) {
                        currentShieldInfo.mainMeleeShieldObjectSystem.updateDurabilityAmountState ();
                    }
                }
            }
        }
    }

    public void updateDurabilityAmountStateOnObjectByName (string weaponName)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].Name.Equals (weaponName) && meleeWeaponGrabbedInfoList [i].weaponStored != null) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        currentGrabPhysicalObjectMeleeAttackSystem.updateDurabilityAmountState ();
                    }

                    return;
                }
            }
        }
    }

    public float getDurabilityAmountStateOnObjectByName (string weaponName)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            if (lastDroppeWeaponDurabilityStored) {
                lastDroppeWeaponDurabilityStored = false;

                if (lastDroppedWeaponName.Equals (weaponName)) {
                    return lastDroppedWeaponDurabilityValue;
                }
            }

            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].Name.Equals (weaponName) && meleeWeaponGrabbedInfoList [i].weaponStored != null) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        return currentGrabPhysicalObjectMeleeAttackSystem.getDurabilityAmount ();
                    }
                }
            }
        }

        return -1;
    }

    public float getDurabilityAmountStateOnObjectShieldByName (string weaponName)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            //			if (lastDroppeWeaponDurabilityStored) {
            //				lastDroppeWeaponDurabilityStored = false;
            //
            //				if (lastDroppedWeaponName.Equals (weaponName)) {
            //					return lastDroppedWeaponDurabilityValue;
            //				}
            //			}

            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int i = 0; i < shieldGrabbedInfoListCount; i++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [i];

                if (currentShieldInfo.Name.Equals (weaponName) && currentShieldInfo.shieldStored != null) {

                    if (currentShieldInfo.mainMeleeShieldObjectSystem != null) {
                        return currentShieldInfo.mainMeleeShieldObjectSystem.getDurabilityAmount ();
                    }
                }
            }
        }

        return -1;
    }

    public void initializeDurabilityValue (float newAmount, string weaponName, int currentObjectIndex)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].Name.Equals (weaponName) && meleeWeaponGrabbedInfoList [i].weaponStored != null) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        currentGrabPhysicalObjectMeleeAttackSystem.initializeDurabilityValue (newAmount, currentObjectIndex);
                    }

                    return;
                }
            }
        }
    }

    public void setInventoryObjectIndex (string weaponName, int currentObjectIndex)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].Name.Equals (weaponName) && meleeWeaponGrabbedInfoList [i].weaponStored != null) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        currentGrabPhysicalObjectMeleeAttackSystem.setInventoryObjectIndex (currentObjectIndex);
                    }

                    return;
                }
            }
        }
    }

    public void initializeDurabilityValueOnShield (float newAmount, string shieldName, int currentObjectIndex)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int i = 0; i < shieldGrabbedInfoListCount; i++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [i];

                if (currentShieldInfo.Name.Equals (shieldName) && currentShieldInfo.shieldStored != null) {

                    if (currentShieldInfo.mainMeleeShieldObjectSystem != null) {
                        currentShieldInfo.mainMeleeShieldObjectSystem.initializeDurabilityValue (newAmount, currentObjectIndex);
                    }

                    return;
                }
            }
        }
    }

    public void setInventoryObjectIndexOnShield (string shieldName, int currentObjectIndex)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int i = 0; i < shieldGrabbedInfoListCount; i++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [i];

                if (currentShieldInfo.Name.Equals (shieldName) && currentShieldInfo.shieldStored != null) {

                    if (currentShieldInfo.mainMeleeShieldObjectSystem != null) {
                        currentShieldInfo.mainMeleeShieldObjectSystem.setInventoryObjectIndex (currentObjectIndex);
                    }

                    return;
                }
            }
        }
    }

    public void repairObjectFully (string weaponName)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            for (int i = 0; i < meleeWeaponGrabbedInfoListCount; i++) {
                if (meleeWeaponGrabbedInfoList [i].Name.Equals (weaponName) && meleeWeaponGrabbedInfoList [i].weaponStored != null) {
                    grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = meleeWeaponGrabbedInfoList [i].weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                    if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                        currentGrabPhysicalObjectMeleeAttackSystem.repairObjectFully ();
                    }

                    return;
                }
            }
        }
    }

    public void repairObjectShieldFully (string weaponName)
    {
        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            int shieldGrabbedInfoListCount = shieldGrabbedInfoList.Count;

            for (int i = 0; i < shieldGrabbedInfoListCount; i++) {
                shieldGrabbedInfo currentShieldInfo = shieldGrabbedInfoList [i];

                if (currentShieldInfo.Name.Equals (weaponName) && currentShieldInfo.shieldStored != null) {

                    if (currentShieldInfo.mainMeleeShieldObjectSystem != null) {
                        currentShieldInfo.mainMeleeShieldObjectSystem.repairObjectFully ();
                    }

                    return;
                }
            }
        }
    }

    public void breakFullDurabilityOnCurrentWeapon ()
    {
        if (!characterIsCarryingWeapon ()) {
            return;
        }

        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnAttackEnabled || mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = mainGrabbedObjectMeleeAttackSystem.getCurrentGrabPhysicalObjectMeleeAttackSystem ();

            if (currentGrabPhysicalObjectMeleeAttackSystem != null) {
                currentGrabPhysicalObjectMeleeAttackSystem.breakFullDurabilityOnCurrentWeapon ();
            }
        }
    }

    public void breakFullDurabilityOnCurrentShield ()
    {
        if (!characterIsCarryingWeapon ()) {
            return;
        }

        if (mainGrabbedObjectMeleeAttackSystem.checkDurabilityOnBlockEnabled) {
            if (currentShieldGrabbedInfo != null) {
                currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.breakFullDurabilityOnCurrentWeapon ();
            }
        }
    }

    public bool checkDurabilityOnBlockWithShield (float extraMultiplier)
    {
        if (currentShieldGrabbedInfo != null) {
            return currentShieldGrabbedInfo.mainMeleeShieldObjectSystem.checkDurabilityOnBlockWithShield (extraMultiplier);
        }

        return false;
    }

    public bool checkIfReturnThrowWeaponOrDropItExternally ()
    {
        if (isCurrentWeaponThrown ()) {
            if (dropThrownWeaponIfChangingToAnotherWeaponEnabled) {
                mainGrabbedObjectMeleeAttackSystem.dropMeleeWeaponsExternallyWithoutResult ();

                return false;
            } else {
                mainGrabbedObjectMeleeAttackSystem.inputThrowOrReturnObject ();

                return true;
            }
        }

        return false;
    }

    public void checkIfDropThrownWeaponWhenUsingDevice ()
    {
        if (isCurrentWeaponThrown ()) {
            if (dropThrownWeaponWhenUsingDeviceEnabled) {
                mainGrabbedObjectMeleeAttackSystem.dropMeleeWeaponsExternallyWithoutResult ();
            }
        }
    }

    public bool isCurrentWeaponThrown ()
    {
        return mainGrabbedObjectMeleeAttackSystem.isCurrentWeaponThrown ();
    }

    public bool isAimingBowActive ()
    {
        return mainGrabbedObjectMeleeAttackSystem.isAimingBowActive ();
    }

    public bool isCuttingModeActive ()
    {
        return mainGrabbedObjectMeleeAttackSystem.isCuttingModeActive ();
    }

    public bool isBlockActive ()
    {
        return mainGrabbedObjectMeleeAttackSystem.isBlockActive ();
    }

    public bool isDrawWeaponAtStartIfFoundOnInitializingInventoryActive ()
    {
        return drawWeaponAtStartIfFoundOnInitializingInventory;
    }

    //EDITOR FUNCTIONS
    public void setWeaponNameToStartGameFromEditor (string newName)
    {
        setWeaponNameToStartGame (newName);

        updateComponent ();
    }

    public void addNewMeleeWeaponPrefab (GameObject newWeaponPrefab, string newWeaponName, bool useBowWeaponType)
    {
        if (newWeaponPrefab != null) {
            int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == newWeaponName);

            if (weaponPrefabIndex < 0) {

                meleeWeaponPrefabInfo newMeleeWeaponPrefabInfo = new meleeWeaponPrefabInfo ();

                newMeleeWeaponPrefabInfo.Name = newWeaponName;

                newMeleeWeaponPrefabInfo.weaponPrefab = newWeaponPrefab;

                newMeleeWeaponPrefabInfo.useBowWeaponType = useBowWeaponType;

                newMeleeWeaponPrefabInfo.weaponPrefabIndex = meleeWeaponPrefabInfoList.Count;

                meleeWeaponPrefabInfoList.Add (newMeleeWeaponPrefabInfo);

                updateComponent ();
            } else {
                meleeWeaponPrefabInfo newMeleeWeaponPrefabInfo = meleeWeaponPrefabInfoList [weaponPrefabIndex];

                if (newMeleeWeaponPrefabInfo.weaponPrefab == null) {
                    newMeleeWeaponPrefabInfo.weaponPrefab = newWeaponPrefab;

                    updateComponent ();
                }
            }
        }
    }

    public void removeMeleeWeaponPrefab (string newWeaponName)
    {
        int weaponPrefabIndex = meleeWeaponPrefabInfoList.FindIndex (s => s.Name == newWeaponName);

        if (weaponPrefabIndex > -1) {
            meleeWeaponPrefabInfoList.RemoveAt (weaponPrefabIndex);

            int weaponPrefabIndexCounter = 0;

            for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
                meleeWeaponPrefabInfoList [k].weaponPrefabIndex = weaponPrefabIndexCounter;

                weaponPrefabIndexCounter++;
            }

            updateComponent ();
        }
    }

    public bool checkIfMeleeShieldExists (string shieldName)
    {
        int shieldPrefabIndex = shieldPrefabInfoList.FindIndex (s => s.Name == shieldName);

        if (shieldPrefabIndex < 0) {
            return true;
        }

        return false;
    }

    public void removeMeleeShieldPrefab (string shieldName)
    {
        int shieldPrefabIndex = shieldPrefabInfoList.FindIndex (s => s.Name == shieldName);

        if (shieldPrefabIndex > -1) {
            shieldPrefabInfoList.RemoveAt (shieldPrefabIndex);

            int shieldPrefabIndexCounter = 0;

            for (int k = 0; k < shieldPrefabInfoList.Count; k++) {
                shieldPrefabInfoList [k].shieldPrefabIndex = shieldPrefabIndexCounter;

                shieldPrefabIndexCounter++;
            }

            updateComponent ();
        }
    }

    public void addNewMeleeShieldPrefab (GameObject newShieldPrefab, string newShieldName)
    {
        if (newShieldPrefab != null) {
            int shieldPrefabIndex = shieldPrefabInfoList.FindIndex (s => s.Name == newShieldName);

            if (shieldPrefabIndex < 0) {
                shieldPrefabInfo newShieldPrefabInfo = new shieldPrefabInfo ();

                newShieldPrefabInfo.Name = newShieldName;

                newShieldPrefabInfo.shieldPrefab = newShieldPrefab;

                newShieldPrefabInfo.shieldPrefabIndex = shieldPrefabInfoList.Count;

                if (shieldPrefabInfoList.Count > 0) {
                    newShieldPrefabInfo.shieldBackMountPointTransformReference = shieldPrefabInfoList [0].shieldBackMountPointTransformReference;
                    newShieldPrefabInfo.shieldHandMountPointTransformReference = shieldPrefabInfoList [0].shieldHandMountPointTransformReference;
                }

                shieldPrefabInfoList.Add (newShieldPrefabInfo);

                updateComponent ();
            } else {
                shieldPrefabInfo newShieldPrefabInfo = shieldPrefabInfoList [shieldPrefabIndex];

                if (newShieldPrefabInfo.shieldPrefab == null) {
                    newShieldPrefabInfo.shieldPrefab = newShieldPrefab;

                    updateComponent ();
                }
            }
        }
    }


    //INPUT FUNCTIONS
    bool canUseInput ()
    {
        if (mainGrabbedObjectMeleeAttackSystem.mainPlayerController.iscloseCombatAttackInProcess ()) {
            return false;
        }

        if (!mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isActionActive ()) {
            return false;
        }

        return true;
    }

    public void inputDrawOrKeepMeleeWeapon ()
    {
        if (isCurrentWeaponThrown ()) {
            mainGrabbedObjectMeleeAttackSystem.inputThrowOrReturnObject ();

            return;
        }

        if (getCurrentNumberOfWeaponsAvailable () == 0) {
            return;
        }

        drawOrKeepMeleeWeapon (true);
    }

    public void drawOrKeepMeleeWeaponWithoutCheckingInputActive ()
    {
        drawOrKeepMeleeWeapon (false);
    }

    public void drawMeleeWeaponGrabbedCheckingAnimationDelay ()
    {
        bool activateDrawCoroutineResult = false;

        if (currentWeaponIndex < meleeWeaponGrabbedInfoList.Count && currentWeaponIndex >= 0) {
            meleeWeaponGrabbedInfo currentMeleeWeaponGrabbedInfo = meleeWeaponGrabbedInfoList [currentWeaponIndex];

            if (currentMeleeWeaponGrabbedInfo.weaponStored != null) {
                grabPhysicalObjectMeleeAttackSystem currentGrabPhysicalObjectMeleeAttackSystem = currentMeleeWeaponGrabbedInfo.weaponStored.GetComponent<grabPhysicalObjectMeleeAttackSystem> ();

                if (currentGrabPhysicalObjectMeleeAttackSystem.useDrawKeepWeaponAnimation) {
                    StartCoroutine (drawMeleeWeaponGrabbedCheckingAnimationDelayCoroutine ());

                    activateDrawCoroutineResult = true;
                }
            }
        }

        if (!activateDrawCoroutineResult) {
            drawOrKeepMeleeWeapon (false);
        }
    }

    IEnumerator drawMeleeWeaponGrabbedCheckingAnimationDelayCoroutine ()
    {
        yield return new WaitForSeconds (0.2f);

        drawOrKeepMeleeWeapon (false);
    }

    void drawOrKeepMeleeWeapon (bool checkIfInputActive)
    {
        if (!meleeWeaponsGrabbedManagerActive) {
            return;
        }

        if (!isGrabObjectsEnabled ()) {
            return;
        }

        if (mainGrabbedObjectMeleeAttackSystem.isAttackInProcess ()) {
            return;
        }

        if ((mainGrabbedObjectMeleeAttackSystem.canUseWeaponsInput () || !checkIfInputActive) &&
            !mainGrabbedObjectMeleeAttackSystem.isObjectThrownTravellingToTarget ()) {
            if (mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                keepWeapon (currentWeaponIndex);
            } else {
                checkIfDrawWeapon ();
            }
        }
    }

    public void resetCurrentAttackByIndex ()
    {
        mainGrabbedObjectMeleeAttackSystem.resetCurrentAttackByIndex ();
    }

    public void activateGrabbedObjectMeleeAttackByIndex ()
    {
        mainGrabbedObjectMeleeAttackSystem.activateGrabbedObjectMeleeAttackByIndex ();
    }

    public void activateOrDeactivateBlockGrabbedObjectMeleee (bool state)
    {
        if (state) {
            mainGrabbedObjectMeleeAttackSystem.inputActivateBlock ();
        } else {
            mainGrabbedObjectMeleeAttackSystem.inputDeactivateBlock ();
        }
    }

    public void activateOrDeactivateBlockGrabbedObjectMeleeeCheckingCurrentState (bool state)
    {
        if (state) {
            if (!mainGrabbedObjectMeleeAttackSystem.isBlockActive ()) {
                mainGrabbedObjectMeleeAttackSystem.inputActivateBlock ();
            }
        } else {
            if (mainGrabbedObjectMeleeAttackSystem.isBlockActive ()) {
                mainGrabbedObjectMeleeAttackSystem.inputDeactivateBlock ();
            }
        }
    }

    public void setIgnoreUseDrawKeepWeaponAnimationState (bool state)
    {
        mainGrabbedObjectMeleeAttackSystem.setIgnoreUseDrawKeepWeaponAnimationState (state);
    }

    public void setOriginalIgnoreUseDrawKeepWeaponAnimationState ()
    {
        mainGrabbedObjectMeleeAttackSystem.setOriginalIgnoreUseDrawKeepWeaponAnimationState ();
    }

    public void setIsCarryingObjectState (bool state)
    {
        carryingObject = state;

        if (carryingObject) {
            fullBodyAwarenessActive = mainGrabbedObjectMeleeAttackSystem.mainPlayerController.isFullBodyAwarenessActive ();
        } else {

        }

        checkIKHandsOnGrabWeaponStateChange (state);

        if (useEventOnGrabDropWeapon) {
            if (state) {
                eventOnGrabWeapon.Invoke ();
            } else {
                eventOnDropWeapon.Invoke ();
            }
        }

        if (storePickedWeaponsOnInventory) {
            if (state) {
                mainInventoryManager.setLastWeaponCarriedOnHandsName (getCurrentWeaponName ());
            }
        }
    }

    void checkIKHandsOnGrabWeaponStateChange (bool state)
    {
        if (useIKOnHands) {
            bool setHandsResult = state;

            if (useIKOnHandsOnlyOnFba) {
                if (!fullBodyAwarenessActive) {
                    setHandsResult = false;
                }
            }

            if (setHandsResult) {
                if (mainGrabbedObjectMeleeAttackSystem.checkIfCurrentWeaponignoreUseIKOnHands ()) {
                    setHandsResult = false;
                }
            }

            if (setHandsResult) {
                bool isCurrentWeaponIsCarriedOnRightHand = mainGrabbedObjectMeleeAttackSystem.isCurrentWeaponIsCarriedOnRightHand ();

                if (isCurrentWeaponIsCarriedOnRightHand) {
                    handsOnMeleeWeaponIKSystem.enableOnlyRightHand ();
                } else {
                    handsOnMeleeWeaponIKSystem.enableOnlyLeftHand ();
                }

                mainIKSystem.setTemporalOnAnimatorIKComponentActiveIfNotInUse (handsOnMeleeWeaponIKSystem);
            } else {
                mainIKSystem.removeThisTemporalOnAnimatorIKComponentIfIsCurrent (handsOnMeleeWeaponIKSystem);
            }
        }
    }

    public void setFullBodyAwarenessActiveState (bool state)
    {
        fullBodyAwarenessActive = state;

        if (fullBodyAwarenessActive) {
            if (carryingObject) {
                checkIKHandsOnGrabWeaponStateChange (true);
            } else {

            }
        } else {
            if (carryingObject) {
                checkIKHandsOnGrabWeaponStateChange (true);
            } else {

            }
        }
    }

    public int getCurrentNumberOfWeaponsAvailable ()
    {
        currentNumberOfWeaponsAvailable = meleeWeaponGrabbedInfoList.Count;

        if (currentNumberOfWeaponsAvailable != 0) {
            int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

            currentNumberOfWeaponsAvailable = 0;

            for (int k = 0; k < meleeWeaponGrabbedInfoListCount; k++) {
                if (meleeWeaponGrabbedInfoList [k].carryingWeapon) {
                    currentNumberOfWeaponsAvailable++;
                }
            }
        }

        return currentNumberOfWeaponsAvailable;
    }

    public bool checkIfCarryingWeaponByName (string weaponName)
    {
        int meleeWeaponGrabbedInfoListCount = meleeWeaponGrabbedInfoList.Count;

        for (int k = 0; k < meleeWeaponGrabbedInfoListCount; k++) {
            if (meleeWeaponGrabbedInfoList [k].carryingWeapon && meleeWeaponGrabbedInfoList [k].Name.Equals (weaponName)) {
                return true;
            }
        }

        return false;
    }

    public string getFirstRegularMeleeWeaponTypeAvailableName ()
    {
        for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
            if (!meleeWeaponPrefabInfoList [k].useBowWeaponType) {
                int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == meleeWeaponPrefabInfoList [k].Name);

                if (weaponIndex != -1) {
                    if (meleeWeaponGrabbedInfoList [weaponIndex].carryingWeapon) {
                        return meleeWeaponPrefabInfoList [k].Name;
                    }
                }
            }
        }

        return "";
    }

    public string getFirstBowMeleeWeaponTypeAvailableName ()
    {
        for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
            if (meleeWeaponPrefabInfoList [k].useBowWeaponType) {
                int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == meleeWeaponPrefabInfoList [k].Name);

                if (weaponIndex != -1) {
                    if (meleeWeaponGrabbedInfoList [weaponIndex].carryingWeapon) {
                        return meleeWeaponPrefabInfoList [k].Name;
                    }
                }
            }
        }

        return "";
    }

    public bool isThereAnyBowMeleeWeaponTypeAvailable ()
    {
        for (int k = 0; k < meleeWeaponPrefabInfoList.Count; k++) {
            if (meleeWeaponPrefabInfoList [k].useBowWeaponType) {
                int weaponIndex = meleeWeaponGrabbedInfoList.FindIndex (s => s.Name == meleeWeaponPrefabInfoList [k].Name);

                if (weaponIndex != -1) {
                    return true;
                }
            }
        }

        return false;
    }

    public void selectAndDrawFirstBowMeleeWeaponTypeAvailable ()
    {
        if (!meleeWeaponsGrabbedManagerActive) {
            return;
        }

        if (!isGrabObjectsEnabled ()) {
            return;
        }

        string weaponNameToUse = getFirstBowMeleeWeaponTypeAvailableName ();

        if (weaponNameToUse != "") {
            checkWeaponToSelectOnQuickAccessSlots (weaponNameToUse);
        }
    }

    public void checkEventOnWeaponStolen ()
    {
        if (useEventOnWeaponStolen) {
            eventOnWeaponStolen.Invoke ();
        }
    }

    //EDITOR FUNCTIONS
    void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Add Shield", gameObject);
    }

    [System.Serializable]
    public class meleeWeaponGrabbedInfo
    {
        public string Name;
        public bool weaponInstantiated;
        public GameObject weaponStored;
        public bool canBeSpawnedInfiniteTimes;
        public bool isCurrentWeapon;
        public bool carryingWeapon;
        public int weaponPrefabIndex;
        public bool hideWeaponMeshWhenNotUsed;

        public bool objectThrown;

        public GameObject weaponMesh;
    }

    [System.Serializable]
    public class meleeWeaponPrefabInfo
    {
        public string Name;
        public GameObject weaponPrefab;
        public int weaponPrefabIndex;

        public bool useBowWeaponType;
    }

    [System.Serializable]
    public class shieldPrefabInfo
    {
        public string Name;
        public GameObject shieldPrefab;
        public Transform shieldHandMountPointTransformReference;
        public Transform shieldBackMountPointTransformReference;
        public int shieldPrefabIndex;

        public bool equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;
    }

    [System.Serializable]
    public class shieldGrabbedInfo
    {
        public string Name;
        public bool shieldInstantiated;
        public GameObject shieldStored;

        [Space]

        public bool isCurrentShield;
        public bool carryingShield;

        public int shieldPrefabIndex;

        [Space]

        public bool hideWeaponMeshWhenNotUsed;
        public bool shieldCarriedOnRightArm;

        [Space]

        public meleeShieldObjectSystem mainMeleeShieldObjectSystem;

        public bool mainMeleeShieldObjectSystemLocated;

        public bool equipShieldWhenPickedIfNotShieldEquippedPrevioulsy;

        [Space]

        public bool useEventsOnEquipShieldChangeState;
        public UnityEvent eventOnEquipShield;
        public UnityEvent eventOnUnequippShield;
    }
}
