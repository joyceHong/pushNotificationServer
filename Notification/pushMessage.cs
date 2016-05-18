using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Notification
{
    public class pushMessage
    {

        public string SendAndroidNotification(string deviceId, string message)
        {
            string GoogleAppID = "AIzaSyCpz1C_OxVakmtYpy_OWkW8RQyLn9bY5hc"; //google server ikey
            var SENDER_ID = "5695645179";
            var value = System.Web.HttpUtility.UrlEncode(message); //如果有傳遞中文，需要編碼

            WebRequest tRequest;
            tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = " application/x-www-form-urlencoded;charset=utf-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
            tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));

            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";
        
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);
            String sResponseFromServer = tReader.ReadToEnd();
            tReader.Close();
            dataStream.Close();
            tResponse.Close();
            return sResponseFromServer;
        }


        public bool SendIPhoneNotification(string deviceID, string message)
        {

            int port = 2195;
            String hostname = "gateway.sandbox.push.apple.com";

            String certificatePath = HttpContext.Current.Server.MapPath("~/Certificate/ckDeveloper.p12");
            X509Certificate2 clientCertificate = new X509Certificate2(System.IO.File.ReadAllBytes(certificatePath), "16145679");
            X509Certificate2Collection certificatesCollection = new X509Certificate2Collection(clientCertificate);

            TcpClient client = new TcpClient(hostname, port);
            SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            try
            {
                //sslStream.AuthenticateAsClient(hostname, certificatesCollection, SslProtocols.Default, false);

                sslStream.AuthenticateAsClient(hostname, certificatesCollection, System.Security.Authentication.SslProtocols.Tls, true);

                MemoryStream memoryStream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(memoryStream);
                writer.Write((byte)0);
                writer.Write((byte)0);
                writer.Write((byte)32);

                writer.Write(HexStringToByteArray(deviceID.ToUpper()));
                String payload = "{\"aps\":{\"alert\":\"" + message + "\",\"badge\":1,\"sound\":\"default\"}}";
                writer.Write((byte)0);
                writer.Write((byte)payload.Length);
                byte[] b1 = System.Text.Encoding.UTF8.GetBytes(payload);
                writer.Write(b1);
                writer.Flush();
                byte[] array = memoryStream.ToArray();
                sslStream.Write(array);
                sslStream.Flush();
                client.Close();
                return true;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                client.Close();
                return false;
            }
            catch (Exception e)
            {
                client.Close();
                return false;
            }
        }

        private static byte[] HexStringToByteArray(String DeviceID)
        {
            //convert Devide token to HEX value.
            byte[] deviceToken = new byte[DeviceID.Length / 2];
            for (int i = 0; i < deviceToken.Length; i++)
                deviceToken[i] = byte.Parse(DeviceID.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);

            return deviceToken;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else // Do not allow this client to communicate with unauthenticated servers. 
                return false;
        }
    }
}
