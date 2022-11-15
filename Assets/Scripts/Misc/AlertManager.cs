using System.Collections;
using System.Collections.Generic;
using Enum;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{

    [SerializeField]
    RectTransform alertPanel;
    [SerializeField]
    Image planetImage;

    [SerializeField]
    Sprite[] planets;

    private void OnEnable()
    {
        GameManager.PlanetNearDeath += ShowAlertPanel;
    }
    void ShowAlertPanel(Planets planet)
    {
        planetImage.sprite = planets[(int)planet];
        Sequence alertSeq = DOTween.Sequence();
        alertSeq.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y - 275, 1));
        alertSeq.Append(planetImage.transform.DOScale(1, 0.2f));
        alertSeq.Append(planetImage.transform.DORotate(new Vector3(0, 0, 180), 1).SetLoops(2, LoopType.Incremental)).SetEase(Ease.Linear);
        alertSeq.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y + 275, 1));
        alertSeq.Join(planetImage.transform.DOScale(0, 0.4f));
    }
}
