﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.NotificationListService
{
    public class ObjectNotificationsDTO
    {
        public int PageNumber { get; set; }
        public int AllPages { get; set; }
        public List<NotificationDTO> Notifications{ get; set; }
    }
}