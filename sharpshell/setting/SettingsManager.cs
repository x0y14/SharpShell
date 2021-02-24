using System;
using System.Collections.Generic;
using System.IO;
using JsonLoader;
using sharpshell.misc;

namespace sharpshell.setting
{
    public class SettingsManager
    {
        public readonly string SettingFilePath;
        public readonly Dictionary<string, dynamic> UserSettings;

        private Loader _jsonLoader;

        public SettingsManager(string path)
        {
            _jsonLoader = new Loader();
            
            SettingFilePath = path;
            UserSettings = _jsonLoader.LoadWithPath(SettingFilePath);
        }

        public string GetPromptStyle()
        {
            var promptStyle = $"%whoami $ ";
            if (UserSettings.ContainsKey("style") && UserSettings["style"] is Dictionary<string, dynamic>)
            {
                // スタイルでプロンプトが設定されているか。
                Dictionary<string, dynamic> styleSettings = UserSettings["style"];
                if (styleSettings.ContainsKey("prompt") && styleSettings["prompt"] is string)
                {
                    promptStyle = styleSettings["prompt"];
                }
            }
            return promptStyle;
        }

        public List<string> GetPath()
        {
            List<string> paths = new List<string>();
            if (UserSettings.ContainsKey("path"))
            {
                try
                {
                    // JsonLoaderではjsonにおける配列(list)はList<object>になっているので、無理矢理List<string>へ変換
                     paths = Util.ConvertPathList(UserSettings["path"]);
                }
                catch (Exception)
                {
                    throw new Exception("list of path can not convert, object to string");
                }
            }

            return paths;
        }
    }
}