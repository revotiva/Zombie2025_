﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameKitController.Audio;
using UnityEngine.UI;
using UnityEngine.Events;

public class playerWeaponSystem : MonoBehaviour
{
    public GameObject playerControllerGameObject;
    public GameObject playerCameraGameObject;
    public weaponInfo weaponSettings;
    public AudioClip outOfAmmo;
    public AudioElement outOfAmmoAudioElement;
    public GameObject weaponProjectile;
    public LayerMask layer;

    public bool projectilesPoolEnabled = true;

    public int maxAmountOfPoolElementsOnWeapon = 30;

    public bool reloadingWithAnimationActive;

    public bool reloading;
    public bool carryingWeaponInThirdPerson;
    public bool carryingWeaponInFirstPerson;
    public bool aimingInThirdPerson;
    public bool aimingInFirstPerson;

    public string [] impactDecalList;
    public int impactDecalIndex;
    public string impactDecalName;

    public string mainDecalManagerName = "Decal Manager";

    public string mainNoiseMeshManagerName = "Noise Mesh Manager";

    decalManager impactDecalManager;

    public bool useHumanBodyBonesEnum;
    public HumanBodyBones weaponParentBone;

    public bool canMarkTargets;
    public List<string> tagListToMarkTargets = new List<string> ();
    public LayerMask markTargetsLayer;
    public string markTargetName;
    public float maxDistanceToMarkTargets;
    public bool canMarkTargetsOnFirstPerson;
    public bool canMarkTargetsOnThirdPerson;
    public bool aimOnFirstPersonToMarkTarget;
    public bool useMarkTargetSound;
    public AudioClip markTargetSound;
    public AudioElement markTargetAudioElement;

    public bool ignoreTriggerOnRaycastEnabled = true;

    public IKWeaponSystem IKWeaponManager;
    public playerWeaponsManager weaponsManager;
    public launchTrayectory parable;
    public Collider playerCollider;

    public bool FBAWeaponColliderAssigned;
    public Collider FBAWeaponCollider;

    public bool headColliderOnFBAAssigned;
    public Collider headColliderOnFBA;

    public Camera mainCamera;
    public Transform mainCameraTransform;
    public Animation weaponAnimation;
    public AudioSource weaponEffectsSource;

    public bool useEventOnSetDualWeapon;
    public UnityEvent eventOnSetRightWeapon;
    public UnityEvent eventOnSetLeftWeapon;

    public bool useEventOnSetSingleWeapon;
    public UnityEvent eventOnSetSingleWeapon;

    List<GameObject> shells = new List<GameObject> ();
    float destroyShellsTimer = 0;
    bool shellsActive;

    public bool useRemoteEventOnObjectsFound;
    public List<string> remoteEventNameList = new List<string> ();

    public bool useRemoteEventOnObjectsFoundOnExplosion;
    public string remoteEventNameOnExplosion;


    public bool useAbilitiesListToEnableOnWeapon;
    public List<string> abilitiesListToEnableOnWeapon = new List<string> ();

    public bool useAbilitiesListToDisableOnWeapon;
    public List<string> abilitiesListToDisableOnWeapon = new List<string> ();

    public bool useObjectDurability;

    public float durabilityUsedOnAttack = 1;

    public durabilityInfo mainDurabilityInfo;


    public bool weaponCrossingSurface;

    public bool showAllSettings;
    public bool showHUDSettings;
    public bool showWeaponSettings;
    public bool showProjectileSettings;
    public bool showAmmoSettings;
    public bool showEventsSettings;
    public bool showFireTypeSettings;
    public bool showSoundAndParticlesSettings;
    public bool showOtherSettings;

    GameObject newShellClone;
    GameObject currentShellToRemove;
    AudioElement newClipToShell = new AudioElement ();
    weaponShellSystem newWeaponShellSystem;

    RaycastHit hit;
    RaycastHit hitCamera;
    RaycastHit hitWeapon;
    float lastShoot;

    AudioSource currentWeaponEffectsSource;
    List<AudioSource> weaponEffectsSourceList = new List<AudioSource> ();

    bool currentWeaponEffectsSourceLocated;

    bool animationForwardPlayed;
    bool animationBackPlayed;
    Transform originalParent;

    bool shellCreated;

    bool shellsOnSceneToRemove;

    bool weaponHasAnimation;
    Vector3 forceDirection;
    GameObject closestEnemy;
    bool aimingHommingProjectile;

    List<GameObject> locatedEnemies = new List<GameObject> ();

    float weaponSpeed = 1;
    float originalWeaponSpeed;

    Vector3 aimedZone;
    bool objectiveFound;

    bool carryingWeaponPreviously;

    bool silencerActive;

    int originalClipSize;
    float originalFireRate;
    float originalProjectileDamage;

    bool originalInfiniteAmmo;


    bool originalAutomaticMode;
    bool orignalUseBurst;
    bool originalSpreadState;
    bool originalIsExplosiveState;
    bool originalDamageTargetOverTimeState;
    bool originalRemoveDamageTargetOverTimeState;
    bool originalSedateCharactersState;
    bool originalPushCharacterState;
    bool originalProjectileWithAbilityState;

    bool originalSliceObjectsDetected;

    bool originalUseRayCastShootValue;

    bool usingSight;

    bool usingSightFullBodyAwareness;

    Coroutine muzzleFlashCoroutine;
    GameObject newMuzzleParticlesClone;

    Rigidbody currentProjectileRigidbody;
    GameObject newProjectileGameObject;
    projectileInfo newProjectileInfo;

    Transform currentProjectilePosition;

    GameObject newClipToDrop;
    ignoreCollisionSystem newClipIgnoreCollisionSystem;

    bool checkReloadWeaponState;

    bool usingScreenSpaceCamera;
    bool targetOnScreen;
    Vector3 screenPoint;

    GameObject originalWeaponProjectile;

    bool shootingBurst;
    int currentBurstAmount;

    bool surfaceFound;
    RaycastHit surfaceFoundHit;

    bool usingDualWeapon;
    bool usingRightHandDualWeapon;

    int originalNumberKey = -1;

    bool pauseDrawKeepWeaponSounds;

    AudioSource currentAudioSource;
    bool createNewWeaponEffectSource;

    int projectilesInMagazine = -1;

    Coroutine reloadCoroutine;
    noiseMeshSystem noiseMeshManager;
    Coroutine shellCoroutine;

    bool noiseMeshManagerFound;

    float screenWidth;
    float screenHeight;

    public bool weaponObjectActive = true;

    float currentRaycastDistance;

    projectileSystem currentProjectileSystem;

    GameObject currentMagazine;

    float lastTimeAimWeapon;

    bool componentsInitialized;

    LayerMask currentLayerToCheck;

    bool projectileSpecialActionActive;

    bool projectileSpecialActionValue;

    int projectilePositionCount;

    AudioClip customShootSoundEffect;
    bool usingCustomShootSoundEffectActive;

    AudioClip customImpactSoundEffect;
    bool usingCustomImpactSoundEffectActive;


    void Awake ()
    {
        originalUseRayCastShootValue = weaponSettings.useRayCastShoot;
    }

    private void InitializeAudioElements ()
    {
        weaponSettings.InitializeAudioElements ();

        if (outOfAmmo != null) {
            outOfAmmoAudioElement.clip = outOfAmmo;
        }

        if (markTargetSound != null) {
            markTargetAudioElement.clip = markTargetSound;
        }
    }

    void Start ()
    {
        InitializeAudioElements ();

        initializeComponents ();
    }

    public void initializeComponents ()
    {
        if (componentsInitialized) {
            return;
        }

        gameObject.name += " (" + getWeaponSystemName () + ") ";
        currentWeaponEffectsSource = weaponEffectsSource;

        currentWeaponEffectsSourceLocated = currentWeaponEffectsSource != null;

        setAmmoValues ();

        originalParent = transform.parent;

        Transform weaponParent = weaponSettings.weaponParent;

        if (!weaponsManager.checkIfFirstPersonActiveExternally ()) {
            transform.SetParent (weaponParent);
        }

        GameObject keepPositionsParent = new GameObject ();

        Transform keepPositionsParentTransform = keepPositionsParent.transform;
        keepPositionsParentTransform.SetParent (weaponParent);
        keepPositionsParentTransform.name = getWeaponSystemName () + " Keep Positions Parent";
        keepPositionsParentTransform.localScale = Vector3.one;
        keepPositionsParentTransform.localPosition = Vector3.zero;
        keepPositionsParentTransform.localRotation = Quaternion.identity;

        IKWeaponManager.thirdPersonWeaponInfo.keepPosition.SetParent (keepPositionsParentTransform);

        IKWeaponManager.thirdPersonWeaponInfo.keepPosition.name = "Keep Position Single Weapon";

        if (IKWeaponManager.thirdPersonWeaponInfo.rightHandDualWeaopnInfo.keepPosition != null) {
            IKWeaponManager.thirdPersonWeaponInfo.rightHandDualWeaopnInfo.keepPosition.SetParent (keepPositionsParentTransform);

            IKWeaponManager.thirdPersonWeaponInfo.rightHandDualWeaopnInfo.keepPosition.name = "Keep Position Right Dual Weapon";
        }

        if (IKWeaponManager.thirdPersonWeaponInfo.leftHandDualWeaponInfo.keepPosition != null) {
            IKWeaponManager.thirdPersonWeaponInfo.leftHandDualWeaponInfo.keepPosition.SetParent (keepPositionsParentTransform);
            IKWeaponManager.thirdPersonWeaponInfo.leftHandDualWeaponInfo.keepPosition.name = "Keep Position Left Dual Weapon";
        }

        enableHUD (false);

        if (weaponSettings.useFireAnimation && weaponSettings.animation != "") {
            weaponHasAnimation = true;
            weaponAnimation [weaponSettings.animation].speed = weaponSettings.animationSpeed;
        } else {
            shellCreated = true;
        }

        if (weaponSettings.useReloadAnimation) {
            weaponHasAnimation = true;
        }

        originalWeaponSpeed = weaponSpeed;

        //get original values from the weapon
        originalFireRate = weaponSettings.fireRate;
        originalProjectileDamage = weaponSettings.projectileDamage;
        originalAutomaticMode = weaponSettings.automatic;
        orignalUseBurst = weaponSettings.useBurst;
        originalSpreadState = weaponSettings.useProjectileSpread;
        originalIsExplosiveState = weaponSettings.isExplosive;
        originalDamageTargetOverTimeState = weaponSettings.damageTargetOverTime;
        originalRemoveDamageTargetOverTimeState = weaponSettings.removeDamageOverTimeState;
        originalSedateCharactersState = weaponSettings.sedateCharacters;
        originalPushCharacterState = weaponSettings.pushCharacter;
        originalProjectileWithAbilityState = weaponSettings.projectileWithAbility;
        originalSliceObjectsDetected = weaponSettings.sliceObjectsDetected;

        originalInfiniteAmmo = weaponSettings.infiniteAmmo;

        usingScreenSpaceCamera = weaponsManager.isUsingScreenSpaceCamera ();

        originalWeaponProjectile = weaponProjectile;

        currentRaycastDistance = weaponSettings.maxRaycastDistance;

        if (weaponSettings.useInfiniteRaycastDistance) {
            currentRaycastDistance = Mathf.Infinity;
        }

        if (weaponsManager.useCustomLayerToCheck) {
            currentLayerToCheck = weaponsManager.customLayerToCheck;
        } else {
            currentLayerToCheck = layer;
        }

        projectilePositionCount = weaponSettings.projectilePosition.Count;

        componentsInitialized = true;
    }

    void OnEnable ()
    {
        weaponObjectActive = true;
    }

    void OnDisable ()
    {
        weaponObjectActive = false;

        weaponUpdateActive = false;

        //		print (getWeaponSystemName () + " disabled");
    }

    public bool currentWeapon;
    float lastTimeWeaponActive;

    public bool weaponUpdateActive;

    Coroutine updateWeaponCoroutine;

    public bool isWeaponUpdateActive ()
    {
        return weaponUpdateActive;
    }

    public void activateWeaponUpdate ()
    {
        if (!weaponObjectActive) {
            return;
        }

        stopActivateWeaponUpdate ();

        updateWeaponCoroutine = StartCoroutine (updateWeaponStateCoroutine ());

        weaponUpdateActive = true;
    }

    public void stopActivateWeaponUpdate ()
    {
        if (updateWeaponCoroutine != null) {
            StopCoroutine (updateWeaponCoroutine);
        }

        weaponUpdateActive = false;
    }

    IEnumerator updateWeaponStateCoroutine ()
    {
        var waitTime = new WaitForFixedUpdate ();

        while (true) {
            yield return waitTime;

            updateWeaponState ();

            if (!currentWeapon) {
                if (Time.time > lastTimeWeaponActive + 5) {
                    stopActivateWeaponUpdate ();
                }
            }
        }
    }

    void updateWeaponState ()
    {
        if (aimingInThirdPerson || carryingWeaponInFirstPerson) {
            if (weaponSettings.useShellOnWeaponShootEnabled) {
                if (!shellCreated && !weaponSettings.createShellsOnReload &&
                    ((weaponHasAnimation && animationForwardPlayed && !weaponAnimation.IsPlaying (weaponSettings.animation)) || !weaponHasAnimation || !weaponSettings.useFireAnimation)) {
                    createShells ();
                }
            }

            if (!reloading) {
                if (weaponHasAnimation) {
                    if (weaponSettings.useFireAnimation) {
                        if (weaponSettings.clipSize > 0 || !weaponSettings.weaponUsesAmmo) {
                            if (weaponSettings.playAnimationBackward) {
                                if (animationForwardPlayed && !weaponAnimation.IsPlaying (weaponSettings.animation)) {
                                    animationForwardPlayed = false;
                                    animationBackPlayed = true;

                                    weaponAnimation [weaponSettings.animation].speed = -weaponSettings.animationSpeed;
                                    weaponAnimation [weaponSettings.animation].time = weaponAnimation [weaponSettings.animation].length;
                                    weaponAnimation.Play (weaponSettings.animation);
                                }

                                if (animationBackPlayed && !weaponAnimation.IsPlaying (weaponSettings.animation)) {
                                    animationBackPlayed = false;
                                }
                            } else {
                                animationForwardPlayed = false;
                                animationBackPlayed = false;
                            }

                            checkReloadWeaponState = false;
                        } else {
                            checkReloadWeaponState = true;
                        }
                    } else {
                        checkReloadWeaponState = true;
                    }
                } else {
                    checkReloadWeaponState = true;
                }

                if (checkReloadWeaponState) {
                    if (weaponSettings.clipSize == 0 && weaponSettings.weaponUsesAmmo) {
                        if (weaponSettings.autoReloadWhenClipEmpty) {

                            if (weaponSettings.remainAmmo > 0 || weaponSettings.infiniteAmmo) {
                                if (!IKWeaponManager.isWeaponMoving () &&
                                    !IKWeaponManager.isAimingWeaponInProcess ()) {

                                    if (carryingWeaponInFirstPerson || (lastTimeAimWeapon > 0 && Time.time > lastTimeAimWeapon + 0.3f)) {

                                        reloadWeapon ();
                                    }

                                }
                            }
                        }
                    }
                }
            }

            if (aimingHommingProjectile) {
                //while the number of located enemies is lowers that the max enemies amount, then
                if (locatedEnemies.Count < projectilePositionCount) {
                    //uses a ray to detect enemies, to locked them

                    if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.forward, out hit, currentRaycastDistance, weaponsManager.targetToDamageLayer)) {
                        GameObject target = applyDamage.getCharacterOrVehicle (hit.collider.gameObject);

                        if (target != null && target != playerControllerGameObject) {
                            if (weaponSettings.tagToLocate.Contains (target.tag)) {
                                GameObject placeToShoot = applyDamage.getPlaceToShootGameObject (target);

                                if (!locatedEnemies.Contains (placeToShoot)) {
                                    //if an enemy is detected, add it to the list of located enemies and instantiated an icon in screen to follow the enemy
                                    locatedEnemies.Add (placeToShoot);

                                    weaponsManager.addElementToPlayerList (placeToShoot, false, false, 0, true, false,
                                        false, false, weaponSettings.locatedEnemyIconName, false, Color.white, true, -1, 0, false);
                                }
                            }
                        }
                    }
                }
            }

            if (shootingBurst) {
                if (Time.time > lastShoot + getFireRate ()) {
                    if ((!animationForwardPlayed && !animationBackPlayed && weaponHasAnimation) || !weaponHasAnimation || !weaponSettings.useFireAnimation) {
                        currentBurstAmount--;

                        if (currentBurstAmount == 0) {
                            shootWeapon (!weaponsManager.checkIfFirstPersonActiveExternally (), false, fullBodyAwarenessActive);

                            shootingBurst = false;
                        } else {
                            shootWeapon (!weaponsManager.checkIfFirstPersonActiveExternally (), true, fullBodyAwarenessActive);
                        }
                    }
                }
            }
        }

        checkDroppedShellsRemoval ();
    }

    public void setWeaponCarryStateAtOnce (bool state)
    {
        carryingWeaponInThirdPerson = state;
        carryingWeaponInFirstPerson = state;
    }

    public void setWeaponCarryState (bool thirdPersonCarry, bool firstPersonCarry)
    {
        carryingWeaponPreviously = carryingWeaponInThirdPerson || carryingWeaponInFirstPerson;

        carryingWeaponInThirdPerson = thirdPersonCarry;
        carryingWeaponInFirstPerson = firstPersonCarry;

        checkParableTrayectory (false, carryingWeaponInFirstPerson);

        checkWeaponAbility (false);

        //functions called when the player draws or keep the weapon in any view
        if (carryingWeaponInThirdPerson || carryingWeaponInFirstPerson) {
            if (weaponSettings.useStartDrawAction) {
                weaponSettings.startDrawAction.Invoke ();
            }
        } else {
            if (weaponSettings.useStopDrawAction) {
                weaponSettings.stopDrawAction.Invoke ();
            }
        }

        checkWeaponDrawEvents ();

        if (!pauseDrawKeepWeaponSounds && weaponSettings.useSoundOnDrawKeepWeapon) {
            if (carryingWeaponInThirdPerson || carryingWeaponInFirstPerson) {
                playSound (weaponSettings.drawWeaponAudioElement);
            } else {
                if (carryingWeaponPreviously) {
                    playSound (weaponSettings.keepWeaponAudioElement);
                }
            }
        }

        pauseDrawKeepWeaponSounds = false;

        if (IKWeaponManager.thirdPersonWeaponInfo.useNewIdleID) {
            if (carryingWeaponInThirdPerson || carryingWeaponInFirstPerson) {
                weaponsManager.setPlayerControllerCurrentIdleIDValue (IKWeaponManager.thirdPersonWeaponInfo.newIdleID);
            } else {
                if (carryingWeaponPreviously) {
                    weaponsManager.setPlayerControllerCurrentIdleIDValue (0);
                }
            }
        }
    }

    public void setPauseDrawKeepWeaponSound ()
    {
        pauseDrawKeepWeaponSounds = true;
    }

    public void setWeaponAimState (bool thirdPersonAim, bool firstPersonAim)
    {
        aimingInThirdPerson = thirdPersonAim;
        aimingInFirstPerson = firstPersonAim;

        if ((aimingInThirdPerson || aimingInFirstPerson) && weaponSettings.clipSize == 0 && weaponSettings.weaponUsesAmmo) {
            if (weaponSettings.autoReloadWhenClipEmpty && lastTimeAimWeapon > 0 && Time.time > lastTimeAimWeapon + 0.3f) {
                manualReload ();
            }
        }

        if (carryingWeaponInFirstPerson) {
            checkParableTrayectory (false, !aimingInFirstPerson);
        }

        if (carryingWeaponInThirdPerson) {
            checkParableTrayectory (aimingInThirdPerson, false);

            bool checkWeaponAbilityResult = true;

            if (fullBodyAwarenessActive) {
                if (weaponSettings.ignoreUpButtonActionOnFBA) {
                    if (!weaponsManager.isAimModeInputPressed ()) {
                        checkWeaponAbilityResult = false;
                    }
                }
            }

            if (checkWeaponAbilityResult) {
                checkWeaponAbility (false);
            }
        }

        if (!aimingInFirstPerson && !aimingInThirdPerson) {
            if (locatedEnemies.Count > 0) {
                aimingHommingProjectile = false;
                //remove the icons in the screen
                removeLocatedEnemiesIcons ();
            }
        }

        if (aimingInThirdPerson || aimingInFirstPerson) {
            lastTimeAimWeapon = Time.time;
        }

        checkWeaponAimEvents ();

        checkSniperSightUsedOnWeapon ();
    }

    public void checkWeaponDrawEvents ()
    {
        bool isFirstPersonActive = weaponsManager.checkIfFirstPersonActiveExternally ();

        bool isCarryingWeaponInThirdPerson = carryingWeaponInThirdPerson;

        bool isCarryingWeaponInFirstPerson = carryingWeaponInFirstPerson;

        if (fullBodyAwarenessActive) {
            isFirstPersonActive = true;

            isCarryingWeaponInFirstPerson = isCarryingWeaponInThirdPerson;
        }

        if (!isFirstPersonActive) {
            if (isCarryingWeaponInThirdPerson) {
                if (weaponSettings.useStartDrawActionThirdPerson) {
                    weaponSettings.startDrawActionThirdPerson.Invoke ();
                }
            } else {
                if (weaponSettings.useStopDrawActionThirdPerson) {
                    weaponSettings.stopDrawActionThirdPerson.Invoke ();
                }
            }
        }

        if (isFirstPersonActive) {
            if (isCarryingWeaponInFirstPerson) {
                if (weaponSettings.useStartDrawActionFirstPerson) {
                    weaponSettings.startDrawActionFirstPerson.Invoke ();
                }
            } else {
                if (weaponSettings.useStopDrawActionFirstPerson) {
                    weaponSettings.stopDrawActionFirstPerson.Invoke ();
                }
            }
        }
    }

    public void checkWeaponDrawEventsOnFullBodyAwarenessChangeState ()
    {
        if (fullBodyAwarenessActive) {
            if (weaponSettings.useStopDrawActionThirdPerson) {
                weaponSettings.stopDrawActionThirdPerson.Invoke ();
            }

            if (weaponSettings.useStartDrawActionFirstPerson) {
                weaponSettings.startDrawActionFirstPerson.Invoke ();
            }
        } else {
            if (weaponSettings.useStopDrawActionFirstPerson) {
                weaponSettings.stopDrawActionFirstPerson.Invoke ();
            }

            if (weaponSettings.useStartDrawActionThirdPerson) {
                weaponSettings.startDrawActionThirdPerson.Invoke ();
            }
        }
    }

    public void checkWeaponAimEvents ()
    {
        //functions called when the player aims or stop to aim the weapon in any view
        bool isFirstPersonActive = weaponsManager.checkIfFirstPersonActiveExternally ();

        bool isAimingInThirdPerson = aimingInThirdPerson;

        bool isAimingInFirstPerson = aimingInFirstPerson;

        if (fullBodyAwarenessActive) {
            isFirstPersonActive = true;

            isAimingInFirstPerson = isAimingInThirdPerson;
        }

        if (!isFirstPersonActive) {
            if (isAimingInThirdPerson) {
                if (weaponSettings.useStartAimActionThirdPerson) {
                    weaponSettings.startAimActionThirdPerson.Invoke ();
                }
            } else {
                if (weaponSettings.useStopAimActionThirdPerson) {
                    weaponSettings.stopAimActionThirdPerson.Invoke ();
                }
            }
        }

        if (isFirstPersonActive) {
            if (isAimingInFirstPerson) {
                if (weaponSettings.useStartAimActionFirstPerson) {
                    weaponSettings.startAimActionFirstPerson.Invoke ();
                }
            } else {
                if (weaponSettings.useStopAimActionFirstPerson) {
                    weaponSettings.stopAimActionFirstPerson.Invoke ();
                }
            }
        }
    }

    public void checkWeaponEventsAfterReload ()
    {
        if (fullBodyAwarenessActive) {
            checkWeaponDrawEvents ();
        } else {
            checkWeaponAimEvents ();
        }
    }

    bool ignoreSniperSightActive;
    public void setIgnoreSniperSightActiveState (bool state)
    {
        if (state) {
            checkSniperSightUsedOnWeapon ();
        }

        ignoreSniperSightActive = state;
    }

    public void checkSniperSightUsedOnWeapon ()
    {
        if (ignoreSniperSightActive) {
            return;
        }

        if (weaponsManager.checkIfFirstPersonActiveExternally ()) {
            if (weaponSettings.sniperSightFirstPersonEnabled) {
                IKWeaponManager.checkSniperSightUsedOnWeapon (aimingInFirstPerson);
            }
        } else {
            if (weaponSettings.sniperSightThirdPersonEnabled) {
                IKWeaponManager.checkSniperSightUsedOnWeapon (aimingInThirdPerson);
            }
        }
    }

    public bool fullBodyAwarenessActive;

    public void setFullBodyAwarenessActiveState (bool state)
    {
        fullBodyAwarenessActive = state;
    }

    public IKWeaponSystem getIKWeaponSystem ()
    {
        return IKWeaponManager;
    }

    public bool weaponIsMoving ()
    {
        return IKWeaponManager.isWeaponMoving ();
    }

    public bool isWeaponOnRecoil ()
    {
        return IKWeaponManager.isWeaponOnRecoil ();
    }

    public bool isAimingWeapon ()
    {
        return aimingInFirstPerson || aimingInThirdPerson;
    }

    public bool isAimingInThirdPerson ()
    {
        return aimingInThirdPerson;
    }

    public bool carryingWeapon ()
    {
        return carryingWeaponInFirstPerson || carryingWeaponInThirdPerson;
    }

    public bool isShootingBurst ()
    {
        return shootingBurst;
    }

    int lastAmountOfFiredProjectiles;

    public void resetLastAmountOfFiredProjectiles ()
    {
        lastAmountOfFiredProjectiles = 0;
    }

    public int getLastAmountOfFiredProjectiles ()
    {
        return lastAmountOfFiredProjectiles;
    }

    //fire the current weapon
    public void shootWeapon (bool isThirdPersonView, bool shootAtKeyDown, bool fullbodyAwarenessActive)
    {
        if (!IKWeaponManager.weaponCanFire ()) {
            return;
        }

        if (IKWeaponManager.isWeaponMoving ()) {
            return;
        }

        if (reloading || reloadingWithAnimationActive) {
            return;
        }

        checkWeaponAbility (shootAtKeyDown);

        //if the weapon system is active and the clip size higher than 0
        if (weaponSettings.clipSize > 0 || !weaponSettings.weaponUsesAmmo) {
            //else, fire the current weapon according to the fire rate
            if (Time.time > lastShoot + getFireRate ()) {

                //if the player fires a weapon, set the visible to AI state to true, this will change if the player is using a silencer
                if (weaponSettings.shootAProjectile || weaponSettings.launchProjectile) {
                    weaponsManager.setVisibleToAIState (true);
                }

                //If the current projectile is homing type, check when the shoot button is pressed and release
                if ((weaponSettings.isHommingProjectile && shootAtKeyDown)) {
                    aimingHommingProjectile = true;
                    //print ("1 "+ shootAtKeyDown + " " + locatedEnemiesIcons.Count + " " + aimingHommingProjectile);
                    return;
                }

                if ((weaponSettings.isHommingProjectile && !shootAtKeyDown && locatedEnemies.Count <= 0) ||
                    (!weaponSettings.isHommingProjectile && !shootAtKeyDown)) {
                    aimingHommingProjectile = false;
                    //print ("2 "+shootAtKeyDown + " " + locatedEnemiesIcons.Count + " " + aimingHommingProjectile);
                    return;
                }

                if (weaponHasAnimation && (animationForwardPlayed || animationBackPlayed) && weaponSettings.useFireAnimation) {
                    return;
                }

                if (weaponSettings.automatic && weaponSettings.useBurst) {
                    if (!shootingBurst && weaponSettings.burstAmount > 0) {
                        shootingBurst = true;
                        currentBurstAmount = weaponSettings.burstAmount;
                    }
                }

                //camera shake
                if (weaponsManager.useCameraShakeOnShootEnabled) {
                    checkWeaponCameraShake ();
                }

                //recoil
                IKWeaponManager.startRecoil (isThirdPersonView);

                IKWeaponManager.setLastTimeMoved ();

                if (usingDualWeapon) {
                    if (weaponSettings.shakeUpperBodyShootingDualWeapons) {
                        weaponsManager.checkDualWeaponShakeUpperBodyRotationCoroutine (weaponSettings.dualWeaponShakeAmount, weaponSettings.dualWeaponShakeSpeed, usingRightHandDualWeapon);
                    }
                } else {
                    if (weaponSettings.shakeUpperBodyWhileShooting) {
                        weaponsManager.checkShakeUpperBodyRotationCoroutine (weaponSettings.shakeAmount, weaponSettings.shakeSpeed);
                    }
                }

                //check shot camera noise
                IKWeaponManager.setShotCameraNoise ();

                checkWeaponShootNoise ();

                lastAmountOfFiredProjectiles++;

                //play the fire sound
                playWeaponSoundEffect (true);

                //enable the muzzle flash light
                enableMuzzleFlashLight ();

                Vector3 mainCameraPosition = mainCameraTransform.position;

                //create the muzzle flash
                createMuzzleFlash ();

                weaponCrossingSurface = false;

                if (!isThirdPersonView || fullbodyAwarenessActive) {
                    if (weaponSettings.checkCrossingSurfacesOnCameraDirection) {
                        if (projectilePositionCount > 0) {
                            float raycastDistance = currentRaycastDistance + 2;

                            if (fullbodyAwarenessActive) {
                                raycastDistance += 1;
                            }

                            Vector3 raycastDirection = mainCameraTransform.TransformDirection (Vector3.forward);

                            if (Physics.Raycast (mainCameraPosition, raycastDirection,
                                    out hitCamera, raycastDistance, currentLayerToCheck)) {

                                Vector3 raycastPosition = weaponSettings.projectilePosition [0].position;

                                if (Physics.Raycast (raycastPosition, raycastDirection,
                                        out hitWeapon, raycastDistance, currentLayerToCheck)) {

                                    if (hitCamera.collider != hitWeapon.collider) {
                                        //										print ("too close surface");

                                        weaponCrossingSurface = true;
                                    }
                                }
                            }
                        }
                    }
                }

                //play the fire animation
                if (weaponHasAnimation && weaponSettings.useFireAnimation) {
                    weaponAnimation [weaponSettings.animation].speed = weaponSettings.animationSpeed;
                    weaponAnimation.Play (weaponSettings.animation);
                    animationForwardPlayed = true;

                    if (weaponSettings.cockSoundAudioElement != null) {
                        playSound (weaponSettings.cockSoundAudioElement);
                    }
                }

                if (!weaponSettings.createShellsOnReload) {
                    shellCreated = false;
                }

                Vector3 currentNormal = weaponsManager.getCurrentNormal ();

                Vector3 cameraDirection = mainCameraTransform.TransformDirection (Vector3.forward);
                //every weapon can shoot 1 or more projectiles at the same time, so for every projectile position to instantiate

                for (int j = 0; j < projectilePositionCount; j++) {
                    currentProjectilePosition = weaponSettings.projectilePosition [j];

                    for (int l = 0; l < weaponSettings.projectilesPerShoot; l++) {

                        //set the info in the projectile, like the damage, the type of projectile, bullet or missile, etc...

                        surfaceFound = false;

                        //create the projectile
                        if (projectilesPoolEnabled) {
                            newProjectileGameObject = GKC_PoolingSystem.Spawn (weaponProjectile, currentProjectilePosition.position, mainCameraTransform.rotation, maxAmountOfPoolElementsOnWeapon);
                        } else {
                            newProjectileGameObject = (GameObject)Instantiate (weaponProjectile, currentProjectilePosition.position, mainCameraTransform.rotation);
                        }

                        currentProjectileSystem = newProjectileGameObject.GetComponent<projectileSystem> ();

                        currentProjectileSystem.checkToResetProjectile ();

                        if (projectileSpecialActionActive) {
                            currentProjectileSystem.setProjectileSpecialActionActiveState (projectileSpecialActionValue);
                        }

                        if (!weaponSettings.launchProjectile) {
                            //set its direction in the weapon forward or the camera forward according to if the weapon is aimed correctly or not
                            if (!weaponCrossingSurface) {
                                if (!weaponSettings.fireWeaponForward) {

                                    if (Physics.Raycast (mainCameraPosition, cameraDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {
                                        if (!ignoreTriggerOnRaycastEnabled || !hit.collider.isTrigger) {
                                            if (hit.collider != playerCollider) {
                                                //Debug.DrawLine (currentProjectilePosition.position, hit.point, Color.red, 2);

                                                newProjectileGameObject.transform.LookAt (hit.point);

                                                surfaceFound = true;
                                                surfaceFoundHit = hit;
                                            } else {
                                                if (Physics.Raycast (hit.point + 0.2f * cameraDirection, cameraDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {
                                                    newProjectileGameObject.transform.LookAt (hit.point);

                                                    surfaceFound = true;
                                                    surfaceFoundHit = hit;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (weaponSettings.launchProjectile) {
                            currentProjectileRigidbody = currentProjectileSystem.getProjectileRigibody ();

                            if (currentProjectileRigidbody == null) {
                                currentProjectileRigidbody = newProjectileGameObject.GetComponent<Rigidbody> ();
                            }

                            currentProjectileRigidbody.isKinematic = true;
                            // If the vehicle has a gravity control component, and the current gravity is not the regular one,
                            // add an artificial gravity component to the projectile like this, it can make a parable in any
                            // surface and direction, setting its gravity in the same of the vehicle.

                            if (weaponSettings.checkGravitySystemOnProjectile) {
                                if (currentNormal != Vector3.up) {
                                    artificialObjectGravity newArtificialObjectGravity = newProjectileGameObject.GetComponent<artificialObjectGravity> ();

                                    if (newArtificialObjectGravity == null) {
                                        newArtificialObjectGravity = newProjectileGameObject.AddComponent<artificialObjectGravity> ();
                                    }

                                    newArtificialObjectGravity.setCurrentGravity (-currentNormal);

                                    if (weaponsManager.isPlayerOnZeroGravityMode ()) {
                                        newArtificialObjectGravity.setZeroGravityActiveState (true);
                                    } else {
                                        if (weaponSettings.checkCircumnavigationValuesOnProjectile) {
                                            GKC_Utils.setGravityValueOnObjectFromPlayerValues (newArtificialObjectGravity, weaponsManager.getPlayerGameObject (), weaponSettings.gravityForceForCircumnavigationOnProjectile);
                                        }
                                    }
                                }
                            }

                            if (weaponSettings.useParableSpeed) {
                                //get the ray hit point where the projectile will fall
                                if (surfaceFound) {
                                    aimedZone = surfaceFoundHit.point;

                                    objectiveFound = true;
                                } else {
                                    if (Physics.Raycast (mainCameraPosition, cameraDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {
                                        if (hit.collider != playerCollider) {
                                            aimedZone = hit.point;

                                            objectiveFound = true;
                                        } else {
                                            if (Physics.Raycast (hit.point + 0.2f * cameraDirection, cameraDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {
                                                aimedZone = hit.point;

                                                objectiveFound = true;
                                            }
                                        }
                                    } else {
                                        objectiveFound = false;
                                    }
                                }

                                //								Debug.DrawLine (newProjectileGameObject.transform.position, aimedZone, Color.gray, 5);
                            }

                            launchCurrentProjectile (newProjectileGameObject, currentProjectileRigidbody, cameraDirection);
                        } else {
                            currentProjectileRigidbody = currentProjectileSystem.getProjectileRigibody ();

                            if (currentProjectileRigidbody == null) {
                                currentProjectileRigidbody = newProjectileGameObject.GetComponent<Rigidbody> ();
                            }

                            if (weaponSettings.checkGravitySystemOnProjectile) {
                                //								print (currentNormal != Vector3.up);

                                if (currentNormal != Vector3.up) {
                                    artificialObjectGravity newArtificialObjectGravity = newProjectileGameObject.GetComponent<artificialObjectGravity> ();

                                    if (newArtificialObjectGravity == null) {
                                        newArtificialObjectGravity = newProjectileGameObject.AddComponent<artificialObjectGravity> ();
                                    }

                                    newArtificialObjectGravity.setCurrentGravity (-currentNormal);

                                    if (weaponsManager.isPlayerOnZeroGravityMode ()) {
                                        newArtificialObjectGravity.setZeroGravityActiveState (true);
                                    } else {
                                        if (weaponSettings.checkCircumnavigationValuesOnProjectile) {
                                            GKC_Utils.setGravityValueOnObjectFromPlayerValues (newArtificialObjectGravity, weaponsManager.getPlayerGameObject (), weaponSettings.gravityForceForCircumnavigationOnProjectile);
                                        }
                                    }
                                }
                            }
                        }

                        //add spread to the projectile
                        Vector3 spreadAmount = Vector3.zero;

                        if (weaponSettings.useProjectileSpread) {
                            spreadAmount = setProjectileSpread ();

                            float extraSpreadAmount = weaponsManager.getSpreadMultiplierStat ();

                            if (extraSpreadAmount > 0) {
                                spreadAmount = extraSpreadAmount * spreadAmount;
                            }

                            float externalSpreadMultiplier = weaponsManager.getExternalSpreadMultiplierValue ();

                            if (externalSpreadMultiplier != 0) {
                                spreadAmount = externalSpreadMultiplier * spreadAmount;
                            }

                            if (spreadAmount != Vector3.zero) {
                                newProjectileGameObject.transform.Rotate (spreadAmount);
                            }
                        }

                        if (weaponSettings.setProjectileMeshRotationToFireRotation) {
                            currentProjectileSystem.setProjectileLocalRotation (currentProjectilePosition.localEulerAngles);
                        }

                        setProjectileInfo ();

                        currentProjectileSystem.setProjectileInfo (newProjectileInfo);

                        if (weaponSettings.isSeeker) {
                            closestEnemy = setSeekerProjectileInfo (currentProjectilePosition.position);

                            if (closestEnemy != null) {
                                currentProjectileSystem.setEnemy (closestEnemy);
                            }
                        }

                        //if the homing projectiles are being using, then
                        if (aimingHommingProjectile) {
                            //if the button to shoot is released, shoot a homing projectile for every located enemy
                            //check that the located enemies are higher that 0
                            if (locatedEnemies.Count > 0) {
                                //shoot the missiles
                                if (j < locatedEnemies.Count) {
                                    currentProjectileSystem.setEnemy (locatedEnemies [j]);
                                }
                            }
                        }

                        bool projectileFiredByRaycast = false;

                        bool projectileDestroyed = false;

                        //if the weapon shoots setting directly the projectile in the hit point, place the current projectile in the hit point position
                        if (weaponSettings.useRayCastShoot || weaponCrossingSurface) {
                            Vector3 forwardDirection = cameraDirection;
                            Vector3 forwardPosition = mainCameraPosition;

                            if (weaponSettings.fireWeaponForward && !weaponCrossingSurface) {
                                forwardDirection = transform.forward;
                                forwardPosition = currentProjectilePosition.position;
                            }

                            if (weaponSettings.checkIfSurfacesCloseToWeapon && carryingWeaponInThirdPerson && !weaponsManager.usedByAI) {
                                Vector3 raycastPosition = currentProjectilePosition.position - 0.4f * currentProjectilePosition.forward;

                                if (fullbodyAwarenessActive) {
                                    raycastPosition = currentProjectilePosition.position - currentProjectilePosition.forward;
                                }

                                if (Physics.Raycast (raycastPosition, cameraDirection,
                                        out hit, weaponSettings.minDistanceToCheck, currentLayerToCheck)) {

                                    forwardDirection = transform.forward;

                                    if (fullbodyAwarenessActive) {
                                        forwardPosition = raycastPosition;
                                    } else {
                                        forwardPosition = currentProjectilePosition.position;
                                    }
                                }
                            }

                            if (spreadAmount.magnitude != 0) {
                                forwardDirection = Quaternion.Euler (spreadAmount) * forwardDirection;
                            }

                            if (Physics.Raycast (forwardPosition, forwardDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {
                                if (hit.collider != playerCollider) {
                                    if (weaponSettings.useFakeProjectileTrails) {
                                        currentProjectileSystem.creatFakeProjectileTrail (hit.point);
                                    }

                                    currentProjectileSystem.rayCastShoot (hit.collider, hit.point, forwardDirection);

                                    projectileFiredByRaycast = true;

                                    //									print (hit.collider.name);
                                } else {
                                    if (Physics.Raycast (hit.point + 0.2f * forwardDirection, forwardDirection, out hit, currentRaycastDistance, currentLayerToCheck)) {

                                        if (weaponSettings.useFakeProjectileTrails) {
                                            currentProjectileSystem.creatFakeProjectileTrail (hit.point);
                                        }

                                        currentProjectileSystem.rayCastShoot (hit.collider, hit.point, forwardDirection);

                                        projectileFiredByRaycast = true;

                                        //										print (hit.collider.name);
                                    } else {
                                        currentProjectileSystem.initializeProjectile ();

                                        if (weaponSettings.useFakeProjectileTrails) {
                                            currentProjectileSystem.creatFakeProjectileTrail (forwardPosition + 50 * forwardDirection);
                                            currentProjectileSystem.setFakeProjectileTrailSpeedMultiplier (0.3f);
                                            currentProjectileSystem.setDestroyTrailAfterTimeState (true);
                                        }

                                        currentProjectileSystem.destroyProjectile ();

                                        projectileDestroyed = true;

                                        //										print ("previous was player, but not next found");
                                    }
                                }
                                //print ("same object fired: " + hit.collider.name);
                            } else {
                                currentProjectileSystem.initializeProjectile ();

                                if (weaponSettings.useFakeProjectileTrails) {
                                    currentProjectileSystem.creatFakeProjectileTrail (forwardPosition + 50 * forwardDirection);
                                    currentProjectileSystem.setFakeProjectileTrailSpeedMultiplier (0.3f);
                                    currentProjectileSystem.setDestroyTrailAfterTimeState (true);
                                }

                                currentProjectileSystem.destroyProjectile ();

                                projectileDestroyed = true;
                            }
                        }

                        if (!projectileFiredByRaycast && !projectileDestroyed) {
                            currentProjectileSystem.initializeProjectile ();
                        }
                    }

                    useAmmo ();

                    lastShoot = Time.time;
                    destroyShellsTimer = 0;
                }

                if (weaponSettings.weaponWithAbility) {
                    lastShoot = Time.time;
                    destroyShellsTimer = 0;

                    if (weaponSettings.useEventOnFireWeapon) {
                        weaponSettings.eventOnFireWeapon.Invoke ();
                    }
                }

                if (weaponSettings.applyForceAtShoot) {
                    forceDirection = (weaponSettings.forceDirection.x * mainCameraTransform.right +
                    weaponSettings.forceDirection.y * mainCameraTransform.up +
                    weaponSettings.forceAmount * weaponSettings.forceDirection.z * mainCameraTransform.forward);

                    weaponsManager.externalForce (forceDirection);
                }

                if (weaponSettings.isHommingProjectile && !shootAtKeyDown && aimingHommingProjectile) {
                    //if the button to shoot is released, shoot a homing projectile for every located enemy
                    //check that the located enemies are higher that 0
                    if (locatedEnemies.Count > 0) {
                        //remove the icons in the screen
                        removeLocatedEnemiesIcons ();
                    }

                    aimingHommingProjectile = false;
                }

                if (useObjectDurability) {
                    checkDurabilityOnAttack ();
                }
            }
        } else {
            //else, the clip in the weapon is over, so check if there is remaining ammo
            if (weaponSettings.remainAmmo == 0 && !weaponSettings.infiniteAmmo && shootAtKeyDown) {

                weaponsManager.setWeaponRemainAmmoFromInventory (this);

                if (weaponSettings.remainAmmo == 0) {
                    playWeaponSoundEffect (false);

                    if (!weaponsManager.isPauseWeaponReloadActive ()) {
                        //check if player changes automatically to next weapon with ammo
                        if (weaponsManager.changeToNextWeaponIfAmmoEmpty) {
                            weaponsManager.changeToNextWeaponWithAmmo ();
                        } else {
                            weaponsManager.checkWeaponStateOnEmptyAmmo ();
                        }
                    }
                }
            }
        }
    }

    public void checkWeaponCameraShake ()
    {
        weaponsManager.checkWeaponCameraShake (usingRightHandDualWeapon);

        //		if (IKWeaponManager.sameValueBothViews) {
        //			weaponsManager.setHeadBobShotShakeState (IKWeaponManager.thirdPersonshotShakeInfo);
        //		} else {
        //			if (carryingWeaponInFirstPerson && IKWeaponManager.useShotShakeInFirstPerson) {
        //				weaponsManager.setHeadBobShotShakeState (IKWeaponManager.firstPersonshotShakeInfo);
        //
        //				return;
        //			}
        //
        //			if (carryingWeaponInThirdPerson && IKWeaponManager.useShotShakeInThirdPerson) {
        //				weaponsManager.setHeadBobShotShakeState (IKWeaponManager.thirdPersonshotShakeInfo);
        //			}
        //		}
    }

    public void checkMeleeAttackShakeInfo ()
    {
        if (carryingWeaponInFirstPerson && IKWeaponManager.firstPersonWeaponInfo.useMeleeAttackShakeInfo) {
            weaponsManager.setHeadBobShotShakeState (IKWeaponManager.firstPersonMeleeAttackShakeInfo);

            return;
        }

        if (carryingWeaponInThirdPerson && IKWeaponManager.thirdPersonWeaponInfo.useMeleeAttackShakeInfo) {
            weaponsManager.setHeadBobShotShakeState (IKWeaponManager.thirdPersonMeleeAttackShakeInfo);
        }
    }

    public void checkWeaponAbility (bool keyDown)
    {
        if (weaponSettings.weaponWithAbility) {
            if (keyDown) {
                if (weaponSettings.useDownButton) {
                    weaponSettings.downButtonAction.Invoke ();
                }
            } else {
                if (weaponSettings.useUpButton) {
                    bool callUpButtonActionEventResult = true;

                    if (fullBodyAwarenessActive) {
                        if (weaponSettings.ignoreUpButtonActionOnFBA) {
                            if (!weaponsManager.isUsingFreeFireMode ()) {
                                callUpButtonActionEventResult = false;
                            }
                        }
                    }

                    if (callUpButtonActionEventResult) {
                        weaponSettings.upButtonAction.Invoke ();
                    }
                }
            }
        }
    }

    public void checkWeaponAbilityHoldButton ()
    {
        if (weaponSettings.weaponWithAbility) {
            if (weaponSettings.useHoldButton) {
                weaponSettings.holdButtonAction.Invoke ();
            }
        }
    }

    public bool canUseSecondaryAction ()
    {
        if (carryingWeaponInFirstPerson) {
            return true;
        }

        if (!weaponSettings.useSecondaryActionsOnlyAimingThirdPerson) {
            return true;
        }

        if (aimingInThirdPerson) {
            return true;
        }

        if (fullBodyAwarenessActive && weaponSettings.allowUseSecondaryActionOnFBA) {
            return true;
        }

        return false;
    }

    public void activateSecondaryAction ()
    {
        if (!canUseSecondaryAction ()) {
            return;
        }

        if (weaponSettings.useSecondaryAction) {
            weaponSettings.secondaryAction.Invoke ();
        }
    }

    public void activateSecondaryActionOnDownPress ()
    {
        if (!canUseSecondaryAction ()) {
            return;
        }

        if (weaponSettings.useSecondaryActionOnDownPress) {
            weaponSettings.secondaryActionOnDownPress.Invoke ();
        }
    }

    public void activateSecondaryActionOnUpPress ()
    {
        if (!canUseSecondaryAction ()) {
            return;
        }

        if (weaponSettings.useSecondaryActionOnUpPress) {
            weaponSettings.secondaryActionOnUpPress.Invoke ();
        }
    }

    public void activateSecondaryActionOnUpHold ()
    {
        if (!canUseSecondaryAction ()) {
            return;
        }

        if (weaponSettings.useSecondaryActionOnHoldPress) {
            weaponSettings.secondaryActionOnHoldPress.Invoke ();
        }
    }

    public void activateForwardAcion ()
    {
        if (weaponSettings.useForwardActionEvent) {
            weaponSettings.forwardActionEvent.Invoke ();

            IKWeaponManager.setLastTimeMoved ();
        }
    }

    public void activateBackwardAcion ()
    {
        if (weaponSettings.useBackwardActionEvent) {
            weaponSettings.backwardActionEvent.Invoke ();

            IKWeaponManager.setLastTimeMoved ();
        }
    }

    public void checkEventsOnWeaponActivatedOrDeactivated (bool state)
    {
        if (state) {
            if (weaponSettings.useEventOnWeaponActivated) {
                weaponSettings.eventOnWeaponActivated.Invoke ();
            }
        } else {
            if (weaponSettings.useEventOnWeaponDeactivated) {
                weaponSettings.eventOnWeaponDeactivated.Invoke ();
            }
        }
    }

    // Remove the located enemies icons
    public void removeLocatedEnemiesIcons ()
    {
        int locatedEnemiesCount = locatedEnemies.Count;

        for (int i = 0; i < locatedEnemiesCount; i++) {
            weaponsManager.removeLocatedEnemiesIcons (locatedEnemies [i]);
        }

        locatedEnemies.Clear ();
    }

    bool newProjectileInfoCreated;

    bool projectileInfoAssigned;

    void setProjectileInfo ()
    {
        if (!newProjectileInfoCreated) {
            newProjectileInfo = new projectileInfo ();

            newProjectileInfoCreated = true;
        }

        newProjectileInfo.forwardDirection = mainCameraTransform.forward;

        if (!projectileInfoAssigned) {
            newProjectileInfo.isHommingProjectile = weaponSettings.isHommingProjectile;

            newProjectileInfo.isSeeker = weaponSettings.isSeeker;
            newProjectileInfo.targetOnScreenForSeeker = weaponSettings.targetOnScreenForSeeker;

            newProjectileInfo.waitTimeToSearchTarget = weaponSettings.waitTimeToSearchTarget;
            newProjectileInfo.useRayCastShoot = weaponSettings.useRayCastShoot;

            newProjectileInfo.useRaycastShootDelay = weaponSettings.useRaycastShootDelay;
            newProjectileInfo.raycastShootDelay = weaponSettings.raycastShootDelay;
            newProjectileInfo.getDelayWithDistance = weaponSettings.getDelayWithDistance;
            newProjectileInfo.delayWithDistanceSpeed = weaponSettings.delayWithDistanceSpeed;
            newProjectileInfo.maxDelayWithDistance = weaponSettings.maxDelayWithDistance;

            newProjectileInfo.useFakeProjectileTrails = weaponSettings.useFakeProjectileTrails;

            newProjectileInfo.useRaycastCheckingOnRigidbody = weaponSettings.useRaycastCheckingOnRigidbody;
            newProjectileInfo.customRaycastCheckingRate = weaponSettings.customRaycastCheckingRate;
            newProjectileInfo.customRaycastCheckingDistance = weaponSettings.customRaycastCheckingDistance;

            newProjectileInfo.projectileDamage = (weaponSettings.projectileDamage + weaponsManager.getExtraDamageStat ()) * weaponsManager.getDamageMultiplierStat ();
            newProjectileInfo.projectileSpeed = weaponSettings.projectileSpeed;

            newProjectileInfo.impactForceApplied = weaponSettings.impactForceApplied;
            newProjectileInfo.forceMode = weaponSettings.forceMode;
            newProjectileInfo.applyImpactForceToVehicles = weaponSettings.applyImpactForceToVehicles;
            newProjectileInfo.impactForceToVehiclesMultiplier = weaponSettings.impactForceToVehiclesMultiplier;

            newProjectileInfo.projectileWithAbility = weaponSettings.projectileWithAbility;

            if (usingCustomShootSoundEffectActive) {
                newProjectileInfo.impactSoundEffect = customShootSoundEffect;
            } else {
                newProjectileInfo.impactSoundEffect = weaponSettings.impactSoundEffect;
            }

            newProjectileInfo.impactAudioElement = weaponSettings.impactAudioElement;

            newProjectileInfo.scorch = weaponSettings.scorch;
            newProjectileInfo.scorchRayCastDistance = weaponSettings.scorchRayCastDistance;

            newProjectileInfo.owner = playerControllerGameObject;

            newProjectileInfo.projectileParticles = weaponSettings.projectileParticles;
            newProjectileInfo.impactParticles = weaponSettings.impactParticles;

            newProjectileInfo.isExplosive = weaponSettings.isExplosive;
            newProjectileInfo.isImplosive = weaponSettings.isImplosive;
            newProjectileInfo.useExplosionDelay = weaponSettings.useExplosionDelay;
            newProjectileInfo.explosionDelay = weaponSettings.explosionDelay;
            newProjectileInfo.explosionForce = weaponSettings.explosionForce;
            newProjectileInfo.explosionRadius = weaponSettings.explosionRadius;
            newProjectileInfo.explosionDamage = weaponSettings.explosionDamage;
            newProjectileInfo.pushCharacters = weaponSettings.pushCharacters;
            newProjectileInfo.canDamageProjectileOwner = weaponSettings.canDamageProjectileOwner;
            newProjectileInfo.applyExplosionForceToVehicles = weaponSettings.applyExplosionForceToVehicles;
            newProjectileInfo.explosionForceToVehiclesMultiplier = weaponSettings.explosionForceToVehiclesMultiplier;

            newProjectileInfo.searchClosestWeakSpot = weaponSettings.searchClosestWeakSpot;

            newProjectileInfo.killInOneShot = weaponSettings.killInOneShot;

            newProjectileInfo.useDisableTimer = weaponSettings.useDisableTimer;
            newProjectileInfo.noImpactDisableTimer = weaponSettings.noImpactDisableTimer;
            newProjectileInfo.impactDisableTimer = weaponSettings.impactDisableTimer;

            newProjectileInfo.targetToDamageLayer = weaponsManager.targetToDamageLayer;
            newProjectileInfo.targetForScorchLayer = weaponsManager.targetForScorchLayer;

            newProjectileInfo.useCustomIgnoreTags = weaponsManager.useCustomIgnoreTags;
            newProjectileInfo.customTagsToIgnoreList = weaponsManager.customTagsToIgnoreList;

            newProjectileInfo.impactDecalIndex = impactDecalIndex;

            newProjectileInfo.launchProjectile = weaponSettings.launchProjectile;

            newProjectileInfo.adhereToSurface = weaponSettings.adhereToSurface;
            newProjectileInfo.adhereToLimbs = weaponSettings.adhereToLimbs;
            newProjectileInfo.ignoreSetProjectilePositionOnImpact = weaponSettings.ignoreSetProjectilePositionOnImpact;

            newProjectileInfo.useGravityOnLaunch = weaponSettings.useGravityOnLaunch;
            newProjectileInfo.useGraivtyOnImpact = weaponSettings.useGraivtyOnImpact;

            if (weaponSettings.breakThroughObjects) {
                newProjectileInfo.breakThroughObjects = weaponSettings.breakThroughObjects;
                newProjectileInfo.infiniteNumberOfImpacts = weaponSettings.infiniteNumberOfImpacts;
                newProjectileInfo.numberOfImpacts = weaponSettings.numberOfImpacts;
                newProjectileInfo.canDamageSameObjectMultipleTimes = weaponSettings.canDamageSameObjectMultipleTimes;

                newProjectileInfo.stopBreakThroughObjectsOnLayer = weaponSettings.stopBreakThroughObjectsOnLayer;
                newProjectileInfo.layerToStopBreahtThroughObjects = weaponSettings.layerToStopBreahtThroughObjects;
                newProjectileInfo.ignoreNewRotationOnProjectileImpact = weaponSettings.ignoreNewRotationOnProjectileImpact;

                newProjectileInfo.canBreakThroughArmorSurface = weaponSettings.canBreakThroughArmorSurface;
                newProjectileInfo.breakThroughArmorSurfacePriorityValue = weaponSettings.breakThroughArmorSurfacePriorityValue;
            }

            if (weaponSettings.damageTargetOverTime) {
                newProjectileInfo.damageTargetOverTime = weaponSettings.damageTargetOverTime;
                newProjectileInfo.damageOverTimeDelay = weaponSettings.damageOverTimeDelay;
                newProjectileInfo.damageOverTimeDuration = weaponSettings.damageOverTimeDuration;
                newProjectileInfo.damageOverTimeAmount = weaponSettings.damageOverTimeAmount;
                newProjectileInfo.damageOverTimeRate = weaponSettings.damageOverTimeRate;
                newProjectileInfo.damageOverTimeToDeath = weaponSettings.damageOverTimeToDeath;
                newProjectileInfo.removeDamageOverTimeState = weaponSettings.removeDamageOverTimeState;
            }

            if (weaponSettings.sedateCharacters) {
                newProjectileInfo.sedateCharacters = weaponSettings.sedateCharacters;
                newProjectileInfo.sedateDelay = weaponSettings.sedateDelay;
                newProjectileInfo.useWeakSpotToReduceDelay = weaponSettings.useWeakSpotToReduceDelay;
                newProjectileInfo.sedateDuration = weaponSettings.sedateDuration;
                newProjectileInfo.sedateUntilReceiveDamage = weaponSettings.sedateUntilReceiveDamage;
            }

            if (weaponSettings.pushCharacter) {
                newProjectileInfo.pushCharacter = weaponSettings.pushCharacter;
                newProjectileInfo.pushCharacterForce = weaponSettings.pushCharacterForce;
                newProjectileInfo.pushCharacterRagdollForce = weaponSettings.pushCharacterRagdollForce;
            }

            newProjectileInfo.setProjectileMeshRotationToFireRotation = weaponSettings.setProjectileMeshRotationToFireRotation;

            if (useRemoteEventOnObjectsFound) {
                newProjectileInfo.useRemoteEventOnObjectsFound = useRemoteEventOnObjectsFound;
                newProjectileInfo.remoteEventNameList = remoteEventNameList;
            }

            if (useRemoteEventOnObjectsFoundOnExplosion) {
                newProjectileInfo.useRemoteEventOnObjectsFoundOnExplosion = useRemoteEventOnObjectsFoundOnExplosion;
                newProjectileInfo.remoteEventNameOnExplosion = remoteEventNameOnExplosion;
            }

            if (weaponSettings.ignoreShield) {
                newProjectileInfo.ignoreShield = weaponSettings.ignoreShield;
                newProjectileInfo.canActivateReactionSystemTemporally = weaponSettings.canActivateReactionSystemTemporally;
                newProjectileInfo.damageReactionID = weaponSettings.damageReactionID;
            }

            newProjectileInfo.damageTypeID = weaponSettings.damageTypeID;

            newProjectileInfo.damageCanBeBlocked = weaponSettings.damageCanBeBlocked;

            newProjectileInfo.projectilesPoolEnabled = projectilesPoolEnabled;

            newProjectileInfo.maxAmountOfPoolElementsOnWeapon = maxAmountOfPoolElementsOnWeapon;

            newProjectileInfo.allowDamageForProjectileOwner = weaponSettings.allowDamageForProjectileOwner;

            newProjectileInfo.projectileCanBeDeflected = weaponSettings.projectileCanBeDeflected;

            if (weaponSettings.sliceObjectsDetected) {
                newProjectileInfo.sliceObjectsDetected = weaponSettings.sliceObjectsDetected;
                newProjectileInfo.layerToSlice = weaponSettings.layerToSlice;
                newProjectileInfo.useBodyPartsSliceList = weaponSettings.useBodyPartsSliceList;
                newProjectileInfo.bodyPartsSliceList = weaponSettings.bodyPartsSliceList;
                newProjectileInfo.maxDistanceToBodyPart = weaponSettings.maxDistanceToBodyPart;
                newProjectileInfo.randomSliceDirection = weaponSettings.randomSliceDirection;

                newProjectileInfo.showSliceGizmo = weaponSettings.showSliceGizmo;

                newProjectileInfo.activateRigidbodiesOnNewObjects = weaponSettings.activateRigidbodiesOnNewObjects;

                newProjectileInfo.useGeneralProbabilitySliceObjects = weaponSettings.useGeneralProbabilitySliceObjects;
                newProjectileInfo.generalProbabilitySliceObjects = weaponSettings.generalProbabilitySliceObjects;
            }

            projectileInfoAssigned = true;
        }
    }

    public void updateProjectileInfoValues ()
    {
        projectileInfoAssigned = false;

        setProjectileInfo ();
    }

    public void setProjectileInfoAssignedValue (bool state)
    {
        projectileInfoAssigned = state;
    }

    public GameObject setSeekerProjectileInfo (Vector3 shootPosition)
    {
        if (mainCamera == null) {
            mainCamera = weaponsManager.getMainCamera ();
        }

        return applyDamage.setSeekerProjectileInfo (shootPosition, weaponSettings.tagToLocate, usingScreenSpaceCamera,
            weaponSettings.targetOnScreenForSeeker, mainCamera, weaponsManager.targetToDamageLayer, transform.position, false, null);
    }

    public Vector3 setProjectileSpread ()
    {
        float spreadAmount = 0;

        if (carryingWeaponInFirstPerson) {
            if (aimingInFirstPerson) {
                spreadAmount = weaponSettings.firstPersonSpreadAmountAiming;
            } else {
                spreadAmount = weaponSettings.firstPersonSpreadAmountNoAiming;
            }
        }

        if (carryingWeaponInThirdPerson) {
            if (fullBodyAwarenessActive) {
                if (weaponsManager.isUsingFreeFireMode ()) {
                    spreadAmount = weaponSettings.firstPersonSpreadAmountNoAiming;
                } else {
                    spreadAmount = weaponSettings.firstPersonSpreadAmountAiming;
                }
            } else {
                if (weaponsManager.isUsingFreeFireMode ()) {
                    spreadAmount = weaponSettings.thirdPersonSpreadAmountNoAiming;
                } else {
                    spreadAmount = weaponSettings.thirdPersonSpreadAmountAiming;
                }
            }
        }

        if (spreadAmount > 0) {
            Vector3 randomSpread = Vector3.zero;
            randomSpread.x = Random.Range (-spreadAmount, spreadAmount);
            randomSpread.y = Random.Range (-spreadAmount, spreadAmount);
            randomSpread.z = Random.Range (-spreadAmount, spreadAmount);

            return randomSpread;
        }

        return Vector3.zero;
    }

    public void createShells ()
    {
        shellCreated = true;

        if (weaponSettings.useShellDelay) {
            createShellsWithDelay ();
        } else {
            createShellsAtOnce ();
        }
    }

    public void createShellsWithDelay ()
    {
        if (shellCoroutine != null) {
            StopCoroutine (shellCoroutine);
        }

        shellCoroutine = StartCoroutine (createShellsWithDelayCoroutine ());
    }

    //a delay for reload the weapon
    IEnumerator createShellsWithDelayCoroutine ()
    {
        bool firstPersonActive = weaponsManager.checkIfFirstPersonActiveExternally ();

        if (firstPersonActive) {
            WaitForSeconds delay = new WaitForSeconds (weaponSettings.shellDelayFirsPerson);

            yield return delay;
        } else {
            WaitForSeconds delay = new WaitForSeconds (weaponSettings.shellDelayThirdPerson);

            yield return delay;
        }

        createShellsAtOnce ();
    }

    bool weaponShellLocated;

    void createShellsAtOnce ()
    {
        if (weaponSettings.checkToCreateShellsIfNoRemainAmmo && !hasAnyAmmo ()) {
            return;
        }

        if (!weaponShellLocated) {
            weaponShellLocated = weaponSettings.shell != null;
        }

        if (weaponShellLocated) {
            int shellPositionCount = weaponSettings.shellPosition.Count;

            for (int j = 0; j < shellPositionCount; j++) {
                //if the current weapon drops shells, create them

                Transform currentShellPosition = weaponSettings.shellPosition [j];

                if (projectilesPoolEnabled) {
                    newShellClone = GKC_PoolingSystem.Spawn (weaponSettings.shell, currentShellPosition.position, currentShellPosition.rotation, maxAmountOfPoolElementsOnWeapon);
                } else {
                    newShellClone = (GameObject)Instantiate (weaponSettings.shell, currentShellPosition.position, currentShellPosition.rotation);
                }

                newWeaponShellSystem = newShellClone.GetComponent<weaponShellSystem> ();

                if (weaponSettings.shellDropAudioElements.Count > 0) {
                    newClipToShell = weaponSettings.shellDropAudioElements [Random.Range (0, weaponSettings.shellDropAudioElements.Count - 1)];
                }

                if (newWeaponShellSystem != null) {
                    newWeaponShellSystem.setShellValues ((weaponSettings.shellEjectionForce * GKC_Utils.getCurrentScaleTime ()) * currentShellPosition.right, playerCollider, newClipToShell);

                    if (FBAWeaponColliderAssigned) {
                        newWeaponShellSystem.setExtraColliderToIgnore (FBAWeaponCollider);
                    }

                    if (headColliderOnFBAAssigned) {
                        newWeaponShellSystem.setExtraColliderToIgnore (headColliderOnFBA);
                    }
                }

                if (weaponSettings.removeDroppedShellsAfterTime) {
                    shells.Add (newShellClone);

                    if (shells.Count > weaponSettings.maxAmountOfShellsBeforeRemoveThem) {
                        currentShellToRemove = shells [0];

                        shells.RemoveAt (0);

                        if (projectilesPoolEnabled) {
                            GKC_PoolingSystem.Despawn (currentShellToRemove);
                        } else {
                            Destroy (currentShellToRemove);
                        }
                    }

                    shellsActive = true;

                    shellsOnSceneToRemove = true;
                }
            }
        }
    }

    public void checkDroppedShellsRemoval ()
    {
        //if the amount of shells from the projectiles is higher than 0, check the time to remove then
        if (shellsOnSceneToRemove && weaponSettings.removeDroppedShellsAfterTime) {
            if (shellsActive) {
                destroyShellsTimer += Time.deltaTime;

                if (destroyShellsTimer > 3) {
                    int shellsCount = shells.Count;

                    for (int i = 0; i < shellsCount; i++) {
                        if (projectilesPoolEnabled) {
                            GKC_PoolingSystem.Despawn (shells [i]);
                        } else {
                            Destroy (shells [i]);
                        }
                    }

                    shells.Clear ();

                    destroyShellsTimer = 0;

                    shellsOnSceneToRemove = false;

                    shellsActive = false;
                }
            }
        }
    }

    bool shootAudioElementLocated;

    AudioElement shootAudioElementToPlay;

    //play the fire sound or the empty clip sound
    void playWeaponSoundEffect (bool hasAmmo)
    {
        if (weaponSettings.useSoundsPool) {
            currentWeaponEffectsSource = getAudioSourceFromPool ();

            currentWeaponEffectsSourceLocated = currentWeaponEffectsSource != null;
        }

        if (hasAmmo) {
            if (!shootAudioElementLocated) {
                shootAudioElementLocated = weaponSettings.shootAudioElement != null;
            }

            if (shootAudioElementLocated) {
                if (silencerActive) {
                    shootAudioElementToPlay = weaponSettings.silencerShootAudioElement;
                } else {
                    shootAudioElementToPlay = weaponSettings.shootAudioElement;
                }

                if (currentWeaponEffectsSourceLocated) {
                    if (silencerActive) {
                        currentWeaponEffectsSource.clip = weaponSettings.silencerShootEffect;
                    } else {
                        if (weaponSettings.useRandomShootSound) {
                            int randomClipIndex = Random.Range (0, weaponSettings.randomShootSoundList.Length);

                            currentWeaponEffectsSource.clip = weaponSettings.randomShootSoundList [randomClipIndex];
                        } else {

                            if (usingCustomImpactSoundEffectActive) {
                                currentWeaponEffectsSource.clip = customImpactSoundEffect;
                            } else {
                                currentWeaponEffectsSource.clip = weaponSettings.shootSoundEffect;
                            }
                        }
                    }

                    currentWeaponEffectsSource.pitch = weaponSpeed;

                    shootAudioElementToPlay.audioSource = currentWeaponEffectsSource;

                    shootAudioElementToPlay.clip = currentWeaponEffectsSource.clip;
                }

                AudioPlayer.Play (shootAudioElementToPlay, gameObject);
            }
        } else {
            if (Time.time > lastShoot + getFireRate ()) {
                if (currentWeaponEffectsSourceLocated) {
                    outOfAmmoAudioElement.audioSource = currentWeaponEffectsSource;
                    currentWeaponEffectsSource.pitch = weaponSpeed;

                    //                  GKC_Utils.checkAudioSourcePitch (currentWeaponEffectsSource);
                }

                AudioPlayer.PlayOneShot (outOfAmmoAudioElement, gameObject);

                lastShoot = Time.time;
            }
        }
    }

    bool weaponEffectsSourceAssigned;

    public void playSound (AudioElement clipSound)
    {
        if (playSoundPaused) {
            return;
        }

        if (!weaponEffectsSourceAssigned) {
            weaponEffectsSourceAssigned = weaponEffectsSource != null;
        }

        if (weaponEffectsSourceAssigned) {
            clipSound.audioSource = weaponEffectsSource;
            //			GKC_Utils.checkAudioSourcePitch (weaponEffectsSource);
        }

        AudioPlayer.PlayOneShot (clipSound, gameObject);
    }

    bool playSoundPaused;

    public void pauseOrResumePlaySoundOnWeapon (bool state)
    {
        playSoundPaused = state;
    }

    public AudioSource getAudioSourceFromPool ()
    {
        createNewWeaponEffectSource = false;

        if (weaponEffectsSourceList.Count < weaponSettings.maxSoundsPoolAmount) {
            createNewWeaponEffectSource = true;

        } else {
            int weaponEffectsSourceListCount = weaponEffectsSourceList.Count;

            for (int j = 0; j < weaponEffectsSourceListCount; j++) {
                if (!weaponEffectsSourceList [j].isPlaying) {
                    return weaponEffectsSourceList [j];
                }
            }

            weaponSettings.maxSoundsPoolAmount++;

            createNewWeaponEffectSource = true;
        }

        if (createNewWeaponEffectSource) {
            if (projectilesPoolEnabled) {
                newWeaponEffectSource = GKC_PoolingSystem.Spawn (weaponSettings.weaponEffectSourcePrefab,
                    transform.position, transform.rotation, maxAmountOfPoolElementsOnWeapon);
            } else {
                newWeaponEffectSource = (GameObject)Instantiate (weaponSettings.weaponEffectSourcePrefab, transform.position,
                    Quaternion.identity, weaponSettings.weaponEffectSourceParent);
            }

            currentAudioSource = newWeaponEffectSource.GetComponent<AudioSource> ();

            weaponEffectsSourceList.Add (currentAudioSource);

            return currentAudioSource;
        }

        return null;
    }

    GameObject newWeaponEffectSource;

    bool shootParticlesLocated;

    //create the muzzle flash particles if the weapon has it
    void createMuzzleFlash ()
    {
        if (!shootParticlesLocated) {
            shootParticlesLocated = weaponSettings.shootParticles != null;
        }

        if (shootParticlesLocated) {
            for (int j = 0; j < projectilePositionCount; j++) {

                currentProjectilePosition = weaponSettings.projectilePosition [j];

                if (projectilesPoolEnabled) {
                    newMuzzleParticlesClone = GKC_PoolingSystem.Spawn (weaponSettings.shootParticles, currentProjectilePosition.position, currentProjectilePosition.rotation, maxAmountOfPoolElementsOnWeapon);
                } else {
                    newMuzzleParticlesClone = (GameObject)Instantiate (weaponSettings.shootParticles, currentProjectilePosition.position, currentProjectilePosition.rotation);
                }

                if (weaponSettings.setShootParticlesLayerOnFirstPerson) {
                    if (carryingWeaponInFirstPerson) {
                        weaponsManager.setWeaponPartLayer (newMuzzleParticlesClone);
                    }
                }

                if (projectilesPoolEnabled) {

                } else {
                    Destroy (newMuzzleParticlesClone, 1);
                }

                newMuzzleParticlesClone.transform.SetParent (currentProjectilePosition);
            }
        }
    }

    public void setAmmoValues ()
    {
        if (weaponSettings.ammoPerClip == 0) {
            weaponSettings.ammoPerClip = weaponSettings.clipSize;

            originalClipSize = weaponSettings.ammoPerClip;
        }

        if (projectilesInMagazine != -1) {
            setWeaponClipSizeValue (projectilesInMagazine);
        }

        if (weaponSettings.startWithEmptyClip) {
            setWeaponClipSizeValue (0);
        } else {
            weaponSettings.clipSize += weaponsManager.getMagazineExtraSizeStat ();
        }

        weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;
    }

    //	//decrease the amount of ammo in the clip
    public void useAmmo ()
    {
        if (ignoreUseAmmoActive) {
            return;
        }

        weaponSettings.clipSize--;

        updateAmmoInfo ();

        //update hud ammo info
        weaponsManager.updateAmmo ();
    }

    public void useAmmo (int amount)
    {
        if (amount > weaponSettings.clipSize) {
            amount = weaponSettings.clipSize;
        }

        weaponSettings.clipSize -= amount;

        updateAmmoInfo ();

        //update hud ammo info
        weaponsManager.updateAmmo ();
    }

    public void updateAmmoInfo ()
    {
        if (fullBodyAwarenessActive) {
            return;
        }

        if (weaponSettings.useHUD) {
            if (!weaponSettingsHUDLocated) {
                weaponSettingsHUDLocated = weaponSettings.HUD != null;
            }

            if (weaponSettingsHUDLocated) {
                weaponSettings.clipSizeText.text = weaponSettings.clipSize.ToString ();

                if (!weaponSettings.infiniteAmmo) {
                    weaponSettings.remainAmmoText.text = weaponSettings.remainAmmo.ToString ();
                } else {
                    weaponSettings.remainAmmoText.text = "Inf";
                }
            }
        }
    }

    //check the amount of ammo
    void checkRemainAmmo ()
    {
        if (!weaponSettings.weaponUsesAmmo) {
            return;
        }

        int currentAmmoPerClip = weaponSettings.ammoPerClip + weaponsManager.getMagazineExtraSizeStat ();

        // If the weapon has not infinite ammo
        if (!weaponSettings.infiniteAmmo) {
            // The clip is empty
            int usedAmmo = 0;

            if (weaponSettings.clipSize == 0) {
                // If the remaining ammo is lower that the ammo per clip, set the final projectiles in the clip 
                if (weaponSettings.remainAmmo < currentAmmoPerClip) {
                    setWeaponClipSizeValue (weaponSettings.remainAmmo);
                } else {
                    // else, refill it
                    setWeaponClipSizeValue (currentAmmoPerClip);
                }

                usedAmmo = weaponSettings.clipSize;

                //if the remaining ammo is higher than 0, remove the current projectiles added in the clip
                if (weaponSettings.remainAmmo > 0) {
                    weaponSettings.remainAmmo -= weaponSettings.clipSize;
                }

            } else {
                //the clip has some bullets in it yet

                if (weaponSettings.removePreviousAmmoOnClip) {
                    setWeaponClipSizeValue (0);

                    if (weaponSettings.remainAmmo < (currentAmmoPerClip)) {
                        usedAmmo = weaponSettings.remainAmmo;
                    } else {
                        usedAmmo = currentAmmoPerClip;
                    }
                } else {
                    if (weaponSettings.remainAmmo < (currentAmmoPerClip - weaponSettings.clipSize)) {
                        //						print (weaponSettings.remainAmmo);

                        usedAmmo = weaponSettings.remainAmmo;
                    } else {
                        //						print (currentAmmoPerClip + " " + weaponSettings.clipSize);

                        usedAmmo = currentAmmoPerClip - weaponSettings.clipSize;
                    }
                }

                weaponSettings.remainAmmo -= usedAmmo;
                weaponSettings.clipSize += usedAmmo;
            }

            if (isUseRemainAmmoFromInventoryActive ()) {
                if (usedAmmo > 0) {
                    weaponsManager.useAmmoFromInventory (weaponSettings.ammoName, usedAmmo);

                    checkToUpdateInventoryWeaponAmmoTextFromWeaponSystem ();
                }
            } else {
                if (usedAmmo > 0) {
                    checkToUpdateInventoryWeaponAmmoTextFromWeaponSystem ();
                }
            }
        } else {
            //else, the weapon has infinite ammo, so refill it
            setWeaponClipSizeValue (currentAmmoPerClip);
        }

        updateAmmoInfo ();

        weaponsManager.updateAmmo ();

        weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;
    }

    public void reloadSingleProjectile ()
    {
        if (!weaponSettings.weaponUsesAmmo) {
            return;
        }

        int usedAmmo = 1;

        // If the weapon has not infinite ammo
        if (!weaponSettings.infiniteAmmo) {
            // The clip is empty
            if (weaponSettings.clipSize == 0) {
                // If the remaining ammo is lower that the ammo per clip, set the final projectiles in the clip 
                setWeaponClipSizeValue (1);

                // If the remaining ammo is higher than 0, remove the current projectiles added in the clip
                if (weaponSettings.remainAmmo > 0) {
                    weaponSettings.remainAmmo -= 1;
                }
            } else {
                // The clip has some bullets in it yet
                if (weaponSettings.removePreviousAmmoOnClip) {
                    setWeaponClipSizeValue (1);
                }

                weaponSettings.remainAmmo -= usedAmmo;
                weaponSettings.clipSize += usedAmmo;
            }

            if (isUseRemainAmmoFromInventoryActive ()) {
                if (usedAmmo > 0) {
                    weaponsManager.useAmmoFromInventory (weaponSettings.ammoName, usedAmmo);

                    checkToUpdateInventoryWeaponAmmoTextFromWeaponSystem ();
                }
            } else {
                if (usedAmmo > 0) {
                    checkToUpdateInventoryWeaponAmmoTextFromWeaponSystem ();
                }
            }
        } else {
            // else, the weapon has infinite ammo, so refill it
            weaponSettings.clipSize += usedAmmo;

            int currentAmmoPerClip = weaponSettings.ammoPerClip + weaponsManager.getMagazineExtraSizeStat ();

            int newValue = Mathf.Clamp (weaponSettings.clipSize, 0, currentAmmoPerClip);

            setWeaponClipSizeValue (newValue);
        }

        updateAmmoInfo ();

        weaponsManager.updateAmmo ();

        weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;
    }

    public void checkToUpdateInventoryWeaponAmmoTextFromWeaponSystem ()
    {
        weaponsManager.checkToUpdateInventoryWeaponAmmoText ();
    }

    public void checkToUpdateInventoryWeaponAmmoTextByWeaponNumberKey ()
    {
        weaponsManager.checkToUpdateInventoryWeaponAmmoTextByWeaponNumberKey (getWeaponNumberKey ());
    }

    public void setInitialProjectilesInMagazine (int newProjectilesInMagazine)
    {
        projectilesInMagazine = newProjectilesInMagazine;

        setAmmoValues ();
    }

    public int getProjectilesInMagazine ()
    {
        return weaponSettings.clipSize;
    }

    public void setRemainAmmoAmount (int newRemainAmmoAmount)
    {
        if (weaponSettings.remainAmmo != newRemainAmmoAmount) {
            weaponSettings.remainAmmo = newRemainAmmoAmount;

            updateAmmoInfo ();

            weaponsManager.updateAmmo ();

            weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;
        }
    }

    bool ignoreReloadAnimationAndDelayActive;

    public void setIgnoreReloadAnimationAndDelayActiveState (bool state)
    {
        ignoreReloadAnimationAndDelayActive = state;
    }

    bool ignoreUseAmmoActive;

    public void setIgnoreUseAmmoActiveState (bool state)
    {
        ignoreUseAmmoActive = state;
    }

    public void reloadWeapon ()
    {
        if (weaponsManager.isPauseWeaponReloadActive ()) {
            return;
        }

        if (ignoreReloadAnimationAndDelayActive) {
            checkRemainAmmo ();

            return;
        }

        stopReloadCoroutine ();

        reloadCoroutine = StartCoroutine (waitToReload ());
    }

    public void stopReloadCoroutine ()
    {
        if (reloadCoroutine != null) {
            StopCoroutine (reloadCoroutine);
        }
    }

    public void cancelReloadIfActive ()
    {
        if (reloading || reloadingWithAnimationActive) {
            stopReloadCoroutine ();
            stopReloadAction ();

            if (weaponSettings.useEventOnCancelReload) {
                weaponSettings.eventOnCancelReload.Invoke ();
            }
        }
    }

    public void stopReloadAction ()
    {
        //check the ammo values
        checkRemainAmmo ();

        //stop reload
        reloading = false;

        if (weaponSettings.useReloadAnimation) {
            if (weaponHasAnimation && canUseReloadAnimation ()) {
                weaponAnimation.Rewind (weaponSettings.reloadAnimationName);
            }
        }
    }

    public bool isWeaponReloading ()
    {
        return reloading;
    }

    public bool canUseReloadAnimation ()
    {
        if (weaponsManager.checkIfFirstPersonActiveExternally ()) {
            if (weaponSettings.useReloadOnFirstPerson) {
                return true;
            }
        } else {
            if (weaponSettings.useReloadOnThirdPerson) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Adds a delay when reloading weapons for the given amount of seconds.
    /// </summary>
    /// <param name="waitTimeAmount">Number of seconds to wait before reloading.</param>
    IEnumerator waitToReload ()
    {
        //print ("reload");
        //if the remaining ammo is higher than 0 or infinite
        shootingBurst = false;

        bool ignoreAnyWaitTimeOnReload = weaponSettings.ignoreAnyWaitTimeOnReload;

        if (weaponSettings.remainAmmo > 0 || weaponSettings.infiniteAmmo) {
            //reload
            reloading = true;

            if (!ignoreAnyWaitTimeOnReload) {
                bool firstPersonActive = weaponsManager.checkIfFirstPersonActiveExternally ();

                if (firstPersonActive) {
                    if (weaponSettings.usePreReloadDelayFirstPerson) {
                        WaitForSeconds delay = new WaitForSeconds (weaponSettings.preReloadDelayFirstPerson);

                        yield return delay;
                    }
                } else {
                    if (weaponSettings.usePreReloadDelayThirdPerson) {
                        WaitForSeconds delay = new WaitForSeconds (weaponSettings.preReloadDelayThirdPerson);

                        yield return delay;
                    }
                }

                if (firstPersonActive) {
                    IKWeaponManager.reloadWeaponFirstPerson ();
                }

                if (weaponSettings.useReloadEvent) {
                    bool useFirstPersonReloadEventsResult = false;

                    if (firstPersonActive || (fullBodyAwarenessActive && !weaponSettings.useThirdPersonEventOnReloadOnFBAView)) {
                        useFirstPersonReloadEventsResult = true;
                    }

                    if (useFirstPersonReloadEventsResult) {
                        if (weaponSettings.useReloadEventFirstPerson) {
                            if (usingDualWeapon) {
                                weaponSettings.reloadDualWeaponFirstPersonEvent.Invoke ();
                            } else {
                                weaponSettings.reloadSingleWeaponFirstPersonEvent.Invoke ();
                            }
                        }
                    } else {
                        if (weaponSettings.useReloadEventThirdPerson) {
                            if (usingDualWeapon) {
                                weaponSettings.reloadDualWeaponThirdPersonEvent.Invoke ();
                            } else {
                                weaponSettings.reloadSingleWeaponThirdPersonEvent.Invoke ();
                            }
                        }
                    }
                }

                if (weaponSettings.dropClipWhenReload) {

                    if ((firstPersonActive && weaponSettings.dropClipWhenReloadFirstPerson) ||
                        (!firstPersonActive && weaponSettings.dropClipWhenReloadThirdPerson)) {

                        if (firstPersonActive) {
                            WaitForSeconds delay = new WaitForSeconds (weaponSettings.delayDropClipWhenReloadFirstPerson);

                            yield return delay;
                        } else {
                            WaitForSeconds delay = new WaitForSeconds (weaponSettings.delayDropClipWhenReloadThirdPerson);

                            yield return delay;
                        }

                        newClipToDrop = (GameObject)Instantiate (weaponSettings.clipModel, weaponSettings.positionToDropClip.position, weaponSettings.positionToDropClip.rotation);

                        newClipIgnoreCollisionSystem = newClipToDrop.GetComponent<ignoreCollisionSystem> ();

                        if (newClipIgnoreCollisionSystem != null) {
                            newClipIgnoreCollisionSystem.activateIgnoreCollision (playerCollider);
                        }
                    }
                }

                if (weaponSettings.useShellOnWeaponShootEnabled) {
                    if (weaponSettings.createShellsOnReload) {
                        if (firstPersonActive) {
                            WaitForSeconds delay = new WaitForSeconds (weaponSettings.createShellsOnReloadDelayFirstPerson);

                            yield return delay;
                        } else {
                            WaitForSeconds delay = new WaitForSeconds (weaponSettings.createShellsOnReloadDelayThirdPerson);

                            yield return delay;
                        }

                        createShells ();
                    }
                }

                if (firstPersonActive) {
                    if (weaponSettings.useReloadDelayFirstPerson) {
                        WaitForSeconds delay = new WaitForSeconds (weaponSettings.reloadDelayFirstPerson);

                        yield return delay;
                    }
                } else {
                    if (weaponSettings.useReloadDelayThirdPerson) {
                        WaitForSeconds delay = new WaitForSeconds (weaponSettings.reloadDelayThirdPerson);

                        yield return delay;
                    }
                }

                if (weaponSettings.useReloadAnimation && canUseReloadAnimation ()) {
                    if (weaponHasAnimation) {
                        weaponAnimation [weaponSettings.reloadAnimationName].speed = weaponSettings.reloadAnimationSpeed;

                        weaponAnimation.Play (weaponSettings.reloadAnimationName);
                    }
                }

                //play the reload sound
                if (weaponSettings.reloadAudioElement != null) {
                    playSound (weaponSettings.reloadAudioElement);
                }

                //wait an amount of time
                if (firstPersonActive) {
                    WaitForSeconds delay = new WaitForSeconds (weaponSettings.reloadTimeFirstPerson);

                    yield return delay;
                } else {
                    WaitForSeconds delay = new WaitForSeconds (weaponSettings.reloadTimeThirdPerson);

                    yield return delay;
                }
            }

            //check the ammo values
            checkRemainAmmo ();

            //stop reload
            reloading = false;
        } else {
            //else, the ammo is over, play the empty weapon sound
            playWeaponSoundEffect (false);
        }

        yield return null;
    }

    public void manualReload ()
    {
        if (!reloading && weaponSettings.weaponUsesAmmo) {
            if (weaponSettings.clipSize < weaponSettings.ammoPerClip + weaponsManager.getMagazineExtraSizeStat ()) {
                reloadWeapon ();
            }
        }
    }

    public void setReloadingWithAnimationActiveState (bool state)
    {
        reloadingWithAnimationActive = state;
    }

    public bool isReloadingWithAnimationActive ()
    {
        return reloadingWithAnimationActive;
    }

    bool weaponSettingsHUDLocated;

    public void enableHUD (bool state)
    {
        if (fullBodyAwarenessActive) {
            if (state && !weaponSettings.useHUDOnFBA) {
                state = false;
            }
        }

        if (!weaponSettingsHUDLocated) {
            weaponSettingsHUDLocated = weaponSettings.HUD != null;
        }

        if (weaponSettingsHUDLocated) {
            if (weaponSettings.useHUD) {
                if (usingDualWeapon) {
                    if ((carryingWeaponInThirdPerson && weaponSettings.useHUDDualWeaponThirdPerson)
                        || (carryingWeaponInFirstPerson && weaponSettings.useHUDDualWeaponFirstPerson)) {

                        if (weaponSettings.HUD.activeSelf != state) {
                            weaponSettings.HUD.SetActive (state);
                        }
                    }
                } else {
                    if (weaponSettings.HUD.activeSelf != state) {
                        weaponSettings.HUD.SetActive (state);
                    }
                }

                updateAmmoInfo ();
            } else {
                if (weaponSettings.HUD.activeSelf) {
                    weaponSettings.HUD.SetActive (false);
                }
            }
        }
    }

    public void enableHUDTemporarily (bool state)
    {
        if (fullBodyAwarenessActive) {
            if (state) {
                state = false;
            }
        }

        if (!weaponSettingsHUDLocated) {
            weaponSettingsHUDLocated = weaponSettings.HUD != null;
        }

        if (weaponSettingsHUDLocated) {
            if (weaponSettings.useHUD) {
                if (weaponSettings.HUD.activeSelf != state) {
                    weaponSettings.HUD.SetActive (state);
                }
            }
        }
    }

    public void changeHUDPosition (bool thirdPerson)
    {
        if (weaponSettings.useHUD) {
            if (!weaponSettingsHUDLocated) {
                weaponSettingsHUDLocated = weaponSettings.HUD != null;
            }

            if (weaponSettingsHUDLocated) {
                if (usingDualWeapon) {
                    if (weaponSettings.changeHUDPositionDualWeapon) {
                        if (thirdPerson) {
                            if (usingRightHandDualWeapon) {
                                weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDRightHandTransformThirdPerson.position;
                            } else {
                                weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDLeftHandTransformThirdPerson.position;
                            }
                        } else {
                            if (usingRightHandDualWeapon) {
                                weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDRightHandTransformFirstPerson.position;
                            } else {
                                weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDLeftHandTransformFirstPerson.position;
                            }
                        }
                    }
                } else {
                    if (weaponSettings.changeHUDPosition) {
                        if (thirdPerson) {
                            weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDTransformInThirdPerson.position;
                        } else {
                            weaponSettings.ammoInfoHUD.transform.position = weaponSettings.HUDTransformInFirstPerson.position;
                        }
                    }
                }
            }
        }
    }

    public GameObject getWeaponHUDGameObject ()
    {
        return weaponSettings.HUD;
    }

    public bool canIncreaseRemainAmmo ()
    {
        if (weaponSettings.auxRemainAmmo < weaponSettings.ammoLimit) {
            return true;
        } else {
            return false;
        }
    }

    public bool hasMaximumAmmoAmount ()
    {
        if (weaponSettings.useAmmoLimit) {
            if (weaponSettings.remainAmmo >= weaponSettings.ammoLimit) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
    }

    public bool hasAmmoLimit ()
    {
        return weaponSettings.useAmmoLimit;
    }

    public int ammoAmountToMaximumLimit ()
    {
        return weaponSettings.ammoLimit - weaponSettings.auxRemainAmmo;
    }

    //the player has used an ammo pickup, so increase the weapon ammo
    public void getAmmo (int amount)
    {
        //		print (amount);

        bool empty = false;

        if (weaponSettings.remainAmmo == 0 && weaponSettings.clipSize == 0) {
            empty = true;
        }

        if (isUseRemainAmmoFromInventoryActive ()) {
            weaponSettings.clipSize += amount;
            weaponSettings.remainAmmo -= amount;
        } else {
            weaponSettings.remainAmmo += amount;
        }

        if (empty && (carryingWeaponInFirstPerson || aimingInThirdPerson)) {
            if (weaponSettings.autoReloadWhenClipEmpty) {
                manualReload ();
            }
        }

        if (weaponSettings.remainAmmo < 0) {
            weaponSettings.remainAmmo = 0;
        }

        weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;

        updateAmmoInfo ();
    }

    public void useRemainAmmo (int amount)
    {
        weaponSettings.remainAmmo -= amount;

        if (weaponSettings.remainAmmo < 0) {
            weaponSettings.remainAmmo = 0;
        }

        weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;

        updateAmmoInfo ();
    }

    public void addAuxRemainAmmo (int amount)
    {
        weaponSettings.auxRemainAmmo += amount;
    }

    public bool hasAnyAmmo ()
    {
        if (weaponSettings.remainAmmo > 0 || weaponSettings.clipSize > 0 || weaponSettings.infiniteAmmo || !weaponSettings.weaponUsesAmmo) {
            return true;
        }
        return false;
    }

    public void getAndUpdateAmmo (int amount)
    {
        getAmmo (amount);
        weaponsManager.updateAmmo ();
    }

    public bool remainAmmoInClip ()
    {
        return weaponSettings.clipSize > 0;
    }

    public void launchCurrentProjectile (GameObject currentProjectile, Rigidbody projectileRigidbody, Vector3 cameraDirection)
    {
        //launch the projectile according to the velocity calculated according to the hit point of a raycast from the camera position
        projectileRigidbody.isKinematic = false;

        if (weaponSettings.useParableSpeed) {
            Vector3 newVel = getParableSpeed (currentProjectile.transform.position, aimedZone, cameraDirection);

            if (newVel == -Vector3.one) {
                newVel = 100 * currentProjectile.transform.forward;
            }

            projectileRigidbody.AddForce (newVel, ForceMode.VelocityChange);
        } else {
            Vector3 fireDirection = cameraDirection;

            if (weaponSettings.parableDirectionTransform != null) {
                fireDirection = weaponSettings.parableDirectionTransform.forward;
            }

            projectileRigidbody.AddForce (weaponSettings.projectileSpeed * fireDirection, ForceMode.Impulse);
        }
    }

    //calculate the speed applied to the launched projectile to make a parable according to a hit point
    Vector3 getParableSpeed (Vector3 origin, Vector3 target, Vector3 cameraDirection)
    {
        //if a hit point is not found, return
        if (!objectiveFound) {
            if (weaponSettings.useMaxDistanceWhenNoSurfaceFound) {
                target = origin + weaponSettings.maxDistanceWhenNoSurfaceFound * cameraDirection;
            } else {
                return -Vector3.one;
            }
        }

        //get the distance between positions
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;

        //remove the Y axis value
        toTargetXZ -= playerCameraGameObject.transform.InverseTransformDirection (toTargetXZ).y * playerCameraGameObject.transform.up;

        float y = playerCameraGameObject.transform.InverseTransformDirection (toTarget).y;
        float xz = toTargetXZ.magnitude;

        //get the velocity according to distance ang gravity
        float t = GKC_Utils.distance (origin, target) / 20;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        //create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;

        //get direction of xz but with magnitude 1
        result = v0xz * result;

        // set magnitude of xz to v0xz (starting speed in xz plane), setting the local Y value
        result -= playerCameraGameObject.transform.InverseTransformDirection (result).y * playerCameraGameObject.transform.up;

        result += v0y * playerCameraGameObject.transform.up;

        return result;
    }

    public void checkParableTrayectory (bool usingThirdPerson, bool usingFirstPerson)
    {
        //enable or disable the parable linerenderer
        if (((usingThirdPerson && weaponSettings.activateLaunchParableThirdPerson) ||
            (usingFirstPerson && weaponSettings.activateLaunchParableFirstPerson)) &&
            weaponSettings.launchProjectile) {

            if (parable != null) {
                parable.changeParableState (true);

                if (projectilePositionCount > 0) {
                    parable.shootPosition = weaponSettings.projectilePosition [0];
                }
            }
        } else {
            if (parable != null) {
                parable.changeParableState (false);
            }
        }
    }

    public void setCurrentWeaponState (bool state)
    {
        currentWeapon = state;

        if (!currentWeapon) {
            lastTimeWeaponActive = Time.time;
        }

        if (currentWeapon) {
            if (useAbilitiesListToEnableOnWeapon) {
                GKC_Utils.enableOrDisableAbilityGroupByName (playerControllerGameObject.transform, true, abilitiesListToEnableOnWeapon);
            }
        } else {
            if (useAbilitiesListToDisableOnWeapon) {
                GKC_Utils.enableOrDisableAbilityGroupByName (playerControllerGameObject.transform, false, abilitiesListToDisableOnWeapon);
            }
        }
    }

    public Transform getWeaponParent ()
    {
        return originalParent;
    }

    public Transform getWeaponsParent ()
    {
        if (weaponSettings.weaponParent != null) {
            return weaponSettings.weaponParent;
        }

        return null;
    }

    public Transform getMainCameraTransform ()
    {
        return mainCameraTransform;
    }

    public void setReducedVelocity (float multiplierValue)
    {
        weaponSpeed *= multiplierValue;
    }

    public void setNormalVelocity ()
    {
        weaponSpeed = originalWeaponSpeed;
    }

    public string getWeaponSystemAmmoName ()
    {
        return weaponSettings.ammoName;
    }

    public string getWeaponSystemName ()
    {
        return weaponSettings.Name;
    }

    public int getWeaponNumberKey ()
    {
        return weaponSettings.numberKey;
    }

    public int getOriginalWeaponNumberKey ()
    {
        return originalNumberKey;
    }

    public int getWeaponClipSize ()
    {
        return weaponSettings.clipSize;
    }

    void setWeaponClipSizeValue (int newValue)
    {
        weaponSettings.clipSize = newValue;
    }

    public int getMagazineSize ()
    {
        return weaponSettings.ammoPerClip;
    }

    public bool isCurrentMagazineEmpty ()
    {
        return weaponSettings.clipSize == 0;
    }

    public bool isRemainAmmoEmpty ()
    {
        return weaponSettings.remainAmmo == 0;
    }

    public string getCurrentAmmoText ()
    {
        if (!weaponSettings.infiniteAmmo) {
            return weaponSettings.clipSize + "/" + weaponSettings.remainAmmo;
        } else {
            return weaponSettings.clipSize + "/" + "Inf";
        }
    }

    public int getAllRemainAmmoOnWeapon ()
    {
        if (!weaponSettings.infiniteAmmo) {
            return weaponSettings.clipSize + weaponSettings.remainAmmo;
        } else {
            return weaponSettings.clipSize;
        }
    }

    public int getRegularRemainAmmo ()
    {
        return weaponSettings.remainAmmo;
    }

    public Texture getWeaponIcon ()
    {
        return weaponSettings.weaponIcon;
    }

    public Texture getWeaponInventorySlotIcon ()
    {
        return weaponSettings.weaponInventorySlotIcon;
    }

    public void setWeaponInventorySlotIcon (Texture newTexture)
    {
        if (newTexture == null) {
            return;
        }

        weaponSettings.weaponInventorySlotIcon = newTexture;
    }

    public void setWeaponInventorySlotIconInEditor (Texture newTexture)
    {
        setWeaponInventorySlotIcon (newTexture);

        updateComponent ();
    }

    public void setNumberKey (int newNumberKey)
    {
        if (originalNumberKey == -1) {
            originalNumberKey = weaponSettings.numberKey;
        }

        weaponSettings.numberKey = newNumberKey;
    }

    public void enableMuzzleFlashLight ()
    {
        if (!weaponSettings.useMuzzleFlash) {
            return;
        }

        if (muzzleFlashCoroutine != null) {
            StopCoroutine (muzzleFlashCoroutine);
        }

        muzzleFlashCoroutine = StartCoroutine (enableMuzzleFlashCoroutine ());
    }

    IEnumerator enableMuzzleFlashCoroutine ()
    {
        weaponSettings.muzzleFlahsLight.enabled = true;

        WaitForSeconds delay = new WaitForSeconds (weaponSettings.muzzleFlahsDuration);

        yield return delay;

        weaponSettings.muzzleFlahsLight.enabled = false;

        yield return null;
    }

    public void checkWeaponShootNoise ()
    {
        if (silencerActive) {
            return;
        }

        if (weaponSettings.useNoise) {
            if (weaponSettings.useNoiseDetection) {
                applyDamage.sendNoiseSignal (weaponSettings.noiseRadius, playerControllerGameObject.transform.position,
                    weaponSettings.noiseDetectionLayer, weaponSettings.noiseDecibels,
                    weaponSettings.forceNoiseDetection, weaponSettings.showNoiseDetectionGizmo, weaponSettings.noiseID);
            }

            if (weaponSettings.useNoiseMesh) {
                if (!noiseMeshManagerFound) {
                    noiseMeshManagerFound = noiseMeshManager != null;

                    if (!noiseMeshManagerFound) {
                        noiseMeshManager = noiseMeshSystem.Instance;

                        noiseMeshManagerFound = noiseMeshManager != null;
                    }

                    if (!noiseMeshManagerFound) {
                        GKC_Utils.instantiateMainManagerOnSceneWithTypeOnApplicationPlaying (noiseMeshSystem.getMainManagerName (), typeof (noiseMeshSystem), true);

                        noiseMeshManager = noiseMeshSystem.Instance;

                        noiseMeshManagerFound = noiseMeshManager != null;
                    }

                    if (!noiseMeshManagerFound) {

                        noiseMeshManager = FindObjectOfType<noiseMeshSystem> ();

                        noiseMeshManagerFound = noiseMeshManager != null;
                    }
                }

                if (noiseMeshManagerFound) {
                    noiseMeshManager.addNoiseMesh (weaponSettings.noiseRadius, playerControllerGameObject.transform.position + Vector3.up, weaponSettings.noiseExpandSpeed);
                }
            }
        }
    }

    //functions to change current weapon stats
    public void setMagazineSize (int magazineSize)
    {
        weaponSettings.ammoPerClip = magazineSize;

        weaponsManager.updateWeaponHUDInfo ();

        updateAmmoInfo ();
    }

    public int getAmmoAmountToRefillMagazine ()
    {
        return weaponSettings.ammoPerClip - weaponSettings.clipSize;
    }

    public playerWeaponsManager getPlayerWeaponsManger ()
    {
        return weaponsManager;
    }

    public void setOriginalMagazineSize ()
    {
        if (weaponSettings.clipSize > originalClipSize) {
            int extraBulletsOnMagazine = 0;
            extraBulletsOnMagazine = weaponSettings.clipSize - originalClipSize;

            if (extraBulletsOnMagazine > 0) {
                weaponSettings.remainAmmo += extraBulletsOnMagazine;

                setWeaponClipSizeValue (originalClipSize);

                weaponSettings.auxRemainAmmo = weaponSettings.remainAmmo;
            }
        }

        setMagazineSize (originalClipSize);
    }

    public void setCurrentProjectilesInMagazine (int newAmount)
    {
        setWeaponClipSizeValue (newAmount);

        weaponsManager.updateWeaponHUDInfo ();

        updateAmmoInfo ();
    }


    public void setSilencerState (bool state)
    {
        silencerActive = state;
    }

    public void setAutomaticFireMode (bool state)
    {
        weaponSettings.automatic = state;
    }

    public void setOriginalAutomaticFireMode ()
    {
        setAutomaticFireMode (originalAutomaticMode);
    }

    public void setFireRate (float newFireRate)
    {
        weaponSettings.fireRate = newFireRate;
    }

    public void setOriginalFireRate ()
    {
        setFireRate (originalFireRate);
    }

    public void setProjectileDamage (float newDamage)
    {
        weaponSettings.projectileDamage = newDamage;
    }

    public void setOriginalProjectileDamage ()
    {
        setProjectileDamage (originalProjectileDamage);
    }

    public void setWeaponShootSoundEffect (AudioClip newClip)
    {
        weaponSettings.shootSoundEffect = newClip;
        weaponSettings.shootAudioElement.clip = newClip;
    }

    public void setWeaponImpactSoundEffect (AudioClip newClip)
    {
        weaponSettings.impactSoundEffect = newClip;
        weaponSettings.impactAudioElement.clip = newClip;

        projectileInfoAssigned = false;
    }

    public void setCustomWeaponImpactSoundEffect (AudioClip newClip)
    {
        customShootSoundEffect = newClip;

        usingCustomShootSoundEffectActive = customShootSoundEffect != null;

        weaponSettings.impactAudioElement.clip = newClip;

        projectileInfoAssigned = false;
    }

    public void setCustomImpactSoundEffect (AudioClip newClip)
    {
        customImpactSoundEffect = newClip;

        usingCustomImpactSoundEffectActive = customImpactSoundEffect != null;

        weaponSettings.shootAudioElement.clip = newClip;

        projectileInfoAssigned = false;
    }

    public void setWeaponUseRayCastShoot (bool state)
    {
        weaponSettings.useRayCastShoot = state;
    }

    public void setOriginalWeaponUseRayCastShoot ()
    {
        setWeaponUseRayCastShoot (originalUseRayCastShootValue);
    }

    public void setBurstModeState (bool state)
    {
        weaponSettings.useBurst = state;

        if (weaponSettings.useBurst) {
            weaponSettings.automatic = true;
        } else {
            weaponSettings.automatic = originalAutomaticMode;
        }
    }

    public void setOriginalBurstMode ()
    {
        setBurstModeState (orignalUseBurst);
    }

    public void setBurstAmount (int newAmount)
    {
        weaponSettings.burstAmount = newAmount;
    }

    public void setUsingSightState (bool state)
    {
        usingSight = state;
    }

    public void setUsingSightFullBodyAwareness (bool state)
    {
        usingSightFullBodyAwareness = state;
    }

    public bool isUsingSightFullBodyAwareness ()
    {
        return usingSightFullBodyAwareness;
    }

    public void setProjectileDamageMultiplier (float multiplier)
    {
        weaponSettings.projectileDamage = originalProjectileDamage * multiplier;
    }

    public void setSpreadState (bool state)
    {
        weaponSettings.useProjectileSpread = state;
    }

    public void setOriginalSpreadState ()
    {
        setSpreadState (originalSpreadState);
    }

    public bool isUsingSight ()
    {
        return usingSight;
    }

    public void setKillOneShotState (bool state)
    {
        weaponSettings.killInOneShot = state;
    }

    public void setExplosiveAmmoState (bool state)
    {
        weaponSettings.isExplosive = state;
    }

    public void setOriginalExplosiveAmmoState ()
    {
        weaponSettings.isExplosive = originalIsExplosiveState;
    }

    public void setDamageOverTimeAmmoState (bool state)
    {
        weaponSettings.damageTargetOverTime = state;
    }

    public void setOriginalDamageOverTimeAmmoState ()
    {
        weaponSettings.damageTargetOverTime = originalDamageTargetOverTimeState;
    }

    public void setRemoveDamageOverTimeAmmoState (bool state)
    {
        weaponSettings.removeDamageOverTimeState = state;
        resetAmmoState (state);
    }

    public void setOriginalRemoveDamageOverTimeAmmoState ()
    {
        weaponSettings.removeDamageOverTimeState = originalRemoveDamageTargetOverTimeState;
        resetAmmoState (originalRemoveDamageTargetOverTimeState);
    }

    public void setSedateAmmoState (bool state)
    {
        weaponSettings.sedateCharacters = state;
        resetAmmoState (state);
    }

    public void setOriginalSedateAmmoState ()
    {
        weaponSettings.sedateCharacters = originalSedateCharactersState;
        resetAmmoState (originalSedateCharactersState);
    }

    public void resetAmmoState (bool state)
    {
        if (state) {
            setProjectileDamage (0);
            setExplosiveAmmoState (false);
            setDamageOverTimeAmmoState (false);
        } else {
            setOriginalProjectileDamage ();
            setOriginalExplosiveAmmoState ();
            setOriginalDamageOverTimeAmmoState ();
        }
    }

    public float getFireRate ()
    {
        return weaponSettings.fireRate * weaponsManager.getFireRateMultiplierStat ();
    }

    public void setPushCharacterState (bool state)
    {
        weaponSettings.pushCharacter = state;
    }

    public void setOriginalPushCharacterState ()
    {
        weaponSettings.pushCharacter = originalPushCharacterState;
    }

    public bool useCustomReticleEnabled ()
    {
        return weaponSettings.useCustomReticle;
    }

    public Texture getRegularCustomReticle ()
    {
        return weaponSettings.regularCustomReticle;
    }

    public bool useAimCustomReticleEnabled ()
    {
        return weaponSettings.useAimCustomReticle;
    }

    public Texture getAimCustomReticle ()
    {
        return weaponSettings.aimCustomReticle;
    }

    public bool useDynamicReticleEnabled ()
    {
        return weaponSettings.useDynamicReticle;
    }

    public string getDynamicReticleName ()
    {
        return weaponSettings.dynamicReticleName;
    }

    public bool isUseRemainAmmoFromInventoryActive ()
    {
        return weaponSettings.useRemainAmmoFromInventory || weaponsManager.isUseAmmoFromInventoryInAllWeaponsActive ();
    }

    public void setNewWeaponProjectile (GameObject newProjectile)
    {
        weaponProjectile = newProjectile;
    }

    public void setOriginalWeaponProjectile ()
    {
        weaponProjectile = originalWeaponProjectile;
    }

    public void setProjectileWithAbilityState (bool newValue)
    {
        weaponSettings.projectileWithAbility = newValue;
    }

    public void setOriginalProjectileWithAbilityValue ()
    {
        weaponSettings.projectileWithAbility = originalProjectileWithAbilityState;
    }

    public void setUsingDualWeaponState (bool state)
    {
        usingDualWeapon = state;

        if (!usingDualWeapon) {
            if (useEventOnSetSingleWeapon) {
                eventOnSetSingleWeapon.Invoke ();
            }
        }
    }

    public void setUsingRightHandDualWeaponState (bool state)
    {
        usingRightHandDualWeapon = state;

        if (useEventOnSetDualWeapon) {
            if (usingRightHandDualWeapon) {
                eventOnSetRightWeapon.Invoke ();
            } else {
                eventOnSetLeftWeapon.Invoke ();
            }
        }
    }

    public void resetWeaponMeshTransform ()
    {
        weaponSettings.weaponMesh.transform.localPosition = Vector3.zero;
        weaponSettings.weaponMesh.transform.localRotation = Quaternion.identity;
    }

    public void enableOrDisableWeaponMesh (bool state)
    {
        //print ("mesh state "+gameObject.name + " " + state);
        if (weaponSettings.weaponMesh.activeSelf != state) {
            weaponSettings.weaponMesh.SetActive (state);
        }
    }

    public void placeMagazineInPlayerHand (bool state)
    {
        if (weaponSettings.clipModel != null) {
            if (currentMagazine == null) {
                currentMagazine = (GameObject)Instantiate (weaponSettings.magazineInHandGameObject, Vector3.zero, Quaternion.identity);
            }

            if (currentMagazine.activeSelf != state) {
                currentMagazine.SetActive (state);
            }

            if (state) {
                IKWeaponManager.placeMagazineInPlayerHand (currentMagazine.transform, weaponSettings.magazineInHandTransform);
            }
        }
    }

    public void setNewReloadTimeFirstPerson (float newValue)
    {
        weaponSettings.reloadTimeFirstPerson = newValue;
    }

    public void setInfiniteAmmoValue (bool state)
    {
        weaponSettings.infiniteAmmo = state;
    }

    public void setOriginalInfiniteAmmo ()
    {
        setInfiniteAmmoValue (originalInfiniteAmmo);
    }

    public void setSliceObjectsDetectedState (bool state)
    {
        weaponSettings.sliceObjectsDetected = state;
    }

    public void setOriginalSliceObjectsDetected ()
    {
        setSliceObjectsDetectedState (originalSliceObjectsDetected);
    }

    public void setProjectileSpecialActionActiveState (bool state)
    {
        projectileSpecialActionActive = state;
    }

    public void setProjectileSpecialActionValue (bool state)
    {
        projectileSpecialActionValue = state;
    }

    public void checkDurabilityOnAttack ()
    {
        if (useObjectDurability) {
            if (weaponsManager.isCheckDurabilityOnObjectEnabled ()) {
                mainDurabilityInfo.addOrRemoveDurabilityAmountToObjectByName (-durabilityUsedOnAttack, false);

                if (mainDurabilityInfo.isDurabilityEmpty ()) {
                    weaponsManager.checkEventOnEmptyDurability ();
                }
            }
        }
    }

    public void updateDurabilityAmountState ()
    {
        if (useObjectDurability) {
            mainDurabilityInfo.updateDurabilityAmountState ();
        }
    }

    public void initializeDurabilityValue (float newAmount, int currentObjectIndex)
    {
        if (useObjectDurability) {
            mainDurabilityInfo.initializeDurabilityValue (newAmount);

            mainDurabilityInfo.setInventoryObjectIndex (currentObjectIndex);
        }
    }

    public void setInventoryObjectIndex (int currentObjectIndex)
    {
        if (useObjectDurability) {
            mainDurabilityInfo.setInventoryObjectIndex (currentObjectIndex);
        }
    }

    public float getDurabilityAmount ()
    {
        if (useObjectDurability) {
            return mainDurabilityInfo.getDurabilityAmount ();
        }

        return -1;
    }

    public void repairObjectFully ()
    {
        if (useObjectDurability) {
            mainDurabilityInfo.repairObjectFully ();
        }
    }

    public void breakFullDurabilityOnCurrentWeapon ()
    {
        if (useObjectDurability) {
            if (weaponsManager.isCheckDurabilityOnObjectEnabled ()) {
                mainDurabilityInfo.breakFullDurability ();

                weaponsManager.checkEventOnEmptyDurability ();
            }
        }
    }

    public void setDurabilityUsedOnAttackValue (float newValue)
    {
        durabilityUsedOnAttack = newValue;
    }

    public void setObjectNameFromEditor (string newName)
    {
        if (useObjectDurability) {
            mainDurabilityInfo.setObjectNameFromEditor (newName);
        }
    }

    public void setDurabilityAmountFromEditor (float newValue)
    {
        if (useObjectDurability) {
            mainDurabilityInfo.setDurabilityAmountFromEditor (newValue);
        }
    }

    public void setMaxDurabilityAmountFromEditor (float newValue)
    {
        if (useObjectDurability) {
            mainDurabilityInfo.setMaxDurabilityAmountFromEditor (newValue);
        }
    }

    public void setCanGrabObjectsCarryingWeaponsState (bool state)
    {
        weaponsManager.setCanGrabObjectsCarryingWeaponsState (state);
    }

    public void setOriginalCanGrabObjectsCarryingWeaponsState ()
    {
        weaponsManager.setOriginalCanGrabObjectsCarryingWeaponsState ();
    }

    //EDITOR FUNCTIONS
    public void setDurabilityUsedOnAttackValueFromEditor (float newValue)
    {
        setDurabilityUsedOnAttackValue (newValue);

        updateComponent ();
    }

    public void setMagazineSizeFromEditor (float newAmount)
    {
        setWeaponClipSizeValue ((int)newAmount);

        updateComponent ();
    }

    public void setRemainAmmoAmountFromEditor (int newRemainAmmoAmount)
    {
        weaponSettings.remainAmmo = newRemainAmmoAmount;

        updateComponent ();
    }

    public void setInfiniteAmmoValueFromEditor (bool state)
    {
        setInfiniteAmmoValue (state);

        updateComponent ();
    }

    public void setFireRateFromEditor (float newFireRate)
    {
        setFireRate (newFireRate);

        updateComponent ();
    }

    public void setProjectileDamageFromEditor (float newDamage)
    {
        setProjectileDamage (newDamage);

        updateComponent ();
    }

    public void setCharacter (GameObject pController, GameObject pCamera)
    {
        playerControllerGameObject = pController;
        playerCameraGameObject = pCamera;

        updateComponent ();
    }

    public void setWeaponParent (Transform parent, Animator animatorToUse)
    {
        weaponSettings.weaponParent = parent;

        if (useHumanBodyBonesEnum) {
            weaponSettings.weaponParent = animatorToUse.GetBoneTransform (weaponParentBone);
        }

        updateComponent ();
    }

    public void getWeaponComponents ()
    {
        IKWeaponManager = transform.parent.GetComponent<IKWeaponSystem> ();

        weaponsManager = IKWeaponManager.getPlayerWeaponsManager ();

        mainCameraTransform = weaponsManager.getMainCameraTransform ();

        mainCamera = weaponsManager.getMainCamera ();

        weaponEffectsSource = GetComponent<AudioSource> ();

        weaponAnimation = GetComponent<Animation> ();

        parable = GetComponentInChildren<launchTrayectory> ();

        playerCollider = playerControllerGameObject.GetComponent<Collider> ();

        FBAWeaponCollider = weaponsManager.getExtraColliderOnFireWeaponOnFBA ();

        FBAWeaponColliderAssigned = FBAWeaponCollider != null;

        headColliderOnFBA = weaponsManager.getHeadColliderOnFBA ();

        headColliderOnFBAAssigned = headColliderOnFBA != null;

        updateComponent ();
    }

    public void getImpactListInfo ()
    {
        if (impactDecalManager == null) {
            GKC_Utils.instantiateMainManagerOnSceneWithType (mainDecalManagerName, typeof (decalManager));

            impactDecalManager = FindObjectOfType<decalManager> ();
        }

        if (impactDecalManager != null) {
            impactDecalList = new string [impactDecalManager.impactListInfo.Count + 1];

            for (int i = 0; i < impactDecalManager.impactListInfo.Count; i++) {
                string name = impactDecalManager.impactListInfo [i].name;
                impactDecalList [i] = name;
            }

            updateComponent ();
        }
    }

    public void setWeaponSystemAmmoName (string newWeaponAmmoName)
    {
        weaponSettings.ammoName = newWeaponAmmoName;

        updateComponent ();
    }

    public void setWeaponSystemName (string newName)
    {
        weaponSettings.Name = newName;

        updateComponent ();
    }

    public void updateComponent ()
    {
        GKC_Utils.updateComponent (this);
    }
}