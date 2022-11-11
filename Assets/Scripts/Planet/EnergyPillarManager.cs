using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class EnergyPillarManager : MonoBehaviour
{
    [SerializeField]
    public int pillarID;
    [SerializeField]
    public int planetID;

    [HideInInspector]
    public bool isCharged;

    float chargeAmount;

    Canvas pillarCanvas;
    Material pillarMaterial;

    Transform player;


    //To be changed after adding asseets
    [SerializeField]
    TextMeshProUGUI chargeAmountText;
    float emissiveIntensity = 10f;
    Color emissiveColor;

    void Start()
    {
        pillarCanvas = transform.GetChild(0).GetComponent<Canvas>();
        pillarMaterial = transform.GetChild(1).GetComponent<Renderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        emissiveColor = Color.gray;
        Debug.Log(pillarMaterial.color);
    }

    void OnTriggerEnter()
    {
        Debug.Log("Entered Pillar: " + pillarID + " on planet: " + planetID);
        pillarCanvas.transform.LookAt(pillarCanvas.transform.position + player.transform.rotation * Vector3.forward, Vector3.up);
        pillarCanvas.transform.DOScale(0.05f, 0.5f).From(0).SetEase(Ease.Linear);


    }

    void OnTriggerExit()
    {
        pillarCanvas.transform.DOScale(0, 0.5f).From(0.05f).SetEase(Ease.Linear).OnComplete(CompleteTween);

        void CompleteTween()
        {
            pillarCanvas.gameObject.SetActive(true);
        }

    }


    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (chargeAmount < 100)
            {
                chargeAmount = chargeAmount + (Time.deltaTime * 50);
                chargeAmountText.text = ((int)chargeAmount).ToString();

                pillarMaterial.SetColor("_EmissionColor", emissiveColor * ((chargeAmount / 100) * emissiveIntensity));
                pillarMaterial.EnableKeyword("_EMISSION");



            }

        }
    }




}
