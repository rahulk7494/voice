using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class SendMail : Form
    {
        public SendMail()
        {
            InitializeComponent();
            try
            {
                String to = textBox1.Text + "@gmail.com";
                String subj = textBox2.Text;
                String body = textBox3.Text;
                //Mail Message
                MailMessage mM = new MailMessage();
                //Mail Address
                mM.From = new MailAddress("rahulk7494@gmail.com");
                //receiver email id
                mM.To.Add("rahulk7494@gmail.com");
                //subject of the email
                mM.Subject = ""+subj;
                //deciding for the attachment
                //mM.Attachments.Add(new Attachment(@"C:\\attachedfile.jpg"));
                //add the body of the email
                mM.Body = ""+body;
                mM.IsBodyHtml = true;
                //SMTP client
                SmtpClient sC = new SmtpClient("smtp.gmail.com");
                //port number for Gmail mail
                sC.Port = 587;
                //credentials to login in to Gmail account
                sC.Credentials = new NetworkCredential("rahulk7494@gmail.com", "687248service");
                //enabled SSL
                sC.EnableSsl = true;
                //Send an email
                sC.Send(mM);
            }//end of try block
            catch (Exception ex)
            {
                Console.WriteLine("" + ex);
            }//end of catch
        }

    }
}
