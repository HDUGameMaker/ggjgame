using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace miruo
{
    public class GameControler : MonoBehaviour
    {

        public TextMeshProUGUI speakText;
        private List<Topic> topics;

        public float smileToJump;

        public int emoMode;
        public GameObject playerObj;
        public Rigidbody playerRig;

        private int moveMode;
        private bool couldReceive;
        private Topic nowTopic;
        private Coroutine nowTopicCor;
        private bool couldGoNextTopic;
        // Start is called before the first frame update
        void Start()
        {
            moveMode = 1;
            List<Transform> childList = new List<Transform>();
            nowTopic = transform.GetChild(0).GetComponent<TopicControler>().mytopic;
            couldGoNextTopic = false;
            nowTopicCor = StartCoroutine(TopicCount());
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                smileToJump = Mathf.Lerp(smileToJump, 1f, 0.02f);
            }
            else
            {
                smileToJump = Mathf.Lerp(smileToJump, 0f, 0.02f);
            }
            
            
            //执行持续节点
            if (couldGoNextTopic)
            {
                if(nowTopic.nextTopic != null)
                {
                    nowTopic = nowTopic.nextTopic.GetComponent<TopicControler>().mytopic;
                    couldGoNextTopic = false;
                    nowTopicCor = StartCoroutine(TopicCount());
                }
                else
                {
                    KillRobot();
                }
                
            }
        }
        private void FixedUpdate()
        {
            if (smileToJump <= 0.001)
            {
                smileToJump = 0f;
            }
            if (couldReceive)
            {
                if (smileToJump > 0.2)
                {
                    moveMode = 1;
                }
            }
            playerRig.AddForce(new Vector3(0, 10, 0) * smileToJump);
            if (moveMode == 1)
            {
                playerRig.AddForce(new Vector3(5, 0, 0));
            }
            
        }
        IEnumerator TopicCount()
        {
            yield return null;
            foreach (Textstring nowText in nowTopic.texts)
            {
                TopicShow(nowText.period);
                yield return new WaitForSeconds(nowText.time);
            }
            couldGoNextTopic = true;
        }
        void TopicShow(string ts)
        {
            speakText.text = ts;
        }
        void KillRobot()
        {

        }
        public void touchBox()
        {
            moveMode = 0;
            couldReceive = true;
        }
    }
    [Serializable]
    public class Topic
    {
        public int id;
        public string name;
        public List<Textstring> texts;
        public Chosen mychosen;
        [Header("自然继续的下一个话题")]
        public GameObject nextTopic;
        [Header("错误中断话题")]
        public GameObject leftTopic;
        [Header("正确中断话题")]
        public GameObject rightTopic;
        [Header("玩家退出话题")]
        public GameObject exitTopic;
    }
    [Serializable]
    public class Textstring
    {
        public float time;
        public string period;
    }
    [Serializable]
    public class Chosen
    {
        [Header("0是不启用")]
        public int mode;
        public float startTime;
        public float endTime;
    }
}