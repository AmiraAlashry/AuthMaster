using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthMaster.Application.Helpers
{
    public static class EmailTemplates
    {
        public static string GenerateOtpEmail(string userName, string otpCode)
        {
            return $@"
            <div style='font-family: Arial, sans-serif; background-color: #f4f7f6; padding: 40px 20px; text-align: center; color: #333;'>
                <div style='max-width: 500px; margin: 0 auto; background-color: #ffffff; padding: 30px; border-radius: 10px; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                    <h2 style='color: #2c3e50; margin-bottom: 10px;'>Welcome to AuthMaster!</h2>
                    <p style='font-size: 16px; color: #555;'>Hi <strong>{userName}</strong>,</p>
                    <p style='font-size: 16px; color: #555;'>Thank you for registering. Please use the following verification code to complete your registration:</p>
                    
                    <div style='margin: 30px 0; padding: 15px; background-color: #f8f9fa; border: 2px dashed #007bff; border-radius: 8px; display: inline-block;'>
                        <span style='font-size: 32px; font-weight: bold; color: #007bff; letter-spacing: 5px;'>{otpCode}</span>
                    </div>
                    
                    <p style='font-size: 14px; color: #888; margin-top: 20px;'>If you didn't request this email, you can safely ignore it.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;' />
                    <p style='font-size: 12px; color: #aaa;'>&copy; {DateTime.Now.Year} AuthMaster Team. All rights reserved.</p>
                </div>
            </div>";
        }
    }
}
