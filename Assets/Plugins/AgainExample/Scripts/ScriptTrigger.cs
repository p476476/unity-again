using Again.Runtime;
using UnityEngine;

namespace AgainExample.Scripts
{
    public class ScriptTrigger : MonoBehaviour
    {
        public string scriptName;

        private void Start()
        {
            AgainSystem.Instance.Execute(scriptName);
        }
    }
}