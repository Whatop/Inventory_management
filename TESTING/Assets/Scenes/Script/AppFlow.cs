using UnityEngine;

public enum MainObjectMode
{
    Inventory,
    DeliveryAdmin,
    Driver
}

public class AppFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainScene;
    public GameObject deliveryListPanel;
    public GameObject deliveryCreatePanel;
    public GameObject driverHomePanel;
    public GameObject allScene;

    [Header("Shared")]
    [Tooltip("인벤토리/배달/기사 등 공용 루트(필요 시만 켜기)")]
    public GameObject mainObject;
    public GameObject interactionUI;

    [Header("MainObject Mode Groups (Optional)")]
    public GameObject inventoryModeRoot;
    public GameObject deliveryModeRoot;
    public GameObject driverModeRoot;

    [Header("Service")]
    public DeliveryService service;

    void Start()
    {
        ShowMain();
    }

    void HideAll()
    {
        if (mainScene) mainScene.SetActive(false);
        if (deliveryListPanel) deliveryListPanel.SetActive(false);
        if (deliveryCreatePanel) deliveryCreatePanel.SetActive(false);
        if (driverHomePanel) driverHomePanel.SetActive(false);
        if (allScene) allScene.SetActive(false);
    }

    public void SetMainObjectMode(MainObjectMode mode)
    {
        if (mainObject) mainObject.SetActive(true);

        if (inventoryModeRoot) inventoryModeRoot.SetActive(mode == MainObjectMode.Inventory);
        if (deliveryModeRoot) deliveryModeRoot.SetActive(mode == MainObjectMode.DeliveryAdmin);
        if (driverModeRoot) driverModeRoot.SetActive(mode == MainObjectMode.Driver);
    }

    void SetMainObjectActive(bool on)
    {
        if (mainObject) mainObject.SetActive(on);
        if (!on)
        {
            if (inventoryModeRoot) inventoryModeRoot.SetActive(false);
            if (deliveryModeRoot) deliveryModeRoot.SetActive(false);
            if (driverModeRoot) driverModeRoot.SetActive(false);
        }
    }

    // ----------------------------
    // Canonical
    // ----------------------------
    public void ShowMain()
    {
        HideAll();
        if (mainScene) mainScene.SetActive(true);

        // 메인(선택) 화면에서는 공용 루트 숨김
        SetMainObjectActive(false);
    }

    public void ShowAll()
    {
        HideAll();
        if (allScene) allScene.SetActive(true);
        SetMainObjectMode(MainObjectMode.Inventory);
    }

    public void ShowDeliveryListAdmin()
    {
        HideAll();
        if (deliveryListPanel) deliveryListPanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.DeliveryAdmin);
    }

    public void ShowDeliveryCreate()
    {
        HideAll();
        if (deliveryCreatePanel) deliveryCreatePanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.DeliveryAdmin);
    }

    public void ShowDriverHome()
    {
        HideAll();
        if (driverHomePanel) driverHomePanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.Driver);
    }

    // ----------------------------
    // Aliases
    // ----------------------------
    public void ShowDeliveryList() => ShowDeliveryListAdmin();
    public void ShowCreate() => ShowDeliveryCreate();
    public void ShowDriver() => ShowDriverHome();
}
