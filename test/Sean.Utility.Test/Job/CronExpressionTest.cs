using Sean.Utility.Job;

namespace Sean.Utility.Test.Job
{
    [TestClass]
    public class CronExpressionTest
    {
        [TestMethod]
        public void TestCronExpression1()
        {
            string cron = "0/2 * * * * ?";// ÿ2����ִ��1��
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
            string cron = "0 0/2 * * * ?";// ÿ2����ִ��1��
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
            string cron = "30 0/2 * * * ?";// ÿ2����ִ��1��
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
            string cron = "0 0 0/2 * * ?";// ÿ2Сʱִ��1��
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
            string cron = "30 15 0/2 * * ?";// ÿ2Сʱִ��1��
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
            string cron = "0/2 * * * * ?";// ÿ2����ִ��1��
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
            string cron = "0 * * * * ?";// ÿ����ִ��1��
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
            string cron = "0 0/5 * * * ?";// ÿ5����ִ��1��
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
            string cron = "0 0 1 * * ?";// ÿ���賿1��ִ��
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
            string cron = "0 15 10 ? * MON-FRI";// ��һ�������10��15��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // ��������
            DateTime nextExecutionTime = DateTime.Parse("2024-10-04 09:00:00"); // ����
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 10:15:00"), nextExecutionTime);

            // ����������Ӧ������������һ
            nextExecutionTime = DateTime.Parse("2024-10-05 11:00:00"); // ����
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-07 10:15:00"), nextExecutionTime); // ��һ
        }

        [TestMethod]
        public void TestCronExpression_WeeklyMonToFri_Number()
        {
            string cron = "0 15 10 ? * 2-6";// ��һ�������10��15��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // ��������
            DateTime nextExecutionTime = DateTime.Parse("2024-10-04 09:00:00"); // ����
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 10:15:00"), nextExecutionTime);

            // ����������Ӧ������������һ
            nextExecutionTime = DateTime.Parse("2024-10-05 11:00:00"); // ����
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-07 10:15:00"), nextExecutionTime); // ��һ
        }

        [TestMethod]
        public void TestCronExpression_MonthlyLastDay()
        {
            string cron = "0 15 10 L * ?";// ÿ�����һ���10��15��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // ����1�����һ��
            DateTime nextExecutionTime = DateTime.Parse("2024-01-15 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-01-31 10:15:00"), nextExecutionTime);

            // ����2�����һ�죨���꣩
            nextExecutionTime = DateTime.Parse("2024-02-15 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 10:15:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_ThirdFridayOfMonth()
        {
            string cron = "0 15 10 ? * 6#3";// ÿ�µ������������10��15��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // 2024��10�µĵ�������������10��18��
            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-18 10:15:00"), nextExecutionTime);

            // ��һ��Ӧ����11�µĵ����������壨11��15�գ�
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-11-15 10:15:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_SpecificMonths()
        {
            string cron = "0 0 12 1 1,6,12 ?";// 1�¡�6�¡�12�µĵ�1������12��ִ��
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
            string cron = "0 0-10,20-30/2 9-17 * * ?";// ÿ��9�㵽17�㣬������0-10��20-30֮����Ϊż��������ִ��
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 16:25:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 16:26:00"), nextExecutionTime); // ����26��20-30��Χ������ż��

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-01 16:28:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_WithYearField()
        {
            string cron = "0 0 0 1 1 ? 2025-2030";// 2025-2030��ÿ��1��1��ִ��
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
            string cron = "0 0 0 1 JAN,JUN,DEC ?";// 1�¡�6�¡�12�µĵ�1��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-02-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-06-01 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_EnglishDayNames()
        {
            string cron = "0 0 12 ? * MON,WED,FRI";// ��һ�����������������12��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // 2024-10-01 �����ڶ�
            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-02 12:00:00"), nextExecutionTime); // ����

            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 12:00:00"), nextExecutionTime); // ����
        }

        [TestMethod]
        public void TestCronExpression_StepWithRange()
        {
            string cron = "0 0/15 9-17 * * ?";// ÿ��9�㵽17�㣬ÿ15����ִ��
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
            string cron = "0 0 0 28-31 * ?";// ÿ��28-31��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // ����2�£�ֻ��28��29�죩
            DateTime nextExecutionTime = DateTime.Parse("2024-02-27 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-28 00:00:00"), nextExecutionTime);

            // ����4�£�ֻ��30�죩
            nextExecutionTime = DateTime.Parse("2024-04-29 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-04-30 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_QuestionMark()
        {
            string cron = "0 0 12 ? * FRI";// ÿ��������12��ִ�У����ֶβ�ָ��
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-10-01 09:00:00"); // �ܶ�
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-10-04 12:00:00"), nextExecutionTime); // ����
        }

        [TestMethod]
        public void TestCronExpression_MultipleValues()
        {
            string cron = "0 0,15,30,45 * * * ?";// ÿСʱ��0,15,30,45��ִ��
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
            string cron = "0 0 0 1 1 ?";// ÿ��1��1��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            DateTime nextExecutionTime = DateTime.Parse("2024-12-31 23:59:59");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2025-01-01 00:00:00"), nextExecutionTime);
        }

        [TestMethod]
        public void TestCronExpression_LeapYear()
        {
            string cron = "0 0 0 29 2 ?";// ÿ��2��29��ִ��
            CronExpression cronExpression = new CronExpression(cron);

            // 2023��������
            DateTime nextExecutionTime = DateTime.Parse("2023-02-28 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 00:00:00"), nextExecutionTime); // ����2024�꣨���꣩

            // 2024������
            nextExecutionTime = DateTime.Parse("2024-02-28 09:00:00");
            nextExecutionTime = cronExpression.GetNextExecutionTime(nextExecutionTime);
            Assert.AreEqual(DateTime.Parse("2024-02-29 00:00:00"), nextExecutionTime);
        }
    }
}