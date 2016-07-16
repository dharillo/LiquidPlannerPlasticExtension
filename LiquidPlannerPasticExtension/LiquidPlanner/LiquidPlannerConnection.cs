using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace LiquidPlannerPasticExtension.LiquidPlanner
{
    internal class LiquidPlannerConnection
    {
        public string Hostname { get; set; }

        private string password;

        public string UserName { get; set; }
        public string Password { get { return null; } set { password = value; } }
        public int WorkspaceId { get; set; }

        public LiquidPlannerConnection(string username, string password)
        {
            this.UserName = username;
            this.password = password;
        }

        public Response Get(string url)
        {
            return Request("GET", url);
        }

        public Response Post(string url, object data)
        {
            return Request("POST", url, data);
        }

        public Response Put(string url, object data)
        {
            return Request("PUT", url, data);
        }

        private Response Request(string verb, string url, object data = null)
        {
            HttpWebRequest request;
            string uri;

            uri = "https://app.liquidplanner.com/api" + url;

            request = CreateRequest(uri, verb, data);
            return ParseWebResponse(request);
        }

        private HttpWebRequest CreateRequest(string uri, string method, object data = null)
        {
            HttpWebRequest request;
            request = WebRequest.Create(uri) as HttpWebRequest;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = method;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Headers.Set("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(this.UserName + ":" + this.password)));

            // If there is additional data to send, serialize it to JSON and send it
            // using the request stream.
            if (data != null)
            {
                request.ContentType = "application/json";
                string jsonPayload = JsonConvert.SerializeObject(data);
                Console.WriteLine(jsonPayload);
                byte[] jsonPayloadByteArray = Encoding.ASCII.GetBytes(jsonPayload.ToCharArray());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(jsonPayloadByteArray, 0, jsonPayloadByteArray.Length);
                    stream.Close();
                }
            }

            return request;
        }

        private Response ParseWebResponse(HttpWebRequest request)
        {
            Response lpResponse = new Response();
            try
            {
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (Stream stream = webResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            lpResponse.response = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                lpResponse.error = e;
            }

            return lpResponse;
        }

        /// <summary>
        /// Gets the account information of the logged user.
        /// </summary>
        /// <returns>Information of the logget user account</returns>
        public Member GetAccount()
        {
            return GetObject<Member>(Get("/account"));
        }

        public List<Workspace> GetWorkspaces()
        {
            return GetObject<List<Workspace>>(Get("/workspaces"));
        }

        public List<Item> GetProjects()
        {
            return GetObject<List<Item>>(Get("/workspaces/" + this.WorkspaceId + "/projects"));
        }

        public List<Item> GetTasks()
        {
            return GetObject<List<Item>>(Get("workspaces/" + this.WorkspaceId + "/tasks"));
        }

        public Item CreateTask(BaseObject data)
        {
            return GetObject<Item>(Post("/workspaces/" + this.WorkspaceId + "/tasks/" + data.Id, new
                {
                    task = data
                }));
        }

        /// <summary>
        /// Converts the data stored in the <see cref="Response"/> instance
        /// given to the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize from the
        /// response data given.</typeparam>
        /// <param name="response">Response instance with the information to
        /// extract</param>
        /// <returns>Deserialized object extracted from the <see cref="Response.response"/>
        /// field.</returns>
        /// <exception cref="Exception">If the response has any error.</exception>
        public T GetObject<T>(Response response)
        {
            if (response.error != null)
                throw response.error;

            Console.WriteLine(response.response);
            return JsonConvert.DeserializeObject<T>(response.response);
        }
    }
}
