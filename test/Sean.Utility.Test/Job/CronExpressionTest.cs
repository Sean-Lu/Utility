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


        [TestMethod]
        public void TestCronExpression_Every2Seconds()
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
        public void TestCronExpression_EveryMinute()
        {
            string cron = "0 * * * * ?";// 每分钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:16:30");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:17:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:18:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_Every5Minutes()
        {
            string cron = "0 0/5 * * * ?";// 每5分钟执行1次
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:03:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:05:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:10:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_DailyAt1AM()
        {
            string cron = "0 0 1 * * ?";// 每天凌晨1点执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:16:55");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 01:00:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-03 01:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_WeeklyMonToFri()
        {
            string cron = "0 15 10 ? * MON-FRI";// 周一到周五的10点15分执行
            CronExpression cronExpression = new CronExpression(cron);

            // 测试周五
            DateTime nextExecutionTime = DateTime.Parse("2024-10-04 09:00:00"); // 周五
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 10:15:00"), nextExecutionTime);

            // 测试周六，应该跳到下周周一
            nextExecutionTime = DateTime.Parse("2024-10-05 11:00:00"); // 周六
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-07 10:15:00"), nextExecutionTime); // 周一
        }

        [TestMethod]
        public void TestCronExpression_WeeklyMonToFri_Number()
        {
            string cron = "0 15 10 ? * 2-6";// 周一到周五的10点15分执行
            CronExpression cronExpression = new CronExpression(cron);

            // 测试周五
            DateTime nextExecutionTime = DateTime.Parse("2024-10-04 09:00:00"); // 周五
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 10:15:00"), nextExecutionTime);

            // 测试周六，应该跳到下周周一
            nextExecutionTime = DateTime.Parse("2024-10-05 11:00:00"); // 周六
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-07 10:15:00"), nextExecutionTime); // 周一
        }

        [TestMethod]
        public void TestCronExpression_MonthlyLastDay()
        {
            string cron = "0 15 10 L * ?";// 每月最后一天的10点15分执行
            CronExpression cronExpression = new CronExpression(cron);

            // 测试1月最后一天
            DateTime nextExecutionTime = DateTime.Parse("2024-01-15 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-01-31 10:15:00"), nextExecutionTime);

            // 测试2月最后一天（闰年）
            nextExecutionTime = DateTime.Parse("2024-02-15 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 10:15:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_ThirdFridayOfMonth()
        {
            string cron = "0 15 10 ? * 6#3";// 每月第三个星期五的10点15分执行
            CronExpression cronExpression = new CronExpression(cron);

            // 2024年10月的第三个星期五是10月18日
            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-18 10:15:00"), nextExecutionTime);

            // 下一个应该是11月的第三个星期五（11月15日）
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-11-15 10:15:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_SpecificMonths()
        {
            string cron = "0 0 12 1 1,6,12 ?";// 1月、6月、12月的第1天中午12点执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-02-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-06-01 12:00:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-12-01 12:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_ComplexRange()
        {
            string cron = "0 0-10,20-30/2 9-17 * * ?";// 每天9点到17点，分钟在0-10或20-30之间且为偶数的整点执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 16:25:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 16:26:00"), nextExecutionTime); // 分钟26在20-30范围内且是偶数

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 16:28:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_WithYearField()
        {
            string cron = "0 0 0 1 1 ? 2025-2030";// 2025-2030年每年1月1日执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2025-01-01 00:00:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2026-01-01 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_EnglishMonthNames()
        {
            string cron = "0 0 0 1 JAN,JUN,DEC ?";// 1月、6月、12月的第1天执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-02-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-06-01 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_EnglishDayNames()
        {
            string cron = "0 0 12 ? * MON,WED,FRI";// 周一、周三、周五的中午12点执行
            CronExpression cronExpression = new CronExpression(cron);

            // 2024-10-01 是星期二
            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 12:00:00"), nextExecutionTime); // 周三

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 12:00:00"), nextExecutionTime); // 周五
        }

        [TestMethod]
        public void TestCronExpression_StepWithRange()
        {
            string cron = "0 0/15 9-17 * * ?";// 每天9点到17点，每15分钟执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:05:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 09:15:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 09:30:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_EndOfMonthBehavior()
        {
            string cron = "0 0 0 28-31 * ?";// 每月28-31日执行
            CronExpression cronExpression = new CronExpression(cron);

            // 测试2月（只有28或29天）
            DateTime nextExecutionTime = DateTime.Parse("2024-02-27 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-28 00:00:00"), nextExecutionTime);

            // 测试4月（只有30天）
            nextExecutionTime = DateTime.Parse("2024-04-29 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-04-30 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_QuestionMark()
        {
            string cron = "0 0 12 ? * FRI";// 每周五中午12点执行，日字段不指定
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00"); // 周二
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 12:00:00"), nextExecutionTime); // 周五
        }

        [TestMethod]
        public void TestCronExpression_MultipleValues()
        {
            string cron = "0 0,15,30,45 * * * ?";// 每小时的0,15,30,45分执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 11:05:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:15:00"), nextExecutionTime);

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 11:30:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_InvalidFormat()
        {
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                string cron = "invalid cron expression";
                CronExpression cronExpression = new CronExpression(cron);
            });
        }

        [TestMethod]
        public void TestCronExpression_NewYearTransition()
        {
            string cron = "0 0 0 1 1 ?";// 每年1月1日执行
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-12-31 23:59:59");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2025-01-01 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_LeapYear()
        {
            string cron = "0 0 0 29 2 ?";// 每年2月29日执行
            CronExpression cronExpression = new CronExpression(cron);

            // 2023不是闰年
            DateTime nextExecutionTime = DateTime.Parse("2023-02-28 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 00:00:00"), nextExecutionTime); // 跳到2024年（闰年）

            // 2024是闰年
            nextExecutionTime = DateTime.Parse("2024-02-28 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 00:00:00"), nextExecutionTime);
        }
    }
}