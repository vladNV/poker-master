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

        private bool finish;
        private bool playerCall;
        private long currentMaxBet = 0;
        // feature
        private bool isAllIn;

        public PokerWindow() { }
        public PokerWindow(int port, string login)
        {
            InitializeComponent();
            player = new PlayerModel();
            player.setLogin(login);
            player.setPort(port);
            player.setChips(1000);
            youLogin.Text += login;
            try
            {
                connect();
            } catch (Exception e)
            {
                MessageBox.Show("Server connection error!", "Error connection", 
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(e.Message);
                return;
            }
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
                throw e;
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
                string message = null;
                Thread.Sleep(2000);
                // сообщение для сервера;
                while (true)
                {
                    playerCall = false;
                    // если картинки карты не удаляется, доп финализация
                    if (finish)
                    {
                        finalize();
                        finish = false;
                    }
                    if (player.getChips() <= 0)
                    {
                        // lose
                        MessageBox.Show("Game over, all chips ended", "Game info",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
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
                    DrawCards(args);
                    Draw(args);
                    updateChips(args);
                    // нужно проверить, не повысил ли кто то ставку
                    // если да то делаем неактивной кнопку check
                    if (args[0].Equals("u_turn"))
                    {
                        if(!playerCall)
                        {
                            if (hasCheck(args))
                            {
                                active2();
                            }
                            else {
                                active();
                            }
                        } else {
                            active();
                        }
                        setText("you turn");
                        message = "action";
                    } else if (args[0].Equals("win")) {
                        Console.WriteLine(player.getLogin() + " won !");
                        string[] param = args[4].Split('|');
                        printWinners(extractMsgAboutWinners(param[1]));
                        Thread.Sleep(5000);
                        finalize();
                        finish = true;
                    } else {
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
        private void updateChips(string[] args)
        {
            string[] param;
            // достаем нужного игрока
            try
            {
                for (int i = 0; i < PLAYERS; i++)
                {
                    if (i == player.getNumber())
                    {
                        param = args[i + 1].Split('|');
                        // обновляем фишки
                        player.setChips(long.Parse(param[2].Split(':')[1]));
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("client controller, update chips exception");
                Console.WriteLine(e.Message);
            }

        }
        private bool hasCheck(string[] args)
        {
            string[] param;
            for (int i = 0; i < PLAYERS; i++)
            {
                if (i != player.getNumber())
                {
                    param = args[i + 1].Split('|');
                    // проверяем ставку
                    string[] call = param[3].Split(':');
                    // устанавливаем ставку
                    if (call[1].Equals("call")) {
                        currentMaxBet = long.Parse(call[2]);
                        return true;
                    }
                } 
            }
            return false;
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
        public void global_FormClosed(object sender, EventArgs e)
        {
            Application.Exit();
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
                long call = long.Parse(textBox1.Text);
                if (call > player.getChips())
                {
                    if (player.getChips() != 0 && player.getChips() < currentMaxBet)
                    {
                        // all in
                        isAllIn = true;
                        call = player.getChips();
                        player.setChips(1);
                    }
                    else
                    {
                        // у вас не достаточно фишек, проверка
                        MessageBox.Show("You cannot raise, so low chips", "Warning!",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        msg_action = "not_action";
                        return;
                    }
                }
                if (call < currentMaxBet)
                {
                    MessageBox.Show("You cannot raise, you must call >= " + currentMaxBet, "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    msg_action = "not_action";
                    return;
                }
                player.setChips(player.getChips() - call);
                playerCall = true;
                msg_action = "call:" + call;
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
        delegate void ActiveCallBack2();
        delegate void SetWinnerText(string text);
        delegate void Finalize();
        delegate void UnActiveCallBack();
        delegate void DrawCardsCallback(string[] args);
        delegate void DrawCallback(string[] args);
        delegate void SetStatus(string t);

        public void Draw(string[] args)
        {
            // fix draw
            if (player1cards.InvokeRequired && player2cards.InvokeRequired
                    && tableCard.InvokeRequired && player1.InvokeRequired
                        && p1act.InvokeRequired && p2act.InvokeRequired
                            && p1chips.InvokeRequired && p2chips.InvokeRequired
                                && bank.InvokeRequired)
            {
                DrawCallback d = new DrawCallback(Draw);
                Invoke(d, new object[] { args });
            }
            else
            {
                string[] param1 = args[1].Split('|');
                string[] param2 = args[2].Split('|');
                string[] table = args[3].Split('|');
                //player1cards.Text = param1[1];
                //player2cards.Text = param2[1];
                //tableCard.Text = table[1];

                p1chips.Text = param1[2];
                p2chips.Text = param2[2];
                bank.Text = table[2];

                // действия
                animation(param1[3], param2[3]);

            }
        }
        public void DrawCards(string[] args)
        {
            if (p1c1.InvokeRequired && p1c2.InvokeRequired
                    && p2c1.InvokeRequired && p2c2.InvokeRequired
                        && tc1.InvokeRequired && tc2.InvokeRequired
                            && tc3.InvokeRequired && tc4.InvokeRequired
                                && tc5.InvokeRequired)
            {
                DrawCardsCallback dc = new DrawCardsCallback(DrawCards);
                Invoke(dc, new object[] { args });
            }
            else
            {
                try
                {
                    string[] card1 = args[1].Split('|');
                    string[] card2 = args[2].Split('|');
                    string[] table = args[3].Split('|');

                    card1 = card1[1].Split(':');
                    card2 = card2[1].Split(':');
                    table = table[1].Split(':');

                    card1 = card1[1].Split(',');
                    card2 = card2[1].Split(',');
                    table = table[1].Split(',');

                    if (p1c1.ImageLocation == null && p1c2.ImageLocation == null)
                    {
                        if (player.getNumber() == 0)
                        {
                            p1c1.ImageLocation = CardImage.getResource(card1[0]);
                            p1c2.ImageLocation = CardImage.getResource(card1[1]);

                        }
                        else
                        {
                            p1c1.ImageLocation = CardImage.getBack();
                            p1c2.ImageLocation = CardImage.getBack();
                        }
                        p1c1.SizeMode = PictureBoxSizeMode.StretchImage;
                        p1c2.SizeMode = PictureBoxSizeMode.StretchImage;
                    }


                    if (p2c1.ImageLocation == null && p2c2.ImageLocation == null)
                    {
                        if (player.getNumber() == 1)
                        {
                            p2c1.ImageLocation = CardImage.getResource(card2[0]);
                            p2c2.ImageLocation = CardImage.getResource(card2[1]);

                        }
                        else
                        {
                            p2c1.ImageLocation = CardImage.getBack();
                            p2c2.ImageLocation = CardImage.getBack();
                        }
                        p2c1.SizeMode = PictureBoxSizeMode.StretchImage;
                        p2c2.SizeMode = PictureBoxSizeMode.StretchImage;

                    }

                    if (table.Length > 1)
                    {
                        if (tc1.ImageLocation == null)
                        {
                            tc1.ImageLocation = CardImage.getResource(table[0]);
                            tc1.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        if (table.Length >= 2)
                        {
                            if (tc2.ImageLocation == null)
                            {
                                tc2.ImageLocation = CardImage.getResource(table[1]);
                                tc2.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            if (table.Length >= 3)
                            {
                                if (tc3.ImageLocation == null)
                                {
                                    tc3.ImageLocation = CardImage.getResource(table[2]);
                                    tc3.SizeMode = PictureBoxSizeMode.StretchImage;
                                }

                                if (table.Length >= 4)
                                {
                                    if (tc4.ImageLocation == null)
                                    {
                                        tc4.ImageLocation = CardImage.getResource(table[3]);
                                        tc4.SizeMode = PictureBoxSizeMode.StretchImage;
                                    }
                                    if (table.Length == 5)
                                    {
                                        if (tc5.ImageLocation == null)
                                        {
                                            tc5.ImageLocation = CardImage.getResource(table[4]);
                                            tc5.SizeMode = PictureBoxSizeMode.StretchImage;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Draw cards exepction");
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void setText(string t)
        {
            if (status.InvokeRequired)
            {
                SetStatus s = new SetStatus(setText);
                this.Invoke(s, new object[] { t });
            }
            else
            {
                status.Text = t;
            }
        }
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

        private void active2()
        {
            if (button1.InvokeRequired && button2.InvokeRequired
                && button3.InvokeRequired && textBox1.InvokeRequired)
            {
                ActiveCallBack2 a2 = new ActiveCallBack2(active2);
                this.Invoke(a2, new object[] { });
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                textBox1.Enabled = true;
            }
        }

        private void printWinners(string text)
        {
            winner.Text = "WINNERS: ";
            if (winner.InvokeRequired)
            {
                SetWinnerText s = new SetWinnerText(printWinners);
                Invoke(s, new object[] { text });
            } else
            {
                winner.Text += text;
            }
        }

        private void finalize()
        {
            if (player1cards.InvokeRequired && player2cards.InvokeRequired
                && tableCard.InvokeRequired && p1c1.InvokeRequired && p1c2.InvokeRequired
                    && p2c1.InvokeRequired && p2c2.InvokeRequired
                        && tc1.InvokeRequired && tc2.InvokeRequired
                            && tc3.InvokeRequired && tc4.InvokeRequired && tc5.InvokeRequired)
            {
                Finalize f = new Finalize(finalize);
                Invoke(f, new object[] { });
            } else
            {
                isAllIn = false;
                player1cards.Text = "";
                player2cards.Text = "";
                tableCard.Text = "Card on table:";
                winner.Text = "Winners: ";
                msg_action = "";
                p1c1.ImageLocation = null;
                p1c2.ImageLocation = null;

                p2c1.ImageLocation = null;
                p2c2.ImageLocation = null;

                tc1.ImageLocation = null;
                tc2.ImageLocation = null;

                tc3.ImageLocation = null;
                tc4.ImageLocation = null;
                tc5.ImageLocation = null;

            }
        }

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
    }
}
