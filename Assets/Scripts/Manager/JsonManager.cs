using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;

public class JsonManager : MonoBehaviour
{
    public Configuration m_Configuration;

    private string filePath;
    private FileInfo fileInfo;

    public void Awake()
    {
        filePath = Application.persistentDataPath + "//config.json";
        fileInfo = new FileInfo(filePath);
        LoadConfig();
    }
    public void LoadConfig()
    {
        //如果不存在配置文件，则用当前默认值创建一个
        if(!fileInfo.Exists)
        {
            m_Configuration = new Configuration();
            SaveConfig();
        }
        else
        {
            StreamReader m_SR;
            m_SR = fileInfo.OpenText();
            JsonReader m_JR=new JsonReader(m_SR.ReadToEnd());

            m_Configuration = JsonMapper.ToObject<Configuration>(m_JR);

            m_SR.Close();
            m_SR.Dispose();
        }
    }

    public void SaveConfig()
    {
        File.Create(filePath).Close();
        StreamWriter m_SW;
        m_SW = fileInfo.AppendText();

        StringBuilder m_SB = new StringBuilder();
        JsonWriter jsonWriter = new JsonWriter(m_SB);
        jsonWriter.PrettyPrint = true;
        jsonWriter.IndentValue = 2;
        JsonMapper.ToJson(m_Configuration, jsonWriter);
        m_SW.Write(m_SB.ToString());
        m_SW.Close();
        m_SW.Dispose();
    }
}
