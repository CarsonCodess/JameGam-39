using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeStatUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private bool skip;
    [SerializeField] private TMP_Text upgradeName;
    [SerializeField] private TMP_Text upgradeDescription;
    [SerializeField] private TMP_Text upgradeCost;
    [SerializeField] private float scaleMult = 1.1f;
    private UpgradeStat _stat;
    private Vector2 _defaultScale;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }
    
    public void Initialize(UpgradeStat stat)
    {
        _stat = stat;
        upgradeName.text = stat.name;
        upgradeDescription.text = stat.desc;
        upgradeCost.text = stat.cost;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_defaultScale * scaleMult, 0.5f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_defaultScale / scaleMult, 0.5f).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (skip)
        {
            UpgradeScreen.instance.HideUpgradeScreen();
            return;
        }
        if(_stat.name == "Teleport")
            Player.instance.UnlockTeleport();
        Player.instance.Damage(float.Parse(_stat.cost.TrimEnd(" HP".ToCharArray())));
        UpgradeScreen.instance.HideUpgradeScreen();
    }
}
