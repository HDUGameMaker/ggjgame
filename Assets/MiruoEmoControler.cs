using System.Collections.Generic;
using UnityEngine;
namespace miruo
{
    public class MiruoEmoControler : MonoBehaviour
    {
        public int nowEmoMode;
        public List<GameObject> Emos;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void ChangeEmo(int s)
        {

            Emos[nowEmoMode].SetActive(false);
            Emos[s].SetActive(true);
            nowEmoMode = s;

        }
    }
}