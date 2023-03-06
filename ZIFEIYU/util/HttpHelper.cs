﻿using Newtonsoft.Json;
using System.Text;

namespace ZIFEIYU.util
{
    public class HttpHelper
    {
        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            using (HttpClient client = new HttpClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    HttpResponseMessage response = client.PostAsync(url, httpContent).Result;
                    var code = ((int)response.StatusCode);
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        /// <summary>
        /// 发起POST异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            postData = postData ?? "";
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, timeOut);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    HttpResponseMessage response = await client.PostAsync(url, httpContent);


                    return await response.Content.ReadAsStringAsync();
                }
            }
        }

        /// <summary>
        /// POST同步返回实体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static T HttpPost<T>(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            return HttpPost(url, postData, contentType, timeOut, headers).ToEntity<T>();
        }

        /// <summary>
        /// POST异步返回实体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>
        /// <returns></returns>
        public static async Task<T> HttpPostAsync<T>(string url, string postData = null, string contentType = "application/json", int timeOut = 30, Dictionary<string, string> headers = null)
        {
            var res = await HttpPostAsync(url, postData, contentType, timeOut, headers);
            return res.ToEntity<T>();
        }

        /// <summary>
        ///发起GET同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <returns>状态码和返回数据</returns>
        public static string HttpGet(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (contentType != null)
                    client.DefaultRequestHeaders.Add("ContentType", contentType);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                HttpResponseMessage response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            using (HttpClient client = new HttpClient())
            {
                if (contentType != null)
                    client.DefaultRequestHeaders.Add("ContentType", contentType);
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                HttpResponseMessage response = await client.GetAsync(url);
                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        ///Get同步返回实体
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="url"></param>
        /// <param name="contentType"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static T HttpGet<T>(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            return HttpGet(url, contentType, headers).ToEntity<T>();
        }

        /// <summary>
        ///Get异步返回实体
        /// </summary>
        /// <typeparam name="T">返回实体类型</typeparam>
        /// <param name="url"></param>
        /// <param name="contentType">请求参数类型</param>
        /// <param name="headers">标头</param>
        /// <returns></returns>
        public static async Task<T> HttpGetAsync<T>(string url, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            var res = await HttpGetAsync(url, contentType, headers);
            return res.ToEntity<T>();
        }
    }

    /// <summary>
    /// Json扩展方法
    /// </summary>
    public static class JsonExtends
    {
        public static T ToEntity<T>(this string val)
        {
            return JsonConvert.DeserializeObject<T>(val);
        }

        //public static List<T> ToEntityList<T>(this string val)
        //{
        //    return JsonConvert.DeserializeObject<List<T>>(val);
        //}
        public static string ToJson<T>(this T entity, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(entity, formatting);
        }
    }
}