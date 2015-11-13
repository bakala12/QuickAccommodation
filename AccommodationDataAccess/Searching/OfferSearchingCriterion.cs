﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AccommodationDataAccess.Model;

namespace AccommodationDataAccess.Searching
{
    public abstract class OfferSearchingCriterion : ISearchingCriterion<AvailableOffer>
    {
        protected OfferSearchingCriterion(SearchingCriterionType criterionType)
        {
            CriterionType = criterionType;
        }

        public SearchingCriterionType CriterionType { get; }
        public abstract Expression<Func<AvailableOffer, bool>> SelectableExpression { get; }
    }
}