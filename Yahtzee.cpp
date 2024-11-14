#include <iostream>
#include <string>
#include <stdlib.h>
#include <vector>
#include <iomanip>
using namespace std;

/*class Player{
public:


};
class Game{

};
class Dice{

};*/

class ScoreCard {
public:
    /*struct ScoreType{
        string name;
        int score;
        bool used;
        void setNum(int scoreNum){
            cout << "Setting this cuh" << endl;
             this->score = scoreNum;}
        void setBool(bool x){this->used = x;}
    };
    ScoreType scoresheet[13]{ {"Aces", 0, false}, { "Twos", 0, false },{"Threes",0,false},
        {"Fours",0,false},{"Fives",0,false},{"Sixes",0,false},{"3 of a Kind", 0, false},
        {"4 of a Kind", 0, false}, {"Full House", 0, false}, {"Sm. Straight", 0, false},
        {"Lg. Straight", 0, false}, {"Yahtzee", 0, false}, {"Chance", 0, false} };
    */
    string options[13]{ "Aces", "Twos", "Threes", "Fours", "Fives", "Sixes", "Three of a Kind", "Four of a Kind",
            "Full House", "Small Straight", "Large Straight","Yahtzee!", "Chance" };

    void Verify(int d1, int d2, int d3, int d4, int d5)
    {
        if (aceUsed && twoUsed && threeUsed && fourUsed && fiveUsed && sixUsed && threeKindUsed && fourKindUsed && fullHouseUsed
            && smStraightUsed && lgStraightUsed && yahtzeeUsed && chanceUsed)
        {
            cout << "You're done here, wait for everyone else to finish!" << endl;
            return;
        }

        int temp;
        int dice[5]{ d1, d2, d3, d4, d5 };
        int ones = 0, two = 0, three = 0, four = 0, five = 0, six = 0;
        vector<int> validNums;
        vector<int> invalid;
        vector<int> scores;
        cout << "Select an option:" << endl;

        for (int i = 0; i < 5; ++i)
        {
            if (dice[i] == 1)
            {
                ones++;
            }
            if (dice[i] == 2)
            {
                two++;
            }
            if (dice[i] == 3)
            {
                three++;
            }
            if (dice[i] == 4)
            {
                four++;
            }
            if (dice[i] == 5)
            {
                five++;
            }
            if (dice[i] == 6)
            {
                six++;
            }
        }

        if (ones > 0 && !aceUsed) {
            scores.push_back(ones);
            validNums.push_back(1);
        }
        else if (!aceUsed) { invalid.push_back(1); }
        if (two > 0 && !twoUsed) {
            validNums.push_back(2);
            scores.push_back(two * 2);
        }
        else if (!twoUsed) { invalid.push_back(2); }
        if (three > 0 && !threeUsed) {
            scores.push_back(three * 3);
            validNums.push_back(3);
        }
        else if (!threeUsed) { invalid.push_back(3); }
        if (four > 0 && !fourUsed) {
            scores.push_back(four * 4);
            validNums.push_back(4);
        }
        else if (!fourUsed) { invalid.push_back(4); }
        if (five > 0 && !fiveUsed) {
            scores.push_back(five * 5);
            validNums.push_back(5);
        }
        else if (!fiveUsed) { invalid.push_back(5); }
        if (six > 0 && !sixUsed) {
            scores.push_back(six * 6);
            validNums.push_back(6);
        }
        else if (!sixUsed) { invalid.push_back(6); }
        if ((ones >= 3 || two >= 3 || three >= 3 || four >= 3 || five >= 3 || six >= 3)
            && !threeKindUsed) {
            scores.push_back(d1 + d2 + d3 + d4 + d5);
            validNums.push_back(7);
        }
        else if (!threeKindUsed) { invalid.push_back(7); }
        if ((ones >= 4 || two >= 4 || three >= 4 || four >= 4 || five >= 4 || six >= 4)
            && !fourKindUsed) {
            scores.push_back(d1 + d2 + d3 + d4 + d5);
            validNums.push_back(8);
        }
        else if (!fourKindUsed) { invalid.push_back(8); }
        if ((ones == 3 || two == 3 || three == 3 || four == 3 || five == 3 || six == 3)
            && (ones == 2 || two == 2 || three == 2 || four == 2 || five == 2 || six == 2)
            && !fullHouseUsed) {
            validNums.push_back(9);
            scores.push_back(25);
        }
        else if (!fullHouseUsed) { invalid.push_back(9); }
        if (((ones >= 1 && two >= 1 && three >= 1 && four >= 1)
            || (two >= 1 && three >= 1 && four >= 1 && five >= 1)
            || (three >= 1 && four >= 1 && five >= 1 && six >= 1))
            && !smStraightUsed) {
            validNums.push_back(10);
            scores.push_back(30);
        }
        else if (!smStraightUsed) { invalid.push_back(10); }
        if (((ones >= 1 && two >= 1 && three >= 1 && four >= 1 && five >= 1)
            || (two >= 1 && three >= 1 && four >= 1 && five >= 1 && six >= 1))
            && !lgStraightUsed) {
            validNums.push_back(11);
            scores.push_back(40);
        }
        else if (!lgStraightUsed) { invalid.push_back(11); }
        if ((ones == 5 || two == 5 || three == 5 || four == 5 || five == 5 || six == 5) && !yahtzeeUsed) {
            scores.push_back(50);
            validNums.push_back(12);
        }
        else if ((ones == 5 || two == 5 || three == 5 || four == 5 || five == 5 || six == 5) && yahtzeeUsed && yahtzee == 50) {
            scores.push_back(100);
            validNums.push_back(12);
        }
        else if (!yahtzeeUsed) { invalid.push_back(12); }
        if (!chanceUsed) {
            scores.push_back(d1 + d2 + d3 + d4 + d5);
            validNums.push_back(13);
        }
        else if (!chanceUsed) { invalid.push_back(13); }
        bool cuh = false;

        int id = 0;
        if (!validNums.empty()) {

            for (int i = 0; i < validNums.size(); ++i)
            {

                cout << validNums.at(i) << ": " << options[validNums.at(i) - 1] << ": " << scores.at(i) << " points" << endl;
            }

            do {
                cout << "Enter a valid choice:" << endl;
                cin >> temp;

                for (int i = 0; i < validNums.size(); ++i) {
                    if (temp == validNums.at(i))
                    {
                        id = i;
                        cuh = true;
                    }
                }

            } while (!cuh);

            if (yahtzeeUsed && temp == 12)
            {
                bonus += 100;
            }
            else {
                SetNum(scores.at(id), temp);
            }
        }
        else
        {
            for (int i = 0; i < invalid.size(); ++i)
            {
                cout << invalid.at(i) << ": " << options[invalid.at(i) - 1] << "->" << 0 << " points" << endl;
            }

            do {
                cout << "Pick a value to set to 0 for the remainder of the game:" << endl;
                cin >> temp;

                for (int i = 0; i < invalid.size(); ++i) {
                    if (temp == invalid.at(i))
                    {
                        id = i;
                        cuh = true;
                    }
                }
            } while (!cuh);
            SetNum(0, temp);
        }
    }


    void SetNum(int newVal, int num)
    {
        if (num == 1) { aces = newVal; aceUsed = true; }
        else if (num == 2) { two = newVal; twoUsed = true; }
        else if (num == 3) { three = newVal; threeUsed = true; }
        else if (num == 4) { four = newVal; fourUsed = true; }
        else if (num == 5) { five = newVal; fiveUsed = true; }
        else if (num == 6) { six = newVal; sixUsed = true; }
        else if (num == 7) { threeKind = newVal; threeKindUsed = true; }
        else if (num == 8) { fourKind = newVal; fourKindUsed = true; }
        else if (num == 9) { fullHouse = newVal; fullHouseUsed = true; }
        else if (num == 10) { smStraight = newVal; smStraightUsed = true; }
        else if (num == 11) { lgStraight = newVal; lgStraightUsed = true; }
        else if (num == 12) { yahtzee = newVal; yahtzeeUsed = true; }
        else if (num == 13) { chance = newVal; chanceUsed = true; }
    }

    void Print() {

        cout << setw(21) << setfill('-') << " " << endl;
        cout << "Scorecard:" << endl;
        cout << setw(21) << setfill('-') << " " << endl;

        int total = 0;
        int arr[13]{ aces, two, three, four, five, six, threeKind, fourKind, fullHouse, smStraight, lgStraight, yahtzee, chance };
        for (int i = 0; i < 13; ++i)
        {
            cout << setw(15) << setfill(' ') << left << options[i] << "|"
                << setw(3) << right << arr[i] << "|" << endl;
            total += arr[i];
        }
        cout << setw(15) << setfill(' ') << left << "Bonus: " << "|"
            << setw(3) << right << bonus << "|" << endl;
        total += bonus;

        cout << setw(15) << setfill(' ') << left << "Total: " << "|"
            << setw(3) << right << total << "|" << endl;

        cout << setw(21) << setfill('-') << " " << endl;
    }
protected:
    int aces = 0, two = 0, three = 0, four = 0, five = 0, six = 0, threeKind = 0, fourKind = 0, fullHouse = 0, smStraight = 0, lgStraight = 0, yahtzee = 0, chance = 0;
    bool aceUsed = false, twoUsed = false, threeUsed = false, fourUsed = false, fiveUsed = false, sixUsed = false, threeKindUsed = false,
        fourKindUsed = false, fullHouseUsed = false, smStraightUsed = false, lgStraightUsed = false, yahtzeeUsed = false, chanceUsed = false;
    int bonus = 0;

};


/*int main() {

    ScoreCard scrumble;
    int d1 = 0, d2, d3, d4, d5;
    scrumble.Print();
    while (d1 != -1) {
        cin >> d1; cin >> d2; cin >> d3; cin >> d4; cin >> d5;
        scrumble.Verify(d1, d2, d3, d4, d5);
        scrumble.Print();
    }

    return 0;
}*/
