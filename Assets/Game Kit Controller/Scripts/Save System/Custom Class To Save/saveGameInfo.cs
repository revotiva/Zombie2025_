﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveGameInfo : MonoBehaviour
{
    [Header ("Main Settings")]
    [Space]

    public bool gameSavedOnHomeMenu;

    [Space]

    public bool ignorePlayerIDOnSaveLoadInfo;
    public bool ignoreSaveNumberOnSaveLoadInfo;

    public int saveNumberToIgnoreSaveLoadInfo = -1;

    [Space]
    [Space]

    public bool saveInfoOnRegularTxtFile;

    public string saveInfoOnRegulaFileName;
    public string loadInfoOnRegulaFileName;


    public virtual void saveGame (int saveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo, bool savingGameToChangeScene)
    {

    }

    public virtual void loadGame (int saveNumberToLoad, int playerID, string currentSaveDataPath, bool showDebugInfo)
    {

    }

    public virtual void saveGameFromEditor (int saveNumber, int playerID, string currentSaveDataPath, bool showDebugInfo)
    {

    }

    public virtual void initializeValuesOnComponent ()
    {

    }

    public virtual void deleteGameInfo (int saveNumberToDelete, int playerID, string currentSaveDataPath, bool showDebugInfo)
    {

    }

    public virtual void copyGameInfo (int saveNumberToStoreDuplicate, int saveNumberToDuplicate, int playerID,
        string currentSaveDataPath, bool copyAllInfoWithoutChecks, bool showDebugInfo)
    {

    }

    public virtual void showSaveInfoDebug (int saveNumberToShow, int playerID, string currentSaveDataPath,
        bool showAllGameInfoDebug, bool showDebugInfo)
    {

    }

    public virtual void checkSaveInfoToLoadOnStartOnAllScenes (int saveNumberToLoad, int playerID,
        string currentSaveDataPath, bool showDebugInfo)
    {

    }
}
