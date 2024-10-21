using Sean.Utility.Job;

namespace Sean.Utility.Test.Job
{
    [TestClass]
    public class CronExpressionTest
    {
        [TestMethod]
        public void TestCronExpression1()
        {
            string cron = "0/2 * * * * ?";// 每2秒钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:16:55");

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:16:56"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:16:58"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:17:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:17:02"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:17:04"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression2()
        {
            string cron = "0 0/2 * * * ?";// 每2分钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:53:55");

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:54:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:56:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:58:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 12:00:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 12:02:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression3()
        {
            string cron = "30 0/2 * * * ?";// 每2分钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:53:55");

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:54:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:56:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:58:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 12:00:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 12:02:30"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression4()
        {
            string cron = "0 0 0/2 * * ?";// 每2小时执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 17:53:55");

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 18:00:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 20:00:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 22:00:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 00:00:00"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 02:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression5()
        {
            string cron = "30 15 0/2 * * ?";// 每2小时执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 17:53:55");

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 18:15:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 20:15:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 22:15:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 00:15:30"), nextExecutionTime);
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 02:15:30"), nextExecutionTime);
        }
    }
}