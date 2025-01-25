using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace basketball_app
{
    public class ScoreHandler
    {
        public int HomeTeamScore = 0;
        public int AwayTeamScore = 0;
        StreamWriter sw;


        public string UpdateScore(bool isHome, int toAdd)
        {
            if (isHome)
                HomeTeamScore += toAdd;
            else
                AwayTeamScore += toAdd;

            WriteScore(isHome);

            return isHome ? HomeTeamScore.ToString() : AwayTeamScore.ToString();
        }

        public void ResetScore()
        {
            HomeTeamScore = 0;
            AwayTeamScore = 0;

            WriteScore(true);
            WriteScore(false);
        }

        public void WriteScore(bool isHome)
        {
            if (isHome)
            {
                sw = new StreamWriter("homeScore.txt");
                sw.Write(HomeTeamScore);
                sw.Close();
            }
            else
            {
                sw = new StreamWriter("awayScore.txt");
                sw.Write(AwayTeamScore);
                sw.Close();
            }
        }
    }

    public class FoulHandler
    {
        public int HomeTeamFouls = 0;
        public int AwayTeamFouls = 0;
        StreamWriter sw;


        public string AddFouls(bool isHome)
        {
            if (isHome)
                HomeTeamFouls += 1;
            else
                AwayTeamFouls += 1;

            WriteFouls(isHome);

            return isHome ? HomeTeamFouls.ToString() : AwayTeamFouls.ToString();
        }

        public string RemoveFouls(bool isHome)
        {
            if (isHome)
                HomeTeamFouls = HomeTeamFouls == 0 ? 0 : HomeTeamFouls - 1;
            else
                AwayTeamFouls = AwayTeamFouls == 0 ? 0 : AwayTeamFouls - 1;

            WriteFouls(isHome);

            return isHome ? HomeTeamFouls.ToString() : AwayTeamFouls.ToString();
        }

        public void ResetFouls()
        {
            HomeTeamFouls = 0;
            AwayTeamFouls = 0;

            WriteFouls(true);
            WriteFouls(false);
        }

        public void WriteFouls(bool isHome)
        {
            if (isHome)
            {
                sw = new StreamWriter("homeFouls.txt");
                sw.Write(HomeTeamFouls);
                sw.Close();
            }
            else
            {
                sw = new StreamWriter("awayFouls.txt");
                sw.Write(AwayTeamFouls);
                sw.Close();
            }
        }
    }

    public class TimeoutHandler
    {
        public int HomeTeamTimeouts = 5;
        public int AwayTeamTimeouts = 5;
        StreamWriter sw;

        public string AddTimeout(bool isHome)
        {
            Debug.WriteLine($"Home: {HomeTeamTimeouts}");
            Debug.WriteLine($"Away: {AwayTeamTimeouts}");
            if (isHome)
                HomeTeamTimeouts += 1;
            else
                AwayTeamTimeouts += 1;

            WriteTimeouts(isHome);

            return isHome ? HomeTeamTimeouts.ToString() : AwayTeamTimeouts.ToString();
        }

        public string RemoveTimeout(bool isHome)
        {
            Debug.WriteLine($"Home: {HomeTeamTimeouts}");
            Debug.WriteLine($"Away: {AwayTeamTimeouts}");
            if (isHome)
                HomeTeamTimeouts = HomeTeamTimeouts == 0 ? 0 : HomeTeamTimeouts - 1;
            else
                AwayTeamTimeouts = AwayTeamTimeouts == 0 ? 0 : AwayTeamTimeouts - 1;

            WriteTimeouts(isHome);

            return isHome ? HomeTeamTimeouts.ToString() : AwayTeamTimeouts.ToString();
        }

        public void ResetTimeouts()
        {
            Debug.WriteLine($"Home: {HomeTeamTimeouts}");
            Debug.WriteLine($"Away: {AwayTeamTimeouts}");
            HomeTeamTimeouts = 5;
            AwayTeamTimeouts = 5;
            WriteTimeouts(true);
            WriteTimeouts(false);
        }

        public void WriteTimeouts(bool isHome)
        {
            if (isHome)
            {
                sw = new StreamWriter("homeTimeouts.txt");
                sw.Write(HomeTeamTimeouts);
                sw.Close();
            }
            else
            {
                sw = new StreamWriter("awayTimeouts.txt");
                sw.Write(AwayTeamTimeouts);
                sw.Close();
            }
        }
    }

    public class TimeHandler
    {
        public Timer timer = new Timer();
        Action<string> changeTime;
        public int quarterTime = 4800;
        public int halfTime = 6000;
        public int time = 4800;
        public int quarter = 1;
        StreamWriter sw;

        public void InitializeTimer(Action<string> action)
        {
            // measured in tenths of seconds
            timer.Interval = 93.75;
            timer.Elapsed += OnTimerTick;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Stop();
            changeTime = action;

            FindTime();
        }

        private void OnTimerTick(Object source, EventArgs e)
        {
            if (time > 0)
            {
                time -= 1;

                var currTime = FindTime();
                changeTime(currTime);
            }
            else
            {
                timer.Stop();
                WriteTime("0.0");
            }
        }

        public string SetHalftime()
        {
            time = halfTime;
            quarter += 1;
            return FindTime();
        }
        public string ResetClock()
        {
            quarter = 1;
            time = quarterTime;
            return FindTime();
        }

        public string ResetTime()
        {
            time = quarterTime;
            return FindTime();
        }
        public string AddOneSecond()
        {
            time += 10;
            return FindTime();
        }
        public string SubtractOneSecond()
        {
            time = time < 10 ? 0 : time - 10;
            return FindTime();
        }
        public string AddOneMinute()
        {
            time += 600;
            return FindTime();
        }

        public string SubtractOneMinute()
        {
            time = time < 600 ? 0 : time - 600;
            return FindTime();
        }

        public string FindTime()
        {
            int tempTime = time;
            int minutes = time / 600;

            tempTime -= minutes * 600;

            string seconds = (tempTime / 10).ToString("D2");

            string newTime;
            if (time / 600 > 0)
                newTime = $"{minutes}:{seconds}";
            else
                newTime = $"{seconds}.{time % 10}";

            WriteTime(newTime);
            return newTime;
        }

        public void WriteTime(string newTime)
        {
            sw = new StreamWriter("timer.txt");
            sw.Write(newTime);
            sw.Close();
        }
    }
}
