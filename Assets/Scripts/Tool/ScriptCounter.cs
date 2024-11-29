#if UNITY_EDITOR
namespace Tool
{
    using UnityEditor;
    using UnityEngine;
    using System.IO;

    public class ScriptCounter : MonoBehaviour
    {
        [MenuItem("Tools/Count Scripts")]
        public static void CountScripts()
        {
            string[] scriptFiles = Directory.GetFiles(Application.dataPath + "/Scripts", "*.cs", SearchOption.AllDirectories);
            Debug.Log("Number of scripts in the project: " + scriptFiles.Length);
        }
    }
}
#endif