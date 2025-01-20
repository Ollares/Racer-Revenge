using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorTools
{
    [ExecuteInEditMode]
    public class ScreenshotMaker : EditorWindow
    {
        private List<Vector2Int> resolutions = new List<Vector2Int>();
        private int scale = 1;
        private string screenshotSavePath = "";
        private string lastScreenshotSavePath = "";

        // Add menu item named "My Window" to the Window menu
        [MenuItem("Tools/Screenshot")]
        public static void ShowWindow()
        {
            // Show existing window instance. If one doesn't exist, make one.
            EditorWindow editorWindow = EditorWindow.GetWindow(typeof(ScreenshotMaker));
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.titleContent = new GUIContent("ScreenshotMaker");
            editorWindow.Show();

            ScreenshotMaker target = editorWindow as ScreenshotMaker;
            target.SetDefaultResolutions();
            target.screenshotSavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Application.productName);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Set Default Resolutions"))
            {
                SetDefaultResolutions();
            }
            EditorGUILayout.LabelField("Resolutions", EditorStyles.boldLabel);
            for (int i = 0; i < resolutions.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label($"Resolution {i + 1}");

                resolutions[i] = EditorGUILayout.Vector2IntField("", resolutions[i], GUILayout.Width(200));

                if (GUILayout.Button("-"))
                    resolutions.RemoveAt(i);

                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("+"))
                resolutions.Add(Vector2Int.zero);
            if (GUILayout.Button("Add Game View Resolution"))
            {
                var height = (int)Handles.GetMainGameViewSize().y;
                var width = (int)Handles.GetMainGameViewSize().x;
                resolutions.Add(new Vector2Int(height, width));
            }

            EditorGUILayout.Space();

            scale = EditorGUILayout.IntSlider("Scale", scale, 1, 15);
            EditorGUILayout.HelpBox("The default mode of screenshot is crop - so choose a proper width and height. The scale is a factor " +
                "to multiply or enlarge the renders without loosing quality.", MessageType.None);

            EditorGUILayout.Space();

            GUILayout.Label("Save Path", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(screenshotSavePath, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                screenshotSavePath = EditorUtility.SaveFolderPanel("Path to Save Images", screenshotSavePath, Application.dataPath);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox("Choose the folder in which to save the screenshots ", MessageType.None);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Screenshots will be taken at", EditorStyles.boldLabel);

            for (int i = 0; i < resolutions.Count; i++)
            {
                EditorGUILayout.LabelField($"{resolutions[i].x * scale} x {resolutions[i].y * scale} px");
            }

            if (GUILayout.Button("Take Screenshot", GUILayout.MinHeight(60)))
            {
                if (screenshotSavePath == "")
                {
                    screenshotSavePath = EditorUtility.SaveFolderPanel("Path to Save Images", screenshotSavePath, Application.dataPath);
                    Debug.Log("Path Set");
                    TakeShots();
                }
                else
                {
                    TakeShots();
                }

                Application.OpenURL(screenshotSavePath);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open Last Screenshot", GUILayout.MaxWidth(160), GUILayout.MinHeight(40)))
            {
                if (lastScreenshotSavePath != "")
                {
                    Application.OpenURL("file://" + lastScreenshotSavePath);
                    Debug.Log("Opening File " + lastScreenshotSavePath);
                }
            }
            if (GUILayout.Button("Open Folder", GUILayout.MaxWidth(100), GUILayout.MinHeight(40)))
            {

                Application.OpenURL("file://" + screenshotSavePath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("In case of any error, make sure you have Unity Pro as the plugin requires Unity Pro to work.", MessageType.Info);
        }

        private void SetDefaultResolutions()
        {
            resolutions.Clear();
            resolutions.Add(new Vector2Int(1242, 2688));
            resolutions.Add(new Vector2Int(1242, 2208));
            resolutions.Add(new Vector2Int(2048, 2732));
            resolutions.Add(new Vector2Int(1080, 1920));

            foreach (var res in resolutions)
            {
                GameViewUtils.AddGameResolution(res.x, res.y);
            }
        }

        public void CreateDirectory(int width, int height)
        {
            string strPath = $"{screenshotSavePath}/{width}x{height}/";
            Directory.CreateDirectory(strPath);
        }

        public string ScreenShotName(int width, int height)
        {
            string strPath = $"{screenshotSavePath}/{width}x{height}/screen_{width}x{height}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}.png";
            lastScreenshotSavePath = strPath;
            return strPath;
        }

        public void TakeShots()
        {
            Resolution currentResolution = GetGameViewSize();
            for (var i = 0; i < resolutions.Count; i++)
            {
                Vector2Int resolution = resolutions[i];
                int width = resolution.x;
                int height = resolution.y;
                GameViewUtils.SetGameResolution(width, height);
                EditorApplication.Step();
                TakeShot(width, height);
                EditorApplication.Step();
            }
            GameViewUtils.SetGameResolution(currentResolution.width, currentResolution.height);
        }

        private void TakeShot(int resWidth, int resHeight)
        {
            CreateDirectory(resWidth, resHeight);
            string filename = ScreenShotName(resWidth, resHeight);
            ScreenCapture.CaptureScreenshot(filename, 1);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
        }

        public Resolution GetGameViewSize()
        {
            string[] res = UnityStats.screenRes.Split('x');
            return new Resolution() { height = int.Parse(res[1]), width = int.Parse(res[0]) };
        }
    }

    public static class GameViewUtils
    {
        static object gameViewSizesInstance;
        static MethodInfo getGroup;

        static GameViewUtils()
        {
            var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            getGroup = sizesType.GetMethod("GetGroup");
            gameViewSizesInstance = instanceProp.GetValue(null, null);
        }

        public enum GameViewSizeType
        {
            AspectRatio,
            FixedResolution
        }

        public static void AddGameResolution(int width, int height)
        {
            GameViewSizeGroupType sizeGroupType = GetGameViewSizeGroup();

            if (!SizeExists(sizeGroupType, width, height))
            {
                AddCustomSize(GameViewSizeType.FixedResolution, sizeGroupType, width, height, string.Format("{0}x{1}", width, height));
            }
        }

        public static void SetGameResolution(int width, int height)
        {
            int index = FindSize(GetGameViewSizeGroup(), width, height);
            if (index != -1)
                SetSize(index);
        }

        private static GameViewSizeGroupType GetGameViewSizeGroup()
        {
            GameViewSizeGroupType sizeGroupType = GameViewSizeGroupType.Standalone;

            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            {
                sizeGroupType = GameViewSizeGroupType.iOS;
            }
            else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                sizeGroupType = GameViewSizeGroupType.Android;
            }

            return sizeGroupType;
        }

        public static void SetSize(int index)
        {
            var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var gvWnd = EditorWindow.GetWindow(gvWndType);
            selectedSizeIndexProp.SetValue(gvWnd, index, null);
        }

        public static void AddCustomSize(GameViewSizeType viewSizeType, GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            var group = GetGroup(sizeGroupType);
            var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize");
            var gvsType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
            var ctor = gvsType.GetConstructor(new Type[] { typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType"), typeof(int), typeof(int), typeof(string) });
            var newSize = ctor.Invoke(new object[] {
                (int) viewSizeType, width, height, text });
            addCustomSize.Invoke(group, new object[] { newSize });
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, string text)
        {
            return FindSize(sizeGroupType, text) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, string text)
        {
            var group = GetGroup(sizeGroupType);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            var displayTexts = getDisplayTexts.Invoke(group, null) as string[];
            for (int i = 0; i < displayTexts.Length; i++)
            {
                string display = displayTexts[i];
                int pren = display.IndexOf('(');
                if (pren != -1)
                    display = display.Substring(0, pren - 1);
                if (display == text)
                    return i;
            }
            return -1;
        }

        public static bool SizeExists(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            return FindSize(sizeGroupType, width, height) != -1;
        }

        public static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
        {
            var group = GetGroup(sizeGroupType);
            var groupType = group.GetType();
            var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
            var getCustomCount = groupType.GetMethod("GetCustomCount");
            int sizesCount = (int)getBuiltinCount.Invoke(group, null) + (int)getCustomCount.Invoke(group, null);
            var getGameViewSize = groupType.GetMethod("GetGameViewSize");
            var gvsType = getGameViewSize.ReturnType;
            var widthProp = gvsType.GetProperty("width");
            var heightProp = gvsType.GetProperty("height");
            var indexValue = new object[1];
            for (int i = 0; i < sizesCount; i++)
            {
                indexValue[0] = i;
                var size = getGameViewSize.Invoke(group, indexValue);
                int sizeWidth = (int)widthProp.GetValue(size, null);
                int sizeHeight = (int)heightProp.GetValue(size, null);
                if (sizeWidth == width && sizeHeight == height)
                    return i;
            }
            return -1;
        }

        private static object GetGroup(GameViewSizeGroupType type)
        {
            return getGroup.Invoke(gameViewSizesInstance, new object[] {
                (int) type });
        }
    }
}