using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    float moveSpeed = 1f;//�̵��ӵ�
    float targetSpeed;//��ǥ �̵��ӵ�
    float currentSpeed;//���� �̵� �ӵ�
    float vel = 0f;//���� �̵� �ӵ�
    public float smoothTime = 0.3f; // �ӵ� ��ȭ�� �ε巯�� ����
    bool boosted = false;
    float boostDuration = 0.5f;
    float boostTimer;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        targetSpeed = moveSpeed;
        currentSpeed = moveSpeed;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector2.right * currentSpeed * Time.deltaTime);
        if (boosted)
        {
            boostTimer += Time.deltaTime;

            if (boostTimer >= boostDuration)
            {
                targetSpeed = moveSpeed;
                boosted = false;
                anim.speed = 1f;//�ִϸ��̼� �ӵ� �⺻ ������
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            targetSpeed = moveSpeed * 2;
            boosted = true;
            boostTimer = 0f;
            anim.speed = 2f;//�ִϸ��̼ǵ� 2��� ��������
        }

        //�ε巴�� �ϱ����� Mathf.SmoothDamp�� ��� �Ͽ� ���� ������ ��������
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref vel, smoothTime);//ref���� �����ؼ� �ӵ� üũ
    }

}
