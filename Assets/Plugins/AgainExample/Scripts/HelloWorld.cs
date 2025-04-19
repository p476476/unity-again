using Again.Runtime;
using UnityEngine;

namespace AgainExample.Scripts
{
    public class HelloWorld : MonoBehaviour
    {
        private void Start()
        {
            AgainSystem.Instance.Execute("HelloWorld");
        }
    }
}