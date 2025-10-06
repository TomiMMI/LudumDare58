using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UIHandling : MonoBehaviour
{
    public static UIHandling Instance { get; private set; }

    void Awake()
    {
        UIHandling.Instance = this;
    }

    public Transform TransformScreen2;
    public Transform TransformScreen1;

    public Transform GemBagTransform;
    public Camera Cam;
    public float CameraMoveDuration;

    public PlayerStats player;
    [SerializeField]
    private GemInventory m_InventoryParent;

    public GemSO debug_GemSO;
    public Gem GemPrefab;

    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI MoneyText2;

    public Transform CoinLocation;

    public CoinStack CoinPrefab;
    private void Start()
    {
        Cam = Camera.main;
        player = PlayerStats.Instance;
        player.OnInventoryUpdated += UpdatePlayerInventory;
        UpdatePlayerInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _OnLeftScreenRightArrowPressed(null);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _OnRightScreenLeftArrowPressed(null);
        }
    }

    public void _OnLeftScreenRightArrowPressed(Button button)
    {
        //MOVE TO SCREEN TWO
        Cam.transform.DOMove(new Vector3(TransformScreen2.position.x, TransformScreen2.position.y, Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);
        //TODO : add checks to remove the current gem from the player drag
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        //_DebugAddGem();
        UpdatePlayerInventory();
    }


    public void _OnRightScreenLeftArrowPressed(Button button)
    {
        //MOVE TO SCREEN ONE
        Cam.transform.DOMove(new Vector3(TransformScreen1.position.x, TransformScreen1.position.y, Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);

    }

    public void _DebugAddGem()
    {
        GemInfos gemInfos = new GemInfos(debug_GemSO);
        player.GemsInInventory.Add(gemInfos);
    }

    public void UpdatePlayerInventory()
    {
        foreach (GemInfos gemInfos in player.GemsInInventory.Except(player.CurrentGems.Keys))
        {
            Gem gem = m_InventoryParent.AddToInventory(gemInfos);
            if (gem != null)
            {
                player.CurrentGems[gemInfos] = gem;
            }

        }
        UpdateMoneyText(player.money);
    }
    public void CreateAndAddGemToBag(Transform GeodeTranform, GemSO gemSO)
    {
        GemInfos gemInfos = new GemInfos(gemSO);
        player.GemsInInventory.Add(gemInfos);

        Gem gem = GameObject.Instantiate(GemPrefab);
        gem.InitializeGem(gemInfos);
        gem.transform.position = GeodeTranform.position;
        Sequence s = DOTween.Sequence();
        s.Append(gem.transform.DOMoveY(gem.transform.position.y + 2f, 0.5f));
        s.AppendInterval(0.5f);
        Tween moveTween = gem.transform.DOMove(GemBagTransform.position + new Vector3(0, 1f, 0), 0.5f);
        moveTween.onComplete += () =>
        {
            Destroy(gem.gameObject);
        };
        gem.Disable();
        s.Append(moveTween);
        s.Append(GemBagTransform.DOScale(1.15f, 0.4f).SetEase(Ease.OutBack));
        s.Append(GemBagTransform.DOScale(1, 0.2f).SetEase(Ease.OutBack));


        UpdatePlayerInventory();
    }
    public int pastMoney = 0;
    public void UpdateMoneyText(int newMoney)
    {

        int money = pastMoney;
        Tween t = DOTween.To(() => money, x => money = x, newMoney, 0.5f);
        t.onUpdate += () => {
            MoneyText.text = money.ToString();
            MoneyText2.text = money.ToString();
        };
        pastMoney = newMoney;
    }

    public int hammerUpgradeCost = 10;
    public float hammerUpgradePowerScaling = 1.2f;
    public void OnUpgradeHammerButtonClick()
    {
        if(player.money >= hammerUpgradeCost)
        {
            player.RemoveMoney(hammerUpgradeCost);
            player.hammerForce += 10;

        }
    }


    public void OnUpgradeGrinderButtonClick()
    {

    }
    public void SpawnCoinPrefabAtPosition(Vector3 position, int coinStackValue)
    {
        CoinStack CoinStack = GameObject.Instantiate<CoinStack>(CoinPrefab);
        CoinStack.Initialize(coinStackValue);
        CoinStack.transform.position = position;
    }
}
