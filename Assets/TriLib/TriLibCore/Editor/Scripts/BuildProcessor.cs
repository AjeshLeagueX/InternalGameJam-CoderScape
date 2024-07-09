using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace TriLibCore.Editor
{
    public class BuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => -1000;

        public void OnPreprocessBuild(BuildReport report)
        {
#if TRILIB_ENABLE_WEBGL_THREADS
            PlayerSettings.WebGL.threadsSupport = true;
#else
            PlayerSettings.WebGL.threadsSupport = false;
#endif
#if UNITY_WSA
            if (!Application.isBatchMode && !PlayerSettings.WSA.GetCapability(PlayerSettings.WSACapability.RemovableStorage) && EditorUtility.DisplayDialog(
                    "TriLib", "TriLib cache system needs the [RemovableStorage] WSA Capacity enabled. Do you want to enable it?", "Yes", "No"))
            {
                PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.RemovableStorage, true);
            }
#endif
        }
    }
}