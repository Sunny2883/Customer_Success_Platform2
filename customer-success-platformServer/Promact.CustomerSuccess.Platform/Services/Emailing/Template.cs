
namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public static class Template
    {
        public static string Useremail = "pankajkumarnikk@gmail.com";
        public static string Username = "Pankaj Kumar";

        public static string GetEmailTemplate(string stakeholder)
        {
            return $@"
            <html>
            <head>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        margin: 0;
                        padding: 0;
                        background-color: #f5f5f5;
                    }}
                    .container {{
                        margin: 0 auto;
                        padding: 20px;
                        background-color: #ffffff;
                        border-radius: 5px;
                        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                    }}
                    .header {{
                        background-color: #007bff;
                        color: #ffffff;
                        padding: 10px 20px;
                        border-top-left-radius: 5px;
                        border-top-right-radius: 5px;
                    }}
                    .content {{
                        padding: 20px;
                    }}
                    .footer {{
                        padding: 10px 20px;
                        font-size: 14px;
                        color: #777777;
                    }}
                </style>
            </head>
            <body>
                <div class=""container"">
                    <div class=""content"">
                        <p>Hello {stakeholder},</p>
                        <p>alert!</p>
                        <p>Thanks and Regards,<br/>Promact Infotech</p>
                    </div>
                    <div class=""footer"">
                        <p>This is an automated email. Please do not reply.</p>
                    </div>
                </div>
            </body>
            </html>";
        }
    }
}
