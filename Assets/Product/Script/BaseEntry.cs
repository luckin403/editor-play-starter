﻿using UnityEngine;

namespace Product
{
    public class BaseEntry : MonoBehaviour
    {
        void OnValidate()
        {
            if (Application.isPlaying && UnityEditor.EditorPrefs.GetBool("MockLoadMode"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}