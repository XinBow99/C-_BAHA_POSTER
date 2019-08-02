using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baha_Post
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        GamerClient gamer;
        encodeJsonForGamer info;

        Login login_form;
        userinfo User_info_page;
        public MainWindow()
        {
            InitializeComponent();
            this.login_form = new Login(this);
            frameC.Navigate(this.login_form);

        }

        public void Show_Msg(string msg)
        {
            MessageBox.Show(msg);
        }

        public bool login(string Ac, string Pw)
        {
            this.gamer = new GamerClient(Ac, Pw);
            this.gamer.login_Baha();
            this.info = new encodeJsonForGamer(gamer.GetUser_Info());
            bool result = true;
            if (info.CheckLogin())
            {
                //Show_Msg(info.gamerObject.nickname);
                gamer.Load_Board("60076");
                info.inject_board(gamer.getBoard());
                this.User_info_page = new userinfo(info.gamerObject, info.mainBoard, this);
                frameC.Navigate(User_info_page);
            }
            else
            {
                Show_Msg(info.loginFail.message);
                result = false;
            }
            return result;
        }
        public void post(string rtecontent, string nsubbsn, string subject, string title, string pwd, string code)
        {
            gamer.post_Board(rtecontent, nsubbsn, subject, "60076", title, pwd, code);
            info.inject_post(gamer.getPost());
            if (info.CheckPost())
            {
                Show_Msg("發佈完成：https://forum.gamer.com.tw/C.php?bsn=60076&snA=" + info.DoneObject.snA);
            }
            else
            {
                Show_Msg(info.PostFail.message);
            }
            gamer.Load_Board("60076");
            info.inject_board(gamer.getBoard());
            User_info_page.setBoard(info.mainBoard);
        }

    }
}
