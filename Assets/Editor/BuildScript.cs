using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BuildScript
{
    private static string[] SCENES = FindEnabledEditorScenes();

    private static string APP_NAME = "FastlaneTest";
    private static string TARGET_DIR = "Build";

    [MenuItem("Custom/CI/Build Windows")]
    private static void PerformWindowsBuild()
    {
        string targetDir = APP_NAME + ".exe";
        GenericBuild(SCENES, TARGET_DIR + "/PC/" + targetDir, BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Windows Phone")]
    private static void PerformWindowsStoreBuild()
    {
        string targetDir = APP_NAME + ".exe";
        GenericBuild(SCENES, TARGET_DIR + "/WindowsPhone/" + targetDir, BuildTarget.WP8Player, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Mac OS X")]
    private static void PerformMacOSXBuild()
    {
        string targetDir = APP_NAME + ".app";
        GenericBuild(SCENES, TARGET_DIR + "/MacOS/" + targetDir, BuildTarget.StandaloneOSXIntel, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Web")]
    private static void PerformWebBuild()
    {
        string targetDir = APP_NAME + ".unity3d";
        GenericBuild(SCENES, TARGET_DIR + "/Web/" + targetDir, BuildTarget.WebPlayer, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build Android")]
    private static void PerformAndroidBuild()
    {
        PlayerSettings.bundleIdentifier = "com.Terahard.Teracasino";
        string targetDir = APP_NAME;
        GenericBuild(SCENES, TARGET_DIR + "/Android/" + targetDir, BuildTarget.Android, BuildOptions.None);
    }

    [MenuItem("Custom/CI/Build iOS")]
    private static void PerformIosBuild()
    {
        string targetDir = APP_NAME;
        GenericBuild(SCENES, TARGET_DIR + "/Ios/" + targetDir, BuildTarget.iOS, BuildOptions.None);
    }

    private static string[] FindEnabledEditorScenes()
    {
        List<string> editorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            editorScenes.Add(scene.path);
        }
        return editorScenes.ToArray();
    }


    private static void GenericBuild(string[] scenes, string targetDir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, targetDir, build_target, build_options);
        if (res.Length < 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }

        if (build_target == BuildTarget.iOS)
        {
            ChangeEncryptionPList(build_target, targetDir);
        }
    }











    public static void ChangeEncryptionPList(BuildTarget build_target, string targetDir)
    {
        Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++\nTesting PostProcessBuild [" + build_target.ToString() + "]\n++++++++++++++++++++++++++++++++++++++++++++++++++++");
        if (build_target == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = targetDir + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // Change value of CFBundleVersion in Xcode plist
            var buildKey = "ITSAppUsesNonExemptEncryption";
            rootDict.SetBoolean(buildKey, false);

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());



            {
                StreamReader sr = new StreamReader(plistPath);
                string t_OrginalPlist = sr.ReadToEnd();
                sr.Close();

                Debug.Log("\nPLIST\n" + t_OrginalPlist + "\n");
            }
        }
    }






}