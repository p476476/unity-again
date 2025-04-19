using Again.Runtime;
using UnityEngine;

namespace AgainExample.Scripts
{
    public class HelloWorldSpine : MonoBehaviour
    {
        private void Start()
        {
            AgainSystem.Instance.Execute("Spine範例");
        }
    }
}