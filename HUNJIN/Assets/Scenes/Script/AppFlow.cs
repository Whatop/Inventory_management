using UnityEngine;

public enum MainObjectMode
{
    Inventory,
    DeliveryAdmin,
    Driver
}

/// <summary>
/// 화면 전환(Flow) 전담.
///
/// Panels
/// - MainScene: 버튼 선택
/// - DeliveryListPanel: 관리자 배달 현황
/// - DeliveryCreatePanel: 관리자 배달 등록
/// - DriverHomePanel: 기사 화면
/// - AllScene: (기존 재고/상품 전체 등) 인벤토리 계열 화면
///
/// Note
/// - 외부 스크립트에서 호출 메서드명이 흔들리기 쉬워서
///   ShowDeliveryList/ShowCreate/ShowDriver 같은 "별칭 메서드"를 함께 제공.
/// </summary>
public class AppFlow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainScene;
    public GameObject deliveryListPanel;
    public GameObject deliveryCreatePanel;
    public GameObject driverHomePanel;
    public GameObject allScene;

    [Header("Shared")]
    public GameObject mainObject;    // 공용(스크롤/검색 등) 루트
    public GameObject interactionUI; // 로딩/팝업 등 (선택)

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
        // mainObject는 항상 켜두는 방식 권장
        if (mainObject) mainObject.SetActive(true);

        if (inventoryModeRoot) inventoryModeRoot.SetActive(mode == MainObjectMode.Inventory);
        if (deliveryModeRoot) deliveryModeRoot.SetActive(mode == MainObjectMode.DeliveryAdmin);
        if (driverModeRoot) driverModeRoot.SetActive(mode == MainObjectMode.Driver);
    }

    // ------------------------------------------------------------------
    // Canonical methods (외부에서 이 이름으로 호출하면 됨)
    // ------------------------------------------------------------------

    /// <summary>메인 선택 화면</summary>
    public void ShowMain()
    {
        HideAll();
        if (mainScene) mainScene.SetActive(true);
        SetMainObjectMode(MainObjectMode.Inventory);
    }

    /// <summary>AllScene(기존 재고/상품 전체 등)</summary>
    public void ShowAll()
    {
        HideAll();
        if (allScene) allScene.SetActive(true);
        SetMainObjectMode(MainObjectMode.Inventory);
    }

    /// <summary>관리자: 배달 현황</summary>
    public void ShowDeliveryListAdmin()
    {
        HideAll();
        if (deliveryListPanel) deliveryListPanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.DeliveryAdmin);
    }

    /// <summary>관리자: 배달 등록</summary>
    public void ShowDeliveryCreate()
    {
        HideAll();
        if (deliveryCreatePanel) deliveryCreatePanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.DeliveryAdmin);
    }

    /// <summary>기사 홈</summary>
    public void ShowDriverHome()
    {
        HideAll();
        if (driverHomePanel) driverHomePanel.SetActive(true);
        SetMainObjectMode(MainObjectMode.Driver);
    }

    // ------------------------------------------------------------------
    // Aliases (다른 스크립트에서 헷갈리지 않게 별칭 제공)
    // ------------------------------------------------------------------

    /// <summary>별칭: 관리자 배달 현황</summary>
    public void ShowDeliveryList() => ShowDeliveryListAdmin();

    /// <summary>별칭: 관리자 배달 등록</summary>
    public void ShowCreate() => ShowDeliveryCreate();

    /// <summary>별칭: 기사 화면</summary>
    public void ShowDriver() => ShowDriverHome();
}
