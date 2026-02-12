using UnityEngine;
using UnityEngine.UI;

public class AllSceneController : MonoBehaviour
{
    public AppFlow flow;

    [Header("Panels inside AllScene")]
    public GameObject adminArea;
    public GameObject driverArea;

    [Header("Controllers (optional)")]
    public DeliveryListPanelController deliveryList;
    public DriverHomePanelController driverHome;

    [Header("Buttons")]
    public Button toMainBtn;
    public Button adminTabBtn;
    public Button driverTabBtn;
    public Button refreshAllBtn;

    void Awake()
    {
        if (toMainBtn) toMainBtn.onClick.AddListener(() => flow.ShowMain());
        if (adminTabBtn) adminTabBtn.onClick.AddListener(() => SetTab(true));
        if (driverTabBtn) driverTabBtn.onClick.AddListener(() => SetTab(false));
        if (refreshAllBtn) refreshAllBtn.onClick.AddListener(RefreshAll);

        SetTab(true);
    }

    void SetTab(bool admin)
    {
        if (adminArea) adminArea.SetActive(admin);
        if (driverArea) driverArea.SetActive(!admin);
    }

    void RefreshAll()
    {
        if (deliveryList) deliveryList.Refresh();  
        if (driverHome) driverHome.Refresh();
    }
}
