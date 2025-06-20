﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.SO.DTOs;

public class CampaignStatusDto
{
    public Guid Id { get; set; }

    public Guid? CampaignId { get; set; }

    public string Name { get; set; }

    public string Color { get; set; }

    public string TextColor { get; set; }

    public string Description { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsClose { get; set; }

    public int? Status { get; set; }

    public int? DisplayOrder { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public string CreatedByName { get; set; }

    public string UpdatedByName { get; set; }

}
