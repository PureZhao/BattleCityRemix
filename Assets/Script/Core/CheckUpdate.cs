using LitJson;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CheckUpdate : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DownloadBundleList());
    }

    IEnumerator DownloadBundleList()
    {
        float progress = 0f;
        UnityWebRequest request = UnityWebRequest.Get(GlobalConfig.BundleListUrl);
        string filename = (Path.GetFileName(GlobalConfig.BundleListUrl));
        request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {

            Debug.LogError(request.error);
        }
        else
        {
            while (!request.isDone)
            {
                progress = request.downloadProgress * 100f;
                yield return 0;
            }
            if (request.isDone)
            {
                progress = 1;
                byte[] bytes = request.downloadHandler.data;
                SaveAsset(GlobalConfig.AssetBundleDir, filename, bytes, () =>
                {
                    StartCoroutine(DownloadAssets());
                });
            }
        }
    }


    IEnumerator DownloadAssets()
    {
        // �ȴ����� �ȴ�BundleList.json
        yield return new WaitForSeconds(2f);
        // ��ȡ����bundle
        string json = File.ReadAllText(GlobalConfig.BundleListLocalUrl);
        JsonData data = JsonMapper.ToObject(json);
        data.SetJsonType(JsonType.Array);
        for(int i = 1;i < data.Count; i++)
        {
            // ��Gitee��raw.githubusercontent.com��ʱ����������
            string httpUrl = Path.Combine(GlobalConfig.AssetBundleServerUrlGitee, data[i].ToString());
            string filename = Path.GetFileName(httpUrl);
            string localUrl = Path.Combine(GlobalConfig.AssetBundleDir, data[i].ToString());
            string localDir = Path.GetDirectoryName(localUrl);
            // ������һ������������
            float progress = 0f;
            UnityWebRequest request = UnityWebRequest.Get(httpUrl);
            //Debug.Log("���� " + httpUrl);
            request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {

                Debug.LogError(request.error);
            }
            else
            {
                while (!request.isDone)
                {
                    progress = request.downloadProgress * 100f;
                    yield return 0;
                }
                if (request.isDone)
                {
                    //Debug.Log("������� " + httpUrl);
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        
    }

    /// <summary>
    /// ������Դ������
    /// </summary>
    /// <param name="path">���ر���·��</param>
    /// <param name="filename">�ļ����ƴ���׺</param>
    /// <param name="bytes">byte����</param>
    public void SaveAsset(string path, string filename, byte[] bytes, Action action = null)
    {
        string filePath = Path.Combine(path, filename);
        Stream sw = null;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
        {
            //����ļ�����ɾ�����ļ�
            File.Delete(filePath);
            //Debug.Log("ɾ���ļ�:" + filePath);
        }
        sw = fileInfo.Create();
        sw.Write(bytes, 0, bytes.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
        //Debug.Log(filename + "�ɹ����浽����~");
        action?.Invoke();
    }

}
