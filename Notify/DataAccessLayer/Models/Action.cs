﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace DataAccessLayer.Models
{
    [DataContract]
    public class Action
    {
        public int Id { get; set; }
        [DataMember]
        public string Answer { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Icon { get; set; }

        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
    }
}