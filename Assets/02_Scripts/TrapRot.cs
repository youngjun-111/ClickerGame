using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapRot : MonoBehaviour
{
    float angle = 180f;//ȸ�� �ӵ�
    float targetAngle; // ��ǥ ȸ�� �ӵ�
    float currentAngle; // ���� ȸ�� �ӵ�
    float vel = 0f;//���� �ӵ� ��
    public float smoothTime = 0.3f; // �ӵ� ��ȭ�� �ε巯�� ����
    bool boosted = false;
    float boostTimer;//�ν�Ʈ Ÿ�̸�
    float boosrDuration = 0.5f;//�ν�Ʈ ���� �ð�
    public GameObject deathEffect;//�״� ����Ʈ

    public static int countMoney = 1000;//�� ������ ��
    // Start is called before the first frame update
    void Start()
    {
        currentAngle = angle;
        targetAngle = angle;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, currentAngle * Time.deltaTime);
        if (boosted)
        {
            boostTimer += Time.deltaTime;

            if(boostTimer >= boosrDuration)
            {
                targetAngle = angle;
                boosted = false;
            }
        }
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            targetAngle = angle * 2;
            boosted = true;
            boostTimer = 0f;
        }
        //�ε巴�� �ϱ����� Mathf.SmoothDamp�� ��� �Ͽ� ���� ������ ��������
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref vel, smoothTime);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // �� ����
            Destroy(other.gameObject);
            GameManager.gm.money += countMoney;
            //���� �Ǹ� �ִϸ��̼����� �Ǿ��ִ� �״� ����Ʈ�� ���� �׾��� ��ġ���� ���� ����
            Vector2 enemyPos = new Vector2(other.transform.position.x + 0.2f, other.transform.position.y);
            GameObject effect = Instantiate(deathEffect, enemyPos, Quaternion.identity);
            //������ �� �������� ���� �ð� �Ŀ� ����
            Destroy(effect, 1f);
        }

    }
}
