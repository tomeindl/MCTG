using System.Collections.Generic;
using NUnit.Framework;
using Server;
using Server.Models;
using Server.BusinessLogic;
using Server.Utility;

namespace MonsterCardTradingGame_UnitTests
{
    public class Tests
    {

        User user1 = new User();
        User user2 = new User();

        List<AbstractCard> deck1 = new List<AbstractCard>();
        List<AbstractCard> deck2 = new List<AbstractCard>();
        List<AbstractCard> deck3 = new List<AbstractCard>();

        SpellCard sc1 = new SpellCard(1, "Fire Spell", Elements.fire, 20, 10, Rarity.common);
        SpellCard sc2 = new SpellCard(1, "Water Spell", Elements.water, 20, 10, Rarity.common);
        SpellCard sc3 = new SpellCard(1, "Normal Spell", Elements.normal, 20, 10, Rarity.common);

        MonsterCard knight = new MonsterCard(1, "Knight", Elements.normal, 20, 10, Rarity.common, Races.Knight);
        MonsterCard kraken = new MonsterCard(1, "Kraken", Elements.normal, 2, 10, Rarity.common, Races.Kraken);

        MonsterCard knight2 = new MonsterCard(1, "Knight", Elements.normal, 20, 10, Rarity.common, Races.Knight);

        MonsterCard Dragon = new MonsterCard(1, "Dragon", Elements.normal, 5, 10, Rarity.common, Races.Dragon);
        MonsterCard Goblin = new MonsterCard(1, "Goblin", Elements.normal, 50, 10, Rarity.common, Races.Goblin);
        MonsterCard FireElve = new MonsterCard(1, "Elve", Elements.fire, 50, 1, Rarity.common, Races.FireElve);

        ThiefTrap trap1 = new ThiefTrap(1, "Thief", Elements.normal, 50, 10, Rarity.common);
        InstigatorTrap trap2 = new InstigatorTrap(1, "Inst", Elements.normal, 50, 10, Rarity.common);

        [SetUp]
        public void Setup()
        {
            user1.Username = "User1";
            user2.Username = "User2";

            deck1 = new List<AbstractCard>();
            deck1.Add(new SpellCard(1, "Fire Spell", Elements.fire, 20, 10, Rarity.common));
            deck1.Add(new SpellCard(2, "Water Spell", Elements.water, 20, 10, Rarity.common));
            deck1.Add(new MonsterCard(3, "Goblin", Elements.normal, 15, 10, Rarity.common, Races.Goblin));
            deck1.Add(new MonsterCard(3, "Kraken", Elements.normal, 30, 10, Rarity.common, Races.Kraken));

            deck2 = new List<AbstractCard>();
            deck2.Add(new SpellCard(1, "Fire Spell", Elements.fire, 20, 10, Rarity.common));
            deck2.Add(new SpellCard(2, "Normal Spell", Elements.normal, 15, 10, Rarity.common));
            deck2.Add(new MonsterCard(3, "Water Knight", Elements.water, 15, 10, Rarity.common, Races.Knight));
            deck2.Add(new MonsterCard(3, "Ork", Elements.normal, 30, 10, Rarity.common, Races.Ork));

        }

        [Test]
        public void TestBattle_DeckNoCards()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck3);

            //Act
            bl.StartBattle();

            //Assert
            Assert.AreEqual(bl.winner, "User1");
        }

        [Test]
        public void TestBattle_WaterSpellVSFireSpell()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck3);

            //Act
            var result = bl.resolveElementFight(sc1, sc2);
            var expectedresult = bl.Player2RoundWin(sc1, sc2);
            //var expectedresult = user2.Username + "'s " + sc2.Name + " killed " + sc1.Name + " of " + user1.Username;

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_WaterSpellVSKnight()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveSpellMonster(sc2, knight);
            var expectedresult = bl.Player1RoundWin(sc2, knight);
            
            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_WaterSpellVSNormal()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveElementFight(sc2, sc3);
            var expectedresult = bl.Player2RoundWin(sc2, sc3);

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_DragonVSGoblin()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveMonsterMonster(Dragon, Goblin);
            var expectedresult = bl.Player1RoundWin(Dragon, Goblin);

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_DragonVSFireElves()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveMonsterMonster(Dragon, FireElve);
            var expectedresult = bl.Player2RoundWin(Dragon, FireElve);

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_KrakenVSSpell()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveMonsterSpell(kraken, sc1);
            var expectedresult = bl.Player1RoundWin(kraken, sc1);
            
            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_TrapVSTrap()
        {
            deck1 = new List<AbstractCard>();
            deck2 = new List<AbstractCard>();

            deck1.Add(trap1);
            deck2.Add(trap2);

            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act 
            string result = bl.StartBattle();

            //Assert
            Assert.AreEqual(result, "After 100 Rounds the Fight ended in a draw");
        }

        [Test]
        public void TestBattle_CardsEqualShouldDraw()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveMonsterMonster(kraken, kraken);
            var expectedresult = "Draw";

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_Draw1()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act
            var result = bl.resolveMonsterMonster(knight, knight2);
            var expectedresult = "Draw";

            //Assert
            Assert.AreEqual(result, expectedresult);
        }

        [Test]
        public void TestBattle_Player1Win()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act            
            bl.Player1RoundWin(kraken, sc1);
            
            //Assert
            Assert.Greater(bl.deck1.Count, bl.deck2.Count);
        }

        [Test]
        public void TestBattle_Player2Win()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);
                     
            //Act            
            bl.Player2RoundWin(kraken, sc1);
            bl.Player2RoundWin(kraken, sc1);

            //Assert
            Assert.Greater(bl.deck2.Count, bl.deck1.Count);
        }

        [Test]
        public void TestBattle_Player2LoseGameOver()
        {
            //Arrange
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act            
            bl.Player1RoundWin(kraken, sc1);
            bl.Player1RoundWin(kraken, sc1);
            bl.Player1RoundWin(kraken, sc1);
            bl.Player1RoundWin(kraken, sc1);
            bl.StartBattle();

            //Assert
            Assert.AreEqual(bl.winner, "User1");
        }

        [Test]
        public void TestBattle_SimulateDraw()
        {
            //Arrange
            deck1 = new List<AbstractCard>();
            deck2 = new List<AbstractCard>();
            BattleLogic bl = new BattleLogic(user1, user2, deck1, deck2);

            //Act 
            string result = bl.StartBattle();

            //Assert
            Assert.AreEqual(result, "After 100 Rounds the Fight ended in a draw");
        }

        [Test]
        public void TestBattle_TestThief()
        {
            //Arrange
            int size = deck2.Count;

            //Act 
            deck2 = trap1.ManipulateDeck(deck2);

            //Assert
            Assert.AreEqual(size -1, deck2.Count);
        }

        [Test]
        public void TestBattle_TestInstigator()
        {
            //Arrange
            int size = deck2.Count;

            //Act 
            deck2 = trap2.ManipulateDeck(deck2);

            //Assert
            Assert.AreEqual(size -1, deck2.Count);
        }

        [Test]
        public void TestBattle_ThiefThrowsExeptionOnEmptyDeck()
        {
            //Arrange            
            deck2 = new List<AbstractCard>();
            

            //Assert
            Assert.Throws<InvalidInputException>(() => trap1.ManipulateDeck(deck2));
        }

        [Test]
        public void TestBattle_InsigatorThrowsExeptionOnEmptyDeck()
        {
            //Arrange            
            deck2 = new List<AbstractCard>();


            //Assert
            Assert.Throws<InvalidInputException>(() => trap2.ManipulateDeck(deck2));
        }

        //----BusinessLogic Tests
        [Test]
        public void TestBusinessLogic_PackFactoryThrwosError()
        {
            //Assert
            Assert.Throws<InvalidInputException>(() => PackageFactory.BuyPackage("invalid", user1.Id));
        }

        [Test]
        public void TestBusinessLogic_BattleLobbyWaitingRoom()
        {
           
            //Arrange   
            BattleLobby.Instance.RegisterForBattle(user1);

            //Assert
            Assert.AreEqual(BattleLobby.Instance.getWaitingroom().Count, 1);
            
        }

        [Test]
        public void TestBusinessLogic_DeckFull()
        {
            //Arrange   
            Deck d = new Deck(1, "Deck1");
            d.addCard(knight);
            d.addCard(sc3);
            d.addCard(sc2);
            d.addCard(knight2);

            //Assert
            Assert.False(d.addCard(sc1));
            

        }     
    }
}