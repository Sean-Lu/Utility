using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using Sean.Utility.Contracts;
using Sean.Utility.Net.Mail;

namespace Demo.Framework.Impls.Test
{
    internal class SmtpTest : ISimpleDo
    {
        public void Execute()
        {

        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromMailAddress">发送者 Email 地址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文（支持HTML格式）</param>
        /// <param name="to">收件人列表，多人用逗号隔开</param>
        /// <param name="cc">抄送人列表，多人用逗号隔开</param>
        /// <param name="bcc">密送人列表，多人用逗号隔开</param>
        /// <param name="attachments">附件路径，如果为null，或者文件不存在，则不带附件</param>
        public void SendMail(string fromMailAddress, string subject, string body, string to, string cc = null, string bcc = null, List<string> attachments = null)
        {
            using (var mm = new MailMessage())
            {
                mm.Priority = MailPriority.Normal;
                mm.From = new MailAddress(fromMailAddress);
                //mm.From = new MailAddress(fromEmailAddress, "管理员", Encoding.UTF8);

                // 收件人
                if (!string.IsNullOrWhiteSpace(to))
                {
                    mm.To.Add(to);
                }
                // 抄送人
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    mm.CC.Add(cc);
                }
                // 密送人
                if (!string.IsNullOrWhiteSpace(bcc))
                {
                    mm.Bcc.Add(bcc);
                }

                mm.Subject = subject;
                mm.SubjectEncoding = Encoding.UTF8;
                mm.Body = body;
                mm.BodyEncoding = Encoding.UTF8;
                mm.IsBodyHtml = true;

                // 邮件附件 
                if (attachments != null)
                {
                    foreach (string fileName in attachments)
                    {
                        if (!File.Exists(fileName)) continue;

                        mm.Attachments.Add(new Attachment(fileName));
                    }
                }

                // 发送邮件
                using (var smtpClientProvider = new SmtpClientProvider("127.0.0.1"))
                {
                    //smtpClientProvider.Client.Credentials = new NetworkCredential("userName", "password");
                    smtpClientProvider.Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClientProvider.Client.EnableSsl = true;

                    smtpClientProvider.SendMail(mm);
                }
            }
        }
    }
}
