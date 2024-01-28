using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace miruo
{
    public class GameControler : MonoBehaviour
    {
        //public KrasusGame krasusControler;
        public TextMeshProUGUI speakText;
        public TextControler myTextControler;
        public float smileToJump;
        public WebcamHumanBeauty mywebcam;
        public int emoMode;
        
        //0~8
        //0是略微笑，1是大笑，2是笑哭，3是囧，4是害怕，5是惊讶，6是无语or淡漠，7是萨卡班甲鱼（这个有什么意义吗），8是无奈
        public GameObject playerObj;
        public Rigidbody playerRig;
        public GameObject startTopic;
        public ParticleSystem myParti;
        public MiruoEmoControler myEmo;
        public UIcontroler krasusUICon;
        private bool isGoToRight;
        private int moveMode;
        private bool couldReceive;
        private Topic nowTopic;
        private Coroutine nowTopicCor;
        private bool couldGoNextTopic;
        private Vector3 startPos;
        private int nowForwardState;
        private Coroutine audioCor;
        // Start is called before the first frame update
        void Start()
        {
            nowForwardState = 0;
            startPos = playerObj.transform.position;
            moveMode = 1;
            myParti.Play();
            nowTopic = startTopic.transform.GetComponent<TopicControler>().mytopic;
            couldGoNextTopic = false;
            nowTopicCor = StartCoroutine(TopicCount());
            audioCor = StartCoroutine(AudioCount());
        }
        // Update is called once per frame
        void Update()
        {

            smileToJump = mywebcam.smileMark - 4.5f;
            smileToJump = smileToJump / 3f;
            if (couldReceive)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    smileToJump = Mathf.Lerp(smileToJump, 1f, 0.02f);
                }
                else
                {
                    smileToJump = Mathf.Lerp(smileToJump, 0f, 0.02f);
                }
            }

            if (isGoToRight)
            {
                isGoToRight = false;
                if (nowTopicCor != null)
                {
                    StopCoroutine(nowTopicCor);
                    StopAllCoroutines();
                    nowTopic = nowTopic.rightTopic.GetComponent<TopicControler>().mytopic;
                    couldGoNextTopic = false;
                    nowTopicCor = StartCoroutine(TopicCount());
                    audioCor = StartCoroutine(AudioCount());
                }
            }

            //执行持续节点
            if (couldGoNextTopic)
            {
                if (nowTopic.nextTopic != null)
                {
                    StopAllCoroutines();
                    nowTopic = nowTopic.nextTopic.GetComponent<TopicControler>().mytopic;
                    couldGoNextTopic = false;
                    nowTopicCor = StartCoroutine(TopicCount());
                    audioCor = StartCoroutine(AudioCount());
                }
                else
                {
                    KillRobot();
                    Debug.Log("游戏结束");
                }

            }
        }
        private void FixedUpdate()
        {
            if (smileToJump <= 0.001f)
            {
                smileToJump = 0f;
            }
            if (couldReceive)
            {
                if (smileToJump > 0.2f)
                {
                    moveMode = 2;
                }
            }
            if (couldReceive)
            {
                playerRig.AddForce(new Vector3(0, 20, 0) * smileToJump);
            }
            if (moveMode == 2)
            {
                playerRig.velocity = new Vector3(Mathf.Lerp(playerRig.velocity.x, 1f, 0.05f), playerRig.velocity.y, playerRig.velocity.z);
            }
            if (moveMode == 1)
            {
                playerRig.velocity = new Vector3(Mathf.Lerp(playerRig.velocity.x, 7f, 0.05f), playerRig.velocity.y, playerRig.velocity.z);
            }
            if (moveMode == 0)
            {
                playerRig.velocity = new Vector3(Mathf.Lerp(playerRig.velocity.x, 0f, 0.05f), playerRig.velocity.y, playerRig.velocity.z);
            }
        }
        IEnumerator TopicCount()
        {
            yield return null;
            foreach (Textstring nowText in nowTopic.texts)
            {
                TopicShow(nowText.period);
                myEmo.ChangeEmo(nowText.emoMode-1);
                yield return new WaitForSeconds(nowText.time);
            }
            couldGoNextTopic = true;
        }
        IEnumerator AudioCount()
        {
            yield return new WaitForSeconds(nowTopic.MusicTime);
            nowTopic.topicAudio.Play();
        }
        void TopicShow(string ts)
        {
            myTextControler.displayText(ts);
        }
        public void KillRobot()//gameend
        {
            StopAllCoroutines();
            moveMode = 0;
            myParti.Pause();
            krasusUICon.window_msg("重新载入迭代中...");
            krasusUICon.state_Change(6);
        }
        public void RestartRobot()//gamerestart
        {
            StopAllCoroutines();
            myParti.Play();
            playerObj.transform.position = startPos;
            nowTopic = startTopic.GetComponent<TopicControler>().mytopic;
            moveMode = 1;
            couldGoNextTopic = false;
            nowTopicCor = StartCoroutine(TopicCount());
            audioCor = StartCoroutine(AudioCount());
        }
        
        public void touchBox()
        {
            moveMode = 0;
            couldReceive = true;
        }
        public void exitBox()//通过
        {
            
            couldReceive = false;
            moveMode = 1;
            isGoToRight = true;
            krasusUICon.state_Change(nowForwardState);
            nowForwardState++;
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
        [Header("玩家通过中断话题")]
        public GameObject rightTopic;
        [HideInInspector]
        public AudioSource topicAudio;
        [Header("音效跳出时间")]
        public float MusicTime;

    }
    [Serializable]
    public class Textstring
    {
        public float time;
        public string period;
        //[Header("0是略微笑，1是大笑，2是笑哭，3是囧，4是害怕，5是惊讶，6是无语or淡漠，7是萨卡班甲鱼（这个有什么意义吗），8是无奈")]
        public int emoMode;
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