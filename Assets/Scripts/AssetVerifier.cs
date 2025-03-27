using UnityEngine;
using System.IO;

public class AssetVerifier : MonoBehaviour
{
    void Awake()
    {
        string[] requiredFiles = {
            "Assets/ExternalAssets/Models/enemy01.fbx",
            "Assets/ExternalAssets/Textures/CRT_screen.png"
        };

        foreach (string path in requiredFiles)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[AssetVerifier] Missing file: {path}");
            }
        }
    }
}
