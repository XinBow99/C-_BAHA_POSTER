using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// login.xaml 的互動邏輯
    /// </summary>
    public partial class userinfo : Page
    {
        private MainWindow mainWindow;

        private encodeJsonForGamer.GamerObject GamerObject;
        private encodeJsonForGamer.MainBoard board;
        public userinfo(encodeJsonForGamer.GamerObject GamerObject, encodeJsonForGamer.MainBoard board, MainWindow mainWindow)
        {
            InitializeComponent();
            setObj(GamerObject);
            setBoard(board);
            this.mainWindow = mainWindow;
        }
        public void setObj(encodeJsonForGamer.GamerObject gamer)
        {
            this.GamerObject = gamer;

            UserName.Content = gamer.nickname;
            LV.Content = "勇者等級：" + gamer.lv;
            GP.Content = "ＧＰ數量：" + gamer.gp;
            gold.Content = "巴幣餘額：" + gamer.gold;
            userpic.Source = new BitmapImage(new Uri(gamer.avatar_s));
        }
        public void setBoard(encodeJsonForGamer.MainBoard board)
        {
            this.board = board;

            ObservableCollection<string> boardsn = new ObservableCollection<string>();
            foreach (var titl in board.subboards)
            {
                boardsn.Add(titl.title);
            }
            bsn.ItemsSource = boardsn;

            ObservableCollection<string> que = new ObservableCollection<string>();

            que.Add("問題");
            que.Add("情報");
            que.Add("心得");
            que.Add("討論");
            que.Add("攻略");
            que.Add("密技");
            que.Add("閒聊");
            que.Add("其他");
            que.Add("空白");
            question.ItemsSource = que;

            bsnid.Content = board.board_title;
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.post(
                ContentForBaha.Text,
                board.subboards[bsn.SelectedIndex].sn.ToString(),
                (question.SelectedIndex + 1).ToString(),
                titleB.Text,
                board.pwd,
                board.code
                );
            titleB.Text = "";
            ContentForBaha.Text = "";
        }
    }
}
