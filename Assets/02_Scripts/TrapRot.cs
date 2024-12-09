using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapRot : MonoBehaviour
{
    float angle = 180f;//회전 속도
    float targetAngle; // 목표 회전 속도
    float currentAngle; // 현재 회전 속도
    float vel = 0f;//현재 속도 값
    public float smoothTime = 0.3f; // 속도 변화의 부드러움 정도
    bool boosted = false;
    float boostTimer;//부스트 타이머
    float boosrDuration = 0.5f;//부스트 지속 시간
    public GameObject deathEffect;//죽는 이펙트

    public static int countMoney = 1000;//적 들어오는 값
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
        //부드럽게 하기위해 Mathf.SmoothDamp를 사용 하여 끊김 현상을 방지해줌
        currentAngle = Mathf.SmoothDamp(currentAngle, targetAngle, ref vel, smoothTime);
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적 삭제
            Destroy(other.gameObject);
            GameManager.gm.money += countMoney;
            //삭제 되면 애니메이션으로 되어있는 죽는 이펙트를 적이 죽었던 위치에서 생성 해줌
            Vector2 enemyPos = new Vector2(other.transform.position.x + 0.2f, other.transform.position.y);
            GameObject effect = Instantiate(deathEffect, enemyPos, Quaternion.identity);
            //생성이 된 다음에는 일정 시간 후에 삭제
            Destroy(effect, 1f);
        }

    }
}
