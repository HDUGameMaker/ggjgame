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

        public int moveMode = 1;
        private Topic nowTopic;
        private Coroutine nowTopicCor;
        private bool couldGoNextTopic;
        // Start is called before the first frame update
        void Start()
        {
            List<Transform> childList = new List<Transform>();
            nowTopic = transform.GetChild(0).GetComponent<TopicControler>().mytopic;
            couldGoNextTopic = false;
            nowTopicCor = StartCoroutine(TopicCount());
        }
        // Update is called once per frame
        void Update()
        {


            //ִ�г����ڵ�
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
            if (moveMode == 1)
            {
                playerRig.velocity = Vector3.Lerp(playerRig.velocity, new Vector3(10, 0, 0), 0.02f);

            }
            if (moveMode == 0)
            {
                playerRig.velocity = Vector3.Lerp(playerRig.velocity, Vector3.zero, 0.02f);
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
    }
    [Serializable]
    public class Topic
    {
        public int id;
        public string name;
        public List<Textstring> texts;
        public Chosen mychosen;
        [Header("��Ȼ��������һ������")]
        public GameObject nextTopic;
        [Header("�����жϻ���")]
        public GameObject leftTopic;
        [Header("��ȷ�жϻ���")]
        public GameObject rightTopic;
        [Header("����˳�����")]
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
        [Header("0�ǲ�����")]
        public int mode;
        public float startTime;
        public float endTime;
    }
}