using UnityEngine;
using NaughtyAttributes;

public static class StaticVariables
{
    //Variables

    public static bool isDraggingDoor = false;
    public static bool isLookingAroundDoor = false;
    public static bool isInventoryActive = false;
    public static bool isPauseMenuActive = false;
    public static bool isContainerInventory = false;
    public static bool isGunADS = false;
    public static bool playerHasGun = false;
    public static bool isCombining = false;
    public static bool hasPhone = false;
    public static CursorMode cursorMode = CursorMode.Normal;

    //Functions
}

public enum CursorMode 
{
    Normal,
    Hover,
    Using,
    Moving,
    CantUse,
    CantDragThere,
    Drop
}
