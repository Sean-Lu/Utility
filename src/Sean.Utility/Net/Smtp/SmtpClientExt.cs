using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Sean.Utility.Net.Smtp
{
    /// <summary>  
    /// 邮件发送（基于SMTP协议）
    /// </summary>  
    public class SmtpClientExt
    {
        #region [ 字段、属性 ]
        /// <summary>  
        /// smtp 服务器   
        /// </summary>  
        public string SmtpHost { get; set; }
        /// <summary>  
        /// smtp 服务器端口（默认为25）
        /// </summary>  
        public int SmtpPort { get; set; }
        /// <summary>  
        /// 发送者 Email 地址  
        /// </summary>  
        public string FromEmailAddress { get; set; }
        /// <summary>  
        /// 发送者 Email 密码  
        /// </summary>  
        public string FromEmailPassword { get; set; }
        /// <summary>
        /// 是否使用安全套接字层 (SSL) 加密连接
        /// </summary>
        public bool EnableSsl { get; set; }
        /// <summary>
        /// 是否需要验证发件人的身份凭据
        /// </summary>
        public bool Credential { get; set; }

        /// <summary>
        /// 邮件正文是否是HTML格式，默认为true
        /// </summary>
        public bool IsBodyHtml = true;

        /// <summary>
        /// 邮件主题字符编码，默认为UTF8
        /// </summary>
        public Encoding SubjectEncoding = Encoding.UTF8;
        /// <summary>
        /// 邮件正文字符编码，默认为UTF8
        /// </summary>
        public Encoding BodyEncoding = Encoding.UTF8;

        private readonly SmtpClient _smtp;
        #endregion

        #region [ 构造函数 ]
        /// <summary>
        /// 创建实例：端口默认为25，不使用SSL加密连接，不验证发件人的身份凭据
        /// </summary>
        /// <param name="smtpHost">smtp 服务器</param>
        /// <param name="fromEmailAddress">发送者 Email 地址</param>
        public SmtpClientExt(string smtpHost, string fromEmailAddress)
            : this(smtpHost, 25, fromEmailAddress, string.Empty, false, false)
        { }
        /// <summary>
        /// 创建实例：端口默认为25，不使用SSL加密连接，验证发件人的身份凭据
        /// </summary>
        /// <param name="smtpHost">smtp 服务器</param>
        /// <param name="fromEmailAddress">发送者 Email 地址</param>
        /// <param name="fromEmailPassword">发送者 Email 密码</param>
        public SmtpClientExt(string smtpHost, string fromEmailAddress, string fromEmailPassword)
            : this(smtpHost, 25, fromEmailAddress, fromEmailPassword, false, true)
        { }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="smtpHost">smtp 服务器</param>
        /// <param name="smtpPort">smtp 服务器端口(默认为25)</param>
        /// <param name="fromEmailAddress">发送者 Email 地址</param>
        /// <param name="fromEmailPassword">发送者 Email 密码</param>
        /// <param name="enableSsl">是否使用安全套接字层 (SSL) 加密连接</param>
        /// <param name="credential">是否需要验证发件人的身份凭据</param>
        public SmtpClientExt(string smtpHost, int smtpPort, string fromEmailAddress, string fromEmailPassword, bool enableSsl, bool credential)
        {
            this.SmtpHost = smtpHost;
            this.SmtpPort = smtpPort;
            this.FromEmailAddress = fromEmailAddress;
            this.FromEmailPassword = fromEmailPassword;
            this.EnableSsl = enableSsl;
            this.Credential = credential;

            _smtp = new System.Net.Mail.SmtpClient
            {
                Host = SmtpHost,
                Port = SmtpPort > 0 ? SmtpPort : 25,
                Credentials = Credential ? new NetworkCredential(FromEmailAddress, FromEmailPassword) : null,
                EnableSsl = EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }
        #endregion

        #region [ 发送邮件 ]
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件正文（支持HTML格式）</param>
        /// <param name="to">收件人列表，多人用逗号隔开</param>
        /// <param name="cc">抄送人列表，多人用逗号隔开</param>
        /// <param name="bcc">密送人列表，多人用逗号隔开</param>
        /// <param name="attachments">附件路径，如果为null，或者文件不存在，则不带附件</param>
        public void SendMail(string subject, string body, string to, string cc = null, string bcc = null, List<string> attachments = null)
        {
            var mm = new MailMessage();
            mm.Priority = MailPriority.Normal;
            mm.From = new MailAddress(FromEmailAddress);
            //mm.From = new MailAddress(fromEmailAddress, "管理员", Encoding.UTF8);

            //收件人
            if (!string.IsNullOrWhiteSpace(to))
                mm.To.Add(to);
            //抄送人
            if (!string.IsNullOrWhiteSpace(cc))
                mm.CC.Add(cc);
            //密送人
            if (!string.IsNullOrWhiteSpace(bcc))
                mm.Bcc.Add(bcc);

            mm.Subject = subject;
            mm.SubjectEncoding = SubjectEncoding;
            mm.Body = body;
            mm.BodyEncoding = BodyEncoding;
            mm.IsBodyHtml = IsBodyHtml;

            //邮件附件 
            if (attachments != null)
            {
                foreach (string fn in attachments)
                {
                    var fi = new FileInfo(fn);
                    if (fi.Exists)
                    {
                        mm.Attachments.Add(new Attachment(fn));
                    }
                }
            }

            //发送邮件
            _smtp.Send(mm);
        }
        #endregion
    }
}
