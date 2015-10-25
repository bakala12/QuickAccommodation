﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccommodationDataAccess.Model
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
