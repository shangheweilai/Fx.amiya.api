﻿using Fx.Amiya.DbModels;
using Fx.Amiya.DbModels.Model;
using Fx.Amiya.IDal;
using Fx.Infrastructure.DataAccess.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fx.Amiya.Dal
{
    public class DalLiveReplayFlowOptimize : DalEFCore<LiveReplayFlowOptimize>, IDalLiveReplayFlowOptimize
    {
        public DalLiveReplayFlowOptimize(AmiyaDbContext dbContext) : base(dbContext)
        {
        }
    }
}