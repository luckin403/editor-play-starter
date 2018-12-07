using UnityEngine;

namespace Product
{
    public class BaseEntry : MonoBehaviour
    {
        void OnValidate()
        {
            if (Application.isPlaying && UnityEditor.EditorPrefs.GetBool("MockReloadMode"))
            {
                gameObject.SetActive(false);
                UnityEditor.EditorPrefs.SetBool("MockReloadMode", false);
            }
        }
    }
}