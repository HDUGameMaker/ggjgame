using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightControl : MonoBehaviour
{
    public RawImage[] lights;
    public float[] speeds;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (Time.time % speeds[i] < 0.1f) // 当 Time.time 除以 jumpSpeed 的余数小于 0.1 时，改变亮度
            {
                float alpha = Random.Range(0f, 7f); // 随机生成一个在0和1之间的值
                if(alpha>0.65f)
                {
                    alpha = 0.4f;
                }
                else if (alpha > 0.4f)
                {
                    alpha = 0f;
                }
                Color color = lights[i].color;
                color.a = alpha;
                lights[i].color = color;
            }
        }
    }
}
