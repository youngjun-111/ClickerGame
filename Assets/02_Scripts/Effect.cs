using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    SpriteRenderer rend;
    Vector2 point;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        point = transform.position + Vector3.up;
    }


    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, point, Time.deltaTime * 0.2f);
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - 1 * Time.deltaTime);

        if (rend.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }


}
