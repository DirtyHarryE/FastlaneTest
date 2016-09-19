using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System;

public class IncrementBuildNo
{

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        ReadVersionAndIncrement(pathToBuiltProject, true);
    }

    [MenuItem("Terahard/Increment Build Version")]
    private static void Increment()
    {
        ReadVersionAndIncrement();
    }
    [MenuItem("Terahard/Increment and Assign Build Version")]
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
        string buildTextFileNameAndPath = Application.streamingAssetsPath + "/build.txt";

        int buildInt = 0;
        string buildText = CommonUtils.ReadTextFile(buildTextFileNameAndPath);
        bool success = int.TryParse(buildText, out buildInt);

        if (success && buildText != null)
        {
            buildText = buildText.Trim(); //clean up whitespace if necessary
            int i = int.Parse(buildText);
            i += 1;
            buildInt = i;
            Debug.Log("Build Incremented " + buildInt);

            //save the file (overwrite the original) with the new version number
            CommonUtils.WriteTextFile(buildTextFileNameAndPath, buildInt.ToString());

            //tell unity the file changed (important if the versionTextFileNameAndPath is in the Assets folder)
            AssetDatabase.Refresh();
        }
        else
        {
            buildInt = 0;
            //no file at that path, make it
            CommonUtils.WriteTextFile(buildTextFileNameAndPath, buildInt.ToString());
        }

        if (buildPath != null)
        {
            string[] parts = buildPath.Split('\\');
            string finalBuildPath = "";
            for (int i = 0; i < parts.Length - 2; i++)
            {
                finalBuildPath += parts[i];
            }
            finalBuildPath += "/PersistentBuildData/build.txt";
            Debug.Log(finalBuildPath);
            CommonUtils.WriteTextFile(finalBuildPath, buildInt.ToString());
        }

        if (assign)
        {
            PlayerSettings.Android.bundleVersionCode = buildInt;
            PlayerSettings.iOS.buildNumber = buildInt.ToString();
        }
    }
}