using UnityEngine;
using System.Collections;
using System.IO;

public class CommonUtils
{
    public static string ReadTextFile(string sFileName)
    {
        //Debug.Log("Reading " + sFileName);

        //Check to see if the filename specified exists, if not try adding '.txt', otherwise fail
        string sFileNameFound = "";
        if (File.Exists(sFileName))
        {
            //Debug.Log("Reading '" + sFileName + "'.");
            sFileNameFound = sFileName; //file found
        }
        else if (File.Exists(sFileName + ".txt"))
        {
            sFileNameFound = sFileName + ".txt";
        }
        else
        {
            //Debug.Log("Could not find file '" + sFileName + "'.");
            return null;
        }

        StreamReader sr;
        try
        {
            sr = new StreamReader(sFileNameFound);
        }
        catch (System.Exception e)
        {
            //Debug.LogWarning("Something went wrong with read.  " + e.Message);
            return null;
        }

        string fileContents = sr.ReadToEnd();
        sr.Close();

        return fileContents;
    }

    public static void CreateDirectory(string path)
    {
        Debug.Log("Creating new Path : " + path);
        if (!File.Exists(path) || !Directory.Exists(path))
        {

            string[] dirs = path.Split('/');
            string str = "";
            bool lastDirAFile = dirs[dirs.Length - 1].Contains(".");
            int length = lastDirAFile ? dirs.Length - 1 : dirs.Length;
            for (int i = 0; i < length; i++)
            {
                str = i == 0 ? dirs[0] : str + "/" + dirs[i];
                Debug.Log("Create|" + dirs[i] + "|" + str + ":" + (!Directory.Exists(str)));
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
            }
        }
    }

    public static void WriteTextFile(string sFilePathAndName, string sTextContents)
    {
        CreateDirectory(sFilePathAndName);
        StreamWriter sw = new StreamWriter(sFilePathAndName);
        
        sw.WriteLine(sTextContents);
        sw.Flush();
        sw.Close();
    }
}