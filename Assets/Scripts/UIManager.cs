using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public TMP_Text daysCountText;

    public GameObject endGameUI;

    public Transform CoinLocation;

    public CoinStack CoinPrefab;

    public TMP_Text GeodeSpawnerText;
    public Collection collection;
    public bool isLeft = false;
    public bool WentLeftOnce = false;

    public AudioSource audioSourcePeople;
    public AudioSource audioSourceMusic;
    public AudioSource audioSourceHitSound;
    public SpawnLoop spawnLoop;
    public float isLeftMusicBoost;
    public AudioSource audioSource;
    public AudioSource audioSourceMoney;
    private void Start()
    {
        spawnLoop = GetComponent<SpawnLoop>();
        Cam = Camera.main;
        player = PlayerStats.Instance;
        player.OnInventoryUpdated += UpdatePlayerInventory;
        UpdatePlayerInventory();
        UpdateHammerText();
        StartCoroutine(tutorialLoop());
    }

    Tween HammerTween;

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
        if (player.money >= hammerUpgradeCost)
        {
            if (HammerTween == null)
            {
                HammerTween = HammerButtonTransform.DOPunchScale(Vector3.one * 0.1f, 2, 1).SetLoops(-1);
            }
            else if (!HammerTween.IsPlaying()) 
            {
                HammerTween.Play();
            }
        }
        else if (HammerTween != null)
        {
            HammerTween.Kill();
            HammerTween = null;
        }

        audioSourceMusic.pitch = Mathf.Lerp(0.9f, 1.2f, spawnLoop.m_SpawnCurve.Evaluate(spawnLoop.GetLerpedTime()));
        isLeftMusicBoost = isLeft ? 0.1f : 0f;
        audioSourcePeople.volume = Mathf.Lerp(0.05f+ isLeftMusicBoost, 0.2f+ isLeftMusicBoost, spawnLoop.m_SpawnCurve.Evaluate(spawnLoop.GetLerpedTime()));
    }
    public void  UpdateHammerText()
    {
        HammerText.text = $"UPGRADE HAMMER : {hammerUpgradeCost} $$";

    }
    public void _OnLeftScreenRightArrowPressed(Button button)
    {
        isLeft = false;
        //MOVE TO SCREEN TWO
        Cam.transform.parent.DOMove(new Vector3(TransformScreen2.position.x, TransformScreen2.position.y, Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);
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
        audioSource.Play();
        //_DebugAddGem();
        UpdatePlayerInventory();
    }

    public IEnumerator tutorialLoop()
    {
        float timeElapsed = 0;
        float maxTimeElapsed = 30;
        while (true)
        {
            yield return new WaitForSeconds(4);

            if(player.GemsInInventory.Count == 0)
            {
                Tutoriel.Instance.TutoBreakGeodes();
            }
            if (player.GemsInInventory.Count >= 2 && player.money == 0)
            {
                Tutoriel.Instance.TutoSellGems();
            }
            if(collection.foundGems == 1)
            {
                Tutoriel.Instance.TutoCollectGems();
            }
            if (WentLeftOnce == false && isLeft || (isLeft && timeElapsed > maxTimeElapsed))
            {
                WentLeftOnce = true;
                Tutoriel.Instance.TutoGiveGems();
                yield return new WaitForSeconds(3);
                Tutoriel.Instance.TutoWishlist();
            }

            if (player.GemsInInventory.Count == 0)
            {
                Tutoriel.Instance.TutoBreakGeodes();
            }
        }
    }
    public void _OnRightScreenLeftArrowPressed(Button button)
    {
        isLeft = true;
        audioSource.Play();
        //MOVE TO SCREEN ONE
        Cam.transform.parent.DOMove(new Vector3(TransformScreen1.position.x, TransformScreen1.position.y, Cam.transform.position.z), CameraMoveDuration).SetEase(Ease.OutQuart);

    }

    public void _DebugAddGem()
    {
        GemInfos gemInfos = new GemInfos(debug_GemSO);
        player.GemsInInventory.Add(gemInfos);
    }

    public void UpdatePlayerInventory()
    {
        Debug.Log(player.GemsInInventory.Except(player.CurrentGems.Keys).Count());
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
        if(pastMoney != newMoney)
        {
            audioSourceMoney.Play();
        }
        int money = pastMoney;
        Tween t = DOTween.To(() => money, x => money = x, newMoney, 0.5f);
        t.onUpdate += () =>
        {
            MoneyText.text = money.ToString();
            MoneyText2.text = money.ToString();
        };
        pastMoney = newMoney;
    }

    public int hammerUpgradeCost = 10;
    public float hammerUpgradePowerScaling = 1.1f;
    public Transform HammerButtonTransform;
    public TextMeshProUGUI HammerText;
    public void OnUpgradeHammerButtonClick()
    {
        if (player.money >= hammerUpgradeCost)
        {
            print("BOUGHT UPGRADE");
            PlayerStats.Instance.SpawnSuicidalTextAtLocation(HammerButtonTransform.position + new Vector3(0,1f,0), "STRONGER HAMMER!!!!", Vector2.up * 3f, Vector2.zero, 0.5f);
            Camera.main.DOShakePosition(0.2f, 0.05f);
            player.RemoveMoney(hammerUpgradeCost);
            player.hammerForce += 10;
            hammerUpgradeCost = (int)Mathf.Pow(hammerUpgradeCost,hammerUpgradePowerScaling);
        }
        UpdateHammerText();
    }
    public void OnEndButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
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
    public void ChangeGeodePriceText(int newPrice)
    {
        GeodeSpawnerText.text = "BUY 1 GEODE :\n " + newPrice + " $$";
    }
}
