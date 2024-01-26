using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Collections;

namespace miruo {
    public class GameControler : MonoBehaviour
    {

        public TextMeshProUGUI speakText;
        public GameObject buttonLeft;
        public GameObject buttonRight;
        public List<Topic> topics;

        private Topic nowTopic;
        private Coroutine nowTopicCor;
        private Coroutine nowChosenCor;
        private bool couldGoNextTopic;
        // Start is called before the first frame update
        void Start()
        {
            buttonLeft.SetActive(false);
            buttonRight.SetActive(false);
            List<Transform> childList = new List<Transform>();

            FindChild(transform);
            nowTopic = topics[0];
            couldGoNextTopic = false;
            nowTopicCor = StartCoroutine(TopicCount());
            nowChosenCor = StartCoroutine(ChosenCount());
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //ִ���ҽڵ�
                if (nowTopicCor != null)
                {
                    StopCoroutine(nowTopicCor);
                }
                if(nowChosenCor != null)
                {
                    StopCoroutine(nowChosenCor);
                }
                nowTopic = nowTopic.rightTopic.GetComponent<TopicControler>().mytopic;
                couldGoNextTopic = false;
                nowTopicCor = StartCoroutine(TopicCount());
                nowChosenCor = StartCoroutine(ChosenCount());
                ChosenHide();
            }
            else
            if (Input.GetKeyDown(KeyCode.L))
            {
                //ִ����ڵ�
                if (nowTopicCor != null)
                {
                    StopCoroutine(nowTopicCor);
                }
                if (nowChosenCor != null)
                {
                    StopCoroutine(nowChosenCor);
                }
                nowTopic = nowTopic.leftTopic.GetComponent<TopicControler>().mytopic;
                couldGoNextTopic = false;
                nowTopicCor = StartCoroutine(TopicCount());
                nowChosenCor = StartCoroutine(ChosenCount());
                ChosenHide();
            }
            else
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ִ���˳��ڵ�
                if (nowTopicCor != null)
                {
                    StopCoroutine(nowTopicCor);
                }
                if (nowChosenCor != null)
                {
                    StopCoroutine(nowChosenCor);
                }
                nowTopic = nowTopic.exitTopic.GetComponent<TopicControler>().mytopic;
                couldGoNextTopic = false;
                nowTopicCor = StartCoroutine(TopicCount());
                nowChosenCor = StartCoroutine(ChosenCount());
                ChosenHide();

            }
            else
            {
                //ִ�г����ڵ�
                if (couldGoNextTopic)
                {
                    nowTopic = nowTopic.nextTopic.GetComponent<TopicControler>().mytopic;
                    couldGoNextTopic = false;
                    nowTopicCor = StartCoroutine(TopicCount());
                    if (nowChosenCor != null)
                    {
                        StopCoroutine(nowChosenCor);
                    }
                    ChosenHide();
                    nowChosenCor = StartCoroutine(ChosenCount());
                }
            }


        }

        IEnumerator TopicCount()
        {
            yield return null;
            foreach(Textstring nowText in nowTopic.texts)
            {
                TopicShow(nowText.period);
                yield return new WaitForSeconds(nowText.time);
            }
            couldGoNextTopic = true;
        }
        IEnumerator ChosenCount()
        {
            if(nowTopic.mychosen.mode == 1)
            {
                yield return new WaitForSeconds(nowTopic.mychosen.startTime);
                ChosenShow();
                yield return new WaitForSeconds(nowTopic.mychosen.endTime - nowTopic.mychosen.startTime);
                ChosenHide();
            }
        }
        void TopicShow(string ts)
        {
            speakText.text = ts;
        }
        void ChosenShow()
        {
            buttonLeft.SetActive(true);
            buttonRight.SetActive(true);
        }
        void ChosenHide()
        {
            buttonLeft.SetActive(false);
            buttonRight.SetActive(false);
        }
        void FindChild(Transform nowTrans)
        {
            for(int i = 0; i < nowTrans.childCount; i++)
            {
                if (nowTrans.GetChild(i).childCount != 0)
                {
                    FindChild(nowTrans.GetChild(i));
                }
                TopicControler cacheTopicCon = null;
                nowTrans.GetChild(i).TryGetComponent<TopicControler>(out cacheTopicCon);
                if (cacheTopicCon != null)
                {
                    topics.Add(cacheTopicCon.mytopic);
                }
            }
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