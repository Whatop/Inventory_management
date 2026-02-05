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
    public GameObject mainObject;        // 공용 스크롤/검색
    public GameObject interactionUI;     // 로딩/팝업 등 (선택)

    [Header("MainObject Mode Groups (Optional)")]
    public GameObject inventoryModeRoot;   // MainObject 안에서 인벤 전용 UI 묶음
    public GameObject deliveryModeRoot;    // MainObject 안에서 배달 전용 UI 묶음
    public GameObject driverModeRoot;      // MainObject 안에서 기사 전용 UI 묶음

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
        // mainObject는 항상 켜두는 방식 권장
        if (mainObject) mainObject.SetActive(true);

        if (inventoryModeRoot) inventoryModeRoot.SetActive(mode == MainObjectMode.Inventory);
        if (deliveryModeRoot) deliveryModeRoot.SetActive(mode == MainObjectMode.DeliveryAdmin);
        if (driverModeRoot) driverModeRoot.SetActive(mode == MainObjectMode.Driver);
    }

    public void ShowMain()
    {
        HideAll();
        if (mainScene) mainScene.SetActive(true);
        SetMainObjectMode(MainObjectMode.Inventory); // 메인에서는 기존처럼 인벤 UI가 자연스러움
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
}
