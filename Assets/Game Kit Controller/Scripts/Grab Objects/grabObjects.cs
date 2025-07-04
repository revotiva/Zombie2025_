﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class grabObjects : MonoBehaviour
{
    public float holdDistance = 3;
    public float maxDistanceHeld = 4;
    public float maxDistanceGrab = 10;
    public float holdSpeed = 10;
    public float alphaTransparency = 0.5f;
    public bool objectCanBeRotated;
    public float rotationSpeed;
    public float rotateSpeed;
    public float minTimeToIncreaseThrowForce = 300;
    public float increaseThrowForceSpeed = 1500;
    public float extraThorwForce = 10;
    public float maxThrowForce = 3500;
    public grabMode currentGrabMode;

    public bool useGrabbedObjectOffsetThirdPerson;
    public Vector3 grabbedObjectOffsetThirdPerson;

    public bool dropCurrentObjectOnDeathEnabled = true;
    public bool keepMeleeWeaponOnDeathEnabled;

    public bool takeObjectMassIntoAccountOnThrowEnabled;
    public float objectMassDividerOnThrow = 1;

    public bool grabInFixedPosition;
    public bool rotateToCameraInFixedPosition;
    public bool rotateToCameraInFreePosition;

    public float closestHoldDistanceInFixedPosition;
    public string grabbedObjectTag;
    public string grabbedObjectLayer;
    public Transform grabZoneTransform;

    public bool useCursor = true;
    public GameObject cursor;
    public RectTransform cursorRectTransform;

    public GameObject foundObjectToGrabCursor;

    public GameObject grabbedObjectCursor;
    public GameObject playerCameraTransform;

    public bool usedByAI;

    public bool grabObjectsEnabled;
    public Shader pickableShader;

    public string defaultShaderName = "Legacy Shaders/Transparent/Diffuse";

    public float headTrackBodyWeightOnAim = 1;

    public string customAimReticleName = "Reticle 1";

    public Slider powerSlider;

    public bool useLoadThrowParticles;

    public GameObject [] particles;
    public LayerMask layer;
    public bool enableTransparency = true;
    public bool useGrabbedParticles;
    public bool canUseZoomWhileGrabbed;
    public float zoomSpeed;
    public float maxZoomDistance;
    public float minZoomDistance;
    public ForceMode powerForceMode;

    public bool useThrowObjectsLayer = true;
    public LayerMask throwObjectsLayerToCheck;

    public float throwPower;
    public ForceMode realisticForceMode;

    public LayerMask gravityObjectsLayer;
    public string layerForCustomGravityObject;

    public bool changeGravityObjectsEnabled = true;

    public bool launchedObjectsCanMakeNoise;
    public float minObjectSpeedToActivateNoise;

    public enum grabMode
    {
        powers,
        realistic
    }

    public bool grabbed;
    public bool gear;
    public bool rail;
    public bool regularObject;

    public List<string> ableToGrabTags = new List<string> ();

    public GameObject currentObjectToGrabFound;

    public bool grabObjectsPhysicallyEnabled = true;
    public List<handInfo> handInfoList = new List<handInfo> ();
    public Transform placeToCarryPhysicalObjectsThirdPerson;
    public Transform placeToCarryPhysicalObjectsFirstPerson;
    public bool carryingPhysicalObject;
    public float translatePhysicalObjectSpeed = 5;

    public Transform positionToKeepObject;

    public GameObject currentPhysicalObjectGrabbed;
    public GameObject currentPhysicalObjectToGrabFound;

    public GameObject currentPhysicsObjectElement;
    Coroutine droppingCoroutine;
    Coroutine translateGrabbedPhysicalObject;
    Coroutine grabbingCoroutine;

    public List<GameObject> physicalObjectToGrabFoundList = new List<GameObject> ();

    bool elementsOnPhysicalObjectsFoundList;

    public bool useForceWhenObjectDropped;
    public bool useForceWhenObjectDroppedOnFirstPerson;
    public bool useForceWhenObjectDroppedOnThirdPerson;
    public float forceWhenObjectDroppedOnFirstPerson;
    public float forceWhenObjectDroppedOnThirdPerson;

    public Text keyText;

    Vector3 currentIconPosition;
    public bool showGrabObjectIconEnabled;
    public GameObject grabObjectIcon;
    public RectTransform iconRectTransform;

    bool grabObjectIconLocated;

    bool grabObjectIconActive;

    public bool getClosestDeviceToCameraCenter;
    public bool useMaxDistanceToCameraCenter;
    public float maxDistanceToCameraCenter;

    public bool useObjectToGrabFoundShader;
    public Shader objectToGrabFoundShader;
    public float shaderOutlineWidth;
    public Color shaderOutlineColor;

    public GameObject objectHeld;

    public bool pauseCameraMouseWheelWhileObjectGrabbed;

    public string grabObjectActionName = "Grab Object";
    public string extraTextStartActionKey = "[";
    public string extraTextEndActionKey = "]";

    public GameObject touchButtonIcon;

    int currentPhysicalObjectIndex;
    float minDistanceToTarget;
    Vector3 currentPosition;
    Vector3 objectPosition;
    float currentDistanceToTarget;
    Vector3 screenPoint;
    bool deviceCloseEnoughToScreenCenter;
    Vector3 centerScreen;

    public bool objectFocus;

    Rigidbody objectHeldRigidbody;
    GameObject smoke;
    float holdTimer = 0;
    float railAngle = 0;
    float timer = 0;
    RaycastHit hit;
    Shader dropShader;

    railMechanism currentRailMechanism;

    bool grabbedObjectTagLayerStored;
    string originalGrabbedObjectTag;
    int originalGrabbedObjectLayer;

    int secondaryOriginalGrabbedObjectLayer;

    float orignalHoldDistance;

    bool currentObjectUsingTransparency;

    public bool useInfiniteStrength = true;
    public float strengthAmount;
    public bool showCurrentObjectWeight;
    public GameObject weightPanel;
    public Text currentObjectWeightText;
    public Color regularWeightTextColor;
    public Color tooHeavyWeightTextColor;

    bool weigthPanelActive;

    public playerStatsSystem playerStatsManager;
    public string strengthAmountStatName = "Strength";
    bool hasPlayerStatsManager;

    public PhysicsMaterial highFrictionMaterial;

    public bool useEventOnCheckIfDropObject;
    public eventParameters.eventToCallWithGameObject eventOnCheckIfDropObject;

    public bool useRemoteEventOnObjectsFound;
    public List<string> remoteEventNameListOnGrabObject = new List<string> ();
    public List<string> remoteEventNameListOnDropObject = new List<string> ();

    public bool canGrabVehicles = true;

    public playerController playerControllerManager;
    public otherPowers powersManager;
    public playerInputManager playerInput;
    public playerCamera playerCameraManager;
    public usingDevicesSystem usingDevicesManager;
    public playerWeaponsManager weaponsManager;
    public gravitySystem gravityManager;
    public IKSystem IKManager;
    public Collider mainCollider;
    public Transform mainCameraTransform;
    public Camera mainCamera;

    public grabbedObjectMeleeAttackSystem mainGrabbedObjectMeleeAttackSystem;

    public headTrack mainHeadTrack;

    public bool ignoreDropMeleeWeaponIfCarried;

    Transform fixedGrabedTransform;
    bool rotatingObject;
    bool usingDoor;
    RigidbodyConstraints objectHeldRigidbodyConstraints = RigidbodyConstraints.None;
    Transform objectHeldFollowTransform;

    Vector3 nextObjectHeldPosition;
    Vector3 currentObjectHeldPosition;
    float currentMaxDistanceHeld;
    public bool aiming = false;

    public bool showComponents;

    GameObject currentObjectToThrow;
    Transform currentHoldTransform;
    artificialObjectGravity currentArtificialObjectGravity;

    grabPhysicalObjectSystem currentGrabPhysicalObjectSystem;

    Vector2 axisValues;
    float currentDistanceToGrabbedObject;

    bool currentObjectWasInsideGravityRoom;

    outlineObjectSystem currentOutlineObjectSystem;

    List<Collider> grabeObjectColliderList = new List<Collider> ();

    public Transform grabbedObjectClonnedColliderTransform;
    public BoxCollider grabbedObjectClonnedCollider;

    Vector2 mainCanvasSizeDelta;
    Vector2 halfMainCanvasSizeDelta;

    Vector2 iconPosition2d;
    bool usingScreenSpaceCamera;

    bool targetOnScreen;

    float currentObjectWeight;

    grabbedObjectState currentGrabbedObjectState;

    bool objectUsesConfigurableJoint;

    public bool objectIsVehicle;

    Vector3 throwObjectDirection;
    Coroutine setFixedGrabbedTransformCoroutine;

    Transform currentReferencePosition;

    float currentGrabExtraDistance;

    float screenWidth;
    float screenHeight;

    bool carryingWeaponsPreviously;

    GameObject currentCharacterGrabbed;

    List<int> currentObjectGrabbedLayerList = new List<int> ();
    bool grabbedObjectIsRagdoll;

    public bool grabObjectsInputPaused;

    movableDoor currentMovableDoor;

    ConfigurableJoint currentConfigurableJoint;

    Transform transformParentToCarryObjectOutOfPlayerBody;
    Transform transformInternalReferenceCarryObject;
    bool carryObjectOutOfPlayerBody;

    bool IKSystemEnabledOnCurrentGrabbedObject;

    bool firstGrabbedObjectChecked;

    float lastTimeGrabbedObjectInput;

    bool carryingPhysicalObjectPreviously;
    bool grabObjectsInputDisabled;
    bool holdingLaunchInputActive;

    Coroutine grabbedObjectClonnedColliderCoroutine;

    bool useCustomMassToThrowActive;
    float currentCustomMassToThrow;
    float currentCustomObjectMassDividerOnThrow;

    bool grabbedObjectClonnedColliderTransformLocated;

    grabObjectGenericModeMountPointSystem currentGrabObjectGenericModeMountPointSystem;

    bool currentGrabObjectGenericModeMountPointSystemAssigned;

    bool checkBodyAimStateActive;

    bool isPlayerMovingOn3dWorld;

    bool isCameraTypeFree;

    public bool ignoreThrowAnimationForPhysicalObjects;

    public bool ignoreThrowAnimationForMeleePhysicalObjects;

    bool useThrowAnimationOnCharacterEnabled;

    string currentThrowAnimationName;

    bool useDelayToThrowObjectDuringAnimation;

    float delayToThrowObjectDuringAnimation;


    //Editor variables
    public bool showStrengthSettings;
    public bool showGrabPhysicalObjectsSettings;
    public bool showOutlineShaderSettings;
    public bool showEventsSettings;
    public bool showOtherSettings;
    public bool showDebugSettings;
    public bool showAllSettings;
    public bool showUISettings;


    void Start ()
    {
        orignalHoldDistance = holdDistance;

        if (powerSlider != null) {
            powerSlider.maxValue = maxThrowForce;
            powerSlider.value = maxThrowForce;
        }

        if (!useCursor && cursor != null) {
            if (cursor.activeSelf) {
                cursor.SetActive (false);
            }
        }

        mainCanvasSizeDelta = playerCameraManager.getMainCanvasSizeDelta ();
        halfMainCanvasSizeDelta = mainCanvasSizeDelta * 0.5f;
        usingScreenSpaceCamera = playerCameraManager.isUsingScreenSpaceCamera ();

        if (grabbedObjectClonnedCollider != null) {
            Physics.IgnoreCollision (mainCollider, grabbedObjectClonnedCollider, true);

            playerControllerManager.setIgnoreCollisionOnExternalColliderOnlyWithExtraColliderList (grabbedObjectClonnedCollider, true);

            grabbedObjectClonnedCollider.size = Vector3.zero;

            if (grabbedObjectClonnedCollider.enabled) {
                grabbedObjectClonnedCollider.enabled = false;
            }
        }

        grabbedObjectClonnedColliderTransformLocated = grabbedObjectClonnedColliderTransform != null;

        grabObjectIconLocated = grabObjectIcon != null;

        if (mainCamera == null) {
            mainCamera = playerCameraManager.getMainCamera ();
        }
    }

    void FixedUpdate ()
    {
        if (usedByAI) {
            return;
        }

        if (elementsOnPhysicalObjectsFoundList && physicalObjectToGrabFoundList.Count > 0) {
            screenWidth = Screen.width;
            screenHeight = Screen.height;

            int index = getClosestPhysicalObjectToGrab ();

            if (index != -1) {
                currentPhysicsObjectElement = physicalObjectToGrabFoundList [index];

                if (showGrabObjectIconEnabled) {
                    if (currentPhysicsObjectElement != null) {
                        currentIconPosition = currentPhysicsObjectElement.transform.position;

                        if (usingScreenSpaceCamera) {
                            screenPoint = mainCamera.WorldToViewportPoint (currentIconPosition);
                            targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
                        } else {
                            screenPoint = mainCamera.WorldToScreenPoint (currentIconPosition);
                            targetOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < screenWidth && screenPoint.y > 0 && screenPoint.y < screenHeight;
                        }

                        if (targetOnScreen) {

                            if (usingScreenSpaceCamera) {
                                iconPosition2d = new Vector2 ((screenPoint.x * mainCanvasSizeDelta.x) - halfMainCanvasSizeDelta.x, (screenPoint.y * mainCanvasSizeDelta.y) - halfMainCanvasSizeDelta.y);

                                iconRectTransform.anchoredPosition = iconPosition2d;
                            } else {
                                grabObjectIcon.transform.position = new Vector3 (screenPoint.x, screenPoint.y, 0);
                            }

                            if (!grabObjectIconActive) {
                                enableOrDisableIconButton (true);
                            }
                        } else {
                            if (grabObjectIconActive) {
                                enableOrDisableIconButton (false);
                            }
                        }
                    } else {
                        if (grabObjectIconActive) {
                            enableOrDisableIconButton (false);
                        }
                    }
                } else {
                    if (grabObjectIconActive) {
                        enableOrDisableIconButton (false);
                    }
                }
            } else {
                if (grabObjectIconActive) {
                    enableOrDisableIconButton (false);
                }
            }
        } else {
            if (grabObjectIconActive) {
                enableOrDisableIconButton (false);
            }
        }
    }

    void Update ()
    {
        if (usedByAI) {
            return;
        }

        if (rotatingObject) {
            axisValues = playerInput.getPlayerMouseAxis ();

            objectHeld.transform.Rotate (mainCameraTransform.up, -Mathf.Deg2Rad * rotateSpeed * axisValues.x, Space.World);
            objectHeld.transform.Rotate (mainCameraTransform.right, Mathf.Deg2Rad * rotateSpeed * axisValues.y, Space.World);
        }

        // if an object is grabbed, then move it from its original position, to the other in front of the camera
        if (objectHeld != null && !carryingPhysicalObject) {

            //get the transform for the grabbed object to follow
            currentHoldTransform = mainCameraTransform;

            if (playerCameraManager.is2_5ViewActive ()) {
                currentHoldTransform = playerCameraManager.getCurrentLookDirection2_5d ();
                holdDistance = 0;
            }

            if (playerCameraManager.useTopDownView) {
                currentHoldTransform = playerCameraManager.getCurrentLookDirectionTopDown ();

                holdDistance = 0;
            }

            currentDistanceToGrabbedObject = GKC_Utils.distance (objectHeld.transform.position, currentHoldTransform.position);

            if (!grabbed) {
                timer += Time.deltaTime;

                if ((currentDistanceToGrabbedObject <= currentMaxDistanceHeld ||
                    rail ||
                    gear ||
                    usingDoor ||
                    grabInFixedPosition) && timer > 0.5f) {

                    grabbed = true;

                    timer = 0;
                }
            }

            //if the object is not capable to move in front of the camera, because for example is being blocked for a wall, drop it
            if (currentDistanceToGrabbedObject > currentMaxDistanceHeld && grabbed && regularObject) {
                dropObject ();
            } else {
                //if the object is a cube, a turret, or anything that can move freely, set its position in front of the camera
                if (regularObject) {
                    Vector3 currentGrabObjectPositionOffset = Vector3.zero;

                    if (grabInFixedPosition) {
                        nextObjectHeldPosition = fixedGrabedTransform.position +
                        holdDistance * fixedGrabedTransform.forward;

                        currentObjectHeldPosition = objectHeld.transform.position;

                        if (useGrabbedObjectOffsetThirdPerson) {
                            if (!isFirstPersonActive ()) {
                                currentGrabObjectPositionOffset = grabbedObjectOffsetThirdPerson.x * fixedGrabedTransform.right +
                                grabbedObjectOffsetThirdPerson.y * fixedGrabedTransform.up +
                                grabbedObjectOffsetThirdPerson.z * fixedGrabedTransform.forward;
                            }
                        }
                    } else {
                        if (playerCameraManager.is2_5ViewActive ()) {
                            nextObjectHeldPosition = currentHoldTransform.position +
                            holdDistance * mainCameraTransform.forward;
                        } else {
                            nextObjectHeldPosition = currentHoldTransform.position +
                            (holdDistance + objectHeld.transform.localScale.x) * mainCameraTransform.forward;
                        }

                        if (useGrabbedObjectOffsetThirdPerson) {
                            if (!isFirstPersonActive ()) {
                                currentGrabObjectPositionOffset = grabbedObjectOffsetThirdPerson.x * currentHoldTransform.right +
                                grabbedObjectOffsetThirdPerson.y * currentHoldTransform.up +
                                grabbedObjectOffsetThirdPerson.z * currentHoldTransform.forward;
                            }
                        }

                        currentObjectHeldPosition = objectHeld.transform.position;
                    }

                    if (useGrabbedObjectOffsetThirdPerson) {
                        nextObjectHeldPosition += currentGrabObjectPositionOffset;
                    }

                    objectHeldRigidbody.linearVelocity = holdSpeed * (nextObjectHeldPosition - currentObjectHeldPosition);

                    if (!rotatingObject && ((rotateToCameraInFixedPosition && grabInFixedPosition) || (!grabInFixedPosition && rotateToCameraInFreePosition))) {
                        objectHeld.transform.rotation = Quaternion.Slerp (objectHeld.transform.rotation, mainCameraTransform.rotation, Time.deltaTime * rotationSpeed);
                    }
                }

                //else if the object is on a rail get the angle between the forward of the camera and the object forward
                if (rail) {
                    int dir = 0;

                    float newAngle = Vector3.Angle (objectHeld.transform.forward, mainCameraTransform.forward);

                    if (newAngle >= railAngle + 5) {
                        dir = -1;
                    }

                    if (newAngle <= railAngle - 5) {
                        dir = 1;
                    }

                    //if the camera aims to the object, dont move it, else move in the direction the camera is looking in the local forward and back of the object
                    if (Physics.Raycast (currentHoldTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hit, maxDistanceGrab, layer)) {
                        if (hit.transform.gameObject == objectHeld) {
                            dir = 0;
                            railAngle = Vector3.Angle (objectHeld.transform.forward, mainCameraTransform.forward);
                        }
                    }

                    if (Mathf.Abs (newAngle - railAngle) < 10) {
                        dir = 0;
                    }

                    objectHeld.transform.Translate ((dir * Time.deltaTime * currentRailMechanism.getDisplaceRailSpeed ()) * Vector3.forward);
                }

                if (gear) {
                    //else, the object is a gear, so rotate it
                    objectHeld.transform.Rotate (0, 0, 150 * Time.deltaTime);
                }

                if (usingDoor) {
                    if (currentMovableDoor != null && currentConfigurableJoint != null) {
                        float yAxis = currentConfigurableJoint.axis.y * playerInput.getPlayerMouseAxis ().y;

                        Vector3 extraYRotation = objectHeld.transform.localEulerAngles + yAxis * objectHeld.transform.up;

                        float angleY = extraYRotation.y;

                        if (angleY > 180) {
                            angleY = Mathf.Clamp (angleY, currentMovableDoor.limitXAxis.y, 360);
                        } else if (angleY > 0) {
                            angleY = Mathf.Clamp (angleY, 0, currentMovableDoor.limitXAxis.x);
                        }

                        extraYRotation = new Vector3 (extraYRotation.x, angleY, extraYRotation.z);

                        //extraYRotation += objectHeld.transform.up*yAxis;
                        Quaternion rot = Quaternion.Euler (extraYRotation);

                        objectHeld.transform.localRotation = Quaternion.Slerp (objectHeld.transform.localRotation, rot, Time.deltaTime * currentMovableDoor.rotationSpeed);
                    }
                }

                if (currentGrabMode == grabMode.powers) {
                    if (smoke != null) {
                        //activate the particles while the player is moving an object
                        smoke.transform.transform.LookAt (grabZoneTransform.position);

                        ParticleSystem smokeParticles = smoke.GetComponent<ParticleSystem> ();

                        if (smokeParticles != null) {
                            var smokeParticlesMain = smokeParticles.main;

                            smokeParticlesMain.startSpeed =
                                GKC_Utils.distance (smoke.transform.position, grabZoneTransform.position) / 2;
                        }
                    }
                }
            }
        }

        if (objectHeld != null && carryingPhysicalObject) {
            if (!grabbed) {
                timer += Time.deltaTime;

                if (timer > 0.2f) {
                    grabbed = true;
                    timer = 0;
                }
            }

            if (grabbedObjectClonnedColliderTransformLocated && grabbedObjectClonnedColliderTransform.gameObject.activeSelf) {
                grabbedObjectClonnedColliderTransform.position = currentPhysicalObjectGrabbed.transform.position;
                grabbedObjectClonnedColliderTransform.rotation = currentPhysicalObjectGrabbed.transform.rotation;
            }

            if (carryObjectOutOfPlayerBody) {
                if (transformParentToCarryObjectOutOfPlayerBody != null) {
                    transformParentToCarryObjectOutOfPlayerBody.position = placeToCarryPhysicalObjectsThirdPerson.position;
                    transformParentToCarryObjectOutOfPlayerBody.rotation = placeToCarryPhysicalObjectsThirdPerson.rotation;
                }
            }
        }

        //change cursor size to show that the player is aiming a grabbable object and set to its normal scale and get the object to hold in case the player could grab it
        if (aiming && objectHeld == null && currentPhysicalObjectToGrabFound == null && grabObjectsEnabled) {
            if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hit, maxDistanceGrab, layer)) {

                if (currentObjectToGrabFound != hit.collider.gameObject) {
                    currentObjectToGrabFound = hit.collider.gameObject;

                    if (checkTypeObject (currentObjectToGrabFound)) {
                        GameObject mainObjectFound = applyDamage.getCharacterOrVehicle (currentObjectToGrabFound);

                        if (mainObjectFound == null) {
                            grabObjectParent currentGrabObjectParent = currentObjectToGrabFound.GetComponent<grabObjectParent> ();

                            if (currentGrabObjectParent != null) {
                                mainObjectFound = currentGrabObjectParent.getObjectToGrab ();
                            } else {
                                mainObjectFound = currentObjectToGrabFound;
                            }
                        }

                        if (!physicalObjectToGrabFoundList.Contains (mainObjectFound)) {

                            checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

                            checkIfSetNewShaderToObjectToGrabFound (mainObjectFound);
                        }

                        if (!useInfiniteStrength) {

                            checkObjectWeight (mainObjectFound);
                        }

                        if (!objectFocus) {
                            enableOrDisableFoundObjectToGrabCursor (true);

                            objectFocus = true;
                        }
                    } else {
                        if (objectFocus) {
                            enableOrDisableFoundObjectToGrabCursor (false);
                            objectFocus = false;

                            checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

                            checkDisableWeightPanel ();
                        }
                    }
                }
            } else {
                if (objectFocus) {
                    enableOrDisableFoundObjectToGrabCursor (false);

                    objectFocus = false;

                    checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

                    checkDisableWeightPanel ();

                    currentObjectToGrabFound = null;
                }
            }
        }

        if (showCurrentObjectWeight && weigthPanelActive && !aiming && !currentPhysicalObjectToGrabFound) {
            checkDisableWeightPanel ();
        }

        if (aiming && !playerCameraManager.isCameraTypeFree ()) {
            if (playerCameraManager.currentLockedCameraCursor) {
                cursorRectTransform.position = playerCameraManager.currentLockedCameraCursor.position;
            }
        }

        if (holdingLaunchInputActive) {
            addForceToLaunchObject ();
        }

        if (grabbed && objectHeld == null) {
            dropObject ();

            if (holdingLaunchInputActive) {
                setPowerSliderState (false);

                holdingLaunchInputActive = false;
            }
        }
    }

    public void checkDisableWeightPanel ()
    {
        if (!useInfiniteStrength) {
            if (showCurrentObjectWeight) {
                if (weightPanel != null) {
                    if (weightPanel.activeSelf) {
                        weightPanel.SetActive (false);
                    }
                }

                weigthPanelActive = false;
            }
        }
    }

    public void checkObjectWeight (GameObject objectToCheck)
    {
        if (showCurrentObjectWeight) {
            grabObjectProperties currentGrabObjectProperties = objectToCheck.GetComponent<grabObjectProperties> ();

            if (currentGrabObjectProperties != null) {
                currentObjectWeight = currentGrabObjectProperties.getObjectWeight ();

                if (currentObjectWeight > 0) {
                    if (!weightPanel.activeSelf) {
                        weightPanel.SetActive (true);
                    }

                    currentObjectWeightText.text = currentObjectWeight + "Kg";

                    if (strengthAmount < currentObjectWeight) {
                        currentObjectWeightText.color = tooHeavyWeightTextColor;
                    } else {
                        currentObjectWeightText.color = regularWeightTextColor;
                    }

                    weigthPanelActive = true;
                } else {
                    checkDisableWeightPanel ();
                }
            } else {
                checkDisableWeightPanel ();
            }
        }
    }

    public void enableOrDisableIconButton (bool state)
    {
        if (grabObjectIconLocated) {
            if (grabObjectIcon.activeSelf != state) {
                grabObjectIcon.SetActive (state);
            }

            grabObjectIconActive = state;
        }
    }

    public void grabObject ()
    {
        if (currentPhysicalObjectToGrabFound != null && grabObjectsPhysicallyEnabled) {
            grabCurrenObject (currentPhysicalObjectToGrabFound);
        } else {
            //if the object which the player is looking, grab it
            if (Physics.Raycast (mainCameraTransform.position, mainCameraTransform.TransformDirection (Vector3.forward), out hit, maxDistanceGrab, layer) && objectFocus) {
                grabCurrenObject (hit.collider.gameObject);
            }
        }
    }

    public void grabPhysicalObjectExternally (GameObject objectToGrab)
    {
        currentPhysicalObjectToGrabFound = objectToGrab;

        grabCurrenObject (currentPhysicalObjectToGrabFound);
    }

    public void grabCurrenObject (GameObject objectToGrab)
    {
        if (objectToGrab == null) {
            print ("trying to grab destroyed object, cancelling");

            return;
        }

        if (checkTypeObject (objectToGrab)) {
            enableOrDisableFoundObjectToGrabCursor (false);

            //reset the hold distance
            holdDistance = orignalHoldDistance;

            currentGrabExtraDistance = 0;

            grabObjectProperties currentGrabObjectProperties = null;

            if (canGrabVehicles) {
                objectIsVehicle = applyDamage.isVehicle (objectToGrab);
            } else {
                objectIsVehicle = false;
            }

            bool objectIsCharacter = applyDamage.isCharacter (objectToGrab);

            if (canGrabVehicles) {
                objectHeld = applyDamage.getCharacterOrVehicle (objectToGrab);
            } else {
                objectHeld = applyDamage.getCharacter (objectToGrab);
            }

            if (objectHeld != null) {
                currentGrabObjectProperties = objectHeld.GetComponent<grabObjectProperties> ();
            }

            currentCharacterGrabbed = objectHeld;

            grabbedObjectIsRagdoll = false;

            if (objectIsVehicle || objectIsCharacter) {
                if (objectIsVehicle) {
                    Rigidbody objectToGrabRigidbody = applyDamage.applyForce (objectHeld);

                    if (objectToGrabRigidbody.isKinematic) {

                        objectHeld = null;

                        return;
                    }
                } else {
                    objectHeld = objectToGrab;

                    if (applyDamage.isCharacter (objectHeld)) {
                        Animator currentObjectAnimator = objectHeld.GetComponent<Animator> ();

                        if (currentObjectAnimator != null) {
                            objectHeld = currentObjectAnimator.GetBoneTransform (HumanBodyBones.Hips).gameObject;
                            grabbedObjectIsRagdoll = true;
                        }
                    }
                }
            } else {
                grabObjectParent currentGrabObjectParent = objectToGrab.GetComponent<grabObjectParent> ();

                if (currentGrabObjectParent != null) {
                    objectHeld = currentGrabObjectParent.getObjectToGrab ();
                } else {
                    objectHeld = objectToGrab;
                }

                currentCharacterGrabbed = objectHeld;

                currentGrabObjectProperties = objectHeld.GetComponent<grabObjectProperties> ();
            }

            if (useRemoteEventOnObjectsFound) {
                if (currentCharacterGrabbed != null) {
                    remoteEventSystem currentRemoteEventSystem = currentCharacterGrabbed.GetComponent<remoteEventSystem> ();

                    if (currentRemoteEventSystem != null) {
                        for (int i = 0; i < remoteEventNameListOnGrabObject.Count; i++) {
                            currentRemoteEventSystem.callRemoteEvent (remoteEventNameListOnGrabObject [i]);
                        }
                    }
                }
            }

            if (currentGrabObjectProperties != null) {
                currentObjectWeight = 0;

                if (!useInfiniteStrength) {
                    currentObjectWeight = currentGrabObjectProperties.getObjectWeight ();

                    if (strengthAmount < currentObjectWeight) {
                        objectHeld = null;

                        return;
                    }
                }

                if (currentGrabObjectProperties.useExtraGrabDistance) {
                    currentGrabExtraDistance = currentGrabObjectProperties.getExtraGrabDistance ();

                    holdDistance += currentGrabExtraDistance;
                }

                currentGrabObjectProperties.checkEventsOnGrabObject ();
            }

            currentGrabPhysicalObjectSystem = objectHeld.GetComponentInChildren<grabPhysicalObjectSystem> ();

            bool isUsingGenericModelActive = playerControllerManager.isUsingGenericModelActive ();

            if (isUsingGenericModelActive) {
                if (currentGrabPhysicalObjectSystem != null) {
                    if (!currentGrabPhysicalObjectSystem.isCanBeGrabbedOnGenericModeEnabled ()) {
                        objectHeld = null;

                        return;
                    }
                }
            }

            if (currentGrabPhysicalObjectSystem != null) {
                if (aiming && objectFocus &&
                    currentGrabPhysicalObjectSystem.isIgnoreToGrabPhysicallyIfPickedWithAimingModeActive ()) {

                    currentGrabPhysicalObjectSystem = null;
                }
            }

            //get its tag, to set it again to the object, when it is dropped
            if (!objectIsVehicle) {
                grabbedObjectTagLayerStored = true;
                originalGrabbedObjectTag = objectHeld.tag;

                if (grabbedObjectIsRagdoll) {
                    currentObjectGrabbedLayerList = applyDamage.getBodyColliderLayerList (currentCharacterGrabbed);

                    applyDamage.setBodyColliderLayerList (currentCharacterGrabbed, LayerMask.NameToLayer (grabbedObjectLayer));
                } else {
                    originalGrabbedObjectLayer = objectHeld.layer;
                }

                objectHeld.tag = grabbedObjectTag;
                objectHeld.layer = LayerMask.NameToLayer (grabbedObjectLayer);

                grabObjectParent currentGrabObjectParent = objectToGrab.GetComponentInChildren<grabObjectParent> ();

                if (currentGrabObjectParent != null) {
                    secondaryOriginalGrabbedObjectLayer = currentGrabObjectParent.gameObject.layer;

                    currentGrabObjectParent.gameObject.layer = LayerMask.NameToLayer (grabbedObjectLayer);
                }
            }

            objectHeldRigidbody = objectHeld.GetComponent<Rigidbody> ();

            if (objectHeldRigidbody == null) {
                if (currentGrabPhysicalObjectSystem != null) {
                    objectHeldRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
                }
            }

            if (objectHeldRigidbody != null) {
                objectHeldRigidbody.isKinematic = false;
                objectHeldRigidbody.useGravity = false;
                objectHeldRigidbody.linearVelocity = Vector3.zero;
            }

            //if the object has its gravity modified, pause that script
            currentArtificialObjectGravity = objectHeld.GetComponent<artificialObjectGravity> ();

            if (currentArtificialObjectGravity != null) {
                currentArtificialObjectGravity.setActiveState (false);
            }

            if (currentGrabObjectProperties != null) {
                currentGrabObjectProperties.checkEventToSetPlayer (gameObject);
            }

            pickUpObject currentPickUpObject = objectHeld.GetComponent<pickUpObject> ();

            if (currentPickUpObject != null) {
                currentPickUpObject.activateObjectTrigger ();
            }

            grabObjectEventSystem currentGrabObjectEventSystem = objectHeld.GetComponent<grabObjectEventSystem> ();

            if (currentGrabObjectEventSystem != null) {
                currentGrabObjectEventSystem.callEventOnGrab ();
            }

            objectToPlaceSystem currentObjectToPlaceSystem = objectHeld.GetComponent<objectToPlaceSystem> ();

            if (currentObjectToPlaceSystem != null) {
                currentObjectToPlaceSystem.setObjectInGrabbedState (true);
            }

            currentGrabbedObjectState = null;

            currentConfigurableJoint = objectHeld.GetComponent<ConfigurableJoint> ();

            objectUsesConfigurableJoint = currentConfigurableJoint != null;

            if (currentGrabMode == grabMode.powers) {
                //if the objects is a mechanism, the object is above a rail, so the player could move it only in two directions
                currentRailMechanism = objectHeld.GetComponent<railMechanism> ();

                rotatoryGear currentRotatoryGear = objectHeld.GetComponent<rotatoryGear> ();

                if (currentRailMechanism != null) {
                    railAngle = Vector3.Angle (objectHeld.transform.forward, mainCameraTransform.forward);

                    rail = true;

                    currentRailMechanism.setUsingRailState (true);

                    objectHeld.layer = originalGrabbedObjectLayer;
                }

                //if the object is a gear, it only can be rotated
                else if (currentRotatoryGear != null && currentRotatoryGear.rotationEnabled) {
                    gear = true;
                }

                //else, if it is a door
                else if (!objectUsesConfigurableJoint && objectHeldRigidbody) {
                    regularObject = true;
                    objectHeldRigidbodyConstraints = objectHeldRigidbody.constraints;
                    objectHeldRigidbody.freezeRotation = true;
                }

                currentGrabbedObjectState = objectHeld.GetComponent<grabbedObjectState> ();

                if (currentGrabbedObjectState == null) {
                    currentGrabbedObjectState = objectHeld.AddComponent<grabbedObjectState> ();
                }

                if (currentGrabbedObjectState != null) {
                    currentGrabbedObjectState.setCurrentHolder (gameObject);
                    currentGrabbedObjectState.setGrabbedState (true);
                }
            } else {
                if (!objectUsesConfigurableJoint && objectHeldRigidbody) {
                    regularObject = true;

                    if (!objectIsVehicle) {
                        objectHeldRigidbodyConstraints = objectHeldRigidbody.constraints;
                        objectHeldRigidbody.freezeRotation = true;
                    }
                }
            }

            if (objectUsesConfigurableJoint) {
                usingDoor = true;

                currentMovableDoor = objectHeld.GetComponent<movableDoor> ();

                objectHeldRigidbodyConstraints = objectHeldRigidbody.constraints;

                objectHeldRigidbody.freezeRotation = true;

                playerCameraManager.changeCameraRotationState (false);
            }

            if (currentPickUpObject != null) {
                objectHeld.transform.SetParent (null);
            }

            bool checkTransparencyResult = false;

            if (currentGrabPhysicalObjectSystem == null || !currentGrabPhysicalObjectSystem.isGrabObjectPhysicallyEnabled ()) {
                checkTransparencyResult = true;
            }

            currentObjectUsingTransparency = false;

            if (checkTransparencyResult) {
                //if the transparency is enabled, change all the color of all the materials of the object
                if (enableTransparency) {
                    outlineObjectSystem currentOutlineObjectSystem = objectHeld.GetComponentInChildren<outlineObjectSystem> ();

                    if (currentOutlineObjectSystem != null) {
                        if (pickableShader == null) {
                            pickableShader = Shader.Find (defaultShaderName);
                        }

                        currentOutlineObjectSystem.setTransparencyState (true, pickableShader, alphaTransparency);

                        currentObjectUsingTransparency = true;
                    }
                }
            }

            if (powerSlider != null) {
                powerSlider.value = 0;
            }

            holdTimer = 0;

            enableOrDisableGrabObjectCursor (false);

            if (grabbedObjectCursor != null && !grabbedObjectCursor.activeSelf) {
                grabbedObjectCursor.SetActive (true);
            }

            if (currentGrabMode == grabMode.powers) {
                if (useGrabbedParticles) {
                    //enable particles and reset some powers values
                    smoke = (GameObject)Instantiate (particles [3], objectHeld.transform.position, objectHeld.transform.rotation);

                    smoke.transform.SetParent (objectHeld.transform);

                    smoke.SetActive (true);

                    particles [0].SetActive (true);
                }
            }

            if (regularObject) {
                if (fixedGrabedTransform == null) {
                    GameObject fixedPositionObject = new GameObject ();

                    fixedGrabedTransform = fixedPositionObject.transform;

                    fixedGrabedTransform.name = "Fixed Grabbed Transform";
                }

                if (grabInFixedPosition) {
                    fixedGrabedTransform.SetParent (mainCameraTransform);

                    fixedGrabedTransform.transform.position = objectHeld.transform.position;
                    fixedGrabedTransform.transform.rotation = mainCameraTransform.rotation;

                    currentMaxDistanceHeld = GKC_Utils.distance (objectHeld.transform.position, mainCameraTransform.position) + holdDistance;

                    holdDistance = 0;
                } else {
                    currentMaxDistanceHeld = maxDistanceHeld;
                }
            }

            useThrowAnimationOnCharacterEnabled = false;

            currentObjectWasInsideGravityRoom = false;

            useDelayToThrowObjectDuringAnimation = false;

            if (grabObjectsPhysicallyEnabled && currentGrabPhysicalObjectSystem != null) {
                if (currentGrabPhysicalObjectSystem.isGrabObjectPhysicallyEnabled ()) {

                    bool canGrabObjectPhysically = false;

                    if (currentGrabPhysicalObjectSystem.checkViewToGrabObject) {
                        if (isFirstPersonActive ()) {
                            if (currentGrabPhysicalObjectSystem.isCanBeGrabbedOnFirstPersonEnabled ()) {
                                canGrabObjectPhysically = true;
                            }
                        } else {
                            if (currentGrabPhysicalObjectSystem.isCanBeGrabbedOnThirdPersonEnabled ()) {
                                canGrabObjectPhysically = true;
                            }
                        }
                    } else {
                        canGrabObjectPhysically = true;
                    }

                    bool hasObjectMeleeAttackSystem = currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ();

                    if (hasObjectMeleeAttackSystem) {
                        if (!mainGrabbedObjectMeleeAttackSystem.isCanGrabMeleeObjectsEnabled ()) {
                            canGrabObjectPhysically = false;
                        }
                    }

                    if (canGrabObjectPhysically) {
                        currentGrabPhysicalObjectSystem.grabObject (gameObject);

                        currentGrabbedObjectState.setCarryingObjectPhysicallyState (true);

                        if (hasObjectMeleeAttackSystem) {
                            mainGrabbedObjectMeleeAttackSystem.setNewGrabPhysicalObjectSystem (currentGrabPhysicalObjectSystem);

                            mainGrabbedObjectMeleeAttackSystem.setNewGrabPhysicalObjectMeleeAttackSystem (currentGrabPhysicalObjectSystem.getMainGrabPhysicalObjectMeleeAttackSystem ());
                        }

                        bool checkThrowAnimationResult = true;

                        if (ignoreThrowAnimationForPhysicalObjects) {
                            checkThrowAnimationResult = false;
                        }

                        if (hasObjectMeleeAttackSystem && ignoreThrowAnimationForMeleePhysicalObjects) {
                            checkThrowAnimationResult = false;
                        }

                        if (checkThrowAnimationResult) {
                            useThrowAnimationOnCharacterEnabled = currentGrabPhysicalObjectSystem.useThrowAnimationOnCharacterEnabled;

                            if (useThrowAnimationOnCharacterEnabled) {
                                if (currentGrabPhysicalObjectSystem.isUseRightHandActive () && currentGrabPhysicalObjectSystem.isUseLeftHandActive ()) {
                                    currentThrowAnimationName = currentGrabPhysicalObjectSystem.throwAnimationTwoHands;
                                } else {
                                    currentThrowAnimationName = currentGrabPhysicalObjectSystem.throwAnimationOneHand;
                                }

                                useDelayToThrowObjectDuringAnimation = currentGrabPhysicalObjectSystem.useDelayToThrowObjectDuringAnimation;

                                delayToThrowObjectDuringAnimation = currentGrabPhysicalObjectSystem.delayToThrowObjectDuringAnimation;
                            }
                        }
                    } else {
                        dropObject ();
                    }
                }
            }

            checkDisableWeightPanel ();
        }
    }

    void checkEnableOrDisableTransparencyOnPhysicalObject (bool state)
    {
        if (state) {
            bool checkTransparencyResult = false;

            if (currentGrabPhysicalObjectSystem == null || !currentGrabPhysicalObjectSystem.isGrabObjectPhysicallyEnabled ()) {
                checkTransparencyResult = true;
            }

            if (playerCameraManager.isFullBodyAwarenessActive ()) {
                if (currentGrabPhysicalObjectSystem != null) {
                    if (currentGrabPhysicalObjectSystem.enableTransparencyOnFBA) {
                        checkTransparencyResult = true;
                    }
                }
            }

            currentObjectUsingTransparency = false;

            if (checkTransparencyResult) {
                //if the transparency is enabled, change all the color of all the materials of the object
                if (enableTransparency) {
                    outlineObjectSystem currentOutlineObjectSystem = objectHeld.GetComponentInChildren<outlineObjectSystem> ();

                    if (currentOutlineObjectSystem != null) {
                        if (pickableShader == null) {
                            pickableShader = Shader.Find (defaultShaderName);
                        }

                        currentOutlineObjectSystem.setTransparencyState (true, pickableShader, alphaTransparency);

                        currentObjectUsingTransparency = true;
                    }
                }
            }
        } else {
            if (currentObjectUsingTransparency) {
                if (enableTransparency) {
                    //set the normal shader of the object 
                    outlineObjectSystem currentOutlineObjectSystem = objectHeld.GetComponentInChildren<outlineObjectSystem> ();

                    if (currentOutlineObjectSystem != null) {
                        currentOutlineObjectSystem.setTransparencyState (false, null, 0);
                    }
                }
            }
        }
    }

    //check if the object detected by the raycast is in the able to grab list or is a vehicle
    public bool checkTypeObject (GameObject objectToCheck)
    {
        if (ableToGrabTags.Contains (objectToCheck.tag)) {
            return true;
        }

        if (canGrabVehicles && applyDamage.isVehicle (objectToCheck)) {
            return true;
        }

        characterDamageReceiver currentCharacterDamageReceiver = objectToCheck.GetComponent<characterDamageReceiver> ();

        if (currentCharacterDamageReceiver != null) {
            if (ableToGrabTags.Contains (currentCharacterDamageReceiver.character.tag)) {
                return true;
            }
        }

        grabObjectParent currentGrabObjectParent = objectToCheck.GetComponent<grabObjectParent> ();

        if (currentGrabObjectParent != null) {
            return true;
        }

        return false;
    }

    public void checkToDropObjectIfNotPhysicalWeaponElseKeepWeapon ()
    {
        if (isCarryingMeleeWeapon ()) {
            if (mainGrabbedObjectMeleeAttackSystem != null) {
                mainGrabbedObjectMeleeAttackSystem.checkToKeepWeaponWithoutCheckingInputActive ();
            }
        } else {
            dropObject ();
        }
    }

    public void checkToDropObjectIfCarryingRegularPhysicalObject ()
    {
        if (isCarryingPhysicalObject () && !isCarryingMeleeWeapon ()) {
            dropObject ();
        }
    }

    bool ignoreDropMeleeWeaponOnDeath;

    public void setIgnoreDropMeleeWeaponOnDeathState (bool state)
    {
        ignoreDropMeleeWeaponOnDeath = state;
    }

    public void checkToDropObjectOnDeath ()
    {
        if (dropCurrentObjectOnDeathEnabled) {
            if (keepMeleeWeaponOnDeathEnabled || ignoreDropMeleeWeaponOnDeath) {
                checkToDropObjectIfNotPhysicalWeaponElseKeepWeapon ();
            } else {
                dropObject ();
            }
        }
    }

    //drop the object
    public void dropObject ()
    {
        if (carryingPhysicalObject && !grabbed) {
            return;
        }

        if (useThrowObjectsLayer) {
            throwObjectDirection = Vector3.zero;

            if (objectHeld != null) {
                bool surfaceLocated = false;

                Vector3 raycastPosition = Vector3.zero;

                if (carryingPhysicalObject) {
                    raycastPosition = mainCameraTransform.position;
                } else {
                    raycastPosition = objectHeld.transform.position;
                }

                Vector3 raycastDirection = mainCameraTransform.forward;

                if (Physics.Raycast (raycastPosition, raycastDirection, out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {

                    if (hit.collider.gameObject != objectHeld && hit.collider.gameObject != playerControllerManager.gameObject) {
                        Vector3 heading = hit.point - objectHeld.transform.position;

                        float distance = heading.magnitude;

                        throwObjectDirection = heading / distance;
                        throwObjectDirection.Normalize ();

                        surfaceLocated = true;

                    } else {
                        raycastPosition = hit.point;

                        if (Physics.Raycast (raycastPosition, raycastDirection, out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {

                            if (hit.collider.gameObject != objectHeld && hit.collider.gameObject != playerControllerManager.gameObject) {
                                Vector3 heading = hit.point - objectHeld.transform.position;

                                float distance = heading.magnitude;

                                throwObjectDirection = heading / distance;
                                throwObjectDirection.Normalize ();

                                surfaceLocated = true;

                            }
                        }
                    }
                }

                if (!surfaceLocated) {
                    throwObjectDirection = mainCameraTransform.forward;
                }
            }
        }

        if (grabbedObjectCursor != null && grabbedObjectCursor.activeSelf) {
            grabbedObjectCursor.SetActive (false);
        }

        enableOrDisableFoundObjectToGrabCursor (false);

        if (aiming) {
            enableOrDisableGrabObjectCursor (true);
        }

        if (playerCameraManager != null) {
            playerCameraManager.changeCameraRotationState (true);
        }

        rotatingObject = false;
        usingDoor = false;

        carryingPhysicalObjectPreviously = false;

        objectToPlaceSystem currentObjectToPlaceSystem = null;

        if (objectHeld != null) {
            //set the tag of the object that had before grab it, and if the object has its own gravity, enable again
            if (grabbedObjectTagLayerStored) {
                objectHeld.tag = originalGrabbedObjectTag;

                if (grabbedObjectIsRagdoll && currentObjectGrabbedLayerList.Count > 0) {
                    applyDamage.setBodyColliderLayerList (currentCharacterGrabbed, currentObjectGrabbedLayerList);
                } else {
                    objectHeld.layer = originalGrabbedObjectLayer;

                    grabObjectParent currentGrabObjectParent = objectHeld.GetComponentInChildren<grabObjectParent> ();

                    if (currentGrabObjectParent != null) {
                        currentGrabObjectParent.gameObject.layer = secondaryOriginalGrabbedObjectLayer;
                    }
                }

                grabbedObjectIsRagdoll = false;
                currentObjectGrabbedLayerList.Clear ();

                grabbedObjectTagLayerStored = false;
            }

            currentArtificialObjectGravity = objectHeld.GetComponent<artificialObjectGravity> ();

            if (currentArtificialObjectGravity != null) {
                currentArtificialObjectGravity.setActiveState (true);
            }

            if (objectHeldRigidbody != null) {
                if (!objectIsVehicle && currentArtificialObjectGravity == null) {
                    objectHeldRigidbody.useGravity = true;
                }

                if (!objectUsesConfigurableJoint) {

                    objectHeldRigidbody.freezeRotation = false;

                    if (objectHeldRigidbodyConstraints != RigidbodyConstraints.None) {
                        objectHeldRigidbody.constraints = objectHeldRigidbodyConstraints;
                        objectHeldRigidbodyConstraints = RigidbodyConstraints.None;
                    }
                }
            }

            useCustomMassToThrowActive = false;

            grabObjectProperties currentGrabObjectProperties = objectHeld.GetComponent<grabObjectProperties> ();

            if (currentGrabObjectProperties != null) {
                currentGrabObjectProperties.checkEventsOnDropObject ();

                useCustomMassToThrowActive = currentGrabObjectProperties.useCustomMassToThrow;
                currentCustomMassToThrow = currentGrabObjectProperties.customMassToThrow;
                currentCustomObjectMassDividerOnThrow = currentGrabObjectProperties.customObjectMassDividerOnThrow;
            }

            checkEnableOrDisableTransparencyOnPhysicalObject (false);

            if (useRemoteEventOnObjectsFound) {
                if (currentCharacterGrabbed != null) {
                    remoteEventSystem currentRemoteEventSystem = currentCharacterGrabbed.GetComponent<remoteEventSystem> ();

                    //				print (currentCharacterGrabbed.name);
                    if (currentRemoteEventSystem != null) {
                        for (int i = 0; i < remoteEventNameListOnDropObject.Count; i++) {
                            currentRemoteEventSystem.callRemoteEvent (remoteEventNameListOnDropObject [i]);
                        }
                    }
                }
            }

            grabbedObjectState currentGrabbedObjectState = objectHeld.GetComponent<grabbedObjectState> ();

            if (currentGrabbedObjectState != null) {

                if (currentGrabPhysicalObjectSystem != null) {
                    if (gravityManager.isPlayerInsiderGravityRoom ()) {
                        currentGrabbedObjectState.setInsideZeroGravityRoomState (true);

                        currentGrabbedObjectState.setCurrentZeroGravityRoom (gravityManager.getCurrentZeroGravityRoom ());
                    }
                }

                currentObjectWasInsideGravityRoom = currentGrabbedObjectState.isInsideZeroGravityRoom ();

                currentGrabbedObjectState.checkGravityRoomState ();

                currentGrabbedObjectState.setGrabbedState (false);

                if (!currentObjectWasInsideGravityRoom) {
                    currentGrabbedObjectState.removeGrabbedObjectComponent ();
                }
            }

            currentObjectToPlaceSystem = objectHeld.GetComponent<objectToPlaceSystem> ();

            if (currentObjectToPlaceSystem != null) {
                currentObjectToPlaceSystem.setObjectInGrabbedState (false);
            }

            railMechanism newRailMechanism = objectHeld.GetComponent<railMechanism> ();

            if (newRailMechanism != null) {
                newRailMechanism.setUsingRailState (false);
            }

            grabObjectEventSystem currentGrabObjectEventSystem = objectHeld.GetComponent<grabObjectEventSystem> ();

            if (currentGrabObjectEventSystem != null) {
                currentGrabObjectEventSystem.callEventOnDrop ();
            }

            if (currentGrabPhysicalObjectSystem != null) {
                if (currentGrabPhysicalObjectSystem.isGrabObjectPhysicallyEnabled ()) {
                    carryingPhysicalObjectPreviously = true;

                    currentGrabPhysicalObjectSystem.dropObject ();

                    if (currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                        mainGrabbedObjectMeleeAttackSystem.removeGrabPhysicalObjectMeleeAttackSystem ();
                    }
                }
            }
        }

        if (carryingPhysicalObject && objectHeld == null) {
            dropPhysicalObject ();
        }

        grabbed = false;

        objectHeld = null;
        objectHeldRigidbody = null;
        currentGrabPhysicalObjectSystem = null;

        currentObjectUsingTransparency = false;

        currentMovableDoor = null;

        currentConfigurableJoint = null;

        rail = false;

        gear = false;
        regularObject = false;

        if (particles.Length > 0) {
            if (particles [0].activeSelf) {
                particles [0].SetActive (false);
            }

            if (particles [1].activeSelf) {
                particles [1].SetActive (false);
            }
        }

        objectFocus = false;

        if (smoke != null) {
            Destroy (smoke);
        }

        checkDisableWeightPanel ();

        currentObjectToGrabFound = null;

        currentPhysicalObjectToGrabFound = null;

        if (!isPlayerMovingOn3dWorld || !isCameraTypeFree) {
            if (checkBodyAimStateActive) {
                checkBodyAimState (false);
            }
        }

        playerControllerManager.setCarryingPhysicalObjectState (false);

        if (currentObjectToPlaceSystem != null) {
            currentObjectToPlaceSystem.checkObjectsAround ();
        }
    }

    public void checkJointsInObject (GameObject objectToThrow, float force)
    {
        CharacterJoint currentCharacterJoint = objectToThrow.GetComponent<CharacterJoint> ();

        if (currentCharacterJoint != null) {
            checkJointsInObject (currentCharacterJoint.connectedBody.gameObject, force);
        } else {
            addForceToThrownRigidbody (objectToThrow, force);
        }
    }

    public void addForceToThrownRigidbody (GameObject objectToThrow, float force)
    {
        Component [] components = objectToThrow.GetComponentsInChildren (typeof (Rigidbody));

        int componentsLength = components.Length;

        for (int i = 0; i < componentsLength; i++) {

            Rigidbody child = components [i] as Rigidbody;

            if (!child.isKinematic) {
                bool canApplyForce = true;

                Collider currentChildCollider = child.GetComponent<Collider> ();

                if (currentChildCollider == null) {
                    grabObjectParent currentgrabObjectParent = child.GetComponentInChildren<grabObjectParent> ();

                    if (currentgrabObjectParent == null) {
                        canApplyForce = false;
                    }
                }

                if (canApplyForce) {
                    Vector3 forceDirection = mainCameraTransform.forward;

                    if (carryingPhysicalObjectPreviously && !isFirstPersonActive ()) {
                        if (holdTimer < minTimeToIncreaseThrowForce) {
                            forceDirection = throwObjectDirection = transform.forward;
                        }

                        carryingPhysicalObjectPreviously = false;
                    }

                    if (useThrowObjectsLayer) {
                        if (throwObjectDirection != Vector3.zero) {
                            forceDirection = throwObjectDirection;
                        }
                    }

                    if (useCustomMassToThrowActive) {
                        forceDirection *= force / (currentCustomMassToThrow * currentCustomObjectMassDividerOnThrow);

                    } else {
                        if (takeObjectMassIntoAccountOnThrowEnabled) {

                            forceDirection *= force / (child.mass * objectMassDividerOnThrow);
                        } else {
                            forceDirection *= child.mass * force;

                        }
                    }

                    child.AddForce (forceDirection, powerForceMode);

                    checkIfEnableNoiseOnCollision (objectToThrow, forceDirection.magnitude);
                }
            }
        }
    }

    public float getLastHoldTimer ()
    {
        return holdTimer;
    }

    public Vector3 getThrowDirection ()
    {
        Vector3 raycastPosition = Vector3.zero;

        throwObjectDirection = Vector3.zero;

        raycastPosition = mainCameraTransform.position;

        bool surfaceLocated = false;

        Vector3 raycastDirection = mainCameraTransform.forward;

        if (Physics.Raycast (raycastPosition, raycastDirection, out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {

            if (hit.collider.gameObject != playerControllerManager.gameObject) {
                Vector3 heading = hit.point - raycastPosition;

                float distance = heading.magnitude;

                throwObjectDirection = heading / distance;
                throwObjectDirection.Normalize ();

                surfaceLocated = true;

            } else {
                raycastPosition = hit.point;

                if (Physics.Raycast (raycastPosition, raycastDirection, out hit, Mathf.Infinity, throwObjectsLayerToCheck)) {

                    if (hit.collider.gameObject != playerControllerManager.gameObject) {
                        Vector3 heading = hit.point - raycastPosition;

                        float distance = heading.magnitude;

                        throwObjectDirection = heading / distance;
                        throwObjectDirection.Normalize ();

                        surfaceLocated = true;

                    }
                }
            }
        }

        if (!surfaceLocated) {
            throwObjectDirection = mainCameraTransform.forward;
        }

        return throwObjectDirection;
    }

    public void checkGrabbedObjectAction ()
    {
        if (currentGrabMode == grabObjects.grabMode.powers) {
            if (changeGravityObjectsEnabled) {
                GameObject grabbedObject = getGrabbedObject ();

                Rigidbody currentRigidbody = grabbedObject.GetComponent<Rigidbody> ();

                if (currentRigidbody == null) {
                    if (currentGrabPhysicalObjectSystem != null) {
                        currentRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
                    }
                }

                if (currentRigidbody != null) {
                    dropObject ();

                    //if the current object grabbed is a vehicle, enable its own gravity control component
                    if (objectIsVehicle) {
                        //						currentVehicleGravityControl.activateGravityPower (mainCameraTransform.TransformDirection (Vector3.forward), 
                        //							mainCameraTransform.TransformDirection (Vector3.right));
                    }

                    //else, it is a regular object
                    else {
                        //change the layer, because the object will use a raycast to check the new normal when a collision happens
                        grabbedObject.layer = LayerMask.NameToLayer (layerForCustomGravityObject);

                        //if the object has a regular gravity, attach the scrip and set its values
                        currentArtificialObjectGravity = grabbedObject.GetComponent<artificialObjectGravity> ();

                        if (currentArtificialObjectGravity == null) {
                            currentArtificialObjectGravity = grabbedObject.AddComponent<artificialObjectGravity> ();
                        }

                        currentArtificialObjectGravity.enableGravity (gravityObjectsLayer, highFrictionMaterial, mainCameraTransform.forward);
                    }
                }
            }
        }

        if (currentGrabMode == grabObjects.grabMode.realistic) {
            throwRealisticGrabbedObject ();
        }
    }

    void setPowerSliderState (bool state)
    {
        if (powerSlider != null) {
            if (powerSlider.gameObject.activeSelf != state) {
                powerSlider.gameObject.SetActive (state);
            }
        }

    }

    public void throwRealisticGrabbedObject ()
    {
        setPowerSliderState (false);

        Rigidbody currentRigidbody = objectHeld.GetComponent<Rigidbody> ();

        if (currentRigidbody == null) {
            if (currentGrabPhysicalObjectSystem != null) {
                currentRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
            }
        }

        bool wasRegularObject = regularObject;

        dropObject ();

        if (currentRigidbody != null && wasRegularObject) {
            Vector3 forceDirection = (currentRigidbody.mass * throwPower) * mainCameraTransform.forward;

            currentRigidbody.AddForce (forceDirection, realisticForceMode);

            checkIfEnableNoiseOnCollision (objectHeld, forceDirection.magnitude);
        }
    }

    public void changeGrabbedZoom (int zoomType)
    {
        stopResetFixedGrabedTransformPositionCoroutine ();

        if (zoomType > 0) {
            holdDistance += Time.deltaTime * zoomSpeed;
        } else {
            holdDistance -= Time.deltaTime * zoomSpeed;
        }

        if (!grabInFixedPosition) {
            if (holdDistance > maxZoomDistance) {
                holdDistance = maxZoomDistance;
            }

            if (holdDistance < minZoomDistance) {
                holdDistance = minZoomDistance;
            }
        } else {
            if (holdDistance > currentMaxDistanceHeld) {
                holdDistance = currentMaxDistanceHeld;
            }

            if (holdDistance < 0) {
                if ((holdDistance + currentMaxDistanceHeld - orignalHoldDistance) <= closestHoldDistanceInFixedPosition) {
                    holdDistance = -currentMaxDistanceHeld + orignalHoldDistance + closestHoldDistanceInFixedPosition;
                }
            }
        }
    }

    public void resetFixedGrabedTransformPosition ()
    {
        if (grabInFixedPosition) {
            stopResetFixedGrabedTransformPositionCoroutine ();

            setFixedGrabbedTransformCoroutine = StartCoroutine (resetFixedGrabedTransformPositionCoroutine ());
        }
    }

    public void stopResetFixedGrabedTransformPositionCoroutine ()
    {
        if (setFixedGrabbedTransformCoroutine != null) {
            StopCoroutine (setFixedGrabbedTransformCoroutine);
        }
    }

    IEnumerator resetFixedGrabedTransformPositionCoroutine ()
    {
        Vector3 targetPosition = (orignalHoldDistance + currentGrabExtraDistance) * Vector3.forward;

        float dist = GKC_Utils.distance (fixedGrabedTransform.position, mainCameraTransform.position + targetPosition);

        float duration = dist / zoomSpeed;

        float t = 0;

        bool targetReached = false;

        float timer = 0;

        while (!targetReached && t < 1 && fixedGrabedTransform.localPosition != targetPosition) {
            t += Time.deltaTime / duration;

            fixedGrabedTransform.localPosition = Vector3.Lerp (fixedGrabedTransform.localPosition, targetPosition, t);

            timer += Time.deltaTime;

            if (GKC_Utils.distance (fixedGrabedTransform.localPosition, targetPosition) < 0.01f || timer > duration) {
                targetReached = true;
            }

            yield return null;
        }

        t = 0;
        targetReached = false;

        while (!targetReached && t < 1 && holdDistance != 0) {
            t += Time.deltaTime / duration;
            holdDistance = Mathf.Lerp (holdDistance, 0, t);

            timer += Time.deltaTime;
            if (holdDistance < 0.001f || timer > duration) {
                targetReached = true;

                holdDistance = 0;
            }

            yield return null;
        }
    }

    public void enableOrDisableGrabObjectCursor (bool value)
    {
        if (!value) {
            if (cursorRectTransform != null) {
                cursorRectTransform.localPosition = Vector3.zero;
            }
        }
    }

    public void enableOrDisableGeneralCursor (bool state)
    {
        if (useCursor) {
            if (cursor != null && cursor.activeSelf != state) {
                cursor.SetActive (state);
            }
        }
    }

    public void enableOrDisableGeneralCursorFromExternalComponent (bool state)
    {
        enableOrDisableGeneralCursor (state);
    }

    public void enableOrDisableFoundObjectToGrabCursor (bool state)
    {
        if (foundObjectToGrabCursor != null && foundObjectToGrabCursor.activeSelf != state) {
            foundObjectToGrabCursor.SetActive (state);
        }
    }

    public void checkIfDropObject (GameObject objectToCheck)
    {
        if (objectHeld == objectToCheck) {
            dropObject ();
        }

        if (useEventOnCheckIfDropObject) {
            eventOnCheckIfDropObject.Invoke (objectToCheck);
        }
    }

    public bool checkIfObjectCanBePlaced (GameObject objectToCheck)
    {
        if (objectHeld == objectToCheck) {
            if (carryingPhysicalObject && !grabbed) {
                return false;
            }
        }

        return true;
    }

    public void checkIfDropObject ()
    {
        if (objectHeld == null || currentPhysicalObjectGrabbed != null) {
            dropObject ();
        }
    }

    public void setAimingState (bool state)
    {
        aiming = state;

        enableOrDisableGrabObjectCursor (state);

        if (aiming) {
            if (carryingPhysicalObject) {
                enableOrDisableGrabObjectCursor (false);
            }
        } else {
            if (carryingPhysicalObject) {
                if (grabbedObjectCursor != null && grabbedObjectCursor.activeSelf) {
                    grabbedObjectCursor.SetActive (false);
                }
            }

            checkIfSetOriginalShaderToPreviousObjectToGrabFound ();
        }
    }

    public GameObject getGrabbedObject ()
    {
        return objectHeld;
    }

    public bool isGrabbedObject ()
    {
        return objectHeld != null;
    }

    public void checkIfEnableNoiseOnCollision (GameObject objectToCheck, float launchSpeed)
    {
        if (launchedObjectsCanMakeNoise && launchSpeed >= minObjectSpeedToActivateNoise) {
            noiseSystem currentNoiseSystem = objectToCheck.GetComponent<noiseSystem> ();

            if (currentNoiseSystem != null) {
                currentNoiseSystem.setUseNoiseState (true);
            }
        }
    }

    public void addForceToLaunchObject ()
    {
        if (regularObject && currentGrabMode == grabMode.powers) {
            if (holdTimer > minTimeToIncreaseThrowForce) {
                //if the button is not released immediately, active the power slider
                if (!powerSlider.gameObject.activeSelf) {
                    if (useLoadThrowParticles) {
                        if (!particles [1].activeSelf) {
                            particles [1].SetActive (true);
                        }
                    }

                    setPowerSliderState (true);
                }
            }

            if (holdTimer < maxThrowForce) {
                holdTimer += Time.deltaTime * increaseThrowForceSpeed;

                powerSlider.value += Time.deltaTime * increaseThrowForceSpeed;
            }

            if (!isPlayerMovingOn3dWorld || !isCameraTypeFree) {
                if (!isCarryingMeleeWeapon ()) {
                    if (!checkBodyAimStateActive) {
                        checkBodyAimState (true);
                    }
                }
            }
        }
    }

    public void launchObject ()
    {
        setPowerSliderState (false);

        currentObjectToThrow = objectHeld;

        Rigidbody currentRigidbody = currentObjectToThrow.GetComponent<Rigidbody> ();

        if (currentRigidbody == null) {
            if (currentGrabPhysicalObjectSystem != null) {
                currentRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
            }
        }

        dropObject ();

        bool canAddForceToObjectDropped = false;

        if (currentRigidbody != null) {
            //if the button has been pressed and released quickly, drop the object, else addforce to its rigidbody
            if (!objectIsVehicle && !currentObjectWasInsideGravityRoom) {
                currentRigidbody.useGravity = true;
            }

            if (currentGrabMode == grabMode.powers) {
                if (holdTimer > minTimeToIncreaseThrowForce) {
                    launchedObjects currentLaunchedObjects = currentObjectToThrow.GetComponent<launchedObjects> ();

                    if (currentLaunchedObjects == null) {
                        currentLaunchedObjects = currentObjectToThrow.AddComponent<launchedObjects> ();
                    }

                    if (currentLaunchedObjects != null) {
                        currentLaunchedObjects.setCurrentPlayer (gameObject);
                    }

                    if (useLoadThrowParticles) {
                        GameObject launchParticles = (GameObject)Instantiate (particles [2], grabZoneTransform.position, mainCameraTransform.rotation);

                        launchParticles.transform.SetParent (null);

                        if (!launchParticles.activeSelf) {
                            launchParticles.SetActive (true);
                        }
                    }

                    if (currentObjectToThrow.GetComponent<CharacterJoint> ()) {
                        checkJointsInObject (currentObjectToThrow, holdTimer);
                    } else {
                        holdTimer += extraThorwForce;
                        addForceToThrownRigidbody (currentObjectToThrow, holdTimer);
                    }

                    if (!useDelayToThrowObjectDuringAnimation) {
                        if (useThrowAnimationOnCharacterEnabled) {
                            playerControllerManager.playerCrossFadeInFixedTime (currentThrowAnimationName);
                        }
                    }
                } else {
                    canAddForceToObjectDropped = true;
                }
            } else {
                canAddForceToObjectDropped = true;
            }
        }

        if (canAddForceToObjectDropped) {
            if (useForceWhenObjectDropped) {
                if (useForceWhenObjectDroppedOnFirstPerson && isFirstPersonActive ()) {
                    addForceToThrownRigidbody (currentObjectToThrow, forceWhenObjectDroppedOnFirstPerson);
                } else if (useForceWhenObjectDroppedOnThirdPerson && !isFirstPersonActive ()) {
                    addForceToThrownRigidbody (currentObjectToThrow, forceWhenObjectDroppedOnThirdPerson);
                }
            }
        }
    }


    IEnumerator throwAnimationCoroutine ()
    {
        playerControllerManager.playerCrossFadeInFixedTime (currentThrowAnimationName);

        objectHeld.transform.SetParent (IKManager.getRightHandMountPoint ());

        IKManager.setGrabedObjectState (false, false, null);

        WaitForSeconds delay = new WaitForSeconds (delayToThrowObjectDuringAnimation);

        yield return delay;

        launchObject ();
    }

    public bool isCurrentObjectToGrabFound (GameObject objectToCheck)
    {
        if (currentObjectToGrabFound == objectToCheck || currentPhysicalObjectToGrabFound == objectToCheck) {
            return true;
        }

        return false;
    }

    public bool objectInphysicalObjectToGrabFoundList (GameObject objectToCheck)
    {
        if (physicalObjectToGrabFoundList.Contains (objectToCheck)) {
            return true;
        }

        return false;
    }

    public void addCurrentPhysicalObjectToGrabFound (GameObject objectToGrab)
    {
        if (!physicalObjectToGrabFoundList.Contains (objectToGrab)) {
            physicalObjectToGrabFoundList.Add (objectToGrab);

            elementsOnPhysicalObjectsFoundList = true;

            //			print ("added " + objectToGrab.name);
        }
    }

    public void removeCurrentPhysicalObjectToGrabFound (GameObject objectToGrab)
    {
        if (physicalObjectToGrabFoundList.Contains (objectToGrab)) {
            physicalObjectToGrabFoundList.Remove (objectToGrab);

            if (physicalObjectToGrabFoundList.Count == 0) {
                elementsOnPhysicalObjectsFoundList = false;
            }

            if (currentObjectToGrabFound == objectToGrab) {
                currentObjectToGrabFound = null;
            }
        }

        if (physicalObjectToGrabFoundList.Count == 0) {
            checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

            currentPhysicalObjectToGrabFound = null;

            checkDisableWeightPanel ();

            enableOrDisableIconButton (false);
        }
    }

    public void clearPhysicalObjectToGrabFoundList ()
    {
        physicalObjectToGrabFoundList.Clear ();

        elementsOnPhysicalObjectsFoundList = false;

        checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

        currentPhysicalObjectToGrabFound = null;

        currentObjectToGrabFound = null;

        checkDisableWeightPanel ();
    }

    public bool physicalObjectToGrabFound ()
    {
        return currentPhysicalObjectToGrabFound != null;
    }

    public int getClosestPhysicalObjectToGrab ()
    {
        if (carryingPhysicalObject) {
            return -1;
        }

        currentPhysicalObjectIndex = -1;
        minDistanceToTarget = Mathf.Infinity;
        currentPosition = transform.position;

        centerScreen = new Vector3 (screenWidth / 2, screenHeight / 2, 0);

        bool isCameraTypeFree = playerCameraManager.isCameraTypeFree ();

        for (int i = 0; i < physicalObjectToGrabFoundList.Count; i++) {
            if (physicalObjectToGrabFoundList [i] != null) {
                objectPosition = physicalObjectToGrabFoundList [i].transform.position;

                if (getClosestDeviceToCameraCenter && isCameraTypeFree) {
                    screenPoint = mainCamera.WorldToScreenPoint (objectPosition);

                    currentDistanceToTarget = GKC_Utils.distance (screenPoint, centerScreen);

                    deviceCloseEnoughToScreenCenter = false;

                    if (useMaxDistanceToCameraCenter) {
                        if (currentDistanceToTarget < maxDistanceToCameraCenter) {
                            deviceCloseEnoughToScreenCenter = true;
                        }
                    } else {
                        deviceCloseEnoughToScreenCenter = true;
                    }

                    if (deviceCloseEnoughToScreenCenter) {
                        if (currentDistanceToTarget < minDistanceToTarget) {
                            minDistanceToTarget = currentDistanceToTarget;
                            currentPhysicalObjectIndex = i;
                        }
                    }
                } else {
                    currentDistanceToTarget = GKC_Utils.distance (objectPosition, currentPosition);

                    if (currentDistanceToTarget < minDistanceToTarget) {
                        minDistanceToTarget = currentDistanceToTarget;
                        currentPhysicalObjectIndex = i;
                    }
                }
            } else {
                physicalObjectToGrabFoundList.RemoveAt (i);
                i = 0;

                if (physicalObjectToGrabFoundList.Count == 0) {
                    elementsOnPhysicalObjectsFoundList = false;
                }
            }
        }

        if (getClosestDeviceToCameraCenter && useMaxDistanceToCameraCenter) {
            if (currentPhysicalObjectIndex == -1 && currentPhysicalObjectToGrabFound != null) {
                checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

                currentPhysicalObjectToGrabFound = null;

                checkDisableWeightPanel ();

                return -1;
            }
        }

        if (currentPhysicalObjectIndex != -1) {
            if (currentPhysicalObjectToGrabFound != physicalObjectToGrabFoundList [currentPhysicalObjectIndex]) {

                if (currentPhysicalObjectToGrabFound != null) {
                    checkIfSetOriginalShaderToPreviousObjectToGrabFound ();
                }

                currentPhysicalObjectToGrabFound = physicalObjectToGrabFoundList [currentPhysicalObjectIndex];

                setKeyText (true);

                checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

                checkIfSetNewShaderToObjectToGrabFound (currentPhysicalObjectToGrabFound);

                checkObjectWeight (currentPhysicalObjectToGrabFound);
            }
        }

        return currentPhysicalObjectIndex;
    }

    public void checkIfSetNewShaderToObjectToGrabFound (GameObject objectToCheck)
    {
        if (useObjectToGrabFoundShader) {
            //print ("new on " + objectToCheck.name);

            currentOutlineObjectSystem = objectToCheck.GetComponentInChildren<outlineObjectSystem> ();

            if (currentOutlineObjectSystem != null) {
                currentOutlineObjectSystem.setOutlineState (true, objectToGrabFoundShader, shaderOutlineWidth, shaderOutlineColor, playerControllerManager);
            }
        }
    }

    public void checkIfSetOriginalShaderToPreviousObjectToGrabFound ()
    {
        if (useObjectToGrabFoundShader && currentOutlineObjectSystem != null) {
            if (!usingDevicesManager.isCurrentDeviceToUseFound (currentOutlineObjectSystem.getMeshParent ())) {
                //print ("original");

                currentOutlineObjectSystem.setOutlineState (false, null, 0, Color.white, playerControllerManager);
            }

            currentOutlineObjectSystem = null;
        }
    }

    public void setKeyText (bool state)
    {
        //set the key text in the icon with the current action
        if (keyText != null) {
            if (playerInput.isUsingTouchControls ()) {
                if (!touchButtonIcon.activeSelf) {
                    touchButtonIcon.SetActive (true);

                    keyText.gameObject.SetActive (false);
                }
            } else {
                if (state) {
                    keyText.text = extraTextStartActionKey + playerInput.getButtonKey (grabObjectActionName) + extraTextEndActionKey;
                } else {
                    keyText.text = "";
                }
            }
        }
    }


    public bool isIKSystemEnabledOnCurrentGrabbedObject ()
    {
        return IKSystemEnabledOnCurrentGrabbedObject;
    }

    public void grabPhysicalObject (grabPhysicalObjectSystem.grabPhysicalObjectInfo newGrabPhysicalObjectInfo)
    {
        if (gameObject.activeInHierarchy) {
            if (grabbingCoroutine != null) {
                StopCoroutine (grabbingCoroutine);
            }

            grabbingCoroutine = StartCoroutine (grabPhysicalObjectCoroutine (newGrabPhysicalObjectInfo));
        } else {
            grabPhysicalObjectDirectly (newGrabPhysicalObjectInfo);
        }
    }

    IEnumerator grabPhysicalObjectCoroutine (grabPhysicalObjectSystem.grabPhysicalObjectInfo newGrabPhysicalObjectInfo)
    {
        grabPhysicalObjectDirectly (newGrabPhysicalObjectInfo);

        yield return null;
    }

    void grabPhysicalObjectDirectly (grabPhysicalObjectSystem.grabPhysicalObjectInfo newGrabPhysicalObjectInfo)
    {
        carryingPhysicalObject = true;

        carryingWeaponsPreviously = weaponsManager.isUsingWeapons ();

        weaponsManager.setCarryingPhysicalObjectState (carryingPhysicalObject);

        weaponsManager.checkIfDisableCurrentWeapon ();

        GameObject objectToGrab = newGrabPhysicalObjectInfo.objectToGrab;

        Transform objectToGrabTransform = objectToGrab.transform;

        currentPhysicalObjectGrabbed = objectToGrab;

        checkIfSetOriginalShaderToPreviousObjectToGrabFound ();

        removeCurrentPhysicalObjectToGrabFound (currentPhysicalObjectGrabbed);

        bool firstPersonActive = isFirstPersonActive ();

        if (!firstPersonActive) {
            if (powersManager.isAimingPower ()) {
                powersManager.useAimMode ();
            }

            if (grabbedObjectCursor != null && grabbedObjectCursor.activeSelf) {
                grabbedObjectCursor.SetActive (false);
            }
        }

        grabeObjectColliderList.Clear ();

        Component [] components = objectToGrab.GetComponentsInChildren (typeof (Collider));

        int componentsLength = components.Length;

        for (int i = 0; i < componentsLength; i++) {

            Collider child = components [i] as Collider;

            if (!child.isTrigger) {
                grabeObjectColliderList.Add (child);
            }
        }

        bool objectUsesExtraColliderList = currentGrabPhysicalObjectSystem.getObjectUsesExtraColliderListState ();

        if (objectUsesExtraColliderList) {
            List<Collider> newColliderList = currentGrabPhysicalObjectSystem.getExtraColliderList ();

            if (newColliderList != null && newColliderList.Count > 0) {
                int newColliderListCount = newColliderList.Count;

                for (int j = 0; j < newColliderListCount; j++) {
                    if (grabeObjectColliderList [j] != null) {
                        grabeObjectColliderList.Add (newColliderList [j]);
                    }
                }
            }
        }

        int grabeObjectColliderListCount = grabeObjectColliderList.Count;

        for (int j = 0; j < grabeObjectColliderListCount; j++) {
            if (grabeObjectColliderList [j] != null) {
                Physics.IgnoreCollision (mainCollider, grabeObjectColliderList [j], true);
                Physics.IgnoreCollision (grabeObjectColliderList [j], grabbedObjectClonnedCollider, true);

                playerControllerManager.setIgnoreCollisionOnExternalColliderOnlyWithExtraColliderList (grabeObjectColliderList [j], true);
            }
        }

        if (!firstPersonActive) {
            if (!firstGrabbedObjectChecked) {
                //				if (!currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                //				yield return new WaitForSeconds (0.22f);
                //				}

                firstGrabbedObjectChecked = true;
            }
        }

        if (!grabbedObjectClonnedColliderTransform.gameObject.activeSelf) {
            grabbedObjectClonnedColliderTransform.gameObject.SetActive (true);
        }

        grabbedObjectClonnedCollider.size = currentGrabPhysicalObjectSystem.colliderScale;
        grabbedObjectClonnedCollider.center = currentGrabPhysicalObjectSystem.colliderOffset;

        setGrabbedObjectClonnedColliderEnabledState (true);

        if (currentGrabPhysicalObjectSystem.isUseReferencePositionForEveryViewActive ()) {
            if (firstPersonActive) {
                currentReferencePosition = currentGrabPhysicalObjectSystem.getReferencePositionFirstPerson ();
            } else {
                currentReferencePosition = currentGrabPhysicalObjectSystem.getReferencePositionThirdPerson ();

                if (playerCameraManager.isFullBodyAwarenessActive ()) {
                    if (!currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                        Transform referencePositionFBA = currentGrabPhysicalObjectSystem.getReferencePositionFBA ();

                        if (referencePositionFBA != null) {
                            currentReferencePosition = referencePositionFBA;
                        }
                    }
                }
            }
        } else {
            currentReferencePosition = currentGrabPhysicalObjectSystem.getReferencePosition ();
        }

        checkEnableOrDisableTransparencyOnPhysicalObject (true);

        Transform currentPlaceToCarryPhysicalObjects = placeToCarryPhysicalObjectsThirdPerson;

        if (firstPersonActive) {
            currentPlaceToCarryPhysicalObjects = placeToCarryPhysicalObjectsFirstPerson;
        }

        carryObjectOutOfPlayerBody = currentGrabPhysicalObjectSystem.carryObjectOutOfPlayerBody;

        if (carryObjectOutOfPlayerBody && !firstPersonActive) {
            if (transformParentToCarryObjectOutOfPlayerBody == null) {
                GameObject parentToCarryObjectOutOfPlayerBodyGameObject = new GameObject ();
                transformParentToCarryObjectOutOfPlayerBody = parentToCarryObjectOutOfPlayerBodyGameObject.transform;
            }

            transformParentToCarryObjectOutOfPlayerBody.position = currentPlaceToCarryPhysicalObjects.position;
            transformParentToCarryObjectOutOfPlayerBody.rotation = currentPlaceToCarryPhysicalObjects.rotation;

            currentPlaceToCarryPhysicalObjects = transformParentToCarryObjectOutOfPlayerBody;
        } else {
            carryObjectOutOfPlayerBody = false;
        }

        bool isUsingGenericModelActive = playerControllerManager.isUsingGenericModelActive ();

        if (isUsingGenericModelActive) {
            playerControllerManager.setIgnoreCollisionOnExternalColliderOnlyWithExtraColliderList (grabbedObjectClonnedCollider, true);
        }

        IKSystemEnabledOnCurrentGrabbedObject = newGrabPhysicalObjectInfo.IKSystemEnabled;

        if (newGrabPhysicalObjectInfo.IKSystemEnabled) {
            if (isUsingGenericModelActive) {
                if (currentGrabObjectGenericModeMountPointSystemAssigned) {
                    Transform newParent = currentGrabObjectGenericModeMountPointSystem.getMountPointByName (currentGrabPhysicalObjectSystem.mountPointNameToCarryOnGenericMode);

                    if (newParent == null) {
                        newParent = currentGrabObjectGenericModeMountPointSystem.getDefaultMountPoint ();
                    }

                    if (newParent != null) {
                        currentPlaceToCarryPhysicalObjects = newParent;
                    }

                    currentReferencePosition =
                    currentGrabObjectGenericModeMountPointSystem.getReferencePositionByName (currentGrabPhysicalObjectSystem.mountPointNameToCarryOnGenericMode,
                        currentGrabPhysicalObjectSystem.getObjectName ());
                }
            } else {
                for (int j = 0; j < handInfoList.Count; j++) {

                    handInfo currentHandInfo = handInfoList [j];

                    if (currentHandInfo.IKGoal == AvatarIKGoal.RightHand) {
                        currentHandInfo.handTransform = newGrabPhysicalObjectInfo.rightHandPosition;
                        currentHandInfo.useHand = newGrabPhysicalObjectInfo.useRightHand;
                        currentHandInfo.elbowTransform = newGrabPhysicalObjectInfo.rightElbowPosition;
                        currentHandInfo.useElbow = newGrabPhysicalObjectInfo.useRightElbow;
                        currentHandInfo.grabbingHandID = newGrabPhysicalObjectInfo.rightGrabbingHandID;
                    } else {
                        currentHandInfo.handTransform = newGrabPhysicalObjectInfo.lefHandPosition;
                        currentHandInfo.useHand = newGrabPhysicalObjectInfo.useLeftHand;
                        currentHandInfo.elbowTransform = newGrabPhysicalObjectInfo.lefElbowPosition;
                        currentHandInfo.useElbow = newGrabPhysicalObjectInfo.useLeftElbow;
                        currentHandInfo.grabbingHandID = newGrabPhysicalObjectInfo.leftGrabbingHandID;
                    }

                    currentHandInfo.useAnimationGrabbingHand = newGrabPhysicalObjectInfo.useAnimationGrabbingHand;
                }
            }
        } else {
            if (!firstPersonActive) {
                if (newGrabPhysicalObjectInfo.useRightHandForObjectParent) {
                    if (newGrabPhysicalObjectInfo.rightHandPosition != null) {
                        currentPlaceToCarryPhysicalObjects = playerControllerManager.getCharacterHumanBone (HumanBodyBones.RightHand);

                        if (currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                            currentPlaceToCarryPhysicalObjects = mainGrabbedObjectMeleeAttackSystem.getRightHandMountPoint ();
                        }
                    }
                } else {
                    currentPlaceToCarryPhysicalObjects = playerControllerManager.getCharacterHumanBone (HumanBodyBones.LeftHand);

                    if (currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                        currentPlaceToCarryPhysicalObjects = mainGrabbedObjectMeleeAttackSystem.getLeftHandMountPoint ();
                    }
                }

                if (isUsingGenericModelActive) {
                    if (currentGrabObjectGenericModeMountPointSystemAssigned) {
                        Transform newParent = currentGrabObjectGenericModeMountPointSystem.getMountPointByName (currentGrabPhysicalObjectSystem.mountPointNameToCarryOnGenericMode);

                        if (newParent == null) {
                            newParent = currentGrabObjectGenericModeMountPointSystem.getDefaultMountPoint ();
                        }

                        if (newParent != null) {
                            currentPlaceToCarryPhysicalObjects = newParent;
                        }

                        currentReferencePosition =
                        currentGrabObjectGenericModeMountPointSystem.getReferencePositionByName (currentGrabPhysicalObjectSystem.mountPointNameToCarryOnGenericMode,
                            currentGrabPhysicalObjectSystem.getObjectName ());
                    }
                } else {
                    if (!currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem () && currentGrabPhysicalObjectSystem.useMountPointToKeepObject) {

                        Transform newParent = GKC_Utils.getMountPointTransformByName (currentGrabPhysicalObjectSystem.mountPointTokeepObjectName, transform);

                        if (newParent != null) {
                            currentPlaceToCarryPhysicalObjects = newParent;
                        }
                    }
                }
            }
        }

        if (!isUsingGenericModelActive) {
            IKManager.setGrabedObjectState (true, newGrabPhysicalObjectInfo.IKSystemEnabled, handInfoList);
        }

        if (firstPersonActive) {
            objectToGrabTransform.SetParent (currentPlaceToCarryPhysicalObjects);
        } else {
            objectToGrabTransform.SetParent (currentPlaceToCarryPhysicalObjects);

            if (currentReferencePosition != null) {
                objectToGrabTransform.localPosition = currentReferencePosition.localPosition;
                objectToGrabTransform.localRotation = currentReferencePosition.localRotation;
            } else {
                objectToGrabTransform.localPosition = Vector3.zero;
                objectToGrabTransform.localRotation = Quaternion.identity;
            }
        }

        currentGrabPhysicalObjectSystem.assignObjectParent (currentPlaceToCarryPhysicalObjects);

        currentGrabPhysicalObjectSystem.checkParentAssignedState ();

        if (currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
            mainGrabbedObjectMeleeAttackSystem.checkGrabbedMeleeWeaponLocalPositionRotationValues ();
        }

        if (newGrabPhysicalObjectInfo.changeAnimationSpeed) {
            playerControllerManager.setWalkSpeedValue (newGrabPhysicalObjectInfo.animationSpeed);
        }

        if (newGrabPhysicalObjectInfo.setNewMovementTreeID) {
            playerControllerManager.setPlayerStatusIDValue (newGrabPhysicalObjectInfo.newMovementTreeID);
        }

        if (newGrabPhysicalObjectInfo.applyAnimatorVelocityWithoutMoving) {
            playerControllerManager.setApplyAnimatorVelocityWithoutMovingState (true);
        }

        if (newGrabPhysicalObjectInfo.disableJumpOnGrabbedObject) {
            playerControllerManager.setJumpInputPausedState (true);
        }

        if (newGrabPhysicalObjectInfo.disableRunOnGrabbedObject) {
            playerControllerManager.setRunInputPausedState (true);
        }

        if (newGrabPhysicalObjectInfo.disableCrouchOnGrabbedObjet) {
            playerControllerManager.setCrouchInputPausedState (true);
        }

        Rigidbody grabedObjectRigidbody = objectToGrab.GetComponent<Rigidbody> ();

        if (grabedObjectRigidbody == null) {
            if (currentGrabPhysicalObjectSystem != null) {
                grabedObjectRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
            }
        }

        grabedObjectRigidbody.isKinematic = true;

        if (firstPersonActive) {
            translatePhysicalObject (currentPlaceToCarryPhysicalObjects.position);
        }

        if (gravityManager.isPlayerInsiderGravityRoom ()) {
            gravityManager.getCurrentZeroGravityRoom ().removeObjectFromRoom (objectToGrab);
        }

        if (isUsingGenericModelActive) {
            if (currentGrabPhysicalObjectSystem.setCustomObjectScaleOnGenericMode) {
                Vector3 newGrabbedObjectScale = currentGrabPhysicalObjectSystem.customObjectScaleOnGenericMode;

                if (currentGrabObjectGenericModeMountPointSystemAssigned) {
                    if (currentGrabObjectGenericModeMountPointSystem.isUseCustomGrabbedObjectScaleMultiplierActive ()) {
                        newGrabbedObjectScale *= currentGrabObjectGenericModeMountPointSystem.getCustomGrabbedObjectScaleMultiplier ();
                    }
                }

                currentGrabPhysicalObjectSystem.setCustomScale (newGrabbedObjectScale);

                grabbedObjectClonnedColliderTransform.localScale = currentGrabPhysicalObjectSystem.customObjectScaleOnGenericMode;
            }
        }

        playerControllerManager.setCarryingPhysicalObjectState (true);
    }

    public void setGrabbedObjectClonnedColliderEnabledState (bool state)
    {
        stopSetGrabbedObjectClonnedColliderEnabledStateCoroutine ();

        if (state) {
            if (gameObject.activeInHierarchy) {
                grabbedObjectClonnedColliderCoroutine = StartCoroutine (setGrabbedObjectClonnedColliderEnabledStateCoroutine ());
            } else {
                setGrabbedObjectClonnedColliderEnabledStateDirectly ();
            }
        } else {
            if (grabbedObjectClonnedCollider.enabled) {
                grabbedObjectClonnedCollider.enabled = false;
            }
        }
    }

    void stopSetGrabbedObjectClonnedColliderEnabledStateCoroutine ()
    {
        if (grabbedObjectClonnedColliderCoroutine != null) {
            StopCoroutine (grabbedObjectClonnedColliderCoroutine);
        }
    }

    IEnumerator setGrabbedObjectClonnedColliderEnabledStateCoroutine ()
    {
        if (grabbedObjectClonnedCollider != null) {
            WaitForSeconds delay = new WaitForSeconds (0.6f);

            yield return delay;

            bool setEnabledResult = true;

            if (grabbedObjectClonnedCollider.size == Vector3.zero) {
                setEnabledResult = false;
            }

            if (grabbedObjectClonnedCollider.enabled != setEnabledResult) {
                grabbedObjectClonnedCollider.enabled = setEnabledResult;
            }
        }
    }

    void setGrabbedObjectClonnedColliderEnabledStateDirectly ()
    {
        if (grabbedObjectClonnedCollider != null) {
            bool setEnabledResult = true;

            if (grabbedObjectClonnedCollider.size == Vector3.zero) {
                setEnabledResult = false;
            }

            if (grabbedObjectClonnedCollider.enabled != setEnabledResult) {
                grabbedObjectClonnedCollider.enabled = setEnabledResult;
            }
        }
    }

    public void dropPhysicalObject ()
    {
        if (grabbingCoroutine != null) {
            StopCoroutine (grabbingCoroutine);
        }

        if (droppingCoroutine != null) {
            StopCoroutine (droppingCoroutine);
        }


        bool resetGrabbedPhysicalObjectState = false;

        if (currentPhysicalObjectGrabbed != null) {
            if (currentGrabPhysicalObjectSystem != null) {
                if (currentGrabPhysicalObjectSystem.changeAnimationSpeed) {
                    playerControllerManager.setOriginalWalkSpeed ();
                }

                if (currentGrabPhysicalObjectSystem.setNewMovementTreeID) {
                    playerControllerManager.resetPlayerStatusID ();
                }

                if (currentGrabPhysicalObjectSystem.applyAnimatorVelocityWithoutMoving) {
                    playerControllerManager.setApplyAnimatorVelocityWithoutMovingState (false);
                }

                if (currentGrabPhysicalObjectSystem.disableJumpOnGrabbedObject) {
                    playerControllerManager.setJumpInputPausedState (false);
                }

                if (currentGrabPhysicalObjectSystem.disableRunOnGrabbedObject) {
                    playerControllerManager.setRunInputPausedState (false);
                }

                if (currentGrabPhysicalObjectSystem.disableCrouchOnGrabbedObjet) {
                    playerControllerManager.setCrouchInputPausedState (false);
                }

                currentGrabPhysicalObjectSystem.setOriginalScale ();
            }

            stopTranslatePhysicalObject ();

            Rigidbody grabedObjectRigidbody = currentPhysicalObjectGrabbed.GetComponent<Rigidbody> ();

            if (grabedObjectRigidbody == null) {
                if (currentGrabPhysicalObjectSystem != null) {
                    grabedObjectRigidbody = currentGrabPhysicalObjectSystem.getMainRigidbody ();
                }
            }

            grabedObjectRigidbody.isKinematic = false;

            currentPhysicalObjectGrabbed.transform.SetParent (null);

            resetGrabbedPhysicalObjectState = true;
        } else {
            if (carryingPhysicalObject) {
                resetGrabbedPhysicalObjectState = true;

                playerControllerManager.setOriginalWalkSpeed ();

                playerControllerManager.resetPlayerStatusID ();

                playerControllerManager.setApplyAnimatorVelocityWithoutMovingState (false);

                playerControllerManager.setJumpInputPausedState (false);

                playerControllerManager.setRunInputPausedState (false);
            }
        }

        if (resetGrabbedPhysicalObjectState) {

            carryingPhysicalObject = false;

            weaponsManager.setCarryingPhysicalObjectState (carryingPhysicalObject);

            IKManager.setGrabedObjectState (false, false, null);

            IKSystemEnabledOnCurrentGrabbedObject = false;

            currentPhysicalObjectGrabbed = null;

            currentReferencePosition = null;

            if (grabbedObjectClonnedColliderTransform.gameObject.activeSelf) {
                grabbedObjectClonnedColliderTransform.gameObject.SetActive (false);
            }

            grabbedObjectClonnedColliderTransform.localScale = Vector3.one;

            setGrabbedObjectClonnedColliderEnabledState (false);

            if (gameObject.activeInHierarchy) {
                droppingCoroutine = StartCoroutine (checkStateAfterDroppingPhysicalObjectCoroutine ());
            } else {
                checkStateAfterDroppingPhysicalObject ();
            }
        }
    }

    IEnumerator checkStateAfterDroppingPhysicalObjectCoroutine ()
    {
        WaitForSeconds delay = new WaitForSeconds (0.5f);

        yield return delay;

        checkStateAfterDroppingPhysicalObject ();
    }

    void checkStateAfterDroppingPhysicalObject ()
    {
        if (carryingWeaponsPreviously) {
            if (!playerControllerManager.playerIsBusy () && !playerControllerManager.isPlayerDriving ()) {
                weaponsManager.checkIfDrawSingleOrDualWeapon ();
            }
        }

        for (int j = 0; j < grabeObjectColliderList.Count; j++) {
            if (grabeObjectColliderList [j] != null) {
                Physics.IgnoreCollision (mainCollider, grabeObjectColliderList [j], false);
            }
        }
    }

    public bool isCarryingPhysicalObject ()
    {
        return carryingPhysicalObject;
    }

    public GameObject getCurrentPhysicalObjectGrabbed ()
    {
        return currentPhysicalObjectGrabbed;
    }

    public void translatePhysicalObject (Vector3 worldPosition)
    {
        stopTranslatePhysicalObject ();

        translateGrabbedPhysicalObject = StartCoroutine (translatePhysicalObjectCoroutine (worldPosition));
    }

    public void stopTranslatePhysicalObject ()
    {
        if (translateGrabbedPhysicalObject != null) {
            StopCoroutine (translateGrabbedPhysicalObject);
        }
    }

    IEnumerator translatePhysicalObjectCoroutine (Vector3 worldPosition)
    {
        float dist = GKC_Utils.distance (currentPhysicalObjectGrabbed.transform.position, worldPosition);

        Vector3 targetPosition = Vector3.zero;
        Quaternion targetRotation = Quaternion.identity;

        if (currentReferencePosition != null) {
            targetPosition = currentReferencePosition.localPosition;
            targetRotation = currentReferencePosition.localRotation;
        }

        float duration = dist / translatePhysicalObjectSpeed;
        float t = 0;

        while (t < 1 && (currentPhysicalObjectGrabbed.transform.localPosition != targetPosition || currentPhysicalObjectGrabbed.transform.localRotation != targetRotation)) {
            t += Time.deltaTime / duration;

            currentPhysicalObjectGrabbed.transform.localPosition = Vector3.Lerp (currentPhysicalObjectGrabbed.transform.localPosition, targetPosition, t);
            currentPhysicalObjectGrabbed.transform.localRotation = Quaternion.Lerp (currentPhysicalObjectGrabbed.transform.localRotation, targetRotation, t);

            yield return null;
        }
    }

    public void checkPhysicalObjectGrabbedPosition (bool firstPersonActive)
    {
        if (currentPhysicalObjectGrabbed != null) {
            bool isFullBodyAwarenessActive = playerCameraManager.isFullBodyAwarenessActive ();

            bool hasObjectMeleeAttackSystem = currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ();

            bool ignoreToAdjustPhysicalObjectPosition = false;

            if (hasObjectMeleeAttackSystem) {
                if (mainGrabbedObjectMeleeAttackSystem.isCurrentWeaponThrown ()) {
                    ignoreToAdjustPhysicalObjectPosition = true;
                }
            }

            //print (hasObjectMeleeAttackSystem + " " + ignoreToAdjustPhysicalObjectPosition + " " +
            //mainGrabbedObjectMeleeAttackSystem.isCurrentWeaponThrown ());

            Transform currentPlaceToCarryPhysicalObjects = placeToCarryPhysicalObjectsFirstPerson;

            if (!firstPersonActive) {
                if (currentGrabPhysicalObjectSystem.IKSystemEnabled) {
                    currentPlaceToCarryPhysicalObjects = placeToCarryPhysicalObjectsThirdPerson;
                } else {
                    if (currentGrabPhysicalObjectSystem.useRightHandForObjectParent) {
                        if (currentGrabPhysicalObjectSystem.rightHandPosition != null) {
                            currentPlaceToCarryPhysicalObjects = playerControllerManager.getCharacterHumanBone (HumanBodyBones.RightHand);

                            if (hasObjectMeleeAttackSystem) {
                                currentPlaceToCarryPhysicalObjects = mainGrabbedObjectMeleeAttackSystem.getRightHandMountPoint ();
                            }
                        }
                    } else {
                        currentPlaceToCarryPhysicalObjects = playerControllerManager.getCharacterHumanBone (HumanBodyBones.LeftHand);

                        if (hasObjectMeleeAttackSystem) {
                            currentPlaceToCarryPhysicalObjects = mainGrabbedObjectMeleeAttackSystem.getLeftHandMountPoint ();
                        }
                    }

                    if (!hasObjectMeleeAttackSystem && currentGrabPhysicalObjectSystem.useMountPointToKeepObject) {
                        Transform newParent = GKC_Utils.getMountPointTransformByName (currentGrabPhysicalObjectSystem.mountPointTokeepObjectName, transform);

                        if (newParent != null) {
                            currentPlaceToCarryPhysicalObjects = newParent;
                        }
                    }
                }
            }

            carryObjectOutOfPlayerBody = currentGrabPhysicalObjectSystem.carryObjectOutOfPlayerBody;

            if (carryObjectOutOfPlayerBody && !firstPersonActive) {
                if (transformParentToCarryObjectOutOfPlayerBody == null) {
                    GameObject parentToCarryObjectOutOfPlayerBodyGameObject = new GameObject ();
                    transformParentToCarryObjectOutOfPlayerBody = parentToCarryObjectOutOfPlayerBodyGameObject.transform;
                }

                transformParentToCarryObjectOutOfPlayerBody.position = currentPlaceToCarryPhysicalObjects.position;
                transformParentToCarryObjectOutOfPlayerBody.rotation = currentPlaceToCarryPhysicalObjects.rotation;

                currentPlaceToCarryPhysicalObjects = transformParentToCarryObjectOutOfPlayerBody;
            } else {
                carryObjectOutOfPlayerBody = false;
            }

            if (!ignoreToAdjustPhysicalObjectPosition) {
                if (currentGrabPhysicalObjectSystem.keepGrabbedObjectState && positionToKeepObject != null) {
                    if (currentGrabPhysicalObjectSystem.useBoneToKeepObject) {
                        Transform currentBone = null;

                        bool useMountPointToKeepObject = currentGrabPhysicalObjectSystem.useMountPointToKeepObject;

                        if (useMountPointToKeepObject) {
                            currentBone = GKC_Utils.getMountPointTransformByName (currentGrabPhysicalObjectSystem.mountPointTokeepObjectName, transform);
                        }

                        if (!useMountPointToKeepObject || currentBone == null) {
                            currentBone = mainGrabbedObjectMeleeAttackSystem.getCharacterHumanBone (currentGrabPhysicalObjectSystem.boneToKeepObject);
                        }

                        if (currentBone == null) {
                            currentBone = positionToKeepObject;
                        }

                        if (currentBone != null) {
                            currentPhysicalObjectGrabbed.transform.SetParent (currentBone);
                        }
                    } else {
                        currentPhysicalObjectGrabbed.transform.SetParent (positionToKeepObject);
                    }
                } else {
                    if (firstPersonActive) {
                        currentPhysicalObjectGrabbed.transform.SetParent (currentPlaceToCarryPhysicalObjects);
                    } else {
                        currentPhysicalObjectGrabbed.transform.SetParent (currentPlaceToCarryPhysicalObjects);
                    }
                }

                currentGrabPhysicalObjectSystem.checkNewParentAssignedState (currentPlaceToCarryPhysicalObjects);

                currentGrabPhysicalObjectSystem.assignObjectParent (currentPlaceToCarryPhysicalObjects);

                if (currentReferencePosition != null) {
                    if (currentGrabPhysicalObjectSystem.isUseReferencePositionForEveryViewActive ()) {
                        if (firstPersonActive) {
                            currentReferencePosition = currentGrabPhysicalObjectSystem.getReferencePositionFirstPerson ();
                        } else {
                            currentReferencePosition = currentGrabPhysicalObjectSystem.getReferencePositionThirdPerson ();

                            if (isFullBodyAwarenessActive) {
                                if (!currentGrabPhysicalObjectSystem.hasObjectMeleeAttackSystem ()) {
                                    Transform referencePositionFBA = currentGrabPhysicalObjectSystem.getReferencePositionFBA ();

                                    if (referencePositionFBA != null) {
                                        currentReferencePosition = referencePositionFBA;
                                    }
                                }
                            }
                        }
                    }

                    if (playerCameraManager.isFullBodyAwarenessActive ()) {
                        checkEnableOrDisableTransparencyOnPhysicalObject (true);
                    } else {
                        checkEnableOrDisableTransparencyOnPhysicalObject (false);
                    }

                    if (currentGrabPhysicalObjectSystem.keepGrabbedObjectState && currentGrabPhysicalObjectSystem.referencePositionToKeepObject != null) {
                        currentReferencePosition = currentGrabPhysicalObjectSystem.referencePositionToKeepObject;
                    }

                    Vector3 localPosition = currentReferencePosition.localPosition;
                    Quaternion localRotation = currentReferencePosition.localRotation;

                    if (hasObjectMeleeAttackSystem) {
                        if (currentGrabPhysicalObjectSystem.keepGrabbedObjectState) {
                            bool isUseReferenceToKeepObjectValuesActive = mainGrabbedObjectMeleeAttackSystem.isUseReferenceToKeepObjectValuesActive ();

                            if (isUseReferenceToKeepObjectValuesActive) {
                                localPosition = mainGrabbedObjectMeleeAttackSystem.getReferenceToKeepObjectValuesPosition ();
                                localRotation = Quaternion.Euler (mainGrabbedObjectMeleeAttackSystem.getReferenceToKeepObjectValuesEuler ());
                            } else {
                                Transform customReferencePositionToKeepObject = mainGrabbedObjectMeleeAttackSystem.getCustomReferencePositionToKeepObject ();

                                if (customReferencePositionToKeepObject != null) {
                                    currentReferencePosition = customReferencePositionToKeepObject;
                                }

                                localPosition = currentReferencePosition.localPosition;
                                localRotation = currentReferencePosition.localRotation;
                            }
                        } else {
                            bool isUseGrabbedWeaponReferenceValuesActive = mainGrabbedObjectMeleeAttackSystem.isUseGrabbedWeaponReferenceValuesActive ();

                            if (isUseGrabbedWeaponReferenceValuesActive) {
                                localPosition = mainGrabbedObjectMeleeAttackSystem.getGrabbedWeaponReferenceValuesPosition ();
                                localRotation = Quaternion.Euler (mainGrabbedObjectMeleeAttackSystem.getGrabbedWeaponReferenceValuesEuler ());
                            } else {
                                Transform customGrabbedWeaponReferencePosition = mainGrabbedObjectMeleeAttackSystem.getCustomGrabbedWeaponReferencePosition ();

                                if (customGrabbedWeaponReferencePosition != null) {
                                    currentReferencePosition = customGrabbedWeaponReferencePosition;
                                }

                                localPosition = currentReferencePosition.localPosition;
                                localRotation = currentReferencePosition.localRotation;
                            }
                        }

                        mainGrabbedObjectMeleeAttackSystem.checkEventWhenKeepingOrDrawingMeleeWeapon (currentGrabPhysicalObjectSystem.keepGrabbedObjectState);
                    }

                    currentPhysicalObjectGrabbed.transform.localPosition = localPosition;
                    currentPhysicalObjectGrabbed.transform.localRotation = localRotation;
                } else {
                    currentPhysicalObjectGrabbed.transform.localPosition = Vector3.zero;
                    currentPhysicalObjectGrabbed.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }

    public void checkIfDropObjectIfNotPhysical (bool disableAimState)
    {
        if (disableAimState) {
            setAimingState (false);
        }

        if (!isCarryingPhysicalObject ()) {
            dropObject ();
        }
    }

    public void checkIfDropObjectIfPhysical ()
    {
        if (isCarryingPhysicalObject ()) {
            dropObject ();
        }
    }

    public bool isPauseCameraMouseWheelWhileObjectGrabbedActive ()
    {
        return pauseCameraMouseWheelWhileObjectGrabbed;
    }

    public void setGrabObjectsEnabledState (bool state)
    {
        grabObjectsEnabled = state;
    }

    public bool isGrabObjectsEnabled ()
    {
        return grabObjectsEnabled;
    }

    public void setGrabObjectsPhysicallyEnabledState (bool state)
    {
        grabObjectsPhysicallyEnabled = state;
    }

    public bool isGrabObjectsPhysicallyEnabled ()
    {
        return grabObjectsPhysicallyEnabled;
    }

    public void setGrabObjectsInputPausedState (bool state)
    {
        grabObjectsInputPaused = state;
    }

    public void setGrabObjectsInputDisabledState (bool state)
    {
        grabObjectsInputDisabled = state;
    }

    public bool isCarryingMeleeWeapon ()
    {
        if (mainGrabbedObjectMeleeAttackSystem != null) {
            return mainGrabbedObjectMeleeAttackSystem.isCarryingObject ();
        }

        return false;
    }

    public bool isCarryingRegularPhysicalObject ()
    {
        if (isCarryingPhysicalObject ()) {
            if (mainGrabbedObjectMeleeAttackSystem != null) {
                if (!mainGrabbedObjectMeleeAttackSystem.isCarryingObject ()) {
                    return true;
                }
            } else {
                return true;
            }
        }

        return false;
    }

    public void activateReturnProjectilesOnContact ()
    {
        if (mainGrabbedObjectMeleeAttackSystem != null) {
            mainGrabbedObjectMeleeAttackSystem.activateReturnProjectilesOnContact ();
        }
    }

    public void keepOrCarryGrabbebObject (bool keepGrabbedObject)
    {
        if (objectHeld != null) {
            if (carryingPhysicalObject) {
                currentGrabPhysicalObjectSystem.setKeepOrCarryGrabbebObjectState (keepGrabbedObject);

                checkPhysicalObjectGrabbedPosition (isFirstPersonActive ());
            } else {

            }
        }
    }

    public bool isFirstPersonActive ()
    {
        return playerCameraManager.isFirstPersonActive ();
    }

    bool canUseInput ()
    {
        if (playerControllerManager.iscloseCombatAttackInProcess ()) {
            return false;
        }

        if (playerControllerManager.isUsingGenericModelActive ()) {
            if (currentGrabObjectGenericModeMountPointSystemAssigned) {
                return currentGrabObjectGenericModeMountPointSystem.isGrabObjectsEnabled ();
            } else {
                return false;
            }
        }

        return true;
    }

    public void setCurrentGrabObjectGenericModeMountPointSystem (grabObjectGenericModeMountPointSystem newGrabObjectGenericModeMountPointSystem)
    {
        currentGrabObjectGenericModeMountPointSystem = newGrabObjectGenericModeMountPointSystem;

        currentGrabObjectGenericModeMountPointSystemAssigned = currentGrabObjectGenericModeMountPointSystem != null;
    }

    //CALL INPUT FUNCTIONS
    public void inputGrabObject ()
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (grabObjectsInputDisabled) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (ignoreDropMeleeWeaponIfCarried) {
            if (isCarryingMeleeWeapon ()) {
                return;
            }
        }

        //if the player is in aim mode, grab an object
        if (!playerControllerManager.isUsingDevice () &&
            (aiming || currentPhysicalObjectToGrabFound != null) &&
            objectHeld == null &&
            grabObjectsEnabled) {

            holdingLaunchInputActive = false;

            grabObject ();

            lastTimeGrabbedObjectInput = Time.time;
        }
    }

    public void inputHoldToLaunchObject ()
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (grabObjectsInputDisabled) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        //if the drop button is being holding, add force to the final velocity of the drooped object
        if (grabbed) {
            if (ignoreDropMeleeWeaponIfCarried) {
                if (isCarryingMeleeWeapon ()) {
                    return;
                }
            }

            if (Time.time < 0.3f + lastTimeGrabbedObjectInput) {
                return;
            }

            holdingLaunchInputActive = true;

            isPlayerMovingOn3dWorld = playerControllerManager.isPlayerMovingOn3dWorld ();

            isCameraTypeFree = playerCameraManager.isCameraTypeFree ();
        }
    }

    public void inputReleaseToLaunchObject ()
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (grabObjectsInputDisabled) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        //when the button is released, check the amount of strength accumulated
        if (grabbed) {
            if (Time.time < 0.3f + lastTimeGrabbedObjectInput) {
                return;
            }

            holdingLaunchInputActive = false;

            if (ignoreDropMeleeWeaponIfCarried) {
                if (isCarryingMeleeWeapon ()) {
                    return;
                }
            }

            bool useRegularLaunchResult = true;

            if (useDelayToThrowObjectDuringAnimation && useThrowAnimationOnCharacterEnabled) {
                if (holdTimer > minTimeToIncreaseThrowForce) {
                    useRegularLaunchResult = false;

                    StartCoroutine (throwAnimationCoroutine ());
                }
            }

            if (useRegularLaunchResult) {
                launchObject ();
            }
        }
    }

    public void inputSetRotateObjectState (bool rotationEnabled)
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (objectCanBeRotated && objectHeld != null && !usingDoor && !carryingPhysicalObject) {
            if (rotationEnabled) {
                playerCameraManager.changeCameraRotationState (false);

                rotatingObject = true;
            } else {
                playerCameraManager.changeCameraRotationState (true);

                rotatingObject = false;
            }
        }
    }

    public void inputZoomObject (bool zoomIn)
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (objectHeld != null && regularObject && canUseZoomWhileGrabbed) {
            if (zoomIn) {
                changeGrabbedZoom (1);
            } else {
                changeGrabbedZoom (-1);
            }
        }
    }

    public void inputResetFixedGrabedTransformPosition ()
    {
        if (grabObjectsInputPaused) {
            return;
        }

        if (!canUseInput ()) {
            return;
        }

        if (objectHeld != null && regularObject && canUseZoomWhileGrabbed) {
            resetFixedGrabedTransformPosition ();
        }
    }

    public void initializeStrengthAmount (float newValue)
    {
        strengthAmount = newValue;
    }

    public void increaseStrengthAmount (float newValue)
    {
        strengthAmount += newValue;
    }

    public void updateStrengthAmountWithoutUpdatingStatManager (int statId, float amount)
    {
        strengthAmount = amount;
    }

    public void increaseStrengthAmountAndUpdateStat (float newValue)
    {
        increaseStrengthAmount (newValue);

        if (!hasPlayerStatsManager) {
            if (playerStatsManager != null) {
                hasPlayerStatsManager = true;
            }
        }

        if (hasPlayerStatsManager) {
            playerStatsManager.updateStatValue (strengthAmountStatName, strengthAmount);
        }
    }

    public void setUseInfiniteStrengthState (bool state)
    {
        useInfiniteStrength = state;
    }

    public void setUsedByAIState (bool state)
    {
        usedByAI = state;
    }

    public void setUseObjectToGrabFoundShaderState (bool state)
    {
        useObjectToGrabFoundShader = state;
    }

    void checkBodyAimState (bool state)
    {
        if (usedByAI) {
            return;
        }

        if (mainHeadTrack == null) {
            mainHeadTrack = GKC_Utils.getCharacterHeadTrack (playerControllerManager.gameObject);
        }

        if (mainHeadTrack == null) {
            return;
        }

        if (checkBodyAimStateActive == state) {
            return;
        }

        checkBodyAimStateActive = state;

        if (state) {
            mainHeadTrack.setCameraBodyWeightValue (headTrackBodyWeightOnAim);
        } else {
            mainHeadTrack.setOriginalCameraBodyWeightValue ();
        }

        mainHeadTrack.setHeadTrackActiveWhileAimingState (state);

        mainHeadTrack.setCanUseHeadTrackOnLockedCameraActiveState (state);

        playerControllerManager.enableOrDisableAiminig (state);

        playerCameraManager.setManualAimStateOnLockedCamera (state);

        if (state) {
            playerCameraManager.enableCustomReticle (customAimReticleName);
        } else {
            playerCameraManager.disableAllCustomReticle ();
        }
    }

    //EDITOR FUNCTIONS
    public void setUseObjectToGrabFoundShaderStateFromEditor (bool state)
    {
        setUseObjectToGrabFoundShaderState (state);

        updateComponent ();
    }

    public void setGrabObjectsEnabledStateFromEditor (bool state)
    {
        setGrabObjectsEnabledState (state);

        updateComponent ();
    }

    public void updateComponent ()
    {
        GKC_Utils.updateComponent (this);

        GKC_Utils.updateDirtyScene ("Update Player Culling System", gameObject);
    }

    [System.Serializable]
    public class handInfo
    {
        public string Name;
        public AvatarIKGoal IKGoal;
        public AvatarIKHint IKHint;

        public float currentHandWeight;
        public float weightLerpSpeed;

        public bool useHand;

        public Transform handTransform;

        public Vector3 handPosition;
        public Quaternion handRotation;

        public bool useElbow;

        public Transform elbowTransform;

        public Vector3 elbowPosition;

        public bool useAnimationGrabbingHand;
        public int grabbingHandID;
    }
}