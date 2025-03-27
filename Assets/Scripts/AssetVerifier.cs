using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class AssetVerifier : MonoBehaviour
{
    private string externalAssetsPath = "Assets/ExternalAssets";
    private string readmePath;
    private HashSet<string> ignoredFilenames = new HashSet<string>
    {
        ".DS_Store",
        "README.txt",
        "asset_list.txt"
    };

    void Awake()
    {
        readmePath = Path.Combine(externalAssetsPath, "README.txt");

        if (!Directory.Exists(externalAssetsPath))
        {
            Debug.LogWarning($"[AssetVerifier] Folder not found: {externalAssetsPath}");
            return;
        }

        // 1. Leer el README.txt
        List<string> lines = new List<string>();
        HashSet<string> existingEntries = new HashSet<string>();
        List<string> missingFiles = new List<string>();

        if (File.Exists(readmePath))
        {
            lines.AddRange(File.ReadAllLines(readmePath));

            foreach (string line in lines)
            {
                if (line.Contains("→"))
                {
                    string entry = line.Split('→')[0].Trim(); // e.g. Textures/CRT_screen.png
                    if (entry.Contains("."))
                    {
                        existingEntries.Add(entry);

                        string fullPath = Path.Combine(externalAssetsPath, entry).Replace("\\", "/");
                        if (!File.Exists(fullPath))
                        {
                            Debug.LogWarning($"[AssetVerifier] MISSING: {fullPath}");
                            missingFiles.Add(entry);
                        }
                    }
                }
            }
        }
        else
        {
            // Si no existe, crear encabezado
            lines.Add("This folder contains external assets (e.g., 3D models, textures) that are not included in the repository.");
            lines.Add("");
            lines.Add("To use the project correctly, download the following files and place them here:");
            lines.Add("");
        }

        // 2. Buscar nuevos archivos y agregarlos
        string[] files = Directory.GetFiles(externalAssetsPath, "*.*", SearchOption.AllDirectories);
        List<string> newEntries = new List<string>();

        foreach (string fullPath in files)
        {
            string fileName = Path.GetFileName(fullPath);
            if (Path.GetExtension(fullPath) == ".meta" || ignoredFilenames.Contains(fileName)) continue;

            string relativePath = fullPath.Replace("\\", "/").Replace(externalAssetsPath + "/", "");

            if (!existingEntries.Contains(relativePath))
            {
                newEntries.Add(relativePath);
                existingEntries.Add(relativePath);
            }
        }

        // 3. Agregar nuevas entradas al final
        int count = 1;
        foreach (string line in lines)
        {
            if (line.Contains("→")) count++;
        }

        foreach (string path in newEntries)
        {
            lines.Add($"{count}. {path} → ");
            Debug.Log($"[AssetVerifier] Added to README.txt: {path}");
            count++;
        }

        // 4. Escribir el archivo actualizado
        File.WriteAllLines(readmePath, lines);
        Debug.Log($"[AssetVerifier] README.txt updated with {newEntries.Count} new file(s).");

        if (missingFiles.Count > 0)
        {
            Debug.LogWarning($"[AssetVerifier] {missingFiles.Count} asset(s) are listed but missing. See console above.");
        }
        else
        {
            Debug.Log("[AssetVerifier] All listed assets are present.");
        }
    }
}
