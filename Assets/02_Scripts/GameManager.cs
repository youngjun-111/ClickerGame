using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("트랩 & 돈 변수")]
    public Text moneyTxt;//돈 금액을 표시할 Text컴퍼넌트 변수
    public Text addUpgradeTxt;//트랩강화 텍스트
    public Text trapLvTxt;//트렙 레벨 텍스트
    public long money;//TotalMoney(저장) 
    public long moneyAmount = 100;//기본적으로 클릭당 100원 증가(저장)
    public long trapUpgradePrice = 3000;//트랩 업그레이드 가격(저장)
    public int trapLevel = 1; //첫 트랩 레벨(저장)
    public GameObject upGradePanel;//트랩강화 버튼 오브젝트

    //스폰 관련 & 클릭 이펙트 변수
    [Space(15)]
    public GameObject enemyPrefab;//적 프리팹
    public Transform spawnPoint;//스폰 위치
    public GameObject effectPrefabs;//이펙트 프리팹
    public Button trapBtn;
    public Button enemyBtn;

    [Header("적 관련 변수")]
    public Text addEnmeyTxt;//적추가 텍스트
    public Text enmeyCountTxt;//적 수 텍스트
    public long enemyAddPrice = 5000;//적추가 가격(저장)
    int enemiesPerSpawn = 1;//적의 수(저장)
    public GameObject enmeyPanel;//적추가 버튼 오브젝트

    
    public static GameManager gm;//싱글톤
    private void Awake()
    {
        gm = this;//싱글톤
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());

        //패널안에 자식텍스트 찾기
        addUpgradeTxt = upGradePanel.transform.Find("UpAddAmountTxt").GetComponent<Text>();
        addEnmeyTxt = enmeyPanel.transform.Find("EnemyAddAmountTxt").GetComponent<Text>();

        string path = Application.persistentDataPath + "/save.xml";
        if (System.IO.File.Exists(path))
        {
            Load();
        }

    }

    void Update()
    {
        Money();//클릭 당 돈증가 및 클릭 이펙트 함수
        ShowInfo();//실질 적으로 화면상 탑 패널에 보이는 돈 트랩 강화 단계 적의 수
        TrapPanelText();//트랩 강화 텍스트 함수
        EnmeyPanelText();//적 추가 패널 텍스트 함수
        ButtomCheck();//돈 부족 시 버튼 활성화 체크 함수
    }
    
    //돈 증가 및 클릭이펙트 함수
    public void Money()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject obj = Instantiate(effectPrefabs, mousePosition, Quaternion.identity);
            money += moneyAmount;
        }
    }

    //UI info
    public void ShowInfo()
    {
        if (money == 0)
        {
            moneyTxt.text = "0" + " 골드";
        }
        else
        {
            moneyTxt.text = money.ToString("###,###") + "골드";
        }
        if (trapLevel == 0)
        {
            trapLvTxt.text = "트랩 레벨 :" + " 1";
        }else
        {
            trapLvTxt.text = "트랩 레벨 :" + trapLevel.ToString();
        }
        if (enemiesPerSpawn == 1)
        {
            enmeyCountTxt.text = "적 수 :" + " 1";
        }else
        {
            enmeyCountTxt.text = "적 수 :" + enemiesPerSpawn.ToString();
        }
    }
    //적 소환 코루틴 함수
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                float randomY = Random.Range(0, 0.15f);
                Vector2 spawnPos = new Vector2(spawnPoint.position.x, spawnPoint.position.y + randomY);

                Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
                yield return new WaitForSeconds(0.3f);//0.3초의 딜레이를 줘서 겹치지 않게 해줌
            }
            yield return new WaitForSeconds(2f);//매2초마다 생성이 자동적으로 이루어짐
        }
    }
    //돈 부족 버튼 체크 함수
    public void ButtomCheck()
    {
        if (money >= trapUpgradePrice)
        {
            trapBtn.interactable = true;
        }else
        {
            trapBtn.interactable = false;
        }

        if(money >= enemyAddPrice)
        {
            enemyBtn.interactable = true;
        }else
        {
            enemyBtn.interactable = false;
        }
    }
    
    //트랩 관련 함수
    public void TrapUpgrade()
    {
        if (money >= trapUpgradePrice)
        {
            money -= trapUpgradePrice;
            trapLevel++;
            moneyAmount += trapLevel * 5;
            trapUpgradePrice += trapLevel * 500;
        }
    }
    public void TrapPanelText()
    {
        if (upGradePanel.activeSelf == true)
        {
            addUpgradeTxt.text = "트랩 레벨 : " + trapLevel + "\n\n";
            addUpgradeTxt.text += "트랩 강화 가격\n";
            addUpgradeTxt.text += trapUpgradePrice.ToString("N0") + "골드\n\n";
            addUpgradeTxt.text += "클릭당 가격\n";
            addUpgradeTxt.text += moneyAmount.ToString("N0") + "골드 증가";
        }
        
    }
    //적 관련 함수
    public void SpawnEnemyUpgrade()
    {
        if (money >= enemyAddPrice)
        {
            enemiesPerSpawn++;//업그레이드시 적이 추가됨
            money -= enemyAddPrice;//업그레이드 가격을 totalMoney에서 빼줌
            money += enemiesPerSpawn;
            enemyAddPrice += enemiesPerSpawn * 5000;
        }
    }
    public void EnmeyPanelText()
    {
        if (enmeyPanel.activeSelf == true)
        {
            addEnmeyTxt.text = "적 수 : " + enemiesPerSpawn + "\n\n";
            addEnmeyTxt.text += "적 추가 가격\n";
            addEnmeyTxt.text += enemyAddPrice.ToString("N0") + "골드\n\n";
            addEnmeyTxt.text += "한명당 가격\n";
            addEnmeyTxt.text += TrapRot.countMoney.ToString("N0");
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }


    void Save()
    {
        SaveData saveData = new SaveData();//객체화

        saveData.money = money;
        saveData.moneyAmount = moneyAmount;
        saveData.trapUpgradePrice = trapUpgradePrice;
        saveData.trapLevel = trapLevel;
        saveData.enemyAddPrice = enemyAddPrice;
        saveData.enemiesPerSpawn = enemiesPerSpawn;

        string path = Application.persistentDataPath + "/save.xml";//빌드 셋팅에 등록

        XmlManager.XmlSave<SaveData>(saveData, path);
    }
    void Load()
    {
        SaveData saveData = new SaveData();
        string path = Application.persistentDataPath + "/save.xml";

        saveData = XmlManager.XmlLoad<SaveData>(path);
        money = saveData.money;
        moneyAmount = saveData.moneyAmount;
        trapUpgradePrice = saveData.trapUpgradePrice;
        trapLevel = saveData.trapLevel;

        enemyAddPrice = saveData.enemyAddPrice;
        enemiesPerSpawn = saveData.enemiesPerSpawn;
    }
}

[System.Serializable]
public class SaveData
{
    public long money;//토탈 머니
    public long moneyAmount;//클릭당 가격
    public long trapUpgradePrice;//트랩 업그레이드 가격
    public int trapLevel;//트랩 레벨

    public long enemyAddPrice;//적 추가 가격
    public int enemiesPerSpawn;//적 수
}