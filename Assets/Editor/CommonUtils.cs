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
        if (!File.Exists(path))
        {

            string[] dirs = path.Split('/');
            string str = "";
            for (int i = 0; i < dirs.Length - 1; i++)
            {
                str = i == 0 ? dirs[0] : str + "/" + dirs[i];
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
            }
        }
    }

    public static void WriteTextFile(string sFilePathAndName, string sTextContents)
    {
        if (!File.Exists(sFilePathAndName))
        {

            string[] dirs = sFilePathAndName.Split('/');
            string str = "";
            for (int i = 0; i < dirs.Length - 1; i++)
            {
                str = i == 0 ? dirs[0] : str + "/" + dirs[i];
                if (!Directory.Exists(str))
                {
                    Directory.CreateDirectory(str);
                }
            }
        }
        StreamWriter sw = new StreamWriter(sFilePathAndName);
        
        sw.WriteLine(sTextContents);
        sw.Flush();
        sw.Close();
    }
}