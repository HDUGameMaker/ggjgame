using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using miruo;

public class UIcontroler : MonoBehaviour
{
    public GameObject window_left, window_over, window_left_small;

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
        left_Hide(true);
        left_small_Hide(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 左边窗口的状态变化
    /// </summary>
    /// <param name="state">游戏阶段，默认阶段为6时字体变红</param>
    /// <param name="msg">空着即为默认</param>
    public void state_Change(int state, string msg = "")
    {
        window_left.SetActive(true);
        if (msg != "") window_left.GetComponent<TextControler>().displayText(msg);
        else window_left.GetComponent<TextControler>().displayText(leftmsgs[state]);
        if (state == 6)
        {
            window_left.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.5f, 0, 0, 1);
        }
        else
        {
            window_left.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);
        }
    }

    /// <summary>
    /// 左边窗口的图形隐藏
    /// </summary>
    public void left_Hide(bool f)
    {
        if(f) window_left.GetComponent<Image>().color = new Color(0,0,0,0);
        else window_left.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    }
    public void left_small_Hide(bool f)
    {
        if (f) window_left_small.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        else window_left_small.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
    }
    
    /// <summary>
    /// 中间弹窗
    /// </summary>
    /// <param name="msg">消息信息默认为“重新输入迭代中...”</param>
    public void window_msg(string msg="重新载入迭代中...")
    {
        window_over.SetActive(true);
        window_over.GetComponentInChildren<TextMeshProUGUI>().text = msg;
    }
}
