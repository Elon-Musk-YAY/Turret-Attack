using UnityEngine;
using UnityEditor;

public class SaveRenderTextureToFile
{
    public class ab : EditorWindow
    {
        string x = "Choose file name";
        public string t;
        [MenuItem("Assets/Save RenderTexture to file")]
        static void Init()
        {
            //ab window = ScriptableObject.CreateInstance<ab>();
            EditorWindow window = GetWindow(typeof(ab));
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
            window.ShowPopup();

        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("enter name of texture", EditorStyles.wordWrappedLabel);
            GUILayout.Space(70);
            t = EditorGUILayout.TextField(x,t);
            if (GUILayout.Button("Save!")) Do(t);
            this.Repaint();
        }

        void Do(string _t)
        {
            RenderTexture rt = Selection.activeObject as RenderTexture;

            RenderTexture.active = rt;
            Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
            RenderTexture.active = null;

            byte[] bytes;
            bytes = tex.EncodeToPNG();

            string path = AssetDatabase.GetAssetPath(rt).Replace(System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(rt)), "")+ _t + ".png";
            
            //string path = $"/Users/akshardesai/Downloads/{c}.png";
            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.ImportAsset(path);
            Debug.Log("Saved to " + path);
            this.Close();
        }
    }

    //[MenuItem("Assets/Save RenderTexture to file")]
    //public static void SaveRTToFile()
    //{
    //    //while (c < 10)
    //    //{
    //    ab f = new();
    //    f.ShowUtility();
        

    //    //}
    //}

    [MenuItem("Assets/Save RenderTexture to file", true)]
    public static bool SaveRTToFileValidation()
    {
        return Selection.activeObject is RenderTexture;
    }
}
