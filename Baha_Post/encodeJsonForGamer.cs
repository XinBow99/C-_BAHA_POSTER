using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace Baha_Post
{
    public class encodeJsonForGamer
    {
        private JObject Loginobject;
        public GamerObject gamerObject;
        public LoginFail loginFail;
        public LoginFail PostFail;
        //login
        public class LoginFail
        {
            public int code { get; set; }
            public string message { get; set; }
        }

        public class GamerObject
        {
            public bool success { get; set; }
            public string userid { get; set; }
            public string nickname { get; set; }
            public int gold { get; set; }
            public int gp { get; set; }
            public string avatar { get; set; }
            public string avatar_s { get; set; }
            public int lv { get; set; }
        }
        /***************************************/
        //board
        /*
         * 給取得板塊使用之OBJ
         */
        private JObject Boardobject;
        public MainBoard mainBoard;
        public class Subboard
        {
            public int sn { get; set; }
            public string subbsn { get; set; }
            public string title { get; set; }
            public bool islock { get; set; }
            public string isdisplay { get; set; }
        }

        public class Snippet
        {
            public int sn { get; set; }
            public string title { get; set; }
            public int uses { get; set; }
            public string thumbnail { get; set; }
            public string content_html { get; set; }
            public string post_title { get; set; }
            public int post_subbsn { get; set; }
            public int post_subject { get; set; }
            public string ctime { get; set; }
        }

        public class MainBoard
        {
            public string board_title { get; set; }
            public int type { get; set; }
            public string code { get; set; }
            public List<Subboard> subboards { get; set; }
            public string pwd { get; set; }
            public int bsn { get; set; }
            public List<Snippet> snippets { get; set; }
        }
        public encodeJsonForGamer(string info)
        {
            this.Loginobject = JObject.Parse(info.Replace("\\", ""));
        }
        //發佈完成之所需
        private JObject Pobject;
        public doneObject DoneObject;
        public class doneObject
        {
            public int bsn { get; set; }
            public string snA { get; set; }
            public int top { get; set; }
            public int renum { get; set; }
            public int gp { get; set; }
            public int del { get; set; }
            public int mindflag { get; set; }
            public int daren { get; set; }
            public string dreason { get; set; }
            public string thumbnail { get; set; }
            public string summary { get; set; }
            public string title { get; set; }
            public string ctime { get; set; }
            public int reply_timestamp { get; set; }
            public int locked { get; set; }
        }
        //確認正確性
        public bool CheckLogin()
        {
            bool result = false;
            JToken token = Loginobject["code"];
            if (token == null)
            {
                result = !(result);
                gamerObject = Loginobject.ToObject<GamerObject>();
            }
            else
            {
                loginFail = Loginobject.ToObject<LoginFail>();
            }
            return result;
        }
        public bool CheckPost()
        {
            bool result = false;
            JToken token = Pobject["code"];
            if (token == null)
            {
                result = !(result);
                DoneObject = Pobject.ToObject<doneObject>();
            }
            else
            {
                PostFail = Pobject.ToObject<LoginFail>();
            }
            return result;
        }
        //board
        public void inject_board(string board)
        {
            this.Boardobject = JObject.Parse(board);
            mainBoard = Boardobject.ToObject<MainBoard>();
        }
        //post
        public void inject_post(string board)
        {
            this.Pobject = JObject.Parse(board);
        }
    }
}
