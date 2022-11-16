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
    Sequence alertSeq1, alertSeq2;
    int seqCount;

    private void OnEnable()
    {
        GameManager.PlanetNearDeath += ShowAlertPanel;

        //alertSeq2 = DOTween.Sequence();
    }
    void ShowAlertPanel(Planets planet)
    {

        alertSeq1 = DOTween.Sequence();
        alertSeq1.Pause();
        alertSeq1.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y - 275, 1));
        alertSeq1.Append(planetImage.transform.DOScale(1, 0.2f));
        alertSeq1.Append(planetImage.transform.DORotate(new Vector3(0, 0, 180), 1).SetLoops(2, LoopType.Incremental)).SetEase(Ease.Linear);
        alertSeq1.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y + 275, 1));
        alertSeq1.Join(planetImage.transform.DOScale(0, 0.4f));
        alertSeq2 = DOTween.Sequence();
        alertSeq2.Pause();
        alertSeq2.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y - 275, 1));
        alertSeq2.Append(planetImage.transform.DOScale(1, 0.2f));
        alertSeq2.Append(planetImage.transform.DORotate(new Vector3(0, 0, 180), 1).SetLoops(2, LoopType.Incremental)).SetEase(Ease.Linear);
        alertSeq2.Append(alertPanel.DOAnchorPosY(alertPanel.anchoredPosition.y + 275, 1));
        alertSeq2.Join(planetImage.transform.DOScale(0, 0.4f));
        if (seqCount == 0)
        {
            planetImage.sprite = planets[(int)planet];
            seqCount++;
            alertSeq1.Play();
            alertSeq1.OnComplete(Complete);
            void Complete()
            {
                seqCount--;
            }
        }
        else
        {

            seqCount++;
            alertSeq1.Play();
            alertSeq1.OnComplete(Complete);
            void Complete()
            {
                planetImage.sprite = planets[(int)planet];
                alertSeq2.Play();
                seqCount--;
            }
        }


    }
}
