using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TexasHoldemClient
{
    public partial class PokerWindow : Form
    {
        private PlayerModel player;
        private int PLAYERS = 2;
        private string address = "127.0.0.1";
        private string msg_action = "not_action";
        public PokerWindow() { }
        public PokerWindow(int port, string login)
        {
            InitializeComponent();
            player = new PlayerModel();
            player.setLogin(login);
            player.setPort(port);
            player.setChips(1000);
            youLogin.Text += login;
            connect();
            wait();
            Thread thread = new Thread(new ParameterizedThreadStart(play));
            thread.Start("stage");
        }

        private void connect()
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), player.getPort());
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                string message = "connect|" + player.getLogin() + "|" + player.getChips();
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
                data = new byte[1024];
                string resp;
                int bytes = 0;
                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    resp = Encoding.Unicode.GetString(data, 0, bytes);
                } while (socket.Available > 0);
                Console.WriteLine("server answer: " + resp);
                player.setNumber(int.Parse(resp));
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        private void wait()
        {
            try
            {
                while(true)
                {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), player.getPort());
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    string message = "game";
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[1024];
                    string resp;
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        resp = Encoding.Unicode.GetString(data, 0, bytes);
                    } while (socket.Available > 0);
                    Console.WriteLine("server answer: " + resp);
                    if(resp.Equals("no_players"))
                    {
                        Console.WriteLine("wait ...");
                        continue;
                    } else
                    {
                        resp = resp.Substring(0, resp.Length - 1);
                        string[] args = resp.Split('|');
                        for (int i = 0; i < PLAYERS; i++)
                        {
                            selectLabel(i, args[i]);
                        }
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void play(object stage_param)
        {
            try
            {
                Thread.Sleep(2000);
                // сообщение для сервера;
                string message = null;
                while (true)
                {
                    string stage = (string)stage_param;
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), player.getPort());
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(ipPoint);
                    if(string.IsNullOrEmpty(message))
                    {
                        // отправляем данные о себе
                        message = stage + "|n:" + player.getNumber();
                    } else {
                        // иначе формируем запрос действие
                        message = "action|" + msg_action + "|n:" + player.getNumber();
                        msg_action = "not_action";
                    }
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);
                    data = new byte[1024];
                    string resp;
                    int bytes = 0;
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        resp = Encoding.Unicode.GetString(data, 0, bytes);
                    } while (socket.Available > 0);
                    // получаем ответ от сервера
                    // args[0] - status
                    string[] args = resp.Split(new[] { "||" }, StringSplitOptions.None);
                    Draw(args);
                    if (args[0].Equals("u_turn")) {
                        active();
                        setText("you turn");
                        message = "action";
                    }
                    else if (args[0].Equals("wait_action")) {
                        // ожидаем действие игрока
                        continue;
                    } 
                    else if (args[0].Equals("win"))
                    {
                        Console.WriteLine(player.getLogin() + " won !");
                        string[] param = args[4].Split('|');
                        printWinners(extractMsgAboutWinners(param[1]));
                        Thread.Sleep(4000);
                        finalize();
                    }
                    else {
                        setText("you wait");
                        unactive();
                        // очищаем и готовимся к новому запросу
                        message = "";
                        continue;
                    }
                    // if
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    Thread.Sleep(2000);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string extractMsgAboutWinners(string arg)
        {
            string[] args = arg.Split(',');
            arg = "";
            foreach(string s in args){
                arg += s;
            }
            return arg;

        }

        delegate void SetStatus(string t);

        public void setText(string t)
        {
            if (status.InvokeRequired)
            {
                SetStatus s = new SetStatus(setText);
                this.Invoke(s, new object[] {t });
            }
            else
            {
                status.Text = t;
            }
        }

        public void global_FormClosed(object sender, EventArgs e)
        {
            Application.Exit();
        }

        delegate void DrawCallback(string[] args);

        public void Draw(string[] args)
        {
            // fix draw
            if(player1cards.InvokeRequired && player2cards.InvokeRequired
                    && tableCard.InvokeRequired && player1.InvokeRequired 
                        && p1act.InvokeRequired && p2act.InvokeRequired
                            && p1chips.InvokeRequired && p2chips.InvokeRequired
                                && bank.InvokeRequired)
            {
                DrawCallback d = new DrawCallback(Draw);
                Invoke(d, new object[] { args });
            } else
            {
                string[] param1 = args[1].Split('|');
                string[] param2 = args[2].Split('|');
                string[] table = args[3].Split('|');
                player1cards.Text = param1[1];
                player2cards.Text = param2[1];
                tableCard.Text = table[1];

                p1chips.Text = param1[2];
                p2chips.Text = param2[2];
                bank.Text = table[2];

                // действия
                animation(param1[3], param2[3]);
  
            }
        }

        // fix me, for many players
        private void animation(string p1, string p2)
        {
            p1act.Text = p1;
            p2act.Text = p2;

        }

        public void selectLabel(int number, string text)
        {
            switch(number)
            {
                case 0:
                    player1.Text = text;
                    break;
                case 1:
                    player2.Text = text;
                    break;
                default:
                    break;
            }
        }

        //check
        private void button1_Click(object sender, EventArgs e)
        {
            // делаем action:check
            msg_action = "check";
            unactive();
            // и делаем кнопки неактивными
        }

        // call
        private void button2_Click(object sender, EventArgs e)
        {

            // сделать проверку на ввод символов и сделать проверку на границы!!!
            // проверяем введено ли значение
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                int call = int.Parse(textBox1.Text);
                if(call > player.getChips())
                {
                    // у вас не достаточно фишек, проверка
                    return;
                } else
                {
                    player.setChips(player.getChips() - call);
                    msg_action = "call:" + call;
                }
                unactive();
            }
        }

        // fold
        private void button3_Click(object sender, EventArgs e)
        {
            msg_action = "fold";
            unactive();
        }

        delegate void ActiveCallBack();

        private void active()
        {
            if (button1.InvokeRequired && button2.InvokeRequired
                && button3.InvokeRequired && textBox1.InvokeRequired)
            {
                ActiveCallBack a = new ActiveCallBack(active);
                this.Invoke(a, new object[] {});
            } else
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        delegate void SetWinnerText(string text);

        private void printWinners(string text)
        {
            if (winner.InvokeRequired)
            {
                SetWinnerText s = new SetWinnerText(printWinners);
                Invoke(s, new object[] { text });
            } else
            {
                winner.Text += text;
            }
        }

        delegate void Finalize();

        private void finalize()
        {
            if (player1cards.InvokeRequired && player2cards.InvokeRequired
                && tableCard.InvokeRequired)
            {
                Finalize f = new Finalize(finalize);
                Invoke(f, new object[] { });
            } else
            {
                player1cards.Text = "";
                player2cards.Text = "";
                tableCard.Text = "Card on table:";
                winner.Text = "Winners: ";
                msg_action = "";
            }
        }

        delegate void UnActiveCallBack();

        private void unactive()
        {
            if (button1.InvokeRequired && button2.InvokeRequired
                && button3.InvokeRequired && textBox1.InvokeRequired)
            {
                UnActiveCallBack ua = new UnActiveCallBack(unactive);
                this.Invoke(ua, new object[] { });
            } else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                textBox1.Enabled = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void p2chips_Click(object sender, EventArgs e)
        {

        }
    }
}
