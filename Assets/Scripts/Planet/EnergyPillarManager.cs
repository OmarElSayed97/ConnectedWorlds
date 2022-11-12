using DG.Tweening;
using Enum;
using TMPro;
using UnityEngine;

namespace Planet
{
    public class EnergyPillarManager : MonoBehaviour
    {
        [SerializeField] public Pillars pillarID;
        [SerializeField] public Planets planetID;
        [HideInInspector] public bool isCharged;

        private float chargeAmount;

        private Canvas pillarCanvas;
        private Material pillarMaterial;

        private Transform player;
        private GameManager gameManager;


        //To be changed after adding asseets
        [SerializeField] TextMeshProUGUI chargeAmountText;
        private float emissiveIntensity = 10f;
        private Color emissiveColor;
        private bool isPillarReady;



        void Start()
        {
            gameManager = GameManager.Instance;
            pillarCanvas = transform.GetChild(0).GetComponent<Canvas>();
            pillarMaterial = transform.GetChild(1).GetComponent<Renderer>().material;
            player = GameObject.FindGameObjectWithTag("Player").transform;
            emissiveColor = Color.gray;
        }

        void Update()
        {
            if (gameManager.currentPlanet.pillarsValues[pillarID] >= 100)
            {
                gameManager.currentPlanet.pillarsChargedCount++;
                gameManager.pillarsIndicator[(int)pillarID].enabled = true;

            }

        }


        void OnTriggerEnter()
        {
            // pillarCanvas.transform.LookAt(pillarCanvas.transform.position + player.transform.rotation * Vector3.forward, Vector3.up);
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
                if (gameManager.currentPlanet.pillarsValues[pillarID] < 100)
                {
                    gameManager.currentPlanet.pillarsValues[pillarID] = gameManager.currentPlanet.pillarsValues[pillarID] + (Time.deltaTime * 50);
                    chargeAmountText.text = ((int)gameManager.currentPlanet.pillarsValues[pillarID]).ToString();

                    pillarMaterial.SetColor("_EmissionColor", emissiveColor * ((gameManager.currentPlanet.pillarsValues[pillarID] / 100) * emissiveIntensity));
                    pillarMaterial.EnableKeyword("_EMISSION");



                }

            }
        }




    }
}
