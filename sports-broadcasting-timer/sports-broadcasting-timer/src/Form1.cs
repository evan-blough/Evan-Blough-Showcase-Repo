using System;
using System.Windows.Forms;


namespace basketball_app
{
    public partial class Scoreboard : Form
    {
        private string currentTime;

        public ScoreHandler score = new ScoreHandler();
        public FoulHandler fouls = new FoulHandler();
        public TimeoutHandler timeouts = new TimeoutHandler();
        public TimeHandler time = new TimeHandler();

        public Scoreboard()
        {
            InitializeComponent();
            SetupText();
            time.InitializeTimer(ChangeTime);
        }
        public void SetupText()
        {
            label1.Text = score.HomeTeamScore.ToString();
            label2.Text = score.AwayTeamScore.ToString();
            label7.Text = fouls.HomeTeamFouls.ToString();
            label8.Text = fouls.AwayTeamFouls.ToString();
            label11.Text = timeouts.HomeTeamTimeouts.ToString();
            label12.Text = timeouts.AwayTeamTimeouts.ToString();
            label18.Text = time.FindTime();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = score.UpdateScore(true, 1).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = score.UpdateScore(true, 2).ToString();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = score.UpdateScore(true, 3).ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            label2.Text = score.UpdateScore(false, 1).ToString();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            label2.Text = score.UpdateScore(false, 2).ToString();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            label2.Text = score.UpdateScore(false, 3).ToString();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            label1.Text = score.UpdateScore(true, -1).ToString();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            label2.Text = score.UpdateScore(false, -1).ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            score.ResetScore();
            label1.Text = score.HomeTeamScore.ToString();
            label2.Text = score.AwayTeamScore.ToString();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            label7.Text = fouls.AddFouls(true);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label8.Text = fouls.AddFouls(false);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            fouls.ResetFouls();
            label7.Text = fouls.HomeTeamFouls.ToString();
            label8.Text = fouls.AwayTeamFouls.ToString();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            label7.Text = fouls.RemoveFouls(true);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            label8.Text = fouls.RemoveFouls(false);
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            label11.Text = timeouts.AddTimeout(true);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label12.Text = timeouts.AddTimeout(false);
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            label11.Text = timeouts.RemoveTimeout(true);
        }
        private void button8_Click_1(object sender, EventArgs e)
        {
            label12.Text = timeouts.RemoveTimeout(false);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            timeouts.ResetTimeouts();
            label11.Text = timeouts.HomeTeamTimeouts.ToString();
            label12.Text = timeouts.AwayTeamTimeouts.ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            time.timer.Start();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            time.timer.Stop();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            time.timer.Stop();
            time.time = time.quarterTime;
            time.quarter = time.quarter >= 4 ? 1 : time.quarter += 1;
            label18.Text = time.FindTime();
            label15.Text = time.quarter.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            time.timer.Stop();
            label18.Text = time.quarter == 2 ? time.SetHalftime() : (time.quarter == 4 ? time.ResetClock() : time.FindTime());

            label15.Text = time.quarter.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            time.timer.Stop();
            label18.Text = time.ResetTime();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            label18.Text = time.AddOneMinute();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            label18.Text = time.SubtractOneMinute();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            label18.Text = time.AddOneSecond();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            label18.Text = time.SubtractOneSecond();
        }

        public void ChangeTime(string currTime)
        {
            currentTime = currTime;

            if (label18.InvokeRequired)
            {
                label18.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (currentTime != label18.Text)
                        label18.Text = currentTime;
                });
            }
            else
            {
                if (currentTime != label18.Text)
                    label18.Text = currentTime;
            }
        }

    }
}
