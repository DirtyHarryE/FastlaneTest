﻿using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;
 
public class ChangeIOSEncryptionPList
{
    [PostProcessBuild]
    public static void ChangeEncryptionPList(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.Log("+++++++++++++++++++++++++++++++++++++++++++++++++++++\nTesting PostProcessBuild [" + buildTarget.ToString() + "]\n++++++++++++++++++++++++++++++++++++++++++++++++++++");
        if (buildTarget == BuildTarget.iOS)
        {
            // Get plist
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            // Get root
            PlistElementDict rootDict = plist.root;

            // Change value of CFBundleVersion in Xcode plist
            var buildKey = "ITSAppUsesNonExemptEncryption ";
            rootDict.SetString(buildKey, "true");

            // Write to file
            File.WriteAllText(plistPath, plist.WriteToString());

            Debug.Log("\nPLIST\n" + plist.ToString() + "\n");
        }
    }
}