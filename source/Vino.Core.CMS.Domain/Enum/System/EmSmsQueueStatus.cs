﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vino.Core.CMS.Domain.Enum.System
{
    public enum EmSmsQueueStatus : short
    {
        [Display(Name = "待发送")]
        WaitSend = 0,

        [Display(Name = "已发送")]
        Sent = 1,
    }
}