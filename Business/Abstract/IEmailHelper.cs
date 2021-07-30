using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IEmailHelper
    {
        IDataResult<string> MailConfirmation(string mail, string link);
        Task AdvertisementMail(string mail, string title);
    }
}
