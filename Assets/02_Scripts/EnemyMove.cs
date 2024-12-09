using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    float moveSpeed = 1f;//이동속도
    float targetSpeed;//목표 이동속도
    float currentSpeed;//현재 이동 속도
    float vel = 0f;//현재 이동 속도
    public float smoothTime = 0.3f; // 속도 변화의 부드러움 정도
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
                anim.speed = 1f;//애니메이션 속도 기본 값으로
            }
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            targetSpeed = moveSpeed * 2;
            boosted = true;
            boostTimer = 0f;
            anim.speed = 2f;//애니메이션도 2배로 빨라지게
        }

        //부드럽게 하기위해 Mathf.SmoothDamp를 사용 하여 끊김 현상을 방지해줌
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref vel, smoothTime);//ref값을 복사해서 속도 체크
    }

}
