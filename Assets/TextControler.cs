using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace miruo {

    public class TextControler : MonoBehaviour
    {
        public float speed;
        public TextMeshProUGUI realtext;
        private Coroutine nowTextCor;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void displayText(string chr)
        {
            if (nowTextCor != null)
            {
                StopCoroutine(nowTextCor);
            }
            realtext.text = null;
            nowTextCor = StartCoroutine(StartText(chr));
            
        }
        IEnumerator StartText(string cs)
        {
            for(int i = 0; i < cs.Length; i++)
            {
                realtext.text = realtext.text + cs[i];
                yield return new WaitForSeconds(speed);
            }
        }
    }
}