using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIcontroler : MonoBehaviour
{
    public GameObject window_left;
    public GameObject window_over;
    private List<string> leftmsgs = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：1\n面部识别：接入\n人格模拟：正常\n运行中......");
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：2\n面部识别：接入\n人格模拟：正常\n运行中......");
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：3\n面部识别：接入\n人格模拟：正常\n运行中......");
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：4\n面部识别：接入\n人格模拟：正常\n运行中......");
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：5\n面部识别：接入\n人格模拟：正常\n运行中......");
        leftmsgs.Add("2024/1/28\n操作员：Dr.xof\n迭代：5\n面部识别：接入\n人格模拟：繁忙\n等待响应......");
        leftmsgs.Add("-1/-1/-1\n操作员：FFFOOOXXX\n迭代：Null\n面部识别：0x3A3C47h\n人格模拟：未响应\n中断失败");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void state_Change(int state)
    {
        window_left.SetActive(true);
        window_left.GetComponentInChildren<TextMeshProUGUI>().text = leftmsgs[state];
        if (state == 6)
        {
            window_left.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.5f, 0, 0, 1);
        }
    }

    public void window_msg(string msg="重新载入迭代中...")
    {
        window_over.SetActive(true);
        window_over.GetComponentInChildren<TextMeshProUGUI>().text = msg;
    }
}
