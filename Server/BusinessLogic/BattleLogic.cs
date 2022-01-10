using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.DAL;
using Server.Models;

namespace Server.BusinessLogic
{
    public class BattleLogic
    {
        public User user1 { get; private set; }
        public List<AbstractCard> deck1 { get; private set; }
        public User user2 { get; private set; }
        public List<AbstractCard> deck2 { get; private set; }

        public String BattleLog { get; private set; }

        public String winner { get; private set; }

        public BattleLogic(User u1, User u2, List<AbstractCard> deck1, List<AbstractCard> deck2)
        {
            this.user1 = u1;
            this.user2 = u2;
            this.deck1 = deck1;
            this.deck2 = deck2;
            this.winner = null;
        }

        public string StartBattle()
        {
            Console.WriteLine("Hello from Battle");
            string log = "";
            for (int i = 1; i < 101; i++)
            {
                if (deck1.Count() < 1 && deck2.Count() < 1)
                {
                    log = resolveDraw();
                    BattleLog += log;
                    Console.WriteLine(log);
                    return log;
                }
                else if (deck1.Count() < 1)
                {
                    winner = user2.Username;
                    log = resolveWinner(user2, user1);
                    BattleLog += log;
                    Console.WriteLine(log);
                    return log;
                }
                else if(deck2.Count() < 1)
                {
                    winner = user1.Username;
                    log = resolveWinner(user1, user2);
                    BattleLog += log;
                    Console.WriteLine(log);
                    return log;
                }
                else
                {
                    deck1 = deck1.OrderBy(a => Guid.NewGuid()).ToList();
                    deck2 = deck2.OrderBy(a => Guid.NewGuid()).ToList();

                    log = ResolveRound();
                    BattleLog += log;
                    Console.WriteLine(log);
                }                
            }
            log = resolveDraw();
            BattleLog += log;
            Console.WriteLine(log);
            return log;
        }

        public string ResolveRound()
        {
            AbstractCard card1 = deck1.First();
            AbstractCard card2 = deck2.First();

            //Console.WriteLine(card1.Name + "-" + card2.Name + " " + card1.GetType() + " " + card2.GetType());

            if (card2 is ITrap)
            {
                if (card1 is ITrap)
                {
                    deck1.RemoveAt(0);
                    deck2.RemoveAt(0);
                    return "Two Traps canceld each other";
                }
                else
                {
                    deck1 = ((ITrap)card2).ManipulateDeck(deck1);
                    deck2.RemoveAt(0);
                    return user2.Username + " used Trap Card";                    
                }
            }
            else if (card1 is ITrap)
            {
                if (card2 is ITrap)
                {
                    deck1.RemoveAt(0);
                    deck2.RemoveAt(0);
                    return "Two Traps canceld each other";
                }
                else
                {
                    deck2 = ((ITrap)card1).ManipulateDeck(deck2);
                    deck1.RemoveAt(0);
                    return user1.Username + " used Trap Card";                    
                }

            }
            else if(card1.GetType() == typeof(MonsterCard))
            {
                if(card2.GetType() == typeof(MonsterCard))
                {
                    return resolveMonsterMonster((MonsterCard)card1, (MonsterCard)card2);
                }
                if (card2.GetType() == typeof(SpellCard))
                {
                    return resolveMonsterSpell((MonsterCard)card1, (SpellCard)card2);
                }
            }
            else if(card1.GetType() == typeof(SpellCard))
            {
                if (card2.GetType() == typeof(MonsterCard))
                {
                    return resolveSpellMonster((SpellCard)card1, (MonsterCard)card2);
                }
                if (card2.GetType() == typeof(SpellCard))
                {
                    return resolveElementFight(card1, card2);
                }
            }
            
            

            return "got to end!";
        }

        public string resolveMonsterMonster(MonsterCard c1, MonsterCard c2)
        {
            if (c1.Race == Races.Dragon && c2.Race == Races.Goblin)
            {
                return Player1RoundWin(c1, c2);
            }
            if (c2.Race == Races.Dragon && c1.Race == Races.Goblin)
            {
                return Player2RoundWin(c1, c2);
            }
            if (c1.Race == Races.Wizzard && c2.Race == Races.Ork)
            {
                return Player1RoundWin(c1, c2);
            }
            if (c2.Race == Races.Wizzard && c1.Race == Races.Ork)
            {
                return Player2RoundWin(c1, c2);
            }
            if (c1.Race == Races.FireElve && c2.Race == Races.Dragon)
            {
                return Player1RoundWin(c1, c2);
            }
            if (c2.Race == Races.FireElve && c1.Race == Races.Dragon)
            {
                return Player2RoundWin(c1, c2);
            }
            if(c1.Damage > c2.Damage)
            {
                return Player1RoundWin(c1, c2);
            }
            if (c1.Damage < c2.Damage)
            {
                return Player2RoundWin(c1, c2);
            }
            return "Draw";

        }

        public string resolveSpellMonster(SpellCard c1, MonsterCard c2)
        {
            if (c2.Race == Races.Kraken) return Player2RoundWin(c1, c2);
            if (c2.Race == Races.Knight && c1.Element == Elements.water) return Player1RoundWin(c1, c2);
            return resolveElementFight(c1, c2);
        }

        public string resolveMonsterSpell(MonsterCard c1, SpellCard c2)
        {
            if (c1.Race == Races.Kraken) return Player1RoundWin(c1, c2);
            if (c1.Race == Races.Knight && c2.Element == Elements.water) return Player2RoundWin(c1, c2);
            return resolveElementFight(c1, c2);
        }

        public string resolveElementFight(AbstractCard c1, AbstractCard c2)
        {
            if (c1.Element == Elements.water && c2.Element == Elements.fire)
            {
                if (c1.Damage * 2 > c2.Damage / 2) return Player1RoundWin(c1, c2);
                if (c1.Damage * 2 < c2.Damage / 2) return Player2RoundWin(c1, c2);
                return "Draw";
            }
            if (c2.Element == Elements.water && c1.Element == Elements.fire)
            {
                if (c2.Damage * 2 > c1.Damage / 2) return Player2RoundWin(c1, c2);
                if (c2.Damage * 2 < c1.Damage / 2) return Player1RoundWin(c1, c2);
                return "Draw";
            }
            if (c1.Element == Elements.fire && c2.Element == Elements.normal)
            {
                if (c1.Damage * 2 > c2.Damage / 2) return Player1RoundWin(c1, c2);
                if (c1.Damage * 2 < c2.Damage / 2) return Player2RoundWin(c1, c2);
                return "Draw";
            }
            if (c2.Element == Elements.fire && c1.Element == Elements.normal)
            {
                if (c2.Damage * 2 > c1.Damage / 2) return Player2RoundWin(c1, c2);
                if (c2.Damage * 2 < c1.Damage / 2) return Player1RoundWin(c1, c2);
                return "Draw";
            }
            if (c1.Element == Elements.normal && c2.Element == Elements.water)
            {
                if (c1.Damage * 2 > c2.Damage / 2) return Player1RoundWin(c1, c2);
                if (c1.Damage * 2 < c2.Damage / 2) return Player2RoundWin(c1, c2);
                return "Draw";
            }
            if (c2.Element == Elements.normal && c1.Element == Elements.water)
            {
                if (c2.Damage * 2 > c1.Damage / 2) return Player2RoundWin(c1, c2);
                if (c2.Damage * 2 < c1.Damage / 2) return Player1RoundWin(c1, c2);
                return "Draw";
            }
            if (c1.Damage > c2.Damage) return Player1RoundWin(c1, c2);
            if (c1.Damage < c2.Damage) return Player2RoundWin(c1, c2);
            return "Draw";

        }


        public string Player1RoundWin(AbstractCard c1, AbstractCard c2)
        {
            deck1.Append(deck2.First());
            deck2.RemoveAt(0);
            return user1.Username + "'s " + c1.Name + " killed " + c2.Name + " of " + user2.Username;
        }

        public string Player2RoundWin(AbstractCard c1, AbstractCard c2)
        {
            deck2.Append(deck1.First());
            deck1.RemoveAt(0);
            return user2.Username + "'s " + c2.Name + " killed " + c1.Name + " of " + user1.Username;
        }

        public string resolveDraw()
        {
            decimal pow1 = (Convert.ToDecimal(user1.Elo) - Convert.ToDecimal(user1.Elo)) / 400m;
            decimal pow2 = (Convert.ToDecimal(user2.Elo) - Convert.ToDecimal(user2.Elo)) / 400m;            

            double Ewinner = (1 / (1 + Math.Pow(10, Convert.ToDouble(pow1))));
            double ELooser = (1 / (1 + Math.Pow(10, Convert.ToDouble(pow2))));
            if (Ewinner + ELooser != 1) return "Problem with Elocalc";            

            int newWinnerElo = Convert.ToInt32(user1.Elo + 20 * (1 - Ewinner));
            int newLooserElo = Convert.ToInt32(user2.Elo + 20 * (0 - ELooser));

            //Update DB
            UserAccess.changeUserElo(user1.Id, newWinnerElo);
            UserAccess.changeUserElo(user2.Id, newLooserElo);

            return "After 100 Rounds the Fight ended in a draw";
        }

        public string resolveWinner(User winner, User looser)
        {
            decimal pow1 = (Convert.ToDecimal(looser.Elo) - Convert.ToDecimal(winner.Elo)) /400m;
            decimal pow2 = (Convert.ToDecimal(winner.Elo) - Convert.ToDecimal(looser.Elo)) / 400m;            

            double Ewinner = (1 / (1 + Math.Pow(10, Convert.ToDouble(pow1))));
            double ELooser = (1 / (1 + Math.Pow(10, Convert.ToDouble(pow2))));
            if (Ewinner + ELooser != 1) return "Problem with Elocalc";            

            int newWinnerElo = Convert.ToInt32(winner.Elo + 20 * (1 - Ewinner));
            int newLooserElo = Convert.ToInt32(looser.Elo + 20 * (0 - ELooser));

            //Update DB
            UserAccess.changeUserElo(winner.Id, newWinnerElo);
            UserAccess.changeUserElo(looser.Id, newLooserElo);
            UserAccess.addLoss(looser.Id);
            UserAccess.addWin(winner.Id);

            return winner.Username + " won!";
        }







    }
}
