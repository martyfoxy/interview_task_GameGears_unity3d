using UnityEngine;
using UnityEngine.UI;

public sealed class UiContainer : MonoBehaviour
{
    [SerializeField]
    private Button buffReloadBtn;
    
    [SerializeField]
    private Button bufflessReloadBtn;

    [SerializeField]
    private PlayerPanelHierarchy playerPanel;
    
    [SerializeField]
    private PlayerPanelHierarchy enemyPanel;
}
