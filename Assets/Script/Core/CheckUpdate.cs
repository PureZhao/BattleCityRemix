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
        // 等待两秒 等待BundleList.json
        yield return new WaitForSeconds(2f);
        // 获取所有bundle
        string json = File.ReadAllText(GlobalConfig.BundleListLocalUrl);
        JsonData data = JsonMapper.ToObject(json);
        data.SetJsonType(JsonType.Array);
        for(int i = 1;i < data.Count; i++)
        {
            // 用Gitee，raw.githubusercontent.com有时候抽风连不上
            string httpUrl = Path.Combine(GlobalConfig.AssetBundleServerUrlGitee, data[i].ToString());
            string filename = Path.GetFileName(httpUrl);
            string localUrl = Path.Combine(GlobalConfig.AssetBundleDir, data[i].ToString());
            string localDir = Path.GetDirectoryName(localUrl);
            // 后面做一个进度条界面
            float progress = 0f;
            UnityWebRequest request = UnityWebRequest.Get(httpUrl);
            //Debug.Log("下载 " + httpUrl);
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
                    //Debug.Log("下载完成 " + httpUrl);
                    progress = 1;
                    byte[] bytes = request.downloadHandler.data;
                    SaveAsset(localDir, filename, bytes);
                }
            }
        }
        
    }

    /// <summary>
    /// 保存资源到本地
    /// </summary>
    /// <param name="path">本地保存路径</param>
    /// <param name="filename">文件名称带后缀</param>
    /// <param name="bytes">byte数据</param>
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
            //如果文件存在删除该文件
            File.Delete(filePath);
            //Debug.Log("删除文件:" + filePath);
        }
        sw = fileInfo.Create();
        sw.Write(bytes, 0, bytes.Length);
        sw.Flush();
        sw.Close();
        sw.Dispose();
        //Debug.Log(filename + "成功保存到本地~");
        action?.Invoke();
    }

}
