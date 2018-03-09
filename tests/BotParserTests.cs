using System;
using NUnit.Framework;
using SnorkeldinkToodlesnoot.Field;

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

            var line =                ".,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,x,x,x,x,x,x,0,1,x,x,x,x,x,x,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.,.";

            field.ParseFromString(line);

            var myPos = field.MyPosition;
            var enemyPos = field.EnemyPosition;

            Assert.AreNotEqual(myPos, enemyPos);
        }
    }
}
