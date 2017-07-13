﻿using BAPA_LMS.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BAPA_LMS.Models.ActivityViewModels
{
    public class ActivityDetailViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }

        [Display(Name = "Beskrivning")]
        public string Description { get; set; }

        [Display(Name = "Startdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime StartDate { get; set; }

		[Display(Name = "Starttid")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}")]
		public DateTime StartTime { get; set; }

		[Display(Name = "Slutdatum")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime EndDate { get; set; }

		[Display(Name = "Sluttid")]
		[DisplayFormat(DataFormatString = "{0:HH:mm}")]
		public DateTime EndTime { get; set; }

        [Display(Name = "Aktivitet")]
        public ActivityType Type { get; set; }

        public static implicit operator ActivityDetailViewModel(Activity model)
        {
            return new ActivityDetailViewModel
            {
                Name = model.Name,
                Description = model.Description,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Type = model.Type
            };

        }
    }
}