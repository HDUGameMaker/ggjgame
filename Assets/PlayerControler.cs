using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using miruo;
namespace miruo {
    public class PlayerControler : MonoBehaviour
    {
        public GameControler myGame;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            myGame.touchBox();
        }
    }
}