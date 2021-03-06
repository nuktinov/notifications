﻿using System;
using System.Linq;
using DataAccessLayer.Models;

namespace ServiceLayer.NotificationListService
{
    public static class NotificationPreviewMapper
    {
        public static IQueryable<NotificationPreview> GetNotificationDTO(this IQueryable<Notification> items)
        {
            return items.Select(n => new NotificationPreview { Id = n.Id, Title = n.Title });
        }

        public static IQueryable<Notification> SortingBy(this IQueryable<Notification> notifications, Criterion criterion)
        {

            Sorting? s = criterion.sorting;
            if (s == null)
                s = Sorting.AZ;
            switch (s)
            {
                case Sorting.AZ:
                    notifications = notifications.OrderBy(n => n.Title);
                    break;
                case Sorting.ZA:
                    notifications = notifications.OrderByDescending(n => n.Title);
                    break;
                case Sorting.Newer:
                    {
                        notifications = notifications.SelectMany(n =>  n.NotificationLogs ,
                            (n, l) =>  new { notification = n, Log = l })
                          .Where(p => p.Log.Date == p.notification.NotificationLogs.Max(l => l.Date))  
                          .OrderByDescending(p => p.Log.Date).Select(p => p.notification);
                    }
                    break;
                case Sorting.Older:
                    {
                        notifications = notifications.SelectMany(n => n.NotificationLogs,
                            (n, l) => new { notification = n, Log = l })
                          .Where(p => p.Log.Date == p.notification.NotificationLogs.Max(l => l.Date))  
                          .OrderBy(p => p.Log.Date)
                          .Select(p => p.notification);
                    }
                    break;
                default:
                    throw new Exception("Exception in NotificationServise.SortingBy");
            }
            return notifications;
        }

        public static IQueryable<Notification> Filter(this IQueryable<Notification> notifications, Criterion criterion)
        {
            FilterBy? filterby = criterion.Filterby;
            string searchtext = criterion.SearchText;
            if(filterby != null)
                    switch (filterby)
                    {
                        case FilterBy.Title:
                            if(searchtext!=null)
                                notifications = notifications.Where(n => n.Title.Contains(searchtext));
                            break;
                        case FilterBy.Body:
                            if (searchtext != null)
                                notifications = notifications.Where(n => n.Body.Contains(searchtext));
                            break;
                        case FilterBy.Icon:
                            notifications = notifications.Where(n => n.Icon != null);
                            break;
                        case FilterBy.Image:
                            notifications = notifications.Where(n => n.Image != null);
                            break;
                        default: throw new Exception("Exception in NotificationServise.Filter");
                    }
            return notifications;
        }
        
    }
}
