using Unity.Netcode;
using System.Net;
using System.IO;
using System.Text;
using AddressFamily = System.Net.Sockets.AddressFamily;
using UnityEngine;
using Unity.Netcode.Transports.UTP;
using InputSystem;

namespace Multiplayer
{
    public class ObtainIP : MonoBehaviour
    {
        public static ObtainIP instance;
        public string myAddressLocal;
        public string myAddressGlobal;

        public void Awake()
        {
            instance = this;

            //Get the local IP
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    myAddressLocal = ip.ToString();
                    break;
                }
            }
            //Get the global IP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.ipify.org");
            request.Method = "GET";
            request.Timeout = 1000; //time in ms
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                    myAddressGlobal = reader.ReadToEnd();
                }
                else
                {
                    Debug.LogError("Timed out? " + response.StatusDescription);
                    myAddressGlobal = "127.0.0.1";
                }
            }
            catch (WebException ex)
            {
                Debug.Log("Likely no internet connection: " + ex.Message);
                myAddressGlobal = "127.0.0.1";
            }
            Debug.Log(myAddressGlobal);
            Debug.Log(myAddressLocal);
            //GetComponent<UnityTransport>().ConnectionData.Address = myAddressLocal;
        }
    }
}