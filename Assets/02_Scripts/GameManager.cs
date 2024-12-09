using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Ʈ�� & �� ����")]
    public Text moneyTxt;//�� �ݾ��� ǥ���� Text���۳�Ʈ ����
    public Text addUpgradeTxt;//Ʈ����ȭ �ؽ�Ʈ
    public Text trapLvTxt;//Ʈ�� ���� �ؽ�Ʈ
    public long money;//TotalMoney(����) 
    public long moneyAmount = 100;//�⺻������ Ŭ���� 100�� ����(����)
    public long trapUpgradePrice = 3000;//Ʈ�� ���׷��̵� ����(����)
    public int trapLevel = 1; //ù Ʈ�� ����(����)
    public GameObject upGradePanel;//Ʈ����ȭ ��ư ������Ʈ

    //���� ���� & Ŭ�� ����Ʈ ����
    [Space(15)]
    public GameObject enemyPrefab;//�� ������
    public Transform spawnPoint;//���� ��ġ
    public GameObject effectPrefabs;//����Ʈ ������
    public Button trapBtn;
    public Button enemyBtn;

    [Header("�� ���� ����")]
    public Text addEnmeyTxt;//���߰� �ؽ�Ʈ
    public Text enmeyCountTxt;//�� �� �ؽ�Ʈ
    public long enemyAddPrice = 5000;//���߰� ����(����)
    int enemiesPerSpawn = 1;//���� ��(����)
    public GameObject enmeyPanel;//���߰� ��ư ������Ʈ

    
    public static GameManager gm;//�̱���
    private void Awake()
    {
        gm = this;//�̱���
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyCoroutine());

        //�гξȿ� �ڽ��ؽ�Ʈ ã��
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
        Money();//Ŭ�� �� ������ �� Ŭ�� ����Ʈ �Լ�
        ShowInfo();//���� ������ ȭ��� ž �гο� ���̴� �� Ʈ�� ��ȭ �ܰ� ���� ��
        TrapPanelText();//Ʈ�� ��ȭ �ؽ�Ʈ �Լ�
        EnmeyPanelText();//�� �߰� �г� �ؽ�Ʈ �Լ�
        ButtomCheck();//�� ���� �� ��ư Ȱ��ȭ üũ �Լ�
    }
    
    //�� ���� �� Ŭ������Ʈ �Լ�
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
            moneyTxt.text = "0" + " ���";
        }
        else
        {
            moneyTxt.text = money.ToString("###,###") + "���";
        }
        if (trapLevel == 0)
        {
            trapLvTxt.text = "Ʈ�� ���� :" + " 1";
        }else
        {
            trapLvTxt.text = "Ʈ�� ���� :" + trapLevel.ToString();
        }
        if (enemiesPerSpawn == 1)
        {
            enmeyCountTxt.text = "�� �� :" + " 1";
        }else
        {
            enmeyCountTxt.text = "�� �� :" + enemiesPerSpawn.ToString();
        }
    }
    //�� ��ȯ �ڷ�ƾ �Լ�
    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                float randomY = Random.Range(0, 0.15f);
                Vector2 spawnPos = new Vector2(spawnPoint.position.x, spawnPoint.position.y + randomY);

                Instantiate(enemyPrefab, spawnPos, spawnPoint.rotation);
                yield return new WaitForSeconds(0.3f);//0.3���� �����̸� �༭ ��ġ�� �ʰ� ����
            }
            yield return new WaitForSeconds(2f);//��2�ʸ��� ������ �ڵ������� �̷����
        }
    }
    //�� ���� ��ư üũ �Լ�
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
    
    //Ʈ�� ���� �Լ�
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
            addUpgradeTxt.text = "Ʈ�� ���� : " + trapLevel + "\n\n";
            addUpgradeTxt.text += "Ʈ�� ��ȭ ����\n";
            addUpgradeTxt.text += trapUpgradePrice.ToString("N0") + "���\n\n";
            addUpgradeTxt.text += "Ŭ���� ����\n";
            addUpgradeTxt.text += moneyAmount.ToString("N0") + "��� ����";
        }
        
    }
    //�� ���� �Լ�
    public void SpawnEnemyUpgrade()
    {
        if (money >= enemyAddPrice)
        {
            enemiesPerSpawn++;//���׷��̵�� ���� �߰���
            money -= enemyAddPrice;//���׷��̵� ������ totalMoney���� ����
            money += enemiesPerSpawn;
            enemyAddPrice += enemiesPerSpawn * 5000;
        }
    }
    public void EnmeyPanelText()
    {
        if (enmeyPanel.activeSelf == true)
        {
            addEnmeyTxt.text = "�� �� : " + enemiesPerSpawn + "\n\n";
            addEnmeyTxt.text += "�� �߰� ����\n";
            addEnmeyTxt.text += enemyAddPrice.ToString("N0") + "���\n\n";
            addEnmeyTxt.text += "�Ѹ�� ����\n";
            addEnmeyTxt.text += TrapRot.countMoney.ToString("N0");
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }


    void Save()
    {
        SaveData saveData = new SaveData();//��üȭ

        saveData.money = money;
        saveData.moneyAmount = moneyAmount;
        saveData.trapUpgradePrice = trapUpgradePrice;
        saveData.trapLevel = trapLevel;
        saveData.enemyAddPrice = enemyAddPrice;
        saveData.enemiesPerSpawn = enemiesPerSpawn;

        string path = Application.persistentDataPath + "/save.xml";//���� ���ÿ� ���

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
    public long money;//��Ż �Ӵ�
    public long moneyAmount;//Ŭ���� ����
    public long trapUpgradePrice;//Ʈ�� ���׷��̵� ����
    public int trapLevel;//Ʈ�� ����

    public long enemyAddPrice;//�� �߰� ����
    public int enemiesPerSpawn;//�� ��
}