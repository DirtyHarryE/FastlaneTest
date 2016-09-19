using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System;

public class IncrementVersionNumber
{

    //[PostProcessBuild]
    //public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    //{
    //    ReadVersionAndIncrement(pathToBuiltProject, true);
    //}

    [MenuItem("Terahard/Increment Version Number")]
    private static void Increment()
    {
        ReadVersionAndIncrement();
    }
    [MenuItem("Terahard/Increment and Assign Version Number")]
    private static void IncrementAndAssign()
    {
        ReadVersionAndIncrement(true);
    }
    private static void ReadVersionAndIncrement(bool assign)
    {
        ReadVersionAndIncrement(null, assign);
    }
    private static void ReadVersionAndIncrement(string buildPath = null, bool assign = false)
    {
        //the file name and path.  No path is the base of the Unity project directory (same level as Assets folder)
        string versionTextFileNameAndPath = Application.streamingAssetsPath + "/version.txt";

        string versionText = CommonUtils.ReadTextFile(versionTextFileNameAndPath);

        if (versionText != null)
        {
            versionText = versionText.Trim(); //clean up whitespace if necessary
            string[] lines = versionText.Split('.');

            int majorVersion = int.Parse(lines[0]);
            int minorVersion = int.Parse(lines[1]);
            int subMinorVersion = int.Parse(lines[2]) + 1; //increment here
            string subVersionText = lines[3].Trim();

            //Debug.Log("Major, Minor, SubMinor, SubVerLetter: " + majorVersion + " " + minorVersion + " " + subMinorVersion + " " + subVersionText);

            versionText = majorVersion.ToString("0") + "." +
                          minorVersion.ToString("0") + "." +
                          subMinorVersion.ToString("0") + "." +
                          subVersionText;

            Debug.Log("Version Incremented " + versionText);

            //save the file (overwrite the original) with the new version number
            CommonUtils.WriteTextFile(versionTextFileNameAndPath, versionText);

            //tell unity the file changed (important if the versionTextFileNameAndPath is in the Assets folder)
            AssetDatabase.Refresh();
        }
        else
        {
            versionText = "0.0.0.a";
            //no file at that path, make it
            CommonUtils.WriteTextFile(versionTextFileNameAndPath, versionText);
        }

        if (buildPath != null)
        {
            string[] parts = buildPath.Split('\\');
            string finalBuildPath = "";
            for (int i = 0; i < parts.Length - 2; i++)
            {
                finalBuildPath += parts[i];
            }
            finalBuildPath += parts[parts.Length - 1].Replace(".exe", "_Data/") + "StreamingAssets/version.txt";
            Debug.Log(finalBuildPath);
            CommonUtils.WriteTextFile(finalBuildPath, versionText);
        }

        if (assign)
        {
            PlayerSettings.bundleVersion = versionText;
        }
    }
}