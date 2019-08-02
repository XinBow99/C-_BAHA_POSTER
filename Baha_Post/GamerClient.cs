using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text.RegularExpressions;

namespace Baha_Post
{
    public class GamerClient
    {
        //private
        //test
        private String user = "";
        private String board = "";
        private String postinfo = "";
        //main
        private string ac = "";
        private string pw = "";
        private string ckcode = "7045";
        private Uri baha_root = new Uri("https://api.gamer.com.tw");
        private HttpClient bahaclient;
        private HttpResponseMessage User_infor;
        private HttpResponseMessage board_response;
        private HttpResponseMessage post_response;
        private CookieContainer cookieContainer = new CookieContainer();
        //public
        //this
        public GamerClient(string ac, string pw)
        {
            //cookie

            cookieContainer.Add(baha_root, new Cookie("ckAPP_VCODE", this.ckcode));

            bahaclient = new HttpClient(new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }
            )
            {
                BaseAddress = baha_root
            };

            //帳號
            this.ac = ac;
            this.pw = pw;
            //cookieContainer
            //this.cookie = new CookieContainer();

            //set headers
            this.bahaclient.DefaultRequestHeaders.Add("user-Agent", "Bahadroid (https://www.gamer.com.tw/)");
            this.bahaclient.DefaultRequestHeaders.Add("x-bahamut-app-instanceid", "cc2zQIfDpg4");
            this.bahaclient.DefaultRequestHeaders.Add("x-bahamut-app-android", "tw.com.gamer.android.activecenter");
            this.bahaclient.DefaultRequestHeaders.Add("x-bahamut-app-version", "251");
            this.bahaclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            this.bahaclient.DefaultRequestHeaders.Add("accept-encoding", "gzip");
        }
        //login
        public void login_Baha()
        {
            List<KeyValuePair<String, String>> payload = new List<KeyValuePair<String, String>>();

            payload.Add(new KeyValuePair<string, string>("uid", this.ac));
            payload.Add(new KeyValuePair<string, string>("passwd", this.pw));
            payload.Add(new KeyValuePair<string, string>("vcode", this.ckcode));

            this.User_infor = this.bahaclient.PostAsync("/mobile_app/user/v3/do_login.php", new FormUrlEncodedContent(payload)).Result;
            this.User_infor.EnsureSuccessStatusCode();

            this.user = this.User_infor.Content.ReadAsStringAsync().Result;
        }
        public void Load_Board(string bid)
        {
            this.board_response = this.bahaclient.GetAsync("/mobile_app/forum/v2/post1.php?type=1&bsn=" + bid).Result;
            this.board_response.EnsureSuccessStatusCode();

            this.board = this.board_response.Content.ReadAsStringAsync().Result;
        }
        //post
        public void post_Board(string rtecontent, string nsubbsn, string subject, string bsn, string title, string pwd, string code)
        {
            var cookie = board_response.Headers.GetValues("Set-Cookie");
            cookieContainer.Add(baha_root, new Cookie("ckFROUM_MPOST", pwd));
            HttpClient Postclient = new HttpClient(new HttpClientHandler()
            {
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                CookieContainer = cookieContainer
            }
            )
            {
                BaseAddress = baha_root
            };
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"rtecontent", rtecontent},
                {"nsubbsn", nsubbsn},
                {"code", code},
                {"native", "0"},
                {"subject", subject},
                {"bsn", bsn},
                {"type", "1"},
                {"title", title},
                { "pwd", pwd},
            });
            this.post_response = Postclient.PostAsync("/mobile_app/forum/v2/post2.php", content).Result;
            this.post_response.EnsureSuccessStatusCode();
            
            this.postinfo = this.post_response.Content.ReadAsStringAsync().Result;
        }
        //getPost
        public String getPost()
        {
            return postinfo.ToString();
        }
        //getBoard
        public String getBoard()
        {
            return board.ToString();
        }
        //test
        public String GetUser_Info()
        {
            return Regex.Unescape(user);
        }
    }
}
