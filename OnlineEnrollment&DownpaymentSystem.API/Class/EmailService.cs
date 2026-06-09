using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace OnlineEnrollment_DownpaymentSystem.API.Class
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendCredentialsEmail(string toEmail, string username, string password, string studentName)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "Your Account Credentials - Enrollment System";

                message.Body = new TextPart("html")
                {
                    Text = $@"
        <html>
        <body style='font-family: Arial, sans-serif;'>
            <div style='max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px;'>
                <h2 style='color: #132d6e;'>WELCOME TO ACLC COLLEGE OF MANDAUE!</h2>
                <p>Dear <strong>{studentName}</strong>,</p>
                <p><strong>YOUR APPLICATION HAS BEEN APPROVED!</strong></p>
                <p>Congratulations! You are now officially ENROLLED at ACLC College of Mandaue.</p>
                <p>Here are your account credentials:</p>
                
                <div style='background-color: #f5f5f5; padding: 15px; border-radius: 8px; margin: 20px 0;'>
                    <p style='margin: 5px 0;'><strong>Username:</strong> {username}</p>
                    <p style='margin: 5px 0;'><strong>Password:</strong> {password}</p>
                </div>
                
                <p>Please keep this information secure. You can change your password after logging in.</p>
                
                <p>Click the link below to login:</p>
                <a href='https://yourdomain.com/login' style='display: inline-block; background-color: #132d6e; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Login Here</a>
                
                <p style='margin-top: 20px;'>Thank you,<br/>ACLC College of Mandaue Enrollment Team</p>
            </div>
        </body>
        </html>
    "
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Student Approval Email (when student info is approved)
        public async Task<bool> SendStudentApprovalEmail(string toEmail, string studentName, string studentNumber)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "🎓 Student Application Approved - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #132d6e; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .status {{ background: #dcfce7; color: #065F46; padding: 10px; border-radius: 8px; text-align: center; }}
        .footer {{ text-align: center; padding: 15px; font-size: 12px; color: #6b7a99; border-top: 1px solid #ddd; }}
        .info {{ background: #f5f5f5; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Student Application Approved</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>We are pleased to inform you that your student application has been <strong style='color:#065F46;'>APPROVED</strong>!</p>
            <div class='info'>
                <p><strong>📌 Student Information:</strong></p>
                <p>Student ID: {studentNumber}</p>
                <p>Name: {studentName}</p>
                <p>Status: <span style='color:#065F46;'>Approved ✅</span></p>
            </div>
            <p>You may now proceed with your enrollment. Please log in to your account to continue.</p>
            <div class='status'>
                <strong>Next Steps:</strong> Complete your enrollment and submit required documents.
            </div>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Student Rejection Email
        public async Task<bool> SendStudentRejectionEmail(string toEmail, string studentName, string studentNumber, string reason)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "📋 Student Application Status Update - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #991b1b; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .status {{ background: #fee2e2; color: #991b1b; padding: 10px; border-radius: 8px; text-align: center; }}
        .footer {{ text-align: center; padding: 15px; font-size: 12px; color: #6b7a99; border-top: 1px solid #ddd; }}
        .reason {{ background: #fff3cd; color: #856404; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Student Application Status Update</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>We regret to inform you that your student application has been <strong style='color:#991b1b;'>REJECTED</strong>.</p>
            <div class='info'>
                <p><strong>📌 Student Information:</strong></p>
                <p>Student ID: {studentNumber}</p>
                <p>Name: {studentName}</p>
                <p>Status: <span style='color:#991b1b;'>Rejected ❌</span></p>
            </div>
            <div class='reason'>
                <p><strong>📝 Reason for Rejection:</strong></p>
                <p>{reason}</p>
            </div>
            <p>Please contact the registrar's office for further assistance.</p>
            <div class='status'>
                <strong>Need Help?</strong> Contact us at +63 (32) 123-4567
            </div>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Document Approval Email
        public async Task<bool> SendDocumentApprovalEmail(string toEmail, string studentName, string documentType)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "✅ Document Approved - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #065F46; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .status {{ background: #dcfce7; color: #065F46; padding: 10px; border-radius: 8px; text-align: center; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Document Approved</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>Your document <strong>{documentType}</strong> has been <strong style='color:#065F46;'>APPROVED</strong>!</p>
            <div class='status'>
                ✅ Document has been verified and accepted.
            </div>
            <p>You may check your enrollment status in your student portal.</p>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Document Rejection Email
        public async Task<bool> SendDocumentRejectionEmail(string toEmail, string studentName, string documentType, string reason)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "📄 Document Update Required - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #991b1b; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .reason {{ background: #fff3cd; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Document Update Required</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>Your document <strong>{documentType}</strong> has been <strong style='color:#991b1b;'>REJECTED</strong>.</p>
            <div class='reason'>
                <p><strong>📝 Reason:</strong></p>
                <p>{reason}</p>
            </div>
            <p>Please upload a corrected version of this document.</p>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Enrollment Approval Email
        public async Task<bool> SendEnrollmentApprovalEmail(string toEmail, string studentName, string course, string schoolYear, string semester)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "🎉 Enrollment Approved - Welcome to ACLC College!";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #132d6e; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .success {{ background: #dcfce7; color: #065F46; padding: 15px; border-radius: 8px; text-align: center; }}
        .details {{ background: #f5f5f5; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Enrollment Approved!</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <div class='success'>
                <h3>🎉 Congratulations!</h3>
                <p>Your enrollment has been <strong>APPROVED</strong>!</p>
            </div>
            <div class='details'>
                <p><strong>📚 Enrollment Details:</strong></p>
                <p>Course: {course}</p>
                <p>School Year: {schoolYear}</p>
                <p>Semester: {semester}</p>
            </div>
            <p>You are now officially enrolled at ACLC College of Mandaue.</p>
            <p>Check your student portal for your subjects and schedule.</p>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }

        // Send Enrollment Rejection Email
        public async Task<bool> SendEnrollmentRejectionEmail(string toEmail, string studentName, string studentNumber, string reason)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "📋 Enrollment Status Update - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #991b1b; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .info {{ background: #f5f5f5; padding: 15px; border-radius: 8px; margin: 15px 0; }}
        .reason {{ background: #fff3cd; color: #856404; padding: 15px; border-radius: 8px; margin: 15px 0; }}
        .footer {{ text-align: center; padding: 15px; font-size: 12px; color: #6b7a99; border-top: 1px solid #ddd; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Enrollment Status Update</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>We regret to inform you that your enrollment application has been <strong style='color:#991b1b;'>REJECTED</strong>.</p>
            <div class='info'>
                <p><strong>📌 Student Information:</strong></p>
                <p>Student ID: {studentNumber}</p>
                <p>Name: {studentName}</p>
                <p>Status: <span style='color:#991b1b;'>Rejected ❌</span></p>
            </div>
            <div class='reason'>
                <p><strong>📝 Reason for Rejection:</strong></p>
                <p>{reason}</p>
            </div>
            <p>Please contact the registrar's office for further assistance or to reapply.</p>
            <div class='footer'>
                <strong>Need Help?</strong> Contact us at +63 (32) 123-4567
            </div>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
            <p>This is an automated message. Please do not reply.</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Enrollment rejection email error: {ex.Message}");
                return false;
            }
        }

        // Send Payment Approval Email
        public async Task<bool> SendPaymentApprovalEmail(string toEmail, string studentName, decimal amount, string referenceNumber)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "💰 Payment Confirmed - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #065F46; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .success {{ background: #dcfce7; color: #065F46; padding: 15px; border-radius: 8px; text-align: center; }}
        .details {{ background: #f5f5f5; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Payment Confirmed!</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <div class='success'>
                <h3>✅ Payment Approved!</h3>
                <p>Your payment has been successfully verified and approved.</p>
            </div>
            <div class='details'>
                <p><strong>💰 Payment Details:</strong></p>
                <p>Amount: ₱{amount:N2}</p>
                <p>Reference Number: {referenceNumber}</p>
                <p>Status: <span style='color:#065F46;'>Approved ✅</span></p>
            </div>
            <p>Your enrollment is now complete. You are officially enrolled at ACLC College of Mandaue.</p>
            <p>You may now access your student portal for your subjects and schedule.</p>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
        </div>
    </div>
</body>
</html>"
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }


        // Send Payment Rejection Email
        public async Task<bool> SendPaymentRejectionEmail(string toEmail, string studentName, decimal amount, string referenceNumber, string reason)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:Host"];
                var port = int.Parse(_configuration["SmtpSettings:Port"]);
                var senderEmail = _configuration["SmtpSettings:Username"];
                var senderPassword = _configuration["SmtpSettings:Password"];
                var senderName = _configuration["SmtpSettings:SenderName"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = "⚠️ Payment Update Required - ACLC College";

                message.Body = new TextPart("html")
                {
                    Text = $@"
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 10px; }}
        .header {{ background: #991b1b; color: white; padding: 20px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ padding: 20px; }}
        .warning {{ background: #fff3cd; color: #856404; padding: 15px; border-radius: 8px; margin: 15px 0; }}
        .details {{ background: #f5f5f5; padding: 15px; border-radius: 8px; margin: 15px 0; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h2>ACLC COLLEGE OF MANDAUE</h2>
            <h3>Payment Update Required</h3>
        </div>
        <div class='content'>
            <p>Dear <strong>{studentName}</strong>,</p>
            <p>Your payment has been <strong style='color:#991b1b;'>REJECTED</strong>.</p>
            <div class='details'>
                <p><strong>💰 Payment Details:</strong></p>
                <p>Amount: ₱{amount:N2}</p>
                <p>Reference Number: {referenceNumber}</p>
                <p>Status: <span style='color:#991b1b;'>Rejected ❌</span></p>
            </div>
            <div class='warning'>
                <p><strong>📝 Reason for Rejection:</strong></p>
                <p>{reason}</p>
            </div>
            <p><strong>To claim your refund, please follow these steps:</strong></p>
            <ol>
                <li>Visit the cashier's office at ACLC College of Mandaue</li>
                <li>Bring this email (printed or digital copy)</li>
                <li>Bring your GCash transaction reference number</li>
                <li>Present a valid ID</li>
            </ol>
            <p><strong>Refund Timeline:</strong> 1-3 business days after verification</p>
            <p>If you have questions, please contact the cashier's office.</p>
        </div>
        <div class='footer'>
            <p>ACLC College of Mandaue | Empowering Minds, Building Futures</p>
        </div>
    </div>
</body>
</html>"


                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(senderEmail, senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email error: {ex.Message}");
                return false;
            }
        }
    }
}
