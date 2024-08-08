using UnityEditor;
using UnityEngine;

namespace Editor {
    public class OverrideMaterialShaderOnCreate : AssetModificationProcessor {
        private static async void OnWillCreateAsset(string assetName) {
            if (!assetName.EndsWith(".mat")) return;

            var defaultShaderName = EditorPrefs.GetString(OverrideMaterialShaderSettingsProvider.DefaultMaterialShaderSettingsKey);
            if (string.IsNullOrEmpty(defaultShaderName)) return;

            var shader = Shader.Find(defaultShaderName);
            if (shader == null) return;
            
            while (!AssetDatabase.AssetPathExists(assetName)) {
                await System.Threading.Tasks.Task.Delay(100);
            }
            
            var material = AssetDatabase.LoadAssetAtPath<Material>(assetName);
            if (material == null) return;
            material.shader = shader;
        }
    }

    internal static class OverrideMaterialShaderSettingsProvider {
        public const string DefaultMaterialShaderSettingsKey = "DefaultMaterialShaderSettings_W45SD57";
        private static Shader _shader;

        [SettingsProvider]
        public static SettingsProvider CreateOverrideMaterialShaderSettingsProvider() {
            return new SettingsProvider("Preferences/DefaultMaterialShaderSettings", SettingsScope.User) {
                label = "Override default shader",
                guiHandler = _ => {
                    var settings = EditorPrefs.GetString(DefaultMaterialShaderSettingsKey, null);
                    if (string.IsNullOrEmpty(settings)) {
                        EditorGUILayout.LabelField("Default Shader not overridden.");
                    } else {
                        if (_shader == null || (_shader.name != settings && !string.IsNullOrEmpty(settings))) {
                            _shader = Shader.Find(settings);
                        }
                    }
                    
                    _shader = EditorGUILayout.ObjectField("Default Shader", _shader, typeof(Shader), false) as Shader;
                    if (_shader != null && _shader.name != settings) {
                        EditorPrefs.SetString(DefaultMaterialShaderSettingsKey, _shader.name);
                    }
                }
            };
        }
    }
}