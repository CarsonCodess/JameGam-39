using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMult = 1.1f;
    private Vector2 _defaultScale;

    private void Awake()
    {
        _defaultScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(_defaultScale * scaleMult, 0.5f).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(_defaultScale / scaleMult, 0.5f).SetUpdate(true);
    }
}
