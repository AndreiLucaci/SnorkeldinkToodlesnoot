using System;
using System.Collections.Generic;
using LightRidersBot.Bot;
using NUnit.Framework;
using SnorkeldinkToodlesnoot.Bot;
using SnorkeldinkToodlesnoot.Field;
using SnorkeldinkToodlesnoot.Player;

namespace tests
{
    [TestFixture]
    public class BotParserTests
    {
        [Test]
        public void TestMethod1()
        {
            Field field = new Field
            {
                Height = 16,
                Width = 16
            };
            field.InitField();
            field.MyId = "0";

            var line = ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,x,x,x,x,.,.,.,.,x,x,x,x,x,.,.,.,x,x,.,.,.,.,.,x,x,.,.,.,.,.,.,.,x,x,.,.,.,.,x,x,.,.,.,.,.,.,.,.,x,x,.,.,.,x,x,.,.,.,.,.,.,.,.,.,x,x,.,.,x,x,.,.,.,.,.,.,.,.,.,.,x,x,.,x,x,.,.,.,.,.,.,.,.,.,.,.,x,x,x,x,.,.,.,.,.,.,.,.,.,.,.,.,x,x,x,.,.,.,.,.,.,.,.,.,.,.,.,.,0,x,x,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,1,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.";

            field.ParseFromString(line);
            var myPos = field.MyPosition;
            var enemyPos = field.EnemyPosition;

            var bot = new AwesomeBot();

            var move = bot.DoMove(new BotState
            {
                Field = field,
                MyName = "0",
                Players = new Dictionary<string, Player> { ["player0"] = new Player("player0"), ["player1"] = new Player("player1") },
                MaxTimebank = 10000,
                MaxRounds = 1,
                TimePerMove = 200,
                RoundNumber = 0,
                Timebank = 10000
            });

            Assert.AreNotEqual(myPos, enemyPos);
        }
    }
}
