﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartToWork_OneClick.Models
{
    public class SettingsModel
    {
        public int Id { get; set; }
        public string? PathApplication { get; set; }
        public string? Name { get; set; }
        public bool FixStart { get; set; }
    }
}
