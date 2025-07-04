﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dropPickUpSystem : MonoBehaviour
{
	public bool dropPickupsEnabled = true;

	public bool useCustomTransformPositionToSpawn;

	public Transform customTransformPositionToSpawn;

	public List<dropPickUpElementInfo> dropPickUpList = new List<dropPickUpElementInfo> ();
	public List<pickUpElementInfo> managerPickUpList = new List<pickUpElementInfo> ();
	public float dropDelay;
	public bool destroyAfterDropping;
	public float pickUpScale;
	public bool setPickupScale;
	public bool randomContent;
	public float maxRadiusToInstantiate = 1;
	public Vector3 pickUpOffset;

	public bool dropRandomObject;

	public float extraForceToPickup = 5;
	public float extraForceToPickupRadius = 5;
	public ForceMode forceMode = ForceMode.Impulse;

	public string mainPickupManagerName = "Pickup Manager";

	public string mainInventoryManagerName = "Main Inventory Manager";

	public bool showGizmo;

	public bool useGeneralProbabilityDropObjects;
	[Range (0, 100)] public float generalProbabilityDropObjects;

	public bool useEventToSendSpawnedObjects;
	public eventParameters.eventToCallWithGameObject eventToSendSpawnedObjects;

	public bool ignoreCreateDropPickupObjectsActive;


	GameObject newObject;
	pickUpManager mainPickupManager;

	GameObject objectToInstantiate;

	inventoryListManager mainInventoryListManager;

	bool mainInventoryManagerFound;

	Vector3 targetPosition;
	Quaternion targetRotation;


	public void setIgnoreCreateDropPickupObjectsActiveState (bool state)
	{
		ignoreCreateDropPickupObjectsActive = state;
	}

	//instantiate the objects in the enemy position, setting their configuration
	public void createDropPickUpObjects ()
	{
		if (!dropPickupsEnabled) {
			return;
		}

		if (ignoreCreateDropPickupObjectsActive) {
			return;
		}

		if (!gameObject.activeInHierarchy) {
			return;
		}

		mainCoroutine = StartCoroutine (createDropPickUpObjectsCoroutine ());
	}

	Coroutine mainCoroutine;

	void stopMainCoroutine ()
	{
		if (mainCoroutine != null) {
			StopCoroutine (mainCoroutine);
		}
	}

	IEnumerator createDropPickUpObjectsCoroutine ()
	{
		WaitForSeconds delay = new WaitForSeconds (dropDelay);

		yield return delay;

		if (useCustomTransformPositionToSpawn) {
			targetPosition = customTransformPositionToSpawn.position;

			targetRotation = customTransformPositionToSpawn.rotation;
		} else {
			targetPosition = transform.position;

			targetRotation = transform.rotation;
		}

		targetPosition += getOffset ();

		bool checkDropListResult = true;

		if (dropRandomObject) {
			int randomCategoryIndex = Random.Range (0, managerPickUpList.Count); 

			int nameIndex = Random.Range (0, managerPickUpList [randomCategoryIndex].pickUpTypeList.Count);
	
			objectToInstantiate = managerPickUpList [randomCategoryIndex].pickUpTypeList [nameIndex].pickUpObject;

			instantiateObject (1, 1, 0, 0);

			stopMainCoroutine ();

			checkDropListResult = false;
		}

		if (checkDropListResult) {
			int dropPickUpListCount = dropPickUpList.Count;

			int managerPickUpListCount = managerPickUpList.Count;

			for (int i = 0; i < dropPickUpListCount; i++) {
				dropPickUpElementInfo categoryList = dropPickUpList [i];

				int typeIndex = categoryList.typeIndex;

				int dropPickUpTypeListCount = categoryList.dropPickUpTypeList.Count;

				bool dropRandomObjectFromList = categoryList.dropRandomObjectFromList;

				for (int k = 0; k < dropPickUpTypeListCount; k++) {
					dropPickUpTypeElementInfo pickupTypeList = categoryList.dropPickUpTypeList [k];

					int nameIndex = pickupTypeList.nameIndex;

					//of every object, create the amount set in the inspector, the ammo and the inventory objects will be added in future updates
					int maxAmount = pickupTypeList.amount;
					int quantity = pickupTypeList.quantity;

					if (randomContent) {
						maxAmount = (int)Random.Range (pickupTypeList.amountLimits.x, pickupTypeList.amountLimits.y);
					}

					if (typeIndex < managerPickUpListCount && nameIndex < managerPickUpList [typeIndex].pickUpTypeList.Count) {
						if (pickupTypeList.dropCraftingBlueprintIngredients) {
							checkMainInventoryManager ();

							List<int> amountList = new List<int> ();

							List<GameObject> objectList = 
								mainInventoryListManager.getDisassemblePiecesOfCraftingObjectByName (pickupTypeList.blueprintName, ref amountList);

							if (objectList != null) {
								int objectListCount = objectList.Count;

								for (int l = 0; l < objectListCount; l++) {
									bool canSpawnObjectResult = true;

									if (useGeneralProbabilityDropObjects) {
										if (!pickupTypeList.ignoreProbabilityDropObjects) {
											float randomProbability = Random.Range (0, 100);

											if (pickupTypeList.useCustomProbabilityDropObjects) {
												if (randomProbability < pickupTypeList.customProbabilityDropObjects) {
													canSpawnObjectResult = false;
												}
											} else {

												if (randomProbability < generalProbabilityDropObjects) {
													canSpawnObjectResult = false;
												}
											}
										}
									}

									if (canSpawnObjectResult) {
										objectToInstantiate = objectList [l];

										instantiateObject (pickupTypeList.amountMultiplier, amountList [l], (int)pickupTypeList.quantityLimits.x, (int)pickupTypeList.quantityLimits.y);
									}
								}

							}
						} else {
							bool canSpawnObjectResult = true;

							if (useGeneralProbabilityDropObjects) {
								if (!pickupTypeList.ignoreProbabilityDropObjects) {
									float randomProbability = Random.Range (0, 100);

									if (pickupTypeList.useCustomProbabilityDropObjects) {
										if (randomProbability < pickupTypeList.customProbabilityDropObjects) {
											canSpawnObjectResult = false;
										}
									} else {

										if (randomProbability < generalProbabilityDropObjects) {
											canSpawnObjectResult = false;
										}
									}
								}
							}

							if (canSpawnObjectResult) {
								if (dropRandomObjectFromList) {
									int randomElementIndex = Random.Range (0, categoryList.dropPickUpTypeList.Count);

									nameIndex = categoryList.dropPickUpTypeList [randomElementIndex].nameIndex;

									maxAmount = 1;
									quantity = 1;
								}

								if (pickupTypeList.dropRandomObject) {
									nameIndex = Random.Range (0, managerPickUpList [typeIndex].pickUpTypeList.Count);

									maxAmount = 1;
									quantity = 1;
								}

								objectToInstantiate = managerPickUpList [typeIndex].pickUpTypeList [nameIndex].pickUpObject;

								instantiateObject (maxAmount, quantity, (int)pickupTypeList.quantityLimits.x, (int)pickupTypeList.quantityLimits.y);

								if (dropRandomObjectFromList) {
									stopMainCoroutine ();
								}
							}
						}
					}
				}
			}

			if (destroyAfterDropping) {
				Destroy (gameObject);
			}
		}
	}

	void instantiateObject (int maxAmount, int quantity, int quantityLimitsX, int quantityLimitsY)
	{
		bool objectToInstantiateFound = false;

		if (objectToInstantiate != null) {

			for (int j = 0; j < maxAmount; j++) {
				if (randomContent) {
					quantity = (int)Random.Range (quantityLimitsX, quantityLimitsY);
				}

				newObject = (GameObject)Instantiate (objectToInstantiate, targetPosition, targetRotation);

				newObject.name = objectToInstantiate.name;

				pickUpObject currentPickUpObject = newObject.GetComponent<pickUpObject> ();

				if (currentPickUpObject != null) {
					currentPickUpObject.amount = quantity;
				}

				if (setPickupScale) {
					newObject.transform.localScale = pickUpScale * Vector3.one;
				}

				//set a random position  and rotation close to the enemy position
				newObject.transform.position += maxRadiusToInstantiate * Random.insideUnitSphere;

				//apply force to the objects
				Rigidbody currentRigidbody = newObject.GetComponent<Rigidbody> ();

				Vector3 forcePosition = transform.position;

				if (useCustomTransformPositionToSpawn) {
					forcePosition = customTransformPositionToSpawn.position;
				}

				if (currentRigidbody != null) {
					currentRigidbody.AddExplosionForce (extraForceToPickup, forcePosition, extraForceToPickupRadius, 1, forceMode);
				}

				if (useEventToSendSpawnedObjects) {
					eventToSendSpawnedObjects.Invoke (newObject);
				}
			}

			objectToInstantiateFound = true;
		}

		if (!objectToInstantiateFound) {
			print ("Warning, the pickups haven't been configured correctly in the pickup manager inspector");
		}
	}

	public void setDropPickupsEnabledState (bool state)
	{
		dropPickupsEnabled = state;
    }


    public void getManagerPickUpList ()
	{
		if (mainPickupManager == null) {
			GKC_Utils.instantiateMainManagerOnSceneWithType (mainPickupManagerName, typeof(pickUpManager));

			mainPickupManager = FindObjectOfType<pickUpManager> ();
		} 

		if (mainPickupManager != null) {
			managerPickUpList.Clear ();

			for (int i = 0; i < mainPickupManager.mainPickUpList.Count; i++) {	
				managerPickUpList.Add (mainPickupManager.mainPickUpList [i]);
			}

			updateComponent ();

			print ("Updating Pickup Manager Info");
		}
	}

	public void updateComponent ()
	{
		GKC_Utils.updateComponent (this);

		GKC_Utils.updateDirtyScene ("Update drop pickups system", gameObject);
	}

	public Vector3 getOffset ()
	{
		if (useCustomTransformPositionToSpawn) {
			return (pickUpOffset.x * customTransformPositionToSpawn.right +
			pickUpOffset.y * customTransformPositionToSpawn.up +
			pickUpOffset.z * customTransformPositionToSpawn.forward);
		} else {
			return (pickUpOffset.x * transform.right + pickUpOffset.y * transform.up + pickUpOffset.z * transform.forward);
		}
	}

	void checkMainInventoryManager ()
	{
		if (!mainInventoryManagerFound) {
			getMainInventoryManager ();
		}
	}

	public void getMainInventoryManager ()
	{
		if (mainInventoryListManager == null) {
			mainInventoryManagerFound = mainInventoryListManager != null;

			if (!mainInventoryManagerFound) {
				mainInventoryListManager = inventoryListManager.Instance;

				mainInventoryManagerFound = mainInventoryListManager != null;
			}

			if (!mainInventoryManagerFound) {
				GKC_Utils.instantiateMainManagerOnSceneWithTypeOnApplicationPlaying (inventoryListManager.getMainManagerName (), typeof(inventoryListManager), true);

				mainInventoryListManager = inventoryListManager.Instance;

				mainInventoryManagerFound = mainInventoryListManager != null;
			}

			if (!mainInventoryManagerFound) {

				mainInventoryListManager = FindObjectOfType<inventoryListManager> ();

				mainInventoryManagerFound = mainInventoryListManager != null;
			} 

		} else {
			print ("Main Inventory Manager is already on the scene");
		}
	}

    public void setDropPickupsEnabledStateFromEditor (bool state)
    {
		setDropPickupsEnabledState (state);

		updateComponent ();
    }

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
		if (!Application.isPlaying && showGizmo) {
			Gizmos.color = Color.green;

			if (useCustomTransformPositionToSpawn) {
				Gizmos.DrawWireSphere (customTransformPositionToSpawn.position + getOffset (), maxRadiusToInstantiate);
			} else {
				Gizmos.DrawWireSphere (transform.position + getOffset (), maxRadiusToInstantiate);
			}
		}
	}

	[System.Serializable]
	public class dropPickUpElementInfo
	{
		public string pickUpType;
		public int typeIndex;
		public List<dropPickUpTypeElementInfo> dropPickUpTypeList = new List<dropPickUpTypeElementInfo> ();

		public bool dropRandomObjectFromList;
	}

	[System.Serializable]
	public class dropPickUpTypeElementInfo
	{
		public string name;
		public int amount;
		public int quantity;
		public Vector2 amountLimits;
		public Vector2 quantityLimits;
		public int nameIndex;

		public bool dropRandomObject;

		public bool dropCraftingBlueprintIngredients;
		public string blueprintName;
		public int amountMultiplier = 1;

		public bool useCustomProbabilityDropObjects;

		[Range (0, 100)]public float customProbabilityDropObjects;

		public bool ignoreProbabilityDropObjects;
	}
}